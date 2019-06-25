Add-Type -AssemblyName System.IO.Compression.FileSystem

$dynamoDbUrl = "https://s3.eu-central-1.amazonaws.com/dynamodb-local-frankfurt/dynamodb_local_latest.zip"
$dynamoDbPackageName = "dynamodb_local.zip"
$dynamoDbName = ".\dynamodb\"

$minioUrl = "https://dl.min.io/server/minio/release/windows-amd64/minio.exe"
$minioName = "minio.exe"

$javaUrl = "https://javadl.oracle.com/webapps/download/AutoDL?BundleId=238698_478a62b7d4e34b78b671c754eaaf38ab"

$accessKey = "AKIAIOSFODNN7EXAMPLE"
$secretKey = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY"

[Net.ServicePointManager]::SecurityProtocol = "tls12, tls11, tls"

Write-Host "Proceeding to set up local environment... This may take several minutes."
Write-Host "Please keep this window open until it closes itself."

#Download Local DynamoDB
if (Test-Path $dynamoDbName)
{
	Write-Host "DynamoDB found, skipping installation"
}
else
{
	Write-Host "Installing local DynamoDB..."
	
	Invoke-WebRequest -Uri $dynamoDbUrl -OutFile $dynamoDbPackageName
	[System.IO.Compression.ZipFile]::ExtractToDirectory($dynamoDbPackageName, $dynamoDbName)
	Remove-Item -Path $dynamoDbPackageName
}

#Install Java
$javaVersion = (Get-Command java | Select-Object -ExpandProperty Version).toString()
if ($javaVersion -eq "8.0.2110.12")
{
	Write-Host "Java found, skipping installation"
}
else
{
	Write-Host "Installing Java..."
	
	Invoke-WebRequest -Uri $javaUrl -OutFile "jre8.exe"
	Start-Process .\jre8.exe '/s REBOOT=0 SPONSORS=0 AUTO_UPDATE=0' -wait
}

#Set environment variable
Write-Host "Setting Java environment variable..."

if (Test-Path "C:\Program Files (x86)\Java")
{
	Start-Process -FilePath 'setx' -ArgumentList 'JAVA_HOME "C:\Program Files (x86)\Java\jre1.8.0_211\bin"'
}
else
{
	Start-Process -FilePath 'setx' -ArgumentList 'JAVA_HOME "C:\Program Files\Java\jre1.8.0_211\bin"'
}

#Install and configure AWS
$myDocumentsPath = [Environment]::GetFolderPath("MyDocuments")
if (Test-Path "$($myDocumentsPath)\WindowsPowerShell\Modules\AWSPowerShell")
{
	Write-Host "AWS found, skipping installation"
}
else
{
	Write-Host "Installing AWS..."

	Install-PackageProvider -Scope CurrentUser -Name NuGet -MinimumVersion 2.8.5.201 -Force
	Install-Module -Scope CurrentUser -Name AWSPowerShell -Force
	Set-AWSCredential -AccessKey $accessKey -SecretKey $secretKey -StoreAs default
}

#Install MinIO
if ([System.IO.File]::Exists($minioName))
{
	Write-Host "MinIO found, skipping installation"
}
else
{
	Write-Host "Installing MinIO..."

	Invoke-WebRequest -Uri $minioUrl -OutFile $minioName
	
	#Set config	
	Write-Host "Setting up MinIO config..."
	
	Start-Process "minio.exe" -ArgumentList "server minio"
	sleep 2
	Stop-Process -Name "minio"
	
	$json = Get-Content .\minio\.minio.sys\config\config.json -raw | ConvertFrom-Json
	$json.credential | % { $_.accessKey = $accessKey }
	$json.credential | % { $_.secretKey = $secretKey }
	$json | ConvertTo-Json -depth 32 | Set-Content .\minio\.minio.sys\config\config.json
}

Write-Host "Setup complete!"
sleep 1
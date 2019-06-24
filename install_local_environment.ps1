Add-Type -AssemblyName System.IO.Compression.FileSystem

$dynamodbUrl = "https://s3.eu-central-1.amazonaws.com/dynamodb-local-frankfurt/dynamodb_local_latest.zip"
$dynamoDbPackageName = "dynamodb_local.zip"

$minioUrl = "https://dl.min.io/server/minio/release/windows-amd64/minio.exe"

[Net.ServicePointManager]::SecurityProtocol = "tls12, tls11, tls"

#Download Local DynamoDB
Invoke-WebRequest -Uri $dynamodbUrl -OutFile $dynamoDbPackageName
[System.IO.Compression.ZipFile]::ExtractToDirectory($dynamoDbPackageName, ".\dynamodb\")

#Install Java
$javaUrl = "https://javadl.oracle.com/webapps/download/AutoDL?BundleId=238698_478a62b7d4e34b78b671c754eaaf38ab"
Invoke-WebRequest -Uri $javaUrl -OutFile "jre8.exe"
Start-Process .\jre8.exe '/s REBOOT=0 SPONSORS=0 AUTO_UPDATE=0' -wait

#Set environment variable
if (Test-Path "C:\Program Files (x86)\Java")
{
	cmd.exe /c 'setx JAVA_HOME "C:\Program Files (x86)\Java\jre1.8.0_211\bin"'
}
else
{
	cmd.exe /c 'setx JAVA_HOME "C:\Program Files\Java\jre1.8.0_211\bin"'
}

#Install and configure AWS
Install-PackageProvider -Scope CurrentUser -Name NuGet -MinimumVersion 2.8.5.201 -Force
Install-Module -Scope CurrentUser -Name AWSPowerShell -Force
Set-AWSCredential -AccessKey AKIAIOSFODNN7EXAMPLE -SecretKey wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY -StoreAs default

#Download MinIO
Invoke-WebRequest -Uri $minioUrl -OutFile "minio.exe"

#Create MinIO file
New-Item -Path . -Name "minio.cmd" -Value "minio.exe server minio"
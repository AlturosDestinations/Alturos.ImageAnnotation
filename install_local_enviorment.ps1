Add-Type -AssemblyName System.IO.Compression.FileSystem

$dynamodbUrl = "https://s3.eu-central-1.amazonaws.com/dynamodb-local-frankfurt/dynamodb_local_latest.zip"
$dynamedbPackageName = "dynamodb_local.zip"

$minioUrl = "https://dl.min.io/server/minio/release/windows-amd64/minio.exe"

#Download Local DynamoDB
#Invoke-WebRequest -Uri $dynamodbUrl -OutFile $dynamedbPackageName
[System.IO.Compression.ZipFile]::ExtractToDirectory($dynamedbPackageName, ".\dynamodb\")

#Download MinIO
Invoke-WebRequest -Uri $minioUrl -OutFile "minio.exe"


#Check Java is available
Get-Command java | Select-Object Version

sleep 5
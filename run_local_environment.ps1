#Run MinIO
Write-Host "Starting MinIO"
Start-Process "minio.exe" -ArgumentList "server minio"

#Run DynamoDB
Write-Host "Starting DynamoDB"
Start-Process "java" -ArgumentList "-Djava.library.path=./dynamodb/DynamoDBLocal_lib/ -jar dynamodb/DynamoDBLocal.jar"

sleep 1
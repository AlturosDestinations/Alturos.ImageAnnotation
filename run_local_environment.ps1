#Run MinIO
Invoke-Item minio.cmd

#Run DynamoDB
cmd.exe /c "cd dynamodb_local"
cmd.exe /c "java -Djava.library.path=./DynamoDBLocal_lib/ -jar DynamoDBLocal.jar"
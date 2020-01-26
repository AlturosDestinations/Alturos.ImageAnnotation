## Docker Installation

You can also use docker images for minio and dynamodb

```sh
#Run MinIO
docker run -e MINIO_ACCESS_KEY=AKIAIOSFODNN7EXAMPLE -e MINIO_SECRET_KEY="wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY" -p 9000:9000 minio/minio server /data
#Run dynamodb
docker run -e AWS_ACCESS_KEY_ID='AKIAIOSFODNN7EXAMPLE' -e AWS_SECRET_ACCESS_KEY="wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY" -p 8000:8000 amazon/dynamodb-local -jar DynamoDBLocal.jar -sharedDb -dbPath ./
```

Alturos.ImageAnnotation.exe.config/App.config
```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <add key="extractionFolder" value="packages" />
    <add key="dbTableName" value="ObjectDetectionImageAnnotation" />
    
    <!-- Set these according to your database -->
    <add key="bucketName" value="MyImageAnnotation" />
    <add key="accessKeyId" value="AKIAIOSFODNN7EXAMPLE" />
    <add key="secretAccessKey" value="wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY" />
    
    <!-- Set these if you want to use a local database -->
    <add key="s3ServiceUrl" value="http://localhost:9000" />
    <add key="dynamoDbServiceUrl" value="http://localhost:8000" />
  </appSettings>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.6.1" />
  </startup>
</configuration>

```

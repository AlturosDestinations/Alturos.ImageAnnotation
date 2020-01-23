## Docker Installation

You can also use docker images for minio and dynamodb

```
docker run -e MINIO_ACCESS_KEY=AKIAIOSFODNN7EXAMPLE -e MINIO_SECRET_KEY="wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY" -p 9000:9000 minio/minio server /data
docker run -e AWS_ACCESS_KEY_ID='AKIAIOSFODNN7EXAMPLE' -e AWS_SECRET_ACCESS_KEY="wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY" -p 8000:8000 amazon/dynamodb-local
```

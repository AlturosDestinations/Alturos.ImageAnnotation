## Local Installation
An alternative solution so you don't need Amazon AWS

You can also use [MinIO](https://github.com/minio/minio) instead of Amazon S3, alongside a [local DynamoDB](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DynamoDBLocal.html)
This way all the files are kept on your PC rather than on a remote server.

### Quick installation

Run `install_local_environment.ps1` using the Windows PowerShell (right-click -> `Run with PowerShell`). This should download and set up anything that's necessary.
In case the script doesn't work, you can try a manual setup, as outlined below.

### Manual Installation

* In order to use a local DynamoDB, download it first [here](http://dynamodb-local.s3-website-us-west-2.amazonaws.com/dynamodb_local_latest.zip).
Extract the downloaded zip file into a folder of your choice.

* Next, download and install the Java Runtime Environment [here](https://java.com/download).

* Once installed, set the JAVA_HOME environment variable by opening the command prompt and writing the following:
```
setx JAVA_HOME java_path /M
```
Replace java_path with the path of the "bin" folder in your Java Runtime Environment installation.

* Download and install the AWS CLI from [here](https://aws.amazon.com/de/cli/).

* Write in the command prompt:
```
aws configure
```
You'll be asked to enter some keys. Since the database is entirely local, you can just use fixed demo keys, for instance `AKIAIOSFODNN7EXAMPLE` for the access key id, and `wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY` for the secret access key.
Remember these keys as they will be used later on again.

* Download [MinIO](https://min.io/download) into a directory of your choice.

* Run the command prompt from the same folder as the downloaded `minio.exe` and write the following:
```
minio.exe server minio
```
Close the command prompt. A folder named `minio` should have been created inside the same directory as your `minio.exe` file.
Navigate to `minio\.minio.sys\config` and open the `config.json` file using a text editor. Change the `accessKey` and `secretKey` on lines 4 and 5 to match the demo keys you chose previously.

The json block should look like this:
```
"credential": {
	"accessKey": "AKIAIOSFODNN7EXAMPLE",
	"secretKey": "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY",
	"expiration": "1970-01-01T00:00:00Z",
	"status": "enabled"
}
```

### App Config Setup

Once you have installed all the necessary components, you still need to adjust the `Alturos.ImageAnnotation.exe.config` file.

* Change `bucketName` to the bucket name you wish to you use for your database. See the [Amazon S3 Bucket Naming Requirements](https://docs.aws.amazon.com/awscloudtrail/latest/userguide/cloudtrail-s3-bucket-naming-requirements.html).

* Change the `accessKeyId` and `secretAccessKey` to the keys AWS is using.
If you did a quick installation, the keys should be `AKIAIOSFODNN7EXAMPLE` and `wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY` respectively.
If you did a manual installation, use the same keys you chose while configuring AWS earlier.

* Change the `s3ServiceUrl` to `http://localhost:9000`

* Change the `dynamoDbServiceUrl` to `http://localhost:8000`

### Launching

The setup is now complete.

Run `run_local_environment.ps1` using the Windows PowerShell in order to start MinIO and the local DynamoDB.
Once you launch the project you should be able to use your local database.

If it doesn't work, try checking `http://localhost:8000/shell/` in your browser to see if the DynamoDB is running.
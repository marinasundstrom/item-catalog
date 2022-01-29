# Getting Started

This guide is intended to be run from top-down.

All the necessary services have been configured in the ```tye.yaml``` file.

## Install the .NET 6 SDK

Download and run the [installer](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

## Install Docker Desktop

Download it from [here](https://www.docker.com/products/docker-desktop).

## Install Tye CLI tools

Check the [instructions](https://github.com/dotnet/tye/blob/main/docs/getting_started.md) to install the tools.

## Create SSL certificates

For MacOS:

Certificates should be placed in a folder called ```certs```, situated in the root folder. They are used by Nginx.

Requested file names:

```
localhost.crt
localhost.key
```

This is how you generate self-signed certificates (also used by ASP.NET Core) on macOS:

```
dotnet dev-certs https -ep aspnetapp.pfx -p crypticpassword
dotnet dev-certs https --trust
```

Extract private key

```
openssl pkcs12 -in aspnetapp.pfx -nocerts -out localhost.key
```


Extract certificate

```
openssl pkcs12 -in aspnetapp.pfx -clcerts -nokeys -out localhost.crt
```

Remove passphrase from key

```
cp localhost.key localhost.key.bak
openssl rsa -in localhost.key.bak -out localhost.key
```

## Run the projects

Open a terminal and navigate to the root/solution folder. 

Run the following command:

```sh
tye run
```

### Other useful commands:

Runs the projects, **watch** for changes, and automatically recompile:

```sh
tye run --watch
```

Watch and Debug messages:

```sh
tye run --watch -v Debug
```

## Create the app databases

This will create and seed the main app database.

In the source file ```Server/AppService/WebApi/Program.cs```, scroll down to the following line:

```C#
//await app.Services.SeedAsync();
```

Toggle the comment off.

```C#
await app.Services.SeedAsync();
```

If you are in Watch mode, then the service will restart, and the uncommented code will execute.

Make sure to toggle the comment on again, or else the database will be recreated everytime you make a change to AppService.

*This line of code has to be toggled off everytime the Domain model change in order for the database to be recreated.*

## Seed IdentityService database

The following steps will seed the database.

In a terminal, go to the ```Server/IdentityService``` project directory.

Make sure that the database server is running. *(See previous steps)*

Run this command:

```sh
dotnet run -- /seed
```

## Set up Azurite Storage Emulator

To publicly expose Blobs via their URLs you have to change Azurite's configuration.

*(This requires Azurite to have been run once for the files to be created)*

Open the file ```WebApi/.data/azurite/__azurite_db_blob__.json```:

Add the ```"publicAccess": "blob"``` key-value in the section shown below:

```json
        {
            "name": "$CONTAINERS_COLLECTION$",
            "data": [
                {
                    "accountName": "devstoreaccount1",
                    "name": "images",
                    "properties": {
                        "etag": "\"0x1C839AE6CDF11F0\"",
                        "lastModified": "2021-05-14T15:08:51.726Z",
                        "leaseStatus": "unlocked",
                        "leaseState": "available",
                        "hasImmutabilityPolicy": false,
                        "hasLegalHold": false,
              --- >  "publicAccess": "blob" <---- 
                    },
                   // Omitted
        },
```

Then, restart Azurite. 

Just restart the whole system. Exit Tye, and restart it.

## Launch the web app

In your browser, navigate to: ```https://localhost:8080/```

You are now ready.
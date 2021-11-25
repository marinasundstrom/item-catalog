# Item Catalog

App for cataloging items with pictures.

Built with .NET MAUI using Blazor. Components are from MudBlazor. It is multi-platform.

Watch video [here](https://youtu.be/wXaQB18FvRk).

## Introduction

Multi-platform app, with Web backend and API.

App can be compiled for iOS, macOS, Android, Windows.

Contains a WebApi. Uses SQL Server and emulated Azure Blob Storage out of box. Also Nginx.

Dev environment is based around projects and containers orchestrated by .NET Tye. Enabling you to launch all backend services with one simple command.

## Screenshots

<a href="/Screenshots/macOS.png">
<img src="/Screenshots/macOS.png" height="250"  alt="macOS" /></a>

<a href="/Screenshots/iPhone.png">
<img src="/Screenshots/iPhone.png" height="250"  alt="iPhone"  /></a>

## MAUI, what?

.NET Multi-platform App UI (MAUI) is an modern open-source app framework that is the evolution of Xamarin.Forms. Enabling you to build native apps with .NET - not just mobile but also desktop. It still has the XAML model for declaring UI.

But in this app, it is combined with Blazor.

MAUI contains abstractions that make it easier to leverage common functionality across the platforms. Camera, File Pickers, Compass etc.

## Blazor

Blazor is an open-source framework for building apps with components, based on the familiar Razor syntax.

It lets you combine HTML, CSS and C# to build interactive experiences for the Web, like Single-Page Applications.

There are two kinds of Blazor Web apps: Client-side (WebAssembly) and Server-side rendered.

But, in this MAUI app its works a bit different.

Despite the UI being built with HTML and CSS, there is no Web Server or Web Assembly in this app. The UI is rendered in the same process as the rest of the app. :) 

With some work, you can share components with your standard Blazor Web App.

## How to run

### Build Requirements

* .NET 6 SDK
* Tye CLI tools
* Docker Desktop

### Start app

Just start the app via Visual Studio or CLI.

### Start services

Given that you have the Tye CLI tools installed:

To start all the services, you run this command in he ```WebApi``` project folder:

```sh
tye run
```

This starts all backend dependencies (SQL Server, Azurite, Nginx etc.)

#### Watch

To start with file watch:

```sh
tye run --watch
```

### Important 1 - Startup Issue

The WebApi might not work properly when cold-started. This is because it fails to connect to SQL server due to it not having fully started yet.

If you are in *watch mode*, make change to a random file, like ```Program.cs```. Reverse it before it has been applied.

Then the WebApi will restart, and everything will work.

### Important 2 - Expose Blobs via public URL 

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

### Important 3 - Certificates
Certificates should be placed in ```WebApi/certs```. They are used by Nginx.

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

```
sudo security add-trusted-cert -d -r trustRoot -k /Library/Keychains/System.keychain <<certificate>>'
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
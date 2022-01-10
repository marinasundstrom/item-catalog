# Item Catalog

App for cataloging items with pictures.

Built with .NET MAUI using Blazor. Components are from MudBlazor. It is multi-platform.

There is also a Web version of the app.

Watch video [here](https://youtu.be/wXaQB18FvRk).

## Introduction

Multi-platform app, with Web backend and API. Written entirely in .NET.

App can be compiled to run on iOS, macOS, Android, or Windows.

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

Blazor is an open-source framework for building apps for the Web using components, based on the familiar Razor syntax.

It lets you combine HTML, CSS and C# to build interactive experiences for the Web.

There are two kinds of Blazor Web apps: Client-side (WebAssembly) and Server-side rendered.

But, in this MAUI app it works a bit different.
### Hybrid Native app

Despite the UI being built with HTML and CSS, there is no Web Server or Web Assembly in this app. The UI is rendered in the same process as the rest of the app. :) 

With some work, you can share components with your standard Blazor Web App.

This approach is similar to *React Native*, which lets you use React components that have been written for the web with mobile apps. The obvious difference is that, instead of React components and JavaScript,  Blazor lets your use Razor components and C# across the stack.

## ASP.NET Core

ASP.NET Core is an open-source framework for building Web Apps. 

You can build apps with MVC or Razor pages. You can also build Web APIs. Now we have true "Minimal" APIs which are hooking directly into the core

You can mix and match the technologies since they are based on the same core. Essentially, a pipeline of middlewares that handle HTTP requests.

Blazor (Server-side) is actually a part of ASP.NET Core too - so everything works great together. 

There are many libraries and third-party frameworks that can enhance your ASP.NET Core apps. Open API and Swagger UI is supported out of box via third-party library.

In this app, the server-side logic is hosted in an ASP.NET Core app that exposes a Web API. 

Using Open API you can generate client classes that are used by the MAUI app. Essentially, it hides away the details of making requests to the server via HTTP.

## Tye

A challenge when building distributed applications is to orchestrate services during development time. It might be your own projects, database server, Nginx. How to configure them, and make them communicate.

You can always run stuff locally or in containers, but that implies manual configuration. Setting up a virtual network. Tye handles all this for you. 

Much like full orchestrators, such as *Docker Compose*, it gives you a way to declare your services and the desired configuration in a file (```tye.yaml```). It then runs your projects and containers (in Docker), setting up networks and such for you. Even enabling service discovery.

You can check in this ```tye.yaml``` file with your source code to let other developers get going in no time.

When you are ready to release your application, Tye helps you publish it to Kubernetes.

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

#### Web app

The Web app is hosted at: ```https://localhost:8080/```

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
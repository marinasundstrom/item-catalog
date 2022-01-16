# Solution structure

## Projects
* Client - Blazor app
    * Mobile (iOS, Android, Windows, macOS)
    * Web

* Server
    * AppService 
      * Domain - Entities and rules pertaining to them
      * Application - The application
        * Dtos
      * Infrastructure - Implements basic service and persistence
      * Web API - Provides an API for app

    * Worker service - Performs work
    * Contracts - Message types used for inter-service communication

# Solution structure

## Projects
* App - Blazor app
    * Mobile
    * Web

* Service
    * Domain - Entities and rules pertaining to them
    * Application - The application
        * Dtos
    * Infrastructure - Implements basic service and persistence
    * Web API - Provides an API for app

* Worker service - Performs work
* Contracts - Message types used for inter-service communication
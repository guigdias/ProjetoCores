# ProjetoCores

Distributed REST API built with:

- .NET 8
- MongoDB
- .NET Aspire
- Docker
- DDD Architecture

## Architecture

AppHost (Aspire)
|

Domain
|-Entities
|     |- Color.cs -> Entidade rica, com comportamentos 
|
|- Interfaces
|     | - IColorRepository.cs
|
| - Services
|     | - ColorService.cs

Infrastructure
| - Repositories
|	|-MongoRepository
|
| - Configurations
|       |- MongoConfig

Api
|- Controllers
|       |-ColorController
|
|- DTOs
|   |- UpdateColorDto
|   |- CreateColorDto
|

## Features

- Create Color
- Update Color
- Delete Color
- List Colors
- Get by Id

## How to Run

1. Install .NET 8 SDK
2. Install Docker Desktop
3. Run:

```bash
dotnet run --project ProjetoCores.AppHost

4- Get the localhost URL via PowerShellDeveloper
5- Access the URL, run API, use click the URL to open in a new Tab
6- At the new Tab, add to the URL: /swagger for Interface


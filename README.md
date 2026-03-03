# ProjetoCores

Distributed REST API built with modern .NET technologies, following Domain-Driven Design (DDD) principles and orchestrated using .NET Aspire.

---

## Technologies

- .NET 8
- ASP.NET Core Web API
- MongoDB
- .NET Aspire
- Docker
- Domain-Driven Design (DDD)
- Dependency Injection
- Clean Architecture Principles
- Fluent Validation
---

## Architecture Overview

This project follows a layered architecture based on DDD and clean separation of concerns.

```
AppHost (Aspire)
│
├── Domain
│   ├── Entities
│   │   └── Color.cs
│   │
│   ├── Interfaces
│   │   └── IColorRepository.cs
│   │
│   └── Services
│       └── ColorService.cs
│
├── Infrastructure
│   ├── Repositories
│   │   └── MongoRepository.cs
│   │
│   └── Configurations
│       └── MongoConfig.cs
│
└── Api
    ├── Controllers
    │   └── ColorController.cs
    │
    └── DTOs
    │    ├── CreateColorDto.cs
    │    └── UpdateColorDto.cs
    │    └── MergeColorsDto.cs
    └── Validators
         │
         └──Messages
         │     ├── ColorErrorMessages.cs
         │
         ├── ColorValidator.Cs
         ├── CreateColorDtoValidator.cs
         ├── UpdateColorDtoValidator.cs


```      

---

## 🧠 Layer Responsibilities

### AppHost (Aspire)
- Orchestrates the distributed application
- Manages MongoDB container lifecycle
- Injects infrastructure dependencies
- Centralizes application startup

### Domain
- Contains core business logic
- Defines rich domain entities (`Color`)
- Encapsulates business rules
- Defines repository abstractions

### Infrastructure
- Implements repository interfaces
- Handles MongoDB persistence
- Contains external configurations
- No business logic

### API
- Exposes HTTP endpoints
- Handles request/response mapping via DTOs
- Depends on Domain services
- Does not contain business logic
- Fluent Validation messages and rule validations

---

## Features

- Create a color
- Update a color
- Delete a color
- List all colors
- Get color by ID
- RGB validation
- Automatic RGB → CMYK conversion
- MongoDB container managed by Aspire

---

## How to Run

### Prerequisites

- .NET 8 SDK installed
- Docker Desktop running

### Running the Application

```bash
dotnet run --project ProjetoCores.AppHost
```

Steps:

1. Start the AppHost project
2. Copy one of the generated localhost URLs
3. Open the URL in your browser
4. Append `/swagger` to access the API documentation

Example:

```
https://localhost:7294/swagger
```

---

## Infrastructure

- MongoDB runs as a container
- Managed automatically by .NET Aspire
- Connection string injected via configuration
- Health checks enabled

To verify MongoDB container:

```bash
docker ps
```

---

## Design Principles

- Clean Architecture
- Domain-Driven Design (DDD)
- Dependency Inversion
- Separation of Concerns
- Infrastructure isolated from Domain
- DTOs prevent direct exposure of domain entities
- Unit tests to avoid invalid inputs

---

## Future Improvements

- Unit tests (xUnit / FluentAssertions)
- Integration tests
- CI/CD with GitHub Actions
- Docker Compose production profile
- Kubernetes deployment
- Authentication & Authorization
- Logging and Observability improvements

---

## Author

Guilherme Coelho

Developed as part of a professional backend architecture study and international-level engineering practice.

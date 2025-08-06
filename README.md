
# Complytek Test API – Employee, Department & Project Management

A RESTful API built with **.NET 9** for managing employees, departments, and projects in a company.

---

## Features

- CRUD operations for Employees, Departments, and Projects  
- Assign/Remove Employees to Projects with specific Roles  
- Calculate Total Budget per Department  
- Generate Unique Project Codes via External API  
- Automatic EF Core Migrations & Database Seeding  
- Fully Dockerized Setup (API + SQL Server)

---

##  Tech Stack

- ASP.NET Core Web API (.NET 9)  
- Entity Framework Core (EF Core)  
- SQL Server  
- Docker & Docker Compose  
- Serilog for Structured Logging  
- Clean Architecture Pattern

---

##  Getting Started with Docker

###  Step 1: Clone the Repository

```bash
git clone https://github.com/realkibetgilbert/ComplytekTest.git
cd ComplytekTest.API
```

###  Step 2: Run with Docker Compose

```bash
docker compose up --build
```

This will:

- Build the API image  
- Start API and SQL Server containers  
- Wait for SQL Server to be ready  
- Apply EF Core migrations  
- Seed initial data  

###  Step 3: Access the API

- Swagger UI: [http://localhost:5192](http://localhost:5192)

---

##  API Versioning

This API uses **URL segment-based versioning**.

- All endpoints are prefixed with the version number.  
- Current version: `v1`

###  Example

To access employee endpoints:

```
GET http://localhost:5192/api/v1/employee
```

Ensure all requests are made using the `/api/v1/` prefix.

---

##  Local Development

###  Step 1: Update Connection String

Update the connection string in:

```bash
ComplytekTest.API/appsettings.json
```

###  Step 2: Apply EF Core Migrations

```bash
dotnet ef database update
```

###  Step 3: Run the API Locally

```bash
dotnet run --project ComplytekTest.API
```

---

##  Project Structure

```bash
ComplytekTest.API/               # Presentation Layer
ComplytekTest.API.Application/   # Business Logic
ComplytekTest.API.Infrastructure/ # EF Core, Services, Repositories
ComplytekTest.API.Core/          # Domain
ComplytekTest.API.Test/          # Tests
```

---

##  Retry Logic

- Built-in retry logic ensures DB is ready before applying migrations and seeding during Docker startup.

---

##  Docker Tips

###   Rebuild Containers

```bash
docker compose down
docker compose up --build
```

### Reset Database (Remove Volumes)

```bash
docker compose down -v
docker compose up --build
```

---

##  Questions?

Reach out via email: **kibetgilly354@gmail.com**

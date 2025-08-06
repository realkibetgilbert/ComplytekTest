
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
##  Postman Collection

I Have included a complete Postman collection for testing all the available API endpoints (Employee, Department, and Project).

### 🔗 Download Collection

[ Download ComplytekTestApiCollection.postman_collection.json](./ComplytekTest.API/PostmanCollection/ComplytekTestApiCollection.postman_collection.json)



---

###  Step-by-Step: Testing with Postman

####  Step 1: Import the Collection

1. Open **Postman**.
2. Click on `Import` (top-left corner).
3. Select the file `ComplytekTestApiCollection.postman_collection.json`.
4. The collection will now appear in your Postman sidebar.

####  Step 2: Set Up Environment Variables

Before testing, define environment variables for smooth execution:

1. Go to the **Environments** section in Postman.
2. Create a new environment named `ComplytekTest`.
3. Add these key-value pairs:
    - `BASE_URL`: `http://localhost:5192`
    - `API_VERSION`: `1`
4. Save the environment and **select it from the top-right dropdown** in Postman.

>  You can adjust `BASE_URL` if you're running the API under a different port.

####  Step 3: Use the Endpoints

Now you can test the following endpoints grouped by category:

- **Department**
  - `POST /v1/Department` – Create a department
  - `GET /v1/Department` – Get all departments
  - `GET /v1/Department/{id}` – Get department by ID
  - `PUT /v1/Department/{id}` – Update department
  - `DELETE /v1/Department/{id}` – Delete department
  - `GET /v1/Department/{id}/total-project-budget` – Get department’s total project budget

- **Employee**
  - `POST /v1/Employee` – Create employee
  - `GET /v1/Employee` – List employees
  - `GET /v1/Employee/{id}` – Get employee by ID
  - `PUT /v1/Employee/{id}` – Update employee
  - `DELETE /v1/Employee/{id}` – Delete employee

- **Project**
  - `POST /v1/Project` – Create project
  - `GET /v1/Project` – List projects
  - `GET /v1/Project/{id}` – Get project by ID
  - `PUT /v1/Project/{id}` – Update project
  - `DELETE /v1/Project/{id}` – Delete project
  - `POST /v1/Project/{id}/assign-employee` – Assign employee to project
  - `POST /v1/Project/{id}/remove-employee` – Remove employee from project
  - `GET /v1/Project/by-employee/{employeeId}` – Get projects by employee

---

###  Common Tips

- Always make sure the API and database containers are running (`docker compose up`).
- If the API isn't responding, check if the correct port is open (default is `5192`).
- Some endpoints require valid existing `DepartmentId` or `ProjectId` to test properly.

---

##  Done

You're now ready to test and interact with your API endpoints through Postman.

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

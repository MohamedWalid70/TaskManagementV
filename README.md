# Task Management API

A .NET Web API built with Clean Architecture, featuring user management, task tracking, background processing, and Redis caching.

---

## Tech Stack

- .NET 10
- Entity Framework Core (SQL Server)
- Redis (via Docker)
- JWT Authentication
- Swagger / OpenAPI
- `BackgroundService` (task processor — in-memory Channel queue)
- `IHostedService` (admin seeder — runs once on startup)

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (local or Docker)
- [Docker](https://www.docker.com/products/docker-desktop/) (for Redis)

---

## Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/MohamedWalid70/TaskManagementV.git
cd TaskManagement.Api
```

### 2. Start Redis via Docker

```bash
docker run -d --name redis -p 6379:6379 redis
```

To verify Redis is running:

```bash
docker ps
```

### 3. Apply Database Migrations

```bash
dotnet ef database update --project src/Infrastructure --startup-project src/Api
```

### 4. Run the Project

```bash
dotnet run --project src/Api
```

---

## How to Run

Once running, the API and Swagger UI are available at:

| Profile | API Base URL | Swagger URL |
|---------|-------------|-------------|
| http    | http://localhost:5229 | http://localhost:5229/swagger |
| https   | https://localhost:7253 | https://localhost:7253/swagger |

> The app launches directly to Swagger on startup.

---

## Seeded Admin Credentials

On first startup, an `IHostedService` automatically seeds a default admin user into the database with the following hardcoded credentials:

| Field    | Value             |
|----------|-------------------|
| Email    | admin@example.com |
| Password | Admin@123         |
| Role     | Admin             |

> The admin is seeded only if no admin user exists in the database, making it safe on subsequent startups.

To authenticate as admin in Swagger:

1. Open Swagger at `http://localhost:5229/swagger`
2. Call `POST /api/auth/login` with the credentials above
3. Copy the returned JWT token
4. Click **Authorize** at the top of Swagger and enter: `Bearer <your-token>`

---

## Features

### Authentication
- JWT-based authentication with role claims
- Hashed Passwords
- Role-based authorization (`Admin` / `User`)
- JWT support enabled inside Swagger for easy testing

### Admin Seeding
- Implemented via `IHostedService` — runs once at application startup
- Admin credentials are hardcoded in the seeder (not loaded from configuration)
- Seeding is idempotent — skipped if admin already exists

### Task Management
- Tasks are scoped to the authenticated user
- Users can only view and update their own tasks
- Tasks are sorted by priority first, then by creation date
- Duplicate task titles for the same user on the same day are rejected

### Background Processing
- On task creation, the task ID is enqueued into an in-memory `Channel`
- A `BackgroundService` runs for the entire app lifetime, dequeuing and processing tasks
- Task status transitions: `Pending` → `InProgress` → `Done`
- Processing is simulated via `Task.Delay`

### Redis Caching
- `GET /api/v1/tasks/{id}` loads from the database on first request and caches in Redis
- Subsequent requests are served directly from Redis
- Cache is invalidated when the task is updated
- Cache key pattern: `task:{id}`

---

## Project Structure

```
src/
├── Api/            # Controllers, Program.cs, DTOs(Models), Mapper profiles
├── Application/    # Features, Commands, Queries
├── Domain/         # Entities, Interfaces
└── Infrastructure/ # EF Core, SQL Server, Redis, Background Jobs, Seeder
```

---

## Assumptions

- A user can only access their own tasks; the admin manages users and can view or delete any task
- The admin account cannot be deleted
- Background processing is simulated with `Task.Delay`
- Redis must be running before starting the app; the app will fail to start if Redis is unreachable

# OrderFlow

**Enterprise Order & Fulfillment Management Platform** - a simplified combination of Amazon + Shopify + warehouse/logistics systems, built as a serious enterprise system.

This is not just another CRUD application. It is a **permanent engineering laboratory** - one long-term project that evolves over years from a modular monolith to a production-grade distributed system.

## The Philosophy

❌ **Don't build 20 small tutorial projects**
✅ **Build ONE system that demonstrates evolution: .NET Developer → Senior Engineer → Architect**

Every concept learned—DDD, CQRS, Clean Architecture, SQL optimization, SignalR, Service Bus, Event Grid, Docker, CI/CD, microservices, Kubernetes—gets added to or improves this same system.

Guiding principles:

- Understand "why" before "how" - solve real problems, not hypothetical ones
- Deliberately introduce problems at scale, then fix them (e.g. SQL optimization against 100,000 rows)
- Learn universal patterns first (architecture, DDD, messaging, caching), then cloud specifics
- Build locally first ($0), deploy to Azure only when the phase requires it

**Key Principle:** Don't build "an Azure project" - build production software, then learn how to run it on Azure.

## Core Business Flow

```
Customer → Product → Cart → Order → Payment → Inventory → Warehouse → Shipment → Delivery → Notification
```

## What Makes This Different

The target is "an enterprise distributed order-management and fulfillment platform designed to process millions of orders" - not an ecommerce website. That distinction drives every architectural decision.

## Getting Started

### Prerequisites

- **.NET 10 SDK**
- **SQL Server LocalDB** (`(localdb)\MSSQLLocalDB`) - ships with Visual Studio, or install SQL Server Express LocalDB standalone
- **EF Core tools**: `dotnet tool install --global dotnet-ef`

### Build and run

```bash
# Build
dotnet build OrderFlow.slnx

# Create/update the database
dotnet ef database update --project src/OrderFlow.Infrastructure --startup-project src/OrderFlow.Api

# Run the API
dotnet run --project src/OrderFlow.Api

# Run the tests (14 tests: 12 domain, 2 application)
dotnet test
```

The API listens on `http://localhost:5264` and `https://localhost:7123`.

### Configuration

The connection string lives in [src/OrderFlow.Api/appsettings.Development.json](src/OrderFlow.Api/appsettings.Development.json) under the name `OrderFlow`:

```
Server=(localdb)\MSSQLLocalDB;Database=OrderFlowDb;Trusted_Connection=True;TrustServerCertificate=True
```

### Endpoints

No Swagger/OpenAPI is registered yet - call the endpoints directly.

| Method | Route | Behaviour |
|--------|-------|-----------|
| `POST` | `/api/orders` | Creates an order. Returns `201 Created` with `{ orderId }`. Validated by `CreateOrderValidator` before the handler runs. |
| `GET` | `/api/orders/{id:guid}` | Returns the order, or `404 Not Found`. |

`ConfirmOrder` has a handler but no endpoint yet.

### Performance dataset (Phase 4)

The execution-plan experiments run against 100,000 Orders and five hand-created nonclustered indexes. Neither is in a migration, so `dotnet ef database update` alone produces an empty database that will not reproduce any of the measured numbers. The generation scripts are kept outside this repository.

## Project Structure

```
OrderFlow/
├── src/
│   ├── OrderFlow.Domain/           # Entities, value objects, domain events
│   ├── OrderFlow.Application/      # Commands, queries, handlers, pipeline behaviors, abstractions
│   ├── OrderFlow.Infrastructure/   # EF Core, migrations, repositories, outbox
│   └── OrderFlow.Api/              # Controllers, composition root
└── tests/
    ├── OrderFlow.Domain.Tests/
    └── OrderFlow.Application.Tests/
```

Dependency direction: Domain ← Application ← Infrastructure ← Api. Domain has no project references.

## Technology Stack (current)

| Technology | Version | Role |
|------------|---------|------|
| .NET / ASP.NET Core | 10.0 | Core framework |
| EF Core (SQL Server) | 10.0.11 | ORM, migrations |
| SQL Server LocalDB | 2025 (17.0.4025.3) | Database - `(localdb)\MSSQLLocalDB`, `OrderFlowDb` |
| MediatR | 14.2.0 | Command/query dispatch, pipeline behaviors |
| FluentValidation | 12.1.1 | Request validation pipeline |
| xUnit | 2.9.3 | Unit tests |

Architecture patterns in use: Clean Architecture, DDD tactical patterns (entities, value objects, aggregate roots, domain events), CQRS, Repository + Unit of Work, Outbox (write side).

## Phase Roadmap

| Phase | Focus | What it covers | Status |
|-------|-------|----------------|--------|
| 1 | Modular Monolith | Project layout, layering, foundation | ✅ Complete |
| 2 | Clean Architecture + DDD | Aggregates, entities, value objects, domain events, dependency inversion | ✅ Complete |
| 3 | CQRS + Mediator | Command/query separation, MediatR pipeline behaviors | ✅ Complete |
| 4 | Advanced SQL Server | Indexing, execution plans, SARGability, statistics, locking, Query Store, EF Core query optimization | 🔄 In progress |
| 5 | Authentication / Authorization | OAuth2/OIDC, JWT, RBAC, MFA | Planned |
| 6 | Angular Enterprise UI | Enterprise frontend, state management, RxJS, Signals | Planned |
| 7 | SignalR | Real-time updates, WebSockets, scale-out | Planned |
| 8 | Azure Service Bus | Reliable async messaging, competing consumers, retries | Planned |
| 9 | Event Grid + Functions | Event-driven architecture, pub/sub, serverless handlers | Planned |
| 10 | Redis + Caching | Distributed caching, session management | Planned |
| 11 | Docker | Containerization, multi-stage builds | Planned |
| 12 | CI/CD | Automated deployments, test pipelines, GitOps | Planned |
| 13 | Observability | Logging, metrics, traces, Application Insights | Planned |
| 14 | Microservices | Service boundaries, distributed architecture | Planned |
| 15 | Kubernetes / AKS | Container orchestration, scaling, resilience | Planned |

C# and .NET fundamentals - advanced APIs, async/await, DI, middleware, performance - run through every phase rather than belonging to one.

**Phase 4 progress:** 11 topics complete.

Commit history is the record of what changed and when - use `git log` rather than duplicating it here.

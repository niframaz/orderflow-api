# OrderFlow - Project Context

## Vision

**Enterprise Order & Fulfillment Management Platform** - A simplified combination of Amazon + Shopify + warehouse/logistics systems, designed as a serious enterprise system.

**This is not just another CRUD application.** This is a **permanent engineering laboratory** - one long-term project that evolves over years from a modular monolith to a production-grade distributed system.

### The Philosophy

❌ **Don't build 20 small tutorial projects**
✅ **Build ONE system that demonstrates your evolution: .NET Developer → Senior Engineer → Architect**

Every concept learned—DDD, CQRS, Clean Architecture, SQL optimization, SignalR, Service Bus, Event Grid, Docker, CI/CD, microservices, Kubernetes—gets added to or improves this same system.

### Core Business Flow
```
Customer → Product → Cart → Order → Payment → Inventory → Warehouse → Shipment → Delivery → Notification
```

### What Makes This Different
This is **"An enterprise distributed order-management and fulfillment platform designed to process millions of orders"** - not just an ecommerce website. That distinction drives every architectural decision.

## Phase Roadmap

```
Phase 1:  Modular Monolith (Foundation)
Phase 2:  Clean Architecture + DDD ← CURRENT
Phase 3:  CQRS + Mediator
Phase 4:  Advanced SQL Server
Phase 5:  Authentication / Authorization
Phase 6:  Angular Enterprise UI
Phase 7:  SignalR (Real-time)
Phase 8:  Azure Service Bus
Phase 9:  Event Grid + Functions
Phase 10: Redis + Caching
Phase 11: Docker
Phase 12: CI/CD
Phase 13: Observability
Phase 14: Microservices
Phase 15: AKS / Advanced Azure
```

## Current Status

**Phase:** 2 - Clean Architecture + DDD (In Progress)

**Completed:**
- ✅ **Committed:** Phase 2 foundation (d30dc9c)
  - Clean Architecture layers (Domain, Application, Infrastructure, API)
  - Entity base class with audit fields and domain events
  - Domain entities: Order, OrderItem, Product, Customer
  - ShippingAddress value object (immutable record)
  - Repository pattern (IOrderRepository, IProductRepository)
  - Unit of Work pattern (IUnitOfWork implemented by DbContext)
  - Removed SaveAsync from repositories (UnitOfWork responsibility)
  - Outbox pattern for reliable event publishing
  - EF Core entity configurations
  - MediatR handlers (CreateOrder, ConfirmOrder, GetOrder)

**In Progress:**
- ⏳ Money value object implementation
  - Create Money.cs (Amount + Currency)
  - Update Product.Price: decimal → Money
  - Update OrderItem.UnitPrice: decimal → Money
  - Update EF Core configurations (OwnsOne)
  - Create and apply migration

**Next (After Money):**
- Aggregate Root design validation
- Domain Services
- Domain unit tests
- Complete Phase 2

**Next Phase (Phase 3):**
- Already partially implemented (MediatR handlers exist)
- Add pipeline behaviors (validation, logging, transactions)
- Separate read models from write models
- Optimize queries

## Technology Stack & Learning Path

### Current (Phase 2)
- **.NET 8** - Core framework
- **EF Core** - ORM and data access
- **SQL Server (Local)** - Database
- **Clean Architecture** - Layered architecture
- **DDD** - Domain modeling approach (Entities, Value Objects, Aggregates, Domain Events)
- **MediatR** - CQRS pattern (Commands, Queries, Handlers)

### Complete Learning Roadmap

| Area | What You'll Master | Phase |
|------|-------------------|-------|
| **C# / .NET** | Advanced APIs, async/await, DI, middleware, performance | 1-15 |
| **Clean Architecture** | Separation of concerns, dependency inversion | 2 ← |
| **DDD** | Aggregates, entities, value objects, domain events | 2 ← |
| **CQRS** | Separate read/write models, command/query handlers | 3 |
| **MediatR** | Request/response patterns, pipeline behaviors | 3 |
| **SQL Server** | Indexing, transactions, locking, execution plans, optimization | 4 |
| **EF Core** | Advanced ORM usage, query optimization, change tracking | 4 |
| **Authentication** | OAuth2/OIDC, JWT, RBAC, MFA | 5 |
| **Angular** | Enterprise frontend, state management, RxJS, Signals | 6 |
| **SignalR** | Real-time updates, WebSockets, scale-out | 7 |
| **Azure Service Bus** | Reliable async messaging, competing consumers, retries | 8 |
| **Event Grid** | Event-driven architecture, pub/sub patterns | 9 |
| **Azure Functions** | Serverless processing, event handlers | 9 |
| **Redis** | Distributed caching, session management | 10 |
| **Docker** | Containerization, multi-stage builds | 11 |
| **CI/CD** | Automated deployments, testing pipelines, GitOps | 12 |
| **Observability** | Logging, metrics, traces, Application Insights | 13 |
| **Microservices** | Distributed architecture, service boundaries | 14 |
| **Kubernetes/AKS** | Container orchestration, scaling, resilience | 15 |

## Azure Cost Strategy

### Philosophy: Learn Locally First, Then Azure

**Key Principle:** Build production-grade software first, then learn how to run it on Azure. This means learning universal patterns (architecture, DDD, messaging, caching) that work anywhere, then applying them to Azure.

### Cost Breakdown by Phase

| Phase | Services | Environment | Monthly Cost |
|-------|----------|-------------|--------------|
| **1-2** | .NET + Angular + SQL + DDD | Local | **$0** |
| **3-4** | CQRS + Advanced SQL | Local | **$0** |
| **5** | Authentication | Local / Azure AD B2C free tier | **$0** |
| **6** | Angular UI | Local + optional Static Web Apps | **$0-$5** |
| **7** | SignalR | Local / Azure SignalR free tier | **$0-$5** |
| **8** | Service Bus | RabbitMQ (local) → Azure Service Bus | **$0-$10** |
| **9** | Event Grid + Functions | Local → Azure (free tier) | **$0-$5** |
| **10** | Redis | Local Redis → Azure Cache | **$0-$15** |
| **11** | Docker | Local Docker Desktop | **$0** |
| **12** | CI/CD | GitHub Actions (free tier) | **$0** |
| **13** | Azure SQL + Hosting | Temporary deployment | **$10-$50** |
| **14** | Observability | Application Insights (free tier) | **$0-$10** |
| **15** | AKS | Temporary, tear down after learning | **$50-$100** |

**Current Phase Cost:** $0 (Phase 2, everything local)

**Target Budget:** 
- Phases 1-7: $0/month
- Phases 8-13: $0-$30/month
- Phases 14-15: Variable (use credits, destroy after sessions)

### Local Alternatives Strategy

Learn concepts locally before paying for Azure:

| Azure Service | Local Alternative | Cost |
|---------------|------------------|------|
| Azure Service Bus | RabbitMQ (Docker) | $0 |
| Azure Cache for Redis | Redis (Docker) | $0 |
| Azure SQL Database | SQL Server (local/Express) | $0 |
| Azure Blob Storage | Azurite (local emulator) | $0 |
| Azure Functions | Local Functions runtime | $0 |
| AKS | Minikube/Kind (local) | $0 |

### Safety Rules

**Before creating ANY Azure resource:**
1. ✅ **FREE**: Safe to create and experiment
2. ⚠️ **LOW COST**: Monitor usage, set alerts
3. 🛑 **PAID**: Don't create until explicitly decided
4. 🧹 **DELETE AFTER TESTING**: Temporary resource, clean up immediately

**Protection measures:**
- Spending alerts at $5, $10, $25
- Azure budgets with automatic notifications
- Resource tags: `project=OrderFlow`, `environment=dev`, `auto-delete=yes`
- Use Azure Student Credits ($100/year) or Free Trial ($200/30 days)
- Regularly review Azure Cost Management

## Project Structure

```
OrderFlow/
├── src/
│   ├── OrderFlow.Domain/          # Entities, value objects, domain events, interfaces
│   ├── OrderFlow.Application/      # Use cases, DTOs, application services (Phase 3)
│   ├── OrderFlow.Infrastructure/   # EF Core, repositories, external services
│   └── OrderFlow.Api/             # Controllers, middleware, startup (Phase 1 complete)
└── tests/
    ├── OrderFlow.Domain.Tests/
    ├── OrderFlow.Application.Tests/
    └── OrderFlow.Integration.Tests/
```

## Key Architectural Decisions

### Outbox Pattern
- Ensures reliable event publishing
- Outbox messages stored in same transaction as domain changes
- Background processor publishes events asynchronously

### Entity Base Class
- Audit fields: CreatedAt, UpdatedAt, CreatedBy, UpdatedBy
- Domain events collection for event sourcing
- Soft delete support (IsDeleted, DeletedAt)

### Clean Architecture Layers
- **Domain**: No dependencies, pure business logic
- **Application**: Depends on Domain, defines interfaces
- **Infrastructure**: Implements interfaces, depends on Application
- **API**: Depends on Application and Infrastructure, composition root

### Why Modular Monolith First
- Understand service boundaries through real pain points
- Learn what needs to scale independently
- Extract microservices only when there's genuine architectural reason
- Avoid premature complexity

## Domain Model

### Bounded Contexts (Current)
- **Orders**: Order, OrderItem, OrderStatus
- **Products**: Product, ProductCategory
- **Customers**: Customer, CustomerAddress (planned)

### Bounded Contexts (Future)
- **Inventory**: Stock, Warehouse, Reservations
- **Payments**: PaymentMethod, Transaction, Refund
- **Shipping**: Shipment, Carrier, Tracking
- **Notifications**: Email, SMS, Push

## Notes

**Last Updated:** 2026-09-02

**Project Philosophy:**
- One permanent engineering laboratory evolving from monolith → distributed system
- Master .NET + Angular + Azure through real production-grade software
- Every concept learned improves the same system (not scattered tutorial projects)

**Development Approach:**
- Build locally first ($0 cost), then deploy to Azure when needed
- Understand "why" before "how" - solve real problems, not hypothetical ones
- Deliberately introduce problems at scale, then fix them (e.g., SQL optimization)
- Learn universal patterns first (architecture, DDD), then cloud specifics

**Key Principle:** Don't build "an Azure project" - build production software, then learn how to run it on Azure.

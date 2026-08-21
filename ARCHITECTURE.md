# OrderFlow Architecture

## Overview

OrderFlow is an order management system built with **Clean Architecture**, **Domain-Driven Design (DDD)**, and **CQRS** patterns. The architecture ensures separation of concerns, testability, and maintainability by organizing code into four distinct layers with clear dependency rules.

## Technology Stack

- **.NET 10.0** - Framework
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM
- **MediatR** - Command/Query/Event handling
- **SQL Server** - Database

## Architecture Layers

### 1. Domain Layer (Core)
**Zero dependencies** - Pure business logic

- **Entities**: `Order`, `OrderItem`, `Product`
  - Rich domain models with behavior
  - Inherit from `Entity` base class for domain event support
- **Value Objects**: `ShippingAddress` (immutable)
- **Enums**: `OrderStatus` (Pending → Confirmed → Processing → Shipped → Delivered)
- **Domain Events**: `OrderConfirmedEvent`

**Key Responsibilities:**
- Enforce business rules
- Raise domain events
- Manage entity lifecycle

### 2. Application Layer (Use Cases)
**Depends on**: Domain only

- **Commands** (write operations): `CreateOrderCommand`, `ConfirmOrderCommand`
- **Queries** (read operations): `GetOrderQuery`
- **Handlers**: Process commands/queries using MediatR
- **Event Handlers**: React to domain events
- **Abstractions**: Interfaces for repositories and services

**Key Responsibilities:**
- Orchestrate domain logic
- Define use cases
- Coordinate transactions

### 3. Infrastructure Layer (Technical Details)
**Depends on**: Application + Domain

- **DbContext**: `OrderFlowDbContext` (implements `IUnitOfWork`)
- **Repositories**: `OrderRepository`, `ProductRepository`
- **EF Configurations**: Entity mappings and database schema
- **Messaging**: `DomainEventDispatcher` (integrates with MediatR)

**Key Responsibilities:**
- Data access implementation
- External service integration
- Technical infrastructure concerns

### 4. API Layer (Presentation)
**Depends on**: Application + Domain

- **Controllers**: `OrdersController` (REST endpoints)
- **DI Configuration**: `Program.cs` wires up dependencies
- **HTTP Concerns**: Request/response handling

**Key Responsibilities:**
- HTTP interface
- Dependency injection setup
- API routing

## Key Patterns

### CQRS (Command Query Responsibility Segregation)
- **Commands**: Modify state (CreateOrder, ConfirmOrder)
- **Queries**: Read state (GetOrder → OrderDto)
- Separate models for reads and writes

### Mediator Pattern
- MediatR decouples controllers from handlers
- Single entry point: `mediator.Send(command)`

### Domain Events
1. Raised in domain methods: `order.Confirm()` → `OrderConfirmedEvent`
2. Stored in entity until persistence
3. Dispatched after `SaveChangesAsync()`
4. Handled by `OrderConfirmedNotificationHandler`

### Repository Pattern
- Abstracts data access
- Interfaces in Application, implementations in Infrastructure
- Works with aggregate roots only

### Unit of Work
- Coordinates transactions across repositories
- Single `SaveChangesAsync()` call commits all changes

### Aggregate Pattern
- **Order** is the aggregate root
- **OrderItem** is part of the aggregate (no direct repository access)
- All changes go through the aggregate root

## Request Flow

### Creating an Order
```
HTTP POST /api/orders
    ↓
OrdersController
    ↓
mediator.Send(CreateOrderCommand)
    ↓
CreateOrderHandler
    ├─ Validate products exist
    ├─ new Order(customerId, address)
    ├─ order.AddItem(productId, quantity, price)
    ├─ orderRepository.AddAsync(order)
    └─ unitOfWork.SaveChangesAsync()
    ↓
Return 201 Created with orderId
```

### Domain Event Flow
```
order.Confirm()
    ↓
RaiseDomainEvent(OrderConfirmedEvent)
    ↓
Event stored in _domainEvents
    ↓
unitOfWork.SaveChangesAsync()
    ↓
DomainEventDispatcher.DispatchAsync()
    ↓
Publishes OrderConfirmedNotification
    ↓
OrderConfirmedNotificationHandler executes
```

## Project Structure

```
OrderFlow/
├── src/
│   ├── OrderFlow.Domain/
│   │   ├── Entities/           # Order, OrderItem, Product
│   │   ├── ValueObjects/       # ShippingAddress
│   │   ├── Events/             # OrderConfirmedEvent
│   │   ├── Enums/              # OrderStatus
│   │   └── Common/             # Entity, IDomainEvent
│   │
│   ├── OrderFlow.Application/
│   │   ├── Orders/
│   │   │   ├── Commands/       # CreateOrder, ConfirmOrder
│   │   │   ├── Queries/        # GetOrder
│   │   │   └── Events/         # OrderConfirmedNotificationHandler
│   │   └── Abstractions/
│   │       ├── Persistence/    # IOrderRepository, IUnitOfWork
│   │       └── Messaging/      # IDomainEventDispatcher
│   │
│   ├── OrderFlow.Infrastructure/
│   │   ├── Persistence/
│   │   │   ├── OrderFlowDbContext.cs
│   │   │   └── Configurations/ # EF Core entity configurations
│   │   ├── Repositories/       # OrderRepository, ProductRepository
│   │   └── Messaging/          # DomainEventDispatcher
│   │
│   └── OrderFlow.Api/
│       ├── Controllers/        # OrdersController
│       └── Program.cs          # DI configuration
```

## API Endpoints

- `GET /api/orders/{id}` - Retrieve an order
- `POST /api/orders` - Create a new order

## Business Rules

- Orders start in `Pending` status
- Items can only be added to pending orders
- Order must have at least one item to be confirmed
- No duplicate products in same order
- Status transitions follow strict sequence
- Orders cannot be cancelled once shipped

## Benefits

- **Testability**: Each layer tested independently
- **Maintainability**: Clear separation of concerns
- **Flexibility**: Swap implementations without affecting domain
- **Business Focus**: Domain logic in pure C#
- **Scalability**: CQRS enables separate read/write optimization

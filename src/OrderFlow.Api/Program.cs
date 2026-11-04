using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Abstractions.Messaging;
using OrderFlow.Application.Abstractions.Persistence;
using OrderFlow.Application.Common.Behaviors;
using OrderFlow.Application.Orders.Commands.ConfirmOrder;
using OrderFlow.Application.Orders.Commands.CreateOrder;
using OrderFlow.Infrastructure.Messaging;
using OrderFlow.Infrastructure.Persistence;
using OrderFlow.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderFlowDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("OrderFlow")));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(CreateOrderCommand).Assembly);

builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

builder.Services.AddScoped<ConfirmOrderHandler>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderReadRepository, OrderReadRepository>();

builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<OrderFlowDbContext>());


builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
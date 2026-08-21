using MediatR;
using OrderFlow.Application.Abstractions.Persistence;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.ValueObjects;

namespace OrderFlow.Application.Orders.Commands.CreateOrder
{
    public sealed class CreateOrderHandler(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, Guid>
    {
        public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var address = new ShippingAddress(command.Street, command.City, command.PostalCode, command.Country);

            var order = new Order(command.CustomerId, address);

            var productIds = command.Items.Select(x => x.ProductId).Distinct().ToArray();

            var products = await productRepository.GetByIdsAsync(productIds, cancellationToken);

            foreach(var item in command.Items)
            {
                if (!products.TryGetValue(item.ProductId, out var product))
                {
                    throw new InvalidOperationException($"Product {item.ProductId} was not found.");
                }                    

                order.AddItem(item.ProductId, item.Quantity, product.Price);
            }

            await orderRepository.AddAsync(order, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return order.Id;

        }
    }
}

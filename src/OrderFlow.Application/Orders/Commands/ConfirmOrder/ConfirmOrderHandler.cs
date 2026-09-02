using OrderFlow.Application.Abstractions.Persistence;

namespace OrderFlow.Application.Orders.Commands.ConfirmOrder
{
    public class ConfirmOrderHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        public async Task Handle(ConfirmOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(command.OrderId, cancellationToken) 
                ?? throw new InvalidOperationException("Order not found.");

            order.Confirm();

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

using FluentValidation;

namespace OrderFlow.Application.Orders.Commands.CreateOrder
{
    public sealed class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();

            RuleFor(x => x.Items).NotEmpty();

            RuleForEach(x => x.Items).ChildRules(item =>
                {
                    item.RuleFor(x => x.ProductId).NotEmpty();
                    item.RuleFor(x => x.Quantity).GreaterThan(0);
                });
        }
    }
}

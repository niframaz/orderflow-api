using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Orders.Commands.CreateOrder;
using OrderFlow.Application.Orders.Queries.GetOrder;

namespace OrderFlow.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            var order = await mediator.Send(new GetOrderQuery(id), cancellationToken);

            if (order is null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = await mediator.Send(command, cancellationToken);

            return Created($"/api/orders/{orderId}", new {orderId});
        }
    }
}

using EventBus.Messages.Events;
using Mapster;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumer;

public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<BasketCheckoutConsumer> _logger;

    public BasketCheckoutConsumer(IMediator mediator, ILogger<BasketCheckoutConsumer> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        var command = context.Message.Adapt<CheckoutOrderCommand>();
        var result = await _mediator.Send(command);

        _logger.LogInformation("BasketCheckoutEvent consumed successfully. Created Order Id : {newOrderId}", result);
    }
}

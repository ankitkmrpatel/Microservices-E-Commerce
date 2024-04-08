using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler(IOrderRepository orderRepository, ILogger<UpdateOrderCommandHandler> logger) 
    : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly ILogger<UpdateOrderCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);
        if (orderToUpdate == null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }

        request.Adapt(orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));

        await _orderRepository.UpdateAsync(orderToUpdate);

        _logger.LogInformation($"Order {orderToUpdate.Id} is successfully updated.");
    }
}

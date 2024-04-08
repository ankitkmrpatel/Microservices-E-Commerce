using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger) 
    : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly ILogger<DeleteOrderCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToDelete = await _orderRepository.GetByIdAsync(request.Id);
        if (orderToDelete == null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }            

        await _orderRepository.DeleteAsync(orderToDelete);

        _logger.LogInformation($"Order {orderToDelete.Id} is successfully deleted.");
    }
}

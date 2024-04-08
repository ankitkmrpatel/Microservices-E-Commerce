using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandler(IOrderRepository orderRepository, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger) 
    : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly IEmailService _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    private readonly ILogger<CheckoutOrderCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = request.Adapt<Order>();
        var newOrder = await _orderRepository.AddAsync(orderEntity);
        
        _logger.LogInformation($"Order {newOrder.Id} is successfully created.");
        
        await SendMail(newOrder);

        return newOrder.Id;
    }

    private async Task SendMail(Order order)
    {            
        var email = new Email() { To = order.UserName, Body = $"Order#{order.Id} was created.", Subject = $"Order{order.Id} was created" };

        try
        {
            await _emailService.SendEmail(email);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Order {order.Id} failed due to an error with the mail service: {ex.Message}");
        }
    }
}

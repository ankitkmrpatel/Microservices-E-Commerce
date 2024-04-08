using Mapster;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrdersListQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrdersListQuery, List<OrdersVm>>
{
    private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));

    public async Task<List<OrdersVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
        var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
        return orderList.Adapt<List<OrdersVm>>();
    }
}

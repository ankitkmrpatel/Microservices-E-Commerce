using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    public class OrderService
    {
        [ProducesResponseType(typeof(IEnumerable<OrdersVm>), (int)HttpStatusCode.OK)]
        public async Task<IResult> GetOrdersByUserName(IMediator _mediator, string userName)
        {
            var query = new GetOrdersListQuery(userName);
            var orders = await _mediator.Send(query);
            return Results.Ok(orders);
        }

        //Testing Purpose
        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IResult> CheckoutOrder(IMediator _mediator, [FromBody] CheckoutOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Results.Ok(result);
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IResult> UpdateOrder(IMediator _mediator, [FromBody] UpdateOrderCommand command)
        {
            await _mediator.Send(command);
            return Results.NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IResult> DeleteOrder(IMediator _mediator, int id)
        {
            var command = new DeleteOrderCommand() 
            {
                Id = id,
                UserName = ""
            };

            await _mediator.Send(command);
            return Results.NoContent();
        }
    }
}

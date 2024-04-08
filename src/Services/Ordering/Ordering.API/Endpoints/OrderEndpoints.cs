using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Controllers;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using System.Net;

namespace Ordering.API.Endpoints;

public static class OrderEndpoints
{
    internal static void AddOrderEndpointsAPIs(this WebApplication app)
    {
        var handler = app.Services.GetRequiredService<OrderService>();

        app.MapGet("/order/{userName}", handler.GetOrdersByUserName)
            .WithName("GetOrder")
            .WithOpenApi();

        //app.MapGet("/basket/{userName}/{id}", handler.GetOrdersByUserName)
        //    .WithName("GetOrderById")
        //    .WithOpenApi();

        //Testing Purpose
        app.MapPost("/order", handler.CheckoutOrder)
            .WithName("CheckoutOrder")
            .WithOpenApi();

        app.MapPut("/order", handler.UpdateOrder)
            .WithName("UpdateOrder")
            .WithOpenApi();

        app.MapDelete("/order/{id}", handler.DeleteOrder)
            .WithName("DeleteOrder")
            .WithOpenApi();
    }
}

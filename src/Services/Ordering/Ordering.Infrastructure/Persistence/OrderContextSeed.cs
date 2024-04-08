using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {            
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetPreconfiguredOrders());
            await orderContext.SaveChangesAsync();
            logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
        }
    }

    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
        return new List<Order>
        {
            new()
            {
                UserName = "ankit@sflhub.com",
                FirstName = "Ankit",
                LastName = "Kumar",
                EmailAddress = "ankit@sflhub.com",
                AddressLine = "New Delhi",
                Country = "Delhi",
                TotalPrice = 350,
                CVV = "",
                CardName= "",
                CardNumber = "",
                State = "",
                ZipCode = "",
                Expiration = "",
            }
        };
    }
}

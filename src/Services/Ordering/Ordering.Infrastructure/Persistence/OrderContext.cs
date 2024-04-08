using Microsoft.EntityFrameworkCore;
using Ordering.Domain;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContext(DbContextOptions<OrderContext> options) 
    : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.Now;
                    entry.Entity.CreatedBy = "ankit@sflhub.com";
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    entry.Entity.LastModifiedBy = "ankit@sflhub.com";
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}

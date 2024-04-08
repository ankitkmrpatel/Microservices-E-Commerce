namespace Discount.gRPC.Repo;

public interface IDatabaseConfiguration
{
    string? GetConnectionString();
    void MigrateDb();
}

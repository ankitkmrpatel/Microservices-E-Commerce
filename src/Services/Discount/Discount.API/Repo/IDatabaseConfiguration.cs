namespace Discount.API.Repo;

public interface IDatabaseConfiguration
{
    string? GetConnectionString();
    void MigrateDb();
}

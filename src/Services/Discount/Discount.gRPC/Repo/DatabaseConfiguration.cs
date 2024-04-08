using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.gRPC.Repo
{
    public class DatabaseConfiguration(IConfiguration configuration) : IDatabaseConfiguration
    {
        private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        public void MigrateDb()
        {
            using var connection = new NpgsqlConnection(GetConnectionString());
            connection.Open();

            using var command = new NpgsqlCommand
            {
                Connection = connection
            };

            command.CommandText = "DROP TABLE IF EXISTS Coupon";
            command.ExecuteNonQuery();

            command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
            command.ExecuteNonQuery();


            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
            command.ExecuteNonQuery();
        }

        public string? GetConnectionString()
        {
            return _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        }
    }
}

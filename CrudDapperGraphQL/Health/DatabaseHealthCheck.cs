using CrudDapperGraphQL.Data;
using CrudDapperGraphQL.Data.Models;
using Dapper;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data;

namespace CrudDapperGraphQL.Health
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseHealthCheck(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = new()
        )
        {
            try
            {
                using (var connection = await _dbContext.CreateConnectionAsync())
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT 1";
                        command.ExecuteScalar();
                    }
                    connection.Close();
                    return HealthCheckResult.Healthy();
                };
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(exception: ex);
            }
        }
    }
}

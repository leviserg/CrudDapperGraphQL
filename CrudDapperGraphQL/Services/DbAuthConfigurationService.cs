using CrudDapperGraphQL.Auth;
using CrudDapperGraphQL.Data;
using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;
using Dapper;
using System.Data;

namespace CrudDapperGraphQL.Services
{
    public class DbAuthConfigurationService : IAuthConfigurationService
    {
        // Add your database context or data access code here
        private readonly ApplicationDbContext _dbContext;

        public DbAuthConfigurationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SecretKeysModel> GetSecretKeys()
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var config = await connection.QuerySingleOrDefaultAsync<SecretKeysModel>(
                    SpNames.Auth_GetKeys,
                    null,
                    commandType: CommandType.StoredProcedure
                );
                return config;
            }
        }
    }
}

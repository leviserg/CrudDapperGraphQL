using Microsoft.Data.SqlClient;
using System.Data;

namespace CrudDapperGraphQL.Data
{
    public class ApplicationDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private const string CONN_STRING_KEY = "BookLibraryDbConnection";
        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = Environment.GetEnvironmentVariable(CONN_STRING_KEY);
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = await Task.FromResult(CreateConnection());
            return connection;
        }

    }
}

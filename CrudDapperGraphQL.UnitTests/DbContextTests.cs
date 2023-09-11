using CrudDapperGraphQL.Data;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CrudDapperGraphQL.UnitTests
{
    [TestClass]
    public class DbContextTests
    {

        [TestMethod]
        public void TestDbContextConnection()
        {
            // Arrange

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbContext = new ApplicationDbContext(configuration);
            
            // Act
            IDbConnection connection = dbContext.CreateConnection();

            // Assert
            Assert.IsNotNull(connection);

            try
            {
                connection.Open();
                ConnectionState assertConnectionState = connection.State;
                Assert.AreEqual(ConnectionState.Open, assertConnectionState);
            }
            finally { 
                connection.Close();
                connection.Dispose();
            }

        }
    }
}
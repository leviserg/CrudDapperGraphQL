using CrudDapperGraphQL.Data;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Data;

namespace CrudDapperGraphQL.UnitTests
{
    [TestClass]
    public class DbContextTests
    {

        [TestMethod]
        public void TestDbContextConnection()
        {
            /* TODO: use this part to test actual connection to DB
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbContext = new ApplicationDbContext(configuration);
            
            IDbConnection connection = dbContext.CreateConnection();
            */

            var mockConnection = new Mock<IDbConnection>();
            mockConnection.Setup(m => m.State).Returns(ConnectionState.Closed);
            mockConnection.Setup(m => m.Open()).Callback(() => mockConnection.Setup(m => m.State).Returns(ConnectionState.Open));
            mockConnection.Setup(m => m.Close()).Callback(() => mockConnection.Setup(m => m.State).Returns(ConnectionState.Closed));

            IDbConnection connection = mockConnection.Object;

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
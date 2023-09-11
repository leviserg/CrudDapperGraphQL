using CrudDapperGraphQL.Data.Contracts.Repositories;
using CrudDapperGraphQL.Data.Models;
using Dapper;
using System.Data;

namespace CrudDapperGraphQL.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BookRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Book>> GetBooks(FilterModel filter)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var books = await connection.QueryAsync<Book>(
                    SpNames.Book_GetAll,
                    new { filter?.OrderBy, OrderDirection = (int)filter?.OrderDirection },
                    commandType: CommandType.StoredProcedure
                );
                return books.ToList();
            }
        }

        public async Task<Book> GetBook(int bookId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
               var book = await connection.QueryFirstOrDefaultAsync<Book>(
                    SpNames.Book_Get,
                    new { Id = bookId },
                    commandType: CommandType.StoredProcedure
                );
                return book;
            }
        }

    }
}

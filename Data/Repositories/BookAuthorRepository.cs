using CrudDapperGraphQL.Data.Contracts;
using CrudDapperGraphQL.Data.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Data;

namespace CrudDapperGraphQL.Data.Repositories
{
    public class BookAuthorRepository : IBookAuthorRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BookAuthorRepository(ApplicationDbContext dbContext)
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

    }
}

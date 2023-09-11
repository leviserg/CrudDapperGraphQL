using CrudDapperGraphQL.Data.Contracts.Repositories;
using CrudDapperGraphQL.Data.Models;
using Dapper;
using System.Data;

namespace CrudDapperGraphQL.Data.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AuthorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Author>> GetAuthors(FilterModel filter)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var authors = await connection.QueryAsync<Author>(
                    SpNames.Author_GetAll,
                    new { filter?.OrderBy, OrderDirection = (int)filter?.OrderDirection },
                    commandType: CommandType.StoredProcedure
                );
                return authors.ToList();
            }
        }

        public async Task<Author> GetAuthor(int authorId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var author = await connection.QueryFirstOrDefaultAsync<Author>(
                     SpNames.Author_Get,
                     new { Id = authorId },
                     commandType: CommandType.StoredProcedure
                 );
                return author;
            }
        }
    }
}

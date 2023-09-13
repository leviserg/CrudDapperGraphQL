using CrudDapperGraphQL.Data.Contracts.Repositories;
using CrudDapperGraphQL.Data.Models;
using Dapper;
using SendGrid.Helpers.Mail;
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
                    new
                    {
                        OrderBy = filter?.OrderBy,
                        OrderDirection = (int)filter?.OrderDirection,
                        Limit = filter?.Limit,
                        Offset = filter?.Offset,
                        SearchText = filter?.SearchText
                    },
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

        public async Task<Author> AuthorSave(AuthorSave author)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var result = await connection.QueryAsync<Author>(
                     SpNames.Author_CreateOrUpdate, author,
                     commandType: CommandType.StoredProcedure
                 );
                return result.FirstOrDefault();
            }
        }
    }
}

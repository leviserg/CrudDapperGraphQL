using CrudDapperGraphQL.Data.Contracts.Repositories;
using CrudDapperGraphQL.Data.Models;
using Dapper;
using SendGrid.Helpers.Mail;
using System.Data;

namespace CrudDapperGraphQL.Data.Repositories
{
    public class AuthorRepository : IRepository<Author, AuthorSave>
    {
        private readonly ApplicationDbContext _dbContext;
        public AuthorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Author>> GetAll(FilterModel filter)
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

        public async Task<Author> GetById(int authorId)
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

        public async Task<Author> Save(AuthorSave author)
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
        public async Task<bool> Delete(int authorId)
        {
            try
            {
                using (var connection = _dbContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<Author>(
                         SpNames.Author_Delete, new { Id = authorId },
                         commandType: CommandType.StoredProcedure
                     );
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

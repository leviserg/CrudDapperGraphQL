using CrudDapperGraphQL.Data.Contracts.Repositories;
using CrudDapperGraphQL.Data.Enums;
using CrudDapperGraphQL.Data.Models;
using Dapper;
using SendGrid.Helpers.Mail;
using System.Data;

namespace CrudDapperGraphQL.Data.Repositories
{
    public class BookRepository : IRepository<Book, BookSave>
    {
        private readonly ApplicationDbContext _dbContext;
        public BookRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Book>> GetAll(FilterModel filter)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var books = await connection.QueryAsync<Book>(
                    SpNames.Book_GetAll,
                    new {
                        OrderBy = filter?.OrderBy,
                        OrderDirection = (int)filter?.OrderDirection,
                        Limit = filter?.Limit,
                        Offset = filter?.Offset,
                        SearchText = filter?.SearchText
                    },
                    commandType: CommandType.StoredProcedure
                );
                return books.ToList();
            }
        }

        public async Task<Book> GetById(int bookId)
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

        public async Task<Book> Save(BookSave book)
        {
            var authorIdsTable = new DataTable();
            authorIdsTable.Columns.Add("value", typeof(int));

            book.AuthorIds.ForEach(x => { authorIdsTable.Rows.Add(x); });

            using (var connection = _dbContext.CreateConnection())
            {
                var result = await connection.QueryAsync<Book>(
                     SpNames.Book_CreateOrUpdate,
                     new
                     {
                         Id = book.Id,
                         Title = book.Title,
                         ReleaseDate = book.ReleaseDate,
                         AuthorIds = authorIdsTable.AsTableValuedParameter("[dbo].[ListInt]")
                     },
                     commandType: CommandType.StoredProcedure
                 );
                return result.FirstOrDefault();
            }
        }

        public async Task<bool> Delete(int bookId)
        {
            try
            {
                using (var connection = _dbContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<Author>(
                         SpNames.Book_Delete, new { Id = bookId },
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

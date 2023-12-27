using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;
using HotChocolate.Authorization;

namespace CrudDapperGraphQL.GraphQL
{
    [Authorize]
    public class Mutation
    {
        public async Task<Book> saveBook(
            [Service] IEntityService<Book, BookSave> service, BookSave book)
        {
            return await service.Save(book);
        }

        public async Task<Author> saveAuthor(
            [Service] IEntityService<Author, AuthorSave> service, AuthorSave author)
        {
            return await service.Save(author);
        }
    }


}

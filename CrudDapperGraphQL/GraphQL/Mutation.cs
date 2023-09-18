using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.GraphQL
{
    public class Mutation
    {
        public async Task<Book> saveBook(
            [Service] IBookService service, BookSave book)
        {
            return await service.BookSave(book);
        }

        public async Task<Author> saveAuthor(
            [Service] IAuthorService service, AuthorSave author)
        {
            return await service.AuthorSave(author);
        }
    }


}

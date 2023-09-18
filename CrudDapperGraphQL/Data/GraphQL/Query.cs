using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.Data.GraphQL
{
    public class Query
    {
        public async Task<IEnumerable<Book>> getBooks(
            [Service(ServiceKind.Synchronized)] IBookService service, FilterModel? filter
        )
        {
            FilterModel filterInput = filter?.ToCompleteFilterModel(filter) ?? new FilterModel();
            return await service.GetBooks(filterInput);
        }

        public async Task<Book> getBook(
            [Service] IBookService service, int id)
        {
            return await service.GetBook(id);
        }

        public async Task<IEnumerable<Author>> getAuthors(
            [Service] IAuthorService service, FilterModel? filter
        )
        {
            FilterModel filterInput = filter?.ToCompleteFilterModel(filter) ?? new FilterModel();
            return await service.GetAuthors(filterInput);
        }

        public async Task<Author> getAuthor(
            [Service] IAuthorService service, int id)
        {
            return await service.GetAuthor(id);
        }
    }


}

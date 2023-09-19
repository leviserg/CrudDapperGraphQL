using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;
using HotChocolate.Authorization;

namespace CrudDapperGraphQL.GraphQL
{
    [Authorize]
    public class Query
    {
        public async Task<IEnumerable<Book>> getBooks(
            [Service(ServiceKind.Synchronized)] IBookService service, FilterModel? filter
        )
        {
            FilterModel filterInput = filter?.ToCompleteFilterModel(filter) ?? new FilterModel();
            try
            {
                return await service.GetBooks(filterInput);
            }
            catch(UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
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

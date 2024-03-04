using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Models;
using HotChocolate.Authorization;

namespace CrudDapperGraphQL.GraphQL
{
    [Authorize]
    public class Query
    {
        public async Task<IEnumerable<Book>> getBooks(
            [Service(ServiceKind.Synchronized)] IEntityService<Book, BookSave> service, FilterModel? filter
        )
        {
            FilterModel filterInput = filter?.ToCompleteFilterModel(filter) ?? new FilterModel();
            try
            {
                return await service.GetAll(filterInput);
            }
            catch(UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
        }

        public async Task<Book> getBook(
            [Service] IEntityService<Book, BookSave> service, int id)
        {
            return await service.GetById(id);
        }

        public async Task<IEnumerable<Author>> getAuthors(
            [Service] IEntityService<Author, AuthorSave> service, FilterModel? filter
        )
        {
            FilterModel filterInput = filter?.ToCompleteFilterModel(filter) ?? new FilterModel();
            return await service.GetAll(filterInput);
        }

        public async Task<Author> getAuthor(
            [Service] IEntityService<Author, AuthorSave> service, int id)
        {
            return await service.GetById(id);
        }
    }


}

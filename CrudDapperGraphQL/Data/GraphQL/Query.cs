using CrudDapperGraphQL.Data.Contracts.Services;
using CrudDapperGraphQL.Data.Enums;
using CrudDapperGraphQL.Data.Models;
using System.Collections.Generic;

namespace CrudDapperGraphQL.Data.GraphQL
{
    public class Query
    {
        public Task<IEnumerable<Book>> getBooks(
            [Service(ServiceKind.Synchronized)] IBookService service,
            FilterModel? filter,
            CancellationToken cancellationToken
        )
        {
            FilterModel filterInput = filter?.ToCompleteFilterModel(filter) ?? new FilterModel();
            return service.GetBooks(filterInput, cancellationToken);
        }

        public Task<Book> getBook(
            [Service] IBookService service,
            int id,
            CancellationToken cancellationToken)
        {
            return service.GetBook(id, cancellationToken);
        }

        public Task<IEnumerable<Author>> getAuthors(
            [Service] IAuthorService service,
            FilterModel? filter,
            CancellationToken cancellationToken
        )
        {
            FilterModel filterInput = filter?.ToCompleteFilterModel(filter) ?? new FilterModel();
            return service.GetAuthors(filterInput, cancellationToken);
        }

        public Task<Author> getAuthor(
            [Service] IAuthorService service,
            int id,
            CancellationToken cancellationToken)
        {
            return service.GetAuthor(id, cancellationToken);
        }
    }


}

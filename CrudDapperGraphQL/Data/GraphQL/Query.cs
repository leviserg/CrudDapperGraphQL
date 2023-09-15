using CrudDapperGraphQL.Data.Models;
using CrudDapperGraphQL.Services;
using HotChocolate;

namespace CrudDapperGraphQL.Data.GraphQL
{
    public class Query
    {
        public Task<IEnumerable<Book>> getBooks(
        [Service] BookService service,
        [ScopedService] CancellationToken cancellationToken)
        {
            FilterModel filter = new FilterModel();
            return service.GetBooks(filter,cancellationToken);
        }

        public Task<Book> getBookById(
            [Service] BookService service,
            int id,
            CancellationToken cancellationToken)
        {
            return service.GetBook(id, cancellationToken);
        }
        
    }

    /*
    // QueryType.cs
    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor
                .Field(f => f.getBookById(default!, default!, default!))
                .Type<BookType>();

            descriptor
                .Field(f => f.getBooks(default!, null, default!))
                .Type<ListType<BookType>>();
        }
    }

    // TodoType.cs
    public class BookType : ObjectType<Book>
    {
        protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
        {
            descriptor
                .Field(f => f.Id)
                .Type<IntType>();

            descriptor
                .Field(f => f.Title)
                .Type<StringType>();

            descriptor
                .Field(f => f.ReleaseDate)
                .Type<DateTimeType>();
        }
    }
    */
}

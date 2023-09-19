using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.GraphQL.InputTypes
{
    public class BookSaveInput : ObjectType<BookSave>
    {
        protected override void Configure(IObjectTypeDescriptor<BookSave> descriptor)
        {
            // BookSaveInputType
            descriptor.Name("BookSaveInput");
            descriptor.Field("id").Type<IntType>();
            descriptor.Field("title").Type<StringType>();
            descriptor.Field("releaseDate").Type<DateTimeType>();
            descriptor.Field("authorIds").Type<ListType<IntType>>();
            descriptor.Authorize();

        }
    }
}

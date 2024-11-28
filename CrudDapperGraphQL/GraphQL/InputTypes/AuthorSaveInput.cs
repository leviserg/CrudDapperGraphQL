using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.GraphQL.InputTypes
{
    public class AuthorSaveInput : ObjectType<Author>
    {
        protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
        {
            // AuthorSaveInputType
            descriptor.Name("AuthorSaveInput");
            descriptor.Field("id").Type<IntType>();
            descriptor.Field("name").Type<StringType>();
            descriptor.Field("surname").Type<StringType>();
            descriptor.Authorize();

        }
    }
}

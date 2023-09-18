using CrudDapperGraphQL.Data.Enums;

namespace CrudDapperGraphQL.Data.GraphQL
{
    public class SchemaTypes : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            // FilterModelInputType
            descriptor.Name("FilterModelInput");
            descriptor.Field("orderBy").Type<StringType>();
            descriptor.Field("orderDirection").Type<EnumType<OrderDirectionEnum>>();
            descriptor.Field("limit").Type<IntType>();
            descriptor.Field("offset").Type<IntType>();
            descriptor.Field("searchText").Type<StringType>();

        }
    }
}

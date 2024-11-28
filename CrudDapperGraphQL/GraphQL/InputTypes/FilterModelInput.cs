using CrudDapperGraphQL.Data.Enums;
using CrudDapperGraphQL.Data.Models;

namespace CrudDapperGraphQL.GraphQL.InputTypes
{
    public class FilterModelInput : ObjectType<FilterModel>
    {
        protected override void Configure(IObjectTypeDescriptor<FilterModel> descriptor)
        {
            // FilterModelInputType
            descriptor.Name("FilterModelInput");
            descriptor.Field("orderBy").Type<StringType>();
            descriptor.Field("orderDirection").Type<EnumType<OrderDirectionEnum>>();
            descriptor.Field("limit").Type<IntType>();
            descriptor.Field("offset").Type<IntType>();
            descriptor.Field("searchText").Type<StringType>();
            descriptor.Authorize();
        }
    }
}

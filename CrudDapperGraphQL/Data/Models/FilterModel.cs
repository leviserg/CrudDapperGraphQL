using CrudDapperGraphQL.Data.Enums;

namespace CrudDapperGraphQL.Data.Models
{
    public class FilterModel
    {
        public string OrderBy { get; set; }
        public OrderDirection OrderDirection { get; set; }

        public FilterModel()
        {
            OrderBy = "Title";
            OrderDirection = OrderDirection.ASC;
        }
    }
}

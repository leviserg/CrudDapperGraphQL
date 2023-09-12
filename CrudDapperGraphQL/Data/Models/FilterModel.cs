using CrudDapperGraphQL.Data.Enums;

namespace CrudDapperGraphQL.Data.Models
{
    public class FilterModel
    {
        public string OrderBy { get; set; }
        public OrderDirection OrderDirection { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string SearchText { get; set; }

        public FilterModel()
        {
            OrderBy = "Title";
            OrderDirection = OrderDirection.ASC;
            Limit = 20;
            Offset = 0;
            SearchText= string.Empty;
        }
    }
}

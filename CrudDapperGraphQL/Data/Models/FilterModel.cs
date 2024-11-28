using CrudDapperGraphQL.Data.Enums;

namespace CrudDapperGraphQL.Data.Models
{
    public class FilterModel
    {
        public string? OrderBy { get; set; }
        public OrderDirectionEnum? OrderDirection { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public string? SearchText { get; set; }

        public FilterModel()
        {
            OrderBy = "Title";
            OrderDirection = OrderDirectionEnum.ASC;
            Limit = 20;
            Offset = 0;
            SearchText= string.Empty;
        }

        public FilterModel ToCompleteFilterModel(FilterModel filter)
        {
            return new FilterModel
            {
                OrderBy = filter?.OrderBy ?? "Title",
                OrderDirection = filter?.OrderDirection ?? OrderDirectionEnum.DESC,
                Limit = filter?.Limit ?? 20,
                Offset = filter?.Offset ?? 0,
                SearchText = filter?.SearchText ?? string.Empty
            };
        }

    }
}

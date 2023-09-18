namespace CrudDapperGraphQL.Data.Models
{
    public class BookSave
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<int> AuthorIds { get; set; }

    }
}

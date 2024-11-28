using HotChocolate.Authorization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrudDapperGraphQL.Data.Models
{
    [Authorize]
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int TotalCount { get; set; }

        [JsonIgnore]
        public string BooksJson { get; set; }
        public List<Book> Books => !string.IsNullOrEmpty(BooksJson) ? JsonSerializer.Deserialize<List<Book>>(BooksJson) : null;
    }
}

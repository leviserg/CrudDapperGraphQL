using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrudDapperGraphQL.Data.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }

        [JsonIgnore]
        public string AuthorsJson { get; set; }
        public List<Author> Authors => !string.IsNullOrEmpty(AuthorsJson) ? JsonSerializer.Deserialize<List<Author>>(AuthorsJson) : null;

    }
}

using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrudDapperGraphQL.Data.Models
{
    public class AuthorSave
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

    }
}

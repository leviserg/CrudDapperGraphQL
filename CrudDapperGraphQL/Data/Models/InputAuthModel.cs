using System.ComponentModel.DataAnnotations;

namespace CrudDapperGraphQL.Data.Models
{
    public class InputAuthModel
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 5)]
        public string ClientId { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public string ClientSecret { get; set; }
    }
}

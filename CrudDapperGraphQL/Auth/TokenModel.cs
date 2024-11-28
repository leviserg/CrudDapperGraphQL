namespace CrudDapperGraphQL.Auth
{
    public class TokenModel
    {
        public string? Token { get; set; }
        public int ExpirationSeconds { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}

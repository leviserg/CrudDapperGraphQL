using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace CrudDapperGraphQL.Auth
{
    public static class AuthOptions
    {
        public const int LifeTimeMinutes = 5;
        public const string Issuer = "MyIssuer";
        public const string Audience = "MyServer";
        public static SecurityKey PublicKey { get; set; }
        public static SecurityKey PrivateKey { get; set; }

        public static void Initialize(SecretKeysModel secretKeys)
        {
            PublicKey = GetPublicKey(secretKeys.PublicKeyString);
            PrivateKey = GetPrivateKey(secretKeys.PrivateKeyString);
        }

        private static SecurityKey GetPrivateKey(string keyString)
        {
            var key = RSA.Create();
            key.ImportRSAPrivateKey(source: Convert.FromBase64String(keyString), bytesRead: out int _);
            return new RsaSecurityKey(key);
        }

        private static SecurityKey GetPublicKey(string keyString)
        {
            var key = RSA.Create();
            key.ImportRSAPublicKey(source: Convert.FromBase64String(keyString), bytesRead: out int _);
            return new RsaSecurityKey(key);
        }

    }
}

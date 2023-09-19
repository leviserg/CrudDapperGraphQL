using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text.Json;

namespace CrudDapperGraphQL.Auth
{
    public static class AuthOptions
    {
        public const int LifeTimeMinutes = 5;
        public const string Issuer = "MyIssuer";
        public const string Audience = "MyServer";
        // get from rsaKey.json in KeyGenerator project
        public const string PrivateKeyString = "MIIEowIBAAKCAQEArf4n0zb8vLkiws23urru67nmcY6gG+Um6SbrQ9l/6cKwwkrQl/0+nOdFf3v5AYdXoFiaFBVXP94Ft8ivGP0xUbXEoIYd6AWlx22mp2xNGErGsd6lPFU3ptOHpebKefoO5x017cUZM2Mrbxa5tbvt9MJet2WpuZ5x6+K3eQDXAXmazoxNzAwpBgFP0p4u8LsWZphacjf5pb8JTXOiQo1YSh6EK1+FQylpGclYtxiAOumWg8fdMZkvxkO55RUJXBHKt/QV6NZotcm0WGNw+WrkcoDmyQ0+/+LaqavZXc7oaYiBf11PgvQBpdXAeX+UpJJyHSFdi20Fura4FrBKjjnRgQIDAQABAoIBACHx/e8VQUXIkOGUpQ3HPqm6wRzSiKYolOjT8P5xxqTimP2u/vdILxkJfeObWj9UGmJsJtNYPod1V4Q9oPutGhwo7E2tHNSRlYBNAkCTvo7It+8n0vDsZ0ki58oUNtiJUrMAXe8fjwUZifXIZz7vhUNFUJlTOkO+h9dPiiAPa+MbdgxKFi4oV8XuWzaYP4JX3b/bsUHpU3jWyL7c6CUAG6CvAR09jdagn7gEoLCz4JH5bOhw+uHPd+K3U33SFwzhoXPOGoHcl+2vHAoIjJijc9S3gHxwGP5+T0C1eCBShhNi3i4I6+qCiySHInFkQnjLUcUlE8SBBJZ0sknnsUXJn8kCgYEAygMZa8GZGYfI6ChN9AGebA+PYc/5lIIrssuAzCUW3WErNnntctOZBNPLnCI8yxx3NrjzIk8nmHMbfIUlomRm4n3VbsN/pbXI0mcChC1ZVbGvS7pEFFcs+BZ1sEJEU+C4PmyKtPltoGLmQXVsq/eM9HPU+EciWMkKxQrYPFi1pW8CgYEA3H4U2clyqVX5ReOquiQuuSllczc8KjsyPXJ/pnb+5ZIfb5C3aAF5cI3SoqWBTZvS8TGqatI4DeQNKn28cvYNXj9VljVBGit4YPqd8GrXXVX0ijNViBHCYkEbeYULMWNGChz4xDGzHaVlzl9Hkbg85nJXY6xjzRwXpv21eWBs4A8CgYAJ6PMDTUEEdpvNf8SrNrUd2fmPs9MrjOM15zPPT/Z6L70d9AdI/cZg7T7szuUqlZ/niFUtFrL7kJIFnsaE2+YMMF4bC+4kI/HRGIqQD2V1hbzyuxWB5fDnzrpBRk7xynFfZpW7YQ0WtCNVjLwjB4bbqx8EewDdWCd2GR0YgMWQcwKBgQCH/E6Zvw8zdq2lN/Ncl+IlGm3SMDewCBBFK2+k1/D+3y76HLOwtnASRbWp0A8+MSNY0/u5o+skTgj8ss1dzXiTLtZ3LGL5Y+P7U7XCx/IJQ3DtJxnSMLnE5Uivmqk7jXFt3U5jmg9Q8mgmbkbyjUEqE0zoUTLAFlpUCFc7A4hCawKBgHgW5hRjV6Mh51aiqA3q+JgWzH5yjQCyBdjpEXCM/m/nXMg3fUnV05VO1HqRCVj3wiKZ0Ig5k9PAqSeaQOCceuibm94+xTyDxoHoPdp5Z0gEfjo2UQAOtud3p8uijKZNZOiM+Wt0zgiFujWxwnC032VpBtosp9qvmgQD3j5oP1pe";
        public const string PublicKeyString = "MIIBCgKCAQEArf4n0zb8vLkiws23urru67nmcY6gG+Um6SbrQ9l/6cKwwkrQl/0+nOdFf3v5AYdXoFiaFBVXP94Ft8ivGP0xUbXEoIYd6AWlx22mp2xNGErGsd6lPFU3ptOHpebKefoO5x017cUZM2Mrbxa5tbvt9MJet2WpuZ5x6+K3eQDXAXmazoxNzAwpBgFP0p4u8LsWZphacjf5pb8JTXOiQo1YSh6EK1+FQylpGclYtxiAOumWg8fdMZkvxkO55RUJXBHKt/QV6NZotcm0WGNw+WrkcoDmyQ0+/+LaqavZXc7oaYiBf11PgvQBpdXAeX+UpJJyHSFdi20Fura4FrBKjjnRgQIDAQAB";
        public static SecurityKey PublicKey = GetPublicKey();
        public static SecurityKey PrivateKey = GetPrivateKey();

        private static SecurityKey GetPrivateKey()
        {
            var key = RSA.Create();
            key.ImportRSAPrivateKey(source: Convert.FromBase64String(PrivateKeyString), bytesRead: out int _);
            return new RsaSecurityKey(key);
        }

        private static SecurityKey GetPublicKey()
        {
            var key = RSA.Create();
            key.ImportRSAPublicKey(source: Convert.FromBase64String(PublicKeyString), bytesRead: out int _);
            return new RsaSecurityKey(key);
        }

    }
}

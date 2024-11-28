using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace CrudDapperGraphQL.Auth
{
    public static class AuthExtensions
    {
        public static bool VerifyMd5Hash(string input, string hash)
        {
            string hashOfInput = GetMd5Hash(input);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }

        private static string GetMd5Hash(this string content)
        {
            using MD5 md5Hash = MD5.Create();

            byte[] hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(content));
            string result = string.Concat(hash.Select(b => b.ToString("x2")));
            Debug.WriteLine($"{content} : {result}");
            return result;
        }

    }
}

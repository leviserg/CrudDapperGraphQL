﻿using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace CrudDapperGraphQL.Auth
{
    public static class AuthExtensions
    {
        public static string GetMd5Hash(this string content)
        {
            using MD5 md5Hash = MD5.Create();

            byte[] hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(content));
            string result = string.Concat(hash.Select(b => b.ToString("x2")));
            Debug.WriteLine($"{content} : {result}");
            return result;
        }

        public static bool VerifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(input);
            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

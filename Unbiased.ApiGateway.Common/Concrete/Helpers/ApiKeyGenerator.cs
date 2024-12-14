using System;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;
using System.Text;


namespace Unbiased.ApiGateway.Common.Concrete.Helpers
{
    public static class ApiKeyGenerator
    {
        private const int KeyLength = 32;

        public static string GenerateApiKey(string prefix = null)
        {
            var guid=Guid.NewGuid().ToString();
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = new byte[KeyLength];
                md5.ComputeHash(Encoding.ASCII.GetBytes(prefix is null? guid : prefix));
                byte[] hash = md5.Hash;
                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    strBuilder.Append(hash[i].ToString("x3"));
                }
                return strBuilder.ToString();
            }
        }

        public static string GenerateApiKeyWithPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentException("Prefix cannot be null or empty", nameof(prefix));
            }

            prefix = GenerateApiKey(prefix);
            var key = GenerateApiKey();
            return $"KEY{prefix}.{key}UNBIASED";
        }

        public static string GenerateTimestampedApiKey()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString("X"); 
            var key = GenerateApiKey();
            return $"{timestamp}.{key}";
        }
    }
}

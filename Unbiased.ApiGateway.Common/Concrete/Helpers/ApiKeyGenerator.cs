using System.Security.Cryptography;
using System.Text;


namespace Unbiased.ApiGateway.Common.Concrete.Helpers
{
    /// <summary>
    /// Provides a static class for generating API keys.
    /// </summary>
    public static class ApiKeyGenerator
    {
        private const int KeyLength = 32;

        /// <summary>
        /// Generates a new API key with an optional prefix.
        /// </summary>
        /// <param name="prefix">An optional prefix to include in the API key.</param>
        /// <returns>A new API key as a string.</returns>
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

        /// <summary>
        /// Generates a new API key with a prefix and a suffix.
        /// </summary>
        /// <param name="prefix">The prefix to include in the API key.</param>
        /// <returns>A new API key as a string.</returns>
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

        /// <summary>
        /// Generates a new API key with a timestamp.
        /// </summary>
        /// <returns>A new API key as a string.</returns>
        public static string GenerateTimestampedApiKey()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString("X"); 
            var key = GenerateApiKey();
            return $"{timestamp}.{key}";
        }
    }
}

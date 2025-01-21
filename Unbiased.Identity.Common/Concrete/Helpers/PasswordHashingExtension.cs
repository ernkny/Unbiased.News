using System.Security.Cryptography;

namespace Unbiased.Identity.Common.Concrete.Helpers
{
    public static class PasswordHashingExtension
    {
        public static string ToPBKDF2Hash(this string password, int iterations = 10000, int hashByteSize = 32)
        {
            var salt = GenerateSalt();
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = rfc2898DeriveBytes.GetBytes(hashByteSize);
                return Convert.ToBase64String(hash);
            }
        }

        private static byte[] GenerateSalt(int size = 16)
        {
            byte[] salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}

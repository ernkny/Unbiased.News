using System.Security.Cryptography;

namespace Unbiased.Identity.Common.Concrete.Helpers
{
    public static class PasswordHashingExtension
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;
        public static string ToPBKDF2Hash(string password)
        {
            var salt = GenerateSalt();
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
            return $"{Convert.ToBase64String(hash)}-{Convert.ToHexString(salt)}";
        }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}

using System.Security.Cryptography;

namespace whatsapp2api.Helpers
{
    public static class Crypto
    {
        public static byte[] Salt(int bytes = 256)
        {
            var saltBytes = new byte[bytes];

            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(saltBytes);

            return saltBytes;
        }

        public static byte[] Hash(string text, byte[] salt, int hashIterations = 500,
            int iterations = 20)
        {
            using var rfc2898 = new Rfc2898DeriveBytes(text, salt, hashIterations,
                HashAlgorithmName.SHA512);

            return rfc2898.GetBytes(iterations);
        }
    }
}
using System;
using System.Security.Cryptography;
using System.Text;

namespace bitcINTERFACE
{
    internal static class PasswordHasher
    {
        private const string FormatPrefix = "PBKDF2";
        private const int CurrentIterations = 100000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        internal static string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            byte[] salt = new byte[SaltSize];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = DeriveHash(password, salt, CurrentIterations);
            return string.Join("$", FormatPrefix, CurrentIterations.ToString(), Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        internal static bool VerifyPassword(string password, string storedPasswordHash, out bool needsRehash)
        {
            needsRehash = false;

            if (password == null || string.IsNullOrWhiteSpace(storedPasswordHash))
            {
                return false;
            }

            if (!storedPasswordHash.StartsWith(FormatPrefix + "$", StringComparison.Ordinal))
            {
                needsRehash = true;
                return FixedTimeEquals(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(storedPasswordHash));
            }

            string[] parts = storedPasswordHash.Split('$');
            if (parts.Length != 4 || !int.TryParse(parts[1], out int iterations))
            {
                return false;
            }

            try
            {
                byte[] salt = Convert.FromBase64String(parts[2]);
                byte[] expectedHash = Convert.FromBase64String(parts[3]);
                byte[] actualHash = DeriveHash(password, salt, iterations);

                needsRehash = iterations < CurrentIterations;
                return FixedTimeEquals(actualHash, expectedHash);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static byte[] DeriveHash(string password, byte[] salt, int iterations)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return deriveBytes.GetBytes(HashSize);
            }
        }

        private static bool FixedTimeEquals(byte[] left, byte[] right)
        {
            int difference = left.Length ^ right.Length;
            int length = Math.Min(left.Length, right.Length);

            for (int i = 0; i < length; i++)
            {
                difference |= left[i] ^ right[i];
            }

            return difference == 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Web.Common.Helpers
{
    public static class PasswordHelper
    {
        // Generates a salt
        public static byte[] GenerateSalt(int size = 64)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[size]; // 16 bytes = 128-bit salt
                rng.GetBytes(salt);
                return salt;
            }
        }

        // Hashes a password with a given salt
        public static byte[] HashPassword(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                // Combine the password and the salt
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] passwordWithSalt = new byte[passwordBytes.Length + salt.Length];
                Buffer.BlockCopy(passwordBytes, 0, passwordWithSalt, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, passwordWithSalt, passwordBytes.Length, salt.Length);

                // Hash the combined password and salt
                return sha256.ComputeHash(passwordWithSalt);
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHashBase64, string storedSaltBase64)
        {
            byte[] storedHash = Convert.FromBase64String(storedHashBase64);
            byte[] storedSalt = Convert.FromBase64String(storedSaltBase64);

            byte[] enteredHash = PasswordHelper.HashPassword(enteredPassword, storedSalt);

            return CryptographicOperations.FixedTimeEquals(storedHash, enteredHash); // Prevent timing attacks
        }
    }
}

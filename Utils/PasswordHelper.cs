using System;
using System.Security.Cryptography;

namespace web_ban_hang2.Utils
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100000;

    public static string Hash(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            byte[] salt = new byte[SaltSize];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] key;

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256))
            {
                key = pbkdf2.GetBytes(KeySize);
            }

            return string.Format(
                "PBKDF2-SHA256${0}${1}${2}",
                Iterations,
                Convert.ToBase64String(salt),
                Convert.ToBase64String(key)
            );
        }


        public static bool Verify(
            string password,
            string stored)
        {
            if (string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(stored))
            {
                return false;
            }


            // Hỗ trợ mật khẩu cũ dạng plaintext
            if (!stored.StartsWith(
                "PBKDF2-SHA256$",
                StringComparison.Ordinal))
            {
                return password == stored;
            }


            string[] parts =
                stored.Split('$');


            if (parts.Length != 4)
                return false;


            int iterations;

            if (!int.TryParse(
                parts[1],
                out iterations))
            {
                return false;
            }


            byte[] salt;
            byte[] expected;


            try
            {
                salt =
                    Convert.FromBase64String(parts[2]);

                expected =
                    Convert.FromBase64String(parts[3]);
            }
            catch
            {
                return false;
            }


            byte[] actual;


            using (var pbkdf2 =
                new Rfc2898DeriveBytes(
                    password,
                    salt,
                    iterations,
                    HashAlgorithmName.SHA256))
            {
                actual =
                    pbkdf2.GetBytes(
                        expected.Length);
            }


            return FixedTimeEquals(
                actual,
                expected);
        }


        private static bool FixedTimeEquals(
            byte[] a,
            byte[] b)
        {
            if (a == null ||
                b == null ||
                a.Length != b.Length)
            {
                return false;
            }


            int diff = 0;


            for (int i = 0;
                 i < a.Length;
                 i++)
            {
                diff |= a[i] ^ b[i];
            }


            return diff == 0;
        }
    }

}

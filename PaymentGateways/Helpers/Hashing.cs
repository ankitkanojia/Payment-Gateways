using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PaymentGateways.Helpers
{
    public static class Hashing
    {
        public static int saltLengthLimit = 32;
        public const int HashByteSize = 20; // to match the size of the PBKDF2-HMAC-SHA-1 hash 
        public const int Pbkdf2Iterations = 999;
        public const int IterationIndex = 0;
        public const int Pbkdf2Index = 1;

        public static string HashPassword(string password, byte[] salt, string username)
        {
            try
            {
                var cryptoProvider = new RNGCryptoServiceProvider();

                var hash = GetPbkdf2Bytes(password, salt, Pbkdf2Iterations, HashByteSize, username);

                return Pbkdf2Iterations + ":" + Convert.ToBase64String(salt);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static bool ValidatePassword(string correctHash, string password, byte[] salt, string username)
        {
            try
            {
                char[] delimiter = { ':' };
                var split = correctHash.Split(delimiter);
                var iterations = Int32.Parse(split[IterationIndex]);
                var hash = Convert.FromBase64String(split[Pbkdf2Index]);

                var aspectedHash = GetPbkdf2Bytes(password, salt, iterations, hash.Length, username);

                return SlowEquals(hash, aspectedHash);
            }
            catch
            {
                return false;
            }

        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            try
            {
                var diff = (uint)a.Length ^ (uint)b.Length;
                for (int i = 0; i < a.Length && i < b.Length; i++)
                {
                    diff |= (uint)(a[i] ^ b[i]);
                }
                return diff == 0;
            }
            catch
            {
                return false;
            }
        }

        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes, string username)
        {
            try
            {
                var pbkdf2 = new Rfc2898DeriveBytes(password + username, salt);
                pbkdf2.IterationCount = iterations;
                return pbkdf2.GetBytes(outputBytes);
            }
            catch
            {
                return new byte[saltLengthLimit];
            }
        }

        #region --> Generate HASH (For PayUMoney Payment Gateway) | Suchit Khunt
        public static string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }
        #endregion

        #region --> Generate SALT Key | Add | Suchit Khunt | 29092016      

        public static byte[] Get_SALT(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];

            //Require NameSpace: using System.Security.Cryptography;
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        #endregion

        #region --> Generate HASH Using SHA512 | Add | Suchit Khunt | 29092016
        public static string Get_HASH_SHA512(string password, string username, byte[] salt)
        {
            try
            {
                //required NameSpace: using System.Text;
                //Plain Text in Byte
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(password + username);

                //Plain Text + SALT Key in Byte
                byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + salt.Length];

                for (int i = 0; i < plainTextBytes.Length; i++)
                {
                    plainTextWithSaltBytes[i] = plainTextBytes[i];
                }

                for (int i = 0; i < salt.Length; i++)
                {
                    plainTextWithSaltBytes[plainTextBytes.Length + i] = salt[i];
                }

                HashAlgorithm hash = new SHA512Managed();
                byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
                byte[] hashWithSaltBytes = new byte[hashBytes.Length + salt.Length];

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hashWithSaltBytes[i] = hashBytes[i];
                }

                for (int i = 0; i < salt.Length; i++)
                {
                    hashWithSaltBytes[hashBytes.Length + i] = salt[i];
                }

                return Convert.ToBase64String(hashWithSaltBytes);
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region --> Comapare HASH Value | Add | Suchit Khunt | 29092016
        public static bool CompareHashValue(string password, string username, string OldHASHValue, byte[] SALT)
        {
            try
            {
                string expectedHashString = Get_HASH_SHA512(password, username, SALT);

                return (OldHASHValue == expectedHashString);
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace _4._3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] key = GenerateKey();
            Console.WriteLine("Generated Key: " + Convert.ToBase64String(key));

            Console.Write("Введіть повідомлення для автентифікації: ");
            string message = Console.ReadLine();

            byte[] hash = ComputeHmacSha256(key, message);
            Console.WriteLine("Computed Hash: " + Convert.ToBase64String(hash));

            bool isMessageAuthentic = VerifyHmacSha256(key, message, hash);

            Console.WriteLine("Message Authentic: " + isMessageAuthentic);
        }

        static byte[] GenerateKey()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[32];
                rng.GetBytes(key);
                return key;
            }
        }

        static byte[] ComputeHmacSha256(byte[] key, string message)
        {
            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                byte[] messageBytes = Encoding.Unicode.GetBytes(message);
                return hmac.ComputeHash(messageBytes);
            }
        }

        static bool VerifyHmacSha256(byte[] key, string message, byte[] expectedHash)
        {
            byte[] computedHash = ComputeHmacSha256(key, message);

            return computedHash.SequenceEqual(expectedHash);
        }
    }
}

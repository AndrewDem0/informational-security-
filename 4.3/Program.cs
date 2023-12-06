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
            // Генеруємо випадковий ключ для HMAC
            byte[] key = GenerateKey();
            Console.WriteLine("Generated Key: " + Convert.ToBase64String(key));

            // Користувач вводить повідомлення для аутентифікації
            Console.Write("Введіть повідомлення для автентифікації: ");
            string message = Console.ReadLine();

            // Обчислюємо HMAC для повідомлення з використанням згенерованого ключа
            byte[] hash = ComputeHmacSha256(key, message);
            Console.WriteLine("Computed Hash: " + Convert.ToBase64String(hash));

            // Перевіряємо, чи повідомлення аутентичне, порівнюючи обчислений HMAC з очікуваним
            bool isMessageAuthentic = VerifyHmacSha256(key, message, hash);

            // Виводимо результат перевірки на консоль
            Console.WriteLine("Message Authentic: " + isMessageAuthentic);
            Console.ReadLine();
        }

        // Генерує випадковий ключ для HMAC
        static byte[] GenerateKey()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[32];
                rng.GetBytes(key);
                return key;
            }
        }

        // Обчислює HMAC-SHA256 для заданого повідомлення та ключа
        static byte[] ComputeHmacSha256(byte[] key, string message)
        {
            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                byte[] messageBytes = Encoding.Unicode.GetBytes(message);
                return hmac.ComputeHash(messageBytes);
            }
        }

        // Перевіряє, чи обчислений HMAC співпадає з очікуваним HMAC
        static bool VerifyHmacSha256(byte[] key, string message, byte[] expectedHash)
        {
            byte[] computedHash = ComputeHmacSha256(key, message);

            // Порівнюємо обчислений HMAC з очікуваним HMAC
            return computedHash.SequenceEqual(expectedHash);
        }
    }
}
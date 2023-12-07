using System;
using System.Buffers.Text;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace _5._2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Введення пароля від користувача
            Console.WriteLine("Enter password");
            string userPassword = Console.ReadLine();

            // Запускаємо цикл для виконання хешування з різною кількістю ітерацій
            for (int i = 70000; i < 70000 + (10 * 50000); i += 50000)
            {
                // Викликаємо метод для генерації хешу з використанням PBKDF2
                HashAndPrint(userPassword, i);
            }
            Console.ReadLine();
        }

        public class PBKDF2
        {
            // Метод для генерації випадкової солі
            public static byte[] GenerateSalt()
            {
                using (var randomNumberGenerator = new RNGCryptoServiceProvider())
                {
                    var saltBytes = new byte[32];
                    randomNumberGenerator.GetBytes(saltBytes);
                    return saltBytes;
                }
            }

            // Метод для хешування пароля з використанням PBKDF2
            public static byte[] HashPassword(byte[] passwordBytes, byte[] salt, int i)
            {
                using (var rfc2898 = new Rfc2898DeriveBytes(passwordBytes, salt, i))
                {
                    return rfc2898.GetBytes(20);
                }
            }
        }

        // Метод для виведення результатів хешування на консоль
        private static void HashAndPrint(string password, int i)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Генеруємо сіль, хешуємо пароль та вимірюємо час виконання
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var salt = PBKDF2.GenerateSalt();
            var hashedPassword = PBKDF2.HashPassword(passwordBytes, salt, i);

            stopwatch.Stop();

            // Виводимо результати на консоль
            Console.WriteLine();
            Console.WriteLine("Original Password: " + password);
            Console.WriteLine("Hashed Password   : " + Convert.ToBase64String(hashedPassword));
            Console.WriteLine("i <" + i + "> Elapsed Time: " + stopwatch.ElapsedMilliseconds + "ms");
        }
    }
}

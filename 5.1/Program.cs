using System;
using System.Security.Cryptography;
using System.Text;

namespace _5._1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Створюємо екземпляр класу SaltedHash для роботи з хешем та сіллю
            SaltedHash passwords = new SaltedHash();

            Console.Write("Введіть повідомлення для реалізації операції: ");
            string message = Console.ReadLine();
            // Отримуємо введений рядок та конвертуємо його в масив байтів
            byte[] password = Encoding.Unicode.GetBytes(message);

            // Викликаємо метод GetHash для отримання хешу та солі
            passwords.GetHash(password);
        }

        public class SaltedHash
        {
            private byte[] Salt; // Змінна для зберігання солі
            private byte[] SaltHashPassword; // Змінна для зберігання хешу пароля

            // Метод для отримання хешу та солі
            public void GetHash(byte[] Password)
            {
                // Створюємо генератор випадкових чисел
                var randomNumberGenerator = new RNGCryptoServiceProvider();

                // Генеруємо випадкову сіль довжиною 32 байти
                Salt = new byte[32];
                randomNumberGenerator.GetBytes(Salt);

                // Об'єднуємо сіль та пароль в один масив байтів
                var concatenatedData = new byte[Salt.Length + Password.Length];
                //Buffer.BlockCopy(Salt, 0, concatenatedData, 0, Salt.Length);
                Buffer.BlockCopy(Password, 0, concatenatedData, Salt.Length, Password.Length);

                // Використовуємо SHA256 для обчислення хешу
                using (var sha256 = SHA256.Create())
                {
                    SaltHashPassword = sha256.ComputeHash(concatenatedData);
                }

                // Виводимо сіль та хеш на консоль
                Console.WriteLine("Сіль: " + Convert.ToBase64String(Salt));
                Console.WriteLine("Хеш з сіллю: " + Convert.ToBase64String(SaltHashPassword));
            }
        }
    }
}

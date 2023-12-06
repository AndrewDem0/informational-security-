using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace _4._4
{
    internal class Program
    {
        static Dictionary<string, string> userCredentials = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            bool exit = false;

            do
            {
                Console.WriteLine("Виберіть опцію:");
                Console.WriteLine("1. Реєстрація нового користувача");
                Console.WriteLine("2. Авторизація");
                Console.WriteLine("3. Вихід");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterUser();
                        break;

                    case "2":
                        AuthenticateUser();
                        break;

                    case "3":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Невірний вибір. Будь ласка, спробуйте ще раз.");
                        break;
                }

            } while (!exit);
        }

        static void RegisterUser()
        {
            Console.Write("Введіть логін: ");
            string username = Console.ReadLine();

            if (userCredentials.ContainsKey(username))
            {
                Console.WriteLine("Користувач з таким логіном вже існує.");
                return;
            }

            Console.Write("Введіть пароль: ");
            string password = Console.ReadLine();

            // Зберігаємо хеш пароля
            string hashedPassword = ComputeHashSHA256(Encoding.UTF8.GetBytes(password));
            userCredentials.Add(username, hashedPassword);

            Console.WriteLine("Користувач успішно зареєстрований.");
        }

        static void AuthenticateUser()
        {
            Console.Write("Введіть логін: ");
            string username = Console.ReadLine();

            if (!userCredentials.ContainsKey(username))
            {
                Console.WriteLine("Користувача з таким логіном не знайдено.");
                return;
            }

            Console.Write("Введіть пароль: ");
            string password = Console.ReadLine();

            // Перевіряємо введений пароль із збереженим хешем
            string storedHash = userCredentials[username];
            string inputHash = ComputeHashSHA256(Encoding.UTF8.GetBytes(password));

            if (storedHash == inputHash)
            {
                Console.WriteLine("Авторизація успішна. Вхід розблоковано.");
            }
            else
            {
                Console.WriteLine("Невірний пароль. Спробуйте ще раз.");
            }
        }

        static string ComputeHashSHA256(byte[] toBeHashed)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(toBeHashed);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}

using System.Security.Cryptography;
using System.Text;
using System;
using System.Linq;

namespace _4._2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string MD5Hash = "po1MVkAE7IjUUwu61XxgNg==";//розшифрований пароль 20192020
            byte[] targetHashBytes = Convert.FromBase64String(MD5Hash);
            Console.WriteLine("Хеш - " + MD5Hash);

            for (int i = 0; i < 100000000; i++)
            {
                string el = i.ToString();

                byte[] dataToHash = Encoding.Unicode.GetBytes(el);

                byte[] currentHashBytes = ComputeHashMd5(dataToHash);

                if (currentHashBytes.SequenceEqual(targetHashBytes))
                {
                    Console.WriteLine("Ваш пароль : " + i);
                    break;
                }
            }
            Console.ReadLine();
        }
        static byte[] ComputeHashMd5(byte[] dataForHash)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(dataForHash);
            }
        }
    }
}
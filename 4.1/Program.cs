using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _4._1
{
    internal class Program
    {
        static void Main()
        {
            string inputData = "Hello World!"; // Замініть це на ваші дані
            string inputData1 = "Hello world!"; // Замініть це на ваші дані

            Console.WriteLine("MD5 Hash: " + Convert.ToBase64String(ComputeHashMd5(Encoding.UTF8.GetBytes(inputData))));
            Console.WriteLine("SHA-256 Hash: " + Convert.ToBase64String(ComputeHashSha256(Encoding.UTF8.GetBytes(inputData))));
            Console.WriteLine("SHA-512 Hash: " + Convert.ToBase64String(ComputeHashSha512(Encoding.UTF8.GetBytes(inputData))));
            Console.WriteLine("AFTER CHANGE:   Hello World!      to     Hello world!");
            Console.WriteLine("MD5 Hash: " + Convert.ToBase64String(ComputeHashMd5(Encoding.UTF8.GetBytes(inputData1))));
            Console.WriteLine("SHA-256 Hash: " + Convert.ToBase64String(ComputeHashSha256(Encoding.UTF8.GetBytes(inputData1))));
            Console.WriteLine("SHA-512 Hash: " + Convert.ToBase64String(ComputeHashSha512(Encoding.UTF8.GetBytes(inputData1))));

            Console.ReadLine(); // Для того, щоб консольне вікно не закрилося відразу
        }

        static byte[] ComputeHashMd5(byte[] toBeHashed)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(toBeHashed);
            }
        }

        static byte[] ComputeHashSha256(byte[] toBeHashed)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(toBeHashed);
            }
        }
        static byte[] ComputeHashSha512(byte[] toBeHashed)
        {
            using (var sha256 = SHA512.Create())
            {
                return sha256.ComputeHash(toBeHashed);
            }
        }
    }
}

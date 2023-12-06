using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _4._2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string MD5Hash = "po1MVkAE7IjUUwu61XxgNg==";
            byte[] targetHashBytes = Convert.FromBase64String(MD5Hash);
            Console.WriteLine("Хеш - " + MD5Hash);

            for (int i = 0; i <= 10000000; i++)
            {
                string el = i.ToString();

                byte[] dataToHash = Encoding.Unicode.GetBytes(el);

                byte[] currentHashBytes = ComputeHashMd5(dataToHash);

                if (currentHashBytes.SequenceEqual(targetHashBytes))
                {
                    Console.WriteLine("Ваш пароль : " + i );
                    break;
                }
            }
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

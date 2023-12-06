using System.Security.Cryptography;

namespace Lab1._2
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 20; i++)
            {
                string randomNumber = Convert.ToBase64String(RandomGenerator.GenerateRandomNumber(32));
                Console.WriteLine(randomNumber);
            }
            Console.ReadLine();
        }
    }

    public class RandomGenerator
    {
        public static byte[] GenerateRandomNumber(int length)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[length];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }
    }
}

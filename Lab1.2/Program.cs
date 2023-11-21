using System.Security.Cryptography;

namespace Lab1._2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                var rndNumberGenerator = new RNGCryptoServiceProvider();
                var randomNumber = new byte[32];
                rndNumberGenerator.GetBytes(randomNumber);
                var ConvertedResult = Convert.ToBase64String(randomNumber);

                Console.WriteLine(ConvertedResult);
            }

        }
    }
}


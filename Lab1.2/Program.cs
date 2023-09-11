using System;
using System.Security.Cryptography;
using System.Text;

public class RandomNumber
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

class Program
{
    static void Main(string[] args)
    {
        for (int i = 0; i < 10; i++)
        {
            byte[] randomBytes = RandomNumber.GenerateRandomNumber(32);
            string randomNumber = Convert.ToBase64String(randomBytes);
            Console.WriteLine(randomNumber);
        }

        Console.ReadLine();
    }
}
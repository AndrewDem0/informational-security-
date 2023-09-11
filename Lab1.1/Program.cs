using System;

namespace Lab1._1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Random random1 = new Random(1235);

            for (int i = 0; i < 10; i++)
            {
                int rnd1 = random1.Next(0, 10);

                Console.WriteLine(rnd1);
            }
        }
    }
}
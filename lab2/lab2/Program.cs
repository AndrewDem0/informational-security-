using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // Зчитування вмісту файлу "input.txt"
        byte[] fileData = File.ReadAllBytes("input.txt");

        // Генерування ключа (ключ повинен бути випадковим та тієї ж довжини, що і файл)
        Random random = new Random();
        byte[] key = new byte[fileData.Length];
        random.NextBytes(key);

        // Шифрування даних і запис зашифрованих даних у файл "encrypted.dat"
        byte[] encryptedData = new byte[fileData.Length];
        for (int i = 0; i < fileData.Length; i++)
        {
            encryptedData[i] = (byte)(fileData[i] ^ key[i]);
        }
        File.WriteAllBytes("encrypted.dat", encryptedData);

        // Розшифрування зашифрованих даних
        byte[] decryptedData = new byte[encryptedData.Length];
        for (int i = 0; i < encryptedData.Length; i++)
        {
            decryptedData[i] = (byte)(encryptedData[i] ^ key[i]);
        }
        File.WriteAllBytes("decrypted.txt", decryptedData);

        // Зчитування зашифрованого файлу "encrypted.dat"
        byte[] encryptedData1 = File.ReadAllBytes("encrypted.dat");

        // Шукаємо правильний ключ методом перебору
        byte[] bruteForceDecryptedData = new byte[encryptedData1.Length];
        byte[] key1 = Encoding.UTF8.GetBytes("Mit21"); // Апріорна інформація про ключ

        for (int i = 0; i < encryptedData1.Length; i++)
        {
            bruteForceDecryptedData[i] = (byte)(encryptedData1[i] ^ key[i % key.Length]);
        }

        // Зберігаємо результат дешифрування у файл "brute_force_decrypted.txt"
        File.WriteAllBytes("brute_force_decrypted.txt", bruteForceDecryptedData);

        Console.WriteLine("Знайдено правильний ключ (методом перебору) можете переглянути результат ");
    }
}
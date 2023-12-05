using System;
using System.IO;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        // Зчитування вмісту файлу "input.txt"
        byte[] fileData = File.ReadAllBytes("input.txt");

        // Генерація криптографічно стійкого ключа та зберігання його у файлі
        byte[] key = GenerateCryptographicallySecureKey(fileData.Length, "key.bin");

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

        Console.WriteLine("Знайдено правильний ключ. Результат збережено в decrypted.txt");
    }

    static byte[] GenerateCryptographicallySecureKey(int length, string keyFilePath)
    {
        using (var random = new RNGCryptoServiceProvider())
        {
            byte[] key = new byte[length];
            random.GetBytes(key);

            // Збереження ключа у файлі
            File.WriteAllBytes(keyFilePath, key);

            return key;
        }
    }
}

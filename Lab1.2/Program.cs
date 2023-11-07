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
        byte[] key = new byte[8];
        random.NextBytes(key);

        // Шифрування даних і запис зашифрованих даних у файл "encrypted.dat"
        byte[] encryptedData = new byte[8];
        for (int i = 0; i < 8; i++)
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

        // Дешифрування методом "грубої сили" та запис у файл "brute_force_decrypted.txt"
        string bruteForceDecryptedText = null;


        for (byte bruteForceKey = 0; bruteForceKey <= 255; bruteForceKey++)
        {
            byte[] bruteForceDecryptedData = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                bruteForceDecryptedData[i] = (byte)(encryptedData[i] ^ bruteForceKey);
            }

            string candidateText = Encoding.UTF8.GetString(bruteForceDecryptedData);
            if (candidateText.Length == 8 && candidateText.StartsWith("Mit21"))
            {
                bruteForceDecryptedText = candidateText;
                break; // Завершуємо, якщо знайдено відповідний ключ
            }
        }

        if (bruteForceDecryptedText != null)
        {
            File.WriteAllText("brute_force_decrypted.txt", bruteForceDecryptedText);
            Console.WriteLine("Знайдено правильний ключ (методом грубої сили)");
        }
        else
        {
            Console.WriteLine("Не вдалося знайти правильний ключ методом грубої сили.");
        }
    }
}

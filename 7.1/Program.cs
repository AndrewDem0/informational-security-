using System.Security.Cryptography;
using System.Text;
using System;

class Program
{
    private static RSAParameters publicKey;
    private static RSAParameters privateKey;

    static void Main()
    {
        // Генеруємо та виводимо в консоль пару ключів
        GenerateKeys();
        Console.WriteLine("RSA Key Pair Generated...");
        Console.WriteLine();

        string originalData = "Hello, RSA Encryption!";
        Console.WriteLine("Message to encrypt: " + originalData);
        Console.WriteLine();

        byte[] encryptedData = EncryptData(Encoding.UTF8.GetBytes(originalData));
        Console.WriteLine("Encrypted Data: " + Convert.ToBase64String(encryptedData));
        Console.WriteLine();

        byte[] decryptedData = DecryptData(encryptedData);
        string decryptedString = Encoding.UTF8.GetString(decryptedData);
        Console.WriteLine("Decrypted Data: " + decryptedString);

        Console.ReadKey();
    }

    // Генерація пари ключів
    static void GenerateKeys()
    {
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            rsa.PersistKeyInCsp = false;
            publicKey = rsa.ExportParameters(false); // Експортуємо публічний ключ
            privateKey = rsa.ExportParameters(true); // Експортуємо приватний ключ
        }
    }

    // Шифрування даних
    static byte[] EncryptData(byte[] dataToEncrypt)
    {
        byte[] cipherbytes;
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.PersistKeyInCsp = false; // Вимкнення збереження ключів в CSP
            rsa.ImportParameters(publicKey); // Імпортуємо публічний ключ
            cipherbytes = rsa.Encrypt(dataToEncrypt, true); // Шифруємо дані
        }
        return cipherbytes;
    }

    // Розшифрування даних
    static byte[] DecryptData(byte[] dataToDecrypt)
    {
        byte[] plain;
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.PersistKeyInCsp = false; // Вимкнення збереження ключів в CSP
            rsa.ImportParameters(privateKey); // Імпортуємо приватний ключ
            plain = rsa.Decrypt(dataToDecrypt, true); // Розшифровуємо дані
        }
        return plain;
    }
}
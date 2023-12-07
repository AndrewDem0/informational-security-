using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    private const string PublicKeyFileName = "publicKey.xml";
    private const string EncryptedMessageFileName = "encryptedMessage.txt";

    private static RSAParameters _publicKey;

    static void Main()
    {
        // Завантажити або згенерувати відкритий ключ
        LoadOrGeneratePublicKey();

        // Зберегти відкритий ключ у файл
        SavePublicKeyToFile();

        // Зашифрувати повідомлення
        EncryptMessage("Hello, World!");

        Console.WriteLine("Program executed successfully.");
    }

    private static void LoadOrGeneratePublicKey()
    {
        // Перевірити, чи існує файл із відкритим ключем
        if (File.Exists(PublicKeyFileName))
        {
            // Якщо файл існує, завантажити відкритий ключ з файлу
            _publicKey = LoadPublicKeyFromFile();
        }
        else
        {
            // Якщо файл не існує, згенерувати новий відкритий ключ
            using (var rsa = new RSACryptoServiceProvider())
            {
                _publicKey = rsa.ExportParameters(false);
            }
        }
    }

    private static RSAParameters LoadPublicKeyFromFile()
    {
        // Завантажити відкритий ключ із файлу
        using (var reader = new StreamReader(PublicKeyFileName))
        {
            var xmlString = reader.ReadToEnd();
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlString);
            return rsa.ExportParameters(false);
        }
    }

    private static void SavePublicKeyToFile()
    {
        // Зберегти відкритий ключ у файл
        using (var writer = new StreamWriter(PublicKeyFileName))
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(_publicKey);
            var xmlString = rsa.ToXmlString(false);
            writer.Write(xmlString);
        }
    }

    private static void EncryptMessage(string message)
    {
        // Зашифрувати повідомлення
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(_publicKey);
            var encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(message), false);

            // Зберегти зашифроване повідомлення у файл
            File.WriteAllBytes(EncryptedMessageFileName, encryptedBytes);
        }
    }
}

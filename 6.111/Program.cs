using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        const string original = "hello world";

        var symmetricEncryption = new SymmetricEncryption();

        // DES Encryption
        var des = new DESCryptoServiceProvider();
        var desKey = symmetricEncryption.GenerateRandomNumber(8);
        var desIv = symmetricEncryption.GenerateRandomNumber(8);
        var desEncrypted = symmetricEncryption.Encrypt(Encoding.UTF8.GetBytes(original), desKey, desIv, des);
        var desDecrypted = symmetricEncryption.Decrypt(desEncrypted, desKey, desIv, des);
        var desDecryptedMessage = Encoding.UTF8.GetString(desDecrypted);

        // Triple DES Encryption
        var tripleDes = new TripleDESCryptoServiceProvider();
        var tripleDesKey = symmetricEncryption.GenerateRandomNumber(16);
        var tripleDesIv = symmetricEncryption.GenerateRandomNumber(8);
        var tripleDesEncrypted = symmetricEncryption.Encrypt(Encoding.UTF8.GetBytes(original), tripleDesKey, tripleDesIv, tripleDes);
        var tripleDesDecrypted = symmetricEncryption.Decrypt(tripleDesEncrypted, tripleDesKey, tripleDesIv, tripleDes);
        var tripleDesDecryptedMessage = Encoding.UTF8.GetString(tripleDesDecrypted);

        // AES Encryption
        var aes = new AesCryptoServiceProvider();
        var aesKey = symmetricEncryption.GenerateRandomNumber(32);
        var aesIv = symmetricEncryption.GenerateRandomNumber(16);
        var aesEncrypted = symmetricEncryption.Encrypt(Encoding.UTF8.GetBytes(original), aesKey, aesIv, aes);
        var aesDecrypted = symmetricEncryption.Decrypt(aesEncrypted, aesKey, aesIv, aes);
        var aesDecryptedMessage = Encoding.UTF8.GetString(aesDecrypted);

       
        Console.WriteLine("Original Text = " + original);
        Console.WriteLine("DES Encrypted Text = " + Convert.ToBase64String(desEncrypted));
        Console.WriteLine("DES Decrypted Text = " + desDecryptedMessage);
        Console.WriteLine();
        Console.WriteLine("Triple DES Encrypted Text = " + Convert.ToBase64String(tripleDesEncrypted));
        Console.WriteLine("Triple DES Decrypted Text = " + tripleDesDecryptedMessage);
        Console.WriteLine();
        Console.WriteLine("AES Encrypted Text = " + Convert.ToBase64String(aesEncrypted));
        Console.WriteLine("AES Decrypted Text = " + aesDecryptedMessage);

        Console.ReadLine();
    }
}

// Генеруємо випадковий байтовий масив
public class SymmetricEncryption
{
    public byte[] GenerateRandomNumber(int length)
    {
        var randomNumberGenerator = new RNGCryptoServiceProvider();
        var randomNumber = new byte[length];
        randomNumberGenerator.GetBytes(randomNumber);
        return randomNumber;
    }

    // Зашифровування
    public byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv, SymmetricAlgorithm algorithm)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, algorithm.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                cryptoStream.FlushFinalBlock();
            }
            return memoryStream.ToArray();
        }
    }

    // Розшифровування
    public byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv, SymmetricAlgorithm algorithm)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, algorithm.CreateDecryptor(key, iv), CryptoStreamMode.Write))
            {
                cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                cryptoStream.FlushFinalBlock();
            }
            return memoryStream.ToArray();
        }
    }
}
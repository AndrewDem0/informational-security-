using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        const string original = "Text to encrypt";
        const string password = "YourPassword"; 

        var symmetricEncryption = new SymmetricEncryption();

        // Generate random salt
        var salt = symmetricEncryption.GenerateRandomNumber(16);

     
        int iterations = 70000; // Adjust the number of iterations based on your requirement
        int keySize = 32; // Key size for AES (256 bits)
        int blockSize = 128; // Block size for AES (16 bytes)

        var key = symmetricEncryption.DeriveKeyFromPassword(password, salt, iterations, keySize);

        // Generate a random IV with the correct size
        var iv = symmetricEncryption.GenerateRandomNumber(blockSize / 8);

        // AES Encryption
        var aes = new AesCryptoServiceProvider();
        var aesEncrypted = symmetricEncryption.Encrypt(Encoding.UTF8.GetBytes(original), key, iv, aes);
        var aesDecrypted = symmetricEncryption.Decrypt(aesEncrypted, key, iv, aes);
        var aesDecryptedMessage = Encoding.UTF8.GetString(aesDecrypted);

        // Display Results
        Console.WriteLine("Original Text = " + original);
        Console.WriteLine("AES Encrypted Text = " + Convert.ToBase64String(aesEncrypted));
        Console.WriteLine("AES Decrypted Text = " + aesDecryptedMessage);

        Console.ReadLine();
    }
}

public class SymmetricEncryption
{
    public byte[] GenerateRandomNumber(int length)
    {
        var randomNumberGenerator = new RNGCryptoServiceProvider();
        var randomNumber = new byte[length];
        randomNumberGenerator.GetBytes(randomNumber);
        return randomNumber;
    }

    public byte[] DeriveKeyFromPassword(string password, byte[] salt, int iterations, int keySize)
    {
        var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations);
        return deriveBytes.GetBytes(keySize);
    }

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
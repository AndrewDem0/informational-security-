using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    private const string publicKeyFilePath = "C:\\work prog\\інформаційна безпека\\C#\\Main\\9.1\\publicKey.xml";

    static void Main()
    {
        const string privateKeyContainerName = "RsaContainer";

        // Створення публічного і приватного ключів
        RSAParameters publicKey, privateKey;
        CreateKeyPair(privateKeyContainerName, out publicKey, out privateKey);

        // Збереження публічного ключа у XML-файл
        SavePublicKey(publicKey, publicKeyFilePath);

        // Підписання і перевірка ЕЦП
        string originalMessage = "Hello, world!";
        byte[] signature = SignData(originalMessage, privateKey);
        bool isSignatureValid = VerifySignature(originalMessage, signature, publicKey);

        // Вивід результатів
        Console.WriteLine("Original Message: " + originalMessage);
        Console.WriteLine("Signature: " + Convert.ToBase64String(signature));
        Console.WriteLine("Is Signature Valid: " + isSignatureValid);

        Console.ReadLine();
    }

    static void CreateKeyPair(string privateKeyContainerName, out RSAParameters publicKey, out RSAParameters privateKey)
    {
        var cspParams = new CspParameters
        {
            KeyContainerName = privateKeyContainerName,
            Flags = CspProviderFlags.UseMachineKeyStore
        };

        using (var rsa = new RSACryptoServiceProvider(cspParams))
        {
            rsa.PersistKeyInCsp = false; // Забезпечення того, що ключі не будуть збережені в CSP
            publicKey = rsa.ExportParameters(false);
            privateKey = rsa.ExportParameters(true);
        }
    }

    static void SavePublicKey(RSAParameters publicKey, string filePath)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(publicKey);
            File.WriteAllText(filePath, rsa.ToXmlString(false));
        }
    }

    static byte[] SignData(string data, RSAParameters privateKey)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(privateKey);
            byte[] dataToSign = Encoding.UTF8.GetBytes(data);

            using (var sha256 = SHA256.Create())
            {
                byte[] hashOfData = sha256.ComputeHash(dataToSign);

                // Використання RSAPKCS1SignatureFormatter для підпису
                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm(nameof(SHA256));
                return rsaFormatter.CreateSignature(hashOfData);
            }
        }
    }

    static bool VerifySignature(string data, byte[] signature, RSAParameters publicKey)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(publicKey);
            byte[] dataToVerify = Encoding.UTF8.GetBytes(data);

            using (var sha256 = SHA256.Create())
            {
                byte[] hashOfData = sha256.ComputeHash(dataToVerify);

                // Використання RSAPKCS1SignatureDeformatter для верифікації підпису
                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm(nameof(SHA256));
                return rsaDeformatter.VerifySignature(hashOfData, signature);
            }
        }
    }
}
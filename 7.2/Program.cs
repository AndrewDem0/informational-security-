using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        string publicKeyPath = "C:\\work prog\\інформаційна безпека\\C#\\Main\\7.2\\Demudenko_PublicKey.xml";
        string chipherTextPath = "C:\\work prog\\інформаційна безпека\\C#\\Main\\7.2\\encrypteddata.txt"; // 0-Anton 1-Max 
        string friendPublicKeyPath = "C:\\work prog\\інформаційна безпека\\C#\\Main\\7.2\\Dashkovskiy_RSAPublicKey.xml"; //Dashkovskiy_RSAPublicKey
        string friendsEncryptedMessagePath = "C:\\work prog\\інформаційна безпека\\C#\\Main\\7.2\\encryptedmessage.txt"; //0-Anton 1-Max 


        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Виберіть опцію:");
            Console.WriteLine("a. Шифрування повідомлення своїм ключем");
            Console.WriteLine("b. Шифрування повідомлення чужим ключем");
            Console.WriteLine("c. Розшифрування свого повідомлення приватним ключем");
            Console.WriteLine("d. Розшифрування повідомлення від друга приватним ключем");
            Console.WriteLine("f. введіть якщо ваші ключі ще не створені");
            Console.WriteLine("e. Вихід");

            Console.Write("Ваш вибір..: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "a":
                    Console.Write("ВВедіть повідомлення для шифрування: ");
                    string messageToEncrypt = Console.ReadLine();
                    byte[] dataToEncrypt = Encoding.UTF8.GetBytes(messageToEncrypt);
                    RSAHelper.EncryptData(publicKeyPath, dataToEncrypt, chipherTextPath);
                    Console.WriteLine("Done.");
                    break;
                case "b":
                    Console.Write("ВВедіть повідомлення дра друга: ");
                    string messageToEncryptForFriend = Console.ReadLine();
                    byte[] dataToEncryptForFriend = Encoding.UTF8.GetBytes(messageToEncryptForFriend);
                    RSAHelper.EncryptData(friendPublicKeyPath, dataToEncryptForFriend, chipherTextPath);
                    Console.WriteLine("Done.");
                    break;
                case "c":
                    RSAHelper.DecryptAndPrintMessage(chipherTextPath);
                    break;
                case "d":
                    RSAHelper.DecryptAndPrintMessage(friendsEncryptedMessagePath);

                    break;
                case "e":
                    Environment.Exit(0);
                    break;
                case "f":
                    RSAHelper.GenerateOwnKeys(publicKeyPath);
                    Console.WriteLine();
                    Console.WriteLine("Ваші ключі вже згенеровані програмою");
                    break;
                default:
                    break;
            }
        }
    }
}

public class RSAHelper
{
    private readonly static string CspContainerName = "RsaContainer";

    // Метод для генерації власних ключів та збереження публічного ключа у файл
    public static void GenerateOwnKeys(string publicKeyPath)
    {
        CspParameters cspParameters = new CspParameters(1)
        {
            KeyContainerName = CspContainerName,
            Flags = CspProviderFlags.UseMachineKeyStore,
            ProviderName = "Microsoft Strong Cryptographic Provider",
        };

        using (var rsa = new RSACryptoServiceProvider(2048, cspParameters))
        {
            rsa.PersistKeyInCsp = true;
            File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
        }
    }

    // Метод для шифрування даних та збереження результату у файл
    public static void EncryptData(string publicKeyPath, byte[] dataToEncrypt, string chipherTextPath)
    {
        byte[] cipherBytes;
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            rsa.PersistKeyInCsp = false;
            rsa.FromXmlString(File.ReadAllText(publicKeyPath));
            cipherBytes = rsa.Encrypt(dataToEncrypt, true);
        }
        File.WriteAllBytes(chipherTextPath, cipherBytes);
    }


    // Метод для розшифрування зашифрованих даних
    public static byte[] DecryptData(string chipherTextPath)
    {
        // Зчитування зашифрованих даних з файлу
        byte[] cipherBytes = File.ReadAllBytes(chipherTextPath);

        // Буфер для зберігання розшифрованих даних
        byte[] plainTextBytes;

        // Параметри криптопровайдера
        var cspParams = new CspParameters
        {
            // Вказане ім'я контейнера ключів
            KeyContainerName = CspContainerName,
            // Вказані флаги для використання машинного сховища ключів
            Flags = CspProviderFlags.UseMachineKeyStore
        };

        // Використання криптопровайдера для розшифрування даних
        using (var rsa = new RSACryptoServiceProvider(2048, cspParams))
        {
            // Встановлення прапорця для збереження ключа в криптопровайдері
            rsa.PersistKeyInCsp = true;

            // Розшифрування зашифрованих даних
            plainTextBytes = rsa.Decrypt(cipherBytes, true);
        }

        // Повернення розшифрованих даних
        return plainTextBytes;
    }

    // Метод для розшифрування та виведення розшифрованого повідомлення
    public static void DecryptAndPrintMessage(string chipherTextPath)
    {
        byte[] decryptedData = DecryptData(chipherTextPath);
        string decryptedMessage = Encoding.UTF8.GetString(decryptedData);
        Console.WriteLine("Розшифровано: " + decryptedMessage);
    }

    // Метод для видалення ключів з криптопровайдера
    public static void DeleteKeyInCsp()
    {
        CspParameters cspParameters = new CspParameters
        {
            KeyContainerName = CspContainerName,
            Flags = CspProviderFlags.UseMachineKeyStore
        };

        var rsa = new RSACryptoServiceProvider(cspParameters)
        {
            PersistKeyInCsp = false
        };
        rsa.Clear();
    }
}

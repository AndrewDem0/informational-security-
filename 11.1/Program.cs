using System;
using System.Collections.Generic;
using System.Security;

public class User
{
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public string[] Roles { get; set; }
}

public class Protector
{
    private static Dictionary<string, User> _users = new Dictionary<string, User>();
    private static User _loggedInUser = null;

    public void Run()
    {
        int choice;
        do
        {
            ShowMainMenu();
            choice = GetUserChoice();

            switch (choice)
            {
                case 1:
                    RegisterUser();
                    break;
                case 2:
                    RegisterAdmin();
                    break;
                case 3:
                    LogIn();
                    break;
                case 4:
                    ShowAllUsers();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Невірний вибір. Будь ласка, виберіть знову.");
                    break;
            }

        } while (choice != 5);
    }

    private void ShowMainMenu()
    {
        Console.WriteLine("Головне меню:");
        Console.WriteLine("1. Додати User");
        Console.WriteLine("2. Додати Admin");
        Console.WriteLine("3. Ввійти");
        Console.WriteLine("4. Показати всіх користувачів");
        Console.WriteLine("5. Вийти з програми");
    }

    private void ShowLoggedInMenu()
    {
        Console.WriteLine("Меню після авторизації:");
        Console.WriteLine("1. Доступно для User та Admin");
        Console.WriteLine("2. Доступно лише для Admin");
        Console.WriteLine("3. Вийти в головне меню");
    }

    private int GetUserChoice()
    {
        Console.Write("Введіть номер вашого вибору: ");
        return int.Parse(Console.ReadLine());
    }

    private void RegisterUser()
    {
        Console.Write("Введіть логін для нового користувача: ");
        string login = Console.ReadLine();
        Console.Write("Введіть пароль для нового користувача: ");
        string password = Console.ReadLine();

        try
        {
            Register(login, password, new string[] { "User" });
            Console.WriteLine("Користувач успішно доданий.");
        }
        catch (SecurityException ex)
        {
            Console.WriteLine($"{ex.GetType()}: {ex.Message}");
        }
    }

    private void RegisterAdmin()
    {
        if (_users.Values.Count(user => Array.Exists(user.Roles, role => role == "Admin")) >= 1)
        {
            Console.WriteLine("Адміністратор вже існує. Тільки один адміністратор допускається.");
            return;
        }

        Console.Write("Введіть логін для нового адміністратора: ");
        string login = Console.ReadLine();
        Console.Write("Введіть пароль для нового адміністратора: ");
        string password = Console.ReadLine();

        try
        {
            Register(login, password, new string[] { "Admin" });
            Console.WriteLine("Адміністратор успішно доданий.");
        }
        catch (SecurityException ex)
        {
            Console.WriteLine($"{ex.GetType()}: {ex.Message}");
        }
    }

    private void ShowAllUsers()
    {
        Console.WriteLine("Всі користувачі:");
        foreach (var user in _users.Values)
        {
            Console.WriteLine($"Логін: {user.Login}, Роль: {string.Join(", ", user.Roles)}");
        }
    }

    private void LogIn()
    {
        Console.Write("Введіть логін: ");
        string login = Console.ReadLine();
        Console.Write("Введіть пароль: ");
        string password = Console.ReadLine();

        try
        {
            LogIn(login, password);
            Console.WriteLine("Авторизація пройшла успішно.");

            int choice;
            do
            {
                ShowLoggedInMenu();
                choice = GetUserChoice();

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Цей функціонал доступний для User та Admin.");
                        break;
                    case 2:
                        if (_loggedInUser.Roles != null && Array.Exists(_loggedInUser.Roles, role => role == "Admin"))
                        {
                            Console.WriteLine("Цей функціонал доступний лише для Admin.");
                        }
                        else
                        {
                            Console.WriteLine("У вас недостатньо прав для цієї опції.");
                        }
                        break;
                    case 3:
                        Console.WriteLine("Вихід в головне меню та розавторизація.");
                        _loggedInUser = null;
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Будь ласка, виберіть знову.");
                        break;
                }

            } while (choice != 3);

        }
        catch (SecurityException ex)
        {
            Console.WriteLine($"{ex.GetType()}: {ex.Message}");
        }
    }

    private void Register(string userName, string password, string[] roles = null)
    {
        if (_users.Count >= 4)
        {
            throw new SecurityException("Досягнуто максимальну кількість користувачів.");
        }

        if (_users.ContainsKey(userName))
        {
            throw new SecurityException("Користувач з таким іменем вже зареєстрований.");
        }

        var salt = GenerateSalt();
        var passwordHash = ComputeHash(password, salt);

        var user = new User
        {
            Login = userName,
            PasswordHash = passwordHash,
            Salt = salt,
            Roles = roles
        };

        _users.Add(userName, user);
    }

    private bool CheckPassword(string userName, string password)
    {
        if (!_users.ContainsKey(userName))
        {
            throw new SecurityException("Користувача з таким іменем не існує.");
        }

        var user = _users[userName];
        var hashedPassword = ComputeHash(password, user.Salt);

        return hashedPassword == user.PasswordHash;
    }

    private void LogIn(string userName, string password)
    {
        if (_loggedInUser != null)
        {
            throw new SecurityException("Ви вже авторизовані. Вийдіть перед авторизацією нового користувача.");
        }

        if (!CheckPassword(userName, password))
        {
            throw new SecurityException("Невірний логін або пароль.");
        }

        var identity = new System.Security.Principal.GenericIdentity(userName, "OIBAuth");
        var principal = new System.Security.Principal.GenericPrincipal(identity, _users[userName].Roles);

        System.Threading.Thread.CurrentPrincipal = principal;
        _loggedInUser = _users[userName];
    }

    private string ComputeHash(string input, string salt)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var saltedInput = input + salt;
            var hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(saltedInput));
            return Convert.ToBase64String(hash);
        }
    }

    private string GenerateSalt()
    {
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            var saltBytes = new byte[32]; // 32 bytes for a 256-bit key
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
    }
}

class Program
{
    static void Main()
    {
        Protector protector = new Protector();
        protector.Run();
    }
}

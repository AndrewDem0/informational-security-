using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Principal;
using System.Threading;

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

    public User Register(string userName, string password, string[] roles = null)
    {
        if (_users.Count >= 4)
        {
            throw new SecurityException("Maximum number of users reached.");
        }

        if (_users.ContainsKey(userName))
        {
            throw new SecurityException("User with this name already registered.");
        }

        var salt = GenerateSalt();
        var passwordHash = ComputeHash(password, salt);

        var user = new User
        {
            Login = userName,
            PasswordHash = passwordHash,
            Salt = salt,
            Roles = roles ?? new string[0]
        };

        _users.Add(userName, user);
        return user;
    }

    public bool CheckPassword(string userName, string password)
    {
        if (!_users.ContainsKey(userName))
        {
            throw new SecurityException("User with this name is not registered.");
        }

        var user = _users[userName];
        var passwordHash = ComputeHash(password, user.Salt);

        return user.PasswordHash == passwordHash;
    }

    public void LogIn(string userName, string password)
    {
        if (CheckPassword(userName, password))
        {
            var identity = new GenericIdentity(userName, "OIBAuth");
            var principal = new GenericPrincipal(identity, _users[userName].Roles);
            Thread.CurrentPrincipal = principal;
        }
    }

    public void OnlyForAdminsFeature()
    {
        if (Thread.CurrentPrincipal == null)
        {
            throw new SecurityException("Thread.CurrentPrincipal cannot be null.");
        }

        if (!Thread.CurrentPrincipal.IsInRole("Admins"))
        {
            throw new SecurityException("User must be a member of Admins to access this feature.");
        }

        Console.WriteLine("You have access to this secure feature.");
    }

    private string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }

    private string ComputeHash(string password, string salt)
    {
        using (var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000))
        {
            return Convert.ToBase64String(pbkdf2.GetBytes(32)); // 32 bytes for a 256-bit key
        }
    }
}

class Program
{
    static void Main()
    {
        try
        {
            Protector protector = new Protector();

            // Register users
            protector.Register("admin", "adminpassword", new string[] { "Admins" });
            protector.Register("user1", "user1password", new string[] { "User" });
            protector.Register("user2", "user2password", new string[] { "User" });
            protector.Register("user3", "user3password", new string[] { "User" });

            // Attempt login and access protected feature
            protector.LogIn("admin", "adminpassword");
            protector.OnlyForAdminsFeature();

            protector.LogIn("user1", "user1password");
            protector.OnlyForAdminsFeature(); // This should throw an exception

        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.GetType()}: {ex.Message}");
        }

        Console.ReadLine();
    }
}

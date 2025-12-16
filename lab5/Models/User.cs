namespace lab5.Models;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }

    public User(string username, string password, string fullName, string role)
    {
        Username = username;
        Password = password;
        FullName = fullName;
        Role = role;
    }

    public User()
    {
        Username = string.Empty;
        Password = string.Empty;
        FullName = string.Empty;
        Role = "User";
    }

    public bool IsAdmin()
    {
        return Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
    }

    public override string ToString()
    {
        return $"{FullName} ({Username}) - {Role}";
    }
}

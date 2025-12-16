using System.Text.Json;
using lab5.Models;

namespace lab5.Services;

public class AuthenticationService
{
    private List<User> _users;
    private User? _currentUser;

    public User? CurrentUser => _currentUser;

    public AuthenticationService()
    {
        _users = new List<User>();
        _currentUser = null;
    }

    public void LoadUsers(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Файл {filePath} не найден.");
                return;
            }

            var users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(filePath));
            _users = users ?? new List<User>();

            if (_users.Count > 0)
                Console.WriteLine($"Загружено {_users.Count} пользователей.");
            else
                Console.WriteLine("Ошибка чтения файла");      
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Все пользователи загружены");
        }
    }

    public bool Login(string username, string password)
    {
        _currentUser = _users.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
            u.Password == password);

        return _currentUser != null;
    }

    public void Logout() => _currentUser = null;

    public bool IsAuthenticated() => _currentUser != null;

    public void DisplayLoginScreen()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║              ВХОД В СИСТЕМУ УПРАВЛЕНИЯ ТАКСОПАРКОМ         ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
    }

    public bool AttemptLogin(int maxAttempts = 3)
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            DisplayLoginScreen();

            if (attempt > 0)
                Console.WriteLine($"⚠️  Неверный логин или пароль. Осталось: {maxAttempts - attempt}\n");

            Console.WriteLine("Для выхода введите 'exit' в качестве логина\n");

            var username = ReadInput("Логин: ");
            if (string.IsNullOrWhiteSpace(username))
                continue;

            if (username.Equals("exit", StringComparison.OrdinalIgnoreCase))
                return false;

            Console.Write("Пароль: ");
            var password = ReadPassword();
            Console.WriteLine();

            if (string.IsNullOrWhiteSpace(password))
                continue;

            if (Login(username, password))
            {
                ShowWelcomeMessage();
                return true;
            }
        }

        ShowAccessDenied();
        return false;
    }

    private string? ReadInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    private void ShowWelcomeMessage()
    {
        Console.WriteLine($"\n✓ Добро пожаловать, {_currentUser!.FullName}!");
        Console.WriteLine($"Роль: {_currentUser.Role}");
        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    private void ShowAccessDenied()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                   ДОСТУП ЗАБЛОКИРОВАН                      ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        Console.WriteLine("\nПревышено количество попыток входа.");
    }

    private string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[0..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        } while (key.Key != ConsoleKey.Enter);

        return password;
    }

    public void DisplayAvailableUsers()
    {
        Console.WriteLine("\n" + new string('─', 60));
        Console.WriteLine("ДОСТУПНЫЕ ПОЛЬЗОВАТЕЛИ ДЛЯ ТЕСТИРОВАНИЯ:");
        Console.WriteLine(new string('─', 60));

        foreach (var user in _users)
        {
            Console.WriteLine($"\nЛогин: {user.Username,-15} Пароль: {user.Password}");
            Console.WriteLine($"Имя:   {user.FullName,-15} Роль:   {user.Role}");
        }

        Console.WriteLine(new string('─', 60));
    }
}

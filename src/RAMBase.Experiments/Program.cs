
using System;
using System.Linq;

namespace RAMBase.Experiments
{
  public class Program
  {
    internal static void Main(string[] args) { new Program().Run(); }


    private Settings<RootModel> _settings = RAM
      .Configure<RootModel>()
      .PersistMinOps(-1)
      .PersistMinSeconds(-1);


    public void Run()
    {
      while (true) {
        Console.WriteLine("1 - Login");
        Console.WriteLine("2 - Register");

        var input = Console.ReadKey();
        switch (input.Key) {
        case ConsoleKey.D1: menuLogin(); break;
        case ConsoleKey.D2: menuRegister(); break;
        }
      }
    }


    private void menuLogin()
    {
      Console.Write("Enter username: ");
      var username = Console.ReadLine();

      Console.Write("Enter password: ");
      var passwordHash = User.HashPassword(Console.ReadLine());

      using (var ctx = _settings.CreateContext())
      using (var scope = ctx.CreateReadScope()) {
        var user = scope.Model.Users
          .FirstOrDefault(u => u.Username == username &&
            u.PasswordHash == passwordHash);

        if (user == null)
          Console.WriteLine("Unknown username/password combination.");
        else
          Console.WriteLine("Successfully logged in user: " + user.Username);
      }

      Console.ReadKey();
    }

    private void menuRegister()
    {
      Console.Write("Enter username: ");
      var username = Console.ReadLine();

      Console.Write("Enter password: ");
      var password = Console.ReadLine();

      using (var ctx = _settings.CreateContext())
      using (var scope = ctx.CreateWriteScope()) {
        var user = new User(username, password);
        scope.Model.Users.Add(user);

        Console.WriteLine("Registration successful!");
      }

      Console.ReadKey();
    }

  }
}

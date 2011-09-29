
using System;
using System.Security.Cryptography;
using System.Text;

namespace RAMBase.Experiments
{
  [Serializable]
  public class User
  {
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }

    public User(string username, string password)
    {
      Username = username;
      PasswordHash = HashPassword(password);
    }


    public static string HashPassword(string rawPassword)
    {
      var sha1 = SHA1.Create();
      var bytes = Encoding.UTF8.GetBytes(rawPassword);
      bytes = sha1.ComputeHash(bytes);

      return Convert.ToBase64String(bytes);
    }
  }
}

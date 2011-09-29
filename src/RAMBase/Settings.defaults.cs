
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace RAMBase
{
  public sealed partial class Settings<T>
  {
    private static Settings<T> _default;

    /// <summary>
    /// Gets default settings for the model type T.
    /// </summary>
    public static Settings<T> Default
    {
      get { return _default ?? (_default = new Settings<T>()); }
    }


    private Settings()
    {
      AppName = getDefaultAppName();

      Creator = Activator.CreateInstance<T>;
      Initializer = obj => { /* no-op */ };

      Serializer = defaultSerialize;
      Deserializer = defaultDeserialize;

      PersistFolder = getDefaultPersistPath(AppName);
      PersistMinOps = 1000;
      PersistMinSeconds = 2;
    }


    private string getDefaultAppName()
    {
      return "app-" + Assembly.GetEntryAssembly().GetName().Name;
    }

    private string getDefaultPersistPath(string appName)
    {
      var path = Path.Combine(Path.GetTempPath(), "RAMBase", appName);
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);

      return path;
    }


    private static void defaultSerialize(T obj, Stream stream)
    {
      var formatter = new BinaryFormatter();
      formatter.Serialize(stream, obj);
    }

    private static T defaultDeserialize(Stream stream)
    {
      var formatter = new BinaryFormatter();
      return (T)formatter.Deserialize(stream);
    }
  }
}

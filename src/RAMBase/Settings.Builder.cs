
using System;
using System.IO;

namespace RAMBase
{
  public sealed partial class Settings<T>
  {
    public sealed class Builder
    {
      private Settings<T> _settings = new Settings<T>();

      public static implicit operator Settings<T>(Builder b) { return b.Build(); }

      public Settings<T> Build() { return _settings; }


      public Builder AppNamed(string appName)
      {
        _settings.AppName = appName;
        return this;
      }


      public Builder CreateWith(Func<T> createFunc)
      {
        _settings.Creator = createFunc;
        return this;
      }

      public Builder InitializeWith(Action<T> initAction)
      {
        _settings.Initializer = initAction;
        return this;
      }


      public Builder PersistAt(string folder)
      {
        _settings.PersistFolder = folder;
        return this;
      }

      public Builder PersistMinSeconds(int seconds)
      {
        _settings.PersistMinSeconds = seconds;
        return this;
      }

      public Builder PersistMinOps(int ops)
      {
        _settings.PersistMinOps = ops;
        return this;
      }


      public Builder SerializeWith(Action<T, Stream> serializeAction)
      {
        _settings.Serializer = serializeAction;
        return this;
      }

      public Builder DeserializeWith(Func<Stream, T> deserializeFunc)
      {
        _settings.Deserializer = deserializeFunc;
        return this;
      }
    }
  }
}

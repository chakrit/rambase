
using System;

namespace RAMBase
{
  // the main entry point for RAMBase
  public static class RAM
  {
    public static Settings<T>.Builder Configure<T>()
      where T : new()
    {
      return new Settings<T>.Builder();
    }

    public static Settings<T> Configure<T>(
      Func<Settings<T>.Builder, Settings<T>> configurator)
    {
      return configurator(new Settings<T>.Builder());
    }


    public static IContext<T> CreateContext<T>()
    {
      return Settings<T>.Default.CreateContext();
    }

    public static IContext<T> CreateContext<T>(Settings<T> settings)
    {
      return settings.CreateContext();
    }
  }
}

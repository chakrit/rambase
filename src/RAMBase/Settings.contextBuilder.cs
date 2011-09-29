
using System.Diagnostics;

namespace RAMBase
{
  public sealed partial class Settings<T>
  {
    // For building contexts within the scope of this settings instance
    private ContextBuilder<T> _contextBuilder;
    private object _contextBuilderLock = new object();


    /// <summary>
    /// Creates a new context from this settings instance scope.
    /// </summary>
    /// <returns>
    /// A new context, configured and scoped according to this settings instance.
    /// </returns>
    public IContext<T> CreateContext()
    {
      Debug.WriteLine("RAMBase: Storing persistence files at " + PersistFolder);

      if (_contextBuilder != null)
        return _contextBuilder.CreateContext();

      lock (_contextBuilderLock) {
        if (_contextBuilder == null)
          _contextBuilder = new ContextBuilder<T>(this);

        return CreateContext();
      }
    }
  }
}

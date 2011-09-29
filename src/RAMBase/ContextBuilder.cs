
namespace RAMBase
{
  // NOTE: This is required for maintaining a single instance of each classes
  //   that should only have one instance per settings scope. That is relying
  //   on ContextBuilder<T> constructor being called only once when
  //   the Settings<T> instance is built
  internal sealed class ContextBuilder<T>
  {
    private ModelContainer<T> _container;
    private Persistor<T> _persistor;

    public ContextBuilder(Settings<T> settings)
    {
      _container = new ModelContainer<T>();
      _persistor = new Persistor<T>(settings, _container);

      if (_persistor.IsPersistenceFileAvailable())
        _container.InitializeContainer(settings, _persistor.LoadFromPersistence());
      else
        _container.InitializeContainer(settings);

      _persistor.Start();
    }


    public IContext<T> CreateContext()
    {
      return new Context<T>(_container, _persistor);
    }
  }
}

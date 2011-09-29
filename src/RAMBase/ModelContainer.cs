
using System;
using System.Threading;

namespace RAMBase
{
  // Contains the model for its lifetime. Should only have on instance for
  // each Settings<T> instance
  internal class ModelContainer<T>
  {
    private T _model;

    private bool _modelInitialized = false;
    private ReaderWriterLockSlim _modelLock = new ReaderWriterLockSlim();


    public bool InitializeContainer(Settings<T> settings, T model)
    {
      _modelLock.EnterWriteLock();

      try {
        if (_modelInitialized) return false;

        _model = model;
        return _modelInitialized = true;
      }
      finally {
        _modelLock.ExitWriteLock();
      }
    }

    public bool InitializeContainer(Settings<T> settings)
    {
      _modelLock.EnterWriteLock();

      try {
        if (_modelInitialized) return false;
        _modelInitialized = true;

        _model = settings.Creator.Invoke();
        settings.Initializer.Invoke(_model);
        return true;
      }
      catch {
        _model = default(T);
        _modelInitialized = false;
        throw;
      }
      finally {
        _modelLock.ExitWriteLock();
      }
    }


    public IScope<T> CreateReadLockedScope()
    {
      ensureInitialized();

      _modelLock.EnterReadLock();
      return new Scope<T>(_model, () => _modelLock.ExitReadLock());
    }

    public IScope<T> CreateUpgradableReadLockedScope()
    {
      ensureInitialized();

      _modelLock.EnterUpgradeableReadLock();
      return new Scope<T>(_model, () => _modelLock.ExitUpgradeableReadLock());
    }

    public IScope<T> CreateWriteLockedScope()
    {
      ensureInitialized();

      _modelLock.EnterWriteLock();
      return new Scope<T>(_model, () => _modelLock.ExitWriteLock());
    }


    private void ensureInitialized()
    {
      if (!_modelInitialized)
        throw new InvalidOperationException("Model is not initialized.");
    }
  }
}

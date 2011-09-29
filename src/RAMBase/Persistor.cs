
using System;
using System.Threading;

namespace RAMBase
{
  internal partial class Persistor<T>
  {
    private readonly Settings<T> _settings;
    private readonly ModelContainer<T> _container;

    private readonly object _checkLock = new object();

    private int _opsCount = 0;
    private DateTime _lastRun = DateTime.MinValue;

    private bool _isActive = false;
    private Thread _bgThread;

    public Persistor(Settings<T> settings, ModelContainer<T> container)
    {
      // TODO: Add serializability check in <T>

      _settings = settings;
      _container = container;
    }


    public bool IsPersistenceFileAvailable()
    {
      return !string.IsNullOrEmpty(getPrimaryPersistenceFilename());
    }

    public T LoadFromPersistence()
    {
      return loadPrimaryFile();
    }


    public void Start()
    {
      _bgThread = _bgThread ?? new Thread(persistThread) {
        Priority = ThreadPriority.BelowNormal,
        Name = "RAMBase background persistor."
      };

      _isActive = true;
      _bgThread.Start();
    }

    public void Stop()
    {
      lock (_checkLock) {
        _isActive = false;
        Monitor.Pulse(_checkLock);
      }

      _bgThread.Join();
    }


    public void CheckAndPersist()
    {
      lock (_checkLock) {
        _opsCount += 1;

        if (_opsCount < _settings.PersistMinOps)
          return;

        if ((DateTime.Now - _lastRun).TotalSeconds < _settings.PersistMinSeconds)
          return;

        ForcePersist();
      }
    }

    public void ForcePersist()
    {
      lock (_checkLock) { // re-entrant
        _opsCount = 0;
        _lastRun = DateTime.Now;

        Monitor.Pulse(_checkLock);
      }
    }
  }
}

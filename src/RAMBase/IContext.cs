
using System;

namespace RAMBase
{
  public interface IContext<T> : IDisposable
  {
    IScope<T> CreateReadScope();
    IScope<T> CreateUpgradableReadScope();
    IScope<T> CreateWriteScope();
  }
}

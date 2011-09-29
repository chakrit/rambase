
using System;
using System.Collections.Generic;

namespace RAMBase.Experiments
{
  [Serializable]
  public class RootModel
  {
    public IList<User> Users { get; private set; }

    public RootModel() { Users = new List<User>(); }
  }
}

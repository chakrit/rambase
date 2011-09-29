
using System;
using System.IO;

namespace RAMBase
{
  public sealed partial class Settings<T>
  {
    /// <summary>
    /// Gets the name of the application which is used to uniquely identify
    /// each persistence file.
    /// </summary>
    public string AppName { get; private set; }


    /// <summary>
    /// Gets the function used to create the model.
    /// </summary>
    public Func<T> Creator { get; private set; }

    /// <summary>
    /// Gets the function used to initialize the model after creation.
    /// </summary>
    public Action<T> Initializer { get; private set; }


    /// <summary>
    /// Gets the base folder to use for storing persistence files.
    /// </summary>
    public string PersistFolder { get; private set; }

    /// <summary>
    /// Gets the minimum pause time in seconds required between each
    /// persistence runs to prevent disk abuse.
    /// </summary>
    /// <remarks>
    /// A value of -1 disables this check and activates persistence after
    /// every operation or after PersistMinOps operations has completed.
    /// </remarks>
    public int PersistMinSeconds { get; private set; }

    /// <summary>
    /// Gets the minimum number of completed operations required between each
    /// persistence runs to prevent disk abuse.
    /// </summary>
    /// <remarks>
    /// A value of -1 disables this check and activates persistence after
    /// every operation or after PersistMinSeconds has elapsed.
    /// </remarks>
    public int PersistMinOps { get; private set; }


    /// <summary>
    /// Gets the method to use to serialize the model to a persistence stream.
    /// </summary>
    public Action<T, Stream> Serializer { get; private set; }

    /// <summary>
    /// Gets the method to use to deserialize the model from a persistence stream.
    /// </summary>
    public Func<Stream, T> Deserializer { get; private set; }
  }
}

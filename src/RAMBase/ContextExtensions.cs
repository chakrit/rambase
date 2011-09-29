
using System;

namespace RAMBase
{
  public static class ContextExtensions
  {
    /// <summary>
    /// Performs a read operation against the model.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="readFunc">
    /// The function to execute against the model to obtain your desired result
    /// without modifying the model.
    /// </param>
    /// <returns>
    /// Result obtained from execution the supplied function against the model.
    /// </returns>
    /// <remarks>
    /// This function should never store the model instance reference as this
    /// may cause data inconsistency when accessing from multiple threads.
    /// </remarks>
    public static TResult Read<T, TResult>(this IContext<T> ctx,
      Func<T, TResult> readFunc)
    {
      using (var scope = ctx.CreateReadScope())
        return readFunc(scope.Model);
    }

    /// <summary>
    /// Performs a read operation against the model.
    /// </summary>
    /// <param name="readAction">
    /// The function to execute against the model without modifying it.
    /// </param>
    /// <returns>
    /// Result obtained from execution the supplied function against the model.
    /// </returns>
    /// <remarks>
    /// This function should never store the model instance reference as this
    /// may cause data inconsistency when accessing from multiple threads.
    /// </remarks>
    public static void Read<T>(this IContext<T> ctx, Action<T> readAction)
    {
      using (var scope = ctx.CreateReadScope())
        readAction(scope.Model);
    }

    /// <summary>
    /// Performs a write operation against the model.
    /// </summary>
    /// <param name="readAction">
    /// The function to execute to modify the model.
    /// </param>
    /// <remarks>
    /// This function should never store the model instance reference as this
    /// may cause data inconsistency when accessing from multiple threads.
    /// </remarks>
    public static void Write<T>(this IContext<T> ctx, Action<T> writeAction)
    {
      using (var scope = ctx.CreateWriteScope())
        writeAction(scope.Model);
    }
  }
}

namespace Schierle.Documentor.Utils
{
  using System.Collections.Generic;

  /// <summary>Provides extension methods for enumerables.</summary>
  internal static class EnumerableExtensions
  {
    #region Public Methods and Operators

    /// <summary>Creates an <see cref="IEnumerable{T}" /> that contains a provided object.</summary>
    /// <typeparam name="T">The type of the provided object.</typeparam>
    /// <param name="obj">The object that should be included in the <see cref="IEnumerable{T}" />.</param>
    /// <returns>An <see cref="IEnumerable{T}" /> that contains the provided object.</returns>
    public static IEnumerable<T> Yield<T>(this T obj)
    {
      yield return obj;
    }

    #endregion
  }
}

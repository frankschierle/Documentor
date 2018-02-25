namespace Schierle.Documentor.CodeActions
{
  using System;

  using Microsoft.CodeAnalysis;

  /// <summary>Encapsulates all required information to navigate to a specific
  /// position inside a solution.</summary>
  internal class NavigationTarget
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="NavigationTarget"/> class.</summary>
    /// <param name="documentId">The id of the document to navigate to.</param>
    /// <param name="position">The position inside the target document to navigate to.</param>
    /// <exception cref="ArgumentNullException">Occurs if <paramref name="documentId"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs if <paramref name="position"/> is less than zero (0).</exception>
    public NavigationTarget(DocumentId documentId, int position)
    {
      if (documentId == null)
      {
        throw new ArgumentNullException(nameof(documentId));
      }

      if (position < 0)
      {
        throw new ArgumentOutOfRangeException(nameof(position));
      }

      this.DocumentId = documentId;
      this.Position = position;
    }

    #endregion

    #region Public Properties

    /// <summary>Gets the id of the document to navigate to.</summary>
    public DocumentId DocumentId { get; }

    /// <summary>Gets the position inside the target document to navigate to.</summary>
    public int Position { get; }

    #endregion
  }
}

namespace Schierle.Documentor.Utils
{
  using System;
  using System.Linq;

  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.CSharp.Syntax;

  /// <summary>Provides helper methods to deal with source code documentations.</summary>
  internal static class DocumentationHelper
  {
    #region Public Methods and Operators

    /// <summary>Checks whether a documentation header exists for the given syntax node.</summary>
    /// <param name="syntaxNode">The syntax node to check.</param>
    /// <returns>True if a documentation header exists for the given <paramref name="syntaxNode" />.</returns>
    /// <exception cref="ArgumentNullException">Occurs if <paramref name="syntaxNode" /> is null.</exception>
    public static bool HasDocumentationHeader(SyntaxNode syntaxNode)
    {
      DocumentationCommentTriviaSyntax docTrivia;

      if (syntaxNode == null)
      {
        throw new ArgumentNullException(nameof(syntaxNode));
      }

      docTrivia = GetDocumentationTrivia(syntaxNode);

      return docTrivia != null;
    }

    #endregion

    #region Methods

    /// <summary>Gets the documentation trivia of a syntax node.</summary>
    /// <param name="syntaxNode">The syntax node to get the documentation trivia for.</param>
    /// <returns>The documentation trivia of the <paramref name="syntaxNode" /> or null if no documentation header exists.</returns>
    private static DocumentationCommentTriviaSyntax GetDocumentationTrivia(SyntaxNode syntaxNode)
    {
      SyntaxTriviaList leadingTrivia;
      DocumentationCommentTriviaSyntax docTrivia;

      leadingTrivia = syntaxNode.GetLeadingTrivia();
      docTrivia = leadingTrivia
        .Select(t => t.GetStructure())
        .OfType<DocumentationCommentTriviaSyntax>()
        .FirstOrDefault();

      return docTrivia;
    }

    #endregion
  }
}

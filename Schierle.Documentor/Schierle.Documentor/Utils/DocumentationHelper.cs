namespace Schierle.Documentor.Utils
{
  using System;
  using System.Collections.Generic;
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
      IEnumerable<DocumentationCommentTriviaSyntax> docTrivia;

      if (syntaxNode == null)
      {
        throw new ArgumentNullException(nameof(syntaxNode));
      }

      docTrivia = GetDocumentationTrivia(syntaxNode);

      return docTrivia.Any();
    }

    /// <summary>Gets the position of a declarations first empty documentation tag.</summary>
    /// <param name="declaration">The declaration.</param>
    /// <returns>The position of the declarations first empty documentation tag.
    /// A value less than zero (0) will be returned, if no empty documentation tag exists.</returns>
    public static int GetPositionOfFirstEmptyDocumentationTag(MemberDeclarationSyntax declaration)
    {
      IEnumerable<XmlElementSyntax> docTrivia;
      IEnumerable<XmlElementSyntax> emptyTrivia;
      XmlElementSyntax firstEmptyTrivia;
      int result = -1;

      docTrivia = DocumentationHelper.GetDocumentationTrivia(declaration).SelectMany(t => t.Content.OfType<XmlElementSyntax>());
      emptyTrivia = docTrivia.Where(t => !t.ChildNodes().OfType<XmlTextSyntax>().Any());
      firstEmptyTrivia = emptyTrivia.FirstOrDefault();

      if (firstEmptyTrivia != null)
      {
        result = firstEmptyTrivia.EndTag.SpanStart;
      }

      return result;
    }

    #endregion

    #region Methods

    /// <summary>Gets the documentation trivia of a syntax node.</summary>
    /// <param name="syntaxNode">The syntax node to get the documentation trivia for.</param>
    /// <returns>The documentation trivia of the <paramref name="syntaxNode" />.</returns>
    private static IEnumerable<DocumentationCommentTriviaSyntax> GetDocumentationTrivia(SyntaxNode syntaxNode)
    {
      IEnumerable<DocumentationCommentTriviaSyntax> docTrivia;
      SyntaxTriviaList leadingTrivia;

      leadingTrivia = syntaxNode.GetLeadingTrivia();
      docTrivia = leadingTrivia
        .Select(t => t.GetStructure())
        .OfType<DocumentationCommentTriviaSyntax>();

      return docTrivia;
    }

    #endregion
  }
}

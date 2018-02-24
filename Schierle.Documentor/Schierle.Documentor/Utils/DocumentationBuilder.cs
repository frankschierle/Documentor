namespace Schierle.Documentor.Utils
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.CSharp;
  using Microsoft.CodeAnalysis.CSharp.Syntax;

  /// <summary>Provides methods to build XML documentation headers.</summary>
  internal static class DocumentationBuilder
  {
    #region Public Methods and Operators

    /// <summary>Adds a complete XML documentation header to the given member declaration.</summary>
    /// <remarks>Existing XML documentation headers will be ignored. If the
    /// declarations leading trivia already includes a XML documentation header,
    /// an additional documentation header will be added.</remarks>
    /// <param name="declaration">The declaration to document.</param>
    /// <returns>A new member declaration syntax whose leading trivia includes a
    /// complete XML documentation header.</returns>
    /// <exception cref="ArgumentNullException">Occurs if <paramref name="declaration"/> is null.</exception>
    public static SyntaxNode AddCompleteDocumentationHeader(MemberDeclarationSyntax declaration)
    {
      SyntaxNode result;

      if (declaration == null)
      {
        throw new ArgumentNullException(nameof(declaration));
      }

      result = AddSummaryDocumentationToNode(declaration);
      result = AddTypeParameterDocumentationToNode(result);
      result = AddParameterDocumentationToNode(result);
      result = AddReturnValueDocumentationToNode(result);

      return result;
    }

    #endregion

    #region Methods

    /// <summary>Adds a summary documentation tag to a given node.</summary>
    /// <param name="node">The node to document.</param>
    /// <returns>A new node whose leading trivia includes a XML summary
    /// documentation tag.</returns>
    private static SyntaxNode AddSummaryDocumentationToNode(SyntaxNode node)
    {
      XmlElementSyntax summaryElement = SyntaxFactory.XmlSummaryElement();
      DocumentationCommentTriviaSyntax documentationComment = SyntaxFactory.DocumentationComment(summaryElement);

      return AddLineToLeadingTrivia(node, documentationComment);
    }

    /// <summary>Adds the documentation of the type parameters to a given node.</summary>
    /// <param name="node">The node to document.</param>
    /// <returns>A new node whose leading trivia includes a XML documentation tag
    /// for every type parameter. If no type parameters are present, the original
    /// node is returned.</returns>
    private static SyntaxNode AddTypeParameterDocumentationToNode(SyntaxNode node)
    {
      IEnumerable<TypeParameterSyntax> typeParameters = GetTypeParameters(node);
      DocumentationCommentTriviaSyntax documentationComment;
      SyntaxList<XmlAttributeSyntax> attributeList;
      XmlNameAttributeSyntax nameAttribute;
      XmlElementStartTagSyntax startTag;
      XmlElementEndTagSyntax endTag;
      XmlElementSyntax xmlElement;
      SyntaxNode result = node;

      foreach (TypeParameterSyntax parameter in typeParameters)
      {
        nameAttribute = SyntaxFactory.XmlNameAttribute(
          SyntaxFactory.XmlName(" name"),
          SyntaxFactory.Token(SyntaxKind.DoubleQuoteToken),
          SyntaxFactory.IdentifierName(parameter.Identifier),
          SyntaxFactory.Token(SyntaxKind.DoubleQuoteToken));

        attributeList = SyntaxFactory.List<XmlAttributeSyntax>().Add(nameAttribute);
        startTag = SyntaxFactory.XmlElementStartTag(SyntaxFactory.XmlName("typeparam")).WithAttributes(attributeList);
        endTag = SyntaxFactory.XmlElementEndTag(SyntaxFactory.XmlName("typeparam"));
        xmlElement = SyntaxFactory.XmlElement(startTag, endTag);
        documentationComment = SyntaxFactory.DocumentationComment(xmlElement);

        result = AddLineToLeadingTrivia(result, documentationComment);
      }

      return result;
    }

    /// <summary>Gets all type parameters of a node.</summary>
    /// <param name="node">The node.</param>
    /// <returns>All type parameters of the node.</returns>
    private static IEnumerable<TypeParameterSyntax> GetTypeParameters(SyntaxNode node)
    {
      IEnumerable<TypeParameterSyntax> result = null;

      if (node is ClassDeclarationSyntax classDeclaration)
      {
        result = classDeclaration.TypeParameterList?.Parameters;
      }

      if (node is InterfaceDeclarationSyntax interfaceDeclaration)
      {
        result = interfaceDeclaration.TypeParameterList?.Parameters;
      }

      if (node is MethodDeclarationSyntax methodDeclaration)
      {
        result = methodDeclaration.TypeParameterList?.Parameters;
      }

      if (node is StructDeclarationSyntax structDeclaration)
      {
        result = structDeclaration.TypeParameterList?.Parameters;
      }

      // Make sure return value is not null
      result = result ?? new TypeParameterSyntax[0];

      return result;
    }

    /// <summary>Adds the documentation of the parameters to a given node.</summary>
    /// <param name="node">The node to document.</param>
    /// <returns>A new node whose leading trivia includes a XML documentation tag
    /// for every parameter. If no parameters are present, the original node is
    /// returned.</returns>
    private static SyntaxNode AddParameterDocumentationToNode(SyntaxNode node)
    {
      IEnumerable<ParameterSyntax> parameters = GetParameters(node);
      DocumentationCommentTriviaSyntax documentationComment;
      XmlElementSyntax paramElement;
      SyntaxNode result = node;

      foreach (ParameterSyntax parameter in parameters)
      {
        paramElement = SyntaxFactory.XmlParamElement(parameter.Identifier.Text);
        documentationComment = SyntaxFactory.DocumentationComment(paramElement);

        result = AddLineToLeadingTrivia(result, documentationComment);
      }

      return result;
    }

    /// <summary>Gets all parameters of a node.</summary>
    /// <param name="node">The node.</param>
    /// <returns>All parameters of the node.</returns>
    private static IEnumerable<ParameterSyntax> GetParameters(SyntaxNode node)
    {
      IEnumerable<ParameterSyntax> result = null;

      if (node is IndexerDeclarationSyntax indexerDeclaration)
      {
        result = indexerDeclaration.ParameterList?.Parameters;
      }

      if (node is DelegateDeclarationSyntax delegateDeclaration)
      {
        result = delegateDeclaration.ParameterList?.Parameters;
      }

      if (node is BaseMethodDeclarationSyntax methodDeclaration)
      {
        result = methodDeclaration.ParameterList?.Parameters;
      }

      // Make sure return value is not null
      result = result ?? new ParameterSyntax[0];

      return result;
    }

    /// <summary>Adds the documentation of the return value to a given node.</summary>
    /// <param name="node">The node to document.</param>
    /// <returns>A new node whose leading trivia includes a XML documentation tag
    /// for the nodes return value. If no return value is available, the original
    /// node is returned.</returns>
    private static SyntaxNode AddReturnValueDocumentationToNode(SyntaxNode node)
    {
      DocumentationCommentTriviaSyntax documentationComment;
      XmlElementSyntax returnElement;
      SyntaxNode result = node;

      if (IsReturnValueDocumentationRequired(node))
      {
        returnElement = SyntaxFactory.XmlReturnsElement();
        documentationComment = SyntaxFactory.DocumentationComment(returnElement);

        result = AddLineToLeadingTrivia(node, documentationComment);
      }

      return result;
    }

    /// <summary>Checks whether the documentation of a return value is required
    /// for a given syntax node.</summary>
    /// <param name="node">The node to check.</param>
    /// <returns>True if the documentation of a return value is required. Otherwise false.</returns>
    private static bool IsReturnValueDocumentationRequired(SyntaxNode node)
    {
      bool returnValueDocumentationRequired = false;
      SyntaxList<AccessorDeclarationSyntax> accessors;
      PredefinedTypeSyntax returnTypeSyntax;

      if (node is DelegateDeclarationSyntax delegateDeclaration)
      {
        if (delegateDeclaration.ReturnType.Kind() == SyntaxKind.PredefinedType)
        {
          returnTypeSyntax = (PredefinedTypeSyntax)delegateDeclaration.ReturnType;

          // Return type is a predefined type => Documentation required if
          // return type is NOT void
          returnValueDocumentationRequired = !returnTypeSyntax.Keyword.IsKind(SyntaxKind.VoidKeyword);
        }
        else
        {
          // Return type is a user defined type => documentation required
          returnValueDocumentationRequired = true;
        }
      }

      if (node is IndexerDeclarationSyntax indexerDeclaration)
      {
        if (indexerDeclaration.AccessorList != null)
        {
          accessors = indexerDeclaration.AccessorList.Accessors;

          returnValueDocumentationRequired = accessors.Any(a => a.Keyword.IsKind(SyntaxKind.GetKeyword));
        }
      }

      if (node is MethodDeclarationSyntax methodDeclaration)
      {
        if (methodDeclaration.ReturnType.Kind() == SyntaxKind.PredefinedType)
        {
          returnTypeSyntax = (PredefinedTypeSyntax)methodDeclaration.ReturnType;

          // Return type is a predefined type => Documentation required if
          // return type is NOT void
          returnValueDocumentationRequired = !returnTypeSyntax.Keyword.IsKind(SyntaxKind.VoidKeyword);
        }
        else
        {
          // Return type is a user defined type => documentation required
          returnValueDocumentationRequired = true;
        }
      }

      if (node is OperatorDeclarationSyntax operatorDeclaration)
      {
        if (operatorDeclaration.ReturnType.Kind() == SyntaxKind.PredefinedType)
        {
          returnTypeSyntax = (PredefinedTypeSyntax)operatorDeclaration.ReturnType;

          // Return type is a predefined type => Documentation required if
          // return type is NOT void
          returnValueDocumentationRequired = !returnTypeSyntax.Keyword.IsKind(SyntaxKind.VoidKeyword);
        }
        else
        {
          // Return type is a user defined type => documentation required
          returnValueDocumentationRequired = true;
        }
      }

      return returnValueDocumentationRequired;
    }

    /// <summary>Adds a new line at the end of a nodes leading trivia.</summary>
    /// <param name="node">The node whose leading trivia the new line should be added.</param>
    /// <param name="triviaSyntax">The structured tricia syntax to add.</param>
    /// <returns>The provided <paramref name="node"/> with a leading trivia including
    /// the original leading trivia and the provided <paramref name="triviaSyntax"/>.</returns>
    private static SyntaxNode AddLineToLeadingTrivia(SyntaxNode node, StructuredTriviaSyntax triviaSyntax)
    {
      SyntaxTriviaList currentLeadingTrivia = node.GetLeadingTrivia();
      SyntaxTrivia newContentTrivia = SyntaxFactory.Trivia(triviaSyntax);
      SyntaxTrivia endOfLineTrivia = SyntaxFactory.EndOfLine(Environment.NewLine);
      SyntaxTriviaList newLeadingTrivia = currentLeadingTrivia.Add(newContentTrivia).Add(endOfLineTrivia);

      return node.WithLeadingTrivia(newLeadingTrivia);
    }

    #endregion
  }
}

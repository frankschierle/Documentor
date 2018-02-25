namespace Schierle.Documentor.CodeFixProviders
{
  using System.Collections.Immutable;
  using System.Composition;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.CodeActions;
  using Microsoft.CodeAnalysis.CodeFixes;
  using Microsoft.CodeAnalysis.CSharp;
  using Microsoft.CodeAnalysis.CSharp.Syntax;

  using Schierle.Documentor.Analyzers;
  using Schierle.Documentor.CodeActions;
  using Schierle.Documentor.Utils;

  /// <summary>Provides a code fix for diagnostics detected by
  /// <see cref="ElementsMustBeDocumented"/> analyzer.</summary>
  [Shared]
  [ExportCodeFixProvider(LanguageNames.CSharp)]
  public class ElementsMustBeDocumentedCodeFixProvider : CodeFixProvider
  {
    #region Public Properties

    /// <inheritdoc />
    public override ImmutableArray<string> FixableDiagnosticIds
    {
      get { return ImmutableArray.Create(AnalyzerIds.ElementsMustBeDocumented); }
    }

    #endregion

    #region Public Methods and Operators

    /// <inheritdoc />
    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
      return Task.Run(() =>
                      {
                        Diagnostic diagnostic = context.Diagnostics.First();

                        CodeAction codeFix = new NavigateAfterCodeChangeAction(
                          "Add documentation header",
                          c => AddDocumentationHeaderAsync(context.Document, diagnostic, c),
                          (s, c) => CalculateNavigationTarget(s, context.Document, diagnostic, c));

                        context.RegisterCodeFix(codeFix, diagnostic);
                      });
    }

    #endregion

    #region Methods

    /// <summary>Adds a XML documentation header to a diagnosted member declaration.</summary>
    /// <param name="document">The document that includes the diagnostic.</param>
    /// <param name="diagnostic">The detected disgnostic, that should be fixed.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the action.</param>
    /// <returns>A solution including the fixed code.</returns>
    private static async Task<Solution> AddDocumentationHeaderAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
    {
      SyntaxNode root = await document.GetSyntaxRootAsync(cancellationToken);
      SyntaxNode syntaxNode = root.FindNode(diagnostic.Location.SourceSpan);
      MemberDeclarationSyntax targetNode = syntaxNode.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().First();
      SyntaxNode newSyntaxNode = DocumentationBuilder.AddCompleteDocumentationHeader(targetNode);
      Document newDocument = document.WithSyntaxRoot(root.ReplaceNode(targetNode, newSyntaxNode));

      return newDocument.Project.Solution;
    }

    /// <summary>Calculates the position, the caret should be set to, after the
    /// code fix was applied.</summary>
    /// <param name="changedSolution">The changed solution (code fix applied).</param>
    /// <param name="originalDocument">The original document (code fix not applied).</param>
    /// <param name="diagnostic">The diagnosted code fix.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the calculation.</param>
    /// <returns>The position , the caret should be set to, after the code fix
    /// was applied. Null if the caret should not be moved.</returns>
    private static async Task<NavigationTarget> CalculateNavigationTarget(Solution changedSolution, Document originalDocument, Diagnostic diagnostic, CancellationToken cancellationToken)
    {
      MemberDeclarationSyntax originalDeclaration;
      MemberDeclarationSyntax changedDeclaration;
      SyntaxNode originalSyntaxRoot;
      SyntaxNode changedSyntaxRoot;
      Document changedDocument;
      int targetPosition;
      NavigationTarget result = null;

      // Get syntax root of the original document
      originalSyntaxRoot = await originalDocument.GetSyntaxRootAsync(cancellationToken)
                                                 .ConfigureAwait(false);

      // Get the original declaration that was diagnosted
      originalDeclaration = originalSyntaxRoot.FindToken(diagnostic.Location.SourceSpan.Start)
                                              .Parent
                                              .AncestorsAndSelf()
                                              .OfType<MemberDeclarationSyntax>()
                                              .First();

      // Get the changed document
      changedDocument = changedSolution.GetDocument(originalDocument.Id);

      // Get syntax root of the changed document
      changedSyntaxRoot = await changedDocument.GetSyntaxRootAsync(cancellationToken)
                                               .ConfigureAwait(false);

      // Find declaration in changed document
      changedDeclaration = changedSyntaxRoot.DescendantNodes()
                                            .OfType<MemberDeclarationSyntax>()
                                            .FirstOrDefault(newNode => SyntaxFactory.AreEquivalent(newNode, originalDeclaration));

      // Get the target position in the the XML documentation header of the
      // changed declaration inside the changed document
      targetPosition = DocumentationHelper.GetPositionOfFirstEmptyDocumentationTag(changedDeclaration);

      // targetPosition is less than zero if no empty documentation tag was found.
      if (targetPosition > 0)
      {
        result = new NavigationTarget(changedDocument.Id, targetPosition);
      }

      return result;
    }

    #endregion
  }
}

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
  using Microsoft.CodeAnalysis.CSharp.Syntax;

  using Schierle.Documentor.Analyzers;
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
                        CodeAction codeFix = CodeAction.Create("Add documentation header", c => AddDocumentationHeaderAsync(context.Document, diagnostic, c));

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

    #endregion
  }
}

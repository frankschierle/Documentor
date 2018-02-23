namespace Schierle.Documentor.Analyzers
{
  using System.Collections.Generic;
  using System.Linq;

  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.CSharp;
  using Microsoft.CodeAnalysis.CSharp.Syntax;
  using Microsoft.CodeAnalysis.Diagnostics;

  using Schierle.Documentor.Utils;

  /// <summary>Checks that all code element are documented.</summary>
  [DiagnosticAnalyzer(LanguageNames.CSharp)]
  public class DR0001ElementsMustBeDocumented : AnalyzerBase
  {
    #region Public Properties

    /// <inheritdoc />
    public override string Id
    {
      get { return "DR0001"; }
    }

    /// <inheritdoc />
    public override string Title
    {
      get { return "ElementsMustBeDocumented"; }
    }

    /// <inheritdoc />
    public override string Description
    {
      get { return "A code element is missing a documentation header."; }
    }

    /// <inheritdoc />
    public override DiagnosticSeverity DefaultSeverity
    {
      get { return DiagnosticSeverity.Warning; }
    }

    #endregion

    #region Properties

    /// <inheritdoc />
    protected override string Message
    {
      get { return "Element '{0}' should be documented."; }
    }

    #endregion

    #region Public Methods and Operators

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
      context.RegisterSyntaxNodeAction(this.AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
      context.RegisterSyntaxNodeAction(this.AnalyzeConstructorDeclaration, SyntaxKind.ConstructorDeclaration);
      context.RegisterSyntaxNodeAction(this.AnalyzeDelegateDeclaration, SyntaxKind.DelegateDeclaration);
      context.RegisterSyntaxNodeAction(this.AnalyzeDestructorDeclaration, SyntaxKind.DestructorDeclaration);
      context.RegisterSyntaxNodeAction(this.AnalyzeEnumDeclaration, SyntaxKind.EnumDeclaration);
      context.RegisterSyntaxNodeAction(this.AnalyzeEnumMemberDeclaration, SyntaxKind.EnumMemberDeclaration);
      context.RegisterSyntaxNodeAction(this.AnalyzeEventDeclaration, SyntaxKind.EventDeclaration);
      context.RegisterSyntaxNodeAction(this.AnalyzeEventFieldDeclaration, SyntaxKind.EventFieldDeclaration);
      context.RegisterSyntaxNodeAction(this.AnalyzeFieldDeclaration, SyntaxKind.FieldDeclaration);
      context.RegisterSyntaxNodeAction(this.AnalyzeIndexerDeclaration, SyntaxKind.IndexerDeclaration);
      context.RegisterSyntaxNodeAction(this.AnalyzeInterfaceDeclaration, SyntaxKind.InterfaceDeclaration);
      context.RegisterSyntaxNodeAction(this.AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
      context.RegisterSyntaxNodeAction(this.AnalyzeOperatorDeclaration, SyntaxKind.OperatorDeclaration);
      context.RegisterSyntaxNodeAction(this.AnalyzePropertyDeclaration, SyntaxKind.PropertyDeclaration);
      context.RegisterSyntaxNodeAction(this.AnalyzeStrutDeclaration, SyntaxKind.StructDeclaration);
    }

    #endregion

    #region Methods

    /// <summary>Analyzes a <see cref="ClassDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (ClassDeclarationSyntax)context.Node;

      this.Analyze(context, declarationSyntax.Identifier.Yield());
    }

    /// <summary>Analyzes a <see cref="ConstructorDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzeConstructorDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (ConstructorDeclarationSyntax)context.Node;

      this.Analyze(context, declarationSyntax.Identifier.Yield());
    }

    /// <summary>Analyzes a <see cref="DelegateDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzeDelegateDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (DelegateDeclarationSyntax)context.Node;

      this.Analyze(context, declarationSyntax.Identifier.Yield());
    }

    /// <summary>Analyzes a <see cref="DestructorDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzeDestructorDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (DestructorDeclarationSyntax)context.Node;

      this.Analyze(context, declarationSyntax.Identifier.Yield());
    }

    /// <summary>Analyzes a <see cref="EnumDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzeEnumDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (EnumDeclarationSyntax)context.Node;

      this.Analyze(context, declarationSyntax.Identifier.Yield());
    }

    /// <summary>Analyzes a <see cref="EnumMemberDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzeEnumMemberDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (EnumMemberDeclarationSyntax)context.Node;

      this.Analyze(context, declarationSyntax.Identifier.Yield());
    }

    /// <summary>Analyzes a <see cref="EventDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzeEventDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (EventDeclarationSyntax)context.Node;

      this.Analyze(context, declarationSyntax.Identifier.Yield());
    }

    /// <summary>Analyzes a <see cref="EventFieldDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzeEventFieldDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (EventFieldDeclarationSyntax)context.Node;
      IEnumerable<SyntaxToken> identifiers = declarationSyntax.Declaration.Variables.Select(v => v.Identifier);

      this.Analyze(context, identifiers);
    }

    /// <summary>Analyzes a <see cref="FieldDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzeFieldDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (FieldDeclarationSyntax)context.Node;
      IEnumerable<SyntaxToken> identifiers = declarationSyntax.Declaration.Variables.Select(v => v.Identifier);

      this.Analyze(context, identifiers);
    }

    /// <summary>Analyzes a <see cref="IndexerDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzeIndexerDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (IndexerDeclarationSyntax)context.Node;

      this.Analyze(context, declarationSyntax.ThisKeyword.Yield());
    }

    /// <summary>Analyzes a <see cref="InterfaceDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzeInterfaceDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (InterfaceDeclarationSyntax)context.Node;

      this.Analyze(context, declarationSyntax.Identifier.Yield());
    }

    /// <summary>Analyzes a <see cref="MethodDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (MethodDeclarationSyntax)context.Node;

      this.Analyze(context, declarationSyntax.Identifier.Yield());
    }

    /// <summary>Analyzes a <see cref="OperatorDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzeOperatorDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (OperatorDeclarationSyntax)context.Node;

      this.Analyze(context, declarationSyntax.OperatorToken.Yield());
    }

    /// <summary>Analyzes a <see cref="PropertyDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzePropertyDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (PropertyDeclarationSyntax)context.Node;

      this.Analyze(context, declarationSyntax.Identifier.Yield());
    }

    /// <summary>Analyzes a <see cref="StructDeclarationSyntax" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    private void AnalyzeStrutDeclaration(SyntaxNodeAnalysisContext context)
    {
      var declarationSyntax = (StructDeclarationSyntax)context.Node;

      this.Analyze(context, declarationSyntax.Identifier.Yield());
    }

    /// <summary>Analyzes a <see cref="SyntaxNode" />.</summary>
    /// <param name="context">The context for the syntax node analysis.</param>
    /// <param name="identifiers">Diagnostics will be reported for all provided identifiers, if the syntax node does not has a documentation header.</param>
    private void Analyze(SyntaxNodeAnalysisContext context, IEnumerable<SyntaxToken> identifiers)
    {
      Diagnostic diagnostic;

      if (!DocumentationHelper.HasDocumentationHeader(context.Node))
      {
        foreach (SyntaxToken identifier in identifiers)
        {
          diagnostic = this.CreateDiagnostic(identifier.GetLocation(), identifier);

          context.ReportDiagnostic(diagnostic);
        }
      }
    }

    #endregion
  }
}

namespace Schierle.Documentor.Test.CodeFixProviders.ElementsMustBeDocumentedCodeFixProviderTests
{
  using Microsoft.CodeAnalysis.CodeFixes;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Schierle.Documentor.Analyzers;
  using Schierle.Documentor.CodeFixProviders;

  [TestClass]
  public partial class ElementsMustBeDocumentedCodeFixProviderTest : CodeFixVerifierBase
  {
    #region Fields

    private AnalyzerBase analyzer;

    private CodeFixProvider codeFixProvider;

    #endregion

    #region Methods

    protected override AnalyzerBase GetDiagnosticAnalyzer()
    {
      if (this.analyzer == null)
      {
        this.analyzer = new ElementsMustBeDocumented();
      }

      return this.analyzer;
    }

    /// <inheritdoc />
    protected override CodeFixProvider GetCodeFixProvider()
    {
      if (this.codeFixProvider == null)
      {
        this.codeFixProvider = new ElementsMustBeDocumentedCodeFixProvider();
      }

      return this.codeFixProvider;
    }

    #endregion
  }
}

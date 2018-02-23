namespace Schierle.Documentor.Test.Analyzers.ElementsMustBeDocumentedTests
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Schierle.Documentor.Analyzers;

  [TestClass]
  public partial class ElementsMustBeDocumentedTest : DiagnosticVerifierBase
  {
    #region Fields

    private AnalyzerBase analyzer;

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

    #endregion
  }
}

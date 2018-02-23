namespace Schierle.Documentor.Test.Analyzers.DR0001ElementsMustBeDocumentedTests
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Schierle.Documentor.Analyzers;

  [TestClass]
  public partial class DR0001ElementsMustBeDocumentedTest : DiagnosticVerifierBase
  {
    #region Fields

    private AnalyzerBase analyzer;

    #endregion

    #region Methods

    protected override AnalyzerBase GetDiagnosticAnalyzer()
    {
      if (this.analyzer == null)
      {
        this.analyzer = new DR0001ElementsMustBeDocumented();
      }

      return this.analyzer;
    }

    #endregion
  }
}

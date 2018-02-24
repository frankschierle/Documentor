namespace Schierle.Documentor.Test
{
  using System.Collections.Generic;
  using System.Collections.Immutable;
  using System.Linq;
  using System.Threading;

  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.CodeActions;
  using Microsoft.CodeAnalysis.CodeFixes;
  using Microsoft.CodeAnalysis.Diagnostics;
  using Microsoft.CodeAnalysis.Formatting;
  using Microsoft.CodeAnalysis.Simplification;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Schierle.Documentor.Analyzers;

  public abstract class CodeFixVerifierBase : DiagnosticVerifierBase
  {
    #region Methods

    /// <summary>Gets the C# code fix provider being tested.</summary>
    /// <returns>The code fix provider to test.</returns>
    protected abstract CodeFixProvider GetCodeFixProvider();

    /// <summary>Called to test a C# codefix when applied on the inputted string as a source.</summary>
    /// <param name="oldSource">A class in the form of a string before the CodeFix was applied to it.</param>
    /// <param name="newSource">A class in the form of a string after the CodeFix was applied to it.</param>
    /// <param name="codeFixIndex">Index determining which codefix to apply if there are multiple.</param>
    /// <param name="allowNewCompilerDiagnostics">A bool controlling whether or not the test will fail if the CodeFix introduces other warnings after being applied.</param>
    protected void VerifyCodeFix(string oldSource, string newSource, int? codeFixIndex = null, bool allowNewCompilerDiagnostics = false)
    {
      AnalyzerBase analyzer = this.GetDiagnosticAnalyzer();
      CodeFixProvider codeFixProvider = this.GetCodeFixProvider();

      this.VerifyFix(analyzer, codeFixProvider, oldSource, newSource, codeFixIndex, allowNewCompilerDiagnostics);
    }

    /// <summary>Apply the inputted CodeAction to the inputted document. Meant to be used to apply codefixes.</summary>
    /// <param name="document">The Document to apply the fix on.</param>
    /// <param name="codeAction">A CodeAction that will be applied to the Document.</param>
    /// <returns>A Document with the changes from the CodeAction.</returns>
    private static Document ApplyFix(Document document, CodeAction codeAction)
    {
      ImmutableArray<CodeActionOperation> operations = codeAction.GetOperationsAsync(CancellationToken.None).Result;
      Solution solution = operations.OfType<ApplyChangesOperation>().Single().ChangedSolution;

      return solution.GetDocument(document.Id);
    }

    /// <summary>Compare two collections of Diagnostics, and return a list of any new diagnostics that appear only in the second collection.
    /// Note: Considers Diagnostics to be the same if they have the same Ids. In the case of multiple diagnostics with the same Id in a row,
    /// this method may not necessarily return the new one.</summary>
    /// <param name="diagnostics">The Diagnostics that existed in the code before the CodeFix was applied.</param>
    /// <param name="newDiagnostics">The Diagnostics that exist in the code after the CodeFix was applied.</param>
    /// <returns>A list of Diagnostics that only surfaced in the code after the CodeFix was applied.</returns>
    private static IEnumerable<Diagnostic> GetNewDiagnostics(IEnumerable<Diagnostic> diagnostics, IEnumerable<Diagnostic> newDiagnostics)
    {
      Diagnostic[] oldArray = diagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToArray();
      Diagnostic[] newArray = newDiagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToArray();
      int oldIndex = 0;
      int newIndex = 0;

      while (newIndex < newArray.Length)
      {
        if ((oldIndex < oldArray.Length) && (oldArray[oldIndex].Id == newArray[newIndex].Id))
        {
          ++oldIndex;
          ++newIndex;
        }
        else
        {
          yield return newArray[newIndex++];
        }
      }
    }

    /// <summary>Get the existing compiler diagnostics on the inputted document.</summary>
    /// <param name="document">The Document to run the compiler diagnostic analyzers on.</param>
    /// <returns>The compiler diagnostics that were found in the code</returns>
    private static IEnumerable<Diagnostic> GetCompilerDiagnostics(Document document)
    {
      return document.GetSemanticModelAsync().Result.GetDiagnostics();
    }

    /// <summary>Given a document, turn it into a string based on the syntax root.</summary>
    /// <param name="document">The Document to be converted to a string.</param>
    /// <returns>A string containing the syntax of the Document after formatting.</returns>
    private static string GetStringFromDocument(Document document)
    {
      Document simplifiedDoc = Simplifier.ReduceAsync(document, Simplifier.Annotation).Result;
      SyntaxNode root = simplifiedDoc.GetSyntaxRootAsync().Result;

      root = Formatter.Format(root, Formatter.Annotation, simplifiedDoc.Project.Solution.Workspace);

      return root.GetText().ToString();
    }

    /// <summary>Creates a Document from the source string, then gets diagnostics
    /// on it and applies the relevant codefixes. Then gets the string after the
    /// codefix is applied and compares it with the expected result.</summary>
    /// <remarks>If any codefix causes new diagnostics to show up, the test
    /// fails unless allowNewCompilerDiagnostics is set to true.</remarks>
    /// <param name="analyzer">The analyzer to be applied to the source code.</param>
    /// <param name="codeFixProvider">The codefix to be applied to the code wherever the relevant Diagnostic is found.</param>
    /// <param name="oldSource">A class in the form of a string before the CodeFix was applied to it.</param>
    /// <param name="newSource">A class in the form of a string after the CodeFix was applied to it.</param>
    /// <param name="codeFixIndex">Index determining which codefix to apply if there are multiple.</param>
    /// <param name="allowNewCompilerDiagnostics">A bool controlling whether or not the test will fail if the CodeFix introduces other warnings after being applied.</param>
    private void VerifyFix(DiagnosticAnalyzer analyzer, CodeFixProvider codeFixProvider, string oldSource, string newSource, int? codeFixIndex, bool allowNewCompilerDiagnostics)
    {
      Document document = CreateDocument(oldSource);
      Diagnostic[] analyzerDiagnostics = GetSortedDiagnosticsFromDocuments(analyzer, new[] { document });
      IEnumerable<Diagnostic> compilerDiagnostics = GetCompilerDiagnostics(document);
      int attempts = analyzerDiagnostics.Length;
      List<CodeAction> actions;
      CodeFixContext context;
      IEnumerable<Diagnostic> newCompilerDiagnostics;
      string actual;

      for (int i = 0; i < attempts; ++i)
      {
        actions = new List<CodeAction>();
        context = new CodeFixContext(document, analyzerDiagnostics[0], (a, d) => actions.Add(a), CancellationToken.None);
        codeFixProvider.RegisterCodeFixesAsync(context).Wait();

        if (!actions.Any())
        {
          break;
        }

        if (codeFixIndex != null)
        {
          document = ApplyFix(document, actions.ElementAt((int)codeFixIndex));
          break;
        }

        document = ApplyFix(document, actions.ElementAt(0));
        analyzerDiagnostics = GetSortedDiagnosticsFromDocuments(analyzer, new[] { document });

        newCompilerDiagnostics = GetNewDiagnostics(compilerDiagnostics, GetCompilerDiagnostics(document));

        // Check if applying the code fix introduced any new compiler diagnostics
        if (!allowNewCompilerDiagnostics && newCompilerDiagnostics.Any())
        {
          // Format and get the compiler diagnostics again so that the locations make sense in the output
          document = document.WithSyntaxRoot(Formatter.Format(document.GetSyntaxRootAsync().Result, Formatter.Annotation, document.Project.Solution.Workspace));
          newCompilerDiagnostics = GetNewDiagnostics(compilerDiagnostics, GetCompilerDiagnostics(document));

          Assert.IsTrue(false, string.Format("Fix introduced new compiler diagnostics:\r\n{0}\r\n\r\nNew document:\r\n{1}\r\n", string.Join("\r\n", newCompilerDiagnostics.Select(d => d.ToString())), document.GetSyntaxRootAsync().Result.ToFullString()));
        }

        // Check if there are analyzer diagnostics left after the code fix
        if (!analyzerDiagnostics.Any())
        {
          break;
        }
      }

      // After applying all of the code fixes, compare the resulting string to the inputted one
      actual = GetStringFromDocument(document);
      Assert.AreEqual(newSource, actual);
    }

    #endregion
  }
}

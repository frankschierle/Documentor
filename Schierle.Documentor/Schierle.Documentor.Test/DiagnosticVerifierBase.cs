namespace Schierle.Documentor.Test
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Immutable;
  using System.Linq;
  using System.Text;

  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.CSharp;
  using Microsoft.CodeAnalysis.Diagnostics;
  using Microsoft.CodeAnalysis.Text;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Schierle.Documentor.Analyzers;

  /// <summary>Base class of all Unit Tests for DiagnosticAnalyzers.</summary>
  public abstract class DiagnosticVerifierBase
  {
    #region Static Fields

    /// <summary>MetadataReference to mscorlib.</summary>
    private static readonly MetadataReference CorlibReference = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);

    /// <summary>MetadataReference to SytemCore assembly.</summary>
    private static readonly MetadataReference SystemCoreReference = MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);

    /// <summary>MetadataReference to Microsoft.CodeAnalysis.CSharp assembly.</summary>
    private static readonly MetadataReference CSharpSymbolsReference = MetadataReference.CreateFromFile(typeof(CSharpCompilation).Assembly.Location);

    /// <summary>MetadataReference to Microsoft.CodeAnalysis assembly.</summary>
    private static readonly MetadataReference CodeAnalysisReference = MetadataReference.CreateFromFile(typeof(Compilation).Assembly.Location);

    #endregion

    #region Methods

    /// <summary>Given an analyzer and a document to apply it to, run the analyzer
    /// and gather an array of diagnostics found in it. The returned diagnostics are
    /// then ordered by location in the source document.</summary>
    /// <param name="analyzer">The analyzer to run on the documents.</param>
    /// <param name="documents">The documents that the analyzer will be run on.</param>
    /// <returns>An IEnumerable of Diagnostics that surfaced in the source code, sorted by Location.</returns>
    protected static Diagnostic[] GetSortedDiagnosticsFromDocuments(DiagnosticAnalyzer analyzer, Document[] documents)
    {
      var diagnostics = new List<Diagnostic>();
      var projects = new HashSet<Project>();
      CompilationWithAnalyzers compilationWithAnalyzers;
      ImmutableArray<Diagnostic> diags;
      Diagnostic[] results;
      Document document;
      SyntaxTree tree;

      foreach (Document doc in documents)
      {
        projects.Add(doc.Project);
      }

      foreach (Project project in projects)
      {
        compilationWithAnalyzers = project.GetCompilationAsync().Result.WithAnalyzers(ImmutableArray.Create(analyzer));
        diags = compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().Result;

        foreach (Diagnostic diag in diags)
        {
          if ((diag.Location == Location.None) || diag.Location.IsInMetadata)
          {
            diagnostics.Add(diag);
          }
          else
          {
            for (int i = 0; i < documents.Length; i++)
            {
              document = documents[i];
              tree = document.GetSyntaxTreeAsync().Result;

              if (tree == diag.Location.SourceTree)
              {
                diagnostics.Add(diag);
              }
            }
          }
        }
      }

      results = SortDiagnostics(diagnostics);

      diagnostics.Clear();

      return results;
    }

    /// <summary>Create a document from a string through creating a project that contains it.</summary>
    /// <param name="source">Classes in the form of a string.</param>
    /// <returns>A document created from the source string.</returns>
    protected static Document CreateDocument(string source)
    {
      string[] sources =
      {
        source
      };

      return CreateProject(sources).Documents.First();
    }

    protected DiagnosticResult CreateDiagnosticResult(string message, params DiagnosticResultLocation[] locations)
    {
      AnalyzerBase analyzer = this.GetDiagnosticAnalyzer();

      return new DiagnosticResult
      {
        Id = analyzer.Id,
        Message = message,
        Locations = locations,
        Severity = analyzer.DefaultSeverity
      };
    }

    /// <summary>Gets the C# analyzer being tested.</summary>
    protected abstract AnalyzerBase GetDiagnosticAnalyzer();

    /// <summary>Called to test a C# DiagnosticAnalyzer when applied on the
    /// single inputted string as a source.</summary>
    /// <param name="source">A class in the form of a string to run the analyzer on.</param>
    /// <param name="expected">DiagnosticResults that should appear after the analyzer is run on the source.</param>
    protected void VerifyCSharpDiagnostic(string source, params DiagnosticResult[] expected)
    {
      this.VerifyDiagnostics(new[] { source }, this.GetDiagnosticAnalyzer(), expected);
    }

    /// <summary>Called to test a C# DiagnosticAnalyzer when applied on the
    /// inputted strings as a source.</summary>
    /// <param name="sources">An array of strings to create source documents from to run the analyzers on.</param>
    /// <param name="expected">DiagnosticResults that should appear after the analyzer is run on the sources.</param>
    protected void VerifyCSharpDiagnostic(string[] sources, params DiagnosticResult[] expected)
    {
      this.VerifyDiagnostics(sources, this.GetDiagnosticAnalyzer(), expected);
    }

    /// <summary>Checks each of the actual Diagnostics found and compares them
    /// with the corresponding DiagnosticResult in the array of expected results.
    /// Diagnostics are considered equal only if the DiagnosticResultLocation,
    /// Id, Severity, and Message of the DiagnosticResult match the actual
    /// diagnostic.</summary>
    /// <param name="actualResults">The Diagnostics found by the compiler after running the analyzer on the source code.</param>
    /// <param name="analyzer">The analyzer that was being run on the sources.</param>
    /// <param name="expectedResults">Diagnostic Results that should have appeared in the code.</param>
    private static void VerifyDiagnosticResults(IEnumerable<Diagnostic> actualResults, DiagnosticAnalyzer analyzer, params DiagnosticResult[] expectedResults)
    {
      int expectedCount = expectedResults.Count();
      int actualCount = actualResults.Count();
      Diagnostic actual;
      DiagnosticResult expected;
      Location[] additionalLocations;

      if (expectedCount != actualCount)
      {
        string diagnosticsOutput = actualResults.Any() ? FormatDiagnostics(analyzer, actualResults.ToArray()) : "    NONE.";

        Assert.IsTrue(false, string.Format("Mismatch between number of diagnostics returned, expected \"{0}\" actual \"{1}\"\r\n\r\nDiagnostics:\r\n{2}\r\n", expectedCount, actualCount, diagnosticsOutput));
      }

      for (int i = 0; i < expectedResults.Length; i++)
      {
        actual = actualResults.ElementAt(i);
        expected = expectedResults[i];

        if ((expected.Line == -1) && (expected.Column == -1))
        {
          if (actual.Location != Location.None)
          {
            Assert.IsTrue(false, string.Format("Expected:\nA project diagnostic with No location\nActual:\n{0}", FormatDiagnostics(analyzer, actual)));
          }
        }
        else
        {
          VerifyDiagnosticLocation(analyzer, actual, actual.Location, expected.Locations.First());
          additionalLocations = actual.AdditionalLocations.ToArray();

          if (additionalLocations.Length != (expected.Locations.Length - 1))
          {
            Assert.IsTrue(false, string.Format("Expected {0} additional locations but got {1} for Diagnostic:\r\n    {2}\r\n", expected.Locations.Length - 1, additionalLocations.Length, FormatDiagnostics(analyzer, actual)));
          }

          for (int j = 0; j < additionalLocations.Length; ++j)
          {
            VerifyDiagnosticLocation(analyzer, actual, additionalLocations[j], expected.Locations[j + 1]);
          }
        }

        if (actual.Id != expected.Id)
        {
          Assert.IsTrue(false, string.Format("Expected diagnostic id to be \"{0}\" was \"{1}\"\r\n\r\nDiagnostic:\r\n    {2}\r\n", expected.Id, actual.Id, FormatDiagnostics(analyzer, actual)));
        }

        if (actual.Severity != expected.Severity)
        {
          Assert.IsTrue(false, string.Format("Expected diagnostic severity to be \"{0}\" was \"{1}\"\r\n\r\nDiagnostic:\r\n    {2}\r\n", expected.Severity, actual.Severity, FormatDiagnostics(analyzer, actual)));
        }

        if (actual.GetMessage() != expected.Message)
        {
          Assert.IsTrue(false, string.Format("Expected diagnostic message to be \"{0}\" was \"{1}\"\r\n\r\nDiagnostic:\r\n    {2}\r\n", expected.Message, actual.GetMessage(), FormatDiagnostics(analyzer, actual)));
        }
      }
    }

    /// <summary>Helper method to <see cref="VerifyDiagnosticResults" /> that
    /// checks the location of a diagnostic and compares it with the location
    /// in the expected DiagnosticResult.</summary>
    /// <param name="analyzer">The analyzer that was being run on the sources.</param>
    /// <param name="diagnostic">The diagnostic that was found in the code.</param>
    /// <param name="actual">The Location of the Diagnostic found in the code.</param>
    /// <param name="expected">The DiagnosticResultLocation that should have been found.</param>
    private static void VerifyDiagnosticLocation(DiagnosticAnalyzer analyzer, Diagnostic diagnostic, Location actual, DiagnosticResultLocation expected)
    {
      FileLinePositionSpan actualSpan = actual.GetLineSpan();
      LinePosition actualLinePosition;

      Assert.IsTrue((actualSpan.Path == expected.Path) || ((actualSpan.Path != null) && actualSpan.Path.Contains("Test0.") && expected.Path.Contains("Test.")),
                    string.Format("Expected diagnostic to be in file \"{0}\" was actually in file \"{1}\"\r\n\r\nDiagnostic:\r\n    {2}\r\n",
                                  expected.Path,
                                  actualSpan.Path,
                                  FormatDiagnostics(analyzer, diagnostic)));

      actualLinePosition = actualSpan.StartLinePosition;

      // Only check line position if there is an actual line in the real diagnostic
      if (actualLinePosition.Line > 0)
      {
        if ((actualLinePosition.Line + 1) != expected.Line)
        {
          Assert.IsTrue(false, string.Format("Expected diagnostic to be on line \"{0}\" was actually on line \"{1}\"\r\n\r\nDiagnostic:\r\n    {2}\r\n", expected.Line, actualLinePosition.Line + 1, FormatDiagnostics(analyzer, diagnostic)));
        }
      }

      // Only check column position if there is an actual column position in the real diagnostic
      if (actualLinePosition.Character > 0)
      {
        if ((actualLinePosition.Character + 1) != expected.Column)
        {
          Assert.IsTrue(false, string.Format("Expected diagnostic to start at column \"{0}\" was actually at column \"{1}\"\r\n\r\nDiagnostic:\r\n    {2}\r\n", expected.Column, actualLinePosition.Character + 1, FormatDiagnostics(analyzer, diagnostic)));
        }
      }
    }

    /// <summary>Helper method to format a Diagnostic into an easily readable string.</summary>
    /// <param name="analyzer">The analyzer that this verifier tests.</param>
    /// <param name="diagnostics">The Diagnostics to be formatted.</param>
    /// <returns>The Diagnostics formatted as a string.</returns>
    private static string FormatDiagnostics(DiagnosticAnalyzer analyzer, params Diagnostic[] diagnostics)
    {
      var builder = new StringBuilder();
      Type analyzerType;
      ImmutableArray<DiagnosticDescriptor> rules;
      Location location;
      LinePosition linePosition;

      for (int i = 0; i < diagnostics.Length; ++i)
      {
        builder.AppendLine("// " + diagnostics[i]);

        analyzerType = analyzer.GetType();
        rules = analyzer.SupportedDiagnostics;

        foreach (DiagnosticDescriptor rule in rules)
        {
          if ((rule != null) && (rule.Id == diagnostics[i].Id))
          {
            location = diagnostics[i].Location;

            if (location == Location.None)
            {
              builder.AppendFormat("GetGlobalResult({0}.{1})", analyzerType.Name, rule.Id);
            }
            else
            {
              Assert.IsTrue(location.IsInSource, $"Test base does not currently handle diagnostics in metadata locations. Diagnostic in metadata: {diagnostics[i]}\r\n");

              linePosition = diagnostics[i].Location.GetLineSpan().StartLinePosition;

              builder.AppendFormat("{0}({1}, {2}, {3}.{4})",
                                   "GetCSharpResultAt",
                                   linePosition.Line + 1,
                                   linePosition.Character + 1,
                                   analyzerType.Name,
                                   rule.Id);
            }

            if (i != (diagnostics.Length - 1))
            {
              builder.Append(',');
            }

            builder.AppendLine();

            break;
          }
        }
      }

      return builder.ToString();
    }

    /// <summary>
    /// Given classes in the form of strings, their language, and an IDiagnosticAnalyzer to apply to it, return the diagnostics found in the string after converting it to a document.
    /// </summary>
    /// <param name="sources">Classes in the form of strings</param>
    /// <param name="analyzer">The analyzer to be run on the sources</param>
    /// <returns>An IEnumerable of Diagnostics that surfaced in the source code, sorted by Location</returns>
    private static Diagnostic[] GetSortedDiagnostics(string[] sources, DiagnosticAnalyzer analyzer)
    {
      return GetSortedDiagnosticsFromDocuments(analyzer, GetDocuments(sources));
    }

    /// <summary>Sort diagnostics by location in source document.</summary>
    /// <param name="diagnostics">The list of diagnostics to be sorted.</param>
    /// <returns>An IEnumerable containing the diagnostics in order of location.</returns>
    private static Diagnostic[] SortDiagnostics(IEnumerable<Diagnostic> diagnostics)
    {
      return diagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToArray();
    }

    /// <summary>Given an array of strings as sources, turn them into a project and return the documents and spans of it.</summary>
    /// <param name="sources">Classes in the form of strings.</param>
    /// <returns>A tuple containing the documents produced from the sources and their TextSpans if relevant.</returns>
    private static Document[] GetDocuments(string[] sources)
    {
      Project project = CreateProject(sources);
      Document[] documents = project.Documents.ToArray();

      if (sources.Length != documents.Length)
      {
        throw new InvalidOperationException("Amount of sources did not match amount of Documents created");
      }

      return documents;
    }

    /// <summary>Create a project using the inputted strings as sources.</summary>
    /// <param name="sources">Classes in the form of strings.</param>
    /// <returns>A project created out of the documents created from the source strings.</returns>
    private static Project CreateProject(string[] sources)
    {
      ProjectId projectId = ProjectId.CreateNewId("TestProject");
      DocumentId documentId;
      string newFileName;
      Solution solution;

      solution = new AdhocWorkspace()
        .CurrentSolution
        .AddProject(projectId, "TestProject", "TestProject", LanguageNames.CSharp)
        .AddMetadataReference(projectId, CorlibReference)
        .AddMetadataReference(projectId, SystemCoreReference)
        .AddMetadataReference(projectId, CSharpSymbolsReference)
        .AddMetadataReference(projectId, CodeAnalysisReference);

      int count = 0;

      foreach (string source in sources)
      {
        newFileName = "Test" + count + ".cs";
        documentId = DocumentId.CreateNewId(projectId, newFileName);
        solution = solution.AddDocument(documentId, newFileName, SourceText.From(source));
        count++;
      }

      return solution.GetProject(projectId);
    }

    /// <summary>General method that gets a collection of actual diagnostics found
    /// in the source after the analyzer is run, then verifies each of them.</summary>
    /// <param name="sources">An array of strings to create source documents from to run the analyzers on.</param>
    /// <param name="analyzer">The analyzer to be run on the source code.</param>
    /// <param name="expected">DiagnosticResults that should appear after the analyzer is run on the sources.</param>
    private void VerifyDiagnostics(string[] sources, DiagnosticAnalyzer analyzer, params DiagnosticResult[] expected)
    {
      Diagnostic[] diagnostics = GetSortedDiagnostics(sources, analyzer);
      VerifyDiagnosticResults(diagnostics, analyzer, expected);
    }

    #endregion
  }
}

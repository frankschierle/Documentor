namespace Schierle.Documentor.Analyzers
{
  using System.Collections.Immutable;

  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.Diagnostics;

  /// <summary>Abstract base class for all analyzers.</summary>
  public abstract class AnalyzerBase : DiagnosticAnalyzer
  {
    #region Public Properties

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
      get
      {
        DiagnosticDescriptor descriptor = this.CreateDiagnosticDescriptor();

        return ImmutableArray.Create(descriptor);
      }
    }

    /// <summary>Gets the id of the analyzer.</summary>
    public abstract string Id { get; }

    /// <summary>Gets the title of the analyzer.</summary>
    public abstract string Title { get; }

    /// <summary>Gets the description of the analyzer.</summary>
    public abstract string Description { get; }

    /// <summary>Gets the default severity of the analyzer.</summary>
    public abstract DiagnosticSeverity DefaultSeverity { get; }

    #endregion

    #region Properties

    /// <summary>Gets the format message string for diagnostics created by this analyzer.</summary>
    protected abstract string Message { get; }

    #endregion

    #region Public Methods and Operators

    /// <inheritdoc />
    public abstract override void Initialize(AnalysisContext context);

    #endregion

    #region Methods

    /// <summary>Creates a new <see cref="Diagnostic" /> for the analyzer.</summary>
    /// <param name="location">The primary location of the diagnostic. If null, <see cref="Diagnostic.Location" /> will return
    /// <see cref="Location.None" />.</param>
    /// <param name="messageArgs">Arguments to the message of the diagnostic.</param>
    /// <returns>A new <see cref="Diagnostic" /> that can be reported using the analysis context.</returns>
    protected Diagnostic CreateDiagnostic(Location location, params object[] messageArgs)
    {
      DiagnosticDescriptor descriptor = this.CreateDiagnosticDescriptor();

      return Diagnostic.Create(descriptor, location, messageArgs);
    }

    /// <summary>Creates a new <see cref="DiagnosticDescriptor" /> for the analyzer.</summary>
    /// <returns>A new <see cref="DiagnosticDescriptor" /> for the analyzer.</returns>
    private DiagnosticDescriptor CreateDiagnosticDescriptor()
    {
      return new DiagnosticDescriptor(
        this.Id,
        this.Title,
        this.Message,
        "Documentation Rules",
        this.DefaultSeverity,
        true,
        this.Description);
    }

    #endregion
  }
}

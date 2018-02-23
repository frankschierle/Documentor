namespace Schierle.Documentor.Test
{
  using System;

  using Microsoft.CodeAnalysis;

  /// <summary>Location where the diagnostic appears, as determined by path,
  /// line number, and column number.</summary>
  public struct DiagnosticResultLocation
  {
    #region Constructors and Destructors

    public DiagnosticResultLocation(string path, int line, int column)
    {
      if (line < -1)
      {
        throw new ArgumentOutOfRangeException(nameof(line), "line must be >= -1");
      }

      if (column < -1)
      {
        throw new ArgumentOutOfRangeException(nameof(column), "column must be >= -1");
      }

      this.Path = path;
      this.Line = line;
      this.Column = column;
    }

    #endregion

    #region Public Properties

    /// <summary>Gets the path where the diagnostic appears.</summary>
    public string Path { get; }

    /// <summary>Gets the line where the diagnostic appears.</summary>
    public int Line { get; }

    /// <summary>Gets the column where the diagnostic appears.</summary>
    public int Column { get; }

    #endregion
  }

  /// <summary>Struct that stores information about a Diagnostic appearing in a source.</summary>
  public struct DiagnosticResult
  {
    #region Fields

    /// <summary>All locations, the disgnostics appearing.</summary>
    private DiagnosticResultLocation[] locations;

    #endregion

    #region Public Properties

    /// <summary>Gets or sets all locations, the disgnostics appearing.</summary>
    public DiagnosticResultLocation[] Locations
    {
      get
      {
        if (this.locations == null)
        {
          this.locations = new DiagnosticResultLocation[] { };
        }

        return this.locations;
      }

      set { this.locations = value; }
    }

    /// <summary>Gets or sets the severity.</summary>
    public DiagnosticSeverity Severity { get; set; }

    /// <summary>Gets or sets the id.</summary>
    public string Id { get; set; }

    /// <summary>Gets or sets the message.</summary>
    public string Message { get; set; }

    /// <summary>Gets the path.</summary>
    public string Path
    {
      get { return this.Locations.Length > 0 ? this.Locations[0].Path : ""; }
    }

    /// <summary>Gets the line.</summary>
    public int Line
    {
      get { return this.Locations.Length > 0 ? this.Locations[0].Line : -1; }
    }

    /// <summary>Gets or sets the column.</summary>
    public int Column
    {
      get { return this.Locations.Length > 0 ? this.Locations[0].Column : -1; }
    }

    #endregion
  }
}

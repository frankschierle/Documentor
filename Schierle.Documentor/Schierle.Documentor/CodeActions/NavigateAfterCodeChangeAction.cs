namespace Schierle.Documentor.CodeActions
{
  using System;
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;

  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.CodeActions;

  /// <summary>A code action to apply a code change and navigate to a specified
  /// code location afterwards.</summary>
  internal class NavigateAfterCodeChangeAction : CodeAction
  {
    #region Fields

    /// <summary>The operation that changes the code.</summary>
    private readonly Func<CancellationToken, Task<Solution>> codeChangeOperation;

    /// <summary>The method that calculates the navigation target.</summary>
    private readonly Func<Solution, CancellationToken, Task<NavigationTarget>> navigationTargetCalculation;

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="NavigateAfterCodeChangeAction"/> class.</summary>
    /// <param name="title">A short title describing the action that may appear in a menu.</param>
    /// <param name="codeChangeOperation">The operation that changes the code.</param>
    /// <param name="navigationTargetCalculation">The method that calculates the navigation target.</param>
    /// <exception cref="ArgumentNullException">Occurs if any of the parameters is null.</exception>
    public NavigateAfterCodeChangeAction(
      string title,
      Func<CancellationToken, Task<Solution>> codeChangeOperation,
      Func<Solution, CancellationToken, Task<NavigationTarget>> navigationTargetCalculation)
    {
      if (title == null)
      {
        throw new ArgumentNullException(nameof(title));
      }

      if (codeChangeOperation == null)
      {
        throw new ArgumentNullException(nameof(codeChangeOperation));
      }

      if (navigationTargetCalculation == null)
      {
        throw new ArgumentNullException(nameof(navigationTargetCalculation));
      }

      this.Title = title;
      this.codeChangeOperation = codeChangeOperation;
      this.navigationTargetCalculation = navigationTargetCalculation;
    }

    #endregion

    #region Public Properties

    /// <inheritdoc />
    public override string Title { get; }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override async Task<IEnumerable<CodeActionOperation>> ComputeOperationsAsync(CancellationToken cancellationToken)
    {
      var operations = new List<CodeActionOperation>();
      Solution changedSolution = await this.codeChangeOperation(cancellationToken);
      NavigationTarget navigationTarget = await this.navigationTargetCalculation(changedSolution, cancellationToken);

      operations.Add(new ApplyChangesOperation(changedSolution));

      if (navigationTarget != null)
      {
        operations.Add(new DocumentNavigationOperation(navigationTarget.DocumentId, navigationTarget.Position));
      }

      return operations;
    }

    #endregion
  }
}

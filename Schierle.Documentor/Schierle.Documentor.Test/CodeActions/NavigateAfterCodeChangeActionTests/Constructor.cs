namespace Schierle.Documentor.Test.CodeActions.NavigateAfterCodeChangeActionTests
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;

  using FluentAssertions;

  using Microsoft.CodeAnalysis;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Schierle.Documentor.CodeActions;

  public partial class NavigateAfterCodeChangeActionTest
  {
    #region Public Methods and Operators

    [TestMethod]
    [Description("Parameter title must not be null.")]
    public void NavigateAfterCodeChangeAction_TitleIsNull_Exception()
    {
      // Arrange
      string title = null;
      Func<CancellationToken, Task<Solution>> codeChangeOperation = c => null;
      Func<Solution, CancellationToken, Task<NavigationTarget>> navigationTargetCalculation = (s, c) => null;
      Action act;

      // Act
      act = () => new NavigateAfterCodeChangeAction(title, codeChangeOperation, navigationTargetCalculation);

      // Assert
      act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("title");
    }

    [TestMethod]
    [Description("Parameter codeChangeOperation must not be null.")]
    public void NavigateAfterCodeChangeAction_CodeChangeOperationIsNull_Exception()
    {
      // Arrange
      string title = "Title";
      Func<CancellationToken, Task<Solution>> codeChangeOperation = null;
      Func<Solution, CancellationToken, Task<NavigationTarget>> navigationTargetCalculation = (s, c) => null;
      Action act;

      // Act
      act = () => new NavigateAfterCodeChangeAction(title, codeChangeOperation, navigationTargetCalculation);

      // Assert
      act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("codeChangeOperation");
    }

    [TestMethod]
    [Description("Parameter navigationTargetCalculation must not be null.")]
    public void NavigateAfterCodeChangeAction_NavigationTargetCalculationIsNull_Exception()
    {
      // Arrange
      string title = "Title";
      Func<CancellationToken, Task<Solution>> codeChangeOperation = c => null;
      Func<Solution, CancellationToken, Task<NavigationTarget>> navigationTargetCalculation = null;
      Action act;

      // Act
      act = () => new NavigateAfterCodeChangeAction(title, codeChangeOperation, navigationTargetCalculation);

      // Assert
      act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("navigationTargetCalculation");
    }

    [TestMethod]
    [Description("Property Title should return the same value passed as ctor parameter.")]
    public void NavigateAfterCodeChangeAction_TitleIsValid_PropertyTitleReturnsSameObject()
    {
      // Arrange
      string title = "Title";
      Func<CancellationToken, Task<Solution>> codeChangeOperation = c => null;
      Func<Solution, CancellationToken, Task<NavigationTarget>> navigationTargetCalculation = (s, c) => null;
      string actualResult;
      string expectedResult = title;

      // Act
      actualResult = new NavigateAfterCodeChangeAction(title, codeChangeOperation, navigationTargetCalculation).Title;

      // Assert
      actualResult.Should().BeSameAs(expectedResult);
    }

    #endregion
  }
}

namespace Schierle.Documentor.Test.CodeActions.NavigationTargetTests
{
  using System;

  using FluentAssertions;

  using Microsoft.CodeAnalysis;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Schierle.Documentor.CodeActions;

  public partial class NavigationTargetTest
  {
    #region Public Methods and Operators

    [TestMethod]
    [Description("Parameter documentId must not be null.")]
    public void NavigationTarget_DocumentIdIsNull_Exception()
    {
      // Arrange
      DocumentId documentId = null;
      int position = 42;
      Action act;

      // Act
      act = () => new NavigationTarget(documentId, position);

      // Assert
      act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("documentId");
    }

    [TestMethod]
    [Description("Parameter position must not be less than zero.")]
    public void NavigationTarget_PositionIsLessThanZero_Exception()
    {
      // Arrange
      DocumentId documentId = DocumentId.CreateNewId(ProjectId.CreateNewId());
      int position = -1;
      Action act;

      // Act
      act = () => new NavigationTarget(documentId, position);

      // Assert
      act.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("position");
    }

    [TestMethod]
    [Description("Zero is a valid value for parameter position.")]
    public void NavigationTarget_PositionIsZero_NoException()
    {
      // Arrange
      DocumentId documentId = DocumentId.CreateNewId(ProjectId.CreateNewId());
      int position = 0;
      Action act;

      // Act
      act = () => new NavigationTarget(documentId, position);

      // Assert
      act.Should().NotThrow();
    }

    [TestMethod]
    [Description("Property DocumentId should return the same object passed as ctor parameter.")]
    public void NavigatioTarget_ValidDocumentId_PropertyDocumentIdReturnsSameObject()
    {
      // Arrange
      DocumentId documentId = DocumentId.CreateNewId(ProjectId.CreateNewId());
      int position = 42;
      DocumentId actualResult;
      DocumentId expectedResult = documentId;

      // Act
      actualResult = new NavigationTarget(documentId, position).DocumentId;

      // Assert
      actualResult.Should().BeSameAs(expectedResult);
    }

    [TestMethod]
    [Description("Property Position should return the same value passed as ctor parameter.")]
    public void NavigatioTarget_ValidPosition_PropertyPositionReturnsSameValue()
    {
      // Arrange
      DocumentId documentId = DocumentId.CreateNewId(ProjectId.CreateNewId());
      int position = 42;
      int actualResult;
      int expectedResult = position;

      // Act
      actualResult = new NavigationTarget(documentId, position).Position;

      // Assert
      actualResult.Should().Be(expectedResult);
    }

    #endregion
  }
}

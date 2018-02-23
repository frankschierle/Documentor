namespace Schierle.Documentor.Test.Utils.EnumerableExtensionsTests
{
  using System.Collections.Generic;

  using FluentAssertions;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Schierle.Documentor.Utils;

  public partial class EnumerableExtensionsTest
  {
    #region Public Methods and Operators

    [TestMethod]
    [Description("The method should return an IEnumerable that contains the given object.")]
    public void Yield_ValidObj_EnumerableContainsObj()
    {
      // Arrange
      string obj = "HelloWorld";
      IEnumerable<string> actualResult;
      string[] expectedResult = new[] { obj };

      // Act
      actualResult = obj.Yield();

      // Arrange
      actualResult.Should().Equal(expectedResult);
    }

    [TestMethod]
    [Description("Null is a supported value.")]
    public void Yield_ObjIsNull_EnumerableContainsNull()
    {
      //Arrange
      string obj = null;
      IEnumerable<string> actualResult;
      string[] expectedResult = new string[] { null };

      // Act
      actualResult = obj.Yield();

      // Assert
      actualResult.Should().Equal(expectedResult);
    }

    #endregion
  }
}

namespace Schierle.Documentor.Test.Analyzers.ElementsMustBeDocumentedTests
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  public partial class ElementsMustBeDocumentedTest
  {
    #region Public Methods and Operators

    [TestMethod]
    [Description("A diagnostic should be detected for a class declaration without documentation header.")]
    public void Initialize_ClassDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element 'TestClass' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 1, 14);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = "public class TestClass { }";

      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("A diagnostic should be detected for a constructor declaration without documentation header.")]
    public void Initialize_ConstructorDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element 'TestClass' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 5, 10);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
  public TestClass() { }
}";

      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("A diagnostic should be detected for a delegate declaration without documentation header.")]
    public void Initialize_DelegateDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element 'TestDelegate' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 5, 24);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
  public delegate void TestDelegate();
}";

      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("A diagnostic should be detected for a destructor declaration without documentation header.")]
    public void Initialize_DestructorDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element 'TestClass' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 5, 4);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
  ~TestClass() { }
}";

      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("A diagnostic should be detected for an enum declaration without documentation header.")]
    public void Initialize_EnumDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element 'TestEnum' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 1, 13);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = @"public enum TestEnum { }";

      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("A diagnostic should be detected for an enum member declaration without documentation header.")]
    public void Initialize_EnumMemberDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element 'TestMember' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 5, 3);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = @"
/// <summary>The summary.</summary>
public enum TestEnum
{
  TestMember = 1,
}";
      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("A diagnostic should be detected for an event declaration without documentation header.")]
    public void Initialize_EventDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element 'TestEvent' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 5, 29);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
  public event EventHandler TestEvent { add { } remove { } }
}";
      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("A diagnostic should be detected for an event field declaration without documentation header.")]
    public void Initialize_EventFieldDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element 'TestEventField' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 5, 29);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
  public event EventHandler TestEventField;
}";
      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("A diagnostic should be detected for a field declaration without documentation header.")]
    public void Initialize_FieldDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element 'TestField' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 5, 14);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
  public int TestField;
}";
      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("A diagnostic should be detected for an indexer declaration without documentation header.")]
    public void Initialize_IndexerDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element 'this' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 5, 14);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
  public int this[int i] { set {}  }
}";
      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("A diagnostic should be detected for an interface declaration without documentation header.")]
    public void Initialize_InterfaceDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element 'TestInterface' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 1, 18);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = "public interface TestInterface { }";

      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("A diagnostic should be detected for a method declaration without documentation header.")]
    public void Initialize_MethodDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element 'TestMethod' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 5, 15);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
  public void TestMethod() { }
}";
      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("A diagnostic should be detected for an operator declaration without documentation header.")]
    public void Initialize_OperatorDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element '+' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 5, 36);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
  public static TestClass operator +(TestClass a, TestClass b) { return null; }
}";
      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("A diagnostic should be detected for a property declaration without documentation header.")]
    public void Initialize_PropertyDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element 'TestProperty' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 5, 14);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
  public int TestProperty { get; set; }
}";
      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("A diagnostic should be detected for a struct declaration without documentation header.")]
    public void Initialize_StructDeclarationWithoutDocumentationHeader_DiagnosticDetected()
    {
      // Arrange
      string expectedMessage = "Element 'TestStruct' should be documented.";
      var expectedLocation = new DiagnosticResultLocation("Test0.cs", 1, 15);
      DiagnosticResult expectedResult = this.CreateDiagnosticResult(expectedMessage, expectedLocation);
      string sourceCode = "public struct TestStruct { }";

      // Act + Assert
      this.VerifyCSharpDiagnostic(sourceCode, expectedResult);
    }

    #endregion
  }
}

namespace Schierle.Documentor.Test.CodeFixProviders.ElementsMustBeDocumentedCodeFixProviderTests
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  public partial class ElementsMustBeDocumentedCodeFixProviderTest
  {
    #region Public Methods and Operators

    [TestMethod]
    [Description("The code fix should add a summary tag to a class declaration without documentation header.")]
    public void RegisterCodeFixesAsync_ClassDeclarationWithoutDocumentationHeader_SummaryAdded()
    {
      // Arrange
      string sourceCode = "public class TestClass { }";
      string expectedResult = @"/// <summary></summary>
public class TestClass { }";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary and typeparam tag to a generic class declaration without documentation header.")]
    public void RegisterCodeFixesAsync_GenericClassDeclarationWithoutDocumentationHeader_SummaryAndTypeParamAdded()
    {
      // Arrange
      string sourceCode = "public class TestClass<T> { }";
      string expectedResult = @"/// <summary></summary>
/// <typeparam name=""T""></typeparam>
public class TestClass<T> { }";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary tag to a constructor declaration without documentation header.")]
    public void RegisterCodeFixesAsync_ConstructorDeclarationWithoutDocumentationHeader_SummaryAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public TestClass() { }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    public TestClass() { }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary tag to a delegate declaration without documentation header.")]
    public void RegisterCodeFixesAsync_DelegateDeclarationWithoutDocumentationHeader_SummaryAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public delegate void TestDelegate();
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    public delegate void TestDelegate();
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary and param tag to a delegate declaration without documentation header.")]
    public void RegisterCodeFixesAsync_DelegateDeclarationWithParameterWithoutDocumentationHeader_SummaryAndParamAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public delegate void TestDelegate(int i);
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <param name=""i""></param>
    public delegate void TestDelegate(int i);
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary and param tag to a delegate declaration without documentation header.")]
    public void RegisterCodeFixesAsync_DelegateDeclarationWithMultipleParametersWithoutDocumentationHeader_SummaryAndParamAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public delegate void TestDelegate(int i, int j);
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <param name=""i""></param>
    /// <param name=""j""></param>
    public delegate void TestDelegate(int i, int j);
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary and a returns tag to a non void delegate declaration without documentation header.")]
    public void RegisterCodeFixesAsync_NonVoidDelegateDeclarationWithoutDocumentationHeader_SummaryAndReturnsAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public delegate int TestDelegate();
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <returns></returns>
    public delegate int TestDelegate();
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary, param and a returns tag to a non void delegate declaration without documentation header.")]
    public void RegisterCodeFixesAsync_NonVoidDelegateDeclarationWithParametersWithoutDocumentationHeader_SummaryParamAndReturnsAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public delegate int TestDelegate(int i);
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <param name=""i""></param>
    /// <returns></returns>
    public delegate int TestDelegate(int i);
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary, param and a returns tag to a non void delegate declaration without documentation header.")]
    public void RegisterCodeFixesAsync_NonVoidDelegateDeclarationWithMultipleParametersWithoutDocumentationHeader_SummaryParamAndReturnsAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public delegate int TestDelegate(int i, int j);
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <param name=""i""></param>
    /// <param name=""j""></param>
    /// <returns></returns>
    public delegate int TestDelegate(int i, int j);
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary tag to a destructor declaration without documentation header.")]
    public void RegisterCodeFixesAsync_DestructorDeclarationWithoutDocumentationHeader_SummaryAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    ~TestClass() { }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    ~TestClass() { }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary tag to an enum declaration without documentation header.")]
    public void RegisterCodeFixesAsync_EnumDeclarationWithoutDocumentationHeader_SummaryAdded()
    {
      // Arrange
      string sourceCode = "public enum TestEnum { }";
      string expectedResult = @"/// <summary></summary>
public enum TestEnum { }";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary tag to an enum member declaration without documentation header.")]
    public void RegisterCodeFixesAsync_EnumMemberDeclarationWithoutDocumentationHeader_SummaryAdded()
    {
      // Arrange
      string sourceCode = @"/// <summary></summary>
public enum TestEnum {
    TestMember = 1,
}";

      string expectedResult = @"/// <summary></summary>
public enum TestEnum
{
    /// <summary></summary>
    TestMember = 1,
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary tag to an event declaration without documentation header.")]
    public void RegisterCodeFixesAsync_EventDeclarationWithoutDocumentationHeader_SummaryAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public event EventHandler TestEvent { add { } remove { } }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    public event EventHandler TestEvent { add { } remove { } }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary tag to an event field declaration without documentation header.")]
    public void RegisterCodeFixesAsync_EventFieldDeclarationWithoutDocumentationHeader_SummaryAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public event EventHandler TestEventField;
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    public event EventHandler TestEventField;
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary tag to a field declaration without documentation header.")]
    public void RegisterCodeFixesAsync_FieldDeclarationWithoutDocumentationHeader_SummaryAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public int TestField;
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    public int TestField;
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary, param and returns tag to an indexer declaration without documentation header.")]
    public void RegisterCodeFixesAsync_IndexerDeclarationWithoutDocumentationHeader_SummaryParamAndReturnsAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public int this[int i] { get; set; }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <param name=""i""></param>
    /// <returns></returns>
    public int this[int i] { get; set; }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary, param and returns tag to an indexer declaration without documentation header.")]
    public void RegisterCodeFixesAsync_IndexerDeclarationWithMultipleParamsWithoutDocumentationHeader_SummaryParamAndReturnsAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public int this[int i, int j] { get; set; }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <param name=""i""></param>
    /// <param name=""j""></param>
    /// <returns></returns>
    public int this[int i, int j] { get; set; }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary and param tag to an indexer declaration without documentation header.")]
    public void RegisterCodeFixesAsync_SetterOnlyIndexerDeclarationWithoutDocumentationHeader_SummaryAndParamAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public int this[int i] { set; }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <param name=""i""></param>
    public int this[int i] { set; }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary and param tag to an indexer declaration without documentation header.")]
    public void RegisterCodeFixesAsync_SetterOnlyIndexerDeclarationWithMultipleParamsWithoutDocumentationHeader_SummaryAndParamAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public int this[int i, int j] { set; }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <param name=""i""></param>
    /// <param name=""j""></param>
    public int this[int i, int j] { set; }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary tag to an interface declaration without documentation header.")]
    public void RegisterCodeFixesAsync_InterfaceDeclarationWithoutDocumentationHeader_SummaryAdded()
    {
      // Arrange
      string sourceCode = "public interface TestInterface { }";
      string expectedResult = @"/// <summary></summary>
public interface TestInterface { }";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary and typeparam tag to a generic interface declaration without documentation header.")]
    public void RegisterCodeFixesAsync_GenericInterfaceDeclarationWithoutDocumentationHeader_SummaryAndTypeParamAdded()
    {
      // Arrange
      string sourceCode = "public interface TestInterface<T> { }";
      string expectedResult = @"/// <summary></summary>
/// <typeparam name=""T""></typeparam>
public interface TestInterface<T> { }";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary tag to a method declaration without documentation header.")]
    public void RegisterCodeFixesAsync_MethodDeclarationWithoutDocumentationHeader_SummaryAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public void TestMethod() { }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    public void TestMethod() { }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary and param tag to a method declaration without documentation header.")]
    public void RegisterCodeFixesAsync_MethodDeclarationWithParameterWithoutDocumentationHeader_SummaryAndParamAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public void TestMethod(int i) { }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <param name=""i""></param>
    public void TestMethod(int i) { }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary and param tag to a method declaration without documentation header.")]
    public void RegisterCodeFixesAsync_MethodDeclarationWithMultipleParametersWithoutDocumentationHeader_SummaryAndParamAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public void TestMethod(int i, int j) { }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <param name=""i""></param>
    /// <param name=""j""></param>
    public void TestMethod(int i, int j) { }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary and typeparam tag to a method declaration without documentation header.")]
    public void RegisterCodeFixesAsync_GenericMethodDeclarationWithoutDocumentationHeader_SummaryAndTypeparamAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public void TestMethod<T>() { }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <typeparam name=""T""></typeparam>
    public void TestMethod<T>() { }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary, typeparam and param tag to a method declaration without documentation header.")]
    public void RegisterCodeFixesAsync_GenericMethodDeclarationWithParameterWithoutDocumentationHeader_SummaryTypeparamAndParamAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public void TestMethod<T>(int i) { }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <typeparam name=""T""></typeparam>
    /// <param name=""i""></param>
    public void TestMethod<T>(int i) { }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary, typeparam and param tag to a method declaration without documentation header.")]
    public void RegisterCodeFixesAsync_GenericMethodDeclarationWithMultipleParametersWithoutDocumentationHeader_SummaryTypeparamAndParamAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public void TestMethod<T>(int i, int j) { }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <typeparam name=""T""></typeparam>
    /// <param name=""i""></param>
    /// <param name=""j""></param>
    public void TestMethod<T>(int i, int j) { }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary and returns tag to a method declaration without documentation header.")]
    public void RegisterCodeFixesAsync_NonVoidMethodDeclarationWithoutDocumentationHeader_SummaryAndReturnsAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public int TestMethod() { return 42; }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <returns></returns>
    public int TestMethod() { return 42; }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary, param and returns tag to a method declaration without documentation header.")]
    public void RegisterCodeFixesAsync_NonVoidMethodDeclarationWithParameterWithoutDocumentationHeader_SummaryParamAndReturnsAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public int TestMethod(int i) { return 42; }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <param name=""i""></param>
    /// <returns></returns>
    public int TestMethod(int i) { return 42; }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary, param and returns tag to a method declaration without documentation header.")]
    public void RegisterCodeFixesAsync_NonVoidMethodDeclarationWithMultipleParametersWithoutDocumentationHeader_SummaryParamAndReturnsAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public int TestMethod(int i, int j) { return 42; }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <param name=""i""></param>
    /// <param name=""j""></param>
    /// <returns></returns>
    public int TestMethod(int i, int j) { return 42; }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary, typeparam and returns tag to a method declaration without documentation header.")]
    public void RegisterCodeFixesAsync_GenericNonVoidMethodDeclarationWithoutDocumentationHeader_SummaryTypeparamAndReturnsAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public int TestMethod<T>() { return 42; }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <typeparam name=""T""></typeparam>
    /// <returns></returns>
    public int TestMethod<T>() { return 42; }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary, typeparam, param and returns tag to a method declaration without documentation header.")]
    public void RegisterCodeFixesAsync_GenericNonVoidMethodDeclarationWithParameterWithoutDocumentationHeader_SummaryTypeparamParamAndReturnsAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public int TestMethod<T>(int i) { return 42; }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <typeparam name=""T""></typeparam>
    /// <param name=""i""></param>
    /// <returns></returns>
    public int TestMethod<T>(int i) { return 42; }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary, typeparam, param and returns tag to a method declaration without documentation header.")]
    public void RegisterCodeFixesAsync_GenericNonVoidMethodDeclarationWithMultipleParametersWithoutDocumentationHeader_SummaryTypeparamParamAndReturnsAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public int TestMethod<T>(int i, int j) { return 42; }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <typeparam name=""T""></typeparam>
    /// <param name=""i""></param>
    /// <param name=""j""></param>
    /// <returns></returns>
    public int TestMethod<T>(int i, int j) { return 42; }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary tag to an operator declaration without documentation header.")]
    public void RegisterCodeFixesAsync_OperatorDeclarationWithoutDocumentationHeader_SummaryParamAndReturnsAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public TestClass operator +(TestClass a, TestClass b) { return null; }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    /// <param name=""a""></param>
    /// <param name=""b""></param>
    /// <returns></returns>
    public TestClass operator +(TestClass a, TestClass b) { return null; }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix provider should add a summary tag to a property declaration without documentation header.")]
    public void RegisterCodeFixesAsync_PropertyDeclarationWithoutDocumentationHeader_SummaryAdded()
    {
      // Arrange
      string sourceCode = @"
/// <summary>The summary.</summary>
public class TestClass
{
    public int TestProperty { get; set; }
}";

      string expectedResult = @"
/// <summary>The summary.</summary>
public class TestClass
{
    /// <summary></summary>
    public int TestProperty { get; set; }
}";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary tag to a struct declaration without documentation header.")]
    public void RegisterCodeFixesAsync_StructDeclarationWithoutDocumentationHeader_SummaryAdded()
    {
      // Arrange
      string sourceCode = "public struct TestStruct { }";
      string expectedResult = @"/// <summary></summary>
public struct TestStruct { }";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    [TestMethod]
    [Description("The code fix should add a summary and typeparam tag to a struct declaration without documentation header.")]
    public void RegisterCodeFixesAsync_GenericStructDeclarationWithoutDocumentationHeader_SummaryAndTypeparamAdded()
    {
      // Arrange
      string sourceCode = "public struct TestStruct<T> { }";
      string expectedResult = @"/// <summary></summary>
/// <typeparam name=""T""></typeparam>
public struct TestStruct<T> { }";

      // Act + Assert
      this.VerifyCodeFix(sourceCode, expectedResult);
    }

    #endregion
  }
}

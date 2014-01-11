namespace Sazare.Tools.Cui
{
  using System;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class EmptyInputValidatorTest
  {
    [TestMethod]
    public void 空文字は検証OKとなる()
    {
      // Arrange
      var sut = new EmptyInputValidator();

      // Act
      // Assert
      Assert.IsTrue(sut.Validate(string.Empty));
    }

    [TestMethod]
    public void 空文字以外は検証NGとなる()
    {
      // Arrange
      var sut = new EmptyInputValidator();

      // Act
      // Assert
      Assert.IsFalse(sut.Validate("dummy"));
    }
  }
}

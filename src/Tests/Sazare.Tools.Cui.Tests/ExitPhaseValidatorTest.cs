using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sazare.Tools.Cui
{
  [TestClass]
  public class ExitPhaseValidatorTest
  {
    [TestMethod]
    public void 終了を示す文字列を判定できる_大文字()
    {
      // Arrange
      var sut = new ExitPhaseValidator();

      // Act
      // Assert
      Assert.IsTrue(sut.Validate("EXIT"));
    }

    [TestMethod]
    public void 終了を示す文字列を判定できる_小文字()
    {
      // Arrange
      var sut = new ExitPhaseValidator();

      // Act
      // Assert
      Assert.IsTrue(sut.Validate("exit"));
    }

    [TestMethod]
    public void 終了を示す文字列を判定できる_終了文字以外()
    {
      // Arrange
      var sut = new ExitPhaseValidator();

      // Act
      // Assert
      Assert.IsFalse(sut.Validate("dummy"));
    }
  }
}

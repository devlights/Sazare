namespace Sazare.Tools.Cui
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.IO;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class CuiInputManagerTest
  {
    TextReader _consoleIn;

    [TestInitialize]
    public void Initialize()
    {
      _consoleIn = Console.In;
    }

    [TestCleanup]
    public void Cleanup()
    {
      Console.SetIn(_consoleIn);
    }

    [TestMethod]
    public void 入力したデータが一文字読み込める()
    {
      // Arrange
      var reader = new StringReader("test data\n");
      Console.SetIn(reader);

      var manager = new CuiInputManager();

      // Act
      var data = manager.Read();

      // Assert
      Assert.AreEqual((int) 't', data);
    }

    [TestMethod]
    public void 入力したデータが一行読み込める()
    {
      // Arrange
      var reader = new StringReader("test data\n");
      Console.SetIn(reader);

      var manager = new CuiInputManager();

      // Act
      var data = manager.ReadLine();

      // Assert
      Assert.AreEqual("test data", data);
    }
  }
}

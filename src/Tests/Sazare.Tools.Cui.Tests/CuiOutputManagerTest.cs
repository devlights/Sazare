namespace Sazare.Tools.Cui
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.IO;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class CuiOutputManagerTest
  {
    TextWriter _consoleOut;

    [TestInitialize]
    public void Initialize()
    {
      _consoleOut = Console.Out;
    }

    [TestCleanup]
    public void Cleanup()
    {
      Console.SetOut(_consoleOut);
    }

    [TestMethod]
    public void Write_Success_指定したデータが書き込まれているか確認()
    {
      var writer = new StringWriter();
      Console.SetOut(writer);

      // Arrange
      var outputManager = new CuiOutputManager();

      // Act
      var message = "this is dummy message";
      outputManager.Write(message);

      // Assert
      Assert.AreEqual<string>(message, writer.ToString());
    }

    [TestMethod]
    public void WriteLine_Success_指定したデータが書き込まれているか確認()
    {
      var writer = new StringWriter();
      Console.SetOut(writer);

      // Arrange
      var outputManager = new CuiOutputManager();

      // Act
      var message = "this is dummy message";
      outputManager.WriteLine(message);

      // Assert
      Assert.AreEqual<string>(string.Format("{0}{1}", message, Environment.NewLine), writer.ToString());
    }
  }
}

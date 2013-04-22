namespace Sazare.Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.IO;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class OutputTest
  {
    [TestCleanup]
    public void Cleanup()
    {
      Output.SetOutputManager(null);
    }

    [TestMethod]
    public void SetOutputManager_Success_IOutputManagerが設定できることを確認()
    {
      // Arrange
      var outputManager = new DummyOutputManager();

      // Act
      Output.SetOutputManager(outputManager);

      // Assert
      //  エラーが出ていなければOK
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Write_Fail_IOutputManagerを設定していない状態で実行()
    {
      // Arrange
      Output.SetOutputManager(null);

      // Act
      Output.Write("this is dummy message");

      // Assert
      Assert.Fail("例外が発生していない");
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void WriteLine_Fail_IOutputManagerを設定していない状態で実行()
    {
      // Arrange
      Output.SetOutputManager(null);

      // Act
      Output.WriteLine("this is dummy message");

      // Assert
      Assert.Fail("例外が発生していない");
    }

    [TestMethod]
    public void Write_Success_指定したデータが書き込まれているか確認()
    {
      // Arrange
      var writer = new StringWriter();
      var outputManager = new StringWriterOutputManager(writer);
      Output.SetOutputManager(outputManager);

      // Act
      string message = "this is dummy message";
      Output.Write(message);

      // Assert
      Assert.AreEqual<string>(message, writer.ToString());
    }

    [TestMethod]
    public void WriteLine_Success_指定したデータが書き込まれている確認()
    {
      // Arrange
      var writer = new StringWriter();
      var outputManager = new StringWriterOutputManager(writer);
      Output.SetOutputManager(outputManager);

      // Act
      var message = "this is dummy message";
      Output.WriteLine(message);

      // Assert
      Assert.AreEqual<string>(String.Format("{0}{1}", message, Environment.NewLine), writer.ToString());
    }
  }

  class DummyOutputManager : IOutputManager
  {
    public void Write(object data)
    {
      throw new NotImplementedException();
    }

    public void WriteLine(object data)
    {
      throw new NotImplementedException();
    }
  }

  class StringWriterOutputManager : IOutputManager
  {
    StringWriter _writer;

    public StringWriterOutputManager(StringWriter writer)
    {
      _writer = writer;
    }

    public void Write(object data)
    {
      _writer.Write(data);
    }

    public void WriteLine(object data)
    {
      _writer.WriteLine(data);
    }
  }
}

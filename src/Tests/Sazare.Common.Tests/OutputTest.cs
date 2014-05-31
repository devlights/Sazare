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
      Output.OutputManager = null;
    }

    [TestMethod]
    public void SetOutputManager_Success_IOutputManagerが設定できることを確認()
    {
      // Arrange
      var outputManager = new DummyOutputManager();

      // Act
      Output.OutputManager = outputManager;

      // Assert
      //  エラーが出ていなければOK
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Write_Fail_IOutputManagerを設定していない状態で実行()
    {
      // Arrange
      Output.OutputManager = null;

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
      Output.OutputManager = null;

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
      Output.OutputManager = outputManager;

      // Act
      string message = "this is dummy message";
      Output.Write(message);

      // Assert
      Assert.AreEqual<string>(message, writer.ToString());
    }

    [TestMethod]
    public void WriteLine_Success_指定したデータが書き込まれているか確認()
    {
      // Arrange
      var writer = new StringWriter();
      var outputManager = new StringWriterOutputManager(writer);
      Output.OutputManager = outputManager;

      // Act
      var message = "this is dummy message";
      Output.WriteLine(message);

      // Assert
      Assert.AreEqual<string>(String.Format("{0}{1}", message, Environment.NewLine), writer.ToString());
    }

    [TestMethod]
    public void Write_WithFormat_Success_指定したデータが書き込まれているか確認()
    {
      // Arrange
      var writer = new StringWriter();
      var outputManager = new StringWriterOutputManager(writer);
      Output.OutputManager = outputManager;

      // Act
      var format = "unit_{0}_test";
      var message = "this is dummy message";
      Output.Write(format, message);

      // Assert
      Assert.AreEqual<string>(string.Format(format, message), writer.ToString());
    }

    [TestMethod]
    public void WriteLine_WithFormat_Success_指定したデータが書き込まれているか確認()
    {
      // Arrange
      var writer = new StringWriter();
      var outputManager = new StringWriterOutputManager(writer);
      Output.OutputManager = outputManager;

      // Act
      var format = "unit_{0}_test";
      var message = "this is dummy message";
      Output.WriteLine(format, message);

      // Assert
      Assert.AreEqual<string>(string.Format("{0}{1}", string.Format(format, message), Environment.NewLine), writer.ToString());
    }

    [TestMethod]
    public void Write_and_WriteLine_文字列以外の場合の確認()
    {
      // Arrange
      var writer = new StringWriter();
      var outputManager = new StringWriterOutputManager(writer);
      Output.OutputManager = outputManager;

      // Act
      var value = 100;
      Output.Write(value);

      // Assert
      Assert.AreEqual<string>(value.ToString(), writer.ToString());

      //
      // ----------------------------------------------------------------
      //

      // Arrange2
      writer = new StringWriter();
      outputManager = new StringWriterOutputManager(writer);
      Output.OutputManager = outputManager;

      // Act2
      Output.WriteLine(value);

      // Assert2
      Assert.AreEqual<string>(string.Format("{0}{1}", value.ToString(), Environment.NewLine), writer.ToString());
    }
  }

  class DummyOutputManager : IOutputManager
  {
    public void Write(object data)
    {
    }

    public void WriteLine(object data)
    {
    }

    public Stream OutStream
    {
      get
      {
        throw new NotImplementedException();
      }
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

    public Stream OutStream
    {
      get
      {
        throw new NotImplementedException();
      }
    }
  }
}

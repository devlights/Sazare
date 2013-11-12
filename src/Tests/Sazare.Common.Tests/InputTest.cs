namespace Sazare.Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.IO;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class InputTest
  {
    [TestCleanup]
    public void Cleanup()
    {
      Input.InputManager = null;
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void InputManagerを設定していない状態で操作すると例外が発生する()
    {
      // Arrange
      // Act
      Input.Read();

      // Assert
      Assert.Fail("例外が発生していない");
    }

    [TestMethod]
    public void InputManagerを設定することができる()
    {
      // Arrange
      // Act
      Input.InputManager = new MockInputManager();

      // Assert
    }

    [TestMethod]
    public void Readメソッドでデータを取得することができる()
    {
      // Arrange
      Input.InputManager = new MockInputManager();

      // Act
      object data = Input.Read();

      // Assert
      Assert.AreEqual(48, data);
    }

    [TestMethod]
    public void ReadLineメソッドでデータを取得することができる()
    {
      // Arrange
      Input.InputManager = new MockInputManager();

      // Assert
      object data = Input.ReadLine();

      // Act
      Assert.AreEqual("test data\n", data);
    }

    class MockInputManager : IInputManager
    {
      public object Read()
      {
        return 48;
      }

      public object ReadLine()
      {
        return string.Format("{0}\n", "test data", Read());
      }
    }
  }
}

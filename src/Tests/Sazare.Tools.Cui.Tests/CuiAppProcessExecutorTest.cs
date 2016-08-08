namespace Sazare.Tools.Cui
{
  using System;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Sazare.Common;

  [TestClass]
  public class CuiAppProcessExecutorTest
  {
    [TestInitialize]
    public void Initialize()
    {
      Output.OutputManager = new CuiOutputManager();
    }

    [TestCleanup]
    public void Cleanup()
    {
      Output.OutputManager = null;
    }

    /// <summary>
    /// 仕様：引数にNULLは渡せない。
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Execute_Nullを渡した場合例外が発生することを確認()
    {
      // Arrange
      var target = new CuiAppProcessExecutor();

      // Act
      target.Execute(null);

      // Assert
      Assert.Fail("例外が発生していない");
    }

    /// <summary>
    /// 仕様：引数に指定されたIExecutableのExecuteメソッドをコールする.
    /// </summary>
    [TestMethod]
    public void Execute_引数で指定されたIExecutableのExecuteメソッドが呼ばれることを確認()
    {
      // Arrange
      var executable = new ExecutableForUnitTest();
      var target     = new CuiAppProcessExecutor();

      // Act
      target.Execute(executable);

      // Assert
      Assert.IsTrue(executable.ExecuteCalled);
    }

    /// <summary>
    /// 仕様：Executableの実行前と実行後にSTART、ENDログを出力する。
    /// </summary>
    [TestMethod]
    public void Execute_Executableの実行前と実行後にSTARTとENDログが出力されることを確認()
    {
      // Arrange
      var outputManager = new FirstLastRememberOutputManager();
      Output.OutputManager = outputManager;

      var executable = new ExecutableForUnitTest();
      var target = new CuiAppProcessExecutor();

      // Act
      target.Execute(executable);

      // Assert
      Assert.AreEqual<string>(target.StartLogMessage, outputManager.First.ToString());
      Assert.AreEqual<string>(target.EndLogMessage, outputManager.Last.ToString());
    }
  }
  
  /// <summary>
  /// UnitTest用 IExecutable実装クラス
  /// </summary>
  class ExecutableForUnitTest : IExecutable
  {
    public bool ExecuteCalled { get; set; }

    public ExecutableForUnitTest()
    {
      ExecuteCalled = false;  
    }

    public void Execute()
    {
      ExecuteCalled = true;
    }
  }

  class FirstLastRememberOutputManager : IOutputManager
  {
    public object First { get; set; }
    public object Last  { get; set; }

    public void Write(object data)
    {
      SetData(data);
    }

    public void WriteLine(object data)
    {
      SetData(data);
    }

    private void SetData(object data)
    {
      if (First == null)
      {
        First = data;
        return;
      }

      if (Last == null)
      {
        Last = data;
        return;
      }
    }

    public System.IO.Stream OutStream
    {
      get
      {
        throw new NotImplementedException();
      }
    }
  }
}

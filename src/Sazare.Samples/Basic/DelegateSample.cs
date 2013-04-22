namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows.Forms;

  using Sazare.Common;
  
  #region デリゲートのサンプル (.net framework 1.1)
  /// <summary>
  /// デリゲートのサンプル（.NET Framework 1.1）
  /// </summary>
  [Sample]
  class DelegateSample : Sazare.Common.IExecutable
  {
    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      MethodInvoker methodInvoker = new MethodInvoker(DelegateMethod);
      methodInvoker();
    }

    /// <summary>
    /// デリゲートメソッド.
    /// </summary>
    private void DelegateMethod()
    {
      Output.WriteLine("SAMPLE_DELEGATE_METHOD.");
    }
  }
  #endregion
}

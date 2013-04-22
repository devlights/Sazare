namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows.Forms;

  using Sazare.Common;
  
  #region ラムダのサンプル (.net framework 3.5)
  /// <summary>
  /// ラムダ(lambda)のサンプル（.NET Framework 3.5）
  /// </summary>
  [Sample]
  class LambdaSample : Sazare.Common.IExecutable
  {
    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      MethodInvoker methodInvoker = () =>
      {
        Output.WriteLine("SAMPLE_LAMBDA_METHOD.");
      };

      methodInvoker();

      Action action = () =>
      {
        Output.WriteLine("SAMPLE_LAMBDA_METHOD_ACTION.");
      };

      action();

      Func<int, int, int> sum = (x, y) =>
      {
        return (x + y);
      };

      Output.WriteLine(sum(10, 20));

      Func<int, int, int> sum2 = (int x, int y) =>
      {
        return (x + y);
      };

      Output.WriteLine(sum2(10, 20));
    }
  }
  #endregion
}

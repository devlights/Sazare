namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows.Forms;

  #region ラムダのサンプル (.net framework 3.5)
  /// <summary>
  /// ラムダ(lambda)のサンプル（.NET Framework 3.5）
  /// </summary>
  class LambdaSample : IExecutable
  {
    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      MethodInvoker methodInvoker = () =>
      {
        Console.WriteLine("SAMPLE_LAMBDA_METHOD.");
      };

      methodInvoker();

      Action action = () =>
      {
        Console.WriteLine("SAMPLE_LAMBDA_METHOD_ACTION.");
      };

      action();

      Func<int, int, int> sum = (x, y) =>
      {
        return (x + y);
      };

      Console.WriteLine(sum(10, 20));

      Func<int, int, int> sum2 = (int x, int y) =>
      {
        return (x + y);
      };

      Console.WriteLine(sum2(10, 20));
    }
  }
  #endregion
}

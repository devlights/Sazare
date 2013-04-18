namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region 匿名デリゲートのサンプル (.net framework 2.0)
  /// <summary>
  /// 匿名デリゲート(anonymous delegete)のサンプル（.NET Framework 2.0）
  /// </summary>
  [Sample]
  class AnonymousDelegateSample : IExecutable
  {
    /// <summary>
    /// 本サンプルで利用するデリゲートの定義
    /// </summary>
    delegate void SampleDelegate();

    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      //
      // 匿名メソッドを構築して実行.
      //
      SampleDelegate d = delegate()
      {
        Console.WriteLine("SAMPLE_ANONYMOUS_DELEGATE.");
      };

      d.Invoke();
    }

  }
  #endregion
}

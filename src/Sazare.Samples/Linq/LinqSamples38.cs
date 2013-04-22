namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sazare.Common;
  
  #region LinqSamples-38
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  [Sample]
  public class LinqSamples38 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      //
      // Range拡張メソッド.
      // この拡張メソッドは、文字通り指定された範囲の数値シーケンスを生成してくれる。
      //
      Output.WriteLine("=============== Range拡張メソッド ================");

      int start = 0;
      int count = 20;
      foreach (var i in Enumerable.Range(start, count).Where(item => (item % 2) == 0))
      {
        Output.WriteLine(i);
      }
      Output.WriteLine("===============================================");

      //
      // Repeat拡張メソッド.
      // この拡張メソッドは、文字通り指定された回数分、要素を繰り返し生成してくれる。
      //
      Output.WriteLine("=============== Repeat拡張メソッド ================");

      foreach (var i in Enumerable.Repeat(100, 5))
      {
        Output.WriteLine(i);
      }

      foreach (var s in Enumerable.Repeat("gsf_zero1", 5))
      {
        Output.WriteLine(s);
      }

      Output.WriteLine("===============================================");
    }
  }
  #endregion
}

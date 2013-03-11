namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region LinqSamples-38
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples38 : IExecutable
  {
    public void Execute()
    {
      //
      // Range拡張メソッド.
      // この拡張メソッドは、文字通り指定された範囲の数値シーケンスを生成してくれる。
      //
      Console.WriteLine("=============== Range拡張メソッド ================");

      int start = 0;
      int count = 20;
      foreach (var i in Enumerable.Range(start, count).Where(item => (item % 2) == 0))
      {
        Console.WriteLine(i);
      }
      Console.WriteLine("===============================================");

      //
      // Repeat拡張メソッド.
      // この拡張メソッドは、文字通り指定された回数分、要素を繰り返し生成してくれる。
      //
      Console.WriteLine("=============== Repeat拡張メソッド ================");

      foreach (var i in Enumerable.Repeat(100, 5))
      {
        Console.WriteLine(i);
      }

      foreach (var s in Enumerable.Repeat("gsf_zero1", 5))
      {
        Console.WriteLine(s);
      }

      Console.WriteLine("===============================================");
    }
  }
  #endregion
}

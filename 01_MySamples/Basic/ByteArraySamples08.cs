namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region ByteArraySamples-08
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  public class ByteArraySamples08 : IExecutable
  {
    public void Execute()
    {
      //
      // 数値をいろいろな基数に変換.
      //
      int i = 123;

      Console.WriteLine(Convert.ToString(i, 16));
      Console.WriteLine(Convert.ToString(i, 8));
      Console.WriteLine(Convert.ToString(i, 2));
    }
  }
  #endregion
}

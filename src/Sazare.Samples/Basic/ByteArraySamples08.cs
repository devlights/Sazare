namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sazare.Common;
  
  #region ByteArraySamples-08
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  [Sample]
  public class ByteArraySamples08 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      //
      // 数値をいろいろな基数に変換.
      //
      int i = 123;

      Output.WriteLine(Convert.ToString(i, 16));
      Output.WriteLine(Convert.ToString(i, 8));
      Output.WriteLine(Convert.ToString(i, 2));
    }
  }
  #endregion
}

namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sazare.Common;
  
  #region ByteArraySamples-03
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  [Sample]
  public class ByteArraySamples03 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      //
      // 数値を16進数で表示.
      //
      Output.WriteLine("0x{0:X2}", 12345678);
    }
  }
  #endregion
}

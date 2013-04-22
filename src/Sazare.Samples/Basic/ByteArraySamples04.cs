namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sazare.Common;
  
  #region ByteArraySamples-04
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  [Sample]
  public class ByteArraySamples04 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      //
      // 数値からバイト列へ変換
      //
      int i = 123456;
      byte[] buf = BitConverter.GetBytes(i);

      Output.WriteLine(BitConverter.ToString(buf));
    }
  }
  #endregion
}

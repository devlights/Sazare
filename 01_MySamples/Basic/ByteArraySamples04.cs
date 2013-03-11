namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region ByteArraySamples-04
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  public class ByteArraySamples04 : IExecutable
  {
    public void Execute()
    {
      //
      // 数値からバイト列へ変換
      //
      int i = 123456;
      byte[] buf = BitConverter.GetBytes(i);

      Console.WriteLine(BitConverter.ToString(buf));
    }
  }
  #endregion
}

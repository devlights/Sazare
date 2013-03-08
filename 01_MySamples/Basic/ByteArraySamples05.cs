namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region ByteArraySamples-05
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  public class ByteArraySamples05 : IExecutable
  {
    public void Execute()
    {
      //
      // バイト列を数値に
      //
      byte[] buf = new byte[4];
      new Random().NextBytes(buf);

      int i = BitConverter.ToInt32(buf, 0);

      Console.WriteLine(i);
    }
  }
  #endregion
}

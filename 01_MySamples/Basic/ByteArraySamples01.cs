namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region ByteArraySamples-01
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  public class ByteArraySamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // バイト配列を2進数表示.
      //
      byte[] buf = new byte[4];
      buf[0] = 0;
      buf[1] = 0;
      buf[2] = 0;
      buf[3] = 98;

      Console.WriteLine(
            string.Join(
              "",
              buf.Take(4).Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))
            ));
    }
  }
  #endregion
}

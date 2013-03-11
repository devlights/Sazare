namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  #region ByteArraySamples-06
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  public class ByteArraySamples06 : IExecutable
  {
    public void Execute()
    {
      //
      // 文字列をバイト列へ
      //
      string s = "gsf_zero1";
      byte[] buf = Encoding.ASCII.GetBytes(s);

      Console.WriteLine(BitConverter.ToString(buf));
    }
  }
  #endregion
}

namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  using Sazare.Common;
  
  #region ByteArraySamples-06
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  [Sample]
  public class ByteArraySamples06 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      //
      // 文字列をバイト列へ
      //
      string s = "gsf_zero1";
      byte[] buf = Encoding.ASCII.GetBytes(s);

      Output.WriteLine(BitConverter.ToString(buf));
    }
  }
  #endregion
}

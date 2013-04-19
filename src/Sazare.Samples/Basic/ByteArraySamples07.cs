namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  #region ByteArraySamples-07
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  [Sample]
  public class ByteArraySamples07 : IExecutable
  {
    public void Execute()
    {
      //
      // バイト列を文字列へ.
      //
      string s = "gsf_zero1";
      byte[] buf = Encoding.ASCII.GetBytes(s);

      Console.WriteLine(Encoding.ASCII.GetString(buf));
    }
  }
  #endregion
}

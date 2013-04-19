namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region ByteArraySamples-09
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  [Sample]
  public class ByteArraySamples09 : IExecutable
  {
    public void Execute()
    {
      //
      // 利用しているアーキテクチャのエンディアンを判定.
      //
      Console.WriteLine(BitConverter.IsLittleEndian);
    }
  }
  #endregion
}

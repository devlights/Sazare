namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using Sazare.Common;

  #region BufferSamples-01
  /// <summary>
  /// Bufferクラスのサンプルです。
  /// </summary>
  [Sample]
  public class BufferSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // System.Bufferクラスのサンプル
      //   http://msdn.microsoft.com/ja-jp/library/system.buffer.aspx
      //
      // Bufferクラスは、プリミティブ型の配列にのみ利用できるバッファである。
      // Bufferクラス内で保持されるデータは、連続したバイト列として扱われる。
      //
      // 対応している型は
      //   Boolean、Char、SByte、Byte、Int16、UInt16、Int32、UInt32、Int64、UInt64、IntPtr、UIntPtr、Single、および Double
      // となっている。
      //
      // プリミティブ型に関しては、System.Arrayの同様のメソッドよりBufferの方がパフォーマンスが良いとのこと。
      //
      const int BUFFER_LENGTH = 10000000;

      var srcArray = new int[BUFFER_LENGTH];

      var rnd = new Random();
      for (int i = 0; i < BUFFER_LENGTH; i++)
      {
        srcArray[i] = rnd.Next(127);
      }

      //
      // BlockCopyメソッド
      //   Array.Copyと使い方は同じだが、4番目の引数が[length]ではなく[count]となっている事に注意。
      //   この引数には、コピーする「バイト数」を渡す必要がある。
      //   なので、intの配列の場合は配列のLengthに
      //      sizeof(int)
      //   を掛けたものを指定する必要がある。
      //
      var destArray = new int[BUFFER_LENGTH];
      Buffer.BlockCopy(srcArray, 0, destArray, 0, (destArray.Length * sizeof(int)));

      // Array.Copy版
      var destArray2 = new int[BUFFER_LENGTH];
      Array.Copy(srcArray, 0, destArray2, 0, destArray2.Length);

      Output.WriteLine("[Buffer.BlockCopy] srcArray == destArray ==> {0}", srcArray.SequenceEqual(destArray));
      Output.WriteLine("[Array.Copy]       srcArray == destArray2 ==> {0}", srcArray.SequenceEqual(destArray2));
      Output.WriteLine("destArray == destArray2 ==> {0}", destArray.SequenceEqual(destArray2));

    }
  }
  #endregion
}

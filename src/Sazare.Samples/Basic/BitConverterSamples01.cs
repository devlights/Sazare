namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region BitConverterSamples-01
  /// <summary>
  /// System.BitConverterクラスのサンプルです。
  /// </summary>
  [Sample]
  public class BitConverterSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // バイト列から16進文字列への変換.
      //
      byte[] bytes = new byte[] { 1, 2, 10, 15, (byte)'a', (byte)'b', (byte)'q' };
      Console.WriteLine(BitConverter.ToString(bytes));

      //
      // 数値からバイト列への変換.
      // (一旦数値をバイト列に変換してから、16進に変換して表示)
      //
      int i = 100;
      Console.WriteLine(BitConverter.ToString(BitConverter.GetBytes(i)));

      int i2 = 0x12345678;
      Console.WriteLine(BitConverter.ToString(BitConverter.GetBytes(i2)));

      //
      // バイト列から数値への変換.
      //
      bytes = new byte[] { 1 };
      Console.WriteLine(BitConverter.ToBoolean(bytes, 0));

      bytes = new byte[] { 1, 0, 0, 0 };
      Console.WriteLine(BitConverter.ToInt32(bytes, 0));

      bytes = BitConverter.GetBytes((byte)'a');
      Console.WriteLine(BitConverter.ToChar(bytes, 0));
    }
  }
  #endregion
}

namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region BCDSamples-01
  /// <summary>
  /// BCD変換についてのサンプルです。
  /// </summary>
  [Sample]
  public class BCDSamples01 : IExecutable
  {
    public void Execute()
    {
      int val1 = int.MaxValue;
      long val2 = long.MaxValue;

      byte[] bcdVal1 = BCDUtils.ToBCD(val1, 5);
      byte[] bcdVal2 = BCDUtils.ToBCD(val2, 10);

      Console.WriteLine("integer value = {0}", val1);
      Console.WriteLine("BCD   value = {0}", BitConverter.ToString(bcdVal1));
      Console.WriteLine("long  value = {0}", val2);
      Console.WriteLine("BCD   value = {0}", BitConverter.ToString(bcdVal2));

      int val3 = BCDUtils.ToInt(bcdVal1);
      long val4 = BCDUtils.ToLong(bcdVal2);

      Console.WriteLine("val1 == val3 = {0}", val1 == val3);
      Console.WriteLine("val2 == val4 = {0}", val2 == val4);
    }

    /// <summary>
    /// BCD変換を行うユーティリティクラスです。
    /// </summary>
    public static class BCDUtils
    {
      public static int ToInt(byte[] bcd)
      {
        return Convert.ToInt32(ToLong(bcd));
      }

      public static long ToLong(byte[] bcd)
      {
        long result = 0;

        foreach (byte b in bcd)
        {
          int digit1 = b >> 4;
          int digit2 = b & 0x0f;

          result = (result * 100) + (digit1 * 10) + digit2;
        }

        return result;
      }

      public static byte[] ToBCD(int num, int byteCount)
      {
        return ToBCD<int>(num, byteCount);
      }

      public static byte[] ToBCD(long num, int byteCount)
      {
        return ToBCD<long>(num, byteCount);
      }

      private static byte[] ToBCD<T>(T num, int byteCount) where T : struct, IConvertible
      {
        long val = Convert.ToInt64(num);

        byte[] bcdNumber = new byte[byteCount];
        for (int i = 1; i <= byteCount; i++)
        {
          long mod = val % 100;

          long digit2 = mod % 10;
          long digit1 = (mod - digit2) / 10;

          bcdNumber[byteCount - i] = Convert.ToByte((digit1 * 16) + digit2);

          val = (val - mod) / 100;
        }

        return bcdNumber;
      }
    }
  }
  #endregion
}

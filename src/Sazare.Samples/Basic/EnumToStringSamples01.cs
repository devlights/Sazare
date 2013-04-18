namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region EnumToStringSamples-01
  /// <summary>
  /// Enumに関するサンプルです。
  /// </summary>
  [Sample]
  public class EnumToStringSamples01 : IExecutable
  {
    private enum MyColor
    {
      Red = 1,
      Blue,
      White,
      Black
    }

    public void Execute()
    {
      MyColor blue = MyColor.Blue;

      // 2
      Console.WriteLine(blue.ToString("D"));
      // 0x00000002
      Console.WriteLine("0x{0}", blue.ToString("X"));
      // Blue
      Console.WriteLine(blue.ToString("G"));
    }
  }
  #endregion
}

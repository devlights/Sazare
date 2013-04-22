namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region NumberFormatSamples-03
  /// <summary>
  /// 数値フォーマットのサンプルです。
  /// </summary>
  [Sample]
  public class NumberFormatSamples03 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      string s = "123,456";

      try
      {
        // ERROR.
        int i2 = int.Parse(s);
      }
      catch (FormatException ex)
      {
        Console.WriteLine(ex.Message);
      }

      int i3 = int.Parse(s, System.Globalization.NumberStyles.AllowThousands);
      Console.WriteLine(i3);

    }
  }
  #endregion
}

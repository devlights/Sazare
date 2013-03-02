namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region NumberFormatSamples-04
  /// <summary>
  /// 数値フォーマットのサンプルです。
  /// </summary>
  public class NumberFormatSamples04 : IExecutable
  {
    public void Execute()
    {
      int iTestValue1 = 1;
      int iTestValue2 = 10;

      Console.WriteLine("iTestValue1: {0:D2}", iTestValue1);
      Console.WriteLine("iTestValue2: {0:D2}", iTestValue2);

      string sTestValue1 = iTestValue1.ToString();
      string sTestValue2 = iTestValue2.ToString();

      //
      // 元となるデータの型が数値ではない場合、2桁で表示と指定しても
      // フォーマットされない。(文字列の場合はそのまま"1"と表示される。)
      //
      Console.WriteLine("sTestValue1: {0:D2}", sTestValue1);
      Console.WriteLine("sTestValue2: {0:D2}", sTestValue2);
    }
  }
  #endregion
}

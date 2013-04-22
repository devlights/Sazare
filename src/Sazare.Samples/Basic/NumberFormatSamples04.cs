namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sazare.Common;
  
  #region NumberFormatSamples-04
  /// <summary>
  /// 数値フォーマットのサンプルです。
  /// </summary>
  [Sample]
  public class NumberFormatSamples04 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      int iTestValue1 = 1;
      int iTestValue2 = 10;

      Output.WriteLine("iTestValue1: {0:D2}", iTestValue1);
      Output.WriteLine("iTestValue2: {0:D2}", iTestValue2);

      string sTestValue1 = iTestValue1.ToString();
      string sTestValue2 = iTestValue2.ToString();

      //
      // 元となるデータの型が数値ではない場合、2桁で表示と指定しても
      // フォーマットされない。(文字列の場合はそのまま"1"と表示される。)
      //
      Output.WriteLine("sTestValue1: {0:D2}", sTestValue1);
      Output.WriteLine("sTestValue2: {0:D2}", sTestValue2);
    }
  }
  #endregion
}

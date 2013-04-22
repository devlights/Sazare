namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region NumberFormatSamples-02
  /// <summary>
  /// 数値フォーマットのサンプルです。
  /// </summary>
  [Sample]
  public class NumberFormatSamples02 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      int i = 123456;
      Console.WriteLine("{0:N0}", i);

    }
  }
  #endregion
}

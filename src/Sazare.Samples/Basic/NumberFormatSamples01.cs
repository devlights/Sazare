namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sazare.Common;
  
  #region NumberFormatSamples-01
  /// <summary>
  /// 数値フォーマットのサンプルです。
  /// </summary>
  [Sample]
  public class NumberFormatSamples01 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      decimal d = 99M;
      Output.WriteLine(Math.Round(d, 1));
      Output.WriteLine("{0:##0.0}", d);
    }
  }
  #endregion
}

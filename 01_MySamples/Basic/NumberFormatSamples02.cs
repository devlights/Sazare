namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region NumberFormatSamples-02
  /// <summary>
  /// 数値フォーマットのサンプルです。
  /// </summary>
  public class NumberFormatSamples02 : IExecutable
  {
    public void Execute()
    {
      int i = 123456;
      Console.WriteLine("{0:N0}", i);

    }
  }
  #endregion
}

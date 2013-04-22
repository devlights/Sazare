namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sazare.Common;
  
  #region ExtensionMethodSample-01
  /// <summary>
  /// 拡張メソッドのサンプル1です。
  /// </summary>
  [Sample]
  public class ExtensionMethodSample01 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      string s = null;
      s.PrintMyName();
    }
  }

  public static class ExtensionMethodSample01_ExtClass
  {
    public static void PrintMyName(this string self)
    {
      Output.WriteLine(self == null);
      Output.WriteLine("GSF-ZERO1.");
    }
  }
#endregion
}

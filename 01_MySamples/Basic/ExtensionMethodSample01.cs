namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region ExtensionMethodSample-01
  /// <summary>
  /// 拡張メソッドのサンプル1です。
  /// </summary>
  public class ExtensionMethodSample01 : IExecutable
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
      Console.WriteLine(self == null);
      Console.WriteLine("GSF-ZERO1.");
    }
  }
#endregion
}

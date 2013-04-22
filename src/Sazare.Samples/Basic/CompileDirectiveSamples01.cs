namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sazare.Common;
  
  #region CompileDirectiveSamples-01
  /// <summary>
  /// コンパイルディレクティブのサンプル1です。
  /// </summary>
  [Sample]
  public class CompileDirectiveSamples01 : Sazare.Common.IExecutable
  {

    public void Execute()
    {
      Output.WriteLine("001:HELLO C#");

#if(DEBUG)
      Output.WriteLine("002:HELLO C# (DEBUG)");
#else
        Output.WriteLine("003:HELLO C# (ELSE)");
#endif
    }
  }
  #endregion
}

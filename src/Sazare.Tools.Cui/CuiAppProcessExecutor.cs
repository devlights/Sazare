namespace Sazare.Tools.Cui
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sazare.Common;

  /// <summary>
  /// 実行を担当するクラスです。
  /// </summary>
  public class CuiAppProcessExecutor : IExecutor
  {
    public string StartLogMessage { get; set; }
    public string EndLogMessage   { get; set; }

    public CuiAppProcessExecutor()
    {
      StartLogMessage = "================== START ==================";
      EndLogMessage   = "==================  END  ==================";
    }

    public void Execute(IExecutable target)
    {
      if (target == null)
      {
        throw new ArgumentNullException("target");
      }

      Output.WriteLine(StartLogMessage);
      target.Execute();
      Output.WriteLine(EndLogMessage);
    }
  }
}

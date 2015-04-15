namespace Sazare.Tools.Cui
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

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

      using (new TimeTracer())
      {
        Output.WriteLine(StartLogMessage);
        target.Execute();
        Output.WriteLine(EndLogMessage);        
      }
    }
  }

  internal class TimeTracer : IDisposable
  {
    private readonly Stopwatch _watch;

    public TimeTracer()
    {
      _watch = Stopwatch.StartNew();
    }

    #region IDisposable メンバー

    public void Dispose()
    {
      _watch.Stop();
      Output.WriteLine("処理時間： {0}", _watch.Elapsed);
    }

    #endregion
  }
}

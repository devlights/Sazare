namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  #region スレッドプールを利用したスレッド処理
  /// <summary>
  /// スレッドプール(ThreadPool)を利用したスレッド処理のサンプルです。
  /// </summary>
  public class ThreadPoolSample : IExecutable
  {
    /// <summary>
    /// スレッドの状態を表すデータクラスです。
    /// </summary>
    class StateInfo
    {
      public int Count
      {
        get;
        set;
      }

      public DateTime Time
      {
        get;
        set;
      }
    }

    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      for (int i = 0; i < 15; i++)
      {
        ThreadPool.QueueUserWorkItem((stateInfo) =>
        {
          StateInfo p = stateInfo as StateInfo;
          Thread.Sleep(150);
          Console.WriteLine("Thread Count:{0}, Time:{1}", p.Count, p.Time.ToString("hh:mm:ss.fff"));
        },
        new StateInfo
        {
          Count = i,
          Time = DateTime.Now
        });
      }

      Thread.Sleep(2000);
    }
  }
  #endregion
}

namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using System.Threading;

  #region BackgroundWorkerを利用したスレッド処理
  /// <summary>
  /// BackgroundWorkerを利用したスレッド処理のサンプルです。
  /// </summary>
  public class BackgroundWorkerSample : IExecutable
  {
    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      Console.WriteLine("[MAIN] START. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);

      BackgroundWorker worker = new BackgroundWorker();

      //
      // 非同期処理のイベントをハンドル.
      //
      worker.DoWork += (s, e) =>
      {
        Console.WriteLine("[WORK] START. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);

        for (int i = 0; i < 10; i++)
        {
          Console.WriteLine("[WORK] PROCESSING. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
          Thread.Sleep(105);
        }
      };

      //
      // 非同期処理が終了した際のイベントをハンドル.
      //
      worker.RunWorkerCompleted += (s, e) =>
      {
        if (e.Error != null)
        {
          Console.WriteLine("[WORK] ERROR OCCURED. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        Console.WriteLine("[WORK] END. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
      };

      //
      // 非同期処理を開始.
      //
      worker.RunWorkerAsync();

      //
      // メインスレッドの処理.
      //
      for (int i = 0; i < 10; i++)
      {
        Console.WriteLine("[MAIN] PROCESSING. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
        Thread.Sleep(100);
      }

      Thread.Sleep(1000);
      Console.WriteLine("[MAIN] END. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
    }
  }
  #endregion
}

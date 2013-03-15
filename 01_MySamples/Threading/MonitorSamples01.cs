namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  #region MonitorSample-01
  /// <summary>
  /// Monitorクラスについてのサンプルです。
  /// </summary>
  public class MonitorSamples01 : IExecutable
  {
    object _lock = new object();
    bool _go;

    public void Execute()
    {
      new Thread(Waiter).Start();

      Thread.Sleep(TimeSpan.FromSeconds(1));
      lock (_lock)
      {
        _go = true;
        //
        // ブロックしているスレッドに対して、通知を発行.
        //   Monitor.Pulseは、lock内でしか実行できない.
        //
        Monitor.Pulse(_lock);
      }

      Console.WriteLine("Main thread end.");
    }

    void Waiter()
    {
      lock (_lock)
      {
        while (!_go)
        {
          //
          // 通知が来るまで、スレッドをブロック.
          //   Monitor.Waitは、lock内でしか実行できない.
          //
          Monitor.Wait(_lock);
        }
      }

      Console.WriteLine("awake!!");
    }
  }
  #endregion
}

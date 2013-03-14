namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  #region ThreadingNamespaceSamples-05
  public class ThreadingNamespaceSamples05 : IExecutable
  {
    public void Execute()
    {
      //
      // 普通にスレッドタイマーを作成し、コールバックの呼び出し間隔を無効に
      // した状態でタイマーを開始させる.
      //
      var timer = new System.Threading.Timer(TimerCallback);
      timer.Change(0, Timeout.Infinite);

      Thread.Sleep(10000);
    }

    void TimerCallback(object state)
    {
      Console.WriteLine("Timer Callback!!");

      var rnd = new Random();

      // 時間のかかる処理をシミュレート
      Thread.Sleep(rnd.Next(1000));
      Console.WriteLine("\tsleep done.");

      //
      // 再度Changeメソッドを呼び出して、次のコールバックを設定.
      //
      var timer = state as System.Threading.Timer;
      timer.Change(rnd.Next(700), Timeout.Infinite);
    }
  }
  #endregion
}

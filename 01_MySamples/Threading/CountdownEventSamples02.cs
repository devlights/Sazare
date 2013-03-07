namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  #region CountdownEventSamples-02
  /// <summary>
  /// CountdownEventクラスについてのサンプルです。(2)
  /// </summary>
  /// <remarks>
  /// CountdownEventクラスは、.NET 4.0から追加されたクラスです。
  /// JavaのCountDownLatchクラスと同じ機能を持っています。
  /// </remarks>
  public class CountdownEventSamples02 : IExecutable
  {
    public void Execute()
    {
      const int LEAST_TASK_FINISH_COUNT = 3;

      //
      // 複数のスレッドから一つのCountdownEventをシグナルする.
      //
      // CountdownEventがよく利用されるパターンとなる。
      // N個の処理が規定数終了するまで、メインスレッドの続行を待機するイメージ.
      //
      // 以下の処理では、5つタスクを作成して、3つ終わった時点で
      // メインスレッドは処理を続行するようにする.
      //
      // N個の処理が全部終了するまで、メインスレッドの続行を待機する場合は
      // CountdownEventのカウントをタスクの処理数と同じにすれば良い。
      //
      using (CountdownEvent cde = new CountdownEvent(LEAST_TASK_FINISH_COUNT))
      {
        // 初期の状態を表示.
        Console.WriteLine("InitialCount={0}", cde.InitialCount);
        Console.WriteLine("CurrentCount={0}", cde.CurrentCount);
        Console.WriteLine("IsSet={0}", cde.IsSet);

        Task[] tasks = new Task[]
          {
            Task.Factory.StartNew(TaskProc, cde),
            Task.Factory.StartNew(TaskProc, cde),
            Task.Factory.StartNew(TaskProc, cde),
            Task.Factory.StartNew(TaskProc, cde),
            Task.Factory.StartNew(TaskProc, cde)
          };

        //
        // 3つ終わるまで待機.
        //
        cde.Wait();
        Console.WriteLine("5つのタスクの内、3つ終了");

        Console.WriteLine("メインスレッド 続行開始・・・");
        Thread.Sleep(TimeSpan.FromSeconds(1));

        //
        // 残りのタスクを待機.
        //
        Task.WaitAll(tasks);
        Console.WriteLine("全てのタスク終了");

        // 現在の状態を表示.
        Console.WriteLine("InitialCount={0}", cde.InitialCount);
        Console.WriteLine("CurrentCount={0}", cde.CurrentCount);
        Console.WriteLine("IsSet={0}", cde.IsSet);
      }
    }

    void TaskProc(object data)
    {
      Console.WriteLine("Task ID={0} 開始", Task.CurrentId);
      Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(10)));

      //
      // 既に3つ終了しているか否かを確認し、まだならシグナル.
      //
      CountdownEvent cde = data as CountdownEvent;
      if (!cde.IsSet)
      {
        cde.Signal();
        Console.WriteLine("＊＊＊カウントをデクリメント＊＊＊ Task ID={0} CountdownEvent.CurrentCount={1}", Task.CurrentId, cde.CurrentCount);
      }

      Console.WriteLine("Task ID={0} 終了", Task.CurrentId);
    }
  }
  #endregion
}

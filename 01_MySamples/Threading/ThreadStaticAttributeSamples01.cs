namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  #region ThreadStaticAttributeSamples-01
  /// <summary>
  /// ThreadStatic属性に関するサンプルです。
  /// </summary>
  public class ThreadStaticAttributeSamples01 : IExecutable
  {
    private class ThreadState
    {
      /// <summary>
      /// 各スレッド毎に固有の値を持つフィールド.
      /// </summary>
      [ThreadStatic]
      static KeyValuePair<string, int> NameAndId;
      /// <summary>
      /// 各スレッドで共有されるフィールド.
      /// </summary>
      static KeyValuePair<string, int> SharedNameAndId;

      public static void DoThreadProcess()
      {
        Thread thread = Thread.CurrentThread;

        //
        // ThreadStatic属性が付加されたフィールドと共有されたフィールドの両方に値を設定.
        //
        NameAndId = new KeyValuePair<string, int>(thread.Name, thread.ManagedThreadId);
        SharedNameAndId = new KeyValuePair<string, int>(thread.Name, thread.ManagedThreadId);

        Console.WriteLine("[BEFORE] ThreadStatic={0} Shared={1}", NameAndId, SharedNameAndId);

        //
        // 他のスレッドが動作できるようにする.
        //
        Thread.Sleep(TimeSpan.FromMilliseconds(200));

        Console.WriteLine("[AFTER ] ThreadStatic={0} Shared={1}", NameAndId, SharedNameAndId);
      }
    }

    public void Execute()
    {
      List<Thread> threads = new List<Thread>();
      for (int i = 0; i < 5; i++)
      {
        Thread thread = new Thread(ThreadState.DoThreadProcess);

        thread.Name = string.Format("Thread-{0}", i);
        thread.IsBackground = true;

        threads.Add(thread);

        thread.Start();
      }

      threads.ForEach(thread =>
      {
        thread.Join();
      });
    }
  }
  #endregion
}

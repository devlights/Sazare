namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  #region ThreadingNamespaceSamples-03
  [Sample]
  public class ThreadingNamespaceSamples03 : IExecutable
  {

    public void Execute()
    {
      Thread thread = new Thread(ThreadProcess);

      thread.IsBackground = true;
      thread.Start();

      thread.Join();
    }

    void ThreadProcess()
    {
      ///////////////////////////////////////////////////
      //
      // スレッドクラスが持つその他のプロパティについて
      //
      Thread currentThread = Thread.CurrentThread;

      // 
      // IsAliveプロパティ
      //    ⇒スレッドが起動されており、まだ終了または中止されていない場合は
      //    　trueとなる。
      //
      Console.WriteLine("IsAlive={0}", currentThread.IsAlive);

      //
      // IsThreadPoolThread, ManagedThreadIdプロパティ
      //    ⇒それぞれ、スレッドプールスレッドかどうかとマネージスレッドを識別
      //    　する値が取得できる。
      //
      Console.WriteLine("IsThreadPoolThread={0}", currentThread.IsThreadPoolThread);
      Console.WriteLine("ManagedThreadId={0}", currentThread.ManagedThreadId);

      //
      // Priorityプロパティ
      //    ⇒対象のスレッドの優先度（プライオリティ）を取得及び設定します。
      //    　Highestが最も高く、Lowestが最も低い.
      //
      Console.WriteLine("Priority={0}", currentThread.Priority);
    }
  }
  #endregion
}

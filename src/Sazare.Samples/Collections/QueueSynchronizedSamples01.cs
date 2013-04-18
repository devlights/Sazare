namespace Sazare.Samples
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  #region QueueSynchronizedSamples-01
  /// <summary>
  /// Queueの同期処理についてのサンプルです。
  /// </summary>
  [Sample]
  public class QueueSynchronizedSamples01 : IExecutable
  {
    Queue queue;
    public void Execute()
    {
      queue = Queue.Synchronized(new Queue());
      Console.WriteLine("Queue.IsSyncronized == {0}", queue.IsSynchronized);

      for (int i = 0; i < 1000; i++)
      {
        queue.Enqueue(i);
      }

      new Thread(EnumerateCollection).Start();
      new Thread(ModifyCollection).Start();

      Console.WriteLine("Press any key to exit...");
      Console.ReadLine();
    }

    void EnumerateCollection()
    {
      //
      // ロックせずに列挙処理を行う。
      //
      // CollectionのSynchronizedメソッドで作成したオブジェクトは
      // 単一操作に対しては、同期できるが複合アクションはガードできない。
      // （イテレーション、ナビゲーション、プット・イフ・アブセントなど）
      //
      // 別のスレッドにて、コレクションを操作している場合
      // 例外が発生する可能性がある。
      //
      /*
      foreach(int i in queue)
      {
        Console.WriteLine(i);
        Thread.Sleep(0);
      }
      */

      //
      // 第一の方法：
      //
      // ループしている間、コレクションをロックする.
      // 有効であるが、列挙処理を行っている間ずっとロックされたままとなる。
      // 
      /*
      lock(queue.SyncRoot)
      {
        foreach(int i in queue)
        {
          Console.WriteLine(i);
          Thread.Sleep(0);
        }
      }
      */

      //
      // 第二の方法：
      //
      // 一旦ロックを獲得し、コレクションのクローンを作成する。
      // クローン作成後、ロックを解放し、その後クローンに対して列挙処理を行う。
      //
      // これもコレクション自体が大きい場合は時間と負荷がかかるが、それはトレードオフとなる。
      //
      Queue cloneQueue = null;
      lock (queue.SyncRoot)
      {

        Array array = Array.CreateInstance(typeof(int), queue.Count);
        queue.CopyTo(array, 0);

        cloneQueue = new Queue(array);
      }

      foreach (int i in cloneQueue)
      {
        Console.WriteLine(i);

        // わざとタイムスライスを切り替え
        Thread.Sleep(0);
      }
    }

    void ModifyCollection()
    {

      for (; ; )
      {
        if (queue.Count == 0)
        {
          break;
        }

        Console.WriteLine("\t==> Dequeue");
        queue.Dequeue();

        // わざとタイムスライスを切り替え
        Thread.Sleep(0);
      }

    }
  }
  #endregion
}

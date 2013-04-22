namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  #region ThreadingNamespaceSamples-02
  /// <summary>
  /// System.Threading名前空間に存在するクラスのサンプルです。
  /// </summary>
  [Sample]
  public class ThreadingNamespaceSamples02 : Sazare.Common.IExecutable
  {

    public void Execute()
    {
      ////////////////////////////////////////////////////////////
      //
      // 無名データスロット.
      //    データスロットはどれかのスレッドで最初に作成したら
      //    全てのスレッドに対してスロットが割り当てられる。
      //
      LocalDataStoreSlot slot = Thread.AllocateDataSlot();

      List<Thread> threads = new List<Thread>();
      for (int i = 0; i < 10; i++)
      {
        Thread thread = new Thread(DoAnonymousDataSlotProcess);
        thread.Name = string.Format("Thread-{0}", i);
        thread.IsBackground = true;

        threads.Add(thread);

        thread.Start(slot);
      }

      threads.ForEach(thread =>
      {
        thread.Join();
      });

      Console.WriteLine(string.Empty);

      ////////////////////////////////////////////////////////////
      //
      // 名前付きデータスロット.
      //    名前がつけられる事以外は、無名のスロットと同じです。
      //    名前付きデータスロットは、最初にその名前が呼ばれた
      //    際に作成されます。
      //    FreeNamedDataSlotメソッドを呼ぶと現在のスロット設定
      //    が解放されます。
      //
      threads.Clear();
      for (int i = 0; i < 10; i++)
      {
        Thread thread = new Thread(DoNamedDataSlotProcess);
        thread.Name = string.Format("Thread-{0}", i);
        thread.IsBackground = true;

        threads.Add(thread);

        thread.Start(slot);
      }

      threads.ForEach(thread =>
      {
        thread.Join();
      });

      //
      // 利用したデータスロットを解放.
      //
      Thread.FreeNamedDataSlot("SampleSlot");

    }

    void DoAnonymousDataSlotProcess(object stateObj)
    {
      LocalDataStoreSlot slot = stateObj as LocalDataStoreSlot;

      //
      // スロットにデータを設定
      //
      Thread.SetData(slot, string.Format("ManagedThreadId={0}", Thread.CurrentThread.ManagedThreadId));

      //
      // 設定した内容を確認.
      //
      Console.WriteLine("[BEFORE] Thread:{0}   DataSlot:{1}", Thread.CurrentThread.Name, Thread.GetData(slot));

      //
      // 別のスレッドに処理を行って貰う為に一旦Sleepする。
      //
      Thread.Sleep(TimeSpan.FromSeconds(1));

      //
      // 再度確認.
      //
      Console.WriteLine("[AFTER ] Thread:{0}   DataSlot:{1}", Thread.CurrentThread.Name, Thread.GetData(slot));
    }

    void DoNamedDataSlotProcess(object stateObj)
    {
      //
      // スロットにデータを設定
      //
      Thread.SetData(Thread.GetNamedDataSlot("SampleSlot"), string.Format("ManagedThreadId={0}", Thread.CurrentThread.ManagedThreadId));

      //
      // 設定した内容を確認.
      //
      Console.WriteLine("[BEFORE] Thread:{0}   DataSlot:{1}", Thread.CurrentThread.Name, Thread.GetData(Thread.GetNamedDataSlot("SampleSlot")));

      //
      // 別のスレッドに処理を行って貰う為に一旦Sleepする。
      //
      Thread.Sleep(TimeSpan.FromSeconds(1));

      //
      // 再度確認.
      //
      Console.WriteLine("[AFTER ] Thread:{0}   DataSlot:{1}", Thread.CurrentThread.Name, Thread.GetData(Thread.GetNamedDataSlot("SampleSlot")));
    }
  }
  #endregion
}

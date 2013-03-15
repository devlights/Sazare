namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  #region ThreadingNamespaceSamples-04
  public class ThreadingNamespaceSamples04 : IExecutable
  {
    public void Execute()
    {
      //
      // .NET 4.0より、Threadクラスに以下のメソッドが追加された。
      //   ・Yieldメソッド
      //
      // Yieldメソッドは、別のスレッドにタイムスライスを引き渡す為のメソッド。
      // 今までは、Thread.Sleepを利用したりして、タイムスライスを切り替えるよう
      // にしていたが、今後はこのメソッドを利用することが推奨される。
      //
      // 戻り値は、タイムスライスの引き渡しが成功したか否かが返ってくる。
      //

      //
      // テスト用にスレッドを２つ起動する.
      //
      Thread t1 = new Thread(ThreadProc);
      Thread t2 = new Thread(ThreadProc);

      t1.Start("T1");
      t2.Start("T2");

      t1.Join();
      t2.Join();
    }

    void ThreadProc(object stateObj)
    {
      string threadName = stateObj.ToString();
      Console.WriteLine("{0} Start", threadName);

      //
      // タイムスライスを切り替え.
      //
      Console.WriteLine("{0} Yield Call", threadName);
      bool isSuccess = Thread.Yield();
      Console.WriteLine("{0} Yield={1}", threadName, isSuccess);

      Console.WriteLine("{0} End", threadName);
    }
  }
  #endregion
}

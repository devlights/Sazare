namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  #region SemaphoreSlimSamples-02
  /// <summary>
  /// SemaphoreSlimクラスについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// SemaphoreSlimクラスは、.NET 4.0から追加されたクラスです。
  /// 従来から存在していたSemaphoreクラスの軽量版となります。
  /// </remarks>
  public class SemaphoreSlimSamples02 : IExecutable
  {
    public void Execute()
    {
      //
      // SemaphoreSlimのWaitメソッドにはキャンセルトークンを
      // 受け付けるオーバーロードが存在する。
      //
      // CountdownEventやBarrierの場合と同じく、Waitメソッドに
      // キャンセルトークンを指定した場合、別の場所にてキャンセルが
      // 行われると、OperationCanceledExceptionが発生する。
      //
      const int timeout = 2000;

      CancellationTokenSource tokenSource = new CancellationTokenSource();
      CancellationToken token = tokenSource.Token;

      using (SemaphoreSlim semaphore = new SemaphoreSlim(2))
      {
        //
        // あらかじめ、セマフォの上限までWaitしておき
        // 後のスレッドが入れないようにしておく.
        //
        semaphore.Wait();
        semaphore.Wait();

        //
        // ３つのタスクを作成する.
        //  １つ目のタスク：キャンセルトークンを指定して無制限待機.
        //  ２つ目のタスク：キャンセルトークンとタイムアウト値を指定して待機.
        //  ３つ目のタスク：特定時間待機した後、キャンセル処理を行う.
        //
        Parallel.Invoke
          (
            () => WaitProc1(semaphore, token),
            () => WaitProc2(semaphore, timeout, token),
            () => DoCancel(timeout, tokenSource)
          );

        semaphore.Release();
        semaphore.Release();
        Console.WriteLine("CurrentCount={0}", semaphore.CurrentCount);
      }
    }

    // キャンセルトークンを指定して無制限待機.
    void WaitProc1(SemaphoreSlim semaphore, CancellationToken token)
    {
      try
      {
        Console.WriteLine("WaitProc1=待機開始");
        semaphore.Wait(token);
      }
      catch (OperationCanceledException cancelEx)
      {
        Console.WriteLine("WaitProc1={0}", cancelEx.Message);
      }
      finally
      {
        Console.WriteLine("WaitProc1_CurrentCount={0}", semaphore.CurrentCount);
      }
    }

    // キャンセルトークンとタイムアウト値を指定して待機.
    void WaitProc2(SemaphoreSlim semaphore, int timeout, CancellationToken token)
    {
      try
      {
        bool isSuccess = semaphore.Wait(timeout, token);
        if (!isSuccess)
        {
          Console.WriteLine("WaitProc2={0}t★★タイムアウト★★", isSuccess);
        }
      }
      catch (OperationCanceledException cancelEx)
      {
        Console.WriteLine("WaitProc2={0}", cancelEx.Message);
      }
      finally
      {
        Console.WriteLine("WaitProc2_CurrentCount={0}", semaphore.CurrentCount);
      }
    }

    // 特定時間待機した後、キャンセル処理を行う.
    void DoCancel(int timeout, CancellationTokenSource tokenSource)
    {
      Console.WriteLine("待機開始：{0}msec", timeout + 1000);
      Thread.Sleep(timeout + 1000);

      Console.WriteLine("待機終了");
      Console.WriteLine("★★キャンセル発行★★");
      tokenSource.Cancel();
    }
  }
  #endregion
}

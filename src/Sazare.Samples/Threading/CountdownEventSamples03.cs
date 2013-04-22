namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  #region CountdownEventSamples-03
  /// <summary>
  /// CountdownEventクラスについてのサンプルです。(3)
  /// </summary>
  /// <remarks>
  /// CountdownEventクラスは、.NET 4.0から追加されたクラスです。
  /// JavaのCountDownLatchクラスと同じ機能を持っています。
  /// </remarks>
  [Sample]
  public class CountdownEventSamples03 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      //
      // CountdownEventには、CancellationTokenを受け付けるWaitメソッドが存在する.
      // 使い方は、ManualResetEventSlimクラスの場合と同じ。
      // 
      // 参考リソース:
      //   .NET クラスライブラリ探訪-042 (System.Threading.ManualResetEventSlim)
      //   http://d.hatena.ne.jp/gsf_zero1/20110323/p1
      //
      CancellationTokenSource tokenSource = new CancellationTokenSource();
      CancellationToken token = tokenSource.Token;

      using (CountdownEvent cde = new CountdownEvent(1))
      {
        // 初期の状態を表示.
        Console.WriteLine("InitialCount={0}", cde.InitialCount);
        Console.WriteLine("CurrentCount={0}", cde.CurrentCount);
        Console.WriteLine("IsSet={0}", cde.IsSet);

        Task t = Task.Factory.StartNew(() =>
        {
          Thread.Sleep(TimeSpan.FromSeconds(2));

          token.ThrowIfCancellationRequested();
          cde.Signal();
        }, token);

        //
        // 処理をキャンセル.
        //
        tokenSource.Cancel();

        try
        {
          cde.Wait(token);
        }
        catch (OperationCanceledException cancelEx)
        {
          if (token == cancelEx.CancellationToken)
          {
            Console.WriteLine("＊＊＊CountdownEvent.Wait()がキャンセルされました＊＊＊");
          }
        }

        try
        {
          t.Wait();
        }
        catch (AggregateException aggEx)
        {
          aggEx.Handle(ex =>
          {
            if (ex is OperationCanceledException)
            {
              OperationCanceledException cancelEx = ex as OperationCanceledException;

              if (token == cancelEx.CancellationToken)
              {
                Console.WriteLine("＊＊＊タスクがキャンセルされました＊＊＊");
                return true;
              }
            }

            return false;
          });
        }

        // 現在の状態を表示.
        Console.WriteLine("InitialCount={0}", cde.InitialCount);
        Console.WriteLine("CurrentCount={0}", cde.CurrentCount);
        Console.WriteLine("IsSet={0}", cde.IsSet);
      }
    }
  }
  #endregion
}

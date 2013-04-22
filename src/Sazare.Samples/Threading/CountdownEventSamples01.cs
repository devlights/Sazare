namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  #region CountDownEventSamples-01
  /// <summary>
  /// CountdownEventクラスについてのサンプルです。(1)
  /// </summary>
  /// <remarks>
  /// CountdownEventクラスは、.NET 4.0から追加されたクラスです。
  /// JavaのCountDownLatchクラスと同じ機能を持っています。
  /// </remarks>
  [Sample]
  public class CountdownEventSamples01 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      //
      // 初期カウントが1のCountdownEventオブジェクトを作成.
      //
      // この場合、どこかの処理にてカウントを一つ減らす必要がある。
      // カウントが残っている状態でWaitをしていると、いつまでたってもWaitを
      // 抜けることが出来ない。
      //
      using (CountdownEvent cde = new CountdownEvent(1))
      {
        // 初期の状態を表示.
        Console.WriteLine("InitialCount={0}", cde.InitialCount);
        Console.WriteLine("CurrentCount={0}", cde.CurrentCount);
        Console.WriteLine("IsSet={0}", cde.IsSet);

        Task t = Task.Factory.StartNew(() =>
        {
          Thread.Sleep(TimeSpan.FromSeconds(1));

          //
          // カウントをデクリメント.
          //
          // Signalメソッドを引数なしで呼ぶと、１つカウントを減らすことが出来る。
          // (指定した数分、カウントをデクリメントするオーバーロードも存在する。)
          //
          // CountdownEvent.CurrentCountが0の状態で、さらにSignalメソッドを呼び出すと
          // InvalidOperationException (イベントのカウントを 0 より小さい値にデクリメントしようとしました。)が
          // 発生する。
          //
          cde.Signal();
          cde.Signal(); // このタイミングで例外が発生する.
        });

        try
        {
          t.Wait();
        }
        catch (AggregateException aggEx)
        {
          foreach (Exception innerEx in aggEx.Flatten().InnerExceptions)
          {
            Console.WriteLine("ERROR={0}", innerEx.Message);
          }
        }

        //
        // カウントが0になるまで待機.
        //
        cde.Wait();

        // 現在の状態を表示.
        Console.WriteLine("InitialCount={0}", cde.InitialCount);
        Console.WriteLine("CurrentCount={0}", cde.CurrentCount);
        Console.WriteLine("IsSet={0}", cde.IsSet);
      }
    }
  }
  #endregion
}

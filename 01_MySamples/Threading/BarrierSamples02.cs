namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  #region BarrierSamples-02
  /// <summary>
  /// Barrierクラスについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// Barrierクラスは、.NET 4.0から追加されたクラスです。
  /// </remarks>
  public class BarrierSamples02 : IExecutable
  {
    // 計算値を保持する変数
    long _count;
    // キャンセルトークンソース.
    CancellationTokenSource _tokenSource;
    // キャンセルトークン.
    CancellationToken _token;

    public void Execute()
    {
      _tokenSource = new CancellationTokenSource();
      _token = _tokenSource.Token;

      //
      // 5つの処理を、特定のフェーズ毎に同期させながら実行.
      // さらに、フェーズ単位で途中結果を出力するようにするが
      // 5秒経過した時点でキャンセルを行う。
      //
      using (Barrier barrier = new Barrier(5, PostPhaseProc))
      {

        try
        {
          Parallel.Invoke(
            () => ParallelProc(barrier, 10, 123456, 2),
            () => ParallelProc(barrier, 20, 678910, 3),
            () => ParallelProc(barrier, 30, 749827, 5),
            () => ParallelProc(barrier, 40, 847202, 7),
            () => ParallelProc(barrier, 50, 503295, 777),
            () =>
            {
              //
              // 5秒間待機した後にキャンセルを行う.
              //
              Thread.Sleep(TimeSpan.FromSeconds(5));
              Console.WriteLine("■■■■　キャンセル　■■■■");
              _tokenSource.Cancel();
            }
          );
        }
        catch (AggregateException aggEx)
        {
          aggEx.Handle(HandleAggregateException);
        }
      }

      _tokenSource.Dispose();

      Console.WriteLine("最終値：{0}", _count);
    }

    //
    // 各並列処理用のアクション.
    //
    void ParallelProc(Barrier barrier, int randomMaxValue, int randomSeed, int modValue)
    {
      //
      // 第一フェーズ.
      //
      Calculate(barrier, randomMaxValue, randomSeed, modValue, 100);

      //
      // 第二フェーズ.
      //
      Calculate(barrier, randomMaxValue, randomSeed, modValue, 5000);

      //
      // 第三フェーズ.
      //
      Calculate(barrier, randomMaxValue, randomSeed, modValue, 10000);
    }

    //
    // 計算処理.
    //
    void Calculate(Barrier barrier, int randomMaxValue, int randomSeed, int modValue, int loopCountMaxValue)
    {
      Random rnd = new Random(randomSeed);
      Stopwatch watch = Stopwatch.StartNew();

      int loopCount = rnd.Next(loopCountMaxValue);
      Console.WriteLine("[Phase{0}] ループカウント：{1}, TASK:{2}", barrier.CurrentPhaseNumber, loopCount, Task.CurrentId);

      for (int i = 0; i < loopCount; i++)
      {
        //
        // キャンセル状態をチェック.
        // 別の場所にてキャンセルが行われている場合は
        // OperationCanceledExceptionが発生する.
        //
        _token.ThrowIfCancellationRequested();

        // 適度に時間がかかるように調整.
        if (rnd.Next(10000) % modValue == 0)
        {
          Thread.Sleep(TimeSpan.FromMilliseconds(10));
        }

        Interlocked.Add(ref _count, (i + rnd.Next(randomMaxValue)));
      }

      watch.Stop();
      Console.WriteLine("[Phase{0}] SignalAndWait -- TASK:{1}, ELAPSED:{2}", barrier.CurrentPhaseNumber, Task.CurrentId, watch.Elapsed);

      try
      {
        //
        // シグナルを発行し、仲間のスレッドが揃うのを待つ.
        //
        barrier.SignalAndWait(_token);
      }
      catch (BarrierPostPhaseException postPhaseEx)
      {
        //
        // Post Phaseアクションにてエラーが発生した場合はここに来る.
        // (本来であれば、キャンセルするなどのエラー処理が必要)
        //
        Console.WriteLine("*** {0} ***", postPhaseEx.Message);
        throw;
      }
      catch (OperationCanceledException cancelEx)
      {
        //
        // 別の場所にてキャンセルが行われた.
        //
        // 既に処理が完了してSignalAndWaitを呼び、仲間のスレッドを
        // 待っている状態でキャンセルが発生した場合は
        //    「操作が取り消されました。」となる。
        //
        // SignalAndWaitを呼ぶ前に、既にキャンセル状態となっている状態で
        // SignalAndWaitを呼ぶと
        //    「操作がキャンセルされました。」となる。
        //
        Console.WriteLine("キャンセルされました -- MESSAGE:{0}, TASK:{1}", cancelEx.Message, Task.CurrentId);
        throw;
      }
    }

    //
    // Barrierにて、各フェーズ毎が完了した際に呼ばれるコールバック.
    // (Barrierクラスのコンストラクタにて設定する)
    //
    void PostPhaseProc(Barrier barrier)
    {
      //
      // Post Phaseアクションは、同時実行している処理が全てSignalAndWaitを
      // 呼ばなければ発生しない。
      //
      // つまり、この処理が走っている間、他の同時実行処理は全てブロックされている状態となる。
      //
      long current = Interlocked.Read(ref _count);

      Console.WriteLine("現在のフェーズ：{0}, 参加要素数：{1}", barrier.CurrentPhaseNumber, barrier.ParticipantCount);
      Console.WriteLine("t現在値：{0}", current);

      //
      // 以下のコメントを外すと、次のPost Phaseアクションにて
      // 全てのSignalAndWaitを呼び出している、処理にてBarrierPostPhaseExceptionが
      // 発生する。
      //
      //throw new InvalidOperationException("dummy");
    }

    //
    // AggregateException.Handleメソッドに設定されるコールバック.
    //
    bool HandleAggregateException(Exception ex)
    {
      if (ex is OperationCanceledException)
      {
        OperationCanceledException cancelEx = ex as OperationCanceledException;
        if (_token == cancelEx.CancellationToken)
        {
          Console.WriteLine("＊＊＊Barrier内の処理がキャンセルされた MESSAGE={0} ＊＊＊", cancelEx.Message);
          return true;
        }
      }

      return false;
    }
  }
  #endregion
}

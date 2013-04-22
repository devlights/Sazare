namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  #region CountdownEventSamples-04
  /// <summary>
  /// CountdownEventクラスについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// CountdownEventクラスは、.NET 4.0から追加されたクラスです。
  /// </remarks>
  [Sample]
  public class CountdownEventSamples04 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      //
      // CountdownEventクラスには、以下のメソッドが存在する。
      //   ・AddCountメソッド
      //   ・Resetメソッド
      // AddCountメソッドは、CountdownEventの内部カウントをインクリメントする。
      // Resetメソッドは、現在の内部カウントをリセットする。
      //
      // どちらのメソッドも、Int32を引数に取るオーバーロードが用意されており
      // 指定した数を設定することも出来る。
      //
      // 尚、AddCountメソッドを利用する際の注意点として
      //   既に内部カウントが0の状態でAddCountを実行すると例外が発生する。
      // つまり、既にIsSetがTrue（シグナル状態）でAddCountするとエラーとなる。
      //

      //
      // 内部カウントが0の状態で、AddCountしてみる.
      //
      using (CountdownEvent cde = new CountdownEvent(0))
      {
        // 初期の状態を表示.
        PrintCurrentCountdownEvent(cde);

        try
        {
          //
          // 既にシグナル状態の場合に、さらにAddCountしようとすると例外が発生する.
          //
          cde.AddCount();
        }
        catch (InvalidOperationException invalidEx)
        {
          Console.WriteLine("＊＊＊ {0} ＊＊＊", invalidEx.Message);
        }

        // 現在の状態を表示.
        PrintCurrentCountdownEvent(cde);
      }

      Console.WriteLine("");

      using (CountdownEvent cde = new CountdownEvent(1))
      {
        // 初期の状態を表示.
        PrintCurrentCountdownEvent(cde);

        //
        // 10個の別処理を実行する.
        // それぞれの内部処理にてランダムでSLEEPして、終了タイミングをバラバラに設定.
        //
        Console.WriteLine("別処理開始・・・");

        for (int i = 0; i < 10; i++)
        {
          Task.Factory.StartNew(TaskProc, cde);
        }

        do
        {
          // 現在の状態を表示.
          PrintCurrentCountdownEvent(cde, "t");

          Thread.Sleep(TimeSpan.FromSeconds(2));
        }
        while (cde.CurrentCount != 1);

        Console.WriteLine("・・・別処理終了");

        //
        // 待機.
        //
        Console.WriteLine("メインスレッドにて最後のカウントをデクリメント");
        cde.Signal();
        cde.Wait();

        // 現在の状態を表示.
        PrintCurrentCountdownEvent(cde);

        Console.WriteLine("");

        //
        // 内部カウントをリセット.
        //
        Console.WriteLine("内部カウントをリセット");
        cde.Reset();

        // 現在の状態を表示.
        PrintCurrentCountdownEvent(cde);

        //
        // 待機.
        //
        Console.WriteLine("メインスレッドにて最後のカウントをデクリメント");
        cde.Signal();
        cde.Wait();

        // 現在の状態を表示.
        PrintCurrentCountdownEvent(cde);
      }
    }

    void PrintCurrentCountdownEvent(CountdownEvent cde, string prefix = "")
    {
      Console.WriteLine("{0}InitialCount={1}", prefix, cde.InitialCount);
      Console.WriteLine("{0}CurrentCount={1}", prefix, cde.CurrentCount);
      Console.WriteLine("{0}IsSet={1}", prefix, cde.IsSet);
    }

    void TaskProc(object data)
    {
      //
      // 処理開始と共に、CountdownEventの内部カウントをインクリメント.
      //
      CountdownEvent cde = data as CountdownEvent;
      cde.AddCount();

      Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(10)));

      //
      // 内部カウントをデクリメント.
      //
      cde.Signal();
    }
  }
  #endregion
}

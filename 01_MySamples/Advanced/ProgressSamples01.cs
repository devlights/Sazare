namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Windows;
  using System.Windows.Controls;

  #region ProgressSamples-01
  /// <summary>
  /// System.Progress<T>のサンプルです。
  /// </summary>
  /// <remarks>
  /// このクラスは、.NET Framework 4.5から追加された型です。
  /// </remarks>
  public class ProgressSamples01 : IExecutable
  {
    /// <summary>
    /// サンプル用ウィンドウ
    /// </summary>
    /// <remarks>
    /// このウィンドウには、ProgressBarが一つだけ配置されています。
    /// </remarks>
    class SampleWindow : Window
    {
      const double MIN = 0.0;
      const double MAX = 100.0;

      ProgressBar _bar;

      public SampleWindow()
      {
        InitializeControl();
        InitializeEvent();
      }

      void InitializeControl()
      {
        Width = 400;
        Height = 80;

        _bar = new ProgressBar
               {
                 Minimum = MIN
                 ,
                 Maximum = MAX
                 ,
                 Value = MIN
                 ,
                 SmallChange = MIN
               };

        Content = _bar;
      }

      void InitializeEvent()
      {
        //
        // ロードイベント.
        //   やってることは、単純にプログレスバーの進捗を伸ばしていくだけ.
        //   進捗を伸ばす部分に、Progress<T>を利用している.
        //
        //   内部でawaitを使用しているのでラムダにasyncを指定.
        //
        Loaded += async (s, e) =>
        {
          //
          // .NET Framework 4.5より、CancellationTokenSourceのコンストラクタに
          // キャンセル状態になるまでのタイムアウト値を設定できるようになった。
          // 下記の場合だと、5秒後に自動的にキャンセル扱いになる.
          //
          var tokenSource = new CancellationTokenSource(5000);

          //
          // プログレスバーの進捗を伸ばすためのProgress<T>を構築.
          // コンストラクタにActionを渡しても、ProgressChangedイベントにハンドラを設定してもどちらでも良い。
          //
          // Progress<T>は、インスタンス生成時に現在のSynchronizationContextをキャプチャする。
          // なので、UIが絡む処理を行う場合は、必ずUIスレッド上でインスタンスを生成する必要がある。
          //
          var progress = new Progress<int>(SetProgress);
          // わざと、UIスレッドではない場所でインスタンスを生成している.
          // これを下のPerformStepメソッドに渡すと、SetProgressメソッドに入ってきた時点で
          // Control.InvokeRequiredがtrueとなる。つまり、UIスレッド以外からSetProgressが
          // 呼ばれているので、Invokeしないといけない状態となる。
          //var progress2 = Task.Run(() => new Progress<int>(SetProgress)).Result;

          //
          // 処理開始.
          //   awaitを指定しているので、処理はUIスレッドと切り離されて実行され
          //   プログレスバーの進捗設定の時のみUIスレッドで実行される. (IProgress<T>.ReportからのSetProgress呼び出し)
          //   await後のタイトル設定は、PerformStepメソッドが終了次第実行される。
          //
          // 第二引数に渡しているprogressを、progress2に変更して実行すると
          // 画面上のプログレスバーの進捗が一切進まなくなる。
          // これは、progress2の方が、UIスレッドではない場所で生成されたため
          // キャプチャされたSynchronizationContextがDispatcherSynchronizationContextでは無いためである。
          //
          await PerformStep(tokenSource.Token, progress);
          Title = "DONE.";
        };
      }

      async Task PerformStep(CancellationToken token, IProgress<int> progress)
      {
        for (; ; )
        {
          foreach (var value in Enumerable.Range(0, 100))
          {
            if (token.IsCancellationRequested)
            {
              return;
            }

            await Task.Delay(10);

            //
            // Reportメソッドを呼び出すには
            // IProgress<T>にキャストして利用する必要がある。
            // (Progress<T>にて、明示的インターフェース実装されているため）
            //
            progress.Report(value);
          }
        }
      }

      void SetProgress(int newValue)
      {
        //
        // UIスレッドで実行されていない場合を視覚的に見たいので
        // UIスレッドで実行されていない場合はわざと何もしない.
        //
        if (!_bar.Dispatcher.CheckAccess())
        {
          return;
        }

        _bar.Value = newValue;
      }
    }

    [STAThread]
    public void Execute()
    {
      //
      // Progress<T>は、.NET Framework 4.5で追加された型である。
      // Progress<T>は、IProgress<T>インターフェースを実装しており
      // 文字通り、進捗状況を処理するために存在する。
      //
      // 利用する場合、コンストラクタにAction<T>を指定するか
      // ProgressChangedイベントをハンドルするかで処理できる。
      //
      // 尚、Progress<T>はインスタンスが作成される際に
      // 現在のSynchronizationContextをキャプチャし
      // ProgressChangedイベントをキャプチャしたSynchronizationContext上で
      // 実行してくれるため、イベントハンドラ内でコントロールを操作しても問題無い。
      // (インスタンスの作成自体をイベントスレッド以外で行っている場合は別）
      //
      // コンソールアプリのようにSynchronizationContextが紐づかない
      // コンテキストの場合は、ThreadPool上で実行される。
      //
      var app = new Application();
      app.Run(new SampleWindow());
    }
  }
  #endregion
}

namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Sazare.Common;

  #pragma warning disable 4014

  /// <summary>
  /// タスク並列ライブラリについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// タスク並列ライブラリは、.NET 4.0から追加されたライブラリです。
  /// Task.Runメソッドは、.NET 4.5から追加されたメソッドです。
  /// </remarks>
  [Sample]
  class TaskSamples05 : IExecutable
  {
    public void Execute()
    {
      //
      // .NET 4.5からタスクの開始方法に
      //    Task.Run
      // メソッドが追加された。
      // 以前のTask.Factory.StartNewメソッドと
      // 同様に利用する事ができる。
      //
      // http://msdn.microsoft.com/ja-jp/library/system.threading.tasks.task.run(v=vs.110).aspx
      //

      //
      // 引数にActionを指定（最もシンプルなパターン)
      //
      Task.Run(() => Output.WriteLine("Task.Run(Action)")).Wait();

      //
      // 引数にFuncを指定
      //
      var task1 = Task.Run(() => 100);
      Output.WriteLine(task1.Result);

      //
      // C# 5.0のasync/awaitを利用.
      //  サンプルの都合上RunAsyncの呼び出しはawaitしていない
      //
      RunAsync();

      //
      // 上の記述のAwaiter版
      //   サンプルの都合上わざとtask2の完了待ちをしている
      //
      var task2   = Task.Run(() => 300);
      var awaiter = task2.GetAwaiter();
      awaiter.OnCompleted(() =>
        {
          Output.WriteLine(awaiter.GetResult());
        }
      );

      task2.Wait();

      //
      // Task.RunメソッドにはAction, Funcを指定して、且つ、CancellationTokenを
      // 指定することもできる。
      //
      var tokenSource = new CancellationTokenSource();
      var token       = tokenSource.Token;

      var printDot = new Action(() =>
        {
          while (true)
          {
            if (token.IsCancellationRequested)
            {
              Output.WriteLine(string.Empty);
              break;
            }

            Output.Write(".");
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
          }
        }
      );

      var task3 = Task.Run(printDot, token);
      
      Thread.Sleep(TimeSpan.FromSeconds(3));
      tokenSource.Cancel();

      task3.Wait();
      tokenSource.Dispose();
    }

    async Task RunAsync()
    {
      int result = await Task.Run(() => 200);
      Output.WriteLine(result);
    }
  }
}

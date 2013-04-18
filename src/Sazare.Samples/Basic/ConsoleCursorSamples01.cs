namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  #region ConsoleCursorSamples-01
  /// <summary>
  /// Consoleクラスを利用してプログラムの実行状況を示すサンプルです。
  /// </summary>
  /// <remarks>
  /// このサンプルはEmEditor経由では動作できません。
  /// このクラスのソースコードを別ファイルに保存してコマンドラインにて
  /// 実行してください。
  ///</remarks>
  [Sample]
  public class ConsoleCursorSamples01 : IExecutable
  {
    volatile bool _stop;

    public void Execute()
    {
      //
      // Consoleクラスには、カーソル位置を操作するために
      // 以下のメソッドが利用できる。
      //
      //   ・SetCursorPosition : カーソル位置を設定
      //   ・CursorLeft    : 現在のカーソルの左位置(列)を取得
      //   ・CursorTop     : 現在のカーソルの上位置(行)を取得
      //
      // 上記のメソッドを利用する事で、Linuxなどでよく見かける
      // 処理中状態のカーソルを設定することが出来る。
      //
      Console.WriteLine("処理開始.......");

      ShowProgressMark();
      Thread.Sleep(TimeSpan.FromSeconds(5.0));

      _stop = true;

      Console.WriteLine(string.Empty);
      Console.WriteLine("終了");
    }

    void ShowProgressMark()
    {
      //
      // 現在のカーソル位置を保持.
      //
      int left = Console.CursorLeft;
      int top = Console.CursorTop;

      //
      // バッファに書き込み.
      //
      _stop = false;

      Task.Factory.StartNew(() =>
        {
          while (true)
          {
            if (_stop)
            {
              break;
            }

            Console.SetCursorPosition(left, top);
            Console.Write("|");
            Thread.Sleep(TimeSpan.FromMilliseconds(100.0));

            Console.SetCursorPosition(left, top);
            Console.Write("/");
            Thread.Sleep(TimeSpan.FromMilliseconds(100.0));

            Console.SetCursorPosition(left, top);
            Console.Write("-");
            Thread.Sleep(TimeSpan.FromMilliseconds(100.0));

            Console.SetCursorPosition(left, top);
            Console.Write("\\");
            Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
          }
        }
      );
    }
  }
  #endregion
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{
    #region ConsoleCursorSamples-01

    /// <summary>
    ///     Consoleクラスを利用してプログラムの実行状況を示すサンプルです。
    /// </summary>
    /// <remarks>
    ///     このサンプルはEmEditor経由では動作できません。
    ///     このクラスのソースコードを別ファイルに保存してコマンドラインにて
    ///     実行してください。
    /// </remarks>
    [Sample]
    public class ConsoleCursorSamples01 : IExecutable
    {
        private volatile bool _stop;

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
            Output.WriteLine("処理開始.......");

            ShowProgressMark();
            Thread.Sleep(TimeSpan.FromSeconds(5.0));

            _stop = true;

            Output.WriteLine(string.Empty);
            Output.WriteLine("終了");
        }

        private void ShowProgressMark()
        {
            //
            // 現在のカーソル位置を保持.
            //
            var left = Console.CursorLeft;
            var top = Console.CursorTop;

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
                    Output.Write("|");
                    Thread.Sleep(TimeSpan.FromMilliseconds(100.0));

                    Console.SetCursorPosition(left, top);
                    Output.Write("/");
                    Thread.Sleep(TimeSpan.FromMilliseconds(100.0));

                    Console.SetCursorPosition(left, top);
                    Output.Write("-");
                    Thread.Sleep(TimeSpan.FromMilliseconds(100.0));

                    Console.SetCursorPosition(left, top);
                    Output.Write("\\");
                    Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
                }
            }
                );
        }
    }

    #endregion
}
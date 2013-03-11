namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  #region スレッドを直接作成
  /// <summary>
  /// スレッドを直接作成するサンプル.
  /// </summary>
  public class ThreadSample : IExecutable
  {
    /// <summary>
    /// ロックオブジェクト
    /// </summary>
    object _lockObject = new object();
    /// <summary>
    /// 件数
    /// </summary>
    int _count = 0;

    /// <summary>
    /// スレッドを実行する際の引数として利用されるクラスです。
    /// </summary>
    class ThreadParameter
    {
      public int Count
      {
        get;
        set;
      }

      public DateTime Time
      {
        get;
        set;
      }
    }

    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      //
      // ThreadStartデリゲートを用いた場合.
      //
      ThreadStart ts = () =>
      {
        lock (_lockObject)
        {
          if (_count < 10)
          {
            _count++;
          }
        }

        Console.WriteLine("Count={0}", _count);
      };

      for (int i = 0; i < 15; i++)
      {
        Thread t = new Thread(ts);
        t.IsBackground = false;

        t.Start();

        //
        // 確実にスレッドの走る順序を揃えるには以下のようにする。
        // (もっともこれをやるとスレッドの意味がないが・・)
        //
        //t.Join();
      }

      //
      // ParameterizedThreadStartを用いた場合.
      //
      ParameterizedThreadStart pts = (data) =>
      {
        ThreadParameter p = data as ThreadParameter;
        Thread.Sleep(150);
        Console.WriteLine("Thread Count:{0}, Time:{1}", p.Count, p.Time.ToString("hh:mm:ss.fff"));
      };

      for (int i = 0; i < 15; i++)
      {
        Thread t = new Thread(pts);
        t.IsBackground = false;

        t.Start(new ThreadParameter
        {
          Count = i,
          Time = DateTime.Now
        });

        //
        // 確実にスレッドの走る順序を揃えるには以下のようにする。
        // (もっともこれをやるとスレッドの意味がないが・・)
        //
        //t.Join();
      }
    }
  }
  #endregion
}

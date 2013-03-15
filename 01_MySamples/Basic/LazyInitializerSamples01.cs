namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  #region LazyInitializerSamples-01
  public class LazyInitializerSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // LazyInitializerは、Lazyと同様に遅延初期化を行うための
      // クラスである。このクラスは、staticメソッドのみで構成され
      // Lazyでの記述を簡便化するために存在する。
      //
      // EnsureInitializedメソッドは
      // Lazyクラスにて、LazyThreadSafetyMode.PublicationOnlyを
      // 指定した場合と同じ動作となる。(race-to-initialize)
      //
      var hasHeavy = new HasHeavyData();

      Parallel.Invoke
      (
        () =>
        {
          Console.WriteLine("Created. [{0}]", hasHeavy.Heavy.CreatedThreadId);
        },
        () =>
        {
          Console.WriteLine("Created. [{0}]", hasHeavy.Heavy.CreatedThreadId);
        },
        // 少し待機してから、作成済みの値にアクセス.
        () =>
        {
          Thread.Sleep(TimeSpan.FromMilliseconds(2000));
          Console.WriteLine(">>少し待機してから、作成済みの値にアクセス.");
          Console.WriteLine(">>Created. [{0}]", hasHeavy.Heavy.CreatedThreadId);
        }
      );
    }

    class HasHeavyData
    {
      HeavyObject _heavy;

      public HeavyObject Heavy
      {
        get
        {
          //
          // LazyInitializerを利用して、遅延初期化.
          //
          Console.WriteLine("[ThreadId {0}] 値初期化処理開始. start", Thread.CurrentThread.ManagedThreadId);
          LazyInitializer.EnsureInitialized(ref _heavy, () => new HeavyObject(TimeSpan.FromMilliseconds(100)));
          Console.WriteLine("[ThreadId {0}] 値初期化処理開始. end", Thread.CurrentThread.ManagedThreadId);

          return _heavy;
        }
      }
    }

    class HeavyObject
    {
      int _threadId;

      public HeavyObject(TimeSpan waitSpan)
      {
        Console.WriteLine(">>>>>> HeavyObjectのコンストラクタ start. [{0}]", Thread.CurrentThread.ManagedThreadId);
        Initialize(waitSpan);
        Console.WriteLine(">>>>>> HeavyObjectのコンストラクタ end.   [{0}]", Thread.CurrentThread.ManagedThreadId);
      }

      void Initialize(TimeSpan waitSpan)
      {
        Thread.Sleep(waitSpan);
        _threadId = Thread.CurrentThread.ManagedThreadId;
      }

      public int CreatedThreadId
      {
        get
        {
          return _threadId;
        }
      }
    }
  }
  #endregion
}

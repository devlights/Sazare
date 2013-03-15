namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  #region AppDomainSamples-03
  /// <summary>
  /// AppDomainクラスのサンプルです。
  /// </summary>
  public class AppDomainSamples03 : IExecutable
  {
    // AppDomainのモニタリングを担当するクラス
    class AppDomainMonitor : IDisposable
    {
      static AppDomainMonitor()
      {
        //
        // AppDomain.MonitoringIsEnabledは、特殊なプロパティで
        // 以下の特徴を持つ。
        //
        // ・一度True（監視ON）にしたら、false（監視OFF）に戻すことはできない。
        // ・値がTrue,False関係なく、Falseを設定しようとすると例外が発生する。
        // ・設定は、AppDomain共通設定となり、特定のAppDomainのみの監視は行えない.
        //
        if (!AppDomain.MonitoringIsEnabled)
        {
          AppDomain.MonitoringIsEnabled = true;
        }
      }

      public void Dispose()
      {
        // フルブロッキングコレクションを実行.
        GC.Collect();
        PrintMonitoringValues(AppDomain.CurrentDomain);
      }

      public void PrintMonitoringValues(AppDomain domain)
      {
        //
        // モニタリングをONにすると、以下のプロパティにアクセスして統計情報を取得することができるようになる。
        //
        // ・MonitoringSurvivedMemorySize
        //    最後の完全なブロッキング コレクションの実行後に残された、現在のアプリケーション ドメインによって参照されていることが判明しているバイト数
        // ・MonitoringSurvivedProcessMemorySize
        //    最後の完全なブロッキング コレクションの実行後に残された、プロセス内のすべてのアプリケーション ドメインにおける合計バイト数
        // ・MonitoringTotalAllocatedMemorySize
        //    アプリケーション ドメインが作成されてから、そのアプリケーション ドメインで実行されたすべてのメモリ割り当ての合計サイズ（バイト単位）
        //    収集されたメモリは差し引かれない。
        // ・MonitoringTotalProcessorTime
        //    プロセスが開始されてから、現在のアプリケーション ドメインでの実行中にすべてのスレッドで使用された合計プロセッサ時間
        //
        // 完全なブロッキングコレクション（フルブロッキングコレクション）は、GC.Collectメソッドで実行できる。
        //
        Console.WriteLine("============================================");
        Console.WriteLine("MonitoringSurvivedMemorySize        = {0:N0}", domain.MonitoringSurvivedMemorySize);
        Console.WriteLine("MonitoringSurvivedProcessMemorySize = {0:N0}", AppDomain.MonitoringSurvivedProcessMemorySize);
        Console.WriteLine("MonitoringTotalAllocatedMemorySize  = {0:N0}", domain.MonitoringTotalAllocatedMemorySize);
        Console.WriteLine("MonitoringTotalProcessorTime        = {0}", domain.MonitoringTotalProcessorTime);
        Console.WriteLine("============================================");
      }
    }

    public void Execute()
    {
      using (AppDomainMonitor monitor = new AppDomainMonitor())
      {
        monitor.PrintMonitoringValues(AppDomain.CurrentDomain);

        List<string> aList = new List<string>();
        for (int i = 0; i < 1000; i++)
        {
          aList.Add(string.Format("hello world-{0:D2}", i));
        }

        monitor.PrintMonitoringValues(AppDomain.CurrentDomain);

        // CPUタイムを表示したいので、少しスピン.
        Thread.SpinWait(700000000);
      }
    }
  }
  #endregion
}

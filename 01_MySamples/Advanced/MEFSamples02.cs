namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.Composition;
  using System.ComponentModel.Composition.Hosting;
  using System.Linq;

  #region MEFSamples-02
  /// <summary>
  /// MEFについてのサンプルです。
  /// </summary>
  [Sample]
  public class MEFSamples02 : IExecutable
  {
    // Export用のインターフェース
    public interface IExporter
    {
      string Name { get; }
    }

    [Export(typeof(IExporter))]
    public class FirstExporter : IExporter
    {
      public string Name
      {
        get
        {
          return "☆☆ FIRST EXPORTER ☆☆";
        }
      }
    }

    [Export(typeof(IExporter))]
    public class SecondExporter : IExporter
    {
      public string Name
      {
        get
        {
          return "☆☆ SECOND EXPORTER ☆☆";
        }
      }
    }

    [Export(typeof(IExporter))]
    public class ThirdExporter : IExporter
    {
      public string Name
      {
        get
        {
          return "☆☆ THIRD EXPORTER ☆☆";
        }
      }
    }

    // Importパート (複数のExportを受け付ける）
    //
    // 通常、複数のExportを受け付ける場合は以下の書式で宣言する。
    //   IEnumerable<Lazy<T>>
    //
    // Lazy<T>を利用する事により、遅延ローディングが可能となる。
    // (利用しないExportパートが合成時にインスタンス化されるのを防ぐ）
    //
    // また、メタデータを利用する場合は以下のようになる。
    //   IEnumerable<Lazy<T, TMetaData>>
    //
    // 尚、明示的にnullを初期値として指定しているのは、そのままだとコンパイラによって警告扱いされるため
    [ImportMany(typeof(IExporter))]
    IEnumerable<Lazy<IExporter>> _exporters = null;

    // コンテナ.
    CompositionContainer _container;

    public void Execute()
    {
      //
      // カタログ構築.
      //  AggregateCatalogは、複数のCatalogを一つにまとめる役割を持つ。
      //
      var catalog = new AggregateCatalog();
      // AssemblyCatalogを利用して、自分自身のアセンブリをカタログに追加.
      catalog.Catalogs.Add(new AssemblyCatalog(typeof(MEFSamples01).Assembly));

      //
      // コンテナを構築.
      //
      _container = new CompositionContainer(catalog);
      try
      {
        // 合成実行.
        _container.ComposeParts(this);

        // 実行.
        foreach (Lazy<IExporter> lazyObj in _exporters)
        {
          Console.WriteLine(lazyObj.Value.Name);
        }

      }
      catch (CompositionException ex)
      {
        // 合成に失敗した場合.
        Console.WriteLine(ex.ToString());
      }

      if (_container != null)
      {
        _container.Dispose();
      }
    }
  }
  #endregion
}

namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.Composition;
  using System.ComponentModel.Composition.Hosting;
  using System.Linq;

  #region MEFSamples-01
  /// <summary>
  /// MEFについてのサンプルです。
  /// </summary>
  [Sample]
  public class MEFSamples01 : Sazare.Common.IExecutable
  {
    // Export用のインターフェース
    public interface IExporter
    {
      string Name { get; }
    }

    // Exportパート
    [Export(typeof(IExporter))]
    public class Exporter : IExporter
    {
      public string Name
      {
        get
        {
          return "☆☆☆☆☆☆☆ Exporter ☆☆☆☆☆☆☆";
        }
      }
    }

    // Importパート
    // 尚、明示的にnullを初期値として指定しているのは、そのままだとコンパイラによって警告扱いされるため
    [Import(typeof(IExporter))]
    IExporter _exporter = null;

    // コンテナ
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
        Console.WriteLine(_exporter.Name);
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

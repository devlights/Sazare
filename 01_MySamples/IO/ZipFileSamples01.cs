namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.IO.Compression;
  using System.Linq;

  #region ZipFileSamples-01
  /// <summary>
  /// System.IO.Compression.ZipFileクラスのサンプルです。
  /// </summary>
  /// <remarks>
  /// ZipFileクラスは、.NET Framework 4.5で追加されたクラスです。
  /// このクラスを利用するには、「System.IO.Compression.FileSystem.dll」を
  /// 参照設定に追加する必要があります。
  /// このクラスは、Metroアプリでは利用できません。
  /// Metroアプリでは、代わりにZipArchiveクラスを利用します。
  /// </remarks>
  public class ZipFileSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // ZipFileクラスは、ZIP形式のファイルを扱うためのクラスである。
      // 同じ事が出来るクラスとして、ZipArchiveクラスが存在するが
      // こちらは、きめ細かい処理が行えるクラスとなっており
      // ZipFileクラスは、ユーティリティクラスの扱いに近い。
      //
      // ZipFileクラスに定義されているメソッドは、全てstaticメソッドとなっている。
      //
      // 簡単に圧縮・解凍するためのメソッドとして
      //   ・CreateFromDirectory(string, string)
      //   ・ExtractToDirectory(string, string)
      // が用意されている。
      //
      // 尚、このクラスはMetroスタイルアプリ (新しい名前はWindows 8スタイルUI？)
      // では利用できないクラスである。Metroでは、ZipArchiveを利用することになる。
      // (http://msdn.microsoft.com/en-us/library/system.io.compression.zipfile)
      //

      //
      // 圧縮.
      //
      string srcDirectory = Environment.CurrentDirectory;
      string dstDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      string dstFilePath = Path.Combine(dstDirectory, "ZipTest.zip");

      if (File.Exists(dstFilePath))
      {
        File.Delete(dstFilePath);
      }

      ZipFile.CreateFromDirectory(srcDirectory, dstFilePath);

      //
      // 解凍.
      //
      string extractDirectory = Path.Combine(dstDirectory, "ZipTest");
      if (Directory.Exists(extractDirectory))
      {
        Directory.Delete(extractDirectory, recursive: true);
        Directory.CreateDirectory(extractDirectory);
      }

      ZipFile.ExtractToDirectory(dstFilePath, extractDirectory);
    }
  }
  #endregion
}

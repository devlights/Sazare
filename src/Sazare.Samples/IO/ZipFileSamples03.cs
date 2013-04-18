namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.IO.Compression;
  using System.Linq;

  #region ZipFileSamples-03
  /// <summary>
  /// System.IO.Compression.ZipFileクラスのサンプルです。
  /// </summary>
  /// <remarks>
  /// ZipFileクラスは、.NET Framework 4.5で追加されたクラスです。
  /// このクラスを利用するには、「System.IO.Compression.FileSystem.dll」を
  /// 参照設定に追加する必要があります。
  /// このクラスは、Metroアプリでは利用できません。
  /// Metroアプリでは、代わりにZipArchiveクラスを利用します。
  ///
  /// 尚、ZipArchiveクラスを利用する場合
  ///   System.IO.Compression.dll
  /// を参照設定に追加する必要があります。
  /// </remarks>
  [Sample]
  public class ZipFileSamples03 : IExecutable
  {
    string _zipFilePath;

    string DesktopPath
    {
      get
      {
        return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      }
    }

    public void Execute()
    {
      //
      // ZIPファイルの作成および更新.
      //   作成および更新の場合、ZipArchiveクラスを利用する.
      // 
      // ・エントリの追加： ZipArchive.CreateEntryFromFile OR ZipArchive.CreateEntry
      //
      // CreateEntryFromFileは、メソッドの名前が示す通り元ファイルがある場合に利用する。
      // 元となるファイルが存在する場合はこれが楽である。
      //
      // CreateEntryは、エントリのみを新規作成するメソッド。データは自前で流し込む必要がある。
      //
      Prepare();

      //
      // Zipファイルを新規作成.
      //
      using (var archive = ZipFile.Open(_zipFilePath, ZipArchiveMode.Create))
      {
        //
        // 元ファイルが存在している場合は、CreateEntryFromFileを利用するのが楽.
        //
        archive.CreateEntryFromFile("resources/Persons.txt", "Persons.txt");
      }

      //
      // Zipファイルの内容を更新.
      //
      using (var archive = ZipFile.Open(_zipFilePath, ZipArchiveMode.Update))
      {
        //
        // 元ファイルは存在するが、今度はCreateEntryメソッドで新規エントリのみを作成しデータは、手動で流し込む.
        //
        using (var reader = new BinaryReader(File.Open("resources/database.png", FileMode.Open)))
        {
          var newEntry = archive.CreateEntry("database.png");
          using (var writer = new BinaryWriter(newEntry.Open()))
          {
            WriteAllBytes(reader, writer);
          }
        }
      }

      File.Delete(_zipFilePath);
    }

    void Prepare()
    {
      _zipFilePath = Path.Combine(DesktopPath, "ZipTest2.zip");
      if (File.Exists(_zipFilePath))
      {
        File.Delete(_zipFilePath);
      }
    }

    void WriteAllBytes(BinaryReader reader, BinaryWriter writer)
    {
      try
      {
        for (; ; )
        {
          writer.Write(reader.ReadByte());
        }
      }
      catch (EndOfStreamException)
      {
        writer.Flush();
      }
    }
  }
  #endregion
}

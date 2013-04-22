namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Xml;
  using System.Xml.Linq;

  #region LinqSamples-56
  /// <summary>
  /// LINQ to XMLのサンプルです。
  /// </summary>
  /// <remarks>
  /// LINQ to XMLにてXMLファイルを新規作成するサンプルです.
  /// </remarks>
  [Sample]
  public class LinqSamples56 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      //
      // LINQ to XMLにてXMLを新規作成するには
      // 以下のどちらかのインスタンスを作成する必要がある.
      //   ・XDocument
      //   ・XElement
      // 通常、よく利用されるのはXElementの方となる.
      // 保存を行うには、Saveメソッドを利用する.
      // Saveメソッドには、以下のオーバーロードが存在する. (XElement)
      //   Save(Stream)
      //   Save(String)
      //   Save(TextWriter)
      //   Save(XmlWriter)
      //   Save(Stream, SaveOptions)
      //   Save(String, SaveOptions)
      //   Save(TextWriter, SaveOptions)
      //
      var element = new XElement("RootNode",
                          from i in Enumerable.Range(1, 10)
                          select new XElement("Child", i)
                    );

      //
      // Save(Stream)
      //
      using (var stream = new MemoryStream())
      {
        element.Save(stream);

        stream.Position = 0;
        using (var reader = new StreamReader(stream))
        {
          Console.WriteLine(reader.ReadToEnd());
        }
      }

      Console.WriteLine("===================================");

      //
      // Save(String)
      //
      var tmpFile = Path.GetRandomFileName();
      element.Save(tmpFile);
      Console.WriteLine(File.ReadAllText(tmpFile));
      File.Delete(tmpFile);

      Console.WriteLine("===================================");

      //
      // Save(TextWriter)
      //
      using (var writer = new UTF8StringWriter())
      {
        element.Save(writer);
        Console.WriteLine(writer);
      }

      Console.WriteLine("===================================");

      //
      // Save(XmlWriter)
      //
      using (var backingStore = new UTF8StringWriter())
      {
        using (var xmlWriter = XmlWriter.Create(backingStore, new XmlWriterSettings { Indent = true }))
        {
          element.Save(xmlWriter);
        }

        Console.WriteLine(backingStore);
      }

      Console.WriteLine("===================================");

      //
      // SaveOptions付きで書き込み.
      //   DisableFormattingを指定すると、出力されるXMLに書式が設定されなくなる.
      //
      using (var writer = new UTF8StringWriter())
      {
        element.Save(writer, SaveOptions.DisableFormatting);
        Console.WriteLine(writer);
      }
    }

    class UTF8StringWriter : StringWriter
    {
      public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
  }
  #endregion
}

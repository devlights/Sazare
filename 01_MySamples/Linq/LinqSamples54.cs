namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Xml;
  using System.Xml.Linq;

  #region LinqSamples-54
  /// <summary>
  /// LINQ to XMLのサンプルです。
  /// </summary>
  /// <remarks>
  /// XElement.Loadを利用したストリーム読み込みのサンプルです.
  /// </remarks>
  public class LinqSamples54 : IExecutable
  {
    public void Execute()
    {
      //
      // XElement.Loadには、文字列からロードする他に
      // ストリーミングを指定して内容をロードすることもできる。
      //
      // メソッドは複数のオーバーロードを持っており、以下の書式となる.
      //   Load(Stream)
      //   Load(TextReader)
      //   Load(XmlReader)
      //   Load(Stream, LoadOptions)
      //   Load(TextReader, LoadOptions)
      //   Load(XmlReader, LoadOptions)
      // 大別すると、ストリームのみを指定するものとオプションも指定できるものに分かれる.
      //

      //
      // Load(Stream)のサンプル.
      //   -- File.OpenReadで返るのはFileStream
      //      FileStreamはStreamのサブクラス.
      //
      XElement element = null;
      using (var stream = File.OpenRead("xml/Books.xml"))
      {
        element = XElement.Load(stream);
      }

      Console.WriteLine(element);
      Console.WriteLine("=============================================");

      //
      // Load(TextReader)のサンプル
      //   -- StreamReaderはTextReaderのサブクラス.
      //
      element = null;
      using (var reader = new StreamReader("xml/Data.xml"))
      {
        element = XElement.Load(reader);
      }

      Console.WriteLine(element);
      Console.WriteLine("=============================================");

      //
      // Load(XmlReader)のサンプル.
      //
      element = null;
      using (var reader = XmlReader.Create("xml/PurchaseOrder.xml", new XmlReaderSettings { IgnoreWhitespace = true, IgnoreComments = true }))
      {
        element = XElement.Load(reader);
      }

      Console.WriteLine(element);
    }
  }
  #endregion
}

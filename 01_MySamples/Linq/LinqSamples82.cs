namespace Gsf.Samples
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;
  using System.Xml.XPath;

  #region LinqSamples-82
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// XPath(System.Xml.XPath.Extensions)のサンプルです.
  /// LINQ to XMLとXPathの比較については
  ///   http://msdn.microsoft.com/ja-jp/library/vstudio/bb675178.aspx
  /// に詳細に記載されている.
  /// </remarks>
  public class LinqSamples82 : IExecutable
  {
    public void Execute()
    {
      //
      // XPathSelectElements
      //   XPath式を評価して、XElementを取得する
      //
      // LINQ to XMLには、XPath用の拡張メソッドが定義されたクラスが存在するため
      // それを利用する。以下のクラスである.
      //   System.Xml.XPath.Extensions
      // 尚、利用するにはSystem.Xml.XPathをusingしておく必要がある.
      //
      var root = BuildSampleXml();

      // XPath指定
      foreach (var elem in root.XPathSelectElements("Book/Title"))
      {
        Console.WriteLine("Value:{0}, Type:{1}", elem, elem.GetType().Name);
      }

      // XPath指定
      foreach (var elem in root.XPathSelectElements("//Title"))
      {
        Console.WriteLine("Value:{0}, Type:{1}", elem, elem.GetType().Name);
      }

      Console.WriteLine("=====================================");

      // LINQ to XML
      foreach (var elem in root.Elements("Book").Elements("Title"))
      {
        Console.WriteLine(elem);
      }

      // LINQ to XML
      foreach (var elem in root.Descendants("Title"))
      {
        Console.WriteLine(elem);
      }

      Console.WriteLine("=====================================");

      //
      // XPathEvaluate
      //   XPath式を評価して、結果を取得する
      //
      // LINQ to XMLには、XPath用の拡張メソッドが定義されたクラスが存在するため
      // それを利用する。以下のクラスである.
      //   System.Xml.XPath.Extensions
      // 尚、利用するにはSystem.Xml.XPathをusingしておく必要がある.
      //
      // XPathEvaluateメソッドは、戻り値がobjectになることに注意。
      //
      root = BuildSampleXml();

      // XPath指定
      foreach (var elem in (IEnumerable)root.XPathEvaluate("Book[@id=\"bk102\"]/PublishDate"))
      {
        Console.WriteLine("Value:{0}, Type:{1}", elem, elem.GetType().Name);
      }

      Console.WriteLine("=====================================");

      // LINQ to XML
      var query = from book in root.Elements("Book")
                  where book.Attribute("id").Value == "bk102"
                  select book.Element("PublishDate");

      foreach (var elem in query)
      {
        Console.WriteLine(elem);
      }
    }

    XElement BuildSampleXml()
    {
      //
      // サンプルXMLファイル
      //  see: http://msdn.microsoft.com/ja-jp/library/vstudio/ms256479(v=vs.90).aspx
      //
      return XElement.Load(@"xml/Books.xml");
    }
  }
  #endregion
}

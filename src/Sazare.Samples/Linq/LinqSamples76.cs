namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  #region LinqSamples-76
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// ナビゲーション(Descendants, Ancestorsメソッド)のサンプルです.
  /// </remarks>
  [Sample]
  public class LinqSamples76 : IExecutable
  {
    public void Execute()
    {
      //
      // Descendants(XName)
      //   現在の要素を起点として子孫要素を取得する.
      //   子孫の範囲は、直下だけでなく、ネストした子孫階層のデータも
      //   取得できる. Linq To XMLでよく利用するメソッドの一つ.
      //
      var root = BuildSampleXml();
      var elem = root.Descendants();

      Console.WriteLine("Count={0}", elem.Count());
      Console.WriteLine("=====================================");

      // "Customer"という名前の子孫要素を取得
      elem = root.Descendants("Customer");
      Console.WriteLine("Count={0}", elem.Count());
      Console.WriteLine("First item:");
      Console.WriteLine(elem.First());
      Console.WriteLine("=====================================");

      // 属性付きで絞り込み
      elem = root.Descendants("Customer").Where(x => x.Attribute("CustomerID").Value == "HUNGC");
      Console.WriteLine("Count={0}", elem.Count());
      Console.WriteLine("First item:");
      Console.WriteLine(elem.First());
      Console.WriteLine("=====================================");

      // クエリ式で利用
      elem = from node in root.Descendants("Customer")
             let attr = node.Attribute("CustomerID").Value
             where attr.StartsWith("L")
             from child in node.Descendants("Region")
             where child.Value == "CA"
             select node;

      Console.WriteLine("Count={0}", elem.Count());
      Console.WriteLine("First item:");
      Console.WriteLine(elem.First());
      Console.WriteLine("=====================================");

      // 直接2階層下の要素名を指定
      elem = from node in root.Descendants("Region")
             where node.Value == "CA"
             select node;

      Console.WriteLine("Count={0}", elem.Count());
      Console.WriteLine("First item:");
      Console.WriteLine(elem.First());
      Console.WriteLine("=====================================");

      //
      // Ancestors(XName)      
      //   現在の要素の先祖要素を取得する.
      //   兄弟要素は取得できない（件数が0件となる)
      //   あくまで自分の先祖となる要素を指定する.
      //
      root = BuildSampleXml();
      var startingPoint = root.Descendants("Region").Where(x => x.Value == "CA").First();

      var ancestors = startingPoint.Ancestors();

      Console.WriteLine("Count={0}", ancestors.Count());
      Console.WriteLine("First item:");
      Console.WriteLine(ancestors.First());
      Console.WriteLine("=====================================");

      // ContactNameは、現在の要素(Region)の先祖(FullAddress)ではないため指定しても取得できない
      ancestors = startingPoint.Ancestors("ContactName");

      Console.WriteLine("Count={0}", ancestors.Count());
      if (ancestors.Any())
      {
        Console.WriteLine("First item:");
        Console.WriteLine(ancestors.First());
      }

      Console.WriteLine("=====================================");

      // FullAddress要素の兄弟要素となるContactNameは取得できない
      startingPoint = root.Descendants("FullAddress").First();
      ancestors = startingPoint.Ancestors("ContactName");

      Console.WriteLine("Count={0}", ancestors.Count());
      if (ancestors.Any())
      {
        Console.WriteLine("First item:");
        Console.WriteLine(ancestors.First());
      }

      Console.WriteLine("=====================================");

      // FullAddress要素の先祖であるCustomer要素は取得できる.
      startingPoint = root.Descendants("FullAddress").First();
      ancestors = startingPoint.Ancestors("Customer");

      Console.WriteLine("Count={0}", ancestors.Count());
      if (ancestors.Any())
      {
        Console.WriteLine("First item:");
        Console.WriteLine(ancestors.First());
      }

      Console.WriteLine("=====================================");
    }

    XElement BuildSampleXml()
    {
      //
      // サンプルXMLファイル
      //  see: http://msdn.microsoft.com/ja-jp/library/vstudio/bb387025.aspx
      //
      return XElement.Load(@"xml/CustomersOrders.xml");
    }
  }
  #endregion
}

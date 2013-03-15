namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  #region LinqSamples-69
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// 名前空間 (XNamespace) のサンプルです.
  /// </remarks>
  public class LinqSamples69 : IExecutable
  {
    public void Execute()
    {
      //
      // 名前空間なし
      //   通常そのまま要素を作成すると名前空間無しとなる.
      //   名前空間無しの場合、XNamespace.Noneが設定されている.
      //   XName.Namespaceプロパティがnullにならないことは保証されている.
      //     http://msdn.microsoft.com/ja-jp/library/system.xml.linq.xnamespace.aspx
      //
      var root = BuildSampleXml();
      var name = root.Name;

      Console.WriteLine("is XNamespace.None?? == {0}", root.Name.Namespace == XNamespace.None);
      Console.WriteLine("=====================================");

      //
      // デフォルト名前空間あり
      //   元のXMLにデフォルト名前空間が設定されている場合
      //   取得したXElement -> XNameより名前空間が取得できる
      //
      //   デフォルト名前空間なので、要素を取得する際に名前空間の付与は
      //   必要ない。（そのまま取得できる)
      //
      root = BuildSampleXmlWithDefaultNamespace();
      name = root.Name;

      Console.WriteLine("XName.LocalName={0}", name.LocalName);
      Console.WriteLine("XName.Namespace={0}", name.Namespace);
      Console.WriteLine("XName.NamespaceName={0}", name.NamespaceName);
      Console.WriteLine("=====================================");

      //
      // デフォルト名前空間とカスタム名前空間あり
      //   デフォルト名前空間に関しては、上記の通り。
      //   カスタム名前空間の場合、要素を取得する際に
      //     XNamespace + "要素名"
      //   のように、名前空間を付与して取得する必要がある.
      //   カスタム名前空間内の要素は、XNamespaceを付与しないと
      //   取得できない.
      //
      root = BuildSampleXmlWithNamespace();
      name = root.Name;

      Console.WriteLine("XName.LocalName={0}", name.LocalName);
      Console.WriteLine("XName.Namespace={0}", name.Namespace);
      Console.WriteLine("XName.NamespaceName={0}", name.NamespaceName);

      if (root.Descendants("Value").Count() == 0)
      {
        Console.WriteLine("[Count=0] Namespaceが違うので、要素が取得できない.");
      }

      Console.WriteLine("=====================================");

      var ns = (XNamespace)"http://www.tmpurl.org/MyXml2";
      var elem = root.Descendants(ns + "Value").First();
      name = elem.Name;

      Console.WriteLine("XName.LocalName={0}", name.LocalName);
      Console.WriteLine("XName.Namespace={0}", name.Namespace);
      Console.WriteLine("XName.NamespaceName={0}", name.NamespaceName);
      Console.WriteLine("=====================================");

      //
      // 名前空間付きで要素作成 (プレフィックスなし)
      //   要素作成の際に、名前空間を付与するには
      //   予めXNamespaceを作成しておき、それを
      //      XNamespace + "要素"
      //   という風に、文字列を結合するような要領で利用する。
      //   XNamespaceは、暗黙で文字列から生成できる.
      //
      var defaultNamespace = (XNamespace)"http://www.tmpurl.org/Default";
      var customNamespace = (XNamespace)"http://www.tmpurl.org/Custom";

      var newElement = new XElement(
                         defaultNamespace + "RootNode",
                         Enumerable.Range(1, 3).Select(x => new XElement(customNamespace + "ChildNode", x))
                       );

      Console.WriteLine(newElement);
      Console.WriteLine("=====================================");

      //
      // 名前空間付きで要素作成 (プレフィックスあり)
      //   <ns:Node>xxx</ns:Node>
      // のように、要素に名前空間プレフィックスを付与するには
      // まず、プレフィックスを付与する要素を持つ親要素にて
      //   new XAttribute(XNamespace.Xmlns + "customs", "http://xxxxx/xxxx")
      // の属性を付与する。これにより、親要素にて
      //   <Root xmlns:customs="http://xxxxx/xxxx">
      // という感じになる。
      // 後は、プレフィックスを付与する要素にて通常通り
      //   new XElement(customNamespace + "ChildNode", x)
      // と定義することにより、自動的に合致するプレフィックスが設定される。
      // 
      newElement = new XElement(
                     defaultNamespace + "RootNode",
                     new XAttribute(XNamespace.Xmlns + "customns", "http://www.tmpurl.org/Custom"),
                     from x in Enumerable.Range(1, 3)
                     select new XElement(customNamespace + "ChildNode", x),
                     new XElement(defaultNamespace + "ChildNode", 4)
                   );

      Console.WriteLine(newElement);
      Console.WriteLine("=====================================");

      //
      // カスタム名前空間に属する要素を表示.
      //
      foreach (var e in newElement.Descendants(customNamespace + "ChildNode"))
      {
        Console.WriteLine(e);
      }

      Console.WriteLine("=====================================");

      //
      // デフォルト名前空間に属する要素を表示.
      //
      foreach (var e in newElement.Descendants(defaultNamespace + "ChildNode"))
      {
        Console.WriteLine(e);
      }

      Console.WriteLine("=====================================");

      //
      // 名前空間無しの要素を表示.
      //
      foreach (var e in newElement.Descendants("ChildNode"))
      {
        Console.WriteLine(e);
      }
    }

    XElement BuildSampleXml()
    {
      return XElement.Parse("<Root><Child Id=\"100\" Id2=\"200\"><Value Id=\"300\">hoge</Value></Child></Root>");
    }

    XElement BuildSampleXmlWithDefaultNamespace()
    {
      return XElement.Parse("<Root xmlns=\"http://www.tmpurl.org/MyXml\"><Child Id=\"100\" Id2=\"200\"><Value Id=\"300\">hoge</Value></Child></Root>");
    }

    XElement BuildSampleXmlWithNamespace()
    {
      return XElement.Parse("<Root xmlns=\"http://www.tmpurl.org/MyXml\" xmlns:x=\"http://www.tmpurl.org/MyXml2\"><Child Id=\"100\" Id2=\"200\"><x:Value Id=\"300\">hoge</x:Value></Child></Root>");
    }
  }
  #endregion
}
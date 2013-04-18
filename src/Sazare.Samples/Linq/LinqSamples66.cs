namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  #region LinqSamples-66
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// 属性更新系メソッドのサンプルです.
  /// </remarks>
  [Sample]
  public class LinqSamples66 : IExecutable
  {
    public void Execute()
    {
      //
      // XAttribute.Value
      //   XElement.Attribute(XName)を利用すると
      //   XAttributeオブジェクトが取得できる.
      //   XAttribute.Valueプロパティに値を設定することで
      //   属性の値が更新できる.
      //
      //   尚、Valueプロパティはstring型のみを受け付ける仕様と
      //   なっているので注意。
      //
      var root = BuildSampleXml();
      var elem = root.Elements("Child").First();

      var attr = elem.Attribute("Id");
      attr.Value = 500.ToString();

      Console.WriteLine(root);
      Console.WriteLine("=====================================");

      //
      // XAttribute.SetValue
      //   XAttribute.Valueと違い、こちらはobject型を受け付けるメソッド。
      //   内部で変換が行われた後、値が設定される.
      //
      root = BuildSampleXml();
      elem = root.Elements("Child").First();

      attr = elem.Attribute("Id");
      attr.SetValue(500);

      Console.WriteLine(root);
      Console.WriteLine("=====================================");

      //
      // SetAttributeValue
      //   すでに存在する要素を指定して、本メソッドを実行すると
      //   属性の値が更新される.
      //
      root = BuildSampleXml();
      elem = root.Elements("Child").First();

      elem.SetAttributeValue("Id", 500);

      Console.WriteLine(root);
      Console.WriteLine("=====================================");
    }

    XElement BuildSampleXml()
    {
      return XElement.Parse("<Root><Child Id=\"100\" Id2=\"200\"><Value Id=\"300\">hoge</Value></Child></Root>");
    }
  }
  #endregion
}

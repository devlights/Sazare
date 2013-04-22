namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  #region LinqSamples-68
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// 属性置換系メソッドのサンプルです.
  /// </remarks>
  [Sample]
  public class LinqSamples68 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      // 
      // ReplaceAttributes
      //   現在の要素に付属している属性を一括で置換する。
      //   ノードの置換に利用するReplaceNodesメソッドと同じ要領で
      //   利用できる。（クエリを利用しながら、置換用のシーケンスを作成する)
      //   
      var root = BuildSampleXml();
      var elem = root.Elements("Child").First();

      elem.ReplaceAttributes
        (
          from attr in elem.Attributes()
          where attr.Name.ToString().EndsWith("d")
          select new XAttribute(string.Format("{0}-Update", attr.Name), attr.Value)
        );

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

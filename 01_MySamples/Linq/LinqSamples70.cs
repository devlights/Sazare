namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  #region LinqSamples-70
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// 存在確認プロパティ (HasElements, HasAttributes) のサンプルです.
  /// </remarks>
  public class LinqSamples70 : IExecutable
  {
    public void Execute()
    {
      //
      // HasElements
      //   名前の通り、現在のノードがサブノードを持っているか否かを取得する.
      //
      var root = BuildSampleXml();
      var child = root.Elements("Child").First();
      var grandChild = child.Elements("Value").First();

      Console.WriteLine("root.HasElements: {0}", root.HasElements);
      Console.WriteLine("child.HasElements: {0}", child.HasElements);
      Console.WriteLine("grand-child.HasElements: {0}", grandChild.HasElements);

      Console.WriteLine("=====================================");

      //
      // HasAttributes
      //   名前の通り、現在のノードが属性を持っているか否かを取得する.
      //
      root = BuildSampleXml();
      child = root.Elements("Child").First();
      grandChild = child.Elements("Value").First();

      Console.WriteLine("root.HasAttributes:{0}", root.HasAttributes);
      Console.WriteLine("child.HasAttributes:{0}", child.HasAttributes);
      Console.WriteLine("grand-child.HasAttributes:{0}", grandChild.HasAttributes);

      Console.WriteLine("=====================================");
    }

    XElement BuildSampleXml()
    {
      return XElement.Parse("<Root><Child Id=\"100\" Id2=\"200\"><Value Id=\"300\">hoge</Value></Child></Root>");
    }
  }
  #endregion
}

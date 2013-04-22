namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  using Sazare.Common;
  
  #region LinqSamples-74
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// ナビゲーション(FirstNode, LastNodeプロパティ)のサンプルです.
  /// </remarks>
  [Sample]
  public class LinqSamples74 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      //
      // FirstNode
      //   現在の要素の最初の子要素を取得する
      //
      var root = BuildSampleXml();
      var elem = root.Elements("Child").First();

      Output.WriteLine(root.FirstNode);
      Output.WriteLine(elem.FirstNode);

      //
      // LastNode
      //   現在の要素の最後の子要素を取得する
      //
      root = BuildSampleXml();
      elem = root.Elements("Child").First();

      Output.WriteLine(root.LastNode);
      Output.WriteLine(elem.LastNode);
    }

    XElement BuildSampleXml()
    {
      var root = new XElement("Root",
        new XElement("Child", "value1"),
        new XElement("Child", "value2"),
        new XElement("Child", "value3"),
        new XElement("Child", "value4")
      );

      return root;
    }
  }
  #endregion
}

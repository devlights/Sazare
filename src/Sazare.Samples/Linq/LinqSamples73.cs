namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  using Sazare.Common;
  
  #region LinqSamples-73
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// ナビゲーション(PreviousNode, NextNodeプロパティ)のサンプルです.
  /// </remarks>
  [Sample]
  public class LinqSamples73 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      //
      // PreviousNode
      //   現在の要素の一つ前の兄弟要素を取得する
      //   一つ前の要素が存在しない場合は、nullとなる。
      //
      var root = BuildSampleXml();
      var elem = root.Elements("Child").Where(x => x.Value == "value2").First();

      Output.WriteLine("Prev node = {0}", elem.PreviousNode);

      elem = root.Elements("Child").First();
      Output.WriteLine("Prev node = {0}", elem.PreviousNode == null);

      //
      // NextNode
      //   現在の要素の一つ後の兄弟要素を取得する
      //   一つ後の要素が存在しない場合は、nullとなる
      //
      root = BuildSampleXml();
      elem = root.Elements("Child").Where(x => x.Value == "value3").First();

      Output.WriteLine("Next node = {0}", elem.NextNode);

      elem = root.Elements("Child").Last();
      Output.WriteLine("Next node = {0}", elem.NextNode == null);
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

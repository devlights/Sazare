namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  #region LinqSamples-71
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// 前後存在確認プロパティ (IsBefore, IsAfter) のサンプルです.
  /// </remarks>
  public class LinqSamples71 : IExecutable
  {
    public void Execute()
    {
      //
      // XNode.IsBefore(XNode)
      //   自分自身が引数に指定した要素の前に表示されるか否かを判定する
      //
      var root = BuildSampleXml();
      var elem1 = root.Elements("Child").Where(x => x.Value == "value1").First();
      var elem2 = root.Elements("Child").Where(x => x.Value == "value2").First();
      var elem4 = root.Elements("Child").Where(x => x.Value == "value4").First();

      Console.WriteLine("Child2 before Child1 = {0}", elem2.IsBefore(elem1));
      Console.WriteLine("Child4 before Child2 = {0}", elem4.IsBefore(elem2));
      Console.WriteLine("Child1 before Child2 = {0}", elem1.IsBefore(elem2));
      Console.WriteLine("Child1 before Child4 = {0}", elem1.IsBefore(elem4));
      Console.WriteLine("Child2 before Child4 = {0}", elem2.IsBefore(elem4));

      Console.WriteLine("=====================================");

      //
      // XNode.IsAfter(XNode)
      //   自分自身が引数に指定した要素の後に表示されるか否かを判定する
      //
      root = BuildSampleXml();
      elem1 = root.Elements("Child").Where(x => x.Value == "value1").First();
      elem2 = root.Elements("Child").Where(x => x.Value == "value2").First();
      elem4 = root.Elements("Child").Where(x => x.Value == "value4").First();

      Console.WriteLine("Child2 after Child1 = {0}", elem2.IsAfter(elem1));
      Console.WriteLine("Child4 after Child2 = {0}", elem4.IsAfter(elem2));
      Console.WriteLine("Child1 after Child2 = {0}", elem1.IsAfter(elem2));
      Console.WriteLine("Child1 after Child4 = {0}", elem1.IsAfter(elem4));
      Console.WriteLine("Child2 after Child4 = {0}", elem2.IsAfter(elem4));
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

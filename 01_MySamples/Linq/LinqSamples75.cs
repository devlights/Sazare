namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  #region LinqSamples-75
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// ナビゲーション(Parentプロパティ)のサンプルです.
  /// </remarks>
  [Sample]
  public class LinqSamples75 : IExecutable
  {
    public void Execute()
    {
      //
      // Parent
      //   文字通り、現在の要素の親要素を取得する.
      //   親要素が存在しない場合、nullとなる.
      //
      var root = BuildSampleXml();
      var elem = root.Elements("Child").First();

      Console.WriteLine("root.Parent = {0}", root.Parent == null ? "null" : root.Parent.ToString());
      Console.WriteLine("elem.Parent = {0}", elem.Parent);

      var newElem = new XElement("GrandChild", "value5");
      Console.WriteLine("newElem.Parent = {0}", newElem.Parent == null ? "null" : newElem.Parent.ToString());

      root.Elements("Child").Last().Add(newElem);
      Console.WriteLine("newElem.Parent = {0}", newElem.Parent);
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

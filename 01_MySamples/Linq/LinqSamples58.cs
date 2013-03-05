namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  #region LinqSamples-58
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// 要素のクローンとアタッチについてのサンプルです.
  /// </remarks>
  public class LinqSamples58 : IExecutable
  {
    public void Execute()
    {
      //
      // 親要素を持たない要素を作成し、特定のXMLツリーの中に組み込む. (アタッチ)
      //
      var noParent = new XElement("NoParent", true);
      var tree1 = new XElement("Parent", noParent);

      var noParent2 = tree1.Element("NoParent");
      Console.WriteLine("参照が同じ？ = {0}", noParent == noParent2);
      Console.WriteLine(tree1);

      // 値を変更して確認.
      noParent.SetValue(false);
      Console.WriteLine(noParent.Value);
      Console.WriteLine(tree1.Element("NoParent").Value);

      Console.WriteLine("==========================================");

      //
      // 親要素を持つ要素を作成し、特定のXMLツリーの中に組み込む. (クローン)
      //
      var origTree = new XElement("Parent", new XElement("WithParent", true));
      var tree2 = new XElement("Parent", origTree.Element("WithParent"));

      Console.WriteLine("参照が同じ？ = {0}", origTree.Element("WithParent") == tree2.Element("WithParent"));
      Console.WriteLine(tree2);

      // 値を変更して確認
      origTree.Element("WithParent").SetValue(false);
      Console.WriteLine(origTree.Element("WithParent").Value);
      Console.WriteLine(tree2.Element("WithParent").Value);
    }
  }
  #endregion
}

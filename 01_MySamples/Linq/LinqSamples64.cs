namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  #region LinqSamples-64
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// 属性取得系メソッドのサンプルです.
  /// </remarks>
  public class LinqSamples64 : IExecutable
  {
    public void Execute()
    {
      //
      // FirstAttribute
      //   現在の要素の最初の属性を取得する.
      //
      var root = BuildSampleXml();
      var elem = root.Elements("Child").First();

      var attr = elem.FirstAttribute;

      Console.WriteLine(attr);
      Console.WriteLine("{0}=\"{1}\"", attr.Name, attr.Value);
      Console.WriteLine("=====================================");

      //
      // LastAttribute
      //   現在の要素の最後の属性を取得する.
      //
      root = BuildSampleXml();
      elem = root.Elements("Child").First();

      attr = elem.LastAttribute;

      Console.WriteLine(attr);
      Console.WriteLine("=====================================");

      //
      // Attribute(XName)
      //   指定した名称を持つ属性を取得する.
      //
      root = BuildSampleXml();
      elem = root.Elements("Child").First();

      attr = elem.Attribute("Id2");

      Console.WriteLine(attr);
      Console.WriteLine(elem.Attribute("Id3") == null);
      Console.WriteLine("=====================================");

      //
      // Attributes()
      //   要素が持つ属性をすべて取得する.
      //
      root = BuildSampleXml();
      elem = root.Elements("Child").First();

      var attrs = elem.Attributes();

      Console.WriteLine("Count={0}", attrs.Count());
      foreach (var a in attrs)
      {
        Console.WriteLine("\t{0}", a);
      }

      Console.WriteLine("=====================================");

      //
      // Attributes(XName)
      //   指定した名称に一致する属性を取得する.
      //   主にXElementのシーケンスに対して利用する.
      //
      root = BuildSampleXml();
      var elems = root.Descendants();

      attrs = elems.Attributes("Id");

      Console.WriteLine("Count={0}", attrs.Count());
      foreach (var a in attrs)
      {
        Console.WriteLine("\t{0}", a);
      }

      Console.WriteLine("=====================================");
    }

    XElement BuildSampleXml()
    {
      return XElement.Parse("<Root><Child Id=\"100\" Id2=\"200\"><Value Id=\"300\">hoge</Value></Child></Root>");
    }
  }
  #endregion
}

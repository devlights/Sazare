namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  #region LinqSamples-83
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// XElementとXAttributeの値取得についてのTipです。
  /// </remarks>
  public class LinqSamples83 : IExecutable
  {
    public void Execute()
    {
      //
      // XElementとXAttributeの値はキャストしたら取得できる
      //   http://msdn.microsoft.com/ja-jp/library/vstudio/bb387049.aspx
      //
      // 対応しているのは、以下の型の場合.
      // string
      // bool,bool?
      // int,int?
      // uint,uint?
      // long,long?
      // ulong,ulong?
      // float,float?
      // double,double?
      // decimal,decimal?
      // DateTime,DateTime?
      // TimeSpan,TimeSpan?
      // GUID,GUID?
      //
      var root = BuildSampleXml();

      var title = (string)root.Descendants("Title").FirstOrDefault() ?? "Nothing";
      var attr = (string)root.Elements("Book").First().Attribute("id") ?? "Nothing";
      var noElem = (string)root.Descendants("NoElem").FirstOrDefault() ?? "Nothing";

      Console.WriteLine(title);
      Console.WriteLine(attr);
      Console.WriteLine(noElem);
    }

    XElement BuildSampleXml()
    {
      //
      // サンプルXMLファイル
      //  see: http://msdn.microsoft.com/ja-jp/library/vstudio/ms256479(v=vs.90).aspx
      //
      return XElement.Load(@"xml/Books.xml");
    }
  }
  #endregion
}

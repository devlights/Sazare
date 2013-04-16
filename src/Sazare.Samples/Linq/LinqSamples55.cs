namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml;
  using System.Xml.Linq;

  #region LinqSamples-55
  /// <summary>
  /// LINQ to XMLのサンプルです。
  /// </summary>
  /// <remarks>
  /// LINQ to XMLにてエラー発生時XmlExceptionが発生することを確認するサンプルです。
  /// </remarks>
  [Sample]
  public class LinqSamples55 : IExecutable
  {
    public void Execute()
    {
      try
      {
        //
        // LINQ to XMLは内部でXmlReaderを利用している.
        // なので、エラーが発生した場合、XmlReaderの場合と
        // 同様にXmlExceptionが発生する.
        //
        XElement.Parse(GetXmlStrings());
      }
      catch (XmlException xmlEx)
      {
        Console.WriteLine(xmlEx.ToString());
      }
    }

    string GetXmlStrings()
    {
      //
      // わざと解析エラーになるXML文字列を作成.
      //
      return @"<data>
                 <id>1</id>
                 <id>2</id>
               </dat>";
    }
  }
  #endregion
}

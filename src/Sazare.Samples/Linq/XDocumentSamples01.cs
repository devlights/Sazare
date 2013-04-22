namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  using Sazare.Common;
  
  #region XDocumentSamples-01
  /// <summary>
  /// XDocumentクラスについてのサンプルです。
  /// </summary>
  [Sample]
  public class XDocumentSamples01 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      //
      // XElementを構築する際、param引数には、直接XElementを設定しても、List<XElement>を指定しても問題ない。
      //
      var doc = new XDocument(new XElement("RootElement",
                        new XElement("Title", "gsf_zero1"),
                        new List<XElement> { new XElement("Age", 30), new XElement("Address", "kyoto, Kyoto, Japan") }));

      Output.WriteLine(doc);
    }
  }
  #endregion
}

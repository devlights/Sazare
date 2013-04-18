namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Windows.Markup;

  /// <summary>
  /// XamlWriterクラスのサンプルです。
  /// </summary>
  /// <remarks>
  /// 最もシンプルなSaveメソッドの利用方法についてです。
  /// </remarks>
  public class XamlWriterSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // 出力対象となるデータを作成.
      //
      var data = new XamlWriterSamples01_Data { IntValue = 100, StringValue = "hello world" };

      //
      // XamlWriterを利用して出力.
      //   ここで出力したファイルデータは、XAMLで利用できる形式となっている.
      //
      var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "XamlWriterSamples01.txt");
      using (var stream = File.Open(filePath, FileMode.Create))
      {
      	XamlWriter.Save(data, stream);
      }
    }
  }

  // サンプルで利用しているデータクラス
  public class XamlWriterSamples01_Data
  {
    public int IntValue { get; set; }
    public string StringValue { get; set; }
  }
}

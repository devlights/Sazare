namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Xml.Linq;

  #region LinqSamples-61
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// 要素更新系メソッドのサンプルです.
  /// </remarks>
  [Sample]
  public class LinqSamples61 : IExecutable
  {
    public void Execute()
    {
      //
      // Value { get; set; } プロパティ
      //   対象の要素の値を取得・設定する.
      //   型がstringとなっているので、文字列に変換して設定する必要がある。
      //   nullを指定すると例外が発生する。(ArgumentNullException)
      //
      var root = BuildSampleXml();
      var elem = root.Descendants("Value").First();

      Console.WriteLine("[before] {0}", elem.Value);
      elem.Value = "updated";
      Console.WriteLine("[after] {0}", elem.Value);
      Console.WriteLine(root);

      try
      {
        elem.Value = null;
      }
      catch (ArgumentNullException argNullEx)
      {
        Console.WriteLine(argNullEx.Message);
      }

      // Valueプロパティはstringを受け付けるので、boolなどの場合は
      // 明示的に文字列にして設定する必要がある.
      elem.Value = bool.TrueString.ToLower();
      Console.WriteLine(root);

      Console.WriteLine("=====================================");

      //
      // SetValue(object)
      //   要素の値を設定する。
      //   型がobjectとなっているので、文字列以外の場合でもそのまま設定可能。
      //   内部で変換される.
      //   nullを指定すると例外が発生する。(ArgumentNullException)
      //
      root = BuildSampleXml();
      elem = root.Descendants("Value").First();

      Console.WriteLine("[before] {0}", elem.Value);
      elem.SetValue("updated");
      Console.WriteLine("[after] {0}", elem.Value);
      Console.WriteLine(root);

      try
      {
        elem.SetValue(null);
      }
      catch (ArgumentNullException argNullEx)
      {
        Console.WriteLine(argNullEx.Message);
      }

      // SetValueメソッドは、object型を受け付けるので
      // bool型などの場合でもそのまま設定できる。内部で変換される.
      elem.SetValue(true);
      Console.WriteLine(root);

      Console.WriteLine("=====================================");

      //
      // SetElementValue(XName, object)
      //   子要素の値を設定する。
      //     要素が存在しない場合： 新規追加
      //     要素が存在する場合： 更新
      //     nullを設定した場合： 削除
      //   となる。自分自身の値を設定するわけでは無いことに注意。
      //
      root = BuildSampleXml();
      elem = root.Elements("Child").First();

      // 要素が存在する場合: 更新
      elem.SetElementValue("Value", "updated");
      Console.WriteLine(root);

      // nullを指定した場合： 削除
      root = BuildSampleXml();
      elem = root.Elements("Child").First();
      elem.SetElementValue("Value", null);
      Console.WriteLine(root);

      // 要素が存在しない場合: 新規追加
      root = BuildSampleXml();
      elem = root.Elements("Child").First();
      elem.SetElementValue("Value2", "inserted");
      Console.WriteLine(root);

      Console.WriteLine("=====================================");
    }

    XElement BuildSampleXml()
    {
      return new XElement("Root",
                   new XElement("Child",
                     new XAttribute("Id", 100),
                     new XElement("Value", "hoge")
                   )
                 );
    }
  }
  #endregion
}

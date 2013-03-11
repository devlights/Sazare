namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region String::IsNullOrWhiteSpaceメソッドのサンプル
  /// <summary>
  /// String.IsNullOrWhiteSpaceメソッドについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// .NET 4.0から追加されたメソッドです。
  /// </remarks>
  public class StringIsNullOrWhiteSpaceSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // String::IsNullOrWhiteSpaceメソッドは、IsNullOrEmptyメソッドの動作に
      // 加え、更に空白文字のみの場合もチェックしてくれる。
      //
      string nullStr = null;
      string emptyStr = string.Empty;
      string spaceStr = "    ";
      string normalStr = "hello world";
      string zenkakuSpaceStr = "　　　";

      //
      // String::IsNullOrEmptyでの結果.
      //
      Console.WriteLine("============= String::IsNullOrEmpty ==============");
      Console.WriteLine("nullStr   = {0}", string.IsNullOrEmpty(nullStr));
      Console.WriteLine("emptyStr  = {0}", string.IsNullOrEmpty(emptyStr));
      Console.WriteLine("spaceStr  = {0}", string.IsNullOrEmpty(spaceStr));
      Console.WriteLine("normalStr = {0}", string.IsNullOrEmpty(normalStr));
      Console.WriteLine("zenkakuSpaceStr = {0}", string.IsNullOrEmpty(zenkakuSpaceStr));

      //
      // String::IsNullOrWhiteSpaceでの結果.
      //  全角空白もスペースと見なされる点に注意。
      //
      Console.WriteLine("============= String::IsNullOrWhiteSpace ==============");
      Console.WriteLine("nullStr   = {0}", string.IsNullOrWhiteSpace(nullStr));
      Console.WriteLine("emptyStr  = {0}", string.IsNullOrWhiteSpace(emptyStr));
      Console.WriteLine("spaceStr  = {0}", string.IsNullOrWhiteSpace(spaceStr));
      Console.WriteLine("normalStr = {0}", string.IsNullOrWhiteSpace(normalStr));
      Console.WriteLine("zenkakuSpaceStr = {0}", string.IsNullOrWhiteSpace(zenkakuSpaceStr));
    }
  }
  #endregion
}

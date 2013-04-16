namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;

  #region CompareOptionsSamples-02
  /// <summary>
  /// CompareOptions列挙型のサンプルです。
  /// </summary>
  [Sample]
  public class CompareOptionsSamples02 : IExecutable
  {
    public void Execute()
    {
      //
      // string.Compareメソッドには、CultureInfoとCompareOptionsを
      // 引数にとるオーバーロードが定義されている。(他にもオーバーロードメソッドが存在します。)
      //
      // このオーバーロードを利用する際、CompareOptions.IgnoreKanaTypeを指定すると
      // 「ひらがな」と「カタカナ」の違いを無視して、文字列比較を行う事が出来る。
      //
      // さらに、CompareOptionsには、IgnoreWidthという値も存在し
      // これを指定すると、全角と半角の違いを無視して、文字列比較を行う事が出来る。
      //
      string ja1 = "ハローワールド";
      string ja2 = "ﾊﾛｰﾜｰﾙﾄﾞ";
      string ja3 = "はろーわーるど";

      CultureInfo ci = new CultureInfo("ja-JP");

      // 全角半角の違いを無視して、「ハローワールド」と「ﾊﾛｰﾜｰﾙﾄﾞ」を比較
      Console.WriteLine("{0}", string.Compare(ja1, ja2, ci, CompareOptions.IgnoreWidth).ToStringResult());
      // 全角半角の違いを無視して、「はろーわーるど」と「ﾊﾛｰﾜｰﾙﾄﾞ」を比較
      Console.WriteLine("{0}", string.Compare(ja3, ja2, ci, CompareOptions.IgnoreWidth).ToStringResult());
      // 全角半角の違いを無視し、且つ、ひらがなとカタカナの違いを無視して、「はろーわーるど」と「ﾊﾛｰﾜｰﾙﾄﾞ」を比較
      Console.WriteLine("{0}", string.Compare(ja3, ja2, ci, (CompareOptions.IgnoreWidth | CompareOptions.IgnoreKanaType)).ToStringResult());
    }
  }
  #endregion
}

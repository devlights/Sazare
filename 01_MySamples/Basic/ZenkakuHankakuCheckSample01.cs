namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  #region 全角チェックと半角チェック
  /// <summary>
  /// 全角チェックと半角チェックのサンプルです。
  /// </summary>
  /// <remarks>
  /// 単純な全角チェックと半角チェックを定義しています。
  /// </remarks>
  public class ZenkakuHankakuCheckSample01 : IExecutable
  {
    public void Execute()
    {
      string zenkakuOnlyStrings = "あいうえお";
      string hankakuOnlyStrings = "ｱｲｳｴｵ";
      string zenkakuAndHankakuStrings = "あいうえおｱｲｳｴｵ";

      Console.WriteLine("IsZenkaku:zenkakuOnly:{0}", IsZenkaku(zenkakuOnlyStrings));
      Console.WriteLine("IsZenkaku:hankakuOnlyStrings:{0}", IsZenkaku(hankakuOnlyStrings));
      Console.WriteLine("IsZenkaku:zenkakuAndHankakuStrings:{0}", IsZenkaku(zenkakuAndHankakuStrings));
      Console.WriteLine("IsHankaku:zenkakuOnly:{0}", IsHankaku(zenkakuOnlyStrings));
      Console.WriteLine("IsHankaku:hankakuOnlyStrings:{0}", IsHankaku(hankakuOnlyStrings));
      Console.WriteLine("IsHankaku:zenkakuAndHankakuStrings:{0}", IsHankaku(zenkakuAndHankakuStrings));
    }

    bool IsZenkaku(string value)
    {
      //
      // 指定された文字列が全て全角文字で構成されているか否かは
      // 文字列を一旦SJISに変換し取得したバイト数と元文字列の文字数＊２が
      // 成り立つか否かで決定できる。
      //
      return (Encoding.GetEncoding("sjis").GetByteCount(value) == (value.Length * 2));
    }

    bool IsHankaku(string value)
    {
      //
      // 指定された文字列が全て半角文字で構成されているか否かは
      // 文字列を一旦SJISに変換し取得したバイト数と元文字列の文字数が
      // 成り立つか否かで決定できる。
      //
      return (Encoding.GetEncoding("sjis").GetByteCount(value) == value.Length);
    }
  }
  #endregion
}

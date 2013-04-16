namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region EnumSamples-001
  /// <summary>
  /// Enumについてのサンプルです。
  /// </summary>
  [Sample]
  public class EnumSamples001 : IExecutable
  {
    //
    // Enumを定義.
    //
    // フラグ値としても利用する場合はFlagAttributeを付ける.
    //
    // 基になる型は明示的に指定しない場合はintとなる。
    // 列挙定数は２の累乗で定義する方がいい模様。（MSDNより）
    // 
    [Flags]
    private enum SampleEnum
    {
      Value1 = 1,
      Value2 = 2,
      Value3 = 4,
      Value4 = 16
    }

    public void Execute()
    {
      //
      // FlagsAttributeを付与している場合は
      // 単体の値としても利用できるが、AND OR XORの
      // 演算も行えるようになる。
      // 
      SampleEnum enum1 = SampleEnum.Value2;
      SampleEnum enum2 = (SampleEnum.Value1 | SampleEnum.Value3);

      Console.WriteLine(enum1);
      Console.WriteLine(enum2);

      Console.WriteLine("enum2 has Value3? == {0}", ((enum2 & SampleEnum.Value3) == SampleEnum.Value3));
      Console.WriteLine("enum2 has Value2? == {0}", ((enum2 & SampleEnum.Value2) == SampleEnum.Value2));

      /////////////////////////////////////////////////////////////
      //
      // System.Enumクラスには、列挙型を扱う上で便利なメソッドが
      // いくつか用意されている。
      //
      // ■Formatメソッド
      // ■GetNameメソッド
      // ■GetNamesメソッド
      // ■GetUnderlyingTypeメソッド
      // ■GetValuesメソッド
      // ■IsDefinedメソッド
      // ■Parseメソッド
      // ■ToObjectメソッド
      // ■ToStringメソッド
      //
      Console.WriteLine(string.Empty);

      //
      // Formatメソッド.
      //
      // 対象となる列挙値を特定のフォーマットにして取得する。
      // 指定出来るオプションは以下の通り。
      //
      // ■G or g: 名前を取得（但し、値が存在しない場合、１０進数でその値が返される）
      // ■X or x: １６進数で値を取得 (但し、0xは先頭に付与されない）
      // ■D or d: １０進数で値を取得
      // ■F or f: Gとほぼ同じ。
      //
      Console.WriteLine("============ {0} ============", "Format");
      Console.WriteLine(Enum.Format(typeof(SampleEnum), 2, "G"));
      Console.WriteLine(Enum.Format(typeof(SampleEnum), (2 | 3), "G"));
      Console.WriteLine(Enum.Format(typeof(SampleEnum), (SampleEnum.Value1 | SampleEnum.Value3), "G"));
      Console.WriteLine(Enum.Format(typeof(SampleEnum), SampleEnum.Value4, "X"));
      Console.WriteLine(Enum.Format(typeof(SampleEnum), SampleEnum.Value4, "D"));
      Console.WriteLine(Enum.Format(typeof(SampleEnum), SampleEnum.Value4, "F"));
      Console.WriteLine(Enum.Format(typeof(SampleEnum), (SampleEnum.Value1 | SampleEnum.Value4), "F"));

      //
      // GetNameメソッド
      //
      // 対象となる値から、対応する列挙値の名前を取得する.
      // 対応する列挙値が存在しない場合は、nullとなる。
      //
      Console.WriteLine("============ {0} ============", "GetName");
      int targetValue = 4;
      Console.WriteLine(Enum.GetName(typeof(SampleEnum), targetValue));
      Console.WriteLine(Enum.GetName(typeof(SampleEnum), -1) == null ? "null" : string.Empty);

      //
      // GetNamesメソッド
      //
      // 対象となる列挙型に定義されている値の名称を一気に取得する.
      //
      Console.WriteLine("============ {0} ============", "GetNames");
      string[] names = Enum.GetNames(typeof(SampleEnum));
      names.ToList().ForEach(Console.WriteLine);

      //
      // GetUnderlyingTypeメソッド
      //
      // 特定の列挙値が属する列挙型を取得する。
      //
      Console.WriteLine("============ {0} ============", "GetUnderlyingType");
      Enum enumVal = SampleEnum.Value2;
      Type enumType = enumVal.GetType();
      Type underlyingType = Enum.GetUnderlyingType(enumType);

      Console.WriteLine(enumType.Name);

      //
      // GetValuesメソッド
      //
      // 対象となる列挙型に設定されている値を一気に取得.
      //
      Console.WriteLine("============ {0} ============", "GetValues");
      Array valueArray = Enum.GetValues(typeof(SampleEnum));
      foreach (var element in valueArray)
      {
        Console.WriteLine(element);
      }

      //
      // IsDefinedメソッド
      //
      // 指定した値が、対象となる列挙型に存在するか否かを調査する。
      //
      Console.WriteLine("============ {0} ============", "IsDefined");
      Console.WriteLine("値{0}がSampleEnumに存在するか？ {1}", 2, Enum.IsDefined(typeof(SampleEnum), 2));
      Console.WriteLine("値{0}がSampleEnumに存在するか？ {1}", 10, Enum.IsDefined(typeof(SampleEnum), 10));

      //
      // Parseメソッド.
      //
      // 文字列から対応する列挙値を取得する。
      // 尚、該当文字列に対応する列挙値が存在しない場合はnullでなく
      // ArgumentExceptionが発生する。
      //
      // Parseメソッドには、以下のパターンのデータを指定することが出来る。
      // ■単一の値
      // ■列挙値の名前
      // ■名前をコンマで繋いだリスト
      //
      // 名前をコンマで繋いだリストを指定した場合は、該当する列挙値の
      // OR演算された結果が取得できる。
      //
      Console.WriteLine("============ {0} ============", "Parse");
      string testVal = "Value4";
      Console.WriteLine(Enum.Parse(typeof(SampleEnum), testVal));

      try
      {
        // 存在しない値を指定.
        Console.WriteLine(Enum.Parse(typeof(SampleEnum), "not_found"));
      }
      catch (ArgumentException)
      {
        Console.WriteLine("文字列 not_found に対応する列挙値が存在しない。");
      }

      testVal = "4";
      Console.WriteLine(Enum.Parse(typeof(SampleEnum), testVal));

      testVal = "Value1,Value2,Value4";
      Console.WriteLine(Enum.Parse(typeof(SampleEnum), testVal));

      //
      // ToObjectメソッド.
      //
      // 指定された値を対応する列挙値に変換する。
      // 各型に対応するためのオーバーロードメソッドが存在する。
      //
      Console.WriteLine("============ {0} ============", "ToObject");
      int v = 1;
      Console.WriteLine(Enum.ToObject(typeof(SampleEnum), v));

      //
      // ToStringメソッド.
      //
      // 対応する列挙値の文字列表現を取得する。
      // これまでに上述した各処理は全てEnumクラスのstaticメソッドで
      // あったが、このメソッドはインスタンスメソッドとなる。
      //
      // 基本的に、Enum.Formatメソッドに"G"を適用した結果となる。
      // （IFormatProviderを指定した場合はカスタム書式となる。）
      //
      Console.WriteLine("============ {0} ============", "ToString");
      SampleEnum e1 = SampleEnum.Value4;
      Console.WriteLine(e1.ToString());

    }
  }
  #endregion
}

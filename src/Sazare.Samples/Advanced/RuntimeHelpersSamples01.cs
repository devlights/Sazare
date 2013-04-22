namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.CompilerServices;

  #region RuntimeHelpersSamples-01
  /// <summary>
  /// RuntimeHelpersクラスのサンプルです。
  /// </summary>
  [Sample]
  public class RuntimeHelpersSamples01 : Sazare.Common.IExecutable
  {
    class SampleClass
    {
      public int Id { get; set; }

      public override int GetHashCode()
      {
        return Id.GetHashCode();
      }
    }

    public void Execute()
    {
      //
      // RuntimeHelpersクラスのGetHashCodeは、他のクラスのGetHashCodeメソッド
      // と挙動が少し違う。以下、MSDN(http://msdn.microsoft.com/ja-jp/library/11tbk3h9.aspx)に
      // ある記述を引用。
      //
      // ・Object.GetHashCode は、オブジェクト値を考慮するシナリオで便利です。 同じ内容の 2 つの文字列は、Object.GetHashCode で同じ値を返します。
      // ・RuntimeHelpers.GetHashCode は、オブジェクト識別子を考慮するシナリオで便利です。 同じ内容の 2 つの文字列は、内容が同じでも異なる文字列オブジェクトであるため、RuntimeHelpers.GetHashCode で異なる値を返します。
      //
      // 以下では、サンプルとなるオブジェクトを2つ作成し、ハッシュコードを出力するようにしている。
      // サンプルクラスでは、GetHashCodeメソッドをオーバーライドしており、Idプロパティのハッシュコードを
      // 返すようにしている。
      //   (注意) このクラスのGetHashCodeメソッドの実装は、サンプルのために簡略化してあります。
      //         実際の実装で、このようなハッシュコードの算出はしてはいけません。
      //
      // 以下の場合、Object.GetHashCodeを呼び出している場合は当然ながら同じハッシュコードとなるが
      // RuntimeHelpers.GetHashCodeを呼び出している場合、違うハッシュコードとなる.
      //
      SampleClass sampleObj1 = new SampleClass { Id = 100 };
      SampleClass sampleObj2 = new SampleClass { Id = 100 };

      Console.WriteLine("[Object.GetHashCode]        sampleObj1 = {0}, sampleObj2 = {1}", sampleObj1.GetHashCode(), sampleObj2.GetHashCode());
      Console.WriteLine("[RuntimeHelper.GetHashCode] sampleObj1 = {0}, sampleObj2 = {1}", RuntimeHelpers.GetHashCode(sampleObj1), RuntimeHelpers.GetHashCode(sampleObj2));

      //
      // 文字列データで検証.
      // 以下は、文字列のハッシュコードが異なるか否かを検証.
      // 変数s1, s2を作成してから、連結して文字列値を作成している理由は
      // CLRによって、内部で文字列がインターン(Intern)されないようにするため.
      //
      // 文字列がInternされていない場合、RuntimeHelpers.GetHashCodeメソッドは
      // 違う値を返す。Object.GetHashCodeは同じハッシュコードを返す.
      //
      string s1 = "hello ";
      string s2 = "world";
      string test1 = s1 + s2;
      string test2 = s1 + s2;

      Console.WriteLine("[Object.GetHashCode]        test1 = {0}, test2 = {1}", test1.GetHashCode(), test2.GetHashCode());
      Console.WriteLine("[RuntimeHelper.GetHashCode] test1 = {0}, test2 = {1}", RuntimeHelpers.GetHashCode(test1), RuntimeHelpers.GetHashCode(test2));

      //
      // 文字列データで検証
      // 以下は、CLRによって文字列がインターンされる値に対してハッシュコードを取得している.
      //
      // この場合、RuntimeHelpers.GetHashCodeでも同じハッシュコードが返ってくる.
      // 尚、CLRによって値がインターンされるのはリテラルだけである.
      // 連結操作によって作成された文字列はインターンされない.
      // 無理矢理インターンするには、String.Internメソッドを利用する.
      //
      string test3 = "hello world";
      string test4 = "hello world";

      Console.WriteLine("[Object.GetHashCode]        test3 = {0}, test4 = {1}", test3.GetHashCode(), test4.GetHashCode());
      Console.WriteLine("[RuntimeHelper.GetHashCode] test3 = {0}, test4 = {1}", RuntimeHelpers.GetHashCode(test3), RuntimeHelpers.GetHashCode(test4));
    }
  }
  #endregion
}

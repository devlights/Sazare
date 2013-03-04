namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region LinqSamples-45
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples45 : IExecutable
  {
    public void Execute()
    {
      var languages = new string[] { "csharp", "visualbasic", "java", "python", "ruby", "php", "c++" };

      //
      // First拡張メソッドは、シーケンスの最初の要素を返すメソッド。
      //
      // predicateを指定した場合は、その条件に合致する最初の要素が返る。
      //
      Console.WriteLine("============ First ============");
      Console.WriteLine(languages.First());
      Console.WriteLine(languages.First(item => item.StartsWith("v")));

      //
      // Last拡張メソッドは、シーケンスの最後の要素を返すメソッド。
      //
      // predicateを指定した場合は、その条件に合致する最後の要素が返る。
      //
      Console.WriteLine("============ Last ============");
      Console.WriteLine(languages.Last());
      Console.WriteLine(languages.Last(item => item.StartsWith("p")));

      //
      // Single拡張メソッドは、シーケンスの唯一の要素を返すメソッド。
      //
      // Singleを利用する場合、対象となるシーケンスには要素が一つだけ
      // でないといけない。複数の要素が存在する場合例外が発生する。
      //
      // また、空のシーケンスに対してSingleを呼ぶと例外が発生する。
      //
      // predicateを指定した場合は、その条件に合致するシーケンスの唯一の
      // 要素が返される。この場合も、結果のシーケンスには要素が一つだけの
      // 状態である必要がある。条件に合致する要素が複数であると例外が発生する、
      //
      Console.WriteLine("============ Single ============");
      var onlyOne = new string[] { "csharp" };
      Console.WriteLine(onlyOne.Single());

      try
      {
        languages.Single();
      }
      catch
      {
        Console.WriteLine("複数の要素が存在する状態でSingleを呼ぶと例外が発生する。");
      }

      Console.WriteLine(languages.Single(item => item.EndsWith("y")));

      try
      {
        languages.Single(item => item.StartsWith("p"));
      }
      catch
      {
        Console.WriteLine("条件に合致する要素が複数存在する場合例外が発生する。");
      }
    }
  }
  #endregion
}

namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region ExpandoObjectクラスのサンプル-03
  /// <summary>
  /// ExpandoObjectについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// .NET 4.0から追加されたクラスです。
  /// </remarks>
  public class ExpandoObjectSamples03 : IExecutable
  {
    public void Execute()
    {
      ///////////////////////////////////////////////////////////////////////
      //
      // ExpandoObjectをDictionaryとして扱う. (メンバーの追加/削除)
      //   ExpandoObjectはIDictionary<string, object>を実装しているので
      //   Dictionaryとしても利用出来る.
      //
      dynamic obj = new System.Dynamic.ExpandoObject();
      obj.Name = "gsf_zero1";
      obj.Age = 30;

      //
      // 定義されているメンバーを列挙.
      //
      IDictionary<string, object> map = obj as IDictionary<string, object>;
      foreach (var pair in map)
      {
        Console.WriteLine("{0}={1}", pair.Key, pair.Value);
      }

      //
      // Ageメンバーを削除.
      //
      map.Remove("Age");

      //
      // 確認.
      //
      foreach (var pair in map)
      {
        Console.WriteLine("{0}={1}", pair.Key, pair.Value);
      }

      // エラーとなる.
      //Console.WriteLine(obj.Age);
    }
  }
  #endregion
}

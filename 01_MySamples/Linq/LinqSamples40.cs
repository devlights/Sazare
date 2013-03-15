namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region LinqSamples-40
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples40 : IExecutable
  {
    public void Execute()
    {
      //
      // All拡張メソッドは、シーケンスの全要素が指定された条件に合致しているか否かを判別するメソッドである。
      //
      // 引数には条件としてpredicateを指定する。
      // このメソッドは、対象シーケンス内の全要素が条件に合致している場合のみTrueを返す。
      // (逆にAny拡張メソッドは、一つでも合致するものが存在した時点でTrueとなる。)
      //
      var names = new string[] { "gsf_zero1", "gsf_zero2", "gsf_zero3", "2222" };

      Console.WriteLine("Allメソッドの結果 = {0}", names.All(item => Char.IsDigit(item.Last())));
      Console.WriteLine("Allメソッドの結果 = {0}", names.All(item => item.StartsWith("g")));
      Console.WriteLine("Allメソッドの結果 = {0}", names.All(item => !string.IsNullOrEmpty(item)));
    }
  }
  #endregion
}

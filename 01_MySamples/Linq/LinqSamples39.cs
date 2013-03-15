namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region LinqSamples-39
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples39 : IExecutable
  {
    public void Execute()
    {
      var numbers = new int[] { 1, 2, 3 };

      // 
      // Any拡張メソッドは、一つでも条件に当てはまるものが存在するか否かを判別するメソッドである。
      // この拡張メソッドは、引数無しのバージョンと引数にpredicateを渡すバージョンの２つが存在する。
      //
      // 引数を渡さずAny拡張メソッドを呼んだ場合、Any拡張メソッドは
      // 該当シーケンスに要素が存在するか否かのみで判断する。
      // つまり、要素が一つでも存在する場合は、Trueとなる。
      //
      // 引数にpredicateを指定するバージョンは、シーケンスの各要素に対してpredicateを適用し
      // 一つでも条件に合致するものが存在した時点で、Trueとなる。
      //
      Console.WriteLine("=========== 引数無しでAny拡張メソッドを利用 ===========");
      Console.WriteLine("要素有り? = {0}", numbers.Any());
      Console.WriteLine("================================================");

      Console.WriteLine("=========== predicateを指定してAny拡張メソッドを利用 ===========");
      Console.WriteLine("要素有り? = {0}", numbers.Any(item => item >= 5));
      Console.WriteLine("要素有り? = {0}", numbers.Any(item => item <= 5));
      Console.WriteLine("================================================================");
    }
  }
  #endregion
}

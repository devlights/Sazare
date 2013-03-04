namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region LinqSamples-35
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples35 : IExecutable
  {
    public void Execute()
    {
      var numbers = new int[]
                {
                  1, 2, 3, 4, 5, 6, 7, 8, 9, 10
                };

      // 
      // Average拡張メソッドは、文字通り平均を求める拡張メソッド。
      //
      // Average拡張メソッドには、各基本型のオーバーロードが用意されており
      // (decimal, double, int, long, single及びそれぞれのNullable型)
      // それぞれに、引数無しとselectorを指定するバージョンのメソッドがある。
      //

      //
      // 引数無しのAverage拡張メソッドの使用.
      //
      Console.WriteLine("引数無し = {0}", numbers.Average());

      //
      // selectorを指定するAverage拡張メソッドの使用.
      //
      Console.WriteLine("引数有り = {0}", numbers.Average(item => (item % 2 == 0) ? item : 0));
    }
  }
  #endregion
}

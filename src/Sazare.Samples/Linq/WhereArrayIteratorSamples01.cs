namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region WhereArrayIteratorSamples-01
  /// <summary>
  /// WhereArrayIteratorに関するサンプルです。
  /// </summary>
  [Sample]
  public class WhereArrayIteratorSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // whereを含んだlinqを作成すると, WhereArrayIteratorとなる。
      //
      var subset = from i in new[] { 1, 2, 3, 5, 6, 7 }
                   where i < 7
                   select i;

      Console.WriteLine("{0}", subset.GetType().Name);
      Console.WriteLine("{0}", subset.GetType().Namespace);

      //
      // whereを含まないlinqを作成すると、WhereSelectArrayIteratorとなる。
      //
      var subset2 = from i in new[] { 1, 2, 3, 5, 6, 7 }
                    select i;

      Console.WriteLine("{0}", subset2.GetType().Name);
      Console.WriteLine("{0}", subset2.GetType().Namespace);
    }
  }
  #endregion
}

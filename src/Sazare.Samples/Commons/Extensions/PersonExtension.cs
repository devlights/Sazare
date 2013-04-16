namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region LinqSamples-18 AND LinqSamples-19 AND 拡張メソッド解決

  public static class PersonExtension
  {
    public static Persons Where(this Persons self, Func<Person, bool> predicate)
    {
      var result = new Persons();

      Console.WriteLine("========= WHERE ========");
      foreach (var aPerson in self)
      {
        if (predicate(aPerson))
        {
          result.Add(aPerson);
        }
      }

      return result;
    }
  }
  #endregion
}

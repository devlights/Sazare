namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region LinqSamples-15
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  [Sample]
  public class LinqSamples15 : Sazare.Common.IExecutable
  {
    class Person
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Team { get; set; }
    }

    public void Execute()
    {
      var persons = new List<Person>
        {
           new Person{ Id = 1, Name = "gsf_zero1", Team = "TeamA" }
          ,new Person{ Id = 2, Name = "gsf_zero2", Team = "TeamB" }
          ,new Person{ Id = 3, Name = "gsf_zero3", Team = "TeamA" }
          ,new Person{ Id = 4, Name = "gsf_zero4", Team = "TeamA" }
          ,new Person{ Id = 5, Name = "gsf_zero5", Team = "TeamB" }
          ,new Person{ Id = 6, Name = "gsf_zero6", Team = "TeamC" }
        };

      var query = from aPerson in persons
                  select aPerson;

      //
      // Teamの値をキーとして、グルーピング処理.
      // これにより、各キー毎に合致するPersonオブジェクトが紐付いた構造に変換される。
      //
      // 実際のオブジェクトの型は
      //   Lookup<Grouping<string, Person>>
      // となっている。
      //
      // 尚、Lookupオブジェクトを外部から新規で構築することはできない。
      //
      // 以下では、keySelectorを指定している。
      ILookup<string, Person> lookup = query.ToLookup(aPerson => aPerson.Team);

      //
      // ILookupに定義されているプロパティにアクセス.
      //   通常、Lookupオブジェクトはループを経由してデータを取得するが、キーを指定して、アクセスすることもできる。
      //
      Console.WriteLine("カウント={0}", lookup.Count);
      foreach (Person teamAPerson in lookup["TeamA"])
      {
        Console.WriteLine(teamAPerson);
      }

      //
      // ILookup<TKey, TElement>は、IEnumerable<IGrouping<TKey, TElement>>を継承しているので
      // ループさせることで、IGrouping<TKey, TElement>を取得することができる。
      //
      // このIGrouping<TKey, TElement>が一対多のマッピングを実現している。
      // 尚、IGrouping<TKey, TElement>は、クエリ式にてgroup byを行った際にも取得できる。
      //
      Console.WriteLine("=========== ToLookupに対してkeySelectorを指定した版 =============");
      foreach (IGrouping<string, Person> grouping in lookup)
      {
        Console.WriteLine("KEY={0}", grouping.Key);
        foreach (var aPerson in grouping)
        {
          Console.WriteLine("\tID={0}, NAME={1}", aPerson.Id, aPerson.Name);
        }
      }

      //
      // keySelectorとelementSelectorを指定してみる。
      //
      var lookup2 = query.ToLookup(aPerson => aPerson.Team, aPerson => string.Format("{0}_{1}", aPerson.Id, aPerson.Name));

      Console.WriteLine("=========== ToLookupに対してkeySelectorとelementSelectorを指定した版 =============");
      foreach (var grouping in lookup2)
      {
        Console.WriteLine("KEY={0}", grouping.Key);
        foreach (var element in grouping)
        {
          Console.WriteLine("\t{0}", element);
        }
      }
    }
  }
  #endregion
}

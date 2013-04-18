namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region LinqSamples-25
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  [Sample]
  public class LinqSamples25 : IExecutable
  {
    class Person
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Team { get; set; }
      public string Project { get; set; }

      public override string ToString()
      {
        return string.Format("[ID={0}, NAME={1}]", Id, Name);
      }
    }

    public void Execute()
    {
      var persons = new List<Person>
                {
                  new Person{ Id = 1001, Name = "gsf_zero1", Team = "A", Project = "P1" },
                  new Person{ Id = 1000, Name = "gsf_zero2", Team = "B", Project = "P1" },
                  new Person{ Id = 111,  Name = "gsf_zero3", Team = "B", Project = "P2" },
                  new Person{ Id = 9889, Name = "gsf_zero4", Team = "C", Project = "P2" },
                  new Person{ Id = 9889, Name = "gsf_zero5", Team = "A", Project = "P1" },
                  new Person{ Id = 100,  Name = "gsf_zero6", Team = "C", Project = "P2" }
                };

      //
      // GroupBy拡張メソッドは、シーケンスの要素をグループ化する際に利用できる。
      // クエリ式にて、[group xx by xx.xx]とする場合、実行時にGroupBy拡張メソッドに置き換えられる。
      //
      // 概念としては、SQLのGROUP BYと同じである。
      //
      // GroupBy拡張メソッドには、全部で8つのオーバーロードが存在する。
      // よく利用されるのは、以下のものとなると思われる。
      //
      //   ・keySelectorのみを指定するもの
      //   ・keySelectorとelementSelectorを指定するもの。
      //   ・複合キーでのグループ化
      //
      // GroupBy拡張メソッドの戻り値は、
      //   IEnumerable<IGrouping<TKey, TElement>>
      // となる。IGroupingインターフェースは、グルーピングを表すインターフェースであり
      // Keyプロパティが定義されている。
      //
      // このインターフェース自身も、IEnumerableインターフェースを継承しているので
      // グルーピングを行った場合は、以下のようにして2重のループでデータを取得する。
      //
      // var query = xxx.GroupBy(item => item.Key);
      // foreach (var group in query)
      // {
      //   Console.WriteLine(group.Key);
      //   foreach (var item in group)
      //   {
      //     Console.WriteLine(item);
      //   }
      // }
      //

      //
      // keySelectorのみを指定.
      //
      // 以下のクエリ式と同じとなる。
      //   from  thePerson in persons
      //   group thePerson by thePerson.Team
      //
      Console.WriteLine("============ keySelectorのみのGroupBy ==============");
      var query1 = persons.GroupBy(thePerson => thePerson.Team);
      foreach (var group in query1)
      {
        Console.WriteLine("=== {0}", group.Key);
        foreach (var thePerson in group)
        {
          Console.WriteLine("\t{0}", thePerson);
        }
      }

      //
      // keySelectorとelementSelectorを指定.
      //
      // 以下のクエリ式と同じとなる。
      //   from   thePerson in persons
      //   group  thePerson.Name by thePerson.Team
      //
      Console.WriteLine("\n============ elementSelectorを指定したGroupBy ==============");
      var query2 = persons.GroupBy(thePerson => thePerson.Team, thePerson => thePerson.Name);
      foreach (var group in query2)
      {
        Console.WriteLine("=== {0}", group.Key);
        foreach (var name in group)
        {
          Console.WriteLine("\t{0}", name);
        }
      }

      //
      // 複合キーにてグループ化.
      //
      // 以下のクエリ式と同じとなる。
      //   from  thePerson in persons
      //   group thePerson by new { thePerson.Project, thePerson.Team }
      //
      Console.WriteLine("\n============ 複合キーを指定したGroupBy ==============");
      var query3 = persons.GroupBy(thePerson => new { thePerson.Project, thePerson.Team });
      foreach (var group in query3)
      {
        Console.WriteLine("=== {0}", group.Key);
        foreach (var thePerson in group)
        {
          Console.WriteLine("\t{0}", thePerson);
        }
      }

      //
      // 以下のクエリ式と同じとなる。
      //   from  thePerson in persons
      //   group   thePerson by new { thePerson.Project, thePerson.Team } into p
      //   orderby p.Key.Project descending, p.Key.Team descending
      //   select  p;
      //
      Console.WriteLine("\n============ 複合キーとorderbyを指定したGroupBy ==============");
      var query4 = persons
              .GroupBy(thePerson => new { thePerson.Project, thePerson.Team })
              .OrderByDescending(group => group.Key.Project)
              .ThenByDescending(group => group.Key.Team);

      foreach (var group in query4)
      {
        Console.WriteLine("=== {0}", group.Key);
        foreach (var thePerson in group)
        {
          Console.WriteLine("\t{0}", thePerson);
        }
      }
    }
  }
  #endregion
}

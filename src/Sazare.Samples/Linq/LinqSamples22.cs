namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region LinqSamples-22
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  [Sample]
  public class LinqSamples22 : Sazare.Common.IExecutable
  {
    class Person
    {
      public int Id { get; set; }
      public string Name { get; set; }

      public override string ToString()
      {
        return string.Format("[ID={0}, NAME={1}]", Id, Name);
      }
    }

    public void Execute()
    {
      var persons = new List<Person>
                {
                  new Person{ Id = 1001, Name = "gsf_zero1" },
                  new Person{ Id = 1000, Name = "gsf_zero2" },
                  new Person{ Id = 111,  Name = "gsf_zero3" },
                  new Person{ Id = 9889, Name = "gsf_zero4" },
                  new Person{ Id = 9889, Name = "gsf_zero5" },
                  new Person{ Id = 100,  Name = "gsf_zero6" }
                };

      //
      // 順序付け演算子には、以下のものが存在する。
      //
      //   ・OrderBy
      //   ・OrderByDescending
      //   ・ThenBy
      //   ・ThenByDescending
      //
      // OrderByは昇順ソート、OrderByDescendingは降順ソートを行う。どちらも単一キーにてソートを行う。
      // 複合キーにて、ソート処理を行う場合は、OrderBy及びOrderByDescendingに続いて、ThenBy及びThenByDescendingを利用する。
      //
      // OrderBy及びOrderByDescendingメソッドは、他のLINQ標準演算子と戻り値が異なっており
      //   IOrderedEnumerable<T>
      // を返す。また、ThenBy及びThenByDescendingメソッドは、引数にIOrderedEnumerable<T>を渡す必要がある。
      // なので、必然的に、ThenByはOrderByの後で呼び出さないと利用出来ない。
      //
      // LINQの並び替え処理は、安定ソート(Stable Sort)である。
      // つまり、同じキーの要素がシーケンス内に複数存在した場合、並び替えた結果は元の順番を保持している。
      //

      //
      // IDで昇順ソート.
      //
      var sortByIdAsc = persons.OrderBy(aPerson => aPerson.Id);

      Console.WriteLine("================= IDで昇順ソート =================");
      Console.WriteLine(string.Join(Environment.NewLine, sortByIdAsc));

      //
      // IDで降順ソート.
      //
      var sortByIdDesc = persons.OrderByDescending(aPerson => aPerson.Id);

      Console.WriteLine("================= IDで降順ソート =================");
      Console.WriteLine(string.Join(Environment.NewLine, sortByIdDesc));

      //
      // 安定ソートの確認。
      //
      var sortByIdAscAndDesc = persons.OrderByDescending(aPerson => aPerson.Id).OrderBy(aPerson => aPerson.Id);

      Console.WriteLine("================= 安定ソートの確認 =================");
      Console.WriteLine(string.Join(Environment.NewLine, sortByIdAscAndDesc));
    }
  }
  #endregion
}

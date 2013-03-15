namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region IComparableSamples-01
  /// <summary>
  /// IComparableについてのサンプルです。
  /// </summary>
  public class IComparableSamples01 : IExecutable
  {
    enum CompareResult : int
    {
      SMALL = -1,
      EQUAL = 0,
      BIG = 1
    }

    class Person : IComparable<Person>
    {
      public int Id { get; set; }
      public string Name { get; set; }

      public int CompareTo(Person other)
      {
        if (other == null)
        {
          return (int)CompareResult.SMALL;
        }

        int result = (int)CompareResult.SMALL;
        int otherId = other.Id;

        if (Id == otherId)
        {
          result = (int)CompareResult.EQUAL;
        }
        else if (Id > otherId)
        {
          result = (int)CompareResult.BIG;
        }

        return result;
      }

      public override string ToString()
      {
        return string.Format("[ID={0}, NAME={1}]", Id, Name);
      }
    }

    public void Execute()
    {
      //
      // IComparableインターフェースは、インスタンスの並び替えをサポートするためのインターフェースである。
      //
      // このインターフェースを実装することで、クラスに対して順序の比較機能を付与することが出来る。
      // （List.Sortなどのソート処理時に、独自のソートルールを適用することができるようになる。）
      //  (ただし、DictionaryやHashTableなどには適用できない。これらの場合は、IEqualityComparableを実装する。）
      //
      var person1 = new Person { Id = 1, Name = "gsf_zero1" };
      var person2 = new Person { Id = 2, Name = "gsf_zero2" };
      var person3 = new Person { Id = 3, Name = "gsf_zero3" };
      var person4 = new Person { Id = 4, Name = "gsf_zero1" };

      var persons = new List<Person> { person1, person2, person3, person4 };
      var random = new Random();

      //
      // オブジェクト同士の比較.
      //
      for (int i = 0; i < 15; i++)
      {
        int index1 = random.Next(persons.Count);
        int index2 = random.Next(persons.Count);

        var p1 = persons[index1];
        var p2 = persons[index2];

        Console.WriteLine("person{0} CompareTo person{1} = {2}", index1, index2, (CompareResult)p1.CompareTo(p2));
      }

      //
      // リストのソート.
      //
      var persons2 = new List<Person> { person3, person2, person4, person1 };

      // ソートせず、そのまま出力.
      Console.WriteLine("\n============== ソートせずそのまま出力. ================");
      persons2.ForEach(Console.WriteLine);

      // ソートを行ってから、出力.
      Console.WriteLine("\n============== ソートしてから出力. ================");
      persons2.Sort();
      persons2.ForEach(Console.WriteLine);
    }
  }
  #endregion
}

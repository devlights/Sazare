namespace Gsf.Samples
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  #region LinqSamples-17
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples17 : IExecutable
  {
    class Person
    {
      public int Id { get; set; }
      public string Name { get; set; }
    }

    class Customer : Person
    {
      public IEnumerable<Order> Orders { get; set; }
    }

    class Order
    {
      public int Id { get; set; }
      public int Quantity { get; set; }
    }

    public void Execute()
    {
      List<Person> persons = new List<Person>
        {
           new Person { Id = 1, Name = "gsf_zero1" }
          ,new Person { Id = 2, Name = "gsf_zero2" }
          ,new Customer { Id = 3, Name = "gsf_zero3", Orders = Enumerable.Empty<Order>() }
          ,new Customer 
             { 
               Id = 4
              ,Name = "gsf_zero4"
              ,Orders =  new List<Order>
                 {
                    new Order { Id = 1, Quantity = 10 }
                   ,new Order { Id = 2, Quantity = 2  }
                 }
             }
          ,new Person { Id = 5, Name = "gsf_zero5" }
        };

      //
      // Castメソッドを利用することにより、特定の型のみのシーケンスに変換することができる。
      // OfTypeメソッドと違い、Castメソッドは単純にキャスト処理を行う為、キャスト出来ない型が
      // 含まれている場合は例外が発生する。
      // (OfTypeメソッドの場合、除外される。）
      //
      //
      // 尚、Castメソッドは他の変換演算子とは違い、ソースシーケンスのスナップショットを作成しない。
      // つまり、通常のクエリと同じく、Castで取得したシーケンスが列挙される度に評価される。
      // 変換演算子の中で、このような動作を行うのはAsEnumerableとOfTypeとCastである。
      //
      Console.WriteLine("========== Cast<Person>の結果 ==========");
      foreach (var data in persons.Cast<Person>())
      {
        Console.WriteLine(data);
      }

      //////////////////////////////////////////////////////////
      //
      // 以下のpersons.Cast<Customer>()はPersonオブジェクトをCustomerオブジェクトに
      // キャスト出来ない為、例外が発生する。
      //
      Console.WriteLine("========== Cast<Customer>の結果 ==========");
      try
      {
        foreach (var data in persons.Cast<Customer>())
        {
          Console.WriteLine(data);
        }
      }
      catch (InvalidCastException ex)
      {
        Console.WriteLine(ex.Message);
      }

      /*
        IEnumerable<Person> p = persons.Cast<Person>();
        persons.Add(new Person());
        
        Console.WriteLine("aaa");
        foreach (var a in p)
        {
          Console.WriteLine(a);
        }
      */

      //
      // 元々GenericではないリストをIEnumerable<T>に変換する場合にも利用出来る.
      // 当然、Castメソッドを利用する場合は、コレクション内部のデータが全てキャスト可能で
      // ないといけない。
      //
      ArrayList arrayList = new ArrayList();
      arrayList.Add(10);
      arrayList.Add(20);
      arrayList.Add(30);
      arrayList.Add(40);

      Console.WriteLine("========== Genericではないコレクションを変換 ==========");
      IEnumerable<int> intList = arrayList.Cast<int>();
      foreach (var data in intList)
      {
        Console.WriteLine(data);
      }
    }
  }
  #endregion
}

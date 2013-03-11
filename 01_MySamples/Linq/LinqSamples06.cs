namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region LinqSamples-06
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples06 : IExecutable
  {
    enum Country
    {
      Japan,
      America,
      China
    }

    class Person
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public AddressInfo Address
      {
        get;
        set;
      }
      public Country Country
      {
        get;
        set;
      }
    }

    class AddressInfo
    {
      public string PostCode
      {
        get;
        set;
      }
      public string Prefecture
      {
        get;
        set;
      }
      public string Municipality
      {
        get;
        set;
      }
      public string HouseNumber
      {
        get;
        set;
      }
      public string[] Tel
      {
        get;
        set;
      }
      public string[] Frends
      {
        get;
        set;
      }
    }


    IEnumerable<Person> CreateSampleData()
    {
      return new Person[]{
           new Person{ 
               Id="00001"
              ,Name="gsf_zero1"
              ,Address=new AddressInfo{
                       PostCode="999-8888"
                      ,Prefecture="東京都"
                      ,Municipality="どこか１"
                      ,HouseNumber="番地１"
                      ,Tel=new []{"090-xxxx-xxxx"}
                      ,Frends=new string[]{}
              }
              ,Country=Country.Japan
           }
          ,new Person{ 
               Id="00002"
              ,Name="gsf_zero2"
              ,Address=new AddressInfo{
                       PostCode="888-7777"
                      ,Prefecture="京都府"
                      ,Municipality="どこか２"
                      ,HouseNumber="番地２"
                      ,Tel=new []{"080-xxxx-xxxx"}
                      ,Frends=new []{"00001"}
              }
              ,Country=Country.Japan
          }
          ,new Person{ 
               Id="00003"
              ,Name="gsf_zero3"
              ,Address=new AddressInfo{
                       PostCode="777-6666"
                      ,Prefecture="北海道"
                      ,Municipality="どこか３"
                      ,HouseNumber="番地３"
                      ,Tel=new []{"070-xxxx-xxxx"}
                      ,Frends=new []{"00001", "00002"}
              }
              ,Country=Country.America
          }
          ,new Person{ 
               Id="00004"
              ,Name="gsf_zero4"
              ,Address=new AddressInfo{
                       PostCode="777-6666"
                      ,Prefecture="北海道"
                      ,Municipality="どこか４"
                      ,HouseNumber="番地４"
                      ,Tel=new []{"060-xxxx-xxxx", "111-111-1111", "222-222-2222"}
                      ,Frends=new []{"00001", "00003"}
              }
              ,Country=Country.America
          }
        };
    }

    public void Execute()
    {
      IEnumerable<Person> persons = CreateSampleData();

      // 
      //  昇順.
      //  (昇順の場合のascendingは付けても付けなくても良い)
      //
      var query1 = from person in persons
                   orderby person.Id.ToInt() ascending
                   select person;

      Console.WriteLine("============================================");
      foreach (Person person in query1)
      {
        Console.WriteLine("Id={0}, Name={1}", person.Id, person.Name);
      }

      //
      // 降順.
      //
      var query2 = from person in persons
                   orderby person.Id.ToInt() descending
                   select person;

      Console.WriteLine("============================================");
      foreach (Person person in query2)
      {
        Console.WriteLine("Id={0}, Name={1}", person.Id, person.Name);
      }

      //
      // 複数の条件でソート.
      //
      var query3 = from person in persons
                   orderby person.Address.PostCode, person.Id.ToInt()
                   select person;

      Console.WriteLine("============================================");
      foreach (Person person in query3)
      {
        Console.WriteLine("Id={0}, Name={1}", person.Id, person.Name);
      }

      //
      // 複数のorderby.
      // (query3の場合と結果が異なる事に注意)
      //
      // query3の場合は一度で2つの条件にてソート処理が行われるが
      // query4は、2回に分けてソート処理が行われる。
      //
      // つまり、orderby person.Address.PostCodeで一旦ソートが
      // 行われるが、その後orderby person.IdによってID順にソート
      // され直されてしまう。
      //
      var query4 = from person in persons
                   orderby person.Address.PostCode
                   orderby person.Id.ToInt()
                   select person;

      Console.WriteLine("============================================");
      foreach (Person person in query4)
      {
        Console.WriteLine("Id={0}, Name={1}", person.Id, person.Name);
      }
    }
  }
  #endregion
}

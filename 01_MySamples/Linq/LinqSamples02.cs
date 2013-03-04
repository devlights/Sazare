namespace Gsf.Samples
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  #region LinqSamples-02
  /// <summary>
  /// Linq‚ÌƒTƒ“ƒvƒ‹‚Å‚·B
  /// </summary>
  public class LinqSamples02 : IExecutable
  {

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
                      ,Prefecture="“Œ‹“s"
                      ,Municipality="‚Ç‚±‚©‚P"
                      ,HouseNumber="”Ô’n‚P"
                      ,Tel=new []{"090-xxxx-xxxx"}
                      ,Frends=new string[]{}
              }
           }
          ,new Person{ 
               Id="00002"
              ,Name="gsf_zero2"
              ,Address=new AddressInfo{
                       PostCode="888-7777"
                      ,Prefecture="‹“s•{"
                      ,Municipality="‚Ç‚±‚©‚Q"
                      ,HouseNumber="”Ô’n‚Q"
                      ,Tel=new []{"080-xxxx-xxxx"}
                      ,Frends=new []{"00001"}
              }
          }
          ,new Person{ 
               Id="00003"
              ,Name="gsf_zero3"
              ,Address=new AddressInfo{
                       PostCode="777-6666"
                      ,Prefecture="–kŠC“¹"
                      ,Municipality="‚Ç‚±‚©‚R"
                      ,HouseNumber="”Ô’n‚R"
                      ,Tel=new []{"070-xxxx-xxxx"}
                      ,Frends=new []{"00001", "00002"}
              }
          }
          ,new Person{ 
               Id="00004"
              ,Name="gsf_zero4"
              ,Address=new AddressInfo{
                       PostCode="777-6666"
                      ,Prefecture="–kŠC“¹"
                      ,Municipality="‚Ç‚±‚©‚S"
                      ,HouseNumber="”Ô’n‚S"
                      ,Tel=new []{"060-xxxx-xxxx", "111-111-1111", "222-222-2222"}
                      ,Frends=new []{"00001", "00003"}
              }
          }
        };
    }

    public void Execute()
    {
      IEnumerable<Person> persons = CreateSampleData();

      //
      // ˆê‚Â‚Ìfrom
      //    ®AˆÈ‰º‚Ì‚æ‚¤‚É‰½‚à‚¹‚¸‚ÉŒ³‚Ì‚Ü‚Ü‚ÌŒ‹‰Ê‚ğ•Ô‚·ƒNƒGƒŠ‚ğ
      //    k‘ŞƒNƒGƒŠ‚ÆŒ¾‚¤‚ç‚µ‚¢‚Å‚·B
      //
      var query1 = from person in persons
                   select person;

      foreach (var person in query1)
      {
        Console.WriteLine("Id={0}, Name={1}", person.Id, person.Name);
      }

      //
      // •¡”‚Ìfrom.
      //
      var query2 = from person in persons
                   from tel in person.Address.Tel
                   select new
                   {
                     Id = person.Id,
                     Tel = tel
                   };

      foreach (var data in query2)
      {
        Console.WriteLine("Id={0}, PostCode={1}", data.Id, data.Tel);
      }

      // 
      // IEnumerable‚ğÀ‘•‚µ‚Ä‚¢‚éê‡‚Í–¾¦“I‚ÈŒ^w’è‚ª•K—v.
      //
      ArrayList aryList = new ArrayList(new string[] { "hoge", "moge", "fuga" });

      var query3 = from string element in aryList
                   select element;

      foreach (var element in query3)
      {
        Console.WriteLine(element);
      }
    }
  }
  #endregion
}

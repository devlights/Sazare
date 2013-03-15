namespace Gsf.Samples
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Linq;

  #region OrderedDictionarySample-01
  /// <summary>
  /// OrderedDictionaryのサンプル1です。
  /// </summary>
  public class OrderedDictionarySample01 : IExecutable
  {

    public void Execute()
    {

      Dictionary<string, string> dicA = new Dictionary<string, string>();
      for (int i = 0; i < 100; i++)
      {
        dicA.Add(i.ToString(), string.Format("HOGE-{0}", i.ToString()));
      }

      PrintDictionary(dicA);
      Console.WriteLine("");

      OrderedDictionary dicB = new OrderedDictionary();
      for (int i = 0; i < 100; i++)
      {
        dicB.Add(i.ToString(), string.Format("HOGE-{0}", i.ToString()));
      }

      PrintDictionary(dicB);
    }

    void PrintDictionary(Dictionary<string, string> dic)
    {
      foreach (KeyValuePair<string, string> pair in dic)
      {
        Console.WriteLine("{0}-{1}", pair.Key, pair.Value);
      }
    }

    void PrintDictionary(OrderedDictionary dic)
    {
      foreach (DictionaryEntry entry in dic)
      {
        Console.WriteLine("{0}-{1}", entry.Key, entry.Value);
      }
    }
  }
  #endregion
}

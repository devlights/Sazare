namespace Sazare.Samples
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Linq;

  using Sazare.Common;
  
  #region OrderedDictionarySample-01
  /// <summary>
  /// OrderedDictionaryのサンプル1です。
  /// </summary>
  [Sample]
  public class OrderedDictionarySample01 : Sazare.Common.IExecutable
  {

    public void Execute()
    {

      Dictionary<string, string> dicA = new Dictionary<string, string>();
      for (int i = 0; i < 100; i++)
      {
        dicA.Add(i.ToString(), string.Format("HOGE-{0}", i.ToString()));
      }

      PrintDictionary(dicA);
      Output.WriteLine("");

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
        Output.WriteLine("{0}-{1}", pair.Key, pair.Value);
      }
    }

    void PrintDictionary(OrderedDictionary dic)
    {
      foreach (DictionaryEntry entry in dic)
      {
        Output.WriteLine("{0}-{1}", entry.Key, entry.Value);
      }
    }
  }
  #endregion
}

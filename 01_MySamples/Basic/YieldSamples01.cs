namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region YieldSamples-01
  /// <summary>
  /// yieldキーワードに関するサンプルです。
  /// </summary>
  [Sample]
  public class YieldSamples01 : IExecutable
  {
    //
    // 最もベーシックな列挙可能クラス.
    //
    class SampleCollection1
    {
      List<string> _list;

      public SampleCollection1()
      {

        _list = new List<string>{
             "Value1"
            ,"Value2"
            ,"Value3"
          };
      }

      public IEnumerator<string> GetEnumerator()
      {
        return _list.GetEnumerator();
      }
    }

    //
    // Yieldを利用したパターン１ (IEnumerator<T>を使用)
    //
    class SampleCollection2
    {
      public IEnumerator<string> GetEnumerator()
      {
        yield return "Value1";
        yield return "Value2";
        yield return "Value3";
      }
    }

    //
    // Yieldを利用したパターン２ (IEnumerable<T>を使用)
    //
    class SampleCollection3
    {
      public IEnumerable<string> InOrder
      {
        get
        {
          yield return "Value1";
          yield return "Value2";
          yield return "Value3";
        }
      }
    }

    //
    // Yieldを利用したパターン３ (複数のIEnumerable<T>を使用)
    //
    class SampleCollection4
    {
      List<string> _list;

      public SampleCollection4()
      {

        _list = new List<string>{
             "Value1"
            ,"Value2"
            ,"Value3"
          };
      }

      public IEnumerable<string> InOrder
      {
        get
        {
          for (int i = 0; i < _list.Count; i++)
          {
            yield return _list[i];
          }
        }
      }

      public IEnumerable<string> ReverseOrder
      {
        get
        {
          for (int i = (_list.Count - 1); i >= 0; i--)
          {
            yield return _list[i];
          }
        }
      }
    }

    //
    // yield breakを利用したパターン
    //
    class SampleCollection5
    {
      public IEnumerable<string> InOrderWithBreak
      {
        get
        {
          yield return "Value1";
          yield return "Value2";
          yield return "Value3";

          yield break;

          // warning CS0162: 到達できないコードが検出されました。
          //yield return "Value4";
        }
      }
    }

    //
    // 簡易パイプライン用の各メソッド.
    //
    IEnumerable<string> Range(int count, string prefix)
    {
      for (int i = 0; i < count; i++)
      {
        yield return string.Format("{0}-{1}", prefix, i);
      }
    }

    IEnumerable<string> Upper(IEnumerable<string> enumerables)
    {
      foreach (string val in enumerables)
      {
        yield return val.ToUpper();
      }
    }

    IEnumerable<string> AddCount(IEnumerable<string> enumerables)
    {

      int count = 1;
      foreach (string val in enumerables)
      {
        yield return string.Format("{0}-{1}", val, count++);
      }
    }

    IEnumerable<string> Filter(Predicate<string> predicate, IEnumerable<string> enumerables)
    {

      foreach (string val in enumerables)
      {

        if (predicate(val))
        {
          yield return val;
        }
      }
    }

    //
    // 処理確認用実行メソッド
    //
    public void Execute()
    {
      SampleCollection1 col1 = new SampleCollection1();
      foreach (string val in col1)
      {
        Console.WriteLine(val);
      }

      AddNewLine();

      SampleCollection2 col2 = new SampleCollection2();
      foreach (string val in col2)
      {
        Console.WriteLine(val);
      }

      AddNewLine();

      SampleCollection3 col3 = new SampleCollection3();
      foreach (string val in col3.InOrder)
      {
        Console.WriteLine(val);
      }

      AddNewLine();

      SampleCollection4 col4 = new SampleCollection4();
      foreach (string val in col4.InOrder)
      {
        Console.WriteLine(val);
      }

      foreach (string val in col4.ReverseOrder)
      {
        Console.WriteLine(val);
      }

      AddNewLine();

      SampleCollection5 col5 = new SampleCollection5();
      foreach (string val in col5.InOrderWithBreak)
      {
        Console.WriteLine(val);
      }

      AddNewLine();

      foreach (string val in Filter(Judge, AddCount(Upper(Range(10, "value")))))
      {
        Console.WriteLine(val);
      }
    }

    void AddNewLine()
    {
      Console.WriteLine(string.Empty);
    }

    bool Judge(string val)
    {

      string[] parts = val.Split('-');
      if (parts.Length != 3)
      {
        return false;
      }

      int number = int.Parse(parts[2]);
      if ((number % 2) == 0)
      {
        return true;
      }

      return false;
    }
  }
  #endregion
}

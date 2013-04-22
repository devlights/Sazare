namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;

  using Sazare.Common;
  
  #region ListForEachDiffSamples-01
  /// <summary>
  /// Listをforeachでループする場合と、List.ForEachする場合の速度差をテスト
  /// </summary>
  [Sample]
  public class ListForEachDiffSamples01 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      Prepare();

      //
      // Listをforeachで処理するか、List.ForEachで処理するかで
      // どちらの方が速いのかを計測。
      //
      // ILレベルで見ると
      //   foreachの場合： callが2つ
      //   List.ForEachの場合： callvirtが1つ
      // となる。
      //
      foreach (var elementCount in new[] { 1000, 3000, 5000, 10000, 50000, 100000, 150000, 500000, 700000, 1000000 })
      {
        Output.WriteLine("===== [Count:{0}] =====", elementCount);

        var theList = new List<int>(Enumerable.Range(1, elementCount));

        var watch = Stopwatch.StartNew();
        Sum_foreach(theList);
        watch.Stop();
        Output.WriteLine("foreach:      {0}", watch.Elapsed);

        watch = Stopwatch.StartNew();
        Sum_List_ForEach(theList);
        watch.Stop();
        Output.WriteLine("List.ForEach: {0}", watch.Elapsed);
      }
    }

    void Prepare()
    {
      int result = 0;
      foreach (var x in new List<int>(Enumerable.Range(1, 1000)))
      {
        result += x;
      }

      result = 0;
      new List<int>(Enumerable.Range(1, 1000)).ForEach(x => result += x);
    }

    int Sum_foreach(List<int> theList)
    {
      int result = 0;
      foreach (var x in theList)
      {
        result += x;
      }
      return result;
    }

    int Sum_List_ForEach(List<int> theList)
    {
      int result = 0;
      theList.ForEach(x => result += x);
      return result;
    }
  }
  #endregion
}

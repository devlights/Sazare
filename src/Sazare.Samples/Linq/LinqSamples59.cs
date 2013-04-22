namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// Linqにて、シーケンスをチャンクに分割して処理するサンプルです.
  /// </summary>
  [Sample]
  public class LinqSamples59 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      //
      // 要素が10のシーケンスを2つずつのチャンクに分割.
      //
      foreach (var chunk in Enumerable.Range(1, 10).Chunk(2))
      {
        Console.WriteLine("Chunk:");
        foreach (var item in chunk)
        {
          Console.WriteLine("\t--> {0}", item);
        }
      }

      //
      // 要素が10000のシーケンスを1000ずつのチャンクに分割し
      // それぞれのチャンクごとにインデックスを付与.
      //
      foreach (var chunk in Enumerable.Range(1, 10000).Chunk(1000).Select((x, i) => new { Index = i, Count = x.Count() }))
      {
        Console.WriteLine(chunk);
      }
    }
  }
}

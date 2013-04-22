namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;

  #region PLinqSamples-01
  [Sample]
  public class PLinqSamples01 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      byte[] numbers = GetRandomNumbers();

      Stopwatch watch = Stopwatch.StartNew();

      // 普通のLINQ
      // var query1 = from x in numbers
      // 並列LINQ（１）（ExecutionModeを付与していないので、並列で実行するか否かはTPLが決定する）
      // var query1 = from x in numbers.AsParallel()
      // 並列LINQ（２）（ExecutionModeを付与しているので、強制的に並列で実行するよう指示）
      var query1 = from x in numbers.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                   select Math.Pow(x, 2);

      foreach (var item in query1)
      {
        Console.WriteLine(item);
      }

      watch.Stop();
      Console.WriteLine(watch.Elapsed);
    }

    byte[] GetRandomNumbers()
    {
      byte[] result = new byte[10];
      Random rnd = new Random();

      rnd.NextBytes(result);

      return result;
    }
  }
  #endregion
}

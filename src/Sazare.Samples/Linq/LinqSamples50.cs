namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Globalization;
  using System.IO;
  using System.Linq;

  #region LinqSamples-50
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  [Sample]
  public class LinqSamples50 : IExecutable
  {
    public void Execute()
    {
      //
      // File.ReadLinesメソッドは、従来までの
      // File.ReadAllLinesメソッドと同じ動作するメソッドである。
      //
      // 違いは、戻り値がIEnumerable<string>となっており
      // 遅延評価される。
      //
      // ReadAllLinesメソッドの場合は、全リストを構築してから
      // 戻り値が返却されるので、コレクションが構築されるまで
      // 待機する必要があるが、ReadLinesメソッドの場合は
      // コレクション全体が返される前に、列挙可能である。
      //
      Console.WriteLine("ファイル作成中・・・・");

      var tmpFilePath = CreateSampleFile(1000000);
      if (string.IsNullOrEmpty(tmpFilePath))
      {
        Console.WriteLine("ファイル作成中にエラー発生");
      }

      Console.WriteLine("ファイル作成完了");

      try
      {
        var watch = Stopwatch.StartNew();
        var elapsed = string.Empty;

        var numberFormatInfo = new NumberFormatInfo { CurrencySymbol = "gsf_zero" };

        //
        // File.ReadAllLines
        //
        var query = from line in File.ReadAllLines(tmpFilePath)
                    where int.Parse(line, NumberStyles.AllowCurrencySymbol, numberFormatInfo) % 2 == 0
                    select line;

        foreach (var element in query)
        {
          if (watch != null)
          {
            watch.Stop();
            elapsed = watch.Elapsed.ToString();
            watch = null;
          }

          //Console.WriteLine(element);
        }

        Console.WriteLine("================== ReadAllLines      : {0} ==================", elapsed);

        //
        // File.ReadLines
        //
        watch = Stopwatch.StartNew();
        elapsed = string.Empty;

        query = from line in File.ReadLines(tmpFilePath)
                where int.Parse(line, NumberStyles.AllowCurrencySymbol, numberFormatInfo) % 2 == 0
                select line;

        foreach (var element in query)
        {
          if (watch != null)
          {
            watch.Stop();
            elapsed = watch.Elapsed.ToString();
            watch = null;
          }

          //Console.WriteLine(element);
        }

        Console.WriteLine("================== ReadLines       : {0} ==================", elapsed);
      }
      finally
      {
        if (File.Exists(tmpFilePath))
        {
          File.Delete(tmpFilePath);
        }
      }
    }

    string CreateSampleFile(int lineCount)
    {
      var tmpFileName = Path.GetTempFileName();

      try
      {
        //
        // 巨大なファイルを作成する.
        //
        using (var writer = new StreamWriter(new BufferedStream(File.OpenWrite(tmpFileName))))
        {
          for (int i = 0; i < lineCount; i++)
          {
            writer.WriteLine(string.Format("gsf_zero{0}", i));
          }

          writer.Flush();
          writer.Close();
        }
      }
      catch
      {
        if (File.Exists(tmpFileName))
        {
          File.Delete(tmpFileName);
        }

        return string.Empty;
      }

      return tmpFileName;
    }
  }
  #endregion
}

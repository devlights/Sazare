namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;

  #region DateParseSamples-01
  public class DateParseSample01 : IExecutable
  {
    public void Execute()
    {
      //
      // ParseExactメソッドの場合は、値が2011, フォーマットがyyyy
      // の場合でも日付変換出来る。
      //
      try
      {
        var d = DateTime.ParseExact("2011", "yyyy", null);
        Console.WriteLine(d);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }

      //
      // TryParseメソッドの場合は、以下のどちらもFalseとなる。
      // 恐らく、IFormatProviderを設定しないと動かないと思われる。
      //
      DateTime d2;
      Console.WriteLine(DateTime.TryParse("2011", out d2));
      Console.WriteLine(DateTime.TryParse("2011", null, DateTimeStyles.None, out d2));

      //
      // TryParseExactメソッドの場合は、値が2011、フォーマットがyyyy
      // の場合でも日付変換出来る。
      //
      DateTime d3;
      Console.WriteLine(DateTime.TryParseExact("2011", "yyyy", null, DateTimeStyles.None, out d3));

      Console.WriteLine(DateTime.Now.ToString("yyyyMMddHHmmssfff"));

      var d98 = DateTime.Now;
      var d99 = DateTime.ParseExact(d98.ToString("yyyyMMddHHmmssfff"), "yyyyMMddHHmmssfff", null);
      Console.WriteLine(d98 == d99);
      Console.WriteLine(d98.Ticks);
      Console.WriteLine(d98 == new DateTime(d98.Ticks));

      // 時分秒を指定していない場合は、00:00:00となる
      var d100 = new DateTime(2011, 11, 12);
      Console.WriteLine("{0}, {1}, {2}", d100.Hour, d100.Minute, d100.Second);
    }
  }
  #endregion
}

namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Linq;

  #region DataTableSamples-02
  /// <summary>
  /// DataTableクラスに関するサンプルです。
  /// </summary>
  public class DataTableSamples02 : IExecutable
  {
    public void Execute()
    {
      DataTable table = new DataTable();

      table.Columns.Add("Val", typeof(int));

      for (int i = 0; i < 10; i++)
      {
        table.LoadDataRow(new object[] { i }, true);
      }

      //
      // Selectメソッドで行を抽出.
      //
      DataRow[] selectedRows = table.Select("(Val % 2) = 0", "Val");
      selectedRows.ToList().ForEach((row) =>
      {
        Console.WriteLine(row["Val"]);
      });
    }
  }
  #endregion
}

namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Linq;

  using Sazare.Common;
  
  #region DataTableSortSamples-01
  /// <summary>
  /// DataTableについてのサンプルです。
  /// </summary>
  [Sample]
  public class DataTableSortSamples01 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      DataTable table = new DataTable("SortSampleTable");

      table.Columns.Add("Col1", typeof(string));
      table.Columns.Add("Col2", typeof(string));

      table.LoadDataRow(new object[] { "1", "1" }, true);
      table.LoadDataRow(new object[] { "1", "3" }, true);
      table.LoadDataRow(new object[] { "1", "4" }, true);
      table.LoadDataRow(new object[] { "1", "2" }, true);
      table.LoadDataRow(new object[] { "2", "1" }, true);
      table.LoadDataRow(new object[] { "2", "3" }, true);
      table.LoadDataRow(new object[] { "2", "5" }, true);
      table.LoadDataRow(new object[] { "2", "4" }, true);
      table.LoadDataRow(new object[] { "2", "2" }, true);

      Output.WriteLine("===================================================");
      foreach (DataRow row in table.Rows)
      {
        DumpRow(row);
      }
      Output.WriteLine("===================================================");

      Output.WriteLine("===================================================");
      table.DefaultView.Sort = "Col1 DESC";
      foreach (DataRowView row in table.DefaultView)
      {
        DumpRow(row);
      }
      Output.WriteLine("===================================================");

      Output.WriteLine("===================================================");
      table.DefaultView.Sort = "Col1 ASC";
      foreach (DataRowView row in table.DefaultView)
      {
        DumpRow(row);
      }
      Output.WriteLine("===================================================");
    }

    void DumpRow(DataRow row)
    {
      Output.WriteLine("{0}, {1}", row[0], row[1]);
    }

    void DumpRow(DataRowView row)
    {
      Output.WriteLine("{0}, {1}", row[0], row[1]);
    }
  }
  #endregion
}

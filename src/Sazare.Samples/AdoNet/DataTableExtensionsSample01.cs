namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Linq;

  #region DataTableExtensionsSample-01
  /// <summary>
  /// System.Data.Extensionsのサンプル1です。
  /// </summary>
  [Sample]
  public class DataTableExtensionsSample01 : IExecutable
  {
    public void Execute()
    {
      DataTable table = BuildSampleTable();
      PrintTable("Before:", table);

      IEnumerable<DataRow> query = from row in table.AsEnumerable()
                                   where (row.Field<int>("COL-1") % 2) != 0
                                   select row;

      DataTable newTable = query.CopyToDataTable();
      PrintTable("After:", newTable);
    }

    DataTable BuildSampleTable()
    {
      DataTable table = new DataTable();

      table.BeginInit();
      table.Columns.Add("COL-1", typeof(int));
      table.Columns.Add("COL-2");
      table.EndInit();

      table.BeginLoadData();
      for (int i = 0; i < 5; i++)
      {
        table.LoadDataRow(new object[] { i, (i + 1).ToString() }, true);
      }
      table.EndLoadData();

      return table;
    }

    void PrintTable(string title, DataTable table)
    {
      Console.WriteLine(title);

      foreach (DataRow row in table.Rows)
      {
        Console.WriteLine("\t{0}, {1}", row[0], row[1]);
      }

      Console.WriteLine();
    }
  }
  #endregion
}

namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Linq;

  using Sazare.Common;
  
  #region DataTableExtensionsSample-02
  /// <summary>
  /// System.Data.Extensionsのサンプル2です。
  /// </summary>
  [Sample]
  public class DataTableExtensionsSample02 : Sazare.Common.IExecutable
  {

    public void Execute()
    {
      DataTable table = BuildSampleTable();

      //
      // 1列目の情報をint型で取得.
      // 2列目の情報をstring型で取得.
      //
      int val1 = table.Rows[0].Field<int>("COL-1");
      string val2 = table.Rows[0].Field<string>("COL-2");
      PrintTable("Before:", table);

      //
      // 1列目の情報を変更.
      //
      table.Rows[0].SetField<int>("COL-1", 100);
      PrintTable("After:", table);

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
      Output.WriteLine(title);

      foreach (DataRow row in table.Rows)
      {
        Output.WriteLine("\t{0}, {1}", row[0], row[1]);
      }

      Output.WriteLine(string.Empty);
    }
  }
  #endregion
}

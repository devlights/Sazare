namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Linq;

  using Sazare.Common;
  
  #region DataTableSamples-03
  /// <summary>
  /// DataTableクラスに関するサンプルです。
  /// </summary>
  [Sample]
  public class DataTableSamples03 : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      DataTable tableA = new DataTable();

      tableA.Columns.Add("Val", typeof(int));

      for (int i = 0; i < 10; i++)
      {
        tableA.LoadDataRow(new object[] { i }, true);
      }

      Output.WriteLine("[TableA]ColumnCount = {0}", tableA.Columns.Count);
      Output.WriteLine("[TableA]RowCount = {0}", tableA.Rows.Count);

      //
      // tableAのスキーマをtableBにコピー.
      // (データはコピーしない。)
      //
      DataTable tableB = tableA.Clone();
      Output.WriteLine("[TableB]ColumnCount = {0}", tableB.Columns.Count);
      Output.WriteLine("[TableB]RowCount = {0}", tableB.Rows.Count);

      //
      // tableAのスキーマとデータをtableCにコピー.
      //
      DataTable tableC = tableA.Copy();
      Output.WriteLine("[TableC]ColumnCount = {0}", tableC.Columns.Count);
      Output.WriteLine("[TableC]RowCount = {0}", tableC.Rows.Count);
    }
  }
  #endregion
}

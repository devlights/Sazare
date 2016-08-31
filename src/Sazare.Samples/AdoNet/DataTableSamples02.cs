using System.Data;
using System.Linq;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{
    #region DataTableSamples-02

    /// <summary>
    ///     DataTableクラスに関するサンプルです。
    /// </summary>
    [Sample]
    public class DataTableSamples02 : IExecutable
    {
        public void Execute()
        {
            var table = new DataTable();

            table.Columns.Add("Val", typeof(int));

            for (var i = 0; i < 10; i++)
            {
                table.LoadDataRow(new object[] {i}, true);
            }

            //
            // Selectメソッドで行を抽出.
            //
            var selectedRows = table.Select("(Val % 2) = 0", "Val");
            selectedRows.ToList().ForEach(row => { Output.WriteLine(row["Val"]); });
        }
    }

    #endregion
}
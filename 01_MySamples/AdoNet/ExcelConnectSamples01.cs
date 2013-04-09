namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Data.Common;
  using System.Linq;

  #region ExcelConnectSamples-01
  /// <summary>
  /// ADO.NETを用いてExcelに接続するサンプルです。
  /// </summary>
  [Sample]
  public class ExcelConnectSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // Excelに接続するには、OLEDBを利用する。
      // プロバイダー名は、「System.Data.OleDb」となる。
      //
      DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
      using (DbConnection conn = factory.CreateConnection())
      {
        //
        // Excel用の接続文字列を構築.
        //
        // Providerは、Microsoft.ACE.OLEDB.12.0を使用する事。
        // （JETドライバを利用するとxlsxを読み込む事が出来ない。）
        //
        // Extended Propertiesには、ISAMのバージョン(Excel 12.0)とHDRを指定する。
        // （2003までのxlsの場合はExcel 8.0でISAMバージョンを指定する。）
        // HDRは先頭行をヘッダ情報としてみなすか否かを指定する。
        // 先頭行をヘッダ情報としてみなす場合はYESを、そうでない場合はNOを設定。
        //
        // HDR=NOと指定した場合、カラム名はシステム側で自動的に割り振られる。
        // (F1, F2, F3.....となる)
        //
        DbConnectionStringBuilder builder = factory.CreateConnectionStringBuilder();

        builder["Provider"] = "Microsoft.ACE.OLEDB.12.0";
        builder["Data Source"] = @"C:\Users\gsf\Tmp\Sample.xlsx";
        builder["Extended Properties"] = "Excel 12.0;HDR=YES";

        conn.ConnectionString = builder.ToString();
        conn.Open();

        //
        // SELECT.
        //
        // 通常のSQLのように発行できる。その際シート指定は
        // [Sheet1$]のように行う。範囲を指定することも出来る。
        //
        using (DbCommand command = conn.CreateCommand())
        {
          command.CommandText = "SELECT * FROM [Sheet1$]";

          DataTable table = new DataTable();
          using (DbDataReader reader = command.ExecuteReader())
          {
            table.Load(reader);
          }

          foreach (DataRow row in table.Rows)
          {
            Console.WriteLine("{0},{1},{2}", row["ID"], row["NAME"], row["AGE"]);
          }
        }

        using (DbCommand command = conn.CreateCommand())
        {
          command.CommandText = "SELECT * FROM [Sheet1$A1:C7]";

          DataTable table = new DataTable();
          using (DbDataReader reader = command.ExecuteReader())
          {
            table.Load(reader);
          }

          foreach (DataRow row in table.Rows)
          {
            Console.WriteLine("{0},{1},{2}", row["ID"], row["NAME"], row["AGE"]);
          }
        }

        //
        // INSERT
        //
        // こちらも普通のSQLと同じように発行できる。
        // 尚、トランザクションは設定できるが効果は無い。
        // （ロールバックを行ってもデータは戻らない。）
        //
        // また、INSERT,UPDATEはエクセルを開いた状態でも
        // 行う事ができる。
        //
        // データの削除は行う事ができない。（制限）
        //
        using (DbCommand command = conn.CreateCommand())
        {
          string query = string.Empty;

          query += " INSERT INTO [Sheet1$] ";
          query += "   (ID, NAME, AGE) ";
          query += "   VALUES ";
          query += "   (7, 'gsf_zero7', 37) ";

          command.CommandText = query;
          command.ExecuteNonQuery();

          using (DbCommand command2 = conn.CreateCommand())
          {
            command2.CommandText = "SELECT * FROM [Sheet1$]";

            DataTable table = new DataTable();
            using (DbDataReader reader = command2.ExecuteReader())
            {
              table.Load(reader);
            }

            foreach (DataRow row in table.Rows)
            {
              Console.WriteLine("{0},{1},{2}", row["ID"], row["NAME"], row["AGE"]);
            }
          }
        }

        //
        // UPDATE
        //
        using (DbCommand command = conn.CreateCommand())
        {
          string query = string.Empty;

          query += " UPDATE [Sheet1$] ";
          query += " SET ";
          query += "    NAME = 'updated' ";
          query += "   ,AGE  = 99 ";
          query += " WHERE ";
          query += "   ID = 7 ";

          command.CommandText = query;
          command.ExecuteNonQuery();

          using (DbCommand command2 = conn.CreateCommand())
          {
            command2.CommandText = "SELECT * FROM [Sheet1$]";

            DataTable table = new DataTable();
            using (DbDataReader reader = command2.ExecuteReader())
            {
              table.Load(reader);
            }

            foreach (DataRow row in table.Rows)
            {
              Console.WriteLine("{0},{1},{2}", row["ID"], row["NAME"], row["AGE"]);
            }
          }
        }

        //
        // DELETE.
        // 削除は行えない。
        //
      }
    }
  }
  #endregion
}

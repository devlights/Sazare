namespace Sazare.Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// 出力を管理する静的クラスです。
  /// </summary>
  public static class Output
  {
    /// <summary>
    /// 出力管理オブジェクト
    /// </summary>
    private static IOutputManager _Manager;

    /// <summary>
    /// 出力管理オブジェクトを設定します。
    /// </summary>
    /// <param name="manager">出力管理オブジェクト</param>
    public static void SetOutputManager(IOutputManager manager)
    {
      _Manager = manager;
    }

    /// <summary>
    /// 指定されたデータを出力します。（改行付与無し）
    /// </summary>
    /// <param name="data">データ</param>
    public static void Write(object data)
    {
      Defence();
      _Manager.Write(data);
    }

    /// <summary>
    /// 指定されたデータを出力します。（改行付与有り）
    /// </summary>
    /// <param name="data">データ</param>
    public static void WriteLine(object data)
    {
      Defence();
      _Manager.WriteLine(data);
    }

    /// <summary>
    /// 現在のオブジェクトの状態をチェックします。
    /// </summary>
    private static void Defence()
    {
      if (_Manager == null)
      {
        throw new InvalidOperationException("No OutputManager was found. please call SetOutputManager(IOutputManager) before your first access");
      }
    }
  }
}

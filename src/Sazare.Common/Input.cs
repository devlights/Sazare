namespace Sazare.Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// 入力を管理する静的クラスです。
  /// </summary>
  public static class Input
  {
    /// <summary>
    /// 入力管理オブジェクト
    /// </summary>
    static IInputManager _Manager;

    /// <summary>
    /// 入力管理オブジェクトを設定します。
    /// </summary>
    /// <param name="manager">入力管理オブジェクト</param>
    public static void SetInputManager(IInputManager manager)
    {
      _Manager = manager;
    }

    /// <summary>
    /// １データを読み込みます。
    /// </summary>
    /// <returns>読み込まれたデータ</returns>
    public static object Read()
    {
      Defence();
      return _Manager.Read();
    }

    /// <summary>
    /// 一行分のデータを読み込みます。
    /// </summary>
    /// <returns>一行分のデータ</returns>
    public static object ReadLine()
    {
      Defence();
      return _Manager.ReadLine();
    }

    /// <summary>
    /// 現在のオブジェクトの状態をチェックします。
    /// </summary>
    static void Defence()
    {
      if (_Manager == null)
      {
        throw new InvalidOperationException("No InputManager was found. please call SetInputManager(IInputManager) before your first access");
      }
    }
  }
}

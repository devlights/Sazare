namespace Sazare.Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region 共通インターフェース定義
  /// <summary>
  /// 各サンプルクラスが共通して実装しているインターフェースです。
  /// </summary>
  public interface IExecutable
  {
    /// <summary>
    /// 処理を実行します。
    /// </summary>
    void Execute();
  }
  #endregion
}

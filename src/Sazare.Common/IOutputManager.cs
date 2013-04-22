namespace Sazare.Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// 出力のインターフェースが定義されています。
  /// </summary>
  public interface IOutputManager
  {
    /// <summary>
    /// 指定されたデータを出力します。(改行付与無し）
    /// </summary>
    /// <param name="data">データ</param>
    void Write(object data);

    /// <summary>
    /// 指定されたデータを出力します。（改行付与有り）
    /// </summary>
    /// <param name="data">データ</param>
    void WriteLine(object data);
  }
}

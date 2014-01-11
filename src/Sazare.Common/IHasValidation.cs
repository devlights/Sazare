namespace Sazare.Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// 検証機能を持っている事を明示するインターフェースです。
  /// </summary>
  /// <typeparam name="T">検証を実施するオブジェクトの型</typeparam>
  public interface IHasValidation<T>
  {
    /// <summary>
    /// 検証を実施します。
    /// </summary>
    /// <param name="value">対象データ</param>
    /// <returns>検証結果がOKの場合True, それ以外はFalse</returns>
    bool Validate(T value);
  }
}

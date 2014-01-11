namespace Sazare.Tools.Cui
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sazare.Common;

  /// <summary>
  /// 空文字の検証を行うクラスです。
  /// </summary>
  internal class EmptyInputValidator : IHasValidation<string>
  {
    /// <summary>
    /// 入力値が空文字か否かを検証します。
    /// </summary>
    /// <param name="value">対象データ</param>
    /// <returns>空文字の場合はTrue, それ以外はFalse.</returns>
    public bool Validate(string value)
    {
      return string.IsNullOrWhiteSpace(value);
    }
  }
}

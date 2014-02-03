namespace Sazare.Tools.Cui
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sazare.Common;

  /// <summary>
  /// 終了文字の検証を行うクラスです。
  /// </summary>
  internal class ExitPhaseValidator : IHasValidation<string>
  {
    /// <summary>
    /// 終了文字
    /// </summary>
    public const string EXIT_PHASE = "EXIT";
    /// <summary>
    /// 終了文字
    /// </summary>
    public const string QUIT_PHASE = "QUIT";

    /// <summary>
    /// 終了文字か否かを検証します。
    /// </summary>
    /// <param name="value">対象データ</param>
    /// <returns>終了文字の場合True, それ以外はFalse.</returns>
    public bool Validate(string value)
    {
      return (value.ToUpper() == EXIT_PHASE || value.ToUpper() == QUIT_PHASE);
    }
  }
}

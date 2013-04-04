namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region ダミークラス
  /// <summary>
  /// ダミークラス
  /// </summary>
  [Sample]
  class Dummy : IExecutable
  {
    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      Console.WriteLine("THIS IS DUMMY CLASS.");
    }
  }
  #endregion
}

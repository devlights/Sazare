namespace Sazare.Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  /// <summary>
  /// 非同期実行できることを示すインターフェースです。
  /// </summary>
  public interface IAsyncExecutable : IExecutable
  {
    /// <summary>
    /// 処理を実行します。
    /// </summary>
    /// <returns>タスク</returns>
    new Task Execute();
  }
}

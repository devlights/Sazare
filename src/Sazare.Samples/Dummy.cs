using Sazare.Common;

namespace Sazare.Samples
{

    #region ダミークラス

    /// <summary>
    ///     ダミークラス
    /// </summary>
    [Sample]
    public class Dummy : IExecutable
    {
        /// <summary>
        ///     処理を実行します。
        /// </summary>
        public void Execute()
        {
            Output.WriteLine("THIS IS THE DUMMY CLASS.");
        }
    }

    #endregion
}
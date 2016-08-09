using System.Windows.Forms;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{

    #region デリゲートのサンプル (.net framework 1.1)

    /// <summary>
    ///     デリゲートのサンプル（.NET Framework 1.1）
    /// </summary>
    [Sample]
    internal class DelegateSample : IExecutable
    {
        /// <summary>
        ///     処理を実行します。
        /// </summary>
        public void Execute()
        {
            MethodInvoker methodInvoker = DelegateMethod;
            methodInvoker();
        }

        /// <summary>
        ///     デリゲートメソッド.
        /// </summary>
        private void DelegateMethod()
        {
            Output.WriteLine("SAMPLE_DELEGATE_METHOD.");
        }
    }

    #endregion
}
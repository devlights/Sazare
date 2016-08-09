using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{

    #region NumberFormatSamples-02

    /// <summary>
    ///     数値フォーマットのサンプルです。
    /// </summary>
    [Sample]
    public class NumberFormatSamples02 : IExecutable
    {
        public void Execute()
        {
            var i = 123456;
            Output.WriteLine("{0:N0}", i);
        }
    }

    #endregion
}
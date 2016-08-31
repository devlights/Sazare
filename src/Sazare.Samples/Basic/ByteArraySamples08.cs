using System;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{
    #region ByteArraySamples-08

    /// <summary>
    ///     バイト配列についてのサンプルです。
    /// </summary>
    [Sample]
    public class ByteArraySamples08 : IExecutable
    {
        public void Execute()
        {
            //
            // 数値をいろいろな基数に変換.
            //
            var i = 123;

            Output.WriteLine(Convert.ToString(i, 16));
            Output.WriteLine(Convert.ToString(i, 8));
            Output.WriteLine(Convert.ToString(i, 2));
        }
    }

    #endregion
}
using System;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{
    #region ByteArraySamples-02

    /// <summary>
    ///     バイト配列についてのサンプルです。
    /// </summary>
    [Sample]
    public class ByteArraySamples02 : IExecutable
    {
        public void Execute()
        {
            //
            // バイト列を16進数文字列へ
            //
            var buf = new byte[5];
            new Random().NextBytes(buf);

            Output.WriteLine(BitConverter.ToString(buf));
        }
    }

    #endregion
}
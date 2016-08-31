using System;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{
    #region ByteArraySamples-09

    /// <summary>
    ///     バイト配列についてのサンプルです。
    /// </summary>
    [Sample]
    public class ByteArraySamples09 : IExecutable
    {
        public void Execute()
        {
            //
            // 利用しているアーキテクチャのエンディアンを判定.
            //
            Output.WriteLine(BitConverter.IsLittleEndian);
        }
    }

    #endregion
}
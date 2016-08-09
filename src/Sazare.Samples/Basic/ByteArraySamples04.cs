using System;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{
    #region ByteArraySamples-04

    /// <summary>
    ///     バイト配列についてのサンプルです。
    /// </summary>
    [Sample]
    public class ByteArraySamples04 : IExecutable
    {
        public void Execute()
        {
            //
            // 数値からバイト列へ変換
            //
            var i = 123456;
            var buf = BitConverter.GetBytes(i);

            Output.WriteLine(BitConverter.ToString(buf));
        }
    }

    #endregion
}
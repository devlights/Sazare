using System;
using System.Text;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{
    #region ByteArraySamples-06

    /// <summary>
    ///     バイト配列についてのサンプルです。
    /// </summary>
    [Sample]
    public class ByteArraySamples06 : IExecutable
    {
        public void Execute()
        {
            //
            // 文字列をバイト列へ
            //
            var s = "gsf_zero1";
            var buf = Encoding.ASCII.GetBytes(s);

            Output.WriteLine(BitConverter.ToString(buf));
        }
    }

    #endregion
}
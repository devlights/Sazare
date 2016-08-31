using System.Text;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{
    #region ByteArraySamples-07

    /// <summary>
    ///     バイト配列についてのサンプルです。
    /// </summary>
    [Sample]
    public class ByteArraySamples07 : IExecutable
    {
        public void Execute()
        {
            //
            // バイト列を文字列へ.
            //
            var s = "gsf_zero1";
            var buf = Encoding.ASCII.GetBytes(s);

            Output.WriteLine(Encoding.ASCII.GetString(buf));
        }
    }

    #endregion
}
using System;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{

    #region NumberFormatSamples-01

    /// <summary>
    ///     数値フォーマットのサンプルです。
    /// </summary>
    [Sample]
    public class NumberFormatSamples01 : IExecutable
    {
        public void Execute()
        {
            var d = 99M;
            Output.WriteLine(Math.Round(d, 1));
            Output.WriteLine("{0:##0.0}", d);
        }
    }

    #endregion
}
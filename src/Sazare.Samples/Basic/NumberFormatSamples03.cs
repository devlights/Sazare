using System;
using System.Globalization;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{

    #region NumberFormatSamples-03

    /// <summary>
    ///     数値フォーマットのサンプルです。
    /// </summary>
    [Sample]
    public class NumberFormatSamples03 : IExecutable
    {
        public void Execute()
        {
            var s = "123,456";

            try
            {
                // ERROR.
                var i2 = int.Parse(s);
                Output.WriteLine(i2);
            }
            catch (FormatException ex)
            {
                Output.WriteLine(ex.Message);
            }

            var i3 = int.Parse(s, NumberStyles.AllowThousands);
            Output.WriteLine(i3);
        }
    }

    #endregion
}
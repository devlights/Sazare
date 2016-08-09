using System.Diagnostics.CodeAnalysis;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{

    #region ExtensionMethodSample-01

    /// <summary>
    ///     拡張メソッドのサンプル1です。
    /// </summary>
    [Sample]
    public class ExtensionMethodSample01 : IExecutable
    {
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void Execute()
        {
            string s = null;
            s.PrintMyName();
        }
    }

    // ReSharper disable once InconsistentNaming
    public static class ExtensionMethodSample01_ExtClass
    {
        public static void PrintMyName(this string self)
        {
            Output.WriteLine(self == null);
            Output.WriteLine("GSF-ZERO1.");
        }
    }

    #endregion
}
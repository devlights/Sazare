// ReSharper disable CheckNamespace

using System;
using System.Reflection;
using Sazare.Common;

namespace Sazare.Samples
{

    #region ByRefの引数をもつメソッドをリフクレクションで取得

    /// <summary>
    ///     ByRefの引数を持つメソッドをリフレクションで取得するサンプルです。
    /// </summary>
    [Sample]
    public class HasByRefParameterMethodReflectionSample : IExecutable
    {
        public void Execute()
        {
            var type = typeof(HasByRefParameterMethodReflectionSample);
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            Type[] paramTypes = {typeof(string), Type.GetType("System.Int32&"), typeof(int)};

            var methodInfo = type.GetMethod("SetPropertyValue", flags, null, paramTypes, null);
            Output.WriteLine(methodInfo);
        }

        // <summary>
        // Dummy Method.
        // </summary>
        protected void SetPropertyValue(string propName, ref int refVal, int val)
        {
            //
            // nop.
            //
        }
    }

    #endregion
}
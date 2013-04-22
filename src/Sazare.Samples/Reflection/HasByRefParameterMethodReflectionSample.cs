namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;

  #region ByRefの引数をもつメソッドをリフクレクションで取得
  /// <summary>
  /// ByRefの引数を持つメソッドをリフレクションで取得するサンプルです。
  /// </summary>
  [Sample]
  public class HasByRefParameterMethodReflectionSample : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      Type type = typeof(HasByRefParameterMethodReflectionSample);
      BindingFlags flags = (BindingFlags.NonPublic | BindingFlags.Instance);
      Type[] paramTypes = new Type[] { typeof(string), Type.GetType("System.Int32&"), typeof(int) };

      MethodInfo methodInfo = type.GetMethod("SetPropertyValue", flags, null, paramTypes, null);
      Console.WriteLine(methodInfo);
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

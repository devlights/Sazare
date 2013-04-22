namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;

  #region Genericなメソッドをリフレクションで取得
  /// <summary>
  /// ジェネリックメソッドをリフレクションで取得するサンプルです。
  /// </summary>
  [Sample]
  public class GenericMethodReflectionSample : Sazare.Common.IExecutable
  {
    public void Execute()
    {
      Type type = typeof(GenericMethodReflectionSample);
      BindingFlags flags = (BindingFlags.NonPublic | BindingFlags.Instance);

      //
      // ジェネリックメソッドが一つしかない場合は以下のようにして取得できる。
      // 
      // ジェネリック定義されている状態のメソッド情報を取得.
      // MethodInfo mi = type.GetMethod("SetPropertyValue", flags);
      // 型引数を設定して、実メソッド情報を取得
      // MethodInfo genericMi = mi.MakeGenericMethod(new Type[]{ typeof(DateTime) });
      //
      // しかし、同名メソッドのオーバーロードが複数存在する場合は一旦GetMethodsにて
      // ループさせ、該当するメソッドを見つける作業が必要となる。
      //
      // [参照URL]
      // http://www.codeproject.com/KB/dotnet/InvokeGenericMethods.aspx
      //
      string methodName = "SetPropertyValue";
      Type[] paramTypes = new Type[] { typeof(string), typeof(DateTime), typeof(DateTime) };
      foreach (MethodInfo mi in type.GetMethods(flags))
      {
        if (mi.IsGenericMethod && mi.IsGenericMethodDefinition && mi.ContainsGenericParameters)
        {
          if (mi.Name == methodName && mi.GetParameters().Length == paramTypes.Length)
          {
            MethodInfo genericMi = mi.MakeGenericMethod(new Type[] { typeof(DateTime) });
            Console.WriteLine(genericMi);
          }
        }
      }
    }

    protected void SetPropertyValue(string propName, ref int refVal, int val)
    {
      //
      // nop.
      //
    }

    protected void SetPropertyValue<T>(string propName, ref T refVal, T val)
    {
      //
      // nop.
      //
    }
  }  
#endregion
}

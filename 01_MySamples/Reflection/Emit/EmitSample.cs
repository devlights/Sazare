namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using System.Reflection.Emit;

  #region Emitのサンプル
  /// <summary>
  /// Emitのサンプルです。
  /// </summary>
  /// <remarks>
  /// HelloWorldを出力するクラスを動的生成します。
  /// </remarks>
  public class EmitSample : IExecutable
  {
    public interface ISayHello
    {
      void SayHello();
    }

    public void Execute()
    {
      //
      // 0.これから作成する型を格納するアセンブリ名作成.
      //
      AssemblyName asmName = new AssemblyName
      {
        Name = "DynamicTypes"
      };

      //
      // 1.AssemlbyBuilderの生成
      //
      AppDomain domain = AppDomain.CurrentDomain;
      AssemblyBuilder asmBuilder = domain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
      //
      // 2.ModuleBuilderの生成.
      //
      ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule("HelloWorld");
      //
      // 3.TypeBuilderの生成.
      //
      TypeBuilder typeBuilder = modBuilder.DefineType("SayHelloImpl", TypeAttributes.Public, typeof(object), new Type[] { typeof(ISayHello) });
      //
      // 4.MethodBuilderの生成
      //
      MethodAttributes methodAttr = (MethodAttributes.Public | MethodAttributes.Virtual);
      MethodBuilder methodBuilder = typeBuilder.DefineMethod("SayHello", methodAttr, typeof(void), new Type[] { });
      typeBuilder.DefineMethodOverride(methodBuilder, typeof(ISayHello).GetMethod("SayHello"));
      //
      // 5.ILGeneratorを生成し、ILコードを設定.
      //
      ILGenerator il = methodBuilder.GetILGenerator();
      il.Emit(OpCodes.Ldstr, "Hello World");
      il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
      il.Emit(OpCodes.Ret);
      //
      // 6.作成した型を取得.
      //
      Type type = typeBuilder.CreateType();
      //
      // 7.型を具現化.
      //
      ISayHello hello = (ISayHello)Activator.CreateInstance(type);
      //
      // 8.実行.
      //
      hello.SayHello();
    }
  }
  #endregion
}

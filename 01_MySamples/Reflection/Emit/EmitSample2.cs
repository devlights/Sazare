namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using System.Reflection.Emit;

  #region Emitのサンプル２
  /// <summary>
  /// Emitのサンプル２です。
  /// </summary>
  /// <remarks>
  /// プロパティを持つクラスを動的生成します。
  /// </remarks>
  public class EmitSample2 : IExecutable
  {
    public void Execute()
    {
      //////////////////////////////////////////////////////////////////
      //
      // プロパティ付きの型を作成.
      //
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
      AssemblyBuilder asmBuilder = domain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
      //
      // 2.ModuleBuilderの生成.
      //
      ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule(asmName.Name, string.Format("{0}.dll", asmName.Name));
      //
      // 3.TypeBuilderの生成.
      //
      TypeBuilder typeBuilder = modBuilder.DefineType("WithPropClass", TypeAttributes.Public, typeof(object), Type.EmptyTypes);
      //
      // 4.FieldBuilderの生成.
      //
      FieldBuilder fieldBuilder = typeBuilder.DefineField("_message", typeof(string), FieldAttributes.Private);
      //
      // 5.PropertyBuilderの生成.
      //
      PropertyBuilder propBuilder = typeBuilder.DefineProperty("Message", System.Reflection.PropertyAttributes.HasDefault, typeof(string), Type.EmptyTypes);
      //
      // 6.プロパティは実際にはGetter/Setterメソッドの呼び出しとなる為、それらのメソッドを作成する必要がある。
      //   それらのメソッドに付加するメソッド属性を定義.
      //
      MethodAttributes propAttr = (MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig);
      //
      // 7.Getメソッドの生成.
      //
      MethodBuilder getterMethodBuilder = typeBuilder.DefineMethod("get_Message", propAttr, typeof(string), Type.EmptyTypes);
      //
      // 8.ILGeneratorを生成し、Getter用のILコードを設定.
      //
      ILGenerator il = getterMethodBuilder.GetILGenerator();
      il.Emit(OpCodes.Ldarg_0);
      il.Emit(OpCodes.Ldfld, fieldBuilder);
      il.Emit(OpCodes.Ret);
      //
      // 9.Setメソッドを生成
      //
      MethodBuilder setterMethodBuilder = typeBuilder.DefineMethod("set_Message", propAttr, null, new Type[] { typeof(string) });
      //
      // 10.ILGeneratorを生成し、Setter用のILコードを設定.
      //
      il = setterMethodBuilder.GetILGenerator();
      il.Emit(OpCodes.Ldarg_0);
      il.Emit(OpCodes.Ldarg_1);
      il.Emit(OpCodes.Stfld, fieldBuilder);
      il.Emit(OpCodes.Ret);
      //
      // 11.PropertyBuilderにGetter/Setterを紐付ける.
      //
      propBuilder.SetGetMethod(getterMethodBuilder);
      propBuilder.SetSetMethod(setterMethodBuilder);
      //
      // 12.作成した型を取得.
      //
      Type type = typeBuilder.CreateType();
      //
      // 13.型を具現化.
      //
      object withPropObj = Activator.CreateInstance(type);
      //
      // 14.実行.
      //
      PropertyInfo propInfo = type.GetProperty("Message");
      propInfo.SetValue(withPropObj, "HelloWorld", null);
      Console.WriteLine(propInfo.GetValue(withPropObj, null));
      //
      // 15.(option) 作成したアセンブリを保存.
      //
      asmBuilder.Save(string.Format("{0}.dll", asmName.Name));
    }
  }
  #endregion
}

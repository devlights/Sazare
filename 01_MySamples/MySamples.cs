// vim:set ts=2 sw=2 et ws is nowrap ft=cs:
//////////////////////////////////////////////////////////////////////
//
// 基本的なクラスライブラリに関するサンプルを集めたファイル.
//
//////////////////////////////////////////////////////////////////////
namespace Gsf.Samples
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.ComponentModel.Composition;
  using System.ComponentModel.Composition.Hosting;
  using System.Data;
  using System.Data.Common;
  using System.Data.Linq;
  using System.Diagnostics;
  using System.Diagnostics.Contracts;
  using System.Drawing;
  using System.Drawing.Imaging;
  using System.Globalization;
  using System.IO;
  using System.IO.Compression;
  using System.Linq;
  using System.Net;
  using System.Net.NetworkInformation;
  using System.Reflection;
  using System.Reflection.Emit;
  using System.Runtime;
  using System.Runtime.CompilerServices;
  using System.Runtime.ConstrainedExecution;
  using System.Runtime.InteropServices;
  using System.Runtime.Remoting;
  using System.Runtime.Remoting.Messaging;
  using System.Runtime.Serialization;
  using System.Runtime.Serialization.Formatters.Binary;
  using System.Security;
  using System.ServiceModel;
  using System.ServiceModel.Syndication;
  using System.Text;
  using System.Text.RegularExpressions;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Windows;
  using System.Windows.Controls;
  //using System.Windows.Forms;
  using System.Windows.Markup;
  using System.Xaml;
  using System.Xml;
  using System.Xml.Linq;

  //
  // Alias設定.
  //
  using GDIColor                     = System.Drawing.Color;
  using GDIImage                     = System.Drawing.Image;
  using GDISize                      = System.Drawing.Size;
  using GDIPoint                     = System.Drawing.Point;

  using WinFormsButton               = System.Windows.Forms.Button;
  using WinFormsControl              = System.Windows.Forms.Control;
  using WinFormsDockStyle            = System.Windows.Forms.DockStyle;
  using WinFormsFlowDirection        = System.Windows.Forms.FlowDirection;
  using WinFormsFlowLayoutPanel      = System.Windows.Forms.FlowLayoutPanel;
  using WinFormsForm                 = System.Windows.Forms.Form;
  using WinFormsFormStartPosition    = System.Windows.Forms.FormStartPosition;
  using WinFormsLabel                = System.Windows.Forms.Label;
  using WinFormsListBox              = System.Windows.Forms.ListBox;
  using WinFormsMessageBox           = System.Windows.Forms.MessageBox;
  using WinFormsProgressBar          = System.Windows.Forms.ProgressBar;
  using WinFormsProgressBarStyle     = System.Windows.Forms.ProgressBarStyle;
  using WinFormsTextBox              = System.Windows.Forms.TextBox;
  using WinFormsApplication          = System.Windows.Forms.Application;

  using WinFormsFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
  using WinFormsMethodInvoker        = System.Windows.Forms.MethodInvoker;

  #region 定数定義
  /// <summary>
  /// 定数定義クラス
  /// </summary>
  public class Constants
  {
    /// <summary>
    /// 行生成数.
    /// </summary>
    public const int GENERATE_ROW_COUNT = 30000;
  }
  #endregion

  #region 共通インターフェース定義
  /// <summary>
  /// 各サンプルクラスが共通して実装しているインターフェースです。
  /// </summary>
  interface IExecutable
  {
    /// <summary>
    /// 処理を実行します。
    /// </summary>
    void Execute();
  }
  #endregion

  #region 共通拡張クラス
  /// <summary>
  /// 共通で利用しているString拡張クラスです。
  /// </summary>
  public static class StringExtensions
  {
    /// <summary>
    /// 数値に変換します。
    /// </summary>
    /// <param name="self">自分自身</param>
    /// <return>自分自身を数値に変換した値.</return>
    public static int ToInt(this string self)
    {
      int i;
      if (!int.TryParse(self, out i))
      {
        return int.MinValue;
      }
      
      return i;
    }
  }
  #endregion

  #region ダミークラス
  /// <summary>
  /// ダミークラス
  /// </summary>
  class Dummy : IExecutable
  {
    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      Console.WriteLine("THIS IS DUMMY CLASS.");
    }
  }
  #endregion
  
  #region サンプルの起動を担当するクラス
  /// <summary>
  /// サンプルの起動を担当するクラスです。
  /// </summary>
  /// <remarks>
  /// 本クラスがエントリポイントとなります。
  /// </remarks>
  public class SampleLauncher
  {
    /// <summary>
    /// エントリポイントメソッド
    /// </summary>
    /// <param name="args">起動時引数</param>
    [STAThread]
    static void Main(string[] args)
    {
      string className = typeof(Dummy).Name;
      if (args.Length != 0)
      {
        className = args[0];
      }

      if (!string.IsNullOrEmpty(className))
      {
        className = string.Format("{0}.{1}", typeof(SampleLauncher).Namespace, className);
      }

      //
      // 指定されたクラスを起動.
      //
      try
      {
        Assembly assembly = Assembly.GetExecutingAssembly();
        ObjectHandle handle = Activator.CreateInstance(assembly.FullName, className);
        if (handle != null)
        {
          object clazz = handle.Unwrap();

          if (clazz != null)
          {
            (clazz as IExecutable).Execute();
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }
  }
  #endregion

  #region デリゲートのサンプル (.net framework 1.1)
  /// <summary>
  /// デリゲートのサンプル（.NET Framework 1.1）
  /// </summary>
  class DelegateSample : IExecutable
  {
    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      WinFormsMethodInvoker methodInvoker = new WinFormsMethodInvoker(DelegateMethod);
      methodInvoker();
    }

    /// <summary>
    /// デリゲートメソッド.
    /// </summary>
    private void DelegateMethod()
    {
      Console.WriteLine("SAMPLE_DELEGATE_METHOD.");
    }
  }
  #endregion

  #region 匿名デリゲートのサンプル (.net framework 2.0)
  /// <summary>
  /// 匿名デリゲート(anonymous delegete)のサンプル（.NET Framework 2.0）
  /// </summary>
  class AnonymousDelegateSample : IExecutable
  {
    /// <summary>
    /// 本サンプルで利用するデリゲートの定義
    /// </summary>
    delegate void SampleDelegate();

    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      //
      // 匿名メソッドを構築して実行.
      //
      SampleDelegate d = delegate()
      {
        Console.WriteLine("SAMPLE_ANONYMOUS_DELEGATE.");
      };

      d.Invoke();
    }

  }
  #endregion

  #region ラムダのサンプル (.net framework 3.5)
  /// <summary>
  /// ラムダ(lambda)のサンプル（.NET Framework 3.5）
  /// </summary>
  class LambdaSample : IExecutable
  {
    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      WinFormsMethodInvoker methodInvoker = () =>
      {
        Console.WriteLine("SAMPLE_LAMBDA_METHOD.");
      };

      methodInvoker();

      Action action = () =>
      {
        Console.WriteLine("SAMPLE_LAMBDA_METHOD_ACTION.");
      };

      action();

      Func<int, int, int> sum = (x, y) =>
      {
        return (x + y);
      };

      Console.WriteLine(sum(10, 20));

      Func<int, int, int> sum2 = (int x, int y) =>
      {
        return (x + y);
      };

      Console.WriteLine(sum2(10, 20));
    }
  }
  #endregion

  #region スレッドを直接作成
  /// <summary>
  /// スレッドを直接作成するサンプル.
  /// </summary>
  class ThreadSample : IExecutable
  {
    /// <summary>
    /// ロックオブジェクト
    /// </summary>
    object _lockObject = new object();
    /// <summary>
    /// 件数
    /// </summary>
    int _count = 0;

    /// <summary>
    /// スレッドを実行する際の引数として利用されるクラスです。
    /// </summary>
    class ThreadParameter
    {
      public int Count
      {
        get;
        set;
      }

      public DateTime Time
      {
        get;
        set;
      }
    }

    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      //
      // ThreadStartデリゲートを用いた場合.
      //
      ThreadStart ts = () =>
      {
        lock (_lockObject)
        {
          if (_count < 10)
          {
            _count++;
          }
        }

        Console.WriteLine("Count={0}", _count);
      };

      for (int i = 0; i < 15; i++)
      {
        Thread t = new Thread(ts);
        t.IsBackground = false;

        t.Start();

        //
        // 確実にスレッドの走る順序を揃えるには以下のようにする。
        // (もっともこれをやるとスレッドの意味がないが・・)
        //
        //t.Join();
      }

      //
      // ParameterizedThreadStartを用いた場合.
      //
      ParameterizedThreadStart pts = (data) =>
      {
        ThreadParameter p = data as ThreadParameter;
        Thread.Sleep(150);
        Console.WriteLine("Thread Count:{0}, Time:{1}", p.Count, p.Time.ToString("hh:mm:ss.fff"));
      };

      for (int i = 0; i < 15; i++)
      {
        Thread t = new Thread(pts);
        t.IsBackground = false;

        t.Start(new ThreadParameter
        {
          Count = i,
          Time = DateTime.Now
        });

        //
        // 確実にスレッドの走る順序を揃えるには以下のようにする。
        // (もっともこれをやるとスレッドの意味がないが・・)
        //
        //t.Join();
      }
    }
  }
  #endregion

  #region スレッドプールを利用したスレッド処理
  /// <summary>
  /// スレッドプール(ThreadPool)を利用したスレッド処理のサンプルです。
  /// </summary>
  class ThreadPoolSample : IExecutable
  {
    /// <summary>
    /// スレッドの状態を表すデータクラスです。
    /// </summary>
    class StateInfo
    {
      public int Count
      {
        get;
        set;
      }

      public DateTime Time
      {
        get;
        set;
      }
    }

    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      for (int i = 0; i < 15; i++)
      {
        ThreadPool.QueueUserWorkItem((stateInfo) =>
        {
          StateInfo p = stateInfo as StateInfo;
          Thread.Sleep(150);
          Console.WriteLine("Thread Count:{0}, Time:{1}", p.Count, p.Time.ToString("hh:mm:ss.fff"));
        }, 
        new StateInfo
        {
          Count = i,
          Time = DateTime.Now
        });
      }

      Thread.Sleep(2000);
    }
  }
  #endregion

  #region 非同期デリゲートを利用したスレッド処理
  /// <summary>
  /// 非同期デリゲートを利用したスレッド処理のサンプルです。
  /// </summary>
  class AsyncDelegateSample : IExecutable
  {
    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      Console.WriteLine("[MAIN] START. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);

      WinFormsMethodInvoker invoker = () =>
      {
        Console.WriteLine("[DELE] START. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);

        for (int i = 0; i < 10; i++)
        {
          Console.WriteLine("[DELE] PROCESSING. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
          Thread.Sleep(105);
        }
      };

      IAsyncResult asyncResult = invoker.BeginInvoke(
        (ar) =>
        {
          WinFormsMethodInvoker caller = ar.AsyncState as WinFormsMethodInvoker;
          caller.EndInvoke(ar);
          Console.WriteLine("[DELE] END. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
        },
        invoker
      );

      for (int i = 0; i < 10; i++)
      {
        Console.WriteLine("[MAIN] PROCESSING. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
        Thread.Sleep(100);
      }

      Thread.Sleep(3500);
      Console.WriteLine("[MAIN] END. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
    }
  }
  #endregion

  #region BackgroundWorkerを利用したスレッド処理
  /// <summary>
  /// BackgroundWorkerを利用したスレッド処理のサンプルです。
  /// </summary>
  class BackgroundWorkerSample : IExecutable
  {
    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      Console.WriteLine("[MAIN] START. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);

      BackgroundWorker worker = new BackgroundWorker();

      //
      // 非同期処理のイベントをハンドル.
      //
      worker.DoWork += (s, e) =>
      {
        Console.WriteLine("[WORK] START. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);

        for (int i = 0; i < 10; i++)
        {
          Console.WriteLine("[WORK] PROCESSING. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
          Thread.Sleep(105);
        }
      };

      //
      // 非同期処理が終了した際のイベントをハンドル.
      //
      worker.RunWorkerCompleted += (s, e) =>
      {
        if (e.Error != null)
        {
          Console.WriteLine("[WORK] ERROR OCCURED. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        Console.WriteLine("[WORK] END. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
      };

      //
      // 非同期処理を開始.
      //
      worker.RunWorkerAsync();

      //
      // メインスレッドの処理.
      //
      for (int i = 0; i < 10; i++)
      {
        Console.WriteLine("[MAIN] PROCESSING. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
        Thread.Sleep(100);
      }

      Thread.Sleep(1000);
      Console.WriteLine("[MAIN] END. ThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
    }
  }
  #endregion

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

  #region Emitのサンプル3
  /// <summary>
  /// Emitのサンプル３です。
  /// </summary>
  /// <remarks>
  /// カスタム属性を持つクラスを動的生成します。
  /// </remarks>
  public class EmitSample3 : IExecutable
  {
    [AttributeUsage(AttributeTargets.Class)]
    public class IsDynamicTypeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CreatorAttribute : Attribute
    {
      public CreatorAttribute(string name)
      {
        CreatorName = name;
      }

      public string CreatorName
      {
        get;
        set;
      }
    }

    private static readonly string ASM_NAME = "DynamicTypes";
    private static readonly string MOD_NAME = string.Format("{0}.dll", ASM_NAME);

    public void Execute()
    {
      AssemblyName asmName = new AssemblyName
      {
        Name = ASM_NAME
      };
      
      AppDomain appDomain = AppDomain.CurrentDomain;
      AssemblyBuilder asmBuilder = appDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
      ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule(ASM_NAME, MOD_NAME);
      TypeBuilder typeBuilder = modBuilder.DefineType("AttrTest", TypeAttributes.Public, typeof(object), Type.EmptyTypes);

      //
      // 型に対してカスタム属性を付加する。
      // 複数の属性を付ける場合は、AddAttributeメソッド内のようにSetCustomAttributeメソッドを
      // 複数呼びます。
      //
      AddAttribute(typeBuilder, typeof(IsDynamicTypeAttribute), Type.EmptyTypes, new object[] { });
      AddAttribute(typeBuilder, typeof(CreatorAttribute), new Type[] { typeof(string) }, new object[] { "gsf.zero1" });

      Type type = typeBuilder.CreateType();
      object obj = Activator.CreateInstance(type);

      object[] attrs = type.GetCustomAttributes(true);
      if (attrs.Length > 0)
      {
        foreach (object attr in attrs)
        {
          Console.WriteLine(attr);

          if (attr is CreatorAttribute)
          {
            Console.WriteLine("\tName={0}", (attr as CreatorAttribute).CreatorName);
          }
        }
      }

      asmBuilder.Save(MOD_NAME);
    }

    private void AddAttribute(TypeBuilder typeBuilder, Type attrType, Type[] attrCtorParamTypes, object[] attrCtorParams)
    {
      ConstructorInfo ctorInfo = attrType.GetConstructor(attrCtorParamTypes);

      if (ctorInfo != null)
      {
        CustomAttributeBuilder attrBuilder = new CustomAttributeBuilder(ctorInfo, attrCtorParams);
        typeBuilder.SetCustomAttribute(attrBuilder);
      }
    }
  }
  #endregion

  #region ByRefの引数をもつメソッドをリフクレクションで取得
  /// <summary>
  /// ByRefの引数を持つメソッドをリフレクションで取得するサンプルです。
  /// </summary>
  public class HasByRefParameterMethodReflectionSample : IExecutable
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

  #region Genericなメソッドをリフレクションで取得
  /// <summary>
  /// ジェネリックメソッドをリフレクションで取得するサンプルです。
  /// </summary>
  public class GenericMethodReflectionSample : IExecutable
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

  #region Reflection-01
  /// <summary>
  /// リフレクションのサンプル1です。
  /// </summary>
  public class ReflectionSample01 : IExecutable
  {
    public void Execute()
    {
      //
      // Typeオブジェクトの取得.
      //
      // 1.typeofを使用.
      Type type1 = typeof(string);

      //
      // 2.型名から取得.
      //
      string typeName = "System.String";
      Type type2 = Type.GetType(typeName);

      //
      // 3.ジェネリック型をtypeofで取得.
      //
      Type type3 = typeof(List<string>);

      //
      // 4.ジェネリック型を型名から取得.
      //
      typeName = "System.Collections.Generic.List`1[System.String]";
      Type type4 = Type.GetType(typeName);

      //
      // 5.型引数が1つ以上の場合.
      //
      typeName = "System.Collections.Generic.Dictionary`2[System.String, System.Int32]";
      Type type5 = Type.GetType(typeName);

      //
      // 6.ジェネリック型を型引数無しでTypeオブジェクトとして取得し、後から型引数を与える場合.
      //
      typeName = "System.Collections.Generic.List`1";
      Type type6 = Type.GetType(typeName);

      Type type7 = null;
      if (type6.IsGenericType && type6.IsGenericTypeDefinition)
      {
        type7 = type6.MakeGenericType(new Type[] { typeof(string) });
      }

      Console.WriteLine(type1);
      Console.WriteLine(type2);
      Console.WriteLine(type3);
      Console.WriteLine(type4);
      Console.WriteLine(type5);
      Console.WriteLine(type6);
      Console.WriteLine(type7);

    }
  }
  #endregion

  #region Reflection-02
  /// <summary>
  /// リフレクションのサンプル2です。
  /// </summary>
  public class ReflectionSample02 : IExecutable
  {
    public void Execute()
    {
      Type type = typeof(List<string>);

      //
      // Activatorを利用してインスタンス化.（Typeのみの指定）
      //
      object obj = Activator.CreateInstance(type);
      Console.WriteLine(obj.GetType());

      //
      // Activatorを利用してインスタンス化.(Assembly名と型名)
      // この場合、戻り値はSystem.Runtime.Remoting.ObjectHandleになります。
      //
      ObjectHandle objHandle = Activator.CreateInstance(Assembly.GetAssembly(type).FullName, type.FullName);
      obj = objHandle.Unwrap();
      Console.WriteLine(objHandle.GetType());
      Console.WriteLine(obj.GetType());

      //
      // Assemblyを利用してインスタンス化.
      // 以下が使えるのは、対象となるクラスが既に読み込み済みのアセンブリの場合です。
      // まだ読み込まれていないアセンブリに含まれるクラスの場合は先にLoadしてから使います。
      //
      obj = Assembly.GetAssembly(type).CreateInstance(type.FullName);
      Console.WriteLine(obj.GetType());
    }
  }
  #endregion

  #region ExtensionMethod-01
  /// <summary>
  /// 拡張メソッドのサンプル1です。
  /// </summary>
  public class ExtensionMethodSample01 : IExecutable
  {
    public void Execute()
    {
      string s = null;
      s.PrintMyName();
    }
  }

  public static class ExtensionMethodSample01_ExtClass
  {
    public static void PrintMyName(this string self)
    {
      Console.WriteLine(self == null);
      Console.WriteLine("GSF-ZERO1.");
    }
  }
  #endregion

  #region DataTableExtensionsSample-01
  /// <summary>
  /// System.Data.Extensionsのサンプル1です。
  /// </summary>
  public class DataTableExtensionsSample01 : IExecutable
  {
    public void Execute()
    {
      DataTable table = BuildSampleTable();
      PrintTable("Before:", table);

      IEnumerable<DataRow> query = from row in table.AsEnumerable()
                     where (row.Field<int>("COL-1") % 2) != 0
                     select row;

      DataTable newTable = query.CopyToDataTable();
      PrintTable("After:", newTable);
    }

    DataTable BuildSampleTable()
    {
      DataTable table = new DataTable();

      table.BeginInit();
      table.Columns.Add("COL-1", typeof(int));
      table.Columns.Add("COL-2");
      table.EndInit();

      table.BeginLoadData();
      for (int i = 0; i < 5; i++)
      {
        table.LoadDataRow(new object[] { i, (i + 1).ToString() }, true);
      }
      table.EndLoadData();

      return table;
    }

    void PrintTable(string title, DataTable table)
    {
      Console.WriteLine(title);

      foreach (DataRow row in table.Rows)
      {
        Console.WriteLine("\t{0}, {1}", row[0], row[1]);
      }

      Console.WriteLine();
    }
  }
  #endregion

  #region DataTableExtensionsSample-02
  /// <summary>
  /// System.Data.Extensionsのサンプル2です。
  /// </summary>
  public class DataTableExtensionsSample02 : IExecutable
  {

    public void Execute()
    {
      DataTable table = BuildSampleTable();

      //
      // 1列目の情報をint型で取得.
      // 2列目の情報をstring型で取得.
      //
      int val1 = table.Rows[0].Field<int>("COL-1");
      string val2 = table.Rows[0].Field<string>("COL-2");
      PrintTable("Before:", table);

      //
      // 1列目の情報を変更.
      //
      table.Rows[0].SetField<int>("COL-1", 100);
      PrintTable("After:", table);

    }

    DataTable BuildSampleTable()
    {
      DataTable table = new DataTable();

      table.BeginInit();
      table.Columns.Add("COL-1", typeof(int));
      table.Columns.Add("COL-2");
      table.EndInit();

      table.BeginLoadData();
      for (int i = 0; i < 5; i++)
      {
        table.LoadDataRow(new object[] { i, (i + 1).ToString() }, true);
      }
      table.EndLoadData();

      return table;
    }

    void PrintTable(string title, DataTable table)
    {
      Console.WriteLine(title);

      foreach (DataRow row in table.Rows)
      {
        Console.WriteLine("\t{0}, {1}", row[0], row[1]);
      }

      Console.WriteLine();
    }
  }
  #endregion

  #region OrderedDictionarySample-01
  /// <summary>
  /// OrderedDictionaryのサンプル1です。
  /// </summary>
  public class OrderedDictionarySample01 : IExecutable
  {

    public void Execute()
    {

      Dictionary<string, string> dicA = new Dictionary<string, string>();
      for (int i = 0; i < 100; i++)
      {
        dicA.Add(i.ToString(), string.Format("HOGE-{0}", i.ToString()));
      }

      PrintDictionary(dicA);
      Console.WriteLine("");

      OrderedDictionary dicB = new OrderedDictionary();
      for (int i = 0; i < 100; i++)
      {
        dicB.Add(i.ToString(), string.Format("HOGE-{0}", i.ToString()));
      }

      PrintDictionary(dicB);
    }

    void PrintDictionary(Dictionary<string, string> dic)
    {
      foreach (KeyValuePair<string, string> pair in dic)
      {
        Console.WriteLine("{0}-{1}", pair.Key, pair.Value);
      }
    }

    void PrintDictionary(OrderedDictionary dic)
    {
      foreach (DictionaryEntry entry in dic)
      {
        Console.WriteLine("{0}-{1}", entry.Key, entry.Value);
      }
    }
  }
  #endregion

  #region CompileDirectiveSamples-01
  /// <summary>
  /// コンパイルディレクティブのサンプル1です。
  /// </summary>
  public class CompileDirectiveSamples01 : IExecutable
  {

    public void Execute()
    {
      Console.WriteLine("001:HELLO C#");

#if(DEBUG)
      Console.WriteLine("002:HELLO C# (DEBUG)");
#else
      Console.WriteLine("003:HELLO C# (ELSE)");
#endif
    }
  }
  #endregion

  #region AsyncResultSamples-01
  /// <summary>
  /// 非同期処理 (IAsyncResult) のサンプル１です。
  /// </summary>
  public class AsyncResultSamples01 : IExecutable
  {

    AutoResetEvent _are = new AutoResetEvent(false);

    public void Execute()
    {
      Func<DateTime, string> func = CallerMethod;

      IAsyncResult result = func.BeginInvoke(DateTime.Now, CallbackMethod, _are);
      _are.WaitOne();
    }

    string CallerMethod(DateTime d)
    {
      return d.ToString("yyyy/MM/dd HH:mm:ss");
    }

    void CallbackMethod(IAsyncResult ar)
    {
      AsyncResult result = ar as AsyncResult;
      Func<DateTime, string> caller = result.AsyncDelegate as Func<DateTime, string>;
      EventWaitHandle handle = result.AsyncState as EventWaitHandle;

      if (!result.EndInvokeCalled)
      {
        Console.WriteLine(caller.EndInvoke(result));
        handle.Set();
      }
    }
  }
  #endregion

  #region AsyncOperationSamples-01
  /// <summary>
  /// AsyncOperationのサンプル1です。
  /// </summary>
  public class AsyncOperationSamples01 : IExecutable
  {

    class SampleForm : WinFormsForm
    {

      public SampleForm()
      {
        Thread.CurrentThread.Name = "Main Thread";
        InitializeComponent();
      }

      protected void InitializeComponent()
      {
        SuspendLayout();

        WinFormsButton b = new WinFormsButton
        {
          Name = "btn01",
          Text = "btn01"
        };
        b.Click += (s, e) =>
        {

          WinFormsMessageBox.Show("Click Handler Begin : " + string.Format("{0}-{1}", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId));

          WinFormsButton btn = s as WinFormsButton;

          // この場所でAsyncOprationを作成するとWindowsFormsSynchronizationContextオブジェクトになる。
          // (現在のスレッドのコンテキストがWindows Formsのため)
          // (WindowsFormsSynchronizationContext.AutoInstallの値がデフォルトでtrueになっているので、最初のコントロールが
          //  newされた時にメッセージスレッド (ここではMain Thread)にWindowsFormsSynchronizationContextが読み込まれる)
          //
          // SynchonizationContextはSystem.Threading名前空間に存在し、以下の派生クラスを持つ。
          //    ⇒System.Windows.Forms.WindowsFormsSynchronizationContext   (WindowsForms用)
          //    ⇒System.Windows.Threading.DispatcherSynchronizationContext (WPF用)
          // それそれの派生クラスは、基本機能に加え、各自独自の動作とプロパティを持っている。
          WinFormsMessageBox.Show("Click Handler Begin SyncContext : " + SynchronizationContext.Current.ToString());

          // AsyncOperationManagerはCreateOperationメソッドが呼ばれた際に現在のSynchronizationContextをコピーして
          // AsyncOperationのコンストラクタに渡している模様。なので、AsyncOperationのPost,Sendが正確にメッセージスレッドに
          // 同期できるかどうかは、このAsyncOperationオブジェクトを作成した時点でのスレッドにSynchronizationContextがあるかどうかによる。
          // （新規でスレッドを作成した場合はSynchronizationContextはnullとなっている。）
          //
          // 尚、SynchronizationContextを直接用いて、Post, Sendを行ってもよい。
          // 現在のスレッドのSynchronizationContextを取得するには、SynchronizationContext.Currentで取得できる。
          // 存在しない場合は、以下のようにして設定できる。
          //
          // SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

          // この場所でAsyncOperationを作成した場合、このスレッド(Main Thread)上では既にWindowsFormsSynchronizationContextが作成されている
          // ので、それがAsyncOperationに渡る。
          // AsyncOperationは自身のPost, PostOperationCompletedが呼ばれた際に内部で保持しているSynchronizationContextに処理を委譲します。
          // （なので、正常にメッセージスレッドにてデリゲートが処理される事になる。）
          AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(btn);

          // 別スレッド側にも同じSynchronizationContextを利用させる場合は以下のようにして
          // コピーを作成し、保持しておき、対象のスレッドに設定する。
          //SynchronizationContext syncContext = SynchronizationContext.Current.CreateCopy();

          Thread t = new Thread(() =>
          {
            WinFormsMessageBox.Show("New Thread : " + string.Format("{0}-{1}", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId));

            // 新規で別のスレッドを作成した場合は最初SynchronizationContextはnullとなっている。
            // 新たに割り当てる場合は以下のようにする。
            //SynchronizationContext.SetSynchronizationContext(syncContext);

            // この場所でAsyncOprationを作成するとSynchronizationContextオブジェクトになる。
            // (AsyncOperationManagerは、現在のスレッドにてSynchronizationContextが存在しない場合は新たにSynchronizationContextを
            //  生成し、設定してからAsyncOperationを生成する。元々SynchronizationContextがAsyncOperationを生成する際に存在している場合は
            //  それをAsyncOperationに渡して生成する。）
            WinFormsMessageBox.Show("New Thread SyncContext Is Null?? (1) : " + (SynchronizationContext.Current == null).ToString());
            //AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(btn);
            WinFormsMessageBox.Show("New Thread SyncContext Is Null?? (2) : " + (SynchronizationContext.Current == null).ToString());

            // ここで表示されるSynchronizationContextは, AsyncOperationがどのスレッドで作成されたかによって
            // 出力される値が変わる。
            //
            // ■イベントハンドラ側でAsyncOperationが生成された場合：WindowsFormsSynchronizationContext
            // ■別スレッド側でAsyncOperationが生成された場合：    SynchronizationContext
            WinFormsMessageBox.Show(asyncOp.SynchronizationContext.ToString());

            // Post及びPostOperationCompletedメソッドの呼び出しは、実際にはAsyncOperationが内部で保持しているSynchronizationContext.Postを
            // 呼び出しているので、対象となるSynchronizationContextによって同期されるスレッドが異なる。
            //
            // ■イベントハンドラ側でAsyncOperationが生成された場合：メッセージスレッド側に同期 (Main Thread)
            // ■別スレッド側でAsyncOperationが生成された場合：    新たにスレッドが作成されその中で処理 (Thread Pool)
            asyncOp.Post((state) =>
            {
              Thread curThread = Thread.CurrentThread;
              WinFormsMessageBox.Show("AsyncOp.Post : " + string.Format("{0}-{1}-IsThreadPool:{2}", curThread.Name, curThread.ManagedThreadId, curThread.IsThreadPoolThread));
            }, asyncOp);

            asyncOp.PostOperationCompleted((state) =>
            {
              Thread curThread = Thread.CurrentThread;
              WinFormsMessageBox.Show("AsyncOp.PostOperationCompleted : " + string.Format("{0}-{1}-IsThreadPool:{2}", curThread.Name, curThread.ManagedThreadId, curThread.IsThreadPoolThread));
            }, asyncOp);
          });

          t.Name = "Sub Thread";
          t.IsBackground = true;
          t.Start();

        };

        WinFormsFlowLayoutPanel layout = new WinFormsFlowLayoutPanel();
        layout.Dock = WinFormsDockStyle.Fill;
        layout.Controls.Add(b);

        Controls.Add(layout);

        ResumeLayout();
      }
    }

    [STAThread]
    public void Execute()
    {
      // 以下のコメントを外す事でコントロールが最初にnewされた際に
      // WindowsFormsSynchronizationContextが読み込まれないように出来ます。
      // falseにすると、デフォルトでSynchronizationContextが読み込まれます。
      //WindowsFormsSynchronizationContext.AutoInstall = false;

      WinFormsApplication.EnableVisualStyles();
      WinFormsApplication.Run(new SampleForm());
    }
  }
  #endregion

  #region BitConverterSamples-01
  /// <summary>
  /// System.BitConverterクラスのサンプルです。
  /// </summary>
  public class BitConverterSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // バイト列から16進文字列への変換.
      //
      byte[] bytes = new byte[] { 1, 2, 10, 15, (byte)'a', (byte)'b', (byte)'q' };
      Console.WriteLine(BitConverter.ToString(bytes));

      //
      // 数値からバイト列への変換.
      // (一旦数値をバイト列に変換してから、16進に変換して表示)
      //
      int i = 100;
      Console.WriteLine(BitConverter.ToString(BitConverter.GetBytes(i)));

      int i2 = 0x12345678;
      Console.WriteLine(BitConverter.ToString(BitConverter.GetBytes(i2)));

      //
      // バイト列から数値への変換.
      //
      bytes = new byte[] { 1 };
      Console.WriteLine(BitConverter.ToBoolean(bytes, 0));

      bytes = new byte[] { 1, 0, 0, 0 };
      Console.WriteLine(BitConverter.ToInt32(bytes, 0));

      bytes = BitConverter.GetBytes((byte)'a');
      Console.WriteLine(BitConverter.ToChar(bytes, 0));
    }
  }
  #endregion

  #region ThreadingNamespaceSamples-01
  /// <summary>
  /// System.Threading名前空間に存在するクラスのサンプルです。
  /// </summary>
  public class ThreadingNamespaceSamples01 : IExecutable
  {

    private class ThreadStartDelegateState
    {
      public Thread TargetThread
      {
        get;
        set;
      }
      public string ArgumentData
      {
        get;
        set;
      }
      public string ReturnData
      {
        get;
        set;
      }

      public void ThreadStartHandlerMethod()
      {
        Thread.Sleep(TimeSpan.FromSeconds(1));
        ReturnData = Thread.CurrentThread.ManagedThreadId.ToString();
      }

      public void ParameterizedThreadStartHandlerMethod(object threadArgument)
      {
        ArgumentData = threadArgument as string;

        ThreadStartHandlerMethod();
      }

      public override string ToString()
      {
        return string.Format("Argument:{0}  Return:{1}", ArgumentData, ReturnData);
      }
    }

    public void Execute()
    {
      //
      // Threadクラス (1)
      //    基本的なメソッドなどについて.
      //

      //////////////////////////////////////////////////////////////////////////////
      //
      // ThreadStartデリゲートを用いた方法.
      //    ThreadStartデリゲートは引数も戻り値も持っていません。
      //    このデリゲートを利用したパターンで各スレッドにてデータを
      //    持たせる場合は、デリゲートメソッドを内包した状態クラスを作成します。
      //    後で結果を受け取る必要がある場合も同様です。
      //
      List<ThreadStartDelegateState> states = new List<ThreadStartDelegateState>();
      for (int i = 0; i < 5; i++)
      {
        ThreadStartDelegateState state = new ThreadStartDelegateState();
        Thread thread = new Thread(state.ThreadStartHandlerMethod);

        state.ArgumentData = string.Format("{0}番目のスレッド", i);
        state.TargetThread = thread;

        states.Add(state);

        //
        // 新規で作成したThreadはデフォルトでフォアグラウンドスレッドとなっている。
        // （ThreadPoolスレッドの場合はデフォルトでバックグラウンドスレッドとなっている。)
        //
        thread.IsBackground = true;

        // 
        // 別スレッド処理を開始する。
        // Startメソッドは即座に制御を呼び元に戻す。
        //
        thread.Start();
      }

      //
      // 全スレッドが終わってから結果を確認したいので、待ち合わせ.
      // (以下の一行をコメントアウトして実行すると、高い確率でReturnDataプロパティの値が
      //  設定されていない状態で結果が出力されるはずです。(結果がセットされる前にメイン処理が
      //  結果確認処理へと進むため))
      //
      states.ForEach(state =>
      {
        state.TargetThread.Join();
      });

      // 結果確認.
      states.ForEach(Console.WriteLine);

      //////////////////////////////////////////////////////////////////////////////
      //
      // ParameterizedThreadStartデリゲートを用いた方法.
      //    ParameterizedThreadStartデリゲートは引数を一つ与える事が出来ます。
      //    このデリゲートを利用したパターンで各スレッドにてデータを
      //    持たせる場合は、Thread.Startメソッドにて引数を引き渡します。
      //    後で結果を受け取る必要がある場合は、ThreadStartデリゲートと同じく
      //    状態クラスを作成し、戻り値を設定する必要があります。
      //
      states.Clear();
      for (int i = 0; i < 5; i++)
      {
        ThreadStartDelegateState state = new ThreadStartDelegateState();
        Thread thread = new Thread(state.ParameterizedThreadStartHandlerMethod);

        state.TargetThread = thread;
        states.Add(state);

        thread.IsBackground = true;
        thread.Start(string.Format("{0}番目のスレッド", i));
      }

      states.ForEach(state =>
      {
        state.TargetThread.Join();
      });
      states.ForEach(Console.WriteLine);
    }
  }
  #endregion

  #region ThreadingNamespaceSamples-02
  /// <summary>
  /// System.Threading名前空間に存在するクラスのサンプルです。
  /// </summary>
  public class ThreadingNamespaceSamples02 : IExecutable
  {

    public void Execute()
    {
      ////////////////////////////////////////////////////////////
      //
      // 無名データスロット.
      //    データスロットはどれかのスレッドで最初に作成したら
      //    全てのスレッドに対してスロットが割り当てられる。
      //
      LocalDataStoreSlot slot = Thread.AllocateDataSlot();

      List<Thread> threads = new List<Thread>();
      for (int i = 0; i < 10; i++)
      {
        Thread thread = new Thread(DoAnonymousDataSlotProcess);
        thread.Name = string.Format("Thread-{0}", i);
        thread.IsBackground = true;

        threads.Add(thread);

        thread.Start(slot);
      }

      threads.ForEach(thread =>
      {
        thread.Join();
      });

      Console.WriteLine(string.Empty);

      ////////////////////////////////////////////////////////////
      //
      // 名前付きデータスロット.
      //    名前がつけられる事以外は、無名のスロットと同じです。
      //    名前付きデータスロットは、最初にその名前が呼ばれた
      //    際に作成されます。
      //    FreeNamedDataSlotメソッドを呼ぶと現在のスロット設定
      //    が解放されます。
      //
      threads.Clear();
      for (int i = 0; i < 10; i++)
      {
        Thread thread = new Thread(DoNamedDataSlotProcess);
        thread.Name = string.Format("Thread-{0}", i);
        thread.IsBackground = true;

        threads.Add(thread);

        thread.Start(slot);
      }

      threads.ForEach(thread =>
      {
        thread.Join();
      });

      //
      // 利用したデータスロットを解放.
      //
      Thread.FreeNamedDataSlot("SampleSlot");

    }

    void DoAnonymousDataSlotProcess(object stateObj)
    {
      LocalDataStoreSlot slot = stateObj as LocalDataStoreSlot;

      //
      // スロットにデータを設定
      //
      Thread.SetData(slot, string.Format("ManagedThreadId={0}", Thread.CurrentThread.ManagedThreadId));

      //
      // 設定した内容を確認.
      //
      Console.WriteLine("[BEFORE] Thread:{0}   DataSlot:{1}", Thread.CurrentThread.Name, Thread.GetData(slot));

      //
      // 別のスレッドに処理を行って貰う為に一旦Sleepする。
      //
      Thread.Sleep(TimeSpan.FromSeconds(1));

      //
      // 再度確認.
      //
      Console.WriteLine("[AFTER ] Thread:{0}   DataSlot:{1}", Thread.CurrentThread.Name, Thread.GetData(slot));
    }

    void DoNamedDataSlotProcess(object stateObj)
    {
      //
      // スロットにデータを設定
      //
      Thread.SetData(Thread.GetNamedDataSlot("SampleSlot"), string.Format("ManagedThreadId={0}", Thread.CurrentThread.ManagedThreadId));

      //
      // 設定した内容を確認.
      //
      Console.WriteLine("[BEFORE] Thread:{0}   DataSlot:{1}", Thread.CurrentThread.Name, Thread.GetData(Thread.GetNamedDataSlot("SampleSlot")));

      //
      // 別のスレッドに処理を行って貰う為に一旦Sleepする。
      //
      Thread.Sleep(TimeSpan.FromSeconds(1));

      //
      // 再度確認.
      //
      Console.WriteLine("[AFTER ] Thread:{0}   DataSlot:{1}", Thread.CurrentThread.Name, Thread.GetData(Thread.GetNamedDataSlot("SampleSlot")));
    }
  }
  #endregion
  
  #region ThreadingNamespaceSamples-03
  public class ThreadingNamespaceSamples03 : IExecutable{

    public void Execute(){
      Thread thread = new Thread(ThreadProcess);

      thread.IsBackground = true;
      thread.Start();

      thread.Join();
    }

    void ThreadProcess(){
      ///////////////////////////////////////////////////
      //
      // スレッドクラスが持つその他のプロパティについて
      //
      Thread currentThread = Thread.CurrentThread;

      // 
      // IsAliveプロパティ
      //    ⇒スレッドが起動されており、まだ終了または中止されていない場合は
      //    　trueとなる。
      //
      Console.WriteLine("IsAlive={0}", currentThread.IsAlive);

      //
      // IsThreadPoolThread, ManagedThreadIdプロパティ
      //    ⇒それぞれ、スレッドプールスレッドかどうかとマネージスレッドを識別
      //    　する値が取得できる。
      //
      Console.WriteLine("IsThreadPoolThread={0}", currentThread.IsThreadPoolThread);
      Console.WriteLine("ManagedThreadId={0}",  currentThread.ManagedThreadId);

      //
      // Priorityプロパティ
      //    ⇒対象のスレッドの優先度（プライオリティ）を取得及び設定します。
      //    　Highestが最も高く、Lowestが最も低い.
      //
      Console.WriteLine("Priority={0}", currentThread.Priority);
    }
  }
  #endregion
  
  #region ThreadingNamespaceSamples-04
  public class ThreadingNamespaceSamples04 : IExecutable
  {
    public void Execute()
    {
      //
      // .NET 4.0より、Threadクラスに以下のメソッドが追加された。
      //   ・Yieldメソッド
      //
      // Yieldメソッドは、別のスレッドにタイムスライスを引き渡す為のメソッド。
      // 今までは、Thread.Sleepを利用したりして、タイムスライスを切り替えるよう
      // にしていたが、今後はこのメソッドを利用することが推奨される。
      //
      // 戻り値は、タイムスライスの引き渡しが成功したか否かが返ってくる。
      //
      
      //
      // テスト用にスレッドを２つ起動する.
      //
      Thread t1 = new Thread(ThreadProc);
      Thread t2 = new Thread(ThreadProc);
      
      t1.Start("T1");
      t2.Start("T2");
      
      t1.Join();
      t2.Join();
    }
    
    void ThreadProc(object stateObj)
    {
      string threadName = stateObj.ToString();
      Console.WriteLine("{0} Start", threadName);
      
      //
      // タイムスライスを切り替え.
      //
      Console.WriteLine("{0} Yield Call", threadName);
      bool isSuccess = Thread.Yield();
      Console.WriteLine("{0} Yield={1}", threadName, isSuccess);
      
      Console.WriteLine("{0} End", threadName);
    }
  }
  #endregion
  
  #region ThreadingNamespaceSamples-05
  public class ThreadingNamespaceSamples05 : IExecutable
  {
    public void Execute()
    {
      //
      // 普通にスレッドタイマーを作成し、コールバックの呼び出し間隔を無効に
      // した状態でタイマーを開始させる.
      //
      var timer = new System.Threading.Timer(TimerCallback);
      timer.Change(0, Timeout.Infinite);
      
      Thread.Sleep(10000);
    }
    
    void TimerCallback(object state)
    {
      Console.WriteLine("Timer Callback!!");
      
      var rnd = new Random();
      
      // 時間のかかる処理をシミュレート
      Thread.Sleep(rnd.Next(1000));
      Console.WriteLine("\tsleep done.");
      
      //
      // 再度Changeメソッドを呼び出して、次のコールバックを設定.
      //
      var timer = state as System.Threading.Timer;
      timer.Change(rnd.Next(700), Timeout.Infinite);
    }
  }
  #endregion

  #region ThreadStaticAttributeSamples-01
  /// <summary>
  /// ThreadStatic属性に関するサンプルです。
  /// </summary>
  public class ThreadStaticAttributeSamples01 : IExecutable
  {
    private class ThreadState
    {
      /// <summary>
      /// 各スレッド毎に固有の値を持つフィールド.
      /// </summary>
      [ThreadStatic]
      static KeyValuePair<string, int> NameAndId;
      /// <summary>
      /// 各スレッドで共有されるフィールド.
      /// </summary>
      static KeyValuePair<string, int> SharedNameAndId;

      public static void DoThreadProcess()
      {
        Thread thread = Thread.CurrentThread;

        //
        // ThreadStatic属性が付加されたフィールドと共有されたフィールドの両方に値を設定.
        //
        NameAndId = new KeyValuePair<string, int>(thread.Name, thread.ManagedThreadId);
        SharedNameAndId = new KeyValuePair<string, int>(thread.Name, thread.ManagedThreadId);

        Console.WriteLine("[BEFORE] ThreadStatic={0} Shared={1}", NameAndId, SharedNameAndId);

        //
        // 他のスレッドが動作できるようにする.
        //
        Thread.Sleep(TimeSpan.FromMilliseconds(200));

        Console.WriteLine("[AFTER ] ThreadStatic={0} Shared={1}", NameAndId, SharedNameAndId);
      }
    }

    public void Execute()
    {
      List<Thread> threads = new List<Thread>();
      for (int i = 0; i < 5; i++)
      {
        Thread thread = new Thread(ThreadState.DoThreadProcess);

        thread.Name = string.Format("Thread-{0}", i);
        thread.IsBackground = true;

        threads.Add(thread);

        thread.Start();
      }

      threads.ForEach(thread =>
      {
        thread.Join();
      });
    }
  }
  #endregion

  #region WhereArrayIteratorSamples-01
  /// <summary>
  /// WhereArrayIteratorに関するサンプルです。
  /// </summary>
  public class WhereArrayIteratorSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // whereを含んだlinqを作成すると, WhereArrayIteratorとなる。
      //
      var subset = from i in new[] { 1, 2, 3, 5, 6, 7 }
             where i < 7
             select i;

      Console.WriteLine("{0}", subset.GetType().Name);
      Console.WriteLine("{0}", subset.GetType().Namespace);

      //
      // whereを含まないlinqを作成すると、WhereSelectArrayIteratorとなる。
      //
      var subset2 = from i in new[] { 1, 2, 3, 5, 6, 7 }
              select i;

      Console.WriteLine("{0}", subset2.GetType().Name);
      Console.WriteLine("{0}", subset2.GetType().Namespace);
    }
  }
  #endregion

  #region EventSettingSamples-01
  /// <summary>
  /// 手動でイベントを制御する方法に関してのサンプルです。(EventHandlerList)
  /// </summary>
  public class EventSettingSamples01 : IExecutable
  {
    class Sample
    {
      object _eventTarget = new object();

      public Sample()
      {
        Events = new EventHandlerList();
      }

      public EventHandlerList Events
      {
        get;
        set;
      }

      public event EventHandler TestEvent;

      public void FireEvents()
      {
        if (TestEvent != null)
        {
          TestEvent(this, EventArgs.Empty);
        }
      }
    }

    public void Execute()
    {
      Sample obj = new Sample();

      EventHandler handler = (s, e) =>
      {
        Console.WriteLine("event raised.");
      };

      obj.TestEvent += handler;
      obj.FireEvents();
      obj.TestEvent -= handler;
      obj.FireEvents();

    }
  }
  #endregion

  #region EventSettingSamples-02
  /// <summary>
  /// 手動でイベントを制御する方法に関してのサンプルです。(EventHandlerList)
  /// </summary>
  public class EventSettingSamples02 : IExecutable
  {
    class Sample
    {
      object _eventTarget = new object();

      public Sample()
      {
        Events = new EventHandlerList();
      }

      public EventHandlerList Events
      {
        get;
        set;
      }

      public event EventHandler TestEvent
      {
        add
        {
          Console.WriteLine("add handler.");
          Events.AddHandler(_eventTarget, value);
        }
        remove
        {
          Console.WriteLine("remove handler.");
          Events.RemoveHandler(_eventTarget, value);
        }
      }

      public void FireEvents()
      {
        EventHandler handler = Events[_eventTarget] as EventHandler;

        if (handler != null)
        {
          handler(this, EventArgs.Empty);
        }
      }
    }

    public void Execute()
    {
      Sample obj = new Sample();

      EventHandler handler = (s, e) =>
      {
        Console.WriteLine("event raised.");
      };

      obj.TestEvent += handler;
      obj.FireEvents();
      obj.TestEvent -= handler;
      obj.FireEvents();

    }
  }
  #endregion

  #region MarshalByRefObjectSamples-01
  /// <summary>
  /// MarshalByRefObjectクラスに関するサンプルです。
  /// </summary>
  public class MarshalByRefObjectSamples01 : IExecutable
  {
    class CanNotMarshalByRef
    {
      public void PrintDomain()
      {
        Console.WriteLine("Object is executing in AppDomain \"{0}\"", AppDomain.CurrentDomain.FriendlyName);
      }
    }

    class CanMarshalByRef : MarshalByRefObject
    {
      public void PrintDomain()
      {
        Console.WriteLine("Object is executing in AppDomain \"{0}\"", AppDomain.CurrentDomain.FriendlyName);
      }
    }

    [Serializable]
    class CanSerialize
    {
      public void PrintDomain()
      {
        Console.WriteLine("Object is executing in AppDomain \"{0}\"", AppDomain.CurrentDomain.FriendlyName);
      }
    }

    public void Execute()
    {
      CanNotMarshalByRef obj1 = new CanNotMarshalByRef();
      obj1.PrintDomain();

      AppDomain newDomain = AppDomain.CreateDomain("new domain");

      /* ** ERROR **  "Gsf.Samples.MarshalByRefObjectSamples01+CanNotMarshalByRef"はシリアル化可能として設定されていません。
      CanNotMarshalByRef obj2 = 
          (CanNotMarshalByRef) newDomain.CreateInstanceAndUnwrap(
              Assembly.GetExecutingAssembly().FullName, 
              typeof(CanNotMarshalByRef).FullName
          );

      obj2.PrintDomain();
      */

      CanMarshalByRef obj3 =
          (CanMarshalByRef)newDomain.CreateInstanceAndUnwrap(
              Assembly.GetExecutingAssembly().FullName,
              typeof(CanMarshalByRef).FullName
          );

      obj3.PrintDomain();

      //
      // Serializable属性を付加しただけでは、実行は行えるが、別のAppDomain内からの
      // 実行ではなくて、呼び元のAppDomainでの実行となる。
      // (つまり、AppDomainの境界を越えていない。)
      //
      CanSerialize obj4 =
          (CanSerialize)newDomain.CreateInstanceAndUnwrap(
              Assembly.GetExecutingAssembly().FullName,
              typeof(CanSerialize).FullName
          );

      obj4.PrintDomain();
    }
  }
  #endregion

  #region NumberFormatSamples-01
  /// <summary>
  /// 数値フォーマットのサンプルです。
  /// </summary>
  public class NumberFormatSamples01 : IExecutable
  {
    public void Execute()
    {
      decimal d = 99M;
      Console.WriteLine(Math.Round(d, 1));
      Console.WriteLine("{0:##0.0}", d);
    }
  }
  #endregion

  #region NumberFormatSamples-02
  /// <summary>
  /// 数値フォーマットのサンプルです。
  /// </summary>
  public class NumberFormatSamples02 : IExecutable
  {
    public void Execute()
    {
      int i = 123456;
      Console.WriteLine("{0:N0}", i);

    }
  }
  #endregion

  #region NumberFormatSamples-03
  /// <summary>
  /// 数値フォーマットのサンプルです。
  /// </summary>
  public class NumberFormatSamples03 : IExecutable
  {
    public void Execute()
    {
      string s = "123,456";

      try
      {
        // ERROR.
        int i2 = int.Parse(s);
      }
      catch (FormatException ex)
      {
        Console.WriteLine(ex.Message);
      }

      int i3 = int.Parse(s, System.Globalization.NumberStyles.AllowThousands);
      Console.WriteLine(i3);

    }
  }
  #endregion
  
  #region NumberFormatSamples-04
  /// <summary>
  /// 数値フォーマットのサンプルです。
  /// </summary>
  public class NumberFormatSamples04 : IExecutable
  {
    public void Execute()
    {
      int iTestValue1 = 1;
      int iTestValue2 = 10;
      
      Console.WriteLine("iTestValue1: {0:D2}", iTestValue1);
      Console.WriteLine("iTestValue2: {0:D2}", iTestValue2);
      
      string sTestValue1 = iTestValue1.ToString();
      string sTestValue2 = iTestValue2.ToString();
      
      //
      // 元となるデータの型が数値ではない場合、2桁で表示と指定しても
      // フォーマットされない。(文字列の場合はそのまま"1"と表示される。)
      //
      Console.WriteLine("sTestValue1: {0:D2}", sTestValue1);
      Console.WriteLine("sTestValue2: {0:D2}", sTestValue2);
    }
  }
  #endregion

  #region StringFormatSamples-01
  /// <sumamry>
  /// 文字列の書式設定に関してのサンプルです。
  /// </summary>
  class StringFormatSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // 書式設定は、以下のようにして設定する.
      //   {0,-20:C}
      // 最初の0はインデックスを表す。必須項目。
      //
      // 桁数を指定する場合は、カンマを付与し桁数を指定する。
      // 桁数の値が負の値の場合は、左寄せ。
      // 桁数の値が正の値の場合は、右寄せとなる。
      // 桁数の指定はオプション。
      //
      // フォーマットを指定する場合は、コロンを付与しフォーマットのタイプを指定する。
      // Cは通貨を表す。
      // フォーマットの指定はオプション。
      //
      // フォーマットの種類などについては
      // http://msdn.microsoft.com/ja-jp/library/txafckwd(v=VS.100).aspx
      // を参照。
      //
      string format = "'{0,20:C}'";
      Console.WriteLine(format, 25000);
    }
  }
  #endregion

  #region EnumToStringSamples-01
  /// <summary>
  /// Enumに関するサンプルです。
  /// </summary>
  public class EnumToStringSamples01 : IExecutable
  {
    private enum MyColor
    {
      Red = 1,
      Blue,
      White,
      Black
    }

    public void Execute()
    {
      MyColor blue = MyColor.Blue;

      // 2
      Console.WriteLine(blue.ToString("D"));
      // 0x00000002
      Console.WriteLine("0x{0}", blue.ToString("X"));
      // Blue
      Console.WriteLine(blue.ToString("G"));
    }
  }
  #endregion

  #region PingSamples-01
  /// <summary>
  /// Pingクラスに関するサンプルです。
  /// </summary>
  public class PingSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // 同期での送信.
      // 
      string hostName = "localhost";
      int timeOut = 3000;

      Ping p = new Ping();
      PingReply r = p.Send(hostName, timeOut);

      if (r.Status == IPStatus.Success)
      {
        Console.WriteLine("Ping.Send() Success.");
      } else
      {
        Console.WriteLine("Ping.Send() Failed.");
      }

      //
      // 非同期での送信.
      //
      hostName = "www.google.com";
      object userToken = null;

      p.PingCompleted += (s, e) =>
      {

        if (e.Cancelled)
        {
          Console.WriteLine("Cancelled..");
          return;
        }

        if (e.Error != null)
        {
          Console.WriteLine(e.Error.ToString());
          return;
        }

        if (e.Reply.Status != IPStatus.Success)
        {
          Console.WriteLine("Ping.SendAsync() Failed");
          return;
        }

        Console.WriteLine("Ping.SendAsync() Success.");
      };

      p.SendAsync(hostName, timeOut, userToken);
      Thread.Sleep(3000);
    }
  }
  #endregion

  #region DataTableSamples-01
  /// <summary>
  /// DataTableクラスに関するサンプルです。
  /// </summary>
  public class DataTableSamples01 : IExecutable
  {
    public void Execute()
    {
      DataTable table = new DataTable();

      table.Columns.Add("Val", typeof(decimal));

      for (int i = 0; i < 10; i++)
      {
        table.LoadDataRow(new object[] { i * 0.1 }, true);
      }

      //
      // 列は[]付きでも無しでも構わないが、付けておいた方が無難.
      // 
      object result = table.Compute("SUM([Val])", "[Val] >= 0.5");
      Console.WriteLine("{0}:{1}", result, result.GetType().FullName);
    }
  }
  #endregion

  #region DataTableSamples-02
  /// <summary>
  /// DataTableクラスに関するサンプルです。
  /// </summary>
  public class DataTableSamples02 : IExecutable
  {
    public void Execute()
    {
      DataTable table = new DataTable();

      table.Columns.Add("Val", typeof(int));

      for (int i = 0; i < 10; i++)
      {
        table.LoadDataRow(new object[] { i }, true);
      }

      //
      // Selectメソッドで行を抽出.
      //
      DataRow[] selectedRows = table.Select("(Val % 2) = 0", "Val");
      selectedRows.ToList().ForEach((row) =>
      {
        Console.WriteLine(row["Val"]);
      });
    }
  }
  #endregion

  #region DataTableSamples-03
  /// <summary>
  /// DataTableクラスに関するサンプルです。
  /// </summary>
  public class DataTableSamples03 : IExecutable
  {
    public void Execute()
    {
      DataTable tableA = new DataTable();

      tableA.Columns.Add("Val", typeof(int));

      for (int i = 0; i < 10; i++)
      {
        tableA.LoadDataRow(new object[] { i }, true);
      }

      Console.WriteLine("[TableA]ColumnCount = {0}", tableA.Columns.Count);
      Console.WriteLine("[TableA]RowCount = {0}", tableA.Rows.Count);

      //
      // tableAのスキーマをtableBにコピー.
      // (データはコピーしない。)
      //
      DataTable tableB = tableA.Clone();
      Console.WriteLine("[TableB]ColumnCount = {0}", tableB.Columns.Count);
      Console.WriteLine("[TableB]RowCount = {0}", tableB.Rows.Count);

      //
      // tableAのスキーマとデータをtableCにコピー.
      //
      DataTable tableC = tableA.Copy();
      Console.WriteLine("[TableC]ColumnCount = {0}", tableC.Columns.Count);
      Console.WriteLine("[TableC]RowCount = {0}", tableC.Rows.Count);
    }
  }
  #endregion

  #region YieldSamples-01
  /// <summary>
  /// yieldキーワードに関するサンプルです。
  /// </summary>
  public class YieldSamples01 : IExecutable
  {
    //
    // 最もベーシックな列挙可能クラス.
    //
    class SampleCollection1
    {
      List<string> _list;

      public SampleCollection1()
      {

        _list = new List<string>{
           "Value1"
          ,"Value2"
          ,"Value3"
        };
      }

      public IEnumerator<string> GetEnumerator()
      {
        return _list.GetEnumerator();
      }
    }

    //
    // Yieldを利用したパターン１ (IEnumerator<T>を使用)
    //
    class SampleCollection2
    {
      public IEnumerator<string> GetEnumerator()
      {
        yield return "Value1";
        yield return "Value2";
        yield return "Value3";
      }
    }

    //
    // Yieldを利用したパターン２ (IEnumerable<T>を使用)
    //
    class SampleCollection3
    {
      public IEnumerable<string> InOrder
      {
        get
        {
          yield return "Value1";
          yield return "Value2";
          yield return "Value3";
        }
      }
    }

    //
    // Yieldを利用したパターン３ (複数のIEnumerable<T>を使用)
    //
    class SampleCollection4
    {
      List<string> _list;

      public SampleCollection4()
      {

        _list = new List<string>{
           "Value1"
          ,"Value2"
          ,"Value3"
        };
      }

      public IEnumerable<string> InOrder
      {
        get
        {
          for (int i = 0; i < _list.Count; i++)
          {
            yield return _list[i];
          }
        }
      }

      public IEnumerable<string> ReverseOrder
      {
        get
        {
          for (int i = (_list.Count - 1); i >= 0; i--)
          {
            yield return _list[i];
          }
        }
      }
    }

    //
    // yield breakを利用したパターン
    //
    class SampleCollection5
    {
      public IEnumerable<string> InOrderWithBreak
      {
        get
        {
          yield return "Value1";
          yield return "Value2";
          yield return "Value3";

          yield break;

          // warning CS0162: 到達できないコードが検出されました。
          //yield return "Value4";
        }
      }
    }

    //
    // 簡易パイプライン用の各メソッド.
    //
    IEnumerable<string> Range(int count, string prefix)
    {
      for (int i = 0; i < count; i++)
      {
        yield return string.Format("{0}-{1}", prefix, i);
      }
    }

    IEnumerable<string> Upper(IEnumerable<string> enumerables)
    {
      foreach (string val in enumerables)
      {
        yield return val.ToUpper();
      }
    }

    IEnumerable<string> AddCount(IEnumerable<string> enumerables)
    {

      int count = 1;
      foreach (string val in enumerables)
      {
        yield return string.Format("{0}-{1}", val, count++);
      }
    }

    IEnumerable<string> Filter(Predicate<string> predicate, IEnumerable<string> enumerables)
    {

      foreach (string val in enumerables)
      {

        if (predicate(val))
        {
          yield return val;
        }
      }
    }

    //
    // 処理確認用実行メソッド
    //
    public void Execute()
    {
      SampleCollection1 col1 = new SampleCollection1();
      foreach (string val in col1)
      {
        Console.WriteLine(val);
      }

      AddNewLine();

      SampleCollection2 col2 = new SampleCollection2();
      foreach (string val in col2)
      {
        Console.WriteLine(val);
      }

      AddNewLine();

      SampleCollection3 col3 = new SampleCollection3();
      foreach (string val in col3.InOrder)
      {
        Console.WriteLine(val);
      }

      AddNewLine();

      SampleCollection4 col4 = new SampleCollection4();
      foreach (string val in col4.InOrder)
      {
        Console.WriteLine(val);
      }

      foreach (string val in col4.ReverseOrder)
      {
        Console.WriteLine(val);
      }

      AddNewLine();

      SampleCollection5 col5 = new SampleCollection5();
      foreach (string val in col5.InOrderWithBreak)
      {
        Console.WriteLine(val);
      }

      AddNewLine();

      foreach (string val in Filter(Judge, AddCount(Upper(Range(10, "value")))))
      {
        Console.WriteLine(val);
      }
    }

    void AddNewLine()
    {
      Console.WriteLine(string.Empty);
    }

    bool Judge(string val)
    {

      string[] parts = val.Split('-');
      if (parts.Length != 3)
      {
        return false;
      }

      int number = int.Parse(parts[2]);
      if ((number % 2) == 0)
      {
        return true;
      }

      return false;
    }
  }
  #endregion

  #region ExcelConnectSamples-01
  /// <summary>
  /// ADO.NETを用いてExcelに接続するサンプルです。
  /// </summary>
  public class ExcelConnectSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // Excelに接続するには、OLEDBを利用する。
      // プロバイダー名は、「System.Data.OleDb」となる。
      //
      DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
      using (DbConnection conn = factory.CreateConnection())
      {
        //
        // Excel用の接続文字列を構築.
        //
        // Providerは、Microsoft.ACE.OLEDB.12.0を使用する事。
        // （JETドライバを利用するとxlsxを読み込む事が出来ない。）
        //
        // Extended Propertiesには、ISAMのバージョン(Excel 12.0)とHDRを指定する。
        // （2003までのxlsの場合はExcel 8.0でISAMバージョンを指定する。）
        // HDRは先頭行をヘッダ情報としてみなすか否かを指定する。
        // 先頭行をヘッダ情報としてみなす場合はYESを、そうでない場合はNOを設定。
        //
        // HDR=NOと指定した場合、カラム名はシステム側で自動的に割り振られる。
        // (F1, F2, F3.....となる)
        //
        DbConnectionStringBuilder builder = factory.CreateConnectionStringBuilder();

        builder["Provider"] = "Microsoft.ACE.OLEDB.12.0";
        builder["Data Source"] = @"C:\Users\gsf\Tmp\Sample.xlsx";
        builder["Extended Properties"] = "Excel 12.0;HDR=YES";

        conn.ConnectionString = builder.ToString();
        conn.Open();

        //
        // SELECT.
        //
        // 通常のSQLのように発行できる。その際シート指定は
        // [Sheet1$]のように行う。範囲を指定することも出来る。
        //
        using (DbCommand command = conn.CreateCommand())
        {
          command.CommandText = "SELECT * FROM [Sheet1$]";

          DataTable table = new DataTable();
          using (DbDataReader reader = command.ExecuteReader())
          {
            table.Load(reader);
          }

          foreach (DataRow row in table.Rows)
          {
            Console.WriteLine("{0},{1},{2}", row["ID"], row["NAME"], row["AGE"]);
          }
        }

        using (DbCommand command = conn.CreateCommand())
        {
          command.CommandText = "SELECT * FROM [Sheet1$A1:C7]";

          DataTable table = new DataTable();
          using (DbDataReader reader = command.ExecuteReader())
          {
            table.Load(reader);
          }

          foreach (DataRow row in table.Rows)
          {
            Console.WriteLine("{0},{1},{2}", row["ID"], row["NAME"], row["AGE"]);
          }
        }

        //
        // INSERT
        //
        // こちらも普通のSQLと同じように発行できる。
        // 尚、トランザクションは設定できるが効果は無い。
        // （ロールバックを行ってもデータは戻らない。）
        //
        // また、INSERT,UPDATEはエクセルを開いた状態でも
        // 行う事ができる。
        //
        // データの削除は行う事ができない。（制限）
        //
        using (DbCommand command = conn.CreateCommand())
        {
          string query = string.Empty;

          query += " INSERT INTO [Sheet1$] ";
          query += "   (ID, NAME, AGE) ";
          query += "   VALUES ";
          query += "   (7, 'gsf_zero7', 37) ";

          command.CommandText = query;
          command.ExecuteNonQuery();

          using (DbCommand command2 = conn.CreateCommand())
          {
            command2.CommandText = "SELECT * FROM [Sheet1$]";

            DataTable table = new DataTable();
            using (DbDataReader reader = command2.ExecuteReader())
            {
              table.Load(reader);
            }

            foreach (DataRow row in table.Rows)
            {
              Console.WriteLine("{0},{1},{2}", row["ID"], row["NAME"], row["AGE"]);
            }
          }
        }

        //
        // UPDATE
        //
        using (DbCommand command = conn.CreateCommand())
        {
          string query = string.Empty;

          query += " UPDATE [Sheet1$] ";
          query += " SET ";
          query += "    NAME = 'updated' ";
          query += "   ,AGE  = 99 ";
          query += " WHERE ";
          query += "   ID = 7 ";

          command.CommandText = query;
          command.ExecuteNonQuery();

          using (DbCommand command2 = conn.CreateCommand())
          {
            command2.CommandText = "SELECT * FROM [Sheet1$]";

            DataTable table = new DataTable();
            using (DbDataReader reader = command2.ExecuteReader())
            {
              table.Load(reader);
            }

            foreach (DataRow row in table.Rows)
            {
              Console.WriteLine("{0},{1},{2}", row["ID"], row["NAME"], row["AGE"]);
            }
          }
        }

        //
        // DELETE.
        // 削除は行えない。
        //
      }
    }
  }
  #endregion

  #region TextConnectSamples-01
  /// <summary>
  /// ADO.NETを利用してテキストファイルに接続するサンプルです。
  /// </summary>
  /// <remarks>
  /// CSVファイルに接続し、各クエリ文を発行します。
  /// </remarks>
  public class TextConnectSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // OleDbプロバイダを利用してテキストファイル(CSV)に接続する.
      //
      DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
      using (DbConnection conn = factory.CreateConnection())
      {
        //
        // テキストファイルに接続する為の接続文字列を構築.
        //
        // 基本的にExcelに接続する場合とほぼ同じ要領となる。
        // Extended Properties内のISAMドライバがExcel 12.0からtextになる。
        // また、フォーマット方式を指定する必要がある。
        //
        // Data Sourceに指定するのは、該当ファイルが存在するディレクトリを指定する。
        // 尚、該当ファイルの構造については別途schema.iniファイルを同じディレクトリに
        // 用意する必要がある。
        //
        DbConnectionStringBuilder builder = factory.CreateConnectionStringBuilder();

        builder["Provider"] = "Microsoft.ACE.OLEDB.12.0";
        builder["Data Source"] = @".";
        builder["Extended Properties"] = "text;HDR=YES;FMT=Delimited";

        conn.ConnectionString = builder.ToString();
        conn.Open();

        //
        // SELECT.
        // FROM句の中に読み込む対象のファイル名を指定する。
        // データが取得される際にschema.iniファイルが参照され、列定義が行われる。
        //
        using (DbCommand command = conn.CreateCommand())
        {
          command.CommandText = "SELECT * FROM [Persons.txt]";

          DataTable table = new DataTable();
          using (DbDataReader reader = command.ExecuteReader())
          {
            table.Load(reader);
          }

          foreach (DataRow row in table.Rows)
          {
            Console.WriteLine("{0},{1},{2}", row["ID"], row["NAME"], row["AGE"]);
          }
        }

        //
        // INSERT.
        // Accessの場合と同じくテキストファイルは追加・参照しか出来ない。
        // （つまり、更新・削除は出来ない）
        //
        using (DbCommand command = conn.CreateCommand())
        {
          string query = string.Empty;

          query += " INSERT INTO [Persons.txt] ";
          query += "   (ID, NAME, AGE) ";
          query += "   VALUES ";
          query += "   (7, 'gsf_zero7', 37) ";

          command.CommandText = query;
          command.ExecuteNonQuery();

          using (DbCommand command2 = conn.CreateCommand())
          {
            command2.CommandText = "SELECT * FROM [Persons.txt]";

            DataTable table = new DataTable();
            using (DbDataReader reader = command2.ExecuteReader())
            {
              table.Load(reader);
            }

            foreach (DataRow row in table.Rows)
            {
              Console.WriteLine("{0},{1},{2}", row["ID"], row["NAME"], row["AGE"]);
            }
          }
        }

        //
        // UPDATE
        // この ISAM では、リンク テーブル内のデータを更新することはできません。
        //

        //
        // DELETE.
        // この ISAM では、リンク テーブル内のデータを削除することはできません。
        //
      }
    }
  }
  #endregion

  #region LinqSamples-01
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples01 : IExecutable
  {
    class Person
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public AddressInfo Address
      {
        get;
        set;
      }
    }

    class AddressInfo
    {
      public string PostCode
      {
        get;
        set;
      }
      public string Prefecture
      {
        get;
        set;
      }
      public string Municipality
      {
        get;
        set;
      }
      public string HouseNumber
      {
        get;
        set;
      }
      public string Tel
      {
        get;
        set;
      }
    }

    IEnumerable<Person> CreateSampleData()
    {
      return new Person[]{
         new Person{ 
             Id="00001"
            ,Name="gsf_zero1"
            ,Address=new AddressInfo{
                     PostCode="999-8888"
                    ,Prefecture="東京都"
                    ,Municipality="どこか１"
                    ,HouseNumber="番地１"
                    ,Tel="090-xxxx-xxxx"
            }
         }
        ,new Person{ 
             Id="00002"
            ,Name="gsf_zero2"
            ,Address=new AddressInfo{
                     PostCode="888-7777"
                    ,Prefecture="京都府"
                    ,Municipality="どこか２"
                    ,HouseNumber="番地２"
                    ,Tel="080-xxxx-xxxx"
            }
        }
        ,new Person{ 
             Id="00003"
            ,Name="gsf_zero3"
            ,Address=new AddressInfo{
                     PostCode="777-6666"
                    ,Prefecture="北海道"
                    ,Municipality="どこか３"
                    ,HouseNumber="番地３"
                    ,Tel="070-xxxx-xxxx"
            }
        }
        ,new Person{ 
             Id="0000x"
            ,Name="gsf_zero3"
            ,Address=new AddressInfo{
                     PostCode="777-6666"
                    ,Prefecture="北海道"
                    ,Municipality="どこか３"
                    ,HouseNumber="番地３"
                    ,Tel="070-xxxx-xxxx"
            }
        }
      };
    }

    public void Execute()
    {
      IEnumerable<Person> persons = CreateSampleData();

      var query = from person in persons
            where int.Parse(person.Id) >= 2
            select person;

      try
      {

        foreach (Person person in query)
        {
          Console.WriteLine("{0},{1}", person.Id, person.Name);
        }

      } 
      catch (Exception ex)
      {
        Console.WriteLine("クエリ実行時にエラーが発生: {0}", ex.Message);
      }
    }

  }
  #endregion

  #region LinqSamples-02
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples02 : IExecutable
  {

    class Person
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public AddressInfo Address
      {
        get;
        set;
      }
    }

    class AddressInfo
    {
      public string PostCode
      {
        get;
        set;
      }
      public string Prefecture
      {
        get;
        set;
      }
      public string Municipality
      {
        get;
        set;
      }
      public string HouseNumber
      {
        get;
        set;
      }
      public string[] Tel
      {
        get;
        set;
      }
      public string[] Frends
      {
        get;
        set;
      }
    }

    IEnumerable<Person> CreateSampleData()
    {
      return new Person[]{
         new Person{ 
             Id="00001"
            ,Name="gsf_zero1"
            ,Address=new AddressInfo{
                     PostCode="999-8888"
                    ,Prefecture="東京都"
                    ,Municipality="どこか１"
                    ,HouseNumber="番地１"
                    ,Tel=new []{"090-xxxx-xxxx"}
                    ,Frends=new string[]{}
            }
         }
        ,new Person{ 
             Id="00002"
            ,Name="gsf_zero2"
            ,Address=new AddressInfo{
                     PostCode="888-7777"
                    ,Prefecture="京都府"
                    ,Municipality="どこか２"
                    ,HouseNumber="番地２"
                    ,Tel=new []{"080-xxxx-xxxx"}
                    ,Frends=new []{"00001"}
            }
        }
        ,new Person{ 
             Id="00003"
            ,Name="gsf_zero3"
            ,Address=new AddressInfo{
                     PostCode="777-6666"
                    ,Prefecture="北海道"
                    ,Municipality="どこか３"
                    ,HouseNumber="番地３"
                    ,Tel=new []{"070-xxxx-xxxx"}
                    ,Frends=new []{"00001", "00002"}
            }
        }
        ,new Person{ 
             Id="00004"
            ,Name="gsf_zero4"
            ,Address=new AddressInfo{
                     PostCode="777-6666"
                    ,Prefecture="北海道"
                    ,Municipality="どこか４"
                    ,HouseNumber="番地４"
                    ,Tel=new []{"060-xxxx-xxxx", "111-111-1111", "222-222-2222"}
                    ,Frends=new []{"00001", "00003"}
            }
        }
      };
    }

    public void Execute()
    {
      IEnumerable<Person> persons = CreateSampleData();

      //
      // 一つのfrom
      //    尚、以下のように何もせずに元のままの結果を返すクエリを
      //    縮退クエリと言うらしいです。
      //
      var query1 = from person in persons
             select person;

      foreach (var person in query1)
      {
        Console.WriteLine("Id={0}, Name={1}", person.Id, person.Name);
      }

      //
      // 複数のfrom.
      //
      var query2 = from person in persons
             from tel in person.Address.Tel
             select new
             {
               Id = person.Id,
               Tel = tel
             };

      foreach (var data in query2)
      {
        Console.WriteLine("Id={0}, PostCode={1}", data.Id, data.Tel);
      }

      // 
      // IEnumerableを実装している場合は明示的な型指定が必要.
      //
      ArrayList aryList = new ArrayList(new string[] { "hoge", "moge", "fuga" });

      var query3 = from string element in aryList
             select element;

      foreach (var element in query3)
      {
        Console.WriteLine(element);
      }
    }
  }
  #endregion

  #region LinqSamples-03
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples03 : IExecutable
  {
    class Person
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public AddressInfo Address
      {
        get;
        set;
      }
    }

    class AddressInfo
    {
      public string PostCode
      {
        get;
        set;
      }
      public string Prefecture
      {
        get;
        set;
      }
      public string Municipality
      {
        get;
        set;
      }
      public string HouseNumber
      {
        get;
        set;
      }
      public string[] Tel
      {
        get;
        set;
      }
      public string[] Frends
      {
        get;
        set;
      }
    }

    IEnumerable<Person> CreateSampleData()
    {
      return new Person[]{
         new Person{ 
             Id="00001"
            ,Name="gsf_zero1"
            ,Address=new AddressInfo{
                     PostCode="999-8888"
                    ,Prefecture="東京都"
                    ,Municipality="どこか１"
                    ,HouseNumber="番地１"
                    ,Tel=new []{"090-xxxx-xxxx"}
                    ,Frends=new string[]{}
            }
         }
        ,new Person{ 
             Id="00002"
            ,Name="gsf_zero2"
            ,Address=new AddressInfo{
                     PostCode="888-7777"
                    ,Prefecture="京都府"
                    ,Municipality="どこか２"
                    ,HouseNumber="番地２"
                    ,Tel=new []{"080-xxxx-xxxx"}
                    ,Frends=new []{"00001"}
            }
        }
        ,new Person{ 
             Id="00003"
            ,Name="gsf_zero3"
            ,Address=new AddressInfo{
                     PostCode="777-6666"
                    ,Prefecture="北海道"
                    ,Municipality="どこか３"
                    ,HouseNumber="番地３"
                    ,Tel=new []{"070-xxxx-xxxx"}
                    ,Frends=new []{"00001", "00002"}
            }
        }
        ,new Person{ 
             Id="00004"
            ,Name="gsf_zero4"
            ,Address=new AddressInfo{
                     PostCode="777-6666"
                    ,Prefecture="北海道"
                    ,Municipality="どこか４"
                    ,HouseNumber="番地４"
                    ,Tel=new []{"060-xxxx-xxxx", "111-111-1111", "222-222-2222"}
                    ,Frends=new []{"00001", "00003"}
            }
        }
      };
    }

    public void Execute()
    {
      IEnumerable<Person> persons = CreateSampleData();

      //
      // 抽出したオブジェクトをそのまま返す
      //
      var query1 = from person in persons
             select person;

      foreach (var person in query1)
      {
        Console.WriteLine("Id={0}, Name={1}", person.Id, person.Name);
      }

      //
      // 抽出結果から新たな型を作成する
      // 
      var query2 = from person in persons
             select new
             {
               Id = person.Id,
               Name = person.Name
             };

      foreach (var person in query2)
      {
        Console.WriteLine("Id={0}, Name={1}", person.Id, person.Name);
      }
    }
  }
  #endregion

  #region LinqSamples-04
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples04 : IExecutable
  {
    class Person
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public AddressInfo Address
      {
        get;
        set;
      }
    }

    class AddressInfo
    {
      public string PostCode
      {
        get;
        set;
      }
      public string Prefecture
      {
        get;
        set;
      }
      public string Municipality
      {
        get;
        set;
      }
      public string HouseNumber
      {
        get;
        set;
      }
      public string[] Tel
      {
        get;
        set;
      }
      public string[] Frends
      {
        get;
        set;
      }
    }

    IEnumerable<Person> CreateSampleData()
    {
      return new Person[]{
         new Person{ 
             Id="00001"
            ,Name="gsf_zero1"
            ,Address=new AddressInfo{
                     PostCode="999-8888"
                    ,Prefecture="東京都"
                    ,Municipality="どこか１"
                    ,HouseNumber="番地１"
                    ,Tel=new []{"090-xxxx-xxxx"}
                    ,Frends=new string[]{}
            }
         }
        ,new Person{ 
             Id="00002"
            ,Name="gsf_zero2"
            ,Address=new AddressInfo{
                     PostCode="888-7777"
                    ,Prefecture="京都府"
                    ,Municipality="どこか２"
                    ,HouseNumber="番地２"
                    ,Tel=new []{"080-xxxx-xxxx"}
                    ,Frends=new []{"00001"}
            }
        }
        ,new Person{ 
             Id="00003"
            ,Name="gsf_zero3"
            ,Address=new AddressInfo{
                     PostCode="777-6666"
                    ,Prefecture="北海道"
                    ,Municipality="どこか３"
                    ,HouseNumber="番地３"
                    ,Tel=new []{"070-xxxx-xxxx"}
                    ,Frends=new []{"00001", "00002"}
            }
        }
        ,new Person{ 
             Id="00004"
            ,Name="gsf_zero4"
            ,Address=new AddressInfo{
                     PostCode="777-6666"
                    ,Prefecture="北海道"
                    ,Municipality="どこか４"
                    ,HouseNumber="番地４"
                    ,Tel=new []{"060-xxxx-xxxx", "111-111-1111", "222-222-2222"}
                    ,Frends=new []{"00001", "00003"}
            }
        }
      };
    }

    public void Execute()
    {
      IEnumerable<Person> persons = CreateSampleData();

      //
      // 普通に絞り込み.
      //
      var query1 = from person in persons
             where person.Name.EndsWith("4")
             select person;

      foreach (var person in query1)
      {
        Console.WriteLine("QUERY-1: Id={0}, Name={1}", person.Id, person.Name);
      }

      //
      // 複数の条件.
      //
      var query2 = from person in persons
             where person.Name.EndsWith("4") || person.Address.Prefecture == "京都府"
             select person;

      foreach (var person in query2)
      {
        Console.WriteLine("QUERY-2: Id={0}, Name={1}", person.Id, person.Name);
      }

      //
      // 複数のwhereを指定
      //
      var query3 = from person in persons
             where person.Name.Contains("gsf")
             where person.Address.Prefecture == "京都府"
             select person;

      foreach (var person in query3)
      {
        Console.WriteLine("QUERY-3: Id={0}, Name={1}", person.Id, person.Name);
      }
    }
  }
  #endregion

  #region LinqSamples-05
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples05 : IExecutable
  {
    enum Country
    {
      Japan,
      America,
      China
    }

    class Person
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public AddressInfo Address
      {
        get;
        set;
      }
      public Country Country
      {
        get;
        set;
      }
    }

    class AddressInfo
    {
      public string PostCode
      {
        get;
        set;
      }
      public string Prefecture
      {
        get;
        set;
      }
      public string Municipality
      {
        get;
        set;
      }
      public string HouseNumber
      {
        get;
        set;
      }
      public string[] Tel
      {
        get;
        set;
      }
      public string[] Frends
      {
        get;
        set;
      }
    }

    IEnumerable<Person> CreateSampleData()
    {
      return new Person[]{
         new Person{ 
             Id="00001"
            ,Name="gsf_zero1"
            ,Address=new AddressInfo{
                     PostCode="999-8888"
                    ,Prefecture="東京都"
                    ,Municipality="どこか１"
                    ,HouseNumber="番地１"
                    ,Tel=new []{"090-xxxx-xxxx"}
                    ,Frends=new string[]{}
            }
            ,Country=Country.Japan
         }
        ,new Person{ 
             Id="00002"
            ,Name="gsf_zero2"
            ,Address=new AddressInfo{
                     PostCode="888-7777"
                    ,Prefecture="京都府"
                    ,Municipality="どこか２"
                    ,HouseNumber="番地２"
                    ,Tel=new []{"080-xxxx-xxxx"}
                    ,Frends=new []{"00001"}
            }
            ,Country=Country.Japan
        }
        ,new Person{ 
             Id="00003"
            ,Name="gsf_zero3"
            ,Address=new AddressInfo{
                     PostCode="777-6666"
                    ,Prefecture="北海道"
                    ,Municipality="どこか３"
                    ,HouseNumber="番地３"
                    ,Tel=new []{"070-xxxx-xxxx"}
                    ,Frends=new []{"00001", "00002"}
            }
            ,Country=Country.America
        }
        ,new Person{ 
             Id="00004"
            ,Name="gsf_zero4"
            ,Address=new AddressInfo{
                     PostCode="777-6666"
                    ,Prefecture="北海道"
                    ,Municipality="どこか４"
                    ,HouseNumber="番地４"
                    ,Tel=new []{"060-xxxx-xxxx", "111-111-1111", "222-222-2222"}
                    ,Frends=new []{"00001", "00003"}
            }
            ,Country=Country.America
        }
      };
    }

    public void Execute()
    {

      IEnumerable<Person> persons = CreateSampleData();

      //
      // into句を使用しない標準的なgroupの利用
      // (Person.Countryでグルーピング)
      //
      var query1 = from person in persons
             group person by person.Country;

      //
      // 結果は、キーと該当するグループの状態で取得できる.
      //
      foreach (var groupedPerson in query1)
      {
        // キー
        Console.WriteLine("Country={0}", groupedPerson.Key);

        // グループ
        // グループを取得するには、もう一度ループする必要がある。
        foreach (var person in groupedPerson)
        {
          Console.WriteLine("\tId={0}, Name={1}", person.Id, person.Name);
        }
      }

      //
      // 同じ結果を、var無しで表現.
      //
      // groupキーワードの結果は以下の型となる。
      //    IGrouping<TKey, TElement>
      // IGroupingインターフェースは、IEnumerable<TElement>を
      // 継承しているので、ループさせるとTElementが順次取得できるようになっている。
      //
      foreach (IGrouping<Country, Person> groupedPerson in query1)
      {
        Console.WriteLine("Country={0}", groupedPerson.Key);

        foreach (Person person in groupedPerson)
        {
          Console.WriteLine("\tId={0}, Name={1}", person.Id, person.Name);
        }
      }
    }
  }
  #endregion

  #region LinqSamples-06
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples06 : IExecutable
  {
    enum Country
    {
       Japan
      ,America
      ,China
    }

    class Person
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public AddressInfo Address
      {
        get;
        set;
      }
      public Country Country
      {
        get;
        set;
      }
    }

    class AddressInfo
    {
      public string PostCode
      {
        get;
        set;
      }
      public string Prefecture
      {
        get;
        set;
      }
      public string Municipality
      {
        get;
        set;
      }
      public string HouseNumber
      {
        get;
        set;
      }
      public string[] Tel
      {
        get;
        set;
      }
      public string[] Frends
      {
        get;
        set;
      }
    }


    IEnumerable<Person> CreateSampleData()
    {
      return new Person[]{
         new Person{ 
             Id="00001"
            ,Name="gsf_zero1"
            ,Address=new AddressInfo{
                     PostCode="999-8888"
                    ,Prefecture="東京都"
                    ,Municipality="どこか１"
                    ,HouseNumber="番地１"
                    ,Tel=new []{"090-xxxx-xxxx"}
                    ,Frends=new string[]{}
            }
            ,Country=Country.Japan
         }
        ,new Person{ 
             Id="00002"
            ,Name="gsf_zero2"
            ,Address=new AddressInfo{
                     PostCode="888-7777"
                    ,Prefecture="京都府"
                    ,Municipality="どこか２"
                    ,HouseNumber="番地２"
                    ,Tel=new []{"080-xxxx-xxxx"}
                    ,Frends=new []{"00001"}
            }
            ,Country=Country.Japan
        }
        ,new Person{ 
             Id="00003"
            ,Name="gsf_zero3"
            ,Address=new AddressInfo{
                     PostCode="777-6666"
                    ,Prefecture="北海道"
                    ,Municipality="どこか３"
                    ,HouseNumber="番地３"
                    ,Tel=new []{"070-xxxx-xxxx"}
                    ,Frends=new []{"00001", "00002"}
            }
            ,Country=Country.America
        }
        ,new Person{ 
             Id="00004"
            ,Name="gsf_zero4"
            ,Address=new AddressInfo{
                     PostCode="777-6666"
                    ,Prefecture="北海道"
                    ,Municipality="どこか４"
                    ,HouseNumber="番地４"
                    ,Tel=new []{"060-xxxx-xxxx", "111-111-1111", "222-222-2222"}
                    ,Frends=new []{"00001", "00003"}
            }
            ,Country=Country.America
        }
      };
    }

    public void Execute()
    {
      IEnumerable<Person> persons = CreateSampleData();

      // 
      //  昇順.
      //  (昇順の場合のascendingは付けても付けなくても良い)
      //
      var query1 = from person in persons
             orderby person.Id.ToInt() ascending
             select person;

      Console.WriteLine("============================================");
      foreach (Person person in query1)
      {
        Console.WriteLine("Id={0}, Name={1}", person.Id, person.Name);
      }

      //
      // 降順.
      //
      var query2 = from person in persons
             orderby person.Id.ToInt() descending
             select person;

      Console.WriteLine("============================================");
      foreach (Person person in query2)
      {
        Console.WriteLine("Id={0}, Name={1}", person.Id, person.Name);
      }

      //
      // 複数の条件でソート.
      //
      var query3 = from person in persons
             orderby person.Address.PostCode, person.Id.ToInt()
             select person;

      Console.WriteLine("============================================");
      foreach (Person person in query3)
      {
        Console.WriteLine("Id={0}, Name={1}", person.Id, person.Name);
      }

      //
      // 複数のorderby.
      // (query3の場合と結果が異なる事に注意)
      //
      // query3の場合は一度で2つの条件にてソート処理が行われるが
      // query4は、2回に分けてソート処理が行われる。
      //
      // つまり、orderby person.Address.PostCodeで一旦ソートが
      // 行われるが、その後orderby person.IdによってID順にソート
      // され直されてしまう。
      //
      var query4 = from person in persons
             orderby person.Address.PostCode
             orderby person.Id.ToInt()
             select person;

      Console.WriteLine("============================================");
      foreach (Person person in query4)
      {
        Console.WriteLine("Id={0}, Name={1}", person.Id, person.Name);
      }
    }
  }
  #endregion

  #region LinqSamples-07
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples07 : IExecutable
  {
    public class Person
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public int Age
      {
        get;
        set;
      }
      public DateTime Birthday
      {
        get;
        set;
      }
    }

    public class Team
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public IEnumerable<string> Members
      {
        get;
        set;
      }
    }

    public enum ProjectState
    {
       NotStarted
      ,Processing
      ,Done
    }

    public class Project
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public IEnumerable<string> Members
      {
        get;
        set;
      }
      public DateTime From
      {
        get;
        set;
      }
      public DateTime? To
      {
        get;
        set;
      }
      public ProjectState State
      {
        get;
        set;
      }
    }

    // メンバー
    IEnumerable<Person> persons =
                new[]{
                   new Person
                   {
                      Id   = "001"
                     ,Name = "gsf_zero1"
                     ,Age  = 30
                     ,Birthday = new DateTime(1979, 11, 18)
                   }
                  ,new Person
                  {
                      Id   = "002"
                     ,Name = "gsf_zero2"
                     ,Age  = 20
                     ,Birthday = new DateTime(1989, 11, 18)
                   }
                  ,new Person
                  {
                      Id   = "003"
                     ,Name = "gsf_zero3"
                     ,Age  = 18
                     ,Birthday = new DateTime(1991, 11, 18)
                  }
                  ,new Person
                  {
                      Id   = "004"
                     ,Name = "gsf_zero4"
                     ,Age  = 40
                     ,Birthday = new DateTime(1969, 11, 18)
                  }
                  ,new Person
                  {
                      Id   = "005"
                     ,Name = "gsf_zero5"
                     ,Age  = 44
                     ,Birthday = new DateTime(1965, 11, 18)
                  }
                };

    // チーム
    IEnumerable<Team> teams =
                new[]{
                   new Team
                   {
                      Id    = "001"
                     ,Name  = "team_1"
                     ,Members = new []{ "001", "004" }
                   }
                  ,new Team
                  {
                      Id    = "002"
                     ,Name  = "team_2"
                     ,Members = new []{ "002", "003" }
                   }
                };

    // プロジェクト
    IEnumerable<Project> projects =
                new[]{
                   new Project
                   {
                      Id    = "001"
                     ,Name  = "project_1"
                     ,Members = new []{ "001", "002" }
                     ,From  = new DateTime(2009, 8, 1)
                     ,To    = new DateTime(2009, 10, 15)
                     ,State   = ProjectState.Done
                   }
                  ,new Project
                  {
                      Id    = "002"
                     ,Name  = "project_2"
                     ,Members = new []{ "003", "004" }
                     ,From  = new DateTime(2009, 9, 1)
                     ,To    = new DateTime(2009, 11, 25)
                     ,State   = ProjectState.Done
                   }
                  ,new Project
                  {
                      Id    = "003"
                     ,Name  = "project_3"
                     ,Members = new []{ "001" }
                     ,From  = new DateTime(2007, 1, 10)
                     ,To    = null
                     ,State   = ProjectState.Processing
                   }
                  ,new Project
                  {
                      Id    = "004"
                     ,Name  = "project_4"
                     ,Members = new []{ "001", "002", "003", "004" }
                     ,From  = new DateTime(2010, 1, 5)
                     ,To    = null
                     ,State   = ProjectState.NotStarted
                   }
                };

    public void Execute()
    {
      //
      // intoのサンプル
      // intoはselect, group, joinの結果を一時的に保持できる。
      //
      // 下記のサンプルの場合、groupの結果をintoで保持してから
      // ソート処理を行い、結果を構築している。（追加のクエリ処理）
      //
      // 各年齢層のカウントを算出.
      //
      var query1 = from person in persons
             group person by (Math.Truncate((person.Age * 0.1)) * 10) into personByAge
             orderby personByAge.Key ascending
             select new
             {
                AgeLevel = personByAge.Key
               ,Count = personByAge.Count()
             };

      foreach (var item in query1)
      {
        //
        // 匿名型は、内部で自動的にToStringが作成されているので
        // そのまま、ToStringを呼べばプロパティの値が表示される。
        //
        Console.WriteLine(item);
      }

      //
      // 上記の場合は、最後に匿名型を構築して
      // クエリ結果としているが、当然そのままIGroupingの結果で
      // 返却しても同じ結果が導ける.
      //
      var query2 = from person in persons
             group person by (Math.Truncate((person.Age * 0.1)) * 10) into personByAge
             orderby personByAge.Key ascending
             select personByAge;

      foreach (var item in query2)
      {
        Console.WriteLine("Age={0}, Count={1}", item.Key, item.Count());
      }
    }

  }
  #endregion

  #region LinqSamples-08
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples08 : IExecutable
  {
    public class Person
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public int Age
      {
        get;
        set;
      }
      public DateTime Birthday
      {
        get;
        set;
      }
    }

    public class Team
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public IEnumerable<string> Members
      {
        get;
        set;
      }
    }

    public enum ProjectState
    {
      NotStarted
       ,
      Processing
        ,
      Done
    }

    public class Project
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public IEnumerable<string> Members
      {
        get;
        set;
      }
      public DateTime From
      {
        get;
        set;
      }
      public DateTime? To
      {
        get;
        set;
      }
      public ProjectState State
      {
        get;
        set;
      }
    }

    // メンバー
    IEnumerable<Person> persons =
                new[]{
                   new Person
                   {
                      Id   = "001"
                     ,Name = "gsf_zero1"
                     ,Age  = 30
                     ,Birthday = new DateTime(1979, 11, 18)
                   }
                  ,new Person
                  {
                      Id   = "002"
                     ,Name = "gsf_zero2"
                     ,Age  = 20
                     ,Birthday = new DateTime(1989, 11, 18)
                   }
                  ,new Person
                  {
                      Id   = "003"
                     ,Name = "gsf_zero3"
                     ,Age  = 18
                     ,Birthday = new DateTime(1991, 11, 18)
                  }
                  ,new Person
                  {
                      Id   = "004"
                     ,Name = "gsf_zero4"
                     ,Age  = 40
                     ,Birthday = new DateTime(1969, 11, 18)
                  }
                  ,new Person
                  {
                      Id   = "005"
                     ,Name = "gsf_zero5"
                     ,Age  = 44
                     ,Birthday = new DateTime(1965, 11, 18)
                  }
                };

    // チーム
    IEnumerable<Team> teams =
                new[]{
                   new Team
                   {
                      Id    = "001"
                     ,Name  = "team_1"
                     ,Members = new []{ "001", "004" }
                   }
                  ,new Team
                  {
                      Id    = "002"
                     ,Name  = "team_2"
                     ,Members = new []{ "002", "003" }
                   }
                };

    // プロジェクト
    IEnumerable<Project> projects =
                new[]{
                   new Project
                   {
                      Id    = "001"
                     ,Name  = "project_1"
                     ,Members = new []{ "001", "002" }
                     ,From  = new DateTime(2009, 8, 1)
                     ,To    = new DateTime(2009, 10, 15)
                     ,State   = ProjectState.Done
                   }
                  ,new Project
                  {
                      Id    = "002"
                     ,Name  = "project_2"
                     ,Members = new []{ "003", "004" }
                     ,From  = new DateTime(2009, 9, 1)
                     ,To    = new DateTime(2009, 11, 25)
                     ,State   = ProjectState.Done
                   }
                  ,new Project
                  {
                      Id    = "003"
                     ,Name  = "project_3"
                     ,Members = new []{ "001" }
                     ,From  = new DateTime(2007, 1, 10)
                     ,To    = null
                     ,State   = ProjectState.Processing
                   }
                  ,new Project
                  {
                      Id    = "004"
                     ,Name  = "project_4"
                     ,Members = new []{ "001", "002", "003", "004" }
                     ,From  = new DateTime(2010, 1, 5)
                     ,To    = null
                     ,State   = ProjectState.NotStarted
                   }
                };

    public void Execute()
    {
      //
      // letのサンプル
      // letは一時的な変数をクエリ内で作成する際に利用する。
      //
      // 各年齢層のカウントを算出.
      //
      var query1 = from person in persons
             let ageLevel = (Math.Truncate((person.Age * 0.1)) * 10)
             group person by ageLevel into personByAge
             orderby personByAge.Key ascending
             select new
             {
                AgeLevel = personByAge.Key
               ,Count = personByAge.Count()
             };

      foreach (var item in query1)
      {
        Console.WriteLine(item);
      }
    }
  }
  #endregion

  #region LinqSamples-09
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples09 : IExecutable
  {
    public class Person
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public int Age
      {
        get;
        set;
      }
      public DateTime Birthday
      {
        get;
        set;
      }
    }

    public class Team
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public IEnumerable<string> Members
      {
        get;
        set;
      }
    }

    public enum ProjectState
    {
       NotStarted
      ,Processing
      ,Done
    }

    public class Project
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public IEnumerable<string> Members
      {
        get;
        set;
      }
      public DateTime From
      {
        get;
        set;
      }
      public DateTime? To
      {
        get;
        set;
      }
      public ProjectState State
      {
        get;
        set;
      }
    }

    // メンバー
    IEnumerable<Person> persons =
                new[]{
                   new Person
                   {
                      Id   = "001"
                     ,Name = "gsf_zero1"
                     ,Age  = 30
                     ,Birthday = new DateTime(1979, 11, 18)
                   }
                  ,new Person
                  {
                      Id   = "002"
                     ,Name = "gsf_zero2"
                     ,Age  = 20
                     ,Birthday = new DateTime(1989, 11, 18)
                   }
                  ,new Person
                  {
                      Id   = "003"
                     ,Name = "gsf_zero3"
                     ,Age  = 18
                     ,Birthday = new DateTime(1991, 11, 18)
                  }
                  ,new Person
                  {
                      Id   = "004"
                     ,Name = "gsf_zero4"
                     ,Age  = 40
                     ,Birthday = new DateTime(1969, 11, 18)
                  }
                  ,new Person
                  {
                      Id   = "005"
                     ,Name = "gsf_zero5"
                     ,Age  = 44
                     ,Birthday = new DateTime(1965, 11, 18)
                  }
                };

    // チーム
    IEnumerable<Team> teams =
                new[]{
                   new Team
                   {
                      Id    = "001"
                     ,Name  = "team_1"
                     ,Members = new []{ "001", "004" }
                   }
                  ,new Team
                  {
                      Id    = "002"
                     ,Name  = "team_2"
                     ,Members = new []{ "002", "003" }
                   }
                };

    // プロジェクト
    IEnumerable<Project> projects =
                new[]{
                   new Project
                   {
                      Id    = "001"
                     ,Name  = "project_1"
                     ,Members = new []{ "001", "002" }
                     ,From  = new DateTime(2009, 8, 1)
                     ,To    = new DateTime(2009, 10, 15)
                     ,State   = ProjectState.Done
                   }
                  ,new Project
                  {
                      Id    = "002"
                     ,Name  = "project_2"
                     ,Members = new []{ "003", "004" }
                     ,From  = new DateTime(2009, 9, 1)
                     ,To    = new DateTime(2009, 11, 25)
                     ,State   = ProjectState.Done
                   }
                  ,new Project
                  {
                      Id    = "003"
                     ,Name  = "project_3"
                     ,Members = new []{ "001" }
                     ,From  = new DateTime(2007, 1, 10)
                     ,To    = null
                     ,State   = ProjectState.Processing
                   }
                  ,new Project
                  {
                      Id    = "004"
                     ,Name  = "project_4"
                     ,Members = new []{ "001", "002", "003", "004" }
                     ,From  = new DateTime(2010, 1, 5)
                     ,To    = null
                     ,State   = ProjectState.NotStarted
                   }
                };

    public void Execute()
    {
      //
      // joinを用いた内部結合(INNER JOIN)のサンプル.
      //
      // joinキーワードの構文は以下の通り.
      //    join 内部側の変数 in 内部側のシーケンス on 外部側のキー equals 内部側のキー
      //
      // Linqのjoinでは等結合（つまり等値の場合の結合）のみが行える。
      //
      //====================================================================================
      // Teamから辿るパターン
      var query1 = from team in teams
             from personId in team.Members
             join person in persons on personId equals person.Id
             orderby team.Id ascending, person.Id ascending
             select new
             {
               Team = team.Name
              ,Person = person.Name
             };

      Console.WriteLine("=========================================");
      foreach (var item in query1)
      {
        Console.WriteLine(item);
      }

      //
      // Personから辿るパターン.
      // joinキーワードを利用せずに手動で結合処理.
      //
      // この場合、上記と同じように
      //
      // from person   in persons
      // from team   in teams
      // join personId in team.Members on person.Id equals personId
      //
      // とすると、コンテストが違うというエラーとなる。
      // これは、join内部で新たなコンテキストが発生するからである。
      // query1では、内部側のシーケンスをjoin内部だけで利用していたので
      // 問題ない.
      //
      var query2 = from person in persons
             from team in teams
             where team.Members.Contains(person.Id)
             orderby team.Id ascending, person.Id ascending
             select new
             {
                Team = team.Name
               ,Person = person.Name
             };

      Console.WriteLine("=========================================");
      foreach (var item in query2)
      {
        Console.WriteLine(item);
      }

      //
      // query2をjoinで収めるパターン.
      //
      // 上記の例をjoinに収めるためには、join内で目的のシーケンスを
      // 構築する必要がある。そのため、以下の例では目的のシーケンスを作成するために
      // join内にて、さらにサブクエリを作成している。
      //
      var query3 = from person in persons
             join team in (
               from team in teams
               from member in team.Members
               select new
               {
                  Id = team.Id
                 ,Name = team.Name
                 ,PersonId = member
               }
             ) on person.Id equals team.PersonId
             orderby team.Id ascending, person.Id ascending
             select new
             {
                Team = team.Name
               ,Person = person.Name
             };

      Console.WriteLine("=========================================");
      foreach (var item in query3)
      {
        Console.WriteLine(item);
      }

      //
      // CROSS JOIN.
      //
      // 普通にfromを並べればCROSS JOIN状態となる。
      //
      var query4 = from person in persons
             from team in teams
             orderby team.Id ascending, person.Id ascending
             select new
             {
                Team = team.Name
               ,Person = person.Name
             };

      Console.WriteLine("=========================================");
      foreach (var item in query4)
      {
        Console.WriteLine(item);
      }
    }
  }
  #endregion

  #region LinqSamples-10
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples10 : IExecutable
  {
    public class Person
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public int Age
      {
        get;
        set;
      }
      public DateTime Birthday
      {
        get;
        set;
      }
    }

    public class Team
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public IEnumerable<string> Members
      {
        get;
        set;
      }
    }

    public enum ProjectState
    {
       NotStarted
      ,Processing
      ,Done
    }

    public class Project
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public IEnumerable<string> Members
      {
        get;
        set;
      }
      public DateTime From
      {
        get;
        set;
      }
      public DateTime? To
      {
        get;
        set;
      }
      public ProjectState State
      {
        get;
        set;
      }
    }

    // メンバー
    IEnumerable<Person> persons =
                new[]{
                   new Person
                   {
                      Id   = "001"
                     ,Name = "gsf_zero1"
                     ,Age  = 30
                     ,Birthday = new DateTime(1979, 11, 18)
                   }
                  ,new Person
                  {
                      Id   = "002"
                     ,Name = "gsf_zero2"
                     ,Age  = 20
                     ,Birthday = new DateTime(1989, 11, 18)
                   }
                  ,new Person
                  {
                      Id   = "003"
                     ,Name = "gsf_zero3"
                     ,Age  = 18
                     ,Birthday = new DateTime(1991, 11, 18)
                  }
                  ,new Person
                  {
                      Id   = "004"
                     ,Name = "gsf_zero4"
                     ,Age  = 40
                     ,Birthday = new DateTime(1969, 11, 18)
                  }
                  ,new Person
                  {
                      Id   = "005"
                     ,Name = "gsf_zero5"
                     ,Age  = 44
                     ,Birthday = new DateTime(1965, 11, 18)
                  }
                };

    // チーム
    IEnumerable<Team> teams =
                new[]{
                   new Team
                   {
                      Id    = "001"
                     ,Name  = "team_1"
                     ,Members = new []{ "001", "004" }
                   }
                  ,new Team
                  {
                      Id    = "002"
                     ,Name  = "team_2"
                     ,Members = new []{ "002", "003" }
                   }
                };

    // プロジェクト
    IEnumerable<Project> projects =
                new[]{
                   new Project
                   {
                      Id    = "001"
                     ,Name  = "project_1"
                     ,Members = new []{ "001", "002" }
                     ,From  = new DateTime(2009, 8, 1)
                     ,To    = new DateTime(2009, 10, 15)
                     ,State   = ProjectState.Done
                   }
                  ,new Project
                  {
                      Id    = "002"
                     ,Name  = "project_2"
                     ,Members = new []{ "003", "004" }
                     ,From  = new DateTime(2009, 9, 1)
                     ,To    = new DateTime(2009, 11, 25)
                     ,State   = ProjectState.Done
                   }
                  ,new Project
                  {
                      Id    = "003"
                     ,Name  = "project_3"
                     ,Members = new []{ "001" }
                     ,From  = new DateTime(2007, 1, 10)
                     ,To    = null
                     ,State   = ProjectState.Processing
                   }
                  ,new Project
                  {
                      Id    = "004"
                     ,Name  = "project_4"
                     ,Members = new []{ "001", "002", "003", "004" }
                     ,From  = new DateTime(2010, 1, 5)
                     ,To    = null
                     ,State   = ProjectState.NotStarted
                   }
                };

    public void Execute()
    {
      //
      // グループ化結合のサンプル
      //
      // join時にintoキーワードを利用して結果を保持した
      // ものをグループ化結合という。
      //

      //
      // 所属するプロジェクトをメンバー単位で取得.
      // (Person側からProjectへのアプローチ)
      //
      // Project.MembersはN件存在するので、joinするために
      // 平坦化する必要がある。(SelectMany)
      //
      var query1 = from person in persons
             join prj in (
               from project in projects
               from member in project.Members
               select new
               {
                  Id = project.Id
                 ,Name = project.Name
                 ,PersonId = member
               }
             ) on person.Id equals prj.PersonId into personProjects
             select new
             {
                Name = person.Name
               ,Projects = personProjects
             };

      foreach (var item in query1)
      {
        Console.WriteLine("Name={0}", item.Name);

        foreach (var project in item.Projects)
        {
          Console.WriteLine("\tProject={0}", project.Name);
        }
      }

      //
      // プロジェクトに所属するメンバーをプロジェクト単位で取得.
      // (Project側からPersonへのアプローチ)
      //
      // 以下の行程を経る必要がある。
      //
      // 1.Project.MembersはN件となるので平坦化.
      //   (メンバーが2人いるプロジェクトだと２データとなる。）
      // 2.1の結果とPersonを結合.
      // 3.2の結果をグルーピング.
      //
      var query2 = from project in (
               // 1の処理.
               from project in projects
               from member in project.Members
               select new
               {
                 Id = project.Id,
                 Name = project.Name,
                 Member = member
               }
             )
             // 2の処理.
             join person in persons on project.Member equals person.Id into personByProject
             select new
             {
               Id = project.Id,
               Name = project.Name,
               Persons = personByProject
             } into selectResult
             // 3の処理.
             group selectResult by new
             {
               selectResult.Id,
               selectResult.Name
             };

      Console.WriteLine("======================================================");
      foreach (var item in query2)
      {
        Console.WriteLine("Name={0}", item.Key.Name);

        foreach (var groupedItem in item)
        {

          foreach (var belongPerson in groupedItem.Persons)
          {
            Console.WriteLine("\tPerson={0}", belongPerson.Name);
          }
        }
      }

      //
      // joinを利用せずに記述した版.
      // (PersonからProject)
      //
      var query3 = from person in persons
             from project in projects
             where project.Members.Contains(person.Id)
             select new
             {
               Person = person,
               Project = project
             } into selectResult
             group selectResult by selectResult.Project.Name into groupByResult
             orderby groupByResult.Key ascending
             select groupByResult;

      Console.WriteLine("======================================================");
      foreach(var item in query3)
      {
        Console.WriteLine("Project={0}", item.Key);

        foreach(var groupedItem in item)
        {
          Console.WriteLine("\tPerson={0}", groupedItem.Person.Name);
        }
      }
    }
  }
  #endregion

  #region LinqSamples-11
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples11 : IExecutable
  {
    public class Person
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public int Age
      {
        get;
        set;
      }
      public DateTime Birthday
      {
        get;
        set;
      }
    }

    public class Team
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public IEnumerable<string> Members
      {
        get;
        set;
      }
    }

    public enum ProjectState
    {
      NotStarted
       ,
      Processing
        ,
      Done
    }

    public class Project
    {
      public string Id
      {
        get;
        set;
      }
      public string Name
      {
        get;
        set;
      }
      public IEnumerable<string> Members
      {
        get;
        set;
      }
      public DateTime From
      {
        get;
        set;
      }
      public DateTime? To
      {
        get;
        set;
      }
      public ProjectState State
      {
        get;
        set;
      }
    }

    // メンバー
    IEnumerable<Person> persons =
                new[]{
                   new Person
                   {
                      Id   = "001"
                     ,Name = "gsf_zero1"
                     ,Age  = 30
                     ,Birthday = new DateTime(1979, 11, 18)
                   }
                  ,new Person
                  {
                      Id   = "002"
                     ,Name = "gsf_zero2"
                     ,Age  = 20
                     ,Birthday = new DateTime(1989, 11, 18)
                   }
                  ,new Person
                  {
                      Id   = "003"
                     ,Name = "gsf_zero3"
                     ,Age  = 18
                     ,Birthday = new DateTime(1991, 11, 18)
                  }
                  ,new Person
                  {
                      Id   = "004"
                     ,Name = "gsf_zero4"
                     ,Age  = 40
                     ,Birthday = new DateTime(1969, 11, 18)
                  }
                  ,new Person
                  {
                      Id   = "005"
                     ,Name = "gsf_zero5"
                     ,Age  = 44
                     ,Birthday = new DateTime(1965, 11, 18)
                  }
                };

    // チーム
    IEnumerable<Team> teams =
                new[]{
                   new Team
                   {
                      Id    = "001"
                     ,Name  = "team_1"
                     ,Members = new []{ "001", "004" }
                   }
                  ,new Team
                  {
                      Id    = "002"
                     ,Name  = "team_2"
                     ,Members = new []{ "002", "003" }
                   }
                };

    // プロジェクト
    IEnumerable<Project> projects =
                new[]{
                   new Project
                   {
                      Id    = "001"
                     ,Name  = "project_1"
                     ,Members = new []{ "001", "002" }
                     ,From  = new DateTime(2009, 8, 1)
                     ,To    = new DateTime(2009, 10, 15)
                     ,State   = ProjectState.Done
                   }
                  ,new Project
                  {
                      Id    = "002"
                     ,Name  = "project_2"
                     ,Members = new []{ "003", "004" }
                     ,From  = new DateTime(2009, 9, 1)
                     ,To    = new DateTime(2009, 11, 25)
                     ,State   = ProjectState.Done
                   }
                  ,new Project
                  {
                      Id    = "003"
                     ,Name  = "project_3"
                     ,Members = new []{ "001" }
                     ,From  = new DateTime(2007, 1, 10)
                     ,To    = null
                     ,State   = ProjectState.Processing
                   }
                  ,new Project
                  {
                      Id    = "004"
                     ,Name  = "project_4"
                     ,Members = new []{ "001", "002", "003", "004" }
                     ,From  = new DateTime(2010, 1, 5)
                     ,To    = null
                     ,State   = ProjectState.NotStarted
                   }
                };

    public void Execute()
    {
      //
      // グループ化結合のサンプル
      //
      // グループ化結合したものにDefaultIfEmptyメソッドを適用したものが
      // Linqでの左外部結合となる。
      //
      
      //
      // 外部結合していない版.
      //
      // 以下のクエリの場合、gsf_zero5が表示されない。
      // (最後のfrom personProject in personProjectsの部分で除外される。）
      var query  = from person in persons
             join prj in (
               from project in projects
               from member  in project.Members
               select new { Id = project.Id, Name = project.Name, Member = member }
             ) on person.Id equals prj.Member into personProjects
             from personProject in personProjects
             select new { 
                Id = person.Id
               ,Name = person.Name
               ,Project = personProject.Name
             };

      query.ToList().ForEach(Console.WriteLine);

      //
      // 外部結合版
      //
      var query2 = from person in persons
             join prj in (
               from project in projects
               from member  in project.Members
               select new { Id = project.Id, Name = project.Name, Member = member }
             ) on person.Id equals prj.Member into personProjects
             // 外部結合するためにDefaultIfEmptyを使用.
             from personProject in personProjects.DefaultIfEmpty()
             select new { 
                Id = person.Id
               ,Name = person.Name
               // 結合対象が存在しなかった場合、その型のdefault(T)の値となってので
               // 望みの形に変換する.
               ,Project = (personProject == null) ? "''" : personProject.Name
             };

      Console.WriteLine("======================================================");
      query2.ToList().ForEach(Console.WriteLine);
    }
  }
  #endregion
  
  #region LinqSamples-12
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples12 : IExecutable
  {
    class Person
    {
      public int Id { get; set; }
      public string Name { get; set; }
    }
    
    public void Execute()
    {
      var persons = new List<Person>
      {
         new Person{ Id = 1, Name = "gsf_zero1" }
        ,new Person{ Id = 2, Name = "gsf_zero2" }
        ,new Person{ Id = 3, Name = "gsf_zero3" }
        ,new Person{ Id = 4, Name = "gsf_zero4" }
        ,new Person{ Id = 5, Name = "gsf_zero5" }
      };
      
      var query = from   aPerson in persons
            where  (aPerson.Id % 2) == 0
            select aPerson;
      
      Console.WriteLine("============ クエリを表示 ============");
      foreach(var aPerson in query)
      {
        Console.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
      }
      
      //
      // ToArrayを利用して、明示的に配列に変換.
      // (このタイミングでクエリが評価され、結果が構築される。)
      //
      Person[] filteredPersons = query.ToArray();
      
      Console.WriteLine("============ ToArrayで作成したリストを表示 ============");
      foreach(var aPerson in filteredPersons)
      {
        Console.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
      }
      
      //
      // 元のリストを変更.
      //
      persons.Add(new Person{ Id = 6, Name = "gsf_zero6" });
      
      //
      // もう一度、各結果を表示.
      //
      Console.WriteLine("============ クエリを表示（2回目） ============");
      foreach(var aPerson in query)
      {
        Console.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
      }
      
      Console.WriteLine("============ ToArrayで作成したリストを表示 （2回目）============");
      foreach(var aPerson in filteredPersons)
      {
        Console.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
      }
    }
  }
  #endregion
  
  #region LinqSamples-13
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples13 : IExecutable
  {
    class Person
    {
      public int Id { get; set; }
      public string Name { get; set; }
    }

    public void Execute()
    {
      var persons = new List<Person>
      {
         new Person{ Id = 1, Name = "gsf_zero1" }
        ,new Person{ Id = 2, Name = "gsf_zero2" }
        ,new Person{ Id = 3, Name = "gsf_zero3" }
        ,new Person{ Id = 4, Name = "gsf_zero4" }
        ,new Person{ Id = 5, Name = "gsf_zero5" }
      };
      
      var query = from   aPerson in persons
            where  (aPerson.Id % 2) != 0
            select aPerson;
      
      Console.WriteLine("============ クエリを表示 ============");
      foreach(var aPerson in query)
      {
        Console.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
      }
      
      //
      // ToListを利用して、明示的にリストに変換.
      // (このタイミングでクエリが評価され、結果が構築される。)
      //
      List<Person> filteredPersons = query.ToList();
      
      Console.WriteLine("============ ToListで作成したリストを表示 ============");
      foreach(var aPerson in filteredPersons)
      {
        Console.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
      }
      
      //
      // 元のリストを変更.
      //
      persons.Add(new Person{ Id = 6, Name = "gsf_zero6" });
      persons.Add(new Person{ Id = 7, Name = "gsf_zero7" });
      
      //
      // もう一度、各結果を表示.
      //
      Console.WriteLine("============ クエリを表示（2回目） ============");
      foreach(var aPerson in query)
      {
        Console.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
      }
      
      Console.WriteLine("============ ToListで作成したリストを表示 （2回目）============");
      foreach(var aPerson in filteredPersons)
      {
        Console.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
      }
    }
  }
  #endregion

  #region LinqSamples-14
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples14 : IExecutable
  {
    class Person
    {
      public int Id { get; set; }
      public string Name { get; set; }
        
      public override string ToString()
      {
        return string.Format("[{0}, {1}]", Id, Name);
      }
    }
    
    public void Execute()
    {
      var persons = new List<Person>
      {
         new Person{ Id = 1, Name = "gsf_zero1" }
        ,new Person{ Id = 2, Name = "gsf_zero2" }
        ,new Person{ Id = 3, Name = "gsf_zero3" }
        ,new Person{ Id = 4, Name = "gsf_zero4" }
        ,new Person{ Id = 5, Name = "gsf_zero5" }
      };
      
      var query = from   aPerson in persons
            where  (aPerson.Id % 2) != 0
            select aPerson;
      
      Console.WriteLine("============ クエリを表示 ============");
      foreach(var aPerson in query)
      {
        Console.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
      }
      
      //
      // ToDictionaryを利用して、明示的にマップに変換.
      // (このタイミングでクエリが評価され、結果が構築される。)
      //
      // ToDictionaryは、キー要素を抽出するための引数を要求する。(keySelector)
      // 引数にkeySelectorのみを指定している場合、マップの値はそのオブジェクト自身となる。
      //   キー：int32型 (Person.Id)
      //   値　：Personオブジェクト
      //
      // このほかにも、elementSelectorを指定するオーバーロードも存在する。
      //
      Dictionary<int, Person> filteredPersons = query.ToDictionary(item => item.Id);
      
      Console.WriteLine("============ ToDictionaryで作成したリストを表示 ============");
      foreach(var pair in filteredPersons)
      {
        Console.WriteLine("KEY={0}, VALUE={1}", pair.Key, pair.Value);
      }
      
      //
      // 元のリストを変更.
      //
      persons.Add(new Person{ Id = 6, Name = "gsf_zero6" });
      persons.Add(new Person{ Id = 7, Name = "gsf_zero7" });
      
      //
      // もう一度、各結果を表示.
      //
      Console.WriteLine("============ クエリを表示（2回目） ============");
      foreach(var aPerson in query)
      {
        Console.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
      }
      
      Console.WriteLine("============ ToDictionaryで作成したリストを表示 （2回目）============");
      foreach(var pair in filteredPersons)
      {
        Console.WriteLine("KEY={0}, VALUE={1}", pair.Key, pair.Value);
      }
      
      //
      // ToDictionaryメソッドにkeySelectorとelementSelectorを指定したバージョン.
      //
      Console.WriteLine("============ keySelectorとelementSelectorの両方を指定したバージョン ============");
      foreach (var pair in query.ToDictionary(item => item.Id, item => string.Format("{0}_{1}", item.Id, item.Name)))
      {
        Console.WriteLine("KEY={0}, VALUE={1}", pair.Key, pair.Value);
      }
    }
  }
  #endregion
  
  #region LinqSamples-15
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples15 : IExecutable
  {
    class Person
    {
      public int  Id   { get; set; }
      public string Name { get; set; }
      public string Team { get; set; }
    }
    
    public void Execute()
    {
      var persons = new List<Person>
      {
         new Person{ Id = 1, Name = "gsf_zero1", Team = "TeamA" }
        ,new Person{ Id = 2, Name = "gsf_zero2", Team = "TeamB" }
        ,new Person{ Id = 3, Name = "gsf_zero3", Team = "TeamA" }
        ,new Person{ Id = 4, Name = "gsf_zero4", Team = "TeamA" }
        ,new Person{ Id = 5, Name = "gsf_zero5", Team = "TeamB" }
        ,new Person{ Id = 6, Name = "gsf_zero6", Team = "TeamC" }
      };
      
      var query = from   aPerson in persons
            select aPerson;
      
      //
      // Teamの値をキーとして、グルーピング処理.
      // これにより、各キー毎に合致するPersonオブジェクトが紐付いた構造に変換される。
      //
      // 実際のオブジェクトの型は
      //   Lookup<Grouping<string, Person>>
      // となっている。
      //
      // 尚、Lookupオブジェクトを外部から新規で構築することはできない。
      //
      // 以下では、keySelectorを指定している。
      ILookup<string, Person> lookup = query.ToLookup(aPerson => aPerson.Team);
      
      //
      // ILookupに定義されているプロパティにアクセス.
      //   通常、Lookupオブジェクトはループを経由してデータを取得するが、キーを指定して、アクセスすることもできる。
      //
      Console.WriteLine("カウント={0}", lookup.Count);
      foreach (Person teamAPerson in lookup["TeamA"])
      {
        Console.WriteLine(teamAPerson);
      }
      
      //
      // ILookup<TKey, TElement>は、IEnumerable<IGrouping<TKey, TElement>>を継承しているので
      // ループさせることで、IGrouping<TKey, TElement>を取得することができる。
      //
      // このIGrouping<TKey, TElement>が一対多のマッピングを実現している。
      // 尚、IGrouping<TKey, TElement>は、クエリ式にてgroup byを行った際にも取得できる。
      //
      Console.WriteLine("=========== ToLookupに対してkeySelectorを指定した版 =============");
      foreach (IGrouping<string, Person> grouping in lookup)
      {
        Console.WriteLine("KEY={0}", grouping.Key);
        foreach (var aPerson in grouping)
        {
          Console.WriteLine("\tID={0}, NAME={1}", aPerson.Id, aPerson.Name);
        }
      }
      
      //
      // keySelectorとelementSelectorを指定してみる。
      //
      var lookup2 = query.ToLookup(aPerson => aPerson.Team, aPerson => string.Format("{0}_{1}", aPerson.Id, aPerson.Name));
      
      Console.WriteLine("=========== ToLookupに対してkeySelectorとelementSelectorを指定した版 =============");
      foreach (var grouping in lookup2)
      {
        Console.WriteLine("KEY={0}", grouping.Key);
        foreach (var element in grouping)
        {
          Console.WriteLine("\t{0}", element);
        }
      }
    }
  }
  #endregion
  
  #region LinqSamples-16
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples16 : IExecutable
  {
    class Person
    {
      public int  Id   { get; set; }
      public string Name { get; set; }
    }
    
    class Customer : Person
    {
      public IEnumerable<Order> Orders { get; set; }
    }
    
    class Order
    {
      public int Id     { get; set; }
      public int Quantity { get; set; }
    }
    
    public void Execute()
    {
      List<Person> persons = new List<Person>
      {
         new Person { Id = 1, Name = "gsf_zero1" }
        ,new Person { Id = 2, Name = "gsf_zero2" }
        ,new Customer { Id = 3, Name = "gsf_zero3", Orders = Enumerable.Empty<Order>() }
        ,new Customer 
           { 
             Id = 4
            ,Name = "gsf_zero4"
            ,Orders =  new List<Order>
               {
                  new Order { Id = 1, Quantity = 10 }
                 ,new Order { Id = 2, Quantity = 2  }
               }
           }
        ,new Person { Id = 5, Name = "gsf_zero5" }
      };

      //
      // OfTypeメソッドを利用することにより、特定の型のみのシーケンスに変換することができる。
      // 例えば、リストの中に基底クラスであるPersonクラスと派生クラスであるCustomerクラスの
      // オブジェクトが混在している場合、OfType<Person>とすると、そのまま。
      // OfType<Customer>とすると、Customerオブジェクトのみのシーケンスに変換される。
      //
      // 尚、OfTypeメソッドは他の変換演算子とは違い、ソースシーケンスのスナップショットを作成しない。
      // つまり、通常のクエリと同じく、OfTypeで取得したシーケンスが列挙される度に評価される。
      // 変換演算子の中で、このような動作を行うのはAsEnumerableとOfTypeとCastである。
      //
      Console.WriteLine("========== OfType<Person>の結果 ==========");
      foreach (var data in persons.OfType<Person>())
      {
        Console.WriteLine(data);
      }
      
      Console.WriteLine("========== OfType<Customer>の結果 ==========");
      foreach (var data in persons.OfType<Customer>())
      {
        Console.WriteLine(data);
      }
      
    /*
      IEnumerable<Customer> c = persons.OfType<Customer>();
      persons.Add(new Customer { Id = 99, Name = "gsf_zero3", Orders = Enumerable.Empty<Order>() });
      
      foreach (var data in c)
      {
        Console.WriteLine(data);
      }
    */
    
      //
      // 元々GenericではないリストをIEnumerable<T>に変換する場合にも利用出来る.
      //
      ArrayList arrayList = new ArrayList();
      arrayList.Add(10);
      arrayList.Add(20);
      arrayList.Add(30);
      arrayList.Add(40);
      
      Console.WriteLine("========== Genericではないコレクションを変換 ==========");
      IEnumerable<int> intList = arrayList.OfType<int>();
      foreach (var data in intList)
      {
        Console.WriteLine(data);
      }
    }
  }
  #endregion
  
  #region LinqSamples-17
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples17 : IExecutable
  {
    class Person
    {
      public int  Id   { get; set; }
      public string Name { get; set; }
    }
    
    class Customer : Person
    {
      public IEnumerable<Order> Orders { get; set; }
    }
    
    class Order
    {
      public int Id     { get; set; }
      public int Quantity { get; set; }
    }
    
    public void Execute()
    {
      List<Person> persons = new List<Person>
      {
         new Person { Id = 1, Name = "gsf_zero1" }
        ,new Person { Id = 2, Name = "gsf_zero2" }
        ,new Customer { Id = 3, Name = "gsf_zero3", Orders = Enumerable.Empty<Order>() }
        ,new Customer 
           { 
             Id = 4
            ,Name = "gsf_zero4"
            ,Orders =  new List<Order>
               {
                  new Order { Id = 1, Quantity = 10 }
                 ,new Order { Id = 2, Quantity = 2  }
               }
           }
        ,new Person { Id = 5, Name = "gsf_zero5" }
      };

      //
      // Castメソッドを利用することにより、特定の型のみのシーケンスに変換することができる。
      // OfTypeメソッドと違い、Castメソッドは単純にキャスト処理を行う為、キャスト出来ない型が
      // 含まれている場合は例外が発生する。
      // (OfTypeメソッドの場合、除外される。）
      //
      //
      // 尚、Castメソッドは他の変換演算子とは違い、ソースシーケンスのスナップショットを作成しない。
      // つまり、通常のクエリと同じく、Castで取得したシーケンスが列挙される度に評価される。
      // 変換演算子の中で、このような動作を行うのはAsEnumerableとOfTypeとCastである。
      //
      Console.WriteLine("========== Cast<Person>の結果 ==========");
      foreach (var data in persons.Cast<Person>())
      {
        Console.WriteLine(data);
      }
      
      //////////////////////////////////////////////////////////
      //
      // 以下のpersons.Cast<Customer>()はPersonオブジェクトをCustomerオブジェクトに
      // キャスト出来ない為、例外が発生する。
      //
      Console.WriteLine("========== Cast<Customer>の結果 ==========");
      try
      {
        foreach (var data in persons.Cast<Customer>())
        {
          Console.WriteLine(data);
        }
      } 
      catch(InvalidCastException ex)
      {
        Console.WriteLine(ex.Message);
      }
      
    /*
      IEnumerable<Person> p = persons.Cast<Person>();
      persons.Add(new Person());
      
      Console.WriteLine("aaa");
      foreach (var a in p)
      {
        Console.WriteLine(a);
      }
    */
    
      //
      // 元々GenericではないリストをIEnumerable<T>に変換する場合にも利用出来る.
      // 当然、Castメソッドを利用する場合は、コレクション内部のデータが全てキャスト可能で
      // ないといけない。
      //
      ArrayList arrayList = new ArrayList();
      arrayList.Add(10);
      arrayList.Add(20);
      arrayList.Add(30);
      arrayList.Add(40);
      
      Console.WriteLine("========== Genericではないコレクションを変換 ==========");
      IEnumerable<int> intList = arrayList.Cast<int>();
      foreach (var data in intList)
      {
        Console.WriteLine(data);
      }
    }
  }
  #endregion
  
  #region LinqSamples-18 AND LinqSamples-19 AND 拡張メソッド解決
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  /// <remarks>
  /// このサンプルは、以下の2つのLINQサンプルが記述されています。
  ///
  ///  ・LinqSamples-18
  ///  ・LinqSamples-19
  ///
  /// さらに以下の事柄にも触れています。
  ///
  ///  ・拡張メソッド解決
  /// </remarks>
  public class LinqSamples18 : IExecutable
  {
    public void Execute()
    {
      Persons persons = new Persons
      {
         new Person { Id = 1, Name = "gsf_zero1" }
        ,new Person { Id = 2, Name = "gsf_zero2" }
        ,new Person { Id = 3, Name = "gsf_zero3" }
      };
      
      //
      // PersonExtensionが定義されているので
      // そのまま実行すると、Whereの部分にてPersonExtension.Whereが
      // 呼ばれる.
      //
      Console.WriteLine("===== 拡張メソッドを定義した状態でそのままクエリ実行 =====");
      var query = from   aPerson in persons
            where  aPerson.Id == 2
            select aPerson;
      
      foreach (var aPerson in query)
      {
        Console.WriteLine(aPerson);
      }
      
      //
      // AsEnumerableメソッドを利用して、PersonsをIEnumerable<Person>に
      // 変換すると、カスタムWhere拡張メソッドは呼ばれない。
      //
      Console.WriteLine("===== AsEnumerableメソッドを利用してから、クエリ実行 =====");
      var query2 = from   aPerson in persons.AsEnumerable()
             where  aPerson.Id == 2
             select aPerson;
      
      foreach (var aPerson in query2)
      {
        Console.WriteLine(aPerson);
      }
    }
  }
  
  public class Person
  {
    public int  Id   { get; set; }
    public string Name { get; set; }
  }
  
  public class Persons : List<Person>
  {
  }
  
  public static class PersonExtension
  {
    public static Persons Where(this Persons self, Func<Person, bool> predicate)
    {
      var result = new Persons();
      
      Console.WriteLine("========= WHERE ========");
      foreach (var aPerson in self)
      {
        if (predicate(aPerson))
        {
          result.Add(aPerson);
        }
      }
      
      return result;
    }
  }
  #endregion
  
  #region LinqSamples-19
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples19 : IExecutable
  {
    class Person
    {
      public int  Id   { get; set; }
      public string Name { get; set; }
    }
    
    public void Execute()
    {
      var persons = new List<Person>
      {
         new Person { Id = 1, Name = "gsf_zero1" }
        ,new Person { Id = 2, Name = "gsf_zero2" }
        ,new Person { Id = 3, Name = "gsf_zero3" }
      };
      
      //
      // Where拡張メソッドを利用。
      //   クエリ式で利用しているwhere句と利用方法は同じ。
      //   Where拡張メソッドは、２つのオーバーロードをもっており
      //
      //   一つは、Func<T, bool>のタイプ。もう一つは、Func<T, int, bool>のタイプとなる。
      //   2つ目のタイプの場合、その時のインデックスが引数として設定される。
      //
      Console.WriteLine("===== Func<T, bool>のタイプ =====");
      foreach (var aPerson in persons.Where(item => item.Id == 2))
      {
        Console.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
      }
      
      Console.WriteLine("===== Func<T, int, bool>のタイプ =====");
      foreach (var aPerson in persons.Where((item, index) => index == 2))
      {
        Console.WriteLine("ID={0}, NAME={1}", aPerson.Id, aPerson.Name);
      }
    }
  }
  #endregion
  
  #region LinqSamples-20
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples20 : IExecutable
  {
    class Person
    {
      public int  Id   { get; set; }
      public string Name { get; set; }
    }

    public void Execute()
    {
      var persons = new List<Person>
      {
         new Person { Id = 1, Name = "gsf_zero1" }
        ,new Person { Id = 2, Name = "gsf_zero2" }
        ,new Person { Id = 3, Name = "gsf_zero3" }
      };
      
      //
      // Select拡張メソッドを利用。
      //   クエリ式で利用しているselect句と利用方法は同じ。
      //   select拡張メソッドは、２つのオーバーロードをもっており
      //
      //   一つは、Func<TSource, TResult>のタイプ。もう一つは、Func<TSource, int, TResult>のタイプとなる。
      //   2つ目のタイプの場合、その時のインデックスが引数として設定される。
      //
      Console.WriteLine("===== Func<TSource, TResult>のタイプ =====");
      foreach (var name in persons.Select(item => item.Name))
      {
        Console.WriteLine("NAME={0}", name);
      }
      
      Console.WriteLine("===== Func<TSource, int, TResult>のタイプ =====");
      foreach (var name in persons.Select((item, index) => string.Format("{0}_{1}", item.Name, index)))
      {
        Console.WriteLine("NAME={0}", name);
      }
    }
  }
  #endregion
  
  #region LinqSamples-21
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples21 : IExecutable
  {
    class Team
    {
      public string        Name  { get; set; }
      public IEnumerable<string> Members { get; set; }
    }
    
    public void Execute()
    {
      var teams = new List<Team>
            {
              new Team { Name = "Team A", Members = new List<string>{ "gsf_zero1", "gsf_zero2" } },
              new Team { Name = "Team B", Members = new List<string>{ "gsf_zero3", "gsf_zero4" } }
            };
      
      //
      // SelectMany拡張メソッドは、コレクションを平坦化する際に利用できる。
      // 例えば、上記で作成したteams変数は以下のような構造になっている。
      //
      //   teams -- [0]:Teamオブジェクト
      //            └Members:IEnumerable<string>
      //        [1]:Teamオブジェクト
      //            └Members:IEnumerable<string>
      //
      // 各Teamオブジェクトは、Membersプロパティを持っているので
      // SelectManyではなく、Select拡張メソッドを利用して
      //  teams.Select(team => team.Members)
      // とすると、結果は、IEnumerable<IEnumerable<string>>となる。
      //
      // このような状態で、SelectMany拡張メソッドを利用して
      //  teams.SelectMany(team => team.Members)
      // とすると、結果は、IEnumerable<string>となる。
      // つまり、SelectMany拡張メソッドは、各Selectorが返すシーケンスを最終的に
      // 平坦化してから結果を返してくれる。
      //
      // 尚、SelectMany拡張メソッドは、クエリ式にて2段以上のfrom句を利用している場合
      // 暗黙的に利用されている。上記のteams.SelectMany(team => team.Members)は
      // 以下のクエリ式と同じである。
      //
      //   from   team   in teams
      //   from   member in team.Members
      //   select member
      //
      // 実行時には、最後のselectの部分がSelectManyに置換される。
      //
      Console.WriteLine("===== Func<TSource, IEnumerable<TResult>>のサンプル =====");
      foreach (var member in teams.SelectMany(team => team.Members))
      {
        Console.WriteLine(member);
      }
      
      Console.WriteLine("===== Func<TSource, int, IEnumerable<TResult>>のサンプル =====");
      foreach (var member in teams.SelectMany((team, index) => (index % 2 == 0) ? team.Members : new List<string>()))
      {
        Console.WriteLine(member);
      }
      
      Console.WriteLine("===== collectionSelectorとresultSelectorを利用しているサンプル (1) =====");
      var query = teams.SelectMany
            (
              team => team.Members,                     // collectionSelector
              (team, member) => new { Team = team.Name, Name = member }   // resultSelector
            );
      
      foreach (var item in query)
      {
        Console.WriteLine(item);
      }
      
      Console.WriteLine("===== collectionSelectorとresultSelectorを利用しているサンプル (2) =====");
      var query2 = teams.SelectMany
             (
               (team, index)  => (index % 2 != 0) ? team.Members : new List<string>(),  // collectionSelector
               (team, member) => new { Team = team.Name, Name = member }        // resultSelector
             );
      
      foreach (var item in query2)
      {
        Console.WriteLine(item);
      }
    }
  }
  #endregion
  
  #region LinqSamples-22
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples22 : IExecutable
  {
    class Person
    {
      public int  Id   { get; set; }
      public string Name { get; set; }
        
      public override string ToString()
      {
        return string.Format("[ID={0}, NAME={1}]", Id, Name);
      }
    }
    
    public void  Execute()
    {
      var persons = new List<Person>
              {
                new Person{ Id = 1001, Name = "gsf_zero1" },
                new Person{ Id = 1000, Name = "gsf_zero2" },
                new Person{ Id = 111,  Name = "gsf_zero3" },
                new Person{ Id = 9889, Name = "gsf_zero4" },
                new Person{ Id = 9889, Name = "gsf_zero5" },
                new Person{ Id = 100,  Name = "gsf_zero6" }
              };
      
      //
      // 順序付け演算子には、以下のものが存在する。
      //
      //   ・OrderBy
      //   ・OrderByDescending
      //   ・ThenBy
      //   ・ThenByDescending
      //
      // OrderByは昇順ソート、OrderByDescendingは降順ソートを行う。どちらも単一キーにてソートを行う。
      // 複合キーにて、ソート処理を行う場合は、OrderBy及びOrderByDescendingに続いて、ThenBy及びThenByDescendingを利用する。
      //
      // OrderBy及びOrderByDescendingメソッドは、他のLINQ標準演算子と戻り値が異なっており
      //   IOrderedEnumerable<T>
      // を返す。また、ThenBy及びThenByDescendingメソッドは、引数にIOrderedEnumerable<T>を渡す必要がある。
      // なので、必然的に、ThenByはOrderByの後で呼び出さないと利用出来ない。
      //
      // LINQの並び替え処理は、安定ソート(Stable Sort)である。
      // つまり、同じキーの要素がシーケンス内に複数存在した場合、並び替えた結果は元の順番を保持している。
      //
      
      //
      // IDで昇順ソート.
      //
      var sortByIdAsc = persons.OrderBy(aPerson => aPerson.Id);
      
      Console.WriteLine("================= IDで昇順ソート =================");
      Console.WriteLine(string.Join(Environment.NewLine, sortByIdAsc));
      
      //
      // IDで降順ソート.
      //
      var sortByIdDesc = persons.OrderByDescending(aPerson => aPerson.Id);
      
      Console.WriteLine("================= IDで降順ソート =================");
      Console.WriteLine(string.Join(Environment.NewLine, sortByIdDesc));
      
      //
      // 安定ソートの確認。
      //
      var sortByIdAscAndDesc = persons.OrderByDescending(aPerson => aPerson.Id).OrderBy(aPerson => aPerson.Id);
      
      Console.WriteLine("================= 安定ソートの確認 =================");
      Console.WriteLine(string.Join(Environment.NewLine, sortByIdAscAndDesc));
    }
  }
  #endregion
  
  #region LinqSamples-23
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples23 : IExecutable
  {
    class Person
    {
      public int  Id   { get; set; }
      public string Name { get; set; }
        
      public override string ToString()
      {
        return string.Format("[ID={0}, NAME={1}]", Id, Name);
      }
    }
    
    public void Execute()
    {
      var persons = new List<Person>
              {
                new Person{ Id = 1001, Name = "gsf_zero1" },
                new Person{ Id = 1000, Name = "gsf_zero2" },
                new Person{ Id = 111,  Name = "gsf_zero3" },
                new Person{ Id = 9889, Name = "gsf_zero4" },
                new Person{ Id = 9889, Name = "gsf_zero5" },
                new Person{ Id = 100,  Name = "gsf_zero6" }
              };
      
      //
      // 順序付け演算子には、以下のものが存在する。
      //
      //   ・OrderBy
      //   ・OrderByDescending
      //   ・ThenBy
      //   ・ThenByDescending
      //
      // OrderByは昇順ソート、OrderByDescendingは降順ソートを行う。どちらも単一キーにてソートを行う。
      // 複合キーにて、ソート処理を行う場合は、OrderBy及びOrderByDescendingに続いて、ThenBy及びThenByDescendingを利用する。
      //
      // OrderBy及びOrderByDescendingメソッドは、他のLINQ標準演算子と戻り値が異なっており
      //   IOrderedEnumerable<T>
      // を返す。また、ThenBy及びThenByDescendingメソッドは、引数にIOrderedEnumerable<T>を渡す必要がある。
      // なので、必然的に、ThenByはOrderByの後で呼び出さないと利用出来ない。
      //
      // LINQの並び替え処理は、安定ソート(Stable Sort)である。
      // つまり、同じキーの要素がシーケンス内に複数存在した場合、並び替えた結果は元の順番を保持している。
      //
      
      //
      // IDの昇順で、且つ、Nameの数字部分の昇順でソート.
      //
      // 以下のクエリ式と同じ事となる。
      //   from  aPerson in persons
      //   orderby aPerson.Id, aPerson.Name.Last().ToString().ToInt()
      //   select  aPerson
      //
      var sortByIdAndNameAsc = persons
                    .OrderBy(aPerson => aPerson.Id)
                    .ThenBy(aPerson => aPerson.Name.Last().ToString().ToInt());

      Console.WriteLine("================= IDの昇順で、且つ、Nameの数字部分の昇順でソート. =================");
      Console.WriteLine(string.Join(Environment.NewLine, sortByIdAndNameAsc));
      
      //
      // IDの昇順で、且つ、Nameの数字部分の降順でソート.
      //
      // 以下のクエリ式と同じ事となる。
      //   from  aPerson in persons
      //   orderby aPerson.Id, aPerson.Name.Last().ToString().ToInt() descending
      //   select  aPerson
      //
      var sortByIdAndNameDesc = persons
                    .OrderBy(aPerson => aPerson.Id)
                    .ThenByDescending(aPerson => aPerson.Name.Last().ToString().ToInt());
      
      Console.WriteLine("================= IDの昇順で、且つ、Nameの数字部分の降順でソート. =================");
      Console.WriteLine(string.Join(Environment.NewLine, sortByIdAndNameDesc));
    }
  }
  #endregion
  
  #region LinqSamples-24
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples24 : IExecutable
  {
    class Person
    {
      public int  Id   { get; set; }
      public string Name { get; set; }
        
      public override string ToString()
      {
        return string.Format("[ID={0}, NAME={1}]", Id, Name);
      }
    }
    
    public void Execute()
    {
      IEnumerable<int>  numbers = new int[] { 1, 2, 3, 4, 5 };
      IEnumerable<Person> persons = new List<Person>
                          {
                            new Person{ Id = 1001, Name = "gsf_zero1" },
                            new Person{ Id = 1000, Name = "gsf_zero2" },
                            new Person{ Id = 111,  Name = "gsf_zero3" },
                            new Person{ Id = 9889, Name = "gsf_zero4" },
                            new Person{ Id = 9889, Name = "gsf_zero5" },
                            new Person{ Id = 100,  Name = "gsf_zero6" }
                          };
              
      //
      // Reverse拡張メソッドは、文字通りソースシーケンスを逆順に変換するメソッドである。
      // このメソッドは、そのままソースシーケンスを逆順に変換するだけである。
      //
      // 尚、本メソッドは、他のLINQ演算子と同様に遅延実行される。
      //
      var reverseNumbers = numbers.Reverse();
      var reversePersons = persons.Reverse();
      
      Console.WriteLine(string.Join(",", reverseNumbers.Select(element => element.ToString())));
      Console.WriteLine(string.Join(",", reversePersons.Select(element => element.ToString())));
    }
  }
  #endregion
  
  #region LinqSamples-25
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples25 : IExecutable
  {
    class Person
    {
      public int  Id    { get; set; }
      public string Name  { get; set; }
      public string Team  { get; set; }
      public string Project { get; set; }
        
      public override string ToString()
      {
        return string.Format("[ID={0}, NAME={1}]", Id, Name);
      }
    }
    
    public void Execute()
    {
      var persons = new List<Person>
              {
                new Person{ Id = 1001, Name = "gsf_zero1", Team = "A", Project = "P1" },
                new Person{ Id = 1000, Name = "gsf_zero2", Team = "B", Project = "P1" },
                new Person{ Id = 111,  Name = "gsf_zero3", Team = "B", Project = "P2" },
                new Person{ Id = 9889, Name = "gsf_zero4", Team = "C", Project = "P2" },
                new Person{ Id = 9889, Name = "gsf_zero5", Team = "A", Project = "P1" },
                new Person{ Id = 100,  Name = "gsf_zero6", Team = "C", Project = "P2" }
              };
      
      //
      // GroupBy拡張メソッドは、シーケンスの要素をグループ化する際に利用できる。
      // クエリ式にて、[group xx by xx.xx]とする場合、実行時にGroupBy拡張メソッドに置き換えられる。
      //
      // 概念としては、SQLのGROUP BYと同じである。
      //
      // GroupBy拡張メソッドには、全部で8つのオーバーロードが存在する。
      // よく利用されるのは、以下のものとなると思われる。
      //
      //   ・keySelectorのみを指定するもの
      //   ・keySelectorとelementSelectorを指定するもの。
      //   ・複合キーでのグループ化
      //
      // GroupBy拡張メソッドの戻り値は、
      //   IEnumerable<IGrouping<TKey, TElement>>
      // となる。IGroupingインターフェースは、グルーピングを表すインターフェースであり
      // Keyプロパティが定義されている。
      //
      // このインターフェース自身も、IEnumerableインターフェースを継承しているので
      // グルーピングを行った場合は、以下のようにして2重のループでデータを取得する。
      //
      // var query = xxx.GroupBy(item => item.Key);
      // foreach (var group in query)
      // {
      //   Console.WriteLine(group.Key);
      //   foreach (var item in group)
      //   {
      //     Console.WriteLine(item);
      //   }
      // }
      //
      
      //
      // keySelectorのみを指定.
      //
      // 以下のクエリ式と同じとなる。
      //   from  thePerson in persons
      //   group thePerson by thePerson.Team
      //
      Console.WriteLine("============ keySelectorのみのGroupBy ==============");
      var query1 = persons.GroupBy(thePerson => thePerson.Team);
      foreach (var group in query1)
      {
        Console.WriteLine("=== {0}", group.Key);
        foreach (var thePerson in group)
        {
          Console.WriteLine("\t{0}", thePerson);
        }
      }

      //
      // keySelectorとelementSelectorを指定.
      //
      // 以下のクエリ式と同じとなる。
      //   from   thePerson in persons
      //   group  thePerson.Name by thePerson.Team
      //
      Console.WriteLine("\n============ elementSelectorを指定したGroupBy ==============");
      var query2 = persons.GroupBy(thePerson => thePerson.Team, thePerson => thePerson.Name);
      foreach (var group in query2)
      {
        Console.WriteLine("=== {0}", group.Key);
        foreach (var name in group)
        {
          Console.WriteLine("\t{0}", name);
        }
      }
      
      //
      // 複合キーにてグループ化.
      //
      // 以下のクエリ式と同じとなる。
      //   from  thePerson in persons
      //   group thePerson by new { thePerson.Project, thePerson.Team }
      //
      Console.WriteLine("\n============ 複合キーを指定したGroupBy ==============");
      var query3 = persons.GroupBy(thePerson => new { thePerson.Project, thePerson.Team });
      foreach (var group in query3)
      {
        Console.WriteLine("=== {0}", group.Key);
        foreach (var thePerson in group)
        {
          Console.WriteLine("\t{0}", thePerson);
        }
      }
      
      //
      // 以下のクエリ式と同じとなる。
      //   from  thePerson in persons
      //   group   thePerson by new { thePerson.Project, thePerson.Team } into p
      //   orderby p.Key.Project descending, p.Key.Team descending
      //   select  p;
      //
      Console.WriteLine("\n============ 複合キーとorderbyを指定したGroupBy ==============");
      var query4 = persons
              .GroupBy(thePerson => new { thePerson.Project, thePerson.Team })
              .OrderByDescending(group => group.Key.Project)
              .ThenByDescending(group => group.Key.Team);
      
      foreach (var group in query4)
      {
        Console.WriteLine("=== {0}", group.Key);
        foreach (var thePerson in group)
        {
          Console.WriteLine("\t{0}", thePerson);
        }
      }
    }
  }
  #endregion
  
  #region LinqSamples-26
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples26 : IExecutable
  {
    class Person
    {
      public string Name { get; set; }
      public Team   Team { get; set; }
    }
    
    class Team
    {
      public string Name { get; set; }
    }
    
    public void Execute()
    {
      var t1 = new Team { Name = "Team 1" };
      var t2 = new Team { Name = "Team 2" };
      
      var p1 = new Person { Name = "gsf_zero1", Team = t1 };
      var p2 = new Person { Name = "gsf_zero2", Team = t2 };
      var p3 = new Person { Name = "gsf_zero3", Team = t1 };
      
      var teams  = new List<Team>   { t1, t2 };
      var people = new List<Person> { p1, p2, p3 };
      
      //
      // 結合する.
      //
      // 以下のクエリ式と同じ事となる。
      //
      //   from   team   in teams
      //   join   person in people on team equals person.Team
      //   select new { TeamName = team.Name, PersonName = person.Name }
      //
      var query = 
          teams.Join         // TOuter
          (
            people,        // TInner
            team   => team,    // TOuterのキー
            person => person.Team, // TInnerのキー
            (team, person) =>    // 結果
              new { TeamName = team.Name, PersonName = person.Name }
          );
      
      foreach (var item in query)
      {
        Console.WriteLine("Team = {0}, Person = {1}", item.TeamName, item.PersonName);
      }
    }
  }
  #endregion
  
  #region LinqSamples-27
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples27 : IExecutable
  {
    class Person
    {
      public string Name { get; set; }
      public Team   Team { get; set; }
    }
    
    class Team
    {
      public string Name { get; set; }
    }

    public void Execute()
    {
      var t1 = new Team { Name = "Team 1" };
      var t2 = new Team { Name = "Team 2" };
      
      var p1 = new Person { Name = "gsf_zero1", Team = t1 };
      var p2 = new Person { Name = "gsf_zero2", Team = t2 };
      var p3 = new Person { Name = "gsf_zero3", Team = t1 };
      
      var teams  = new List<Team>   { t1, t2 };
      var people = new List<Person> { p1, p2, p3 };
      
      //
      // グループ結合する.
      // 
      // Join拡張メソッドと書式的にはほとんど同じであるが、以下の点が異なる。
      //  ・resultSelectorの書式が、(TOuter, IEnumerable<TInner>)となっている。
      // これにより、結果をJOINした結果をグルーピングした状態で保持することが出来る。（階層構造を構築出来る。）
      //
      // 以下のクエリ式と同じ事となる。
      //  from team   in teams
      //  join person in people on team equals person.Team into personCollection
      //  select new { Team = team, Persons = personCollection }
      //
      var query = teams.GroupJoin         // TOuter
            (
              people,           // TInner
              team   => team,       // TOuterのキー
              person => person.Team,    // TInnerのキー
              (team, personCollection) => // 結果 (TOuter, IEnumerable<TInner>)
                new { Team = team, Persons = personCollection }
            );
      
      foreach (var item in query)
      {
        Console.WriteLine("TEAM = {0}", item.Team.Name);
        foreach (var p in item.Persons)
        {
          Console.WriteLine("\tPERSON = {0}", p.Name);
        }
      }
    }
  }
  #endregion
  
  #region LinqSamples-28
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples28 : IExecutable
  {
    class Person
    {
      public string Name { get; set; }
      
      public override string ToString()
      {
        return string.Format("[NAME = {0}]", Name);
      }
    }
    
    class PersonComparer : EqualityComparer<Person>
    {
      public override bool Equals(Person p1, Person p2)
      {
        if (Object.Equals(p1, p2))
        {
          return true;
        }
        
        if (p1 == null || p2 == null)
        {
          return false;
        }
        
        return (p1.Name == p2.Name);
      }
      
      public override int GetHashCode(Person p)
      {
        return p.Name.GetHashCode();
      }
    }
    
    public void Execute()
    {
      //
      // 引数なしのDistinct拡張メソッドを利用.
      // この場合、既定のIEqualityComparer<T>を用いて比較が行われる。
      // 
      var numbers = new int[]
              {
              1, 2, 3, 4, 5,
              1, 2, 3, 6, 7
              };
              
      Console.WriteLine(JoinElements(numbers.Distinct()));
      
      //
      // 引数にIEqualityComparer<T>を指定して、Distinct拡張メソッドを利用。
      // この場合、引数に指定したComparerを用いて比較が行われる。
      //
      var people  = new Person[]
              { 
                new Person { Name = "gsf_zero1" }, 
                new Person { Name = "gsf_zero2" }, 
                new Person { Name = "gsf_zero1" },
                new Person { Name = "gsf_zero3" }
              };
              
      Console.WriteLine(JoinElements(people.Distinct(new PersonComparer())));
    }
    
    string JoinElements<T>(IEnumerable<T> elements)
    {
      return string.Join(",", elements.Select(item => item.ToString()));
    }
  }
  #endregion
  
  #region LinqSamples-29
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples29 : IExecutable
  {
    class Person
    {
      public string Name { get; set; }
      
      public override string ToString()
      {
        return string.Format("[NAME = {0}]", Name);
      }
    }
    
    class PersonComparer : EqualityComparer<Person>
    {
      public override bool Equals(Person p1, Person p2)
      {
        if (Object.Equals(p1, p2))
        {
          return true;
        }
        
        if (p1 == null || p2 == null)
        {
          return false;
        }
        
        return (p1.Name == p2.Name);
      }
      
      public override int GetHashCode(Person p)
      {
        return p.Name.GetHashCode();
      }
    }

    public void Execute()
    {
      //
      // 引数なしのUnion拡張メソッドを利用.
      // この場合、既定のIEqualityComparer<T>を用いて比較が行われる。
      //
      // Union拡張メソッドは、SQLでいうUNIONと同じ。(重複を除外する)
      // SQLでいう、UNION ALL（重複をそのまま残す）を行うにはConcat拡張メソッドを利用する。
      // ConcatしてDistinctするとUnionと同じ事になる。
      // 
      var numbers1 = new int[]
              {
              1, 2, 3, 4, 5
              };
              
      var numbers2 = new int[]
               {
               1, 2, 3, 6, 7
               };
              
      Console.WriteLine("UNION      = {0}", JoinElements(numbers1.Union(numbers2)));
      Console.WriteLine("CONCAT       = {0}", JoinElements(numbers1.Concat(numbers2)));
      Console.WriteLine("CONCAT->DISTINCT = {0}", JoinElements(numbers1.Concat(numbers2).Distinct()));
      
      //
      // 引数にIEqualityComparer<T>を指定して、Union拡張メソッドを利用。
      // この場合、引数に指定したComparerを用いて比較が行われる。
      //
      var people1 = new Person[]
              { 
                new Person { Name = "gsf_zero1" }, 
                new Person { Name = "gsf_zero2" }, 
                new Person { Name = "gsf_zero1" },
                new Person { Name = "gsf_zero3" }
              };

      var people2 = new Person[]
              { 
                new Person { Name = "gsf_zero4" }, 
                new Person { Name = "gsf_zero5" }, 
                new Person { Name = "gsf_zero6" },
                new Person { Name = "gsf_zero1" }
              };
              
      Console.WriteLine("UNION      = {0}", JoinElements(people1.Union(people2, new PersonComparer())));
      Console.WriteLine("CONCAT       = {0}", JoinElements(people1.Concat(people2)));
      Console.WriteLine("CONCAT->DISTINCT = {0}", JoinElements(people1.Concat(people2).Distinct(new PersonComparer())));
    }
    
    string JoinElements<T>(IEnumerable<T> elements)
    {
      return string.Join(",", elements.Select(item => item.ToString()));
    }
  }
  #endregion
  
  #region LinqSamples-30
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples30 : IExecutable
  {
    class Person
    {
      public string Name { get; set; }
      
      public override string ToString()
      {
        return string.Format("[NAME = {0}]", Name);
      }
    }
    
    class PersonComparer : EqualityComparer<Person>
    {
      public override bool Equals(Person p1, Person p2)
      {
        if (Object.Equals(p1, p2))
        {
          return true;
        }
        
        if (p1 == null || p2 == null)
        {
          return false;
        }
        
        return (p1.Name == p2.Name);
      }
      
      public override int GetHashCode(Person p)
      {
        return p.Name.GetHashCode();
      }
    }
    
    public void Execute()
    {
      //
      // 引数なしのIntersect拡張メソッドを利用.
      // この場合、既定のIEqualityComparer<T>を用いて比較が行われる。
      //
      // Intersect拡張メソッドは、積集合を求める。
      // つまり、両方のシーケンスに存在するデータのみが抽出される。
      // (Unionは和集合、Exceptは差集合となる。）
      // 
      var numbers1 = new int[]
              {
              1, 2, 3, 4, 5
              };
              
      var numbers2 = new int[]
               {
               1, 2, 3, 6, 7
               };
              
      Console.WriteLine("INTERSECT = {0}", JoinElements(numbers1.Intersect(numbers2)));
      
      //
      // 引数にIEqualityComparer<T>を指定して、Union拡張メソッドを利用。
      // この場合、引数に指定したComparerを用いて比較が行われる。
      //
      var people1 = new Person[]
              { 
                new Person { Name = "gsf_zero1" }, 
                new Person { Name = "gsf_zero2" }, 
                new Person { Name = "gsf_zero1" },
                new Person { Name = "gsf_zero3" }
              };

      var people2 = new Person[]
              { 
                new Person { Name = "gsf_zero4" }, 
                new Person { Name = "gsf_zero5" }, 
                new Person { Name = "gsf_zero6" },
                new Person { Name = "gsf_zero1" }
              };
              
      Console.WriteLine("INTERSECT = {0}", JoinElements(people1.Intersect(people2, new PersonComparer())));
    }
    
    string JoinElements<T>(IEnumerable<T> elements)
    {
      return string.Join(",", elements.Select(item => item.ToString()));
    }
  }
  #endregion
  
  #region LinqSamples-31
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples31 : IExecutable
  {
    class Person
    {
      public string Name { get; set; }
      
      public override string ToString()
      {
        return string.Format("[NAME = {0}]", Name);
      }
    }
    
    class PersonComparer : EqualityComparer<Person>
    {
      public override bool Equals(Person p1, Person p2)
      {
        if (Object.Equals(p1, p2))
        {
          return true;
        }
        
        if (p1 == null || p2 == null)
        {
          return false;
        }
        
        return (p1.Name == p2.Name);
      }
      
      public override int GetHashCode(Person p)
      {
        return p.Name.GetHashCode();
      }
    }
    
    public void Execute()
    {
      //
      // 引数なしのExcept拡張メソッドを利用.
      // この場合、既定のIEqualityComparer<T>を用いて比較が行われる。
      //
      // Except拡張メソッドは、差集合を求める。
      // (Unionは和集合、Intersectは積集合となる。）
      // 
      // このExcept拡張メソッドには、以下の仕様がある。
      //
      //   ・差集合の対象となるのは、1番目の集合のみであり、2番目の集合からは抽出されない。
      //     →つまり、引数で指定する方のシーケンスからは抽出されない。
      //   ・以下MSDNの記述を引用。
      //     「このメソッドは、second に含まれない、first の要素を返します。また、first に含まれない、second の要素は返しません。」
      // 
      var numbers1 = new int[]
              {
              1, 2, 3, 4, 5
              };
              
      var numbers2 = new int[]
               {
               1, 2, 3, 6, 7
               };
              
      Console.WriteLine("EXCEPT = {0}", JoinElements(numbers1.Except(numbers2)));
      
      //
      // 引数にIEqualityComparer<T>を指定して、Except拡張メソッドを利用。
      // この場合、引数に指定したComparerを用いて比較が行われる。
      //
      var people1 = new Person[]
              { 
                new Person { Name = "gsf_zero1" }, 
                new Person { Name = "gsf_zero2" }, 
                new Person { Name = "gsf_zero1" },
                new Person { Name = "gsf_zero3" }
              };

      var people2 = new Person[]
              { 
                new Person { Name = "gsf_zero4" }, 
                new Person { Name = "gsf_zero5" }, 
                new Person { Name = "gsf_zero6" },
                new Person { Name = "gsf_zero1" }
              };
              
      Console.WriteLine("EXCEPT = {0}", JoinElements(people1.Except(people2, new PersonComparer())));
    }
    
    string JoinElements<T>(IEnumerable<T> elements)
    {
      return string.Join(",", elements.Select(item => item.ToString()));
    }
  }
  #endregion
  
  #region LinqSamples-32
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples32 : IExecutable
  {
    class Person
    {
      public string Name { get; set; }
      
      public override string ToString()
      {
        return string.Format("[NAME={0}]", Name);
      }
    }
    
    public void Execute()
    {
      var people = new Person[]
             {
               new Person{ Name = "gsf_zero1" },
               new Person{ Name = "gsf_zero2" },
               new Person{ Name = "gsf_zero3" },
               new Person{ Name = "gsf_zero4" }
             };
             
      //
      // Count拡張メソッドは、シーケンスの要素数を取得するメソッドである。
      //
      // Count拡張メソッドには、predicateを指定できるオーバーロードが存在し
      // それを利用すると、特定の条件に一致するデータのみの件数を求める事が出来る。
      //
      // 尚、非常に多くの件数を返す可能性がある場合は、Countの代わりにLongCount拡張メソッドを
      // 使用する。使い方は、Count拡張メソッドと同じ。
      
      
      
      //
      // predicate無しで実行. 
      //
      Console.WriteLine("COUNT = {0}", people.Count());
      
      //
      // predicate有りで実行.
      //
      Console.WriteLine("COUNT = {0}", people.Count(person => int.Parse(person.Name.Last().ToString()) % 2 == 0));
      
      //
      // predicate無しで実行.（LongCount)
      //
      Console.WriteLine("COUNT = {0}", people.LongCount());
      
      //
      // predicate有りで実行.（LongCount)
      //
      Console.WriteLine("COUNT = {0}", people.LongCount(person => int.Parse(person.Name.Last().ToString()) % 2 == 0));
    }
  }
  #endregion
  
  #region LinqSample-33
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples33 : IExecutable
  {
    public void Execute()
    {
      var numbers = new int[]
              {
                1, 2, 3, 4, 5
              };
      
      // 
      // Sum拡張メソッドは、文字通り合計を求める拡張メソッド。
      //
      // Sum拡張メソッドには、各基本型のオーバーロードが用意されており
      // (decimal, double, int, long, single及びそれぞれのNullable型)
      // それぞれに、引数無しとselectorを指定するバージョンのメソッドがある。
      //
      
      //
      // 引数無しのSum拡張メソッドの使用.
      //
      Console.WriteLine("引数無し = {0}", numbers.Sum());
      
      //
      // selectorを指定するSum拡張メソッドの使用.
      //
      Console.WriteLine("引数有り = {0}", numbers.Sum(item => (item % 2 == 0) ? item : 0));
    }
  }
  #endregion
  
  #region LinqSamples-34
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples34 : IExecutable
  {
    public void Execute()
    {
      var numbers = new int[]
              {
                1, 2, 3, 4, 5, 0, 10, 98, 99
              };
      
      // 
      // Min, Max拡張メソッドは、文字通り最小値、最大値を求める拡張メソッド。
      //
      // Min, Max拡張メソッドには、各基本型のオーバーロードが用意されており
      // (decimal, double, int, long, single及びそれぞれのNullable型)
      // それぞれに、引数無しとselectorを指定するバージョンのメソッドがある。
      //
      
      //
      // 引数無しのMin, Max拡張メソッドの使用.
      //
      Console.WriteLine("引数無し[Min] = {0}", numbers.Min());
      Console.WriteLine("引数無し[Max] = {0}", numbers.Max());
      
      //
      // selectorを指定するMin, Max拡張メソッドの使用.
      //
      Console.WriteLine("引数有り[Min] = {0}", numbers.Min(item => (item % 2 == 0) ? item : 0));
      Console.WriteLine("引数有り[Max] = {0}", numbers.Max(item => (item % 2 == 0) ? item : 0));
    }
  }
  #endregion
  
  #region LinqSamples-35
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples35 : IExecutable
  {
    public void Execute()
    {
      var numbers = new int[]
              {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
              };
      
      // 
      // Average拡張メソッドは、文字通り平均を求める拡張メソッド。
      //
      // Average拡張メソッドには、各基本型のオーバーロードが用意されており
      // (decimal, double, int, long, single及びそれぞれのNullable型)
      // それぞれに、引数無しとselectorを指定するバージョンのメソッドがある。
      //
      
      //
      // 引数無しのAverage拡張メソッドの使用.
      //
      Console.WriteLine("引数無し = {0}", numbers.Average());
      
      //
      // selectorを指定するAverage拡張メソッドの使用.
      //
      Console.WriteLine("引数有り = {0}", numbers.Average(item => (item % 2 == 0) ? item : 0));
    }
  }
  #endregion
  
  #region LinqSamples-36
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples36 : IExecutable
  {
    public void Execute()
    {
      //
      // Zip拡張メソッド.
      //
      // Zip拡張メソッドは、Pythonのzip関数と同じ動きをするものである。
      // つまり、2つのシーケンスを同時にループさせることが出来る。
      //
      // 第二引数には、resultSelectorを指定する必要があり、好きなデータを返す事ができる。
      //
      // このメソッドは、どちらかのシーケンスが終わるまで処理を続けるという仕様になっているので
      // ２つのシーケンスの要素数が異なる場合は、注意が必要である。
      //
      // つまり、片方のシーケンスが空の場合、このメソッドは一度もループされない。
      //
      IEnumerable<int> numbers1 = new int[]{ 1, 2, 3, 4, 5 };
      IEnumerable<int> numbers2 = new int[]{ 6, 7, 8, 9, 0 };
      
      var query = numbers1.Zip(numbers2, (first, second) => Tuple.Create(first, second));
      
      Console.WriteLine("========= 2つのシーケンスの要素数が同じ場合 ===========");
      foreach (var item in query)
      {
        Console.WriteLine("FIRST={0}, SECOND={1}", item.Item1, item.Item2);
      }
      
      numbers1 = new int[]{ 1, 2, 3 };
      numbers2 = new int[]{ 6, 7, 8, 9, 0 };
      
      query = numbers1.Zip(numbers2, (first, second) => Tuple.Create(first, second));
      
      Console.WriteLine("========= 1つ目のシーケンスの要素が2つ目よりも少ない場合 ===========");
      foreach (var item in query)
      {
        Console.WriteLine("FIRST={0}, SECOND={1}", item.Item1, item.Item2);
      }
      
      numbers1 = new int[]{ 1, 2, 3, 4, 5 };
      numbers2 = new int[]{ 6, 7, 8 };
      
      query = numbers1.Zip(numbers2, (first, second) => Tuple.Create(first, second));
      
      Console.WriteLine("========= 2つ目のシーケンスの要素が1つ目よりも少ない場合 ===========");
      foreach (var item in query)
      {
        Console.WriteLine("FIRST={0}, SECOND={1}", item.Item1, item.Item2);
      }
      
      numbers1 = Enumerable.Empty<int>();
      numbers2 = new int[]{ 6, 7, 8 };
      
      query = numbers1.Zip(numbers2, (first, second) => Tuple.Create(first, second));
      
      Console.WriteLine("========= どちらかのシーケンスが空の場合 ===========");
      foreach (var item in query)
      {
        Console.WriteLine("FIRST={0}, SECOND={1}", item.Item1, item.Item2);
      }
    }
  }
  #endregion
  
  #region LinqSamples-37
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples37 : IExecutable
  {
    class Order
    {
      public string Name   { get; set; }
      public int  Amount { get; set; }
      public int  Month  { get; set; }
    }
    
    public void Execute()
    {
      //
      // Aggregate拡張メソッド.
      //
      // Aggregate拡張メソッドは、指定されたseedを起点値としてfunc関数を繰り返し呼び出し
      // 結果をアキュムレーターに保持してくれるメソッド。
      //
      // pythonのmap関数みたいなものである。
      //
      // Aggregateは、独自の集計処理を行う場合に利用する。
      // 尚、Sum, Min, Max, Average拡張メソッドなどはAggregateの特殊なパターンといえる。
      // (つまり、全部Aggregateで同じように処理できる。）
      //
      // 利用する際の注意点としては、seedを明示的に指定しない場合、暗黙的に
      // ソースシーケンスの最初の要素をseedとして利用して処理してくれることである。
      // 通常の場合はこれで良いが、出来るだけseedは明示的に渡した方がよい。
      //
      
      //
      // Sum拡張メソッドの動きをAggregateで行う.
      //
      var query   = Enumerable.Range(1, 10).Aggregate
              (
              0,        // seed
              (a, s) => a + s // func
              );
      
      Console.WriteLine("========= Sum拡張メソッドの動作 ==========");
      Console.WriteLine("SUM = [{0}]", query);
      Console.WriteLine("======================================");
      
      //
      // 独自の集計処理を行ってみる.
      //   以下は各オーダーの発注最高額とその月を求める。
      //
      Console.WriteLine("========= 独自の集計処理実行 ==========");
      
      //
      // ソースシーケンス.
      //
      var orders = new Order[]
             {
               new Order { Name = "gsf_zero1", Amount = 1000, Month = 3 },
               new Order { Name = "gsf_zero1", Amount = 600, Month = 4 },
               new Order { Name = "gsf_zero1", Amount = 100, Month = 5 },
               new Order { Name = "gsf_zero2", Amount = 100, Month = 3 },
               new Order { Name = "gsf_zero2", Amount = 1000, Month = 4 },
               new Order { Name = "gsf_zero2", Amount = 1200, Month = 5 },
               new Order { Name = "gsf_zero3", Amount = 1000, Month = 3 },
               new Order { Name = "gsf_zero3", Amount = 1200, Month = 4 },
               new Order { Name = "gsf_zero3", Amount = 900, Month = 5 },
             };
      
      //
      // 集計する前に、オーダー名単位でグループ化.
      //
      var orderGroupingQuery = from  theOrder in orders
                   group theOrder by theOrder.Name;
      
      //
      // 最高発注額を求める。
      //
      var maxOrderQuery = from orderGroup in orderGroupingQuery
                select
                  new
                  {
                    Name   = orderGroup.Key,
                    MaxOrder = orderGroup.Aggregate
                           (
                            new { MaxAmount = 0, Month = 0 },  // seed
                            (a, s) => (a.MaxAmount > s.Amount) // func
                                  ? a 
                                  : new { MaxAmount = s.Amount, Month = s.Month }
                           )
                  };
      
      foreach (var item in maxOrderQuery)
      {
        Console.WriteLine(item);
      }
      Console.WriteLine("======================================");
    }
  }
  #endregion
  
  #region LinqSamples-38
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples38 : IExecutable
  {
    public void Execute()
    {
      //
      // Range拡張メソッド.
      // この拡張メソッドは、文字通り指定された範囲の数値シーケンスを生成してくれる。
      //
      Console.WriteLine("=============== Range拡張メソッド ================");
      
      int start = 0;
      int count = 20;
      foreach (var i in Enumerable.Range(start, count).Where(item => (item % 2) == 0))
      {
        Console.WriteLine(i);
      }
      Console.WriteLine("===============================================");
      
      //
      // Repeat拡張メソッド.
      // この拡張メソッドは、文字通り指定された回数分、要素を繰り返し生成してくれる。
      //
      Console.WriteLine("=============== Repeat拡張メソッド ================");
      
      foreach (var i in Enumerable.Repeat(100, 5))
      {
        Console.WriteLine(i);
      }
      
      foreach (var s in Enumerable.Repeat("gsf_zero1", 5))
      {
        Console.WriteLine(s);
      }
      
      Console.WriteLine("===============================================");
    }
  }
  #endregion
  
  #region LinqSamples-39
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples39 : IExecutable
  {
    public void Execute()
    {
      var numbers = new int[]{ 1, 2, 3 };
      
      // 
      // Any拡張メソッドは、一つでも条件に当てはまるものが存在するか否かを判別するメソッドである。
      // この拡張メソッドは、引数無しのバージョンと引数にpredicateを渡すバージョンの２つが存在する。
      //
      // 引数を渡さずAny拡張メソッドを呼んだ場合、Any拡張メソッドは
      // 該当シーケンスに要素が存在するか否かのみで判断する。
      // つまり、要素が一つでも存在する場合は、Trueとなる。
      //
      // 引数にpredicateを指定するバージョンは、シーケンスの各要素に対してpredicateを適用し
      // 一つでも条件に合致するものが存在した時点で、Trueとなる。
      //
      Console.WriteLine("=========== 引数無しでAny拡張メソッドを利用 ===========");
      Console.WriteLine("要素有り? = {0}", numbers.Any());
      Console.WriteLine("================================================");
      
      Console.WriteLine("=========== predicateを指定してAny拡張メソッドを利用 ===========");
      Console.WriteLine("要素有り? = {0}", numbers.Any(item => item >= 5));
      Console.WriteLine("要素有り? = {0}", numbers.Any(item => item <= 5));
      Console.WriteLine("================================================================");
    }
  }
  #endregion
  
  #region LinqSamples-40
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples40 : IExecutable
  {
    public void Execute()
    {
      //
      // All拡張メソッドは、シーケンスの全要素が指定された条件に合致しているか否かを判別するメソッドである。
      //
      // 引数には条件としてpredicateを指定する。
      // このメソッドは、対象シーケンス内の全要素が条件に合致している場合のみTrueを返す。
      // (逆にAny拡張メソッドは、一つでも合致するものが存在した時点でTrueとなる。)
      //
      var names = new string[]{ "gsf_zero1", "gsf_zero2", "gsf_zero3", "2222" };
      
      Console.WriteLine("Allメソッドの結果 = {0}", names.All(item => Char.IsDigit(item.Last())));
      Console.WriteLine("Allメソッドの結果 = {0}", names.All(item => item.StartsWith("g")));
      Console.WriteLine("Allメソッドの結果 = {0}", names.All(item => !string.IsNullOrEmpty(item)));
    }
  }
  #endregion
  
  #region LinqSamples-41
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples41 : IExecutable
  {
    public void Execute()
    {
      //
      // Emptyメソッドは、文字通り空のシーケンスを作成するメソッドである。
      // Unionする際や、Aggregateする際の中間値として利用されることが多い。
      //
      Console.WriteLine("COUNT = {0}", Enumerable.Empty<string>().Count());
      
      //
      // 指定されたシーケンスから合計値が100を超えているシーケンスのみを抽出.
      // Aggregateのseed値として、空のシーケンスを渡すためにEnumerable.Emptyを
      // 使用している。
      //
      var sequences = new List<IEnumerable<int>> 
              { 
                Enumerable.Range(1, 10), 
                Enumerable.Range(30, 3), 
                Enumerable.Range(50, 2),
                Enumerable.Range(200, 1)
              };
              
      var query = 
          sequences.Aggregate(
            Enumerable.Empty<int>(),
            (current, next) => next.Sum() > 100 ? current.Union(next) : current
          );
      
      foreach (var item in query)
      {
        Console.WriteLine(item);
      }
    }
  }
  #endregion
  
  #region LinqSamples-42
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples42 : IExecutable
  {
    class Language
    {
      public string Name { get; set; }
        
      public static Language Create(string name)
      {
        return new Language { Name = name };
      }
    }
    
    class LanguageNameComparer : EqualityComparer<Language>
    {
      public override bool Equals(Language l1, Language l2)
      {
        return (l1.Name == l2.Name);
      }
      
      public override int GetHashCode(Language l)
      {
        return l.Name.GetHashCode();
      }
    }
    
    public void Execute()
    {
      //
      // Containsメソッドは、指定された要素がシーケンス内に存在するか否かを返す。
      //
      // IEqualityComparer<T>を指定するオーバーロードも存在する。
      //
      var names = new string[] { "csharp", "visualbasic", "java", "python", "ruby", "php" };
      Console.WriteLine("要素[python]は存在する? = {0}", names.Contains("python"));
      
      //
      // IEqualityComparer<T>を指定するバージョン.
      //
      var languages = new Language[] 
              { 
                Language.Create("csharp"), 
                Language.Create("visualbasic"), 
                Language.Create("java"),
                Language.Create("python"),
                Language.Create("ruby"),
                Language.Create("php")
              };
              
      Console.WriteLine(
          "要素[python]は存在する? = {0}", 
          languages.Contains(Language.Create("python"), new LanguageNameComparer())
      );
    }
  }
  #endregion
  
  #region LinqSamples-43
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples43 : IExecutable
  {
    public void Execute()
    {
      //
      // Take拡張メソッドは、シーケンスの先頭から指定された件数分を返すメソッド。
      //
      //   ・シーケンスの要素数より多い数を指定した場合、そのシーケンス全てが返る.
      //   ・0以下の値を指定した場合、空のシーケンスが返る.
      //
      var names = new string[] { "gsf_zero1", "gsf_zero2", "gsf_zero3", "gsf_zero4", "gsf_zero5" };
      
      Console.WriteLine("================ Take ======================");
      var top3 = names.Take(3);
      foreach (var item in top3)
      {
        Console.WriteLine(item);
      }
      
      foreach (var item in names.Take(20))
      {
        Console.WriteLine(item);
      }
      
      Console.WriteLine("0以下の数値を指定: COUNT={0}", names.Take(-1).Count());
      
      //
      // TakeWhile拡張メソッドは、指定された条件が満たされる間シーケンスから要素を抽出し
      // 返すメソッド。
      //
      Console.WriteLine("================ TakeWhile ======================");
      var lessThan4 = names.TakeWhile(name => int.Parse(name.Last().ToString()) <= 4);
      foreach (var item in lessThan4)
      {
        Console.WriteLine(item);
      }
    }
  }
  #endregion
  
  #region LinqSamples-44
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples44 : IExecutable
  {
    public void Execute()
    {
      //
      // Skip拡張メソッドは、シーケンスの先頭から指定された件数分をスキップするメソッド。
      //
      //   ・シーケンスの要素数より多い数を指定した場合、空のシーケンスが返る.
      //   ・0以下の値を指定した場合、シーケンスの全ての要素が返る.
      //
      var names = new string[] { "gsf_zero1", "gsf_zero2", "gsf_zero3", "gsf_zero4", "gsf_zero5" };
      
      Console.WriteLine("================ Skip ===========================");
      var last2Elements = names.Skip(3);
      foreach (var item in last2Elements)
      {
        Console.WriteLine(item);
      }
      
      Console.WriteLine("シーケンスの要素数以上の数を指定: COUNT={0}", names.Skip(20).Count());
      
      foreach (var item in names.Skip(-1))
      {
        Console.WriteLine(item);
      }
      
      //
      // SkipWhile拡張メソッドは、指定された条件が満たされる間シーケンスから要素を抽出し
      // 返すメソッド。
      //
      Console.WriteLine("================ SkipWhile ======================");
      var greaterThan4 = names.SkipWhile(name => int.Parse(name.Last().ToString()) <= 3);
      foreach (var item in greaterThan4)
      {
        Console.WriteLine(item);
      }
    }
  }
  #endregion
  
  #region LinqSamples-45
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples45 : IExecutable
  {
    public void Execute()
    {
      var languages = new string[] { "csharp", "visualbasic", "java", "python", "ruby", "php", "c++" };
      
      //
      // First拡張メソッドは、シーケンスの最初の要素を返すメソッド。
      //
      // predicateを指定した場合は、その条件に合致する最初の要素が返る。
      //
      Console.WriteLine("============ First ============");
      Console.WriteLine(languages.First());
      Console.WriteLine(languages.First(item => item.StartsWith("v")));
      
      //
      // Last拡張メソッドは、シーケンスの最後の要素を返すメソッド。
      //
      // predicateを指定した場合は、その条件に合致する最後の要素が返る。
      //
      Console.WriteLine("============ Last ============");
      Console.WriteLine(languages.Last());
      Console.WriteLine(languages.Last(item => item.StartsWith("p")));
      
      //
      // Single拡張メソッドは、シーケンスの唯一の要素を返すメソッド。
      //
      // Singleを利用する場合、対象となるシーケンスには要素が一つだけ
      // でないといけない。複数の要素が存在する場合例外が発生する。
      //
      // また、空のシーケンスに対してSingleを呼ぶと例外が発生する。
      //
      // predicateを指定した場合は、その条件に合致するシーケンスの唯一の
      // 要素が返される。この場合も、結果のシーケンスには要素が一つだけの
      // 状態である必要がある。条件に合致する要素が複数であると例外が発生する、
      //
      Console.WriteLine("============ Single ============");
      var onlyOne = new string[] { "csharp" };      
      Console.WriteLine(onlyOne.Single());
      
      try
      {
        languages.Single();
      }
      catch
      {
        Console.WriteLine("複数の要素が存在する状態でSingleを呼ぶと例外が発生する。");
      }
      
      Console.WriteLine(languages.Single(item => item.EndsWith("y")));
      
      try
      {
        languages.Single(item => item.StartsWith("p"));
      }
      catch
      {
        Console.WriteLine("条件に合致する要素が複数存在する場合例外が発生する。");
      }
    }
  }
  #endregion
  
  #region LinqSamples-46
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples46 : IExecutable
  {
    public void Execute()
    {
      //
      // FirstOrDefault拡張メソッドは、First拡張メソッドと同じ動作をする。
      // 違いは、シーケンスに要素が存在しない場合に規定値を返す点である。
      //
      var emptySequence = Enumerable.Empty<string>();
      var languages   = new string[] { "csharp", "visualbasic", "java", "python", "ruby", "php", "c++" };
      
      try
      {
        // First拡張メソッドは要素が存在しない場合例外が発生する.
        emptySequence.First();
      }
      catch
      {
        Console.WriteLine("First拡張メソッドで例外発生");
      }
      
      Console.WriteLine("FirstOrDefaultの場合: {0}",      emptySequence.FirstOrDefault() ?? "null");
      Console.WriteLine("FirstOrDefaultの場合(predicate): {0}", languages.FirstOrDefault(item => item.EndsWith("z")) ?? "null");
      
      //
      // LastOrDefault拡張メソッドは、Last拡張メソッドと同じ動作をする。
      // 違いは、シーケンスに要素が存在しない場合に規定値を返す点である。
      //
      try
      {
        // Last拡張メソッドは要素が存在しない場合例外が発生する.
        emptySequence.Last();
      }
      catch
      {
        Console.WriteLine("Last拡張メソッドで例外発生");
      }
      
      Console.WriteLine("LastOrDefaultの場合: {0}",      emptySequence.LastOrDefault() ?? "null");
      Console.WriteLine("LastOrDefaultの場合(predicate): {0}", languages.LastOrDefault(item => item.EndsWith("z")) ?? "null");
      
      //
      // SingleOrDefault拡張メソッドは、Single拡張メソッドと同じ動作をする。
      // 違いは、シーケンスに要素が存在しない場合に規定値を返す点である。
      //
      try
      {
        // Last拡張メソッドは要素が存在しない場合例外が発生する.
        emptySequence.Single();
      }
      catch
      {
        Console.WriteLine("Single拡張メソッドで例外発生");
      }
      
      Console.WriteLine("SingleOrDefaultの場合: {0}",      emptySequence.SingleOrDefault() ?? "null");
      Console.WriteLine("SingleOrDefaultの場合(predicate): {0}", languages.SingleOrDefault(item => item.EndsWith("z")) ?? "null");
      
      //
      // DefaultIfEmpty拡張メソッドは、シーケンスが空の場合に規定値を返すメソッド。
      //
      // シーケンスに要素が存在する場合は、そのままの状態で返す。
      // LINQにて外部結合を行う際に必須となるメソッド。
      //
      Console.WriteLine("================ DefaultIfEmpty ====================");
      
      var emptyIntegers = Enumerable.Empty<int>();
      foreach (var item in emptyIntegers.DefaultIfEmpty())
      {
        Console.WriteLine("基本型の場合: {0}", item);
      }
      
      foreach (var item in emptySequence.DefaultIfEmpty())
      {
        Console.WriteLine("参照型の場合: {0}", item ?? "null");
      }
      
      foreach (var item in languages.DefaultIfEmpty())
      {
        Console.WriteLine(item ?? "null");
      }
      
      foreach (var item in emptySequence.DefaultIfEmpty("デフォルト値"))
      {
        Console.WriteLine(item ?? "null");
      }
    }
  }
  #endregion
  
  #region LinqSample-47
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples47 : IExecutable
  {
    public void Execute()
    {
      //
      // ElementAt拡張メソッドは、指定した位置の要素を取得するメソッド。
      //
      // 範囲外のインデックスを指定した場合は例外が発生する.
      //
      var languages = new string[] { "csharp", "visualbasic", "java", "python", "ruby", "php", "c++" };
      Console.WriteLine(languages.ElementAt(1));
      
      try
      {
        languages.ElementAt(100);
      }
      catch(ArgumentOutOfRangeException)
      {
        Console.WriteLine("要素の範囲外のインデックスを指定している。");
      }
      
      //
      // ElementAtOrDefault拡張メソッドは、ElementAt拡張メソッドと同じ動作を
      // しながら、範囲外のインデックスを指定された場合に規定値を返すメソッド。
      //
      Console.WriteLine(languages.ElementAtOrDefault(-1)  ?? "null");
      Console.WriteLine(languages.ElementAtOrDefault(100) ?? "null");
    }
  }
  #endregion
  
  #region LinqSamples-48
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples48 : IExecutable
  {
    class Language
    {
      public string Name { get; set; }
        
      public static Language Create(string name)
      {
        return new Language { Name = name };
      }
    }
    
    class LanguageNameComparer : EqualityComparer<Language>
    {
      public override bool Equals(Language l1, Language l2)
      {
        return (l1.Name == l2.Name);
      }
      
      public override int GetHashCode(Language l)
      {
        return l.Name.GetHashCode();
      }
    }

    public void Execute()
    {
      //
      // SequenceEqual拡張メソッドは、2つのシーケンスが等しいか否かを判別するメソッド。
      //
      // IEqualityComparerを指定できるオーバーロードが存在する.
      //
      var numbers1 = Enumerable.Range(1, 10);
      var numbers2 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      
      Console.WriteLine("numbers1 eq numbers2 == {0}", numbers1.SequenceEqual(numbers2));
      
      //
      // IEqualityComparer<T>を指定するバージョン.
      //
      var languages1 = new Language[]
               { 
                 Language.Create("csharp"), 
                 Language.Create("visualbasic"), 
                 Language.Create("java"),
                 Language.Create("python"),
                 Language.Create("ruby"),
                 Language.Create("php")
               };

      var languages2 = new Language[]
               { 
                 Language.Create("csharp"), 
                 Language.Create("visualbasic"), 
                 Language.Create("java"),
                 Language.Create("python"),
                 Language.Create("ruby"),
                 Language.Create("php")
               };

      var languages3 = new Language[]
               { 
                 Language.Create("csharp"), 
                 Language.Create("visualbasic"), 
                 Language.Create("java"),
               };
               
      var comparer = new LanguageNameComparer();
      
      Console.WriteLine(
          "languages1 eq languages2 == {0}", 
          languages1.SequenceEqual(languages2, comparer)
      );
      
      Console.WriteLine(
          "languages1 eq languages3 == {0}", 
          languages1.SequenceEqual(languages3, comparer)
      );
    }
  }
  #endregion
  
  #region LinqSamples-49
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples49 : IExecutable
  {
    public void Execute()
    {
      //
      // Directory.EnumerateFilesメソッドは、従来までの
      // Directory.GetFilesメソッドと同じ動作するメソッドである。
      //
      // 違いは、戻り値がIEnumerable<string>となっており
      // 遅延評価される。
      //
      // GetFilesメソッドの場合は、全リストを構築してから
      // 戻り値が返却されるので、コレクションが構築されるまで
      // 待機する必要があるが、EnumerateFilesメソッドの場合は
      // コレクション全体が返される前に、列挙可能である。
      //
      // EnumerateDirectoriesメソッド及びEnumerateFileSystemEntriesメソッドも上記と同様。
      //
      var path  = @"c:\windows";
      var filter  = @"*.exe";
      var watch   = Stopwatch.StartNew();
      var elapsed = string.Empty;

      //
      // EnumerateFiles.
      //
      var query = from   file in Directory.EnumerateFiles(path, filter, SearchOption.AllDirectories)
            select file;
      
      foreach (var item in query)
      {
        if (watch != null)
        {
          watch.Stop();
          elapsed = watch.Elapsed.ToString();
          watch = null;
        }
        
        //Console.WriteLine(item);
      }
      
      Console.WriteLine("================== EnumereteFiles       : {0} ==================", elapsed);
      
      //
      // EnumerateDirectories.
      //
      watch   = Stopwatch.StartNew();
      elapsed = string.Empty;
      
      query = from   directory in Directory.EnumerateDirectories(path)
          select directory;
      
      foreach (var item in query)
      {
        if (watch != null)
        {
          watch.Stop();
          elapsed = watch.Elapsed.ToString();
          watch = null;
        }
        
        //Console.WriteLine(item);
      }
      
      Console.WriteLine("================== EnumerateDirectories     : {0} ==================", elapsed);
      
      //
      // EnumerateFileSystemEntries.
      //
      watch   = Stopwatch.StartNew();
      elapsed = string.Empty;
      
      query = from   directory in Directory.EnumerateFileSystemEntries(path)
          select directory;
      
      foreach (var item in query)
      {
        if (watch != null)
        {
          watch.Stop();
          elapsed = watch.Elapsed.ToString();
          watch = null;
        }
        
        //Console.WriteLine(item);
      }
      
      Console.WriteLine("================== EnumerateFileSystemEntries : {0} ==================", elapsed);

      //
      // GetFiles.
      //
      watch   = Stopwatch.StartNew();
      elapsed = string.Empty;
      
      var files = Directory.GetFiles(path, filter, SearchOption.AllDirectories);
      
      foreach (var item in files)
      {
        if (watch != null)
        {
          watch.Stop();
          elapsed = watch.Elapsed.ToString();
          watch = null;
        }
        
        //Console.WriteLine(item);
      }
      
      Console.WriteLine("================== GetFiles           : {0} ==================", elapsed);

      //
      // GetDirectories.
      //
      watch   = Stopwatch.StartNew();
      elapsed = string.Empty;
      
      var dirs = Directory.GetDirectories(path);
      
      foreach (var item in dirs)
      {
        if (watch != null)
        {
          watch.Stop();
          elapsed = watch.Elapsed.ToString();
          watch = null;
        }
        
        //Console.WriteLine(item);
      }
      
      Console.WriteLine("================== GetDirectories       : {0} ==================", elapsed);

      //
      // GetFileSystemEntries.
      //
      watch   = Stopwatch.StartNew();
      elapsed = string.Empty;
      
      var entries = Directory.GetFileSystemEntries(path);
      
      foreach (var item in entries)
      {
        if (watch != null)
        {
          watch.Stop();
          elapsed = watch.Elapsed.ToString();
          watch = null;
        }
        
        //Console.WriteLine(item);
      }
      
      Console.WriteLine("================== GetFileSystemEntries     : {0} ==================", elapsed);
    }
  }
  #endregion
  
  #region LinqSamples-50
  /// <summary>
  /// Linqのサンプルです。
  /// </summary>
  public class LinqSamples50 : IExecutable
  {
    public void Execute()
    {
      //
      // File.ReadLinesメソッドは、従来までの
      // File.ReadAllLinesメソッドと同じ動作するメソッドである。
      //
      // 違いは、戻り値がIEnumerable<string>となっており
      // 遅延評価される。
      //
      // ReadAllLinesメソッドの場合は、全リストを構築してから
      // 戻り値が返却されるので、コレクションが構築されるまで
      // 待機する必要があるが、ReadLinesメソッドの場合は
      // コレクション全体が返される前に、列挙可能である。
      //
      Console.WriteLine("ファイル作成中・・・・");
      
      var tmpFilePath = CreateSampleFile(1000000);
      if (string.IsNullOrEmpty(tmpFilePath))
      {
        Console.WriteLine("ファイル作成中にエラー発生");
      }
      
      Console.WriteLine("ファイル作成完了");
      
      try
      {
        var watch   = Stopwatch.StartNew();
        var elapsed = string.Empty;
        
        var numberFormatInfo = new NumberFormatInfo { CurrencySymbol = "gsf_zero" };
        
        //
        // File.ReadAllLines
        //
        var query = from   line in File.ReadAllLines(tmpFilePath)
              where  int.Parse(line, NumberStyles.AllowCurrencySymbol, numberFormatInfo) % 2 == 0
              select line;
        
        foreach (var element in query)
        {
          if (watch != null)
          {
            watch.Stop();
            elapsed = watch.Elapsed.ToString();
            watch = null;
          }
          
          //Console.WriteLine(element);
        }
        
        Console.WriteLine("================== ReadAllLines      : {0} ==================", elapsed);
        
        //
        // File.ReadLines
        //
        watch   = Stopwatch.StartNew();
        elapsed = string.Empty;
        
        query = from   line in File.ReadLines(tmpFilePath)
            where  int.Parse(line, NumberStyles.AllowCurrencySymbol, numberFormatInfo) % 2 == 0
            select line;
        
        foreach (var element in query)
        {
          if (watch != null)
          {
            watch.Stop();
            elapsed = watch.Elapsed.ToString();
            watch = null;
          }
          
          //Console.WriteLine(element);
        }
        
        Console.WriteLine("================== ReadLines       : {0} ==================", elapsed);
      }
      finally
      {
        if (File.Exists(tmpFilePath))
        {
          File.Delete(tmpFilePath);
        }
      }
    }
    
    string CreateSampleFile(int lineCount)
    {
      var tmpFileName = Path.GetTempFileName();
      
      try
      {
        //
        // 巨大なファイルを作成する.
        //
        using (var writer = new StreamWriter(new BufferedStream(File.OpenWrite(tmpFileName))))
        {
          for (int i = 0; i < lineCount; i++)
          {
            writer.WriteLine(string.Format("gsf_zero{0}", i));
          }
          
          writer.Flush();
          writer.Close();
        }
      }
      catch
      {
        if (File.Exists(tmpFileName))
        {
          File.Delete(tmpFileName);
        }
        
        return string.Empty;
      }

      return tmpFileName;
    }
  }
  #endregion
  
  #region LinqSamples-51
  /// <summary>
  /// LINQ to XMLのサンプルです。
  /// </summary>
  /// <remarks>
  /// 文字列からXDocumentオブジェクトを構築するサンプルです。
  /// </remarks>
  public class LinqSamples51 : IExecutable
  {
    public void Execute()
    {
      //
      // LINQ to XMLは、LINQを利用してXMLを扱うためのAPIである。
      // インメモリXMLノードの管理及びクエリ実行が可能。
      //
      // LINQ to XMLでは、まず最初にXDocumentオブジェクトまたはXElementを構築する.
      // XDocumentオブジェクトの構築には
      //   ・ファイルから読み込み
      //   ・文字列から構築
      //   ・関数型構築
      //   ・新規作成
      // の方法がある。
      //
      // 本サンプルでは、文字列からXDocumentオブジェクトを取得している.
      //

      //
      // 文字列からXDocumentを構築するには、Parseメソッドを利用する.
      // LoadOptionには、以下の値が設定出来る.
      //   None              : 意味の無い空白の削除、及び、ベースURIと行情報の読み込み無し
      //   PreserveWhitespace: 意味の無い空白保持
      //   SetBaseUri        : ベースURIの保持
      //   SetLineInfo       : 行情報の保持
      //
      var doc = XDocument.Parse(MakeSampleXml(), LoadOptions.None);
      
      //
      // 特定の要素の表示.
      //
      var query = from   elem in doc.Descendants("Person")
                  let    name = elem.Attribute("name").Value
                  where  name.StartsWith("b")
                  select new { name };
      
      foreach (var item in query)
      {
        Console.WriteLine(item);
      }
    }
    
    string MakeSampleXml()
    {
      var xmlStrings = 
            @"<?xml version='1.0' encoding='UTF-8' standalone='yes'?>
              <Persons>
                <Person name='foo'/>
                <Person name='bar'/>
                <Person name='baz'/>
              </Persons>";
      
      return xmlStrings;
    }
  }
  #endregion
  
  #region LinqSamples-52
  /// <summary>
  /// LINQ to XMLのサンプルです。
  /// </summary>
  /// <remarks>
  /// XDocumentオブジェクトを関数型構築するサンプルです。
  /// </remarks>
  public class LinqSamples52 : IExecutable
  {
    public void Execute()
    {
      //
      // XDocumentは複数のコンストラクタを持っているが
      // 以下を利用すると、関数型構築が行える.
      // 関数型構築とは、単一のステートメントでXMLツリーを作成するための機能である。
      // 
      // public XDocument(object[])
      // public XDocument(XDeclaration, object[])
      //
      // XDocumentを起点として関数型構築を行う場合
      // ルート要素となるXElementを作成し、その子要素に
      // 様々な要素を設定する.
      //
      // 例：
      // var doc = new XDocument
      //           (
      //             new XElement
      //             (
      //               "RootElement",
      //               new XElement("ChildElement", "ChildValue1"),
      //               new XElement("ChildElement", "ChildValue2"),
      //               new XElement("ChildElement", "ChildValue3")
      //             )
      //           );
      //
      // 上記例は、以下のXMLツリーを構築する.
      // <RootElement>
      //   <ChildElement>ChildValue1</ChildElement>
      //   <ChildElement>ChildValue2</ChildElement>
      //   <ChildElement>ChildValue3</ChildElement>
      // </RootElement>
      //
      var doc = MakeDocument();
      Console.WriteLine(doc);
    }
    
    XDocument MakeDocument()
    {
      var doc   = new XDocument
                  (
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Persons", MakePersonElements())
                  );
    
      return doc;
    }
    
    IEnumerable<XElement> MakePersonElements()
    {
      var names = new []{ "foo", "bar", "baz" };
      var query = from   name in names
                  select new XElement
                         (
                           "Person",
                           new XAttribute("name", name),
                           string.Format("VALUE-{0}", name)
                         );
      
      return query;
    }
  }
  #endregion
  
  #region LinqSamples-53
  /// <summary>
  /// LINQ to XMLのサンプルです。
  /// </summary>
  /// <remarks>
  /// XElement.Loadを利用した読み込みのサンプルです。
  /// </remarks>
  public class LinqSamples53 : IExecutable
  {
    const string FILE_URI     = @"LinqSamples53_Sample.xml";
    const string DOWNLOAD_URI = @"https://sites.google.com/site/gsfzero1/Home/Books.xml?attredirects=0&d=1";
    
    public void Execute()
    {
      //
      // XElementもXDocumentもParseメソッドの他に
      // 自身を構築するためのLoadメソッドを持っている.
      //
      // Parseメソッドは、文字列を解析して構築する時に利用し
      // Loadメソッドは、既に存在しているものを読み込む際に利用する.
      //
      // Loadメソッドは、複数のオーバーロードを持ち
      //   ・URIを指定
      //   ・ストリームを指定
      // に大別される.
      //
      // 本サンプルでは、URIによる読み込みを記述する.
      // URIを指定するLoadメソッドのオーバーロードは以下の通り。
      //
      //   public XDocument Load(string)
      //   public XDocument Load(string, LoadOptions)
      //   public XElement  Load(string)
      //   public XElement  Load(string, LoadOptions)
      //
      CreateSampleXml();
      
      var rootElement = XElement.Load(FILE_URI);
      var query       = from   element in rootElement.Descendants("Person")
                        let    name = element.Attribute("name").Value
                        where  !name.StartsWith("b")
                        select new { name };
      
      foreach (var item in query)
      {
        Console.WriteLine(item);
      }
      
      //
      // URLからXMLを読み込み
      //   XMLファイルは以下のサンプルを利用させてもらっている。
      //     http://msdn.microsoft.com/ja-jp/library/vstudio/bb387066.aspx
      //
      Console.WriteLine(XElement.Load(DOWNLOAD_URI));
    }
    
    void CreateSampleXml()
    {
      if (File.Exists(FILE_URI))
      {
        File.Delete(FILE_URI);
      }
      
      var doc = MakeDocument();
      doc.Save(FILE_URI);
    }
    
    XDocument MakeDocument()
    {
      var doc   = new XDocument
                  (
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Persons", MakePersonElements())
                  );
    
      return doc;
    }
    
    IEnumerable<XElement> MakePersonElements()
    {
      var names = new []{ "foo", "bar", "baz" };
      var query = from   name in names
                  select new XElement
                         (
                           "Person",
                           new XAttribute("name", name),
                           string.Format("VALUE-{0}", name)
                         );
      
      return query;
    }
  }
  #endregion
  
  #region LinqSamples-54
  /// <summary>
  /// LINQ to XMLのサンプルです。
  /// </summary>
  /// <remarks>
  /// XElement.Loadを利用したストリーム読み込みのサンプルです.
  /// </remarks>
  public class LinqSamples54 : IExecutable
  {
    public void Execute()
    {
      //
      // XElement.Loadには、文字列からロードする他に
      // ストリーミングを指定して内容をロードすることもできる。
      //
      // メソッドは複数のオーバーロードを持っており、以下の書式となる.
      //   Load(Stream)
      //   Load(TextReader)
      //   Load(XmlReader)
      //   Load(Stream, LoadOptions)
      //   Load(TextReader, LoadOptions)
      //   Load(XmlReader, LoadOptions)
      // 大別すると、ストリームのみを指定するものとオプションも指定できるものに分かれる.
      //
      
      //
      // Load(Stream)のサンプル.
      //   -- File.OpenReadで返るのはFileStream
      //      FileStreamはStreamのサブクラス.
      //
      XElement element = null;
      using (var stream = File.OpenRead("xml/Books.xml"))
      {
        element = XElement.Load(stream);
      }
      
      Console.WriteLine(element);
      Console.WriteLine("=============================================");
      
      //
      // Load(TextReader)のサンプル
      //   -- StreamReaderはTextReaderのサブクラス.
      //
      element = null;
      using (var reader = new StreamReader("xml/Data.xml"))
      {
        element = XElement.Load(reader);
      }
      
      Console.WriteLine(element);
      Console.WriteLine("=============================================");
      
      //
      // Load(XmlReader)のサンプル.
      //
      element = null;
      using (var reader = XmlReader.Create("xml/PurchaseOrder.xml", new XmlReaderSettings { IgnoreWhitespace = true, IgnoreComments = true }))
      {
        element = XElement.Load(reader);
      }
      
      Console.WriteLine(element);
    }
  }
  #endregion
  
  #region LinqSamples-55
  /// <summary>
  /// LINQ to XMLのサンプルです。
  /// </summary>
  /// <remarks>
  /// LINQ to XMLにてエラー発生時XmlExceptionが発生することを確認するサンプルです。
  /// </remarks>
  public class LinqSamples55 : IExecutable
  {
    public void Execute()
    {
      try
      {
        //
        // LINQ to XMLは内部でXmlReaderを利用している.
        // なので、エラーが発生した場合、XmlReaderの場合と
        // 同様にXmlExceptionが発生する.
        //
        XElement.Parse(GetXmlStrings());
      }
      catch(XmlException xmlEx)
      {
        Console.WriteLine(xmlEx.ToString());
      }
    }
    
    string GetXmlStrings()
    {
      //
      // わざと解析エラーになるXML文字列を作成.
      //
      return @"<data>
                 <id>1</id>
                 <id>2</id>
               </dat>";
    }
  }
  #endregion
  
  #region LinqSamples-56
  /// <summary>
  /// LINQ to XMLのサンプルです。
  /// </summary>
  /// <remarks>
  /// LINQ to XMLにてXMLファイルを新規作成するサンプルです.
  /// </remarks>
  public class LinqSamples56 : IExecutable
  {
    public void Execute()
    {
      //
      // LINQ to XMLにてXMLを新規作成するには
      // 以下のどちらかのインスタンスを作成する必要がある.
      //   ・XDocument
      //   ・XElement
      // 通常、よく利用されるのはXElementの方となる.
      // 保存を行うには、Saveメソッドを利用する.
      // Saveメソッドには、以下のオーバーロードが存在する. (XElement)
      //   Save(Stream)
      //   Save(String)
      //   Save(TextWriter)
      //   Save(XmlWriter)
      //   Save(Stream, SaveOptions)
      //   Save(String, SaveOptions)
      //   Save(TextWriter, SaveOptions)
      //
      var element = new XElement("RootNode",
                          from   i in Enumerable.Range(1, 10)
                          select new XElement("Child", i)
                    );
      
      //
      // Save(Stream)
      //
      using (var stream = new MemoryStream())
      {
        element.Save(stream);
        
        stream.Position = 0;
        using (var reader = new StreamReader(stream))
        {
          Console.WriteLine(reader.ReadToEnd());
        }
      }
      
      Console.WriteLine("===================================");
      
      //
      // Save(String)
      //
      var tmpFile = Path.GetRandomFileName();
      element.Save(tmpFile);
      Console.WriteLine(File.ReadAllText(tmpFile));
      File.Delete(tmpFile);
      
      Console.WriteLine("===================================");
      
      //
      // Save(TextWriter)
      //
      using (var writer = new UTF8StringWriter())
      {
        element.Save(writer);
        Console.WriteLine(writer);
      }
      
      Console.WriteLine("===================================");
      
      //
      // Save(XmlWriter)
      //
      using (var backingStore = new UTF8StringWriter())
      {
        using (var xmlWriter = XmlWriter.Create(backingStore, new XmlWriterSettings { Indent = true }))
        {
          element.Save(xmlWriter);
        }
        
        Console.WriteLine(backingStore);
      }
      
      Console.WriteLine("===================================");
      
      //
      // SaveOptions付きで書き込み.
      //   DisableFormattingを指定すると、出力されるXMLに書式が設定されなくなる.
      //
      using (var writer = new UTF8StringWriter())
      {
        element.Save(writer, SaveOptions.DisableFormatting);
        Console.WriteLine(writer);
      }
    }
    
    class UTF8StringWriter : StringWriter
    {
      public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
  }
  #endregion
  
  #region LinqSamples-57
  /// <summary>
  /// LINQ to XMLのサンプルです。
  /// </summary>
  /// <remarks>
  /// LINQ to XMLにてクエリを使用して対象の要素を取得するサンプルです。
  /// </remarks>
  public class LinqSamples57 : IExecutable
  {
    public void Execute()
    {
      //
      // LINQ to XMLでは、クエリを利用して特定の要素や属性などを取得する。
      // 取得方法はいろいろあるが、今回はElementsメソッドとElementメソッドを用いて要素の取得を行っている.
      //
      //
      // Books.xmlは、ルート要素がCataglogで内部に複数のBook要素を持っている.
      // 各Book要素は、一つのAuthor要素を持っている.
      //
      // Elementsメソッドは、引数に指定された要素名に合致する要素の集合を返す.
      // Elementメソッドは、引数に指定された要素名に合致する最初の要素を返す.
      //
      var root  = XElement.Load(@"xml/Books.xml");
      var query = from   book in root.Elements("Book")
                  select book.Element("Author");
      
      foreach (var author in query)
      {
        Console.WriteLine(author);
      }
    }
  }
  #endregion
  
  #region LinqSamples-58
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// 要素のクローンとアタッチについてのサンプルです.
  /// </remarks>
  public class LinqSamples58 : IExecutable
  {
    public void Execute()
    {
      //
      // 親要素を持たない要素を作成し、特定のXMLツリーの中に組み込む. (アタッチ)
      //
      var noParent = new XElement("NoParent", true);
      var tree1    = new XElement("Parent", noParent);
      
      var noParent2 = tree1.Element("NoParent");
      Console.WriteLine("参照が同じ？ = {0}", noParent == noParent2);
      Console.WriteLine(tree1);
      
      // 値を変更して確認.
      noParent.SetValue(false);
      Console.WriteLine(noParent.Value);
      Console.WriteLine(tree1.Element("NoParent").Value);
      
      Console.WriteLine("==========================================");
      
      //
      // 親要素を持つ要素を作成し、特定のXMLツリーの中に組み込む. (クローン)
      //
      var origTree = new XElement("Parent", new XElement("WithParent", true));
      var tree2    = new XElement("Parent", origTree.Element("WithParent"));
      
      Console.WriteLine("参照が同じ？ = {0}", origTree.Element("WithParent") == tree2.Element("WithParent"));
      Console.WriteLine(tree2);
      
      // 値を変更して確認
      origTree.Element("WithParent").SetValue(false);
      Console.WriteLine(origTree.Element("WithParent").Value);
      Console.WriteLine(tree2.Element("WithParent").Value);
    }
  }
  #endregion
  
  #region LinqSamples-59
  /// <summary>
  /// Linqにて、シーケンスをチャンクに分割して処理するサンプルです.
  /// </summary>
  public class LinqSamples59 : IExecutable
  {
    public void Execute()
    {
      //
      // 要素が10のシーケンスを2つずつのチャンクに分割.
      //
      foreach (var chunk in Enumerable.Range(1, 10).Chunk(2))
      {
        Console.WriteLine("Chunk:");
        foreach (var item in chunk)
        {
          Console.WriteLine("\t--> {0}", item);
        }
      }
      
      //
      // 要素が10000のシーケンスを1000ずつのチャンクに分割し
      // それぞれのチャンクごとにインデックスを付与.
      //
      foreach (var chunk in Enumerable.Range(1, 10000).Chunk(1000).Select((x, i) => new { Index = i, Count = x.Count() }))
      {
        Console.WriteLine(chunk);
      }
    }
  }
  
  public static class LinqSamples59_Extensions
  {
    /// <summary>
    /// シーケンスを指定されたサイズのチャンクに分割します.
    /// </summary>
    public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> self, int chunkSize)
    {
      if (chunkSize <= 0)
      {
        throw new ArgumentException("Chunk size must be greater than 0.", "chunkSize");
      }
      
      while (self.Any())
      {
        yield return self.Take(chunkSize);
        self = self.Skip(chunkSize);
      }
    }
  }
  #endregion
  
  #region LinqSamples-60
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// 要素追加系メソッドのサンプルです.
  /// </remarks>
  public class LinqSamples60 : IExecutable
  {
    public void Execute()
    {
      //
      // Add(object)
      //   名前の通り、現在の要素に指定された要素を追加する.
      //   追加される位置は、その要素の末尾となる
      //
      var root     = BuildSampleXml();
      var newElem1 = new XElement("NewElement", "hehe");
      root.Add(newElem1);
      
      Console.WriteLine(root);
      Console.WriteLine("=====================================");
      
      //
      // AddAfterSelf(object)
      //   現在の要素の後ろに、指定された要素を追加する.
      //   追加される位置は、自分自身の中ではなく外となる事に注意.
      //   尚、ルート要素に対して本メソッドを呼び出すと親が存在しないので
      //   InvalidOperationExceptionが発生する.
      //   (発生する例外がXmlExceptionでは無いことに注意)
      //
      root = BuildSampleXml();
      var newElem4 = new XElement("AfterElement", "AfterSelf");
      
      try
      {
        // ルート要素に対して、自身の後ろに要素を追加しようとするので
        // エラーとなる。XmlExceptionでは無いことに注意.
        root.AddAfterSelf(newElem4);
        Console.WriteLine(root);
      }
      catch (InvalidOperationException invalidEx)
      {
        Console.WriteLine("[ERROR] {0}", invalidEx.Message);
      }
      finally
      {
        Console.WriteLine("=====================================");
      }
      
      root.Elements().First().AddAfterSelf(newElem4);
      
      Console.WriteLine(root);
      Console.WriteLine("=====================================");

      //
      // AddBeforeSelf(object)
      //   現在の要素の前に、指定された要素を追加する.
      //   追加される位置は、自分自身の中ではなく外となる事に注意.
      //   尚、ルート要素に対して本メソッドを呼び出すと親が存在しないので
      //   InvalidOperationExceptionが発生する.
      //   (発生する例外がXmlExceptionでは無いことに注意)
      //
      root = BuildSampleXml();
      var newElem5 = new XElement("BeforeElement", "BeforeSelf");
      
      try
      {
        // ルート要素に対して、自身の前に要素を追加しようとするので
        // エラーとなる。XmlExceptionでは無いことに注意.
        root.AddBeforeSelf(newElem5);
        Console.WriteLine(root);
      }
      catch (InvalidOperationException invalidEx)
      {
        Console.WriteLine("[ERROR] {0}", invalidEx.Message);
      }
      finally
      {
        Console.WriteLine("=====================================");
      }
      
      root.Elements().First().AddBeforeSelf(newElem5);
      
      Console.WriteLine(root);
      Console.WriteLine("=====================================");
      
      //
      // AddFirst(object)
      //   現在要素の先頭子要素として、指定された要素を追加する.
      //   追加される位置は、自分自身の中となる。（先頭要素）
      //
      root = BuildSampleXml();
      var newElem6 = new XElement("FirstElement", "First");
      
      root.AddFirst(newElem6);
      
      Console.WriteLine(root);
      Console.WriteLine("=====================================");
      
      root = BuildSampleXml();
      root.Elements().First().AddFirst(newElem6);
      
      Console.WriteLine(root);
      Console.WriteLine("=====================================");
    }
    
    XElement BuildSampleXml()
    {
      return new XElement("Root",
                   new XElement("Child", 
                     new XAttribute("Id", 100),
                     new XElement("Value", "hoge")
                   )
                 );
    }
  }
  #endregion
  
  #region LinqSamples-61
  /// <summary>
  /// LINQ to XMLのサンプルです.
  /// </summary>
  /// <remarks>
  /// 要素更新系メソッドのサンプルです.
  /// </remarks>
  public class LinqSamples61 : IExecutable
  {
    public void Execute()
    {
      //
      // Value { get; set; } プロパティ
      //   対象の要素の値を取得・設定する.
      //   型がstringとなっているので、文字列に変換して設定する必要がある。
      //   nullを指定すると例外が発生する。(ArgumentNullException)
      //
      var root = BuildSampleXml();
      var elem = root.Descendants("Value").First();
      
      Console.WriteLine("[before] {0}", elem.Value);
      elem.Value = "updated";
      Console.WriteLine("[after] {0}", elem.Value);
      Console.WriteLine(root);
      
      try
      {
        elem.Value = null;
      }
      catch (ArgumentNullException argNullEx)
      {
        Console.WriteLine(argNullEx.Message);
      }
      
      // Valueプロパティはstringを受け付けるので、boolなどの場合は
      // 明示的に文字列にして設定する必要がある.
      elem.Value = bool.TrueString.ToLower();
      Console.WriteLine(root);
      
      Console.WriteLine("=====================================");
      
      //
      // SetValue(object)
      //   要素の値を設定する。
      //   型がobjectとなっているので、文字列以外の場合でもそのまま設定可能。
      //   内部で変換される.
      //   nullを指定すると例外が発生する。(ArgumentNullException)
      //
      root = BuildSampleXml();
      elem = root.Descendants("Value").First();
      
      Console.WriteLine("[before] {0}", elem.Value);
      elem.SetValue("updated");
      Console.WriteLine("[after] {0}", elem.Value);
      Console.WriteLine(root);
      
      try
      {
        elem.SetValue(null);
      }
      catch (ArgumentNullException argNullEx)
      {
        Console.WriteLine(argNullEx.Message);
      }
      
      // SetValueメソッドは、object型を受け付けるので
      // bool型などの場合でもそのまま設定できる。内部で変換される.
      elem.SetValue(true);
      Console.WriteLine(root);
      
      Console.WriteLine("=====================================");
      
      //
      // SetElementValue(XName, object)
      //   子要素の値を設定する。
      //     要素が存在しない場合： 新規追加
      //     要素が存在する場合： 更新
      //     nullを設定した場合： 削除
      //   となる。自分自身の値を設定するわけでは無いことに注意。
      //
      root = BuildSampleXml();
      elem = root.Elements("Child").First();
      
      // 要素が存在する場合: 更新
      elem.SetElementValue("Value", "updated");
      Console.WriteLine(root);
      
      // nullを指定した場合： 削除
      root = BuildSampleXml();
      elem = root.Elements("Child").First();
      elem.SetElementValue("Value", null);
      Console.WriteLine(root);
      
      // 要素が存在しない場合: 新規追加
      root = BuildSampleXml();
      elem = root.Elements("Child").First();
      elem.SetElementValue("Value2", "inserted");
      Console.WriteLine(root);
      
      Console.WriteLine("=====================================");
    }
    
    XElement BuildSampleXml()
    {
      return new XElement("Root",
                   new XElement("Child", 
                     new XAttribute("Id", 100),
                     new XElement("Value", "hoge")
                   )
                 );
    }
  }
  #endregion
  
  #region LinqSamples-62
  public class LinqSamples62 : IExecutable
  {
    public void Execute()
    {
      // Remove()
      var root = BuildSampleXml();
      var elem = root.Descendants("Value").First();
      
      elem.Remove();
      
      Console.WriteLine(root);
      Console.WriteLine("=====================================");
      
      // RemoveAll()
      root = BuildSampleXml();
      elem = root.Elements("Child").First();
      
      elem.RemoveAll();
      
      Console.WriteLine(root);
      Console.WriteLine("=====================================");
      
      // RemoveNodes()
      root = BuildSampleXml();
      elem = root.Elements("Child").First();
      
      elem.RemoveNodes();
      
      Console.WriteLine(root);
      Console.WriteLine("=====================================");
      
      // SetElementValue(XName, object)
      root = BuildSampleXml();
      elem = root.Elements("Child").First();
      
      elem.SetElementValue("Value", null);
      
      Console.WriteLine(root);
      Console.WriteLine("=====================================");
    }
    
    XElement BuildSampleXml()
    {
      return XElement.Parse("<Root><Child Id=\"100\"><Value>hoge</Value></Child></Root>");
    }
  }
  #endregion
  
  #region QueueSynchronizedSamples-01
  /// <summary>
  /// Queueの同期処理についてのサンプルです。
  /// </summary>
  public class QueueSynchronizedSamples01 : IExecutable
  {
    Queue queue;

    public void Execute()
    {
      queue = Queue.Synchronized(new Queue());
      Console.WriteLine("Queue.IsSyncronized == {0}", queue.IsSynchronized);

      for(int i = 0; i < 1000; i++)
      {
        queue.Enqueue(i);
      }

      new Thread(EnumerateCollection).Start();
      new Thread(ModifyCollection).Start();

      Console.WriteLine("Press any key to exit...");
      Console.ReadLine();
    }

    void EnumerateCollection()
    {
      //
      // ロックせずに列挙処理を行う。
      //
      // CollectionのSynchronizedメソッドで作成したオブジェクトは
      // 単一操作に対しては、同期できるが複合アクションはガードできない。
      // （イテレーション、ナビゲーション、プット・イフ・アブセントなど）
      //
      // 別のスレッドにて、コレクションを操作している場合
      // 例外が発生する可能性がある。
      //
      /*
      foreach(int i in queue)
      {
        Console.WriteLine(i);
        Thread.Sleep(0);
      }
      */

      //
      // 第一の方法：
      //
      // ループしている間、コレクションをロックする.
      // 有効であるが、列挙処理を行っている間ずっとロックされたままとなる。
      // 
      /*
      lock(queue.SyncRoot)
      {
        foreach(int i in queue)
        {
          Console.WriteLine(i);
          Thread.Sleep(0);
        }
      }
      */

      //
      // 第二の方法：
      //
      // 一旦ロックを獲得し、コレクションのクローンを作成する。
      // クローン作成後、ロックを解放し、その後クローンに対して列挙処理を行う。
      //
      // これもコレクション自体が大きい場合は時間と負荷がかかるが、それはトレードオフとなる。
      //
      Queue cloneQueue = null;
      lock(queue.SyncRoot)
      {

        Array array = Array.CreateInstance(typeof(int), queue.Count);
        queue.CopyTo(array, 0);

        cloneQueue = new Queue(array);
      }

      foreach(int i in cloneQueue)
      {
        Console.WriteLine(i);

        // わざとタイムスライスを切り替え
        Thread.Sleep(0);
      }
    }

    void ModifyCollection()
    {

      for(;;)
      {
        if(queue.Count == 0)
        {
          break;
        }

        Console.WriteLine("\t==> Dequeue");
        queue.Dequeue();

        // わざとタイムスライスを切り替え
        Thread.Sleep(0);
      }

    }
  }
  #endregion
  
  #region DataTableSortSamples-01
  /// <summary>
  /// DataTableについてのサンプルです。
  /// </summary>
  public class DataTableSortSamples01 : IExecutable
  {
    public void Execute()
    {
      DataTable table = new DataTable("SortSampleTable");

      table.Columns.Add("Col1", typeof(string));
      table.Columns.Add("Col2", typeof(string));

      table.LoadDataRow(new object[]{"1", "1"}, true);
      table.LoadDataRow(new object[]{"1", "3"}, true);
      table.LoadDataRow(new object[]{"1", "4"}, true);
      table.LoadDataRow(new object[]{"1", "2"}, true);
      table.LoadDataRow(new object[]{"2", "1"}, true);
      table.LoadDataRow(new object[]{"2", "3"}, true);
      table.LoadDataRow(new object[]{"2", "5"}, true);
      table.LoadDataRow(new object[]{"2", "4"}, true);
      table.LoadDataRow(new object[]{"2", "2"}, true);

      Console.WriteLine("===================================================");
      foreach(DataRow row in table.Rows)
      {
        DumpRow(row);
      }
      Console.WriteLine("===================================================");

      Console.WriteLine("===================================================");
      table.DefaultView.Sort = "Col1 DESC";
      foreach(DataRowView row in table.DefaultView)
      {
        DumpRow(row);
      }
      Console.WriteLine("===================================================");

      Console.WriteLine("===================================================");
      table.DefaultView.Sort = "Col1 ASC";
      foreach(DataRowView row in table.DefaultView)
      {
        DumpRow(row);
      }
      Console.WriteLine("===================================================");
    }

    void DumpRow(DataRow row)
    {
      Console.WriteLine("{0}, {1}", row[0], row[1]);
    }

    void DumpRow(DataRowView row)
    {
      Console.WriteLine("{0}, {1}", row[0], row[1]);
    }
  }
  #endregion

  #region EnumSamples-001
  /// <summary>
  /// Enumについてのサンプルです。
  /// </summary>
  public class EnumSamples001 : IExecutable
  {
    //
    // Enumを定義.
    //
    // フラグ値としても利用する場合はFlagAttributeを付ける.
    //
    // 基になる型は明示的に指定しない場合はintとなる。
    // 列挙定数は２の累乗で定義する方がいい模様。（MSDNより）
    // 
    [Flags]
    private enum SampleEnum
    {
      Value1 = 1,
      Value2 = 2,
      Value3 = 4,
      Value4 = 16 
    }

    public void Execute()
    {
      //
      // FlagsAttributeを付与している場合は
      // 単体の値としても利用できるが、AND OR XORの
      // 演算も行えるようになる。
      // 
      SampleEnum enum1 = SampleEnum.Value2;
      SampleEnum enum2 = (SampleEnum.Value1 | SampleEnum.Value3);

      Console.WriteLine(enum1);
      Console.WriteLine(enum2);

      Console.WriteLine("enum2 has Value3? == {0}", ((enum2 & SampleEnum.Value3) == SampleEnum.Value3));
      Console.WriteLine("enum2 has Value2? == {0}", ((enum2 & SampleEnum.Value2) == SampleEnum.Value2));

      /////////////////////////////////////////////////////////////
      //
      // System.Enumクラスには、列挙型を扱う上で便利なメソッドが
      // いくつか用意されている。
      //
      // ■Formatメソッド
      // ■GetNameメソッド
      // ■GetNamesメソッド
      // ■GetUnderlyingTypeメソッド
      // ■GetValuesメソッド
      // ■IsDefinedメソッド
      // ■Parseメソッド
      // ■ToObjectメソッド
      // ■ToStringメソッド
      //
      Console.WriteLine(string.Empty);
      
      //
      // Formatメソッド.
      //
      // 対象となる列挙値を特定のフォーマットにして取得する。
      // 指定出来るオプションは以下の通り。
      //
      // ■G or g: 名前を取得（但し、値が存在しない場合、１０進数でその値が返される）
      // ■X or x: １６進数で値を取得 (但し、0xは先頭に付与されない）
      // ■D or d: １０進数で値を取得
      // ■F or f: Gとほぼ同じ。
      //
      Console.WriteLine("============ {0} ============", "Format");
      Console.WriteLine(Enum.Format(typeof(SampleEnum), 2, "G"));
      Console.WriteLine(Enum.Format(typeof(SampleEnum), (2 | 3), "G"));
      Console.WriteLine(Enum.Format(typeof(SampleEnum), (SampleEnum.Value1 | SampleEnum.Value3), "G"));
      Console.WriteLine(Enum.Format(typeof(SampleEnum), SampleEnum.Value4, "X"));
      Console.WriteLine(Enum.Format(typeof(SampleEnum), SampleEnum.Value4, "D"));
      Console.WriteLine(Enum.Format(typeof(SampleEnum), SampleEnum.Value4, "F"));
      Console.WriteLine(Enum.Format(typeof(SampleEnum), (SampleEnum.Value1 | SampleEnum.Value4), "F"));

      //
      // GetNameメソッド
      //
      // 対象となる値から、対応する列挙値の名前を取得する.
      // 対応する列挙値が存在しない場合は、nullとなる。
      //
      Console.WriteLine("============ {0} ============", "GetName");
      int targetValue  = 4;
      Console.WriteLine(Enum.GetName(typeof(SampleEnum), targetValue));
      Console.WriteLine(Enum.GetName(typeof(SampleEnum), -1) == null ? "null" : string.Empty);

      //
      // GetNamesメソッド
      //
      // 対象となる列挙型に定義されている値の名称を一気に取得する.
      //
      Console.WriteLine("============ {0} ============", "GetNames");
      string[] names = Enum.GetNames(typeof(SampleEnum));
      names.ToList().ForEach(Console.WriteLine);

      //
      // GetUnderlyingTypeメソッド
      //
      // 特定の列挙値が属する列挙型を取得する。
      //
      Console.WriteLine("============ {0} ============", "GetUnderlyingType");
      Enum enumVal    = SampleEnum.Value2;
      Type enumType     = enumVal.GetType();
      Type underlyingType = Enum.GetUnderlyingType(enumType);

      Console.WriteLine(enumType.Name);

      //
      // GetValuesメソッド
      //
      // 対象となる列挙型に設定されている値を一気に取得.
      //
      Console.WriteLine("============ {0} ============", "GetValues");
      Array valueArray = Enum.GetValues(typeof(SampleEnum));
      foreach(var element in valueArray)
      {
        Console.WriteLine(element);
      }

      //
      // IsDefinedメソッド
      //
      // 指定した値が、対象となる列挙型に存在するか否かを調査する。
      //
      Console.WriteLine("============ {0} ============", "IsDefined");
      Console.WriteLine("値{0}がSampleEnumに存在するか？ {1}", 2, Enum.IsDefined(typeof(SampleEnum), 2));
      Console.WriteLine("値{0}がSampleEnumに存在するか？ {1}", 10, Enum.IsDefined(typeof(SampleEnum), 10));

      //
      // Parseメソッド.
      //
      // 文字列から対応する列挙値を取得する。
      // 尚、該当文字列に対応する列挙値が存在しない場合はnullでなく
      // ArgumentExceptionが発生する。
      //
      // Parseメソッドには、以下のパターンのデータを指定することが出来る。
      // ■単一の値
      // ■列挙値の名前
      // ■名前をコンマで繋いだリスト
      //
      // 名前をコンマで繋いだリストを指定した場合は、該当する列挙値の
      // OR演算された結果が取得できる。
      //
      Console.WriteLine("============ {0} ============", "Parse");
      string testVal = "Value4";
      Console.WriteLine(Enum.Parse(typeof(SampleEnum), testVal));

      try
      {
        // 存在しない値を指定.
        Console.WriteLine(Enum.Parse(typeof(SampleEnum), "not_found"));
      }
      catch(ArgumentException)
      {
        Console.WriteLine("文字列 not_found に対応する列挙値が存在しない。");
      }

      testVal = "4";
      Console.WriteLine(Enum.Parse(typeof(SampleEnum), testVal));

      testVal = "Value1,Value2,Value4";
      Console.WriteLine(Enum.Parse(typeof(SampleEnum), testVal));

      //
      // ToObjectメソッド.
      //
      // 指定された値を対応する列挙値に変換する。
      // 各型に対応するためのオーバーロードメソッドが存在する。
      //
      Console.WriteLine("============ {0} ============", "ToObject");
      int v = 1;
      Console.WriteLine(Enum.ToObject(typeof(SampleEnum), v));

      //
      // ToStringメソッド.
      //
      // 対応する列挙値の文字列表現を取得する。
      // これまでに上述した各処理は全てEnumクラスのstaticメソッドで
      // あったが、このメソッドはインスタンスメソッドとなる。
      //
      // 基本的に、Enum.Formatメソッドに"G"を適用した結果となる。
      // （IFormatProviderを指定した場合はカスタム書式となる。）
      //
      Console.WriteLine("============ {0} ============", "ToString");
      SampleEnum e1 = SampleEnum.Value4;
      Console.WriteLine(e1.ToString());

    }
  } 
  #endregion

  #region StringInfoSamples-001
  /// <summary>
  /// StringInfoについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// サロゲートペアについて記述しています。
  /// </remarks>
  public class StringInfoSamples001 : IExecutable
  {
    public class StringInfoSampleForm : WinFormsForm
    {
      public StringInfoSampleForm()
      {
        InitializeComponent();
      }

      void InitializeComponent()
      {
        SuspendLayout();

        Size = new GDISize(350, 100);
        StartPosition = WinFormsFormStartPosition.CenterScreen;
        Text = "サロゲートペアの確認サンプル";


        WinFormsTextBox t = new WinFormsTextBox();
        t.Text = "\uD867\uDE3D"; // 魚へんに花という文字。魚のホッケの文字を指定。

        WinFormsButton b = new WinFormsButton{ Text="Exec" };
        b.Click += (s, e) => 
        {

          string str = t.Text;
          WinFormsMessageBox.Show(string.Format("文字：{0}, 長さ：{1}", str, str.Length), "Stringでの表示");

          //
          // サロゲートペアの文字列
          //
          // サロゲートペアの文字列は１文字で
          // ４バイトとなっている。
          //
          // サロゲートペアの文字列に対して
          // String.Lengthプロパティで長さを取得すると
          // １文字なのに２と返ってくる。
          //
          // これを１文字として認識するには以下のクラスを利用する。
          //
          // System.Globalization.StringInfo
          //
          // このクラスの以下のプロパティを利用することで
          // １文字と認識することが出来る。
          //
          // LengthInTextElementsプロパティ
          //
          StringInfo si = new StringInfo(str);
          WinFormsMessageBox.Show(string.Format("文字：{0}, 長さ：{1}", si.String, si.LengthInTextElements), "StringInfoでの表示");
        };

        WinFormsFlowLayoutPanel contentPane = new WinFormsFlowLayoutPanel{ FlowDirection=WinFormsFlowDirection.TopDown, WrapContents=true };
        contentPane.Controls.AddRange(new WinFormsControl[]{ t, b });
        contentPane.Dock = WinFormsDockStyle.Fill;

        Controls.Add(contentPane);

        ResumeLayout();
      }
    }

    [STAThread]
    public void Execute()
    {
      WinFormsApplication.EnableVisualStyles();
      WinFormsApplication.Run(new StringInfoSampleForm());
    }
  }
  #endregion

  #region SecureStringSamples-001
  /// <summary>
  /// SecureStringについてのサンプルです。
  /// </summary>
  public class SecureStringSamples001 : IExecutable
  {
    public void Execute()
    {
      //
      // SecureStringのサンプル.
      //
      // System.Security.SecureStringクラスは、通常の文字列とは
      // 違い、パスワードなどの機密情報を扱ったりする際に利用される。
      //
      // よく利用されるProcessクラスのStartメソッドではパスワードを渡す際は
      // SecureStringを渡す必要がある。
      //
      // このクラスのインスタンスに設定された内容は自動的に暗号化され
      // MakeReadOnlyメソッドを利用して、読み取り専用とすると変更できなくなる。
      //
      // SecureStringにデータを設定する際は、AppendCharメソッドを利用して
      // 1文字ずつデータを設定していく必要がある。
      //
      // SecureStringには、値を比較または変換する為のメソッドが存在しない。
      // 操作を行う為には、System.Runtime.InteropServices.MarshalのCoTaskMemUnicodeメソッドと
      // Copyメソッドを利用してchar[]に変換する必要がある。
      //

      //
      // SecureStringを構築.
      //
      // 実際はユーザからのパスワード入力を元にSecureStringを構築したりする.
      //
      SecureString secureStr = MakeSecureString();

      //
      // ToString()メソッドを呼び出してもSecureStringの中身を
      // 見ることはできない。
      //
      Console.WriteLine(secureStr);

      //
      // IsReadOnlyメソッドで現在読み取り専用としてマークされているか否かが
      // 判別できる。読み取り専用でない場合、変更は可能。
      //
      // 読み取り専用にするにはMakeReadOnlyメソッドを使用する。
      //
      Console.WriteLine("IsReadOnly:{0}", secureStr.IsReadOnly());
      secureStr.MakeReadOnly();
      Console.WriteLine("IsReadOnly:{0}", secureStr.IsReadOnly());

      //
      // SecureStringの中身を復元するには、以下のメソッドを利用する。
      //
      // ■Marshal.SecureStringToCoTaskMemUnicodeメソッド
      // ■Marshal.Copyメソッド
      // ■Marshal.ZeroFreeCoTaskMemUnicodeメソッド
      //
      RestoreSecureString(secureStr);
    }

    SecureString MakeSecureString()
    {
      SecureString secureStr = new SecureString();

      foreach(char ch in "hello world")
      {
        secureStr.AppendChar(ch);
      }

      return secureStr;
    }

    void RestoreSecureString(SecureString secureStr)
    {

      IntPtr pointer = IntPtr.Zero;
      try
      {
        //
        // コピー先のバッファを作成.
        //
        char[] buffer = new char[secureStr.Length];

        //
        // 復元処理.
        //
        pointer = Marshal.SecureStringToCoTaskMemUnicode(secureStr);
        Marshal.Copy(pointer, buffer, 0, buffer.Length);

        Console.WriteLine(new string(buffer));
      }
      finally
      {
        if (pointer != IntPtr.Zero)
        {
          //
          // 解放.
          //
          Marshal.ZeroFreeCoTaskMemUnicode(pointer);
        }
      }
    }
  }
  #endregion
  
  #region System.Timers.Timerのサンプル
  /// <summary>
  /// System.Timers.Timerクラスについてのサンプルです。
  /// </summary>
  public class ServerTimerSamples01 : WinFormsForm, IExecutable
  {
    System.Timers.Timer _timer;
    WinFormsListBox _listBox;
    
    public ServerTimerSamples01()
    {
      InitializeComponent();
      SetTimer();
    }
    
    void InitializeComponent()
    {
      SuspendLayout();
      
      Text = "Timer Sample.";
      FormClosing += OnFormClosing;
      
      _listBox = new WinFormsListBox();
      _listBox.Dock = WinFormsDockStyle.Fill;
      
      Controls.Add(_listBox);
      
      ResumeLayout();
    }
    
    void SetTimer()
    {
      _timer = new System.Timers.Timer();
      
      _timer.Elapsed += OnTimerElapsed;
      
      //
      // System.Timers.Timerはサーバータイマの為
      // ThreadPoolにてイベントが発生する。
      //
      // Elapsedイベント内で、UIコントロールにアクセスする必要がある場合
      // そのままだと、別スレッドからコントロールに対してアクセスしてしまう可能性があるので
      // イベント内にて、Control.Invokeするか、以下のようにSynchronizingObjectを
      // 設定して、イベントの呼び出しをマーシャリングするようにする。
      //
      _timer.SynchronizingObject = this;
       
       //
       // 繰り返しの設定.
       //
      _timer.Interval  = 1000;
      _timer.AutoReset = true;
      
      //
      // タイマを開始.
      //
      _timer.Enabled   = true;      
    }
    
    public void Execute()
    {
      WinFormsApplication.EnableVisualStyles();
      WinFormsApplication.Run(new ServerTimerSamples01());
    }
    
    void OnFormClosing(object sender, WinFormsFormClosingEventArgs e)
    {
      _timer.Enabled = false;
      _timer.Dispose();
    }
    
    void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      _listBox.Items.Add(String.Format("Time:{0}, ThreadID:{1}", e.SignalTime.ToString("HH:mm:ss"), Thread.CurrentThread.ManagedThreadId.ToString()));
    }
  }
  #endregion
  
  #region String::IsNullOrWhiteSpaceメソッドのサンプル
  /// <summary>
  /// String.IsNullOrWhiteSpaceメソッドについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// .NET 4.0から追加されたメソッドです。
  /// </remarks>
  public class StringIsNullOrWhiteSpaceSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // String::IsNullOrWhiteSpaceメソッドは、IsNullOrEmptyメソッドの動作に
      // 加え、更に空白文字のみの場合もチェックしてくれる。
      //
      string nullStr     = null;
      string emptyStr    = string.Empty;
      string spaceStr    = "    ";
      string normalStr     = "hello world";
      string zenkakuSpaceStr = "　　　";
      
      //
      // String::IsNullOrEmptyでの結果.
      //
      Console.WriteLine("============= String::IsNullOrEmpty ==============");
      Console.WriteLine("nullStr   = {0}", string.IsNullOrEmpty(nullStr));
      Console.WriteLine("emptyStr  = {0}", string.IsNullOrEmpty(emptyStr));
      Console.WriteLine("spaceStr  = {0}", string.IsNullOrEmpty(spaceStr));
      Console.WriteLine("normalStr = {0}", string.IsNullOrEmpty(normalStr));
      Console.WriteLine("zenkakuSpaceStr = {0}", string.IsNullOrEmpty(zenkakuSpaceStr));
      
      //
      // String::IsNullOrWhiteSpaceでの結果.
      //  全角空白もスペースと見なされる点に注意。
      //
      Console.WriteLine("============= String::IsNullOrWhiteSpace ==============");
      Console.WriteLine("nullStr   = {0}", string.IsNullOrWhiteSpace(nullStr));
      Console.WriteLine("emptyStr  = {0}", string.IsNullOrWhiteSpace(emptyStr));
      Console.WriteLine("spaceStr  = {0}", string.IsNullOrWhiteSpace(spaceStr));
      Console.WriteLine("normalStr = {0}", string.IsNullOrWhiteSpace(normalStr));
      Console.WriteLine("zenkakuSpaceStr = {0}", string.IsNullOrWhiteSpace(zenkakuSpaceStr));
    }
  }
  #endregion
  
  #region 時刻に関する処理(XX時間XX分形式から10進数形式に変換)
  /// <summary>
  /// 時刻に関する処理(XX時間XX分形式から10進数形式に変換)についてのサンプルです。
  /// </summary>
  public class TimeConvertSample01 : IExecutable
  {
    public void Execute()
    {
      // 元の値。7時間40分とする.
      decimal original = 111.07M;
      
      //
      // 時間の部分は既に確定済みなので、そのまま利用.
      //
      int hour = decimal.ToInt32(original);
      
      //
      // 元の値より、時間の部分を差し引く.
      // 上記の元値の場合は、0.4となる。
      //
      decimal minutes = (original - hour);
      
      //
      // 0.4に対して、100を掛けて分数を確定.
      //
      minutes *= 100;
      
      //
      // 最後に60（一時間の分数）で割る.
      //
      minutes /= 60;
      
      //
      // 計算結果によっては、端数が生じるので四捨五入.
      // (小数点第3位四捨五入)
      //
      minutes = Math.Round(minutes, 2, MidpointRounding.AwayFromZero);
      
      //
      // 結果を構築.
      //
      // 上記の分を求める式は、以下のようにも出来る。
      // minutes = Math.Round(((original % 1) * 100 / 60), 2, MidpointRounding.AwayFromZero);
      //
      decimal result = ((decimal) hour + minutes);
      
      Console.WriteLine("{0}時間", result);
    }
  }
  #endregion
  
  #region 時刻に関する処理(10進数形式からXX時間XX分形式に変換)
  /// <summary>
  /// 時刻に関する処理(10進数形式からXX時間XX分形式に変換)についてのサンプルです。
  /// </summary>
  public class TimeConvertSample02 : IExecutable
  {
    public void Execute()
    {
      // 元の値. 7.67時間とする.
      decimal original = 111.12M;
      
      //
      // 時間の部分は既に確定済みなので、そのまま利用.
      //
      int hour = decimal.ToInt32(original);
      
      //
      // 時間部分の分数を算出.
      //
      int hourMinutes = (hour * 60);
      
      //
      // 元の値の分数を算出.
      //
      decimal originalMinutes = (original * 60);
      
      //
      // 求めた元の値の分数を四捨五入.
      //
      int roundedOriginalMinutes = decimal.ToInt32(Math.Round(originalMinutes, 0, MidpointRounding.AwayFromZero));
      
      //
      // 元の値の分数から時間部分の分数を引く.
      // これが結果の分数となる。
      //
      int minutes = (roundedOriginalMinutes - hourMinutes);
      
      //
      // 結果を構築.
      //
      decimal result = decimal.Parse(string.Format("{0}.{1}", hour, minutes));
      
      Console.WriteLine("結果={0}, {1}時間{2}分", result, hour, minutes);
    }
  }
  #endregion
  
  #region ExpandoObjectクラスのサンプル-01
  /// <summary>
  /// ExpandoObjectクラスについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// .NET 4.0から追加されたクラスです。
  /// </remarks>
  public class ExpandoObjectSamples01 : IExecutable
  {
    public void Execute()
    {
      //////////////////////////////////////////////////////////////////////
      //
      // 動的オブジェクトを作成.
      //
      // System.Dynamic名前空間は、「System.Core.dll」内に存在する。
      // 動的オブジェクトを利用するには、上記のDLLの他に以下のDLLも参照設定
      // する必要がある。
      //
      // ・Microsoft.CSharp.dll
      //
      dynamic obj = new System.Dynamic.ExpandoObject();
      
      //
      // メンバーを定義.
      //
      // プロパティ.
      obj.Value = 10;
      
      // メソッド.
      var action = new Action<string>((line) =>
      {
        Console.WriteLine(line);
      });
      
      obj.WriteLine = action;
      
      //
      // 呼び出してみる.
      //
      obj.WriteLine(obj.Value.ToString());
      
      obj.Value = 100;
      obj.WriteLine(obj.Value.ToString());
      
      obj.Value = "hoge";
      obj.WriteLine(obj.Value.ToString());
    }
  }
  #endregion
  
  #region ExpandoObjectクラスのサンプル-02
  /// <summary>
  /// ExpandoObjectクラスについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// .NET 4.0から追加されたクラスです。
  /// </remarks>
  public class ExpandoObjectSamples02 : IExecutable
  {
    public void Execute()
    {
      ///////////////////////////////////////////////
      //
      // ExpandoObjectにイベントを追加.
      //
      dynamic obj = new System.Dynamic.ExpandoObject();
      
      //
      // イベント定義
      //   ExpandoObjectに対してイベントを定義するには
      //   まず、イベントフィールドを定義して、それをnullで初期化
      //   する必要がある。
      //
      obj.MyEvent = null;
      
      //
      // イベントハンドラを設定.
      //
      obj.MyEvent += new EventHandler((sender, e) =>
      {
        Console.WriteLine("sender={0}", sender);
      });
      
      // イベント着火.
      obj.MyEvent(obj, EventArgs.Empty);
    }
  }
  #endregion
  
  #region ExpandoObjectクラスのサンプル-03
  /// <summary>
  /// ExpandoObjectについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// .NET 4.0から追加されたクラスです。
  /// </remarks>
  public class ExpandoObjectSamples03 : IExecutable
  {
    public void Execute()
    {
      ///////////////////////////////////////////////////////////////////////
      //
      // ExpandoObjectをDictionaryとして扱う. (メンバーの追加/削除)
      //   ExpandoObjectはIDictionary<string, object>を実装しているので
      //   Dictionaryとしても利用出来る.
      //
      dynamic obj = new System.Dynamic.ExpandoObject();
      obj.Name = "gsf_zero1";
      obj.Age  = 30;
      
      //
      // 定義されているメンバーを列挙.
      //
      IDictionary<string, object> map = obj as IDictionary<string, object>;
      foreach (var pair in map)
      {
        Console.WriteLine("{0}={1}", pair.Key, pair.Value);
      }
      
      //
      // Ageメンバーを削除.
      //
      map.Remove("Age");
      
      //
      // 確認.
      //
      foreach (var pair in map)
      {
        Console.WriteLine("{0}={1}", pair.Key, pair.Value);
      }
      
      // エラーとなる.
      //Console.WriteLine(obj.Age);
    }
  }
  #endregion
  
  #region ExpandoObjectクラスのサンプル-04
  /// <summary>
  /// ExpandoObjectクラスについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// .NET 4.0空追加されたクラスです。
  /// </remarks>
  public class ExpandoObjectSamples04 : IExecutable
  {
    public void Execute()
    {
      ///////////////////////////////////////////////////////////////////////
      //
      // ExpandoObjectをINotifyPropertyChangedとして扱う. (プロパティの変更をハンドル)
      //
      dynamic obj = new System.Dynamic.ExpandoObject();
      
      //
      // イベントハンドラ設定.
      //
      (obj as INotifyPropertyChanged).PropertyChanged += (sender, e) =>
      {
        Console.WriteLine("Property Changed:{0}", e.PropertyName);
      };
      
      //
      // メンバー定義.
      //
      obj.Name = "gsf_zero1";
      obj.Age  = 30;
      
      //
      // メンバー削除.
      //
      (obj as IDictionary<string, object>).Remove("Age");
      
      //
      // 値変更.
      //
      obj.Name = "gsf_zero2";
      
      //
      // 実行結果：
      //     Property Changed:Name
      //     Property Changed:Age
      //     Property Changed:Age
      //     Property Changed:Name
      //
    }
  }
  #endregion
  
  #region MEFSamples-01
  /// <summary>
  /// MEFについてのサンプルです。
  /// </summary>
  public class MEFSamples01 : IExecutable
  {
    // Export用のインターフェース
    public interface IExporter
    {
      string Name { get; }
    }
    
    // Exportパート
    [Export(typeof(IExporter))]
    public class Exporter : IExporter
    {
      public string Name
      {
        get
        {
          return "☆☆☆☆☆☆☆ Exporter ☆☆☆☆☆☆☆";
        }
      }
    }
    
    // Importパート
    // 尚、明示的にnullを初期値として指定しているのは、そのままだとコンパイラによって警告扱いされるため
    [Import(typeof(IExporter))]
    IExporter _exporter = null;
    
    // コンテナ
    CompositionContainer _container;
    
    public void Execute()
    {
      //
      // カタログ構築.
      //  AggregateCatalogは、複数のCatalogを一つにまとめる役割を持つ。
      //
      var catalog = new AggregateCatalog();
      // AssemblyCatalogを利用して、自分自身のアセンブリをカタログに追加.
      catalog.Catalogs.Add(new AssemblyCatalog(typeof(MEFSamples01).Assembly));
      
      //
      // コンテナを構築.
      //
      _container = new CompositionContainer(catalog);
      try
      {
        // 合成実行.
        _container.ComposeParts(this);
        
        // 実行.
        Console.WriteLine(_exporter.Name);
      }
      catch (CompositionException ex)
      {
        // 合成に失敗した場合.
        Console.WriteLine(ex.ToString());
      }
      
      if (_container != null)
      {
        _container.Dispose();
      }
    }
  }
  #endregion
  
  #region MEFSamples-02
  /// <summary>
  /// MEFについてのサンプルです。
  /// </summary>
  public class MEFSamples02 : IExecutable
  {
    // Export用のインターフェース
    public interface IExporter
    {
      string Name { get; }
    }
    
    [Export(typeof(IExporter))]
    public class FirstExporter : IExporter
    {
      public string Name
      {
        get
        {
          return "☆☆ FIRST EXPORTER ☆☆";
        }
      }
    }
    
    [Export(typeof(IExporter))]
    public class SecondExporter : IExporter
    {
      public string Name
      {
        get
        {
          return "☆☆ SECOND EXPORTER ☆☆";
        }
      }
    }

    [Export(typeof(IExporter))]
    public class ThirdExporter : IExporter
    {
      public string Name
      {
        get
        {
          return "☆☆ THIRD EXPORTER ☆☆";
        }
      }
    }
    
    // Importパート (複数のExportを受け付ける）
    //
    // 通常、複数のExportを受け付ける場合は以下の書式で宣言する。
    //   IEnumerable<Lazy<T>>
    //
    // Lazy<T>を利用する事により、遅延ローディングが可能となる。
    // (利用しないExportパートが合成時にインスタンス化されるのを防ぐ）
    //
    // また、メタデータを利用する場合は以下のようになる。
    //   IEnumerable<Lazy<T, TMetaData>>
    //
    // 尚、明示的にnullを初期値として指定しているのは、そのままだとコンパイラによって警告扱いされるため
    [ImportMany(typeof(IExporter))]
    IEnumerable<Lazy<IExporter>> _exporters = null;
    
    // コンテナ.
    CompositionContainer _container;
    
    public void Execute()
    {
      //
      // カタログ構築.
      //  AggregateCatalogは、複数のCatalogを一つにまとめる役割を持つ。
      //
      var catalog = new AggregateCatalog();
      // AssemblyCatalogを利用して、自分自身のアセンブリをカタログに追加.
      catalog.Catalogs.Add(new AssemblyCatalog(typeof(MEFSamples01).Assembly));
      
      //
      // コンテナを構築.
      //
      _container = new CompositionContainer(catalog);
      try
      {
        // 合成実行.
        _container.ComposeParts(this);
        
        // 実行.
        foreach (Lazy<IExporter> lazyObj in _exporters)
        {
          Console.WriteLine(lazyObj.Value.Name);
        }
        
      }
      catch (CompositionException ex)
      {
        // 合成に失敗した場合.
        Console.WriteLine(ex.ToString());
      }
      
      if (_container != null)
      {
        _container.Dispose();
      }
    }
  }
  #endregion

  #region MEFSamples-03
  /// <summary>
  /// MEFについてのサンプルです。
  /// </summary>
  public class MEFSamples03 : IExecutable
  {
    // Export用のインターフェース
    public interface IExporter
    {
      string Name { get; }
    }
    
    // Exporter用のメタデータインターフェース
    public interface IExporterMetadata
    {
      string Symbol { get; }
    }
    
    [Export(typeof(IExporter))]
    [ExportMetadata("Symbol", "FIRST")]
    public class FirstExporter : IExporter
    {
      public string Name
      {
        get
        {
          return "☆☆ FIRST EXPORTER ☆☆";
        }
      }
    }
    
    [Export(typeof(IExporter))]
    [ExportMetadata("Symbol", "SECOND")]
    public class SecondExporter : IExporter
    {
      public string Name
      {
        get
        {
          return "☆☆ SECOND EXPORTER ☆☆";
        }
      }
    }

    [Export(typeof(IExporter))]
    [ExportMetadata("Symbol", "THIRD")]
    public class ThirdExporter : IExporter
    {
      public string Name
      {
        get
        {
          return "☆☆ THIRD EXPORTER ☆☆";
        }
      }
    }
    
    // Importパート (複数のExportを受け付け、且つ、メタデータ有り）
    //
    // 通常、複数のExportを受け付ける場合は以下の書式で宣言する。
    //   IEnumerable<Lazy<T>>
    //
    // Lazy<T>を利用する事により、遅延ローディングが可能となる。
    // (利用しないExportパートが合成時に全てインスタンス化されるのを防ぐ）
    //
    // また、メタデータを利用する場合は以下のようになる。
    //   IEnumerable<Lazy<T, TMetaData>>
    //
    // 尚、明示的にnullを初期値として指定しているのは、そのままだとコンパイラによって警告扱いされるため
    [ImportMany(typeof(IExporter))]
    IEnumerable<Lazy<IExporter, IExporterMetadata>> _exporters = null;
    
    // コンテナ.
    CompositionContainer _container;
    
    public void Execute()
    {
      //
      // カタログ構築.
      //  AggregateCatalogは、複数のCatalogを一つにまとめる役割を持つ。
      //
      var catalog = new AggregateCatalog();
      // AssemblyCatalogを利用して、自分自身のアセンブリをカタログに追加.
      catalog.Catalogs.Add(new AssemblyCatalog(typeof(MEFSamples01).Assembly));
      
      //
      // コンテナを構築.
      //
      _container = new CompositionContainer(catalog);
      try
      {
        // 合成実行.
        _container.ComposeParts(this);
        
        // 実行.
        foreach (Lazy<IExporter, IExporterMetadata> lazyObj in _exporters)
        {
          //
          // メタデータを調べ、合致したもののみを実行する.
          // Lazy<T, TMetadata>.Valueを呼ばない限りインスタンスは作成されない。
          //
          if (lazyObj.Metadata.Symbol == "SECOND")
          {
            Console.WriteLine(lazyObj.Value.Name);
          }
        }
        
      }
      catch (CompositionException ex)
      {
        // 合成に失敗した場合.
        Console.WriteLine(ex.ToString());
      }
      
      if (_container != null)
      {
        _container.Dispose();
      }
    }
  }
  #endregion
  
  #region CovarianceSamples-01
  /// <summary>
  /// 共変性についてのサンプルです。
  /// </summary>
  /// <remarks>
  /// 共変性は4.0から追加された機能です。
  /// </remarks>
  public class CovarianceSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // Covariance(共変性)は、簡単に言うと、子のオブジェクトを親の型として扱う事。
      //
      // 例：
      //   string str = "gsf_zero1";
      //   object obj = str;
      //
      // C# 4.0では、この概念をジェネリックインターフェースに対して適用できるようになった。
      // 共変性を表明するには、型引数を定義する際に、「out」キーワードを設定する。
      //
      // .NET 4.0では、IEnumerable<T>は以下のように定義されている。
      //   public interface IEnumerable<out T> : IEnumerable { ... }
      //
      // 「out」キーワードは、この型引数を「出力方向」にしか利用しないことを表明している。
      // つまり、「out」キーワードが付与されるとTを戻り値などの出力値にしか利用できなくなる。
      // (outを指定している状態で、入力方向、つまりメソッドの引数などにTを設定しようとすると
      //  コンパイルエラーが発生する。）
      //
      // 出力方向にしか利用しないので、子の型（つまり狭義の型）を親の型（つまり広義の型）に
      // 設定しても、問題ない。
      //  「内部の型はstringであるが、実際に値を取り出す際には親の型で受け取るので問題ない」
      //
      // Contravariance(反変性)は、この逆を行うものとなる。
      //
      IEnumerable<string> strings = new []{ "gsf_zero1", "gsf_zero2" };
      IEnumerable<object> objects = strings;
      
      foreach (var obj in objects)
      {
        Console.WriteLine("VALUE={0}, TYPE={1}", obj, obj.GetType().Name);
      }
    }
  }
  #endregion
  
  #region ContravarianceSamples-01
  /// <summary>
  /// 反変性についてのサンプルです。
  /// </summary>
  /// <remarks>
  /// 反変性は4.0から追加された機能です。
  /// </remarks>
  public class ContravarianceSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // Contravariance(反変性)は、簡単に言うと、親のオブジェクトを子の型として扱う事。
      // (共変性の逆です。）
      //
      // 例：
      //   class Parent     { ... }
      //   class Child : Parent { ... }
      //
      //   delegate Parent SampleDelegate();
      //
      //   Child SampleMethod() { ... }
      //
      //   // ここで反変性が発生している。
      //   SampleDelegate theDelegate = SampleMethod;
      //
      // C# 4.0では、この概念をジェネリックインターフェースに対して適用できるようになった。
      // 共変性を表明するには、型引数を定義する際に、「in」キーワードを設定する。
      //
      // .NET 4.0では、Action<T>は以下のように定義されている。
      //   public delegate void Action<in T>(T obj)
      //
      // 「in」キーワードは、この型引数を「入力方向」にしか利用しないことを表明している。
      // つまり、「in」キーワードが付与されるとTを引数などの入力値にしか利用できなくなる。
      // (inを指定している状態で、出力方向、つまりメソッドの戻り値などにTを設定しようとすると
      //  コンパイルエラーが発生する。）
      //
      // 入力方向にしか利用しないので、親の型（つまり広義の型）を子の型（つまり狭義の型）に
      // 設定しても、問題ない。
      //  「外部の型はstringであるが、実際にデータが渡される際、内部の引数の型はobjectなので問題ない」
      //
      // 例：
      //   Action<object> objAction = x => Console.WriteLine(x);
      //   Action<string> strAction = objAction;
      //
      //   strAction("gsf_zero1");
      //
      //   上記の例だと、objActionをstrActionに設定している。つまり親クラスの型で定義されているAction<object>を
      //   子のクラスのAction<string>に設定している。
      //   その後、strAction("gsf_zero1")としているので、外部から渡された値はstring型である。
      //   しかし、objActionの引数の型は、親クラスであるobject型なので問題なく動作する。
      //   (親クラスに定義されている振る舞いしか利用できないため。）
      //
      // Covariance(共変性)は、この逆を行うものとなる。
      //
      Action<object> objAction = x => Console.WriteLine(x);
      Action<string> strAction = objAction;
      
      strAction("gsf_zero1");
    }
  }
  #endregion
  
  #region IComparableSamples-01
  /// <summary>
  /// IComparableについてのサンプルです。
  /// </summary>
  public class IComparableSamples01 : IExecutable
  {
    enum CompareResult : int
    {
      SMALL = -1,
      EQUAL = 0,
      BIG   = 1
    }
    
    class Person : IComparable<Person>
    {
      public int  Id   { get; set; }
      public string Name { get; set; }
        
      public int CompareTo(Person other)
      {
        if (other == null)
        {
          return (int) CompareResult.SMALL;
        }
        
        int result  = (int) CompareResult.SMALL;
        int otherId = other.Id;
        
        if (Id == otherId)
        {
          result = (int) CompareResult.EQUAL;
        }
        else if (Id > otherId)
        {
          result = (int) CompareResult.BIG;
        }
        
        return result;
      }
      
      public override string ToString()
      {
        return string.Format("[ID={0}, NAME={1}]", Id, Name);
      }
    }
    
    public void Execute()
    {
      //
      // IComparableインターフェースは、インスタンスの並び替えをサポートするためのインターフェースである。
      //
      // このインターフェースを実装することで、クラスに対して順序の比較機能を付与することが出来る。
      // （List.Sortなどのソート処理時に、独自のソートルールを適用することができるようになる。）
      //  (ただし、DictionaryやHashTableなどには適用できない。これらの場合は、IEqualityComparableを実装する。）
      //
      var person1 = new Person { Id = 1, Name = "gsf_zero1" };
      var person2 = new Person { Id = 2, Name = "gsf_zero2" };
      var person3 = new Person { Id = 3, Name = "gsf_zero3" };
      var person4 = new Person { Id = 4, Name = "gsf_zero1" };
      
      var persons = new List<Person>{ person1, person2, person3, person4 };
      var random  = new Random();
      
      //
      // オブジェクト同士の比較.
      //
      for (int i = 0; i < 15; i++)
      {
        int index1 = random.Next(persons.Count);
        int index2 = random.Next(persons.Count);
        
        var p1 = persons[index1];
        var p2 = persons[index2];
        
        Console.WriteLine("person{0} CompareTo person{1} = {2}", index1, index2, (CompareResult) p1.CompareTo(p2));
      }
      
      //
      // リストのソート.
      //
      var persons2 = new List<Person>{ person3, person2, person4, person1 };
      
      // ソートせず、そのまま出力.
      Console.WriteLine("\n============== ソートせずそのまま出力. ================");
      persons2.ForEach(Console.WriteLine);
      
      // ソートを行ってから、出力.
      Console.WriteLine("\n============== ソートしてから出力. ================");
      persons2.Sort();
      persons2.ForEach(Console.WriteLine);
    }
  }
  #endregion
  
  #region ComparerSamples-01
  /// <summary>
  /// Comparerについてのサンプルです。
  /// </summary>
  public class ComparerSamples01 : IExecutable
  {
    enum CompareResult : int
    {
      SMALL = -1,
      EQUAL = 0,
      BIG   = 1
    }
    
    class Person
    {
      public int  Id   { get; set; }
      public string Name { get; set; }
      
      public override string ToString()
      {
        return string.Format("[ID={0}, NAME={1}]", Id, Name);
      }
    }

    //
    // Comparer<T>クラスは、抽象クラスとなっており。
    // IComparerインターフェースとIComparer<T>インターフェースの両方を実装している。
    //
    // 実際に、実装する必要があるのは以下のメソッドだけである。
    //   int Compare(T x, T y)
    //
    // IComparer.Compareメソッドについては、抽象クラス側にて明示的実装が行われている。
    //
    class PersonIdComparer : Comparer<Person>
    {
      public override int Compare(Person x, Person y)
      {
        if (object.Equals(x, y))
        {
          return (int) CompareResult.EQUAL;
        }
        
        int xId = x.Id;
        int yId = y.Id;
        
        return xId.CompareTo(yId);
      }
    }
    
    class PersonNameComparer : Comparer<Person>
    {
      public override int Compare(Person x, Person y)
      {
        if (object.Equals(x, y))
        {
          return (int) CompareResult.EQUAL;
        }
        
        string xName = x.Name;
        string yName = y.Name;
        
        return xName.CompareTo(yName);
      }
    }
    
    public void Execute()
    {
      //
      // IComparerインターフェース及びIComparer<T>インターフェースは、ともに順序の比較をサポートするための
      // インターフェースである。
      //
      // 同じ目的で利用されるインターフェースに、IComparableインターフェースが存在するが、違いは
      // IComparableインターフェースが、対象となるクラス自身に実装する必要があるのに対して
      // IComparerインターフェースは、個別に比較処理のみを実装したクラスを用意することにある。
      //
      // これにより、同じオブジェクトに対して、複数の比較方法を実装することが出来る。
      // (ソート処理を行う際に、比較処理を担当するオブジェクトを選択することができるようになる。）
      //
      // List.SortやSortedListやSortedDictionaryがこれをサポートする。
      //
      var person1 = new Person { Id = 1, Name = "gsf_zero1" };
      var person2 = new Person { Id = 2, Name = "gsf_zero2" };
      var person3 = new Person { Id = 3, Name = "gsf_zero3" };
      var person4 = new Person { Id = 4, Name = "gsf_zero1" };
      
      var persons = new List<Person>{ person3, person2, person4, person1 };
      
      // ソートせずにそのまま出力.
      persons.ForEach(Console.WriteLine);
      
      // Idで比較処理を行うComparerを指定してソート.
      Console.WriteLine(string.Empty);
      persons.Sort(new PersonIdComparer());
      persons.ForEach(Console.WriteLine);
      
      // NAMEで比較処理を行うComparerを指定してソート.
      Console.WriteLine(string.Empty);
      persons.Sort(new PersonNameComparer());
      persons.ForEach(Console.WriteLine);
    }
  }
  #endregion
  
  #region TupleSamples-01
  /// <summary>
  /// Tupleクラスについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// Tupleクラスは4.0から追加されたクラスです。
  /// </remarks>
  public class TupleSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // Tupleクラスは、.NET 4.0から追加されたクラスである。
      // 複数の値を一組のデータとして、保持することができる。
      // 
      // よく利用されるのは、戻り値にて複数の値を返す必要が有る場合などである。
      // (objectの配列を返すという手もあるが、その場合Boxingが発生してしまうのでパフォーマンスが
      //  厳しく要求される場面では、利用しづらい。その点、Tupleはジェネリッククラスとなっているので
      //  Boxingが発生する事がない。)
      //
      // Tupleクラスは、不可変のオブジェクトとなっている。つまり、コンストラクト時に値を設定した後は
      // その値を変更することが出来ない。（参照の先に存在しているメンバは変更可能。）
      // 各データは、「Item1」「Item2」・・・という形で取得していく。
      //
      // 以下のようなクラス定義が行われており、データの数によってインスタンス化するものが変わる。
      //   Tuple<T1>
      //   Tuple<T1, T2>
      //   Tuple<T1, T2, T3>
      //   Tuple<T1, T2, T3, T4>
      //   Tuple<T1, T2, T3, T4, T5>
      //   Tuple<T1, T2, T3, T4, T5, T6>
      //   Tuple<T1, T2, T3, T4, T5, T6, T7>
      //   Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>
      //
      // データ数が７つ以上の場合は、残りの部分をTRestとして設定する。
      //
      // Tupleを作成する際は、Tuple.Createメソッドを利用してインスタンスを取得するのが楽である。
      // また、その際は型推論を利用すると便利。
      //
      // Tupleクラスでは、ToStringメソッドがオーバーライドされており、以下のように表示される。
      //   Tuple<int, int>   ==> (xxx, yyy)
      //
      Tuple<int, string> t1 = Tuple.Create(100, "gsf_zero1");
      var        t2 = Tuple.Create(200, "gsf_zero2", 30);  // Tuple<int, string, int>となる。
      
      Console.WriteLine(t1.Item1);
      Console.WriteLine(t1.Item2);
      
      Console.WriteLine(t2.Item1);
      Console.WriteLine(t2.Item2);
      Console.WriteLine(t2.Item3);
      
      var t3 = TestMethod(10, 20);
      Console.WriteLine(t3);     // (100, 400)
      
      // 以下はエラーとなる.
      // t3.Item1 = 1000;
    }
    
    private Tuple<int, int> TestMethod(int x, int y)
    {
      return Tuple.Create(x * x, y * y);
    }
  }
  #endregion
  
  #region RuntimeEnvironmentSamples-01
  /// <summary>
  /// RuntimeEnvironmentクラスについてのサンプルです。
  /// </summary>
  public class RuntimeEnvironmentSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // System.Runtime.InteropServices.RuntimeEnvironmentクラスを利用する事で
      // .NETのランタイムパスなどを取得することができる。
      //
      Console.WriteLine("Runtime PATH:{0}",   RuntimeEnvironment.GetRuntimeDirectory());
      Console.WriteLine("System Version:{0}", RuntimeEnvironment.GetSystemVersion());
    }
  }
  #endregion
  
  #region TypeSamples-01
  public class TypeSamples01 : IExecutable
  {
    public void Execute()
    {
      List<int>         theList     = new List<int>{ 1, 2, 3, 4, 5 };
      Dictionary<int, string> theDictionary = new Dictionary<int, string>{ { 1, "hoge" }, { 2, "hehe" } };
      
      //
      // Genericなオブジェクトの型引数の型を取得するには、System.Typeクラスの以下のメソッドを利用する。
      //
      //   ・GetGenericArguments()
      //
      // GetGenericArgumentsメソッドは、System.Typeの配列を返すので、これを利用して型引数の型を判別する。
      //
      var genericArgTypes = theList.GetType().GetGenericArguments();
      Console.WriteLine("=============== List<int>の場合 =================");
      Console.WriteLine("型引数の数={0}, 型引数の型=({1})", genericArgTypes.Count(), string.Join(",", genericArgTypes.Select(item => item.Name)));
      
      genericArgTypes = theDictionary.GetType().GetGenericArguments();
      Console.WriteLine("=============== Dictionary<int, string>の場合 =================");
      Console.WriteLine("型引数の数={0}, 型引数の型=({1})", genericArgTypes.Count(), string.Join(",", genericArgTypes.Select(item => item.Name)));
    }
  }
  #endregion
  
  #region XDocumentSamples-01
  /// <summary>
  /// XDocumentクラスについてのサンプルです。
  /// </summary>
  public class XDocumentSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // XElementを構築する際、param引数には、直接XElementを設定しても、List<XElement>を指定しても問題ない。
      //
      var doc = new XDocument(new XElement("RootElement",
                        new XElement("Title", "gsf_zero1"),
                        new List<XElement>{ new XElement("Age", 30), new XElement("Address", "kyoto, Kyoto, Japan") }));
      
      Console.WriteLine(doc);
    }
  }
  #endregion
  
  #region XDocumentSamples-02
  /// <summary>
  /// XDocumentクラスについてのサンプルです。
  /// </summary>
  public class XDocumentSamples02 : IExecutable
  {
    public void Execute()
    {
      var xmlStrings = 
            @"<Persons>
              <Person>
                <Name>gsf_zero1</Name>
                <Age>30</Age>
              </Person>
            </Persons>";
      
      //
      // Parseメソッドを利用する事で、文字列から直接XDocumentを
      // 構築することが出来る。
      //
      var doc = XDocument.Parse(xmlStrings, LoadOptions.None);
      
      //
      // ノードを置換.
      //
      var name = (from element in doc.Descendants("Name") select element).First();
      
      name.ReplaceWith(new XElement("名前", name.Value));
      Console.WriteLine(doc);
    }
  }
  #endregion
  
  #region ConsoleCursorSamples-01
  /// <summary>
  /// Consoleクラスを利用してプログラムの実行状況を示すサンプルです。
  /// </summary>
  /// <remarks>
  /// このサンプルはEmEditor経由では動作できません。
  /// このクラスのソースコードを別ファイルに保存してコマンドラインにて
  /// 実行してください。
  ///</remarks>
  public class ConsoleCursorSamples01 : IExecutable
  {
    volatile bool _stop;
    
    public void Execute()
    {
      //
      // Consoleクラスには、カーソル位置を操作するために
      // 以下のメソッドが利用できる。
      //
      //   ・SetCursorPosition : カーソル位置を設定
      //   ・CursorLeft    : 現在のカーソルの左位置(列)を取得
      //   ・CursorTop     : 現在のカーソルの上位置(行)を取得
      //
      // 上記のメソッドを利用する事で、Linuxなどでよく見かける
      // 処理中状態のカーソルを設定することが出来る。
      //
      Console.WriteLine("処理開始.......");
      
      ShowProgressMark();
      Thread.Sleep(TimeSpan.FromSeconds(5.0));
      
      _stop = true;
      
      Console.WriteLine(string.Empty);
      Console.WriteLine("終了");
    }
    
    void ShowProgressMark()
    {
      //
      // 現在のカーソル位置を保持.
      //
      int left = Console.CursorLeft;
      int top  = Console.CursorTop;
      
      //
      // バッファに書き込み.
      //
      _stop = false;
      
      Task.Factory.StartNew(() =>
        {
          while (true)
          {
            if (_stop)
            {
              break;
            }
            
            Console.SetCursorPosition(left, top);
            Console.Write("|");
            Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
            
            Console.SetCursorPosition(left, top);
            Console.Write("/");
            Thread.Sleep(TimeSpan.FromMilliseconds(100.0));

            Console.SetCursorPosition(left, top);
            Console.Write("-");
            Thread.Sleep(TimeSpan.FromMilliseconds(100.0));

            Console.SetCursorPosition(left, top);
            Console.Write("\\");
            Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
          }
        }
      );
    }
  }
  #endregion
  
  #region DbCommandTimeoutSample-01
  /// <summary>
  /// DbCommandのタイムアウト機能についてのサンプルです。
  /// </summary>
  public class DbCommandTimeoutSample01 : IExecutable
  {
    public void Execute()
    {
      var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
      using (var conn = factory.CreateConnection())
      {
        conn.ConnectionString = @"User Id=medal;Password=medal;Initial Catalog=Medal;Data Source=.\SQLEXPRESS";
        conn.Open();
        
        using (var command = conn.CreateCommand())
        {
          command.CommandText = @"SELECT a.*, b.* FROM MST_ZIP a FULL OUTER JOIN MST_ZIP b ON a.[publicCode] = b.[publicCode] WHERE a.[cmp] LIKE '%あ%' AND b.[cmpK] LIKE '%ｱ%'";
          command.CommandTimeout = 1;
          
          try
          {
            var watch = Stopwatch.StartNew();
            using (var reader = command.ExecuteReader())
            {
              watch.Stop();
              
              var count = 0;
              /*
              for (; reader.Read(); count++)
              {
              }
              */
              
              Console.WriteLine("COUNT={0}, Elapsed={1}", count, watch.Elapsed);
            }
          }
          catch(DbException ex)
          {
            Console.WriteLine(ex);
          }
        }
      }
    }
  }
  #endregion
  
  #region ByteArraySamples-01
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  public class ByteArraySamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // バイト配列を2進数表示.
      //
      byte[] buf = new byte[4];
      buf[0] = 0;
      buf[1] = 0;
      buf[2] = 0;
      buf[3] = 98;
      
      Console.WriteLine(
            string.Join(
              "", 
              buf.Take(4).Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))
            ));
    }
  }
  #endregion
  
  #region ByteArraySamples-02
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  public class ByteArraySamples02 : IExecutable
  {
    public void Execute()
    {
      //
      // バイト列を16進数文字列へ
      //
      byte[] buf = new byte[5];
      new Random().NextBytes(buf);
      
      Console.WriteLine(BitConverter.ToString(buf));
    }
  }
  #endregion
  
  #region ByteArraySamples-03
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  public class ByteArraySamples03 : IExecutable
  {
    public void Execute()
    {
      //
      // 数値を16進数で表示.
      //
    Console.WriteLine("0x{0:X2}", 12345678);
    }
  }
  #endregion
  
  #region ByteArraySamples-04
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  public class ByteArraySamples04 : IExecutable
  {
    public void Execute()
    {
      //
      // 数値からバイト列へ変換
      //
      int i = 123456;
      byte[] buf = BitConverter.GetBytes(i);
      
      Console.WriteLine(BitConverter.ToString(buf));
    }
  }
  #endregion
  
  #region ByteArraySamples-05
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  public class ByteArraySamples05 : IExecutable
  {
    public void Execute()
    {
      //
      // バイト列を数値に
      //
      byte[] buf = new byte[4];
      new Random().NextBytes(buf);
      
      int i = BitConverter.ToInt32(buf, 0);
      
      Console.WriteLine(i);
    }
  }
  #endregion
  
  #region ByteArraySamples-06
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  public class ByteArraySamples06 : IExecutable
  {
    public void Execute()
    {
      //
      // 文字列をバイト列へ
      //
      string s = "gsf_zero1";
      byte[] buf = Encoding.ASCII.GetBytes(s);
      
      Console.WriteLine(BitConverter.ToString(buf));
    }
  }
  #endregion
  
  #region ByteArraySamples-07
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  public class ByteArraySamples07 : IExecutable
  {
    public void Execute()
    {
      //
      // バイト列を文字列へ.
      //
      string s = "gsf_zero1";
      byte[] buf = Encoding.ASCII.GetBytes(s);
      
      Console.WriteLine(Encoding.ASCII.GetString(buf));
    }
  }
  #endregion
  
  #region ByteArraySamples-08
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  public class ByteArraySamples08 : IExecutable
  {
    public void Execute()
    {
      //
      // 数値をいろいろな基数に変換.
      //
      int i = 123;
      
      Console.WriteLine(Convert.ToString(i, 16));
      Console.WriteLine(Convert.ToString(i, 8));
      Console.WriteLine(Convert.ToString(i, 2));
    }
  }
  #endregion
  
  #region ByteArraySamples-09
  /// <summary>
  /// バイト配列についてのサンプルです。
  /// </summary>
  public class ByteArraySamples09 : IExecutable
  {
    public void Execute()
    {
      //
      // 利用しているアーキテクチャのエンディアンを判定.
      //
      Console.WriteLine(BitConverter.IsLittleEndian);
    }
  }
  #endregion
  
  #region BCDSamples-01
  /// <summary>
  /// BCD変換についてのサンプルです。
  /// </summary>
  public class BCDSamples01 : IExecutable
  {
    public void Execute()
    {
      int  val1 = int.MaxValue;
      long val2 = long.MaxValue;
      
      byte[] bcdVal1 = BCDUtils.ToBCD(val1, 5);
      byte[] bcdVal2 = BCDUtils.ToBCD(val2, 10);
      
      Console.WriteLine("integer value = {0}", val1);
      Console.WriteLine("BCD   value = {0}", BitConverter.ToString(bcdVal1));
      Console.WriteLine("long  value = {0}", val2);
      Console.WriteLine("BCD   value = {0}", BitConverter.ToString(bcdVal2));
      
      int  val3 = BCDUtils.ToInt(bcdVal1);
      long val4 = BCDUtils.ToLong(bcdVal2);
      
      Console.WriteLine("val1 == val3 = {0}", val1 == val3);
      Console.WriteLine("val2 == val4 = {0}", val2 == val4);
    }
    
    /// <summary>
    /// BCD変換を行うユーティリティクラスです。
    /// </summary>
    public static class BCDUtils
    {
      public static int ToInt(byte[] bcd)
      {
        return Convert.ToInt32(ToLong(bcd));
      }

      public static long ToLong(byte[] bcd)
      {
        long result = 0;

        foreach (byte b in bcd)
        {
          int digit1 = b >> 4;
          int digit2 = b & 0x0f;

          result = (result * 100) + (digit1 * 10) + digit2;
        }

        return result;
      }
      
      public static byte[] ToBCD(int num, int byteCount)
      {
        return ToBCD<int>(num, byteCount);
      }
      
      public static byte[] ToBCD(long num, int byteCount)
      {
        return ToBCD<long>(num, byteCount);
      }
      
      private static byte[] ToBCD<T>(T num, int byteCount) where T : struct, IConvertible
      {
        long val = Convert.ToInt64(num);
        
        byte[] bcdNumber = new byte[byteCount];
        for (int i = 1; i <= byteCount; i++)
        {
          long mod = val % 100;

          long digit2 = mod % 10;
          long digit1 = (mod - digit2) / 10;

          bcdNumber[byteCount - i] = Convert.ToByte((digit1 * 16) + digit2);

          val = (val - mod) / 100;
        }

        return bcdNumber;
      }
    }
  }
  #endregion
  
  #region ReflectionSample-03
  /// <summary>
  /// リフレクションのサンプルです。
  /// </summary>
  /// <remarks>
  /// リフレクション実行時のパフォーマンスをアップさせる方法について記述しています。
  /// </remarks>
  public class ReflectionSample03 : IExecutable
  {
    delegate string StringToString(string s);
    
    public void Execute()
    {
      //
      // リフレクションを利用して処理を実行する場合
      // そのままMethodInfoのInvokeを呼んでも良いが
      // 何度も呼ぶ必要がある場合、以下のように一旦delegateに
      // してから実行する方が、パフォーマンスが良い。
      //
      // MethodInfo.Invokeを直接呼ぶパターンでは、毎回レイトバインディング
      // が発生しているが、delegateにしてから呼ぶパターンでは
      // delegateを構築している最初の一回のみレイトバインディングされるからである。
      //
      // 尚、当然一番速いのは本来のメソッドを直接呼ぶパターン。
      //
      
      //
      // MethodInfo.Invokeを利用するパターン.
      //
      MethodInfo mi = typeof(string).GetMethod("Trim", new Type[0]);

      Stopwatch watch = Stopwatch.StartNew();
      for (int i = 0; i < 3000000; i++)
      {
        string result = mi.Invoke("test", null) as string;
      }
      watch.Stop();

      Console.WriteLine("MethodInfo.Invokeを直接呼ぶ: {0}", watch.Elapsed);

      //
      // Delegateを構築して呼ぶパターン.
      //
      StringToString s2s = (StringToString) Delegate.CreateDelegate(typeof(StringToString), mi);
      watch.Reset();
      watch.Start();
      for (int i = 0; i < 3000000; i++)
      {
        string result = s2s("test");
      }
      watch.Stop();

      Console.WriteLine("Delegateを構築して処理: {0}", watch.Elapsed);

      //
      // 本来のメソッドを直接呼ぶパターン.
      //
      watch.Reset();
      watch.Start();
      for (int i = 0; i < 3000000; i++)
      {
      	string result = "test".Trim();
      }
      watch.Stop();

      Console.WriteLine("string.Trimを直接呼ぶ: {0}", watch.Elapsed);
    }
  }
  #endregion
  
  #region DefaultValuesSamples-01
  /// <summary>
  /// 各型のデフォルト値についてのサンプルです。
  /// </summary>
  public class DefaultValuesSamples01 : IExecutable
  {
    class  SampleClass  {}
    struct SampleStruct {}
    
    public void Execute()
    {
      Console.WriteLine("byte   のデフォルト:    {0}",    default(byte));
      Console.WriteLine("char   のデフォルト:    {0}",    default(char) == 0x00);
      Console.WriteLine("short  のデフォルト:    {0}",    default(short));
      Console.WriteLine("ushort のデフォルト:    {0}",    default(ushort));
      Console.WriteLine("int  のデフォルト:    {0}",    default(int));
      Console.WriteLine("uint   のデフォルト:    {0}",    default(uint));
      Console.WriteLine("long   のデフォルト:    {0}",    default(long));
      Console.WriteLine("ulong  のデフォルト:    {0}",    default(ulong));
      Console.WriteLine("float  のデフォルト:    {0}",    default(float));
      Console.WriteLine("double のデフォルト:    {0}",    default(double));
      Console.WriteLine("decimalのデフォルト:    {0}",    default(decimal));
      Console.WriteLine("string のデフォルト:    NULL = {0}", default(string)     == null);
      Console.WriteLine("byte[] のデフォルト:    NULL = {0}", default(byte[])     == null);
      Console.WriteLine("List<string>のデフォルト: NULL = {0}", default(List<string>) == null);
      Console.WriteLine("自前クラスのデフォルト:   NULL = {0}", default(SampleClass)  == null);
      Console.WriteLine("自前構造体のデフォルト:   {0}",    default(SampleStruct));
    }
  }
  #endregion
  
  #region TaskSamples-01
  /// <summary>
  /// タスク並列ライブラリについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// タスク並列ライブラリは、4.0から追加されているライブラリです。
  /// </remarks>
  public class TaskSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // Taskは、タスク並列ライブラリの一部として提供されており
      // 文字通りタスクを並列処理するために利用できる。
      //
      // .NET 4.0まで、非同期処理を行う場合ThreadクラスやThreadPoolクラスが
      // 用意されていたが、利用するのに若干の専門性が必要となるものであった。
      //
      // タスク並列ライブラリは、出来るだけ容易に利用できるようデザインされた
      // 新しいライブラリである。
      //
      // さらにタスク並列ライブラリでは、同時実行の程度を内部で調整してくれることによって
      // CPUを効率的に利用するようになっている。
      //
      // ただし、それでもスレッド処理に関する基礎知識は当然必要となる。
      // (ロック、デッドロック、競合状態など）
      //
      // Taskクラスは、System.Threading.Tasks名前空間に存在する。
      //
      // タスクを利用するのに一番簡単な方法はTaskFactoryのStartNewメソッドを
      // 利用する事である。
      //
      // タスクは内部でスレッドプールを利用しているため、スレッドオブジェクトを
      // 直接作成して開始するよりも軽い負荷で実行できる。
      //
      // タスクにはキャンセル機能がデフォルトで用意されている。(CancellationToken)
      // タスクのキャンセル機能については、別の機会で記述する。
      //
      // タスクには状態管理機能がデフォルトで用意されている。
      // タスクの状態管理機能については、別の機会で記述する。
      //
      
      // 別スレッドでタスクが実行されている事を確認する為に、メインスレッドのスレッドIDを表示
      Console.WriteLine("Main Thread : {0}", Thread.CurrentThread.ManagedThreadId);
      
      //
      // Taskを新規作成して実行.
      //   引数にはActionデリゲートを指定する。
      //
      // Waitメソッドはタスクの終了を待つメソッド。
      //
      Task.Factory.StartNew(DoAction).Wait();
      
      
      //
      // Actionの部分にラムダを指定した版
      //
      Task.Factory.StartNew(() => Console.WriteLine("Lambda : {0}", Thread.CurrentThread.ManagedThreadId) ).Wait();
      
      //
      // 多数のタスクを作成して実行.
      //   Task.WaitAllメソッドは引数で指定されたタスクが全て終了するまで待機するメソッド
      //
      Task.WaitAll(
        Enumerable.Range(1, 20).Select(i => Task.Factory.StartNew(DoActionWithSleep)).ToArray()
      );
    }
    
    private void DoAction()
    {
      Console.WriteLine("DoAction: {0}", Thread.CurrentThread.ManagedThreadId);
    }
    
    private void DoActionWithSleep()
    {
      DoAction();
      Thread.Sleep(200);
    }
  }
  #endregion
  
  #region TaskSamples-02
  /// <summary>
  /// タスク並列ライブラリについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// タスク並列ライブラリは4.0から追加されたライブラリです。
  /// </remarks>
  public class TaskSamples02 : IExecutable
  {
    public void Execute()
    {
      //
      // タスクを直接Newして実行.
      // 
      // タスクは直接Newして実行することも出来る。
      // コンストラクタに実行するActionデリゲートを指定し
      // Startを呼ぶと起動される。
      //
      
      // 別スレッドでタスクが実行されている事を確認する為に、メインスレッドのスレッドIDを表示
      Console.WriteLine("Main Thread : {0}", Thread.CurrentThread.ManagedThreadId);
      
      //
      // Actionデリゲートを明示的に指定.
      //
      Task t = new Task(DoAction);
      t.Start();
      t.Wait();
      
      //
      // ラムダを指定.
      //
      Task t2 = new Task(() => DoAction());
      t2.Start();
      t2.Wait();
      
      //
      // 多数のタスクを作成して実行.
      //
      List<Task> tasks = Enumerable.Range(1, 20).Select(i => new Task(DoActionWithSleep)).ToList();
      
      tasks.ForEach(task => task.Start());
      
      Task.WaitAll(
        tasks.ToArray()
      );
    }
    
    private void DoAction()
    {
      Console.WriteLine("DoAction: {0}", Thread.CurrentThread.ManagedThreadId);
    }
    
    private void DoActionWithSleep()
    {
      DoAction();
      Thread.Sleep(200);
    }
  }
  #endregion
  
  #region TaskSamples-03
  /// <summary>
  /// タスク並列ライブラリについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// タスク並列ライブラリは、.NET 4.0から追加されたライブラリです。
  /// </remarks>
  public class TaskSamples03 : IExecutable
  {
    public void Execute()
    {
      //
      // 入れ子タスクの作成
      //
      // タスクは入れ子にすることも可能。
      //
      // 入れ子のタスクには、以下の2種類が存在する。
      //   ・単純な入れ子タスク（デタッチされた入れ子タスク）
      //   ・子タスク（親のタスクにアタッチされた入れ子タスク）
      //
      // 以下のサンプルでは、単純な入れ子のタスクを作成し実行している。
      // 単純な入れ子のタスクとは、入れ子状態で作成されたタスクが
      // 親のタスクとの関連を持たない状態であることを示す。
      //
      // つまり、親のタスクは子のタスクの終了を待たずに、自身の処理を終了する。
      // 入れ子側のタスクにて、確実に親のタスクの終了前に自分の結果を得る必要がある場合は
      // WaitかResultを用いて、処理を完了させる必要がある。
      //
      // 親との関連を持たない入れ子のタスクは、「デタッチされた入れ子のタスク」と言う。
      //
      // デタッチされた入れ子タスクの作成は、単純に親タスクの中で新たにタスクを生成するだけである。
      //
      
      //
      // 単純な入れ子のタスクを作成.
      //
      Console.WriteLine("外側のタスク開始");
      Task t = new Task(ParentTaskProc);
      t.Start();
      t.Wait();
      Console.WriteLine("外側のタスク終了");
      
    }
    
    void ParentTaskProc()
    {
      PrintTaskId();
    
      //
      // 明示的に、TaskCreationOptionsを指定していないので
      // 以下の入れ子タスクは、「デタッチされた入れ子タスク」
      // として生成される。
      //
      Task detachedTask = new Task(ChildTaskProc, TaskCreationOptions.None);
      detachedTask.Start();
      
      //
      // 以下のWaitをコメントアウトすると
      // 出力が
      //     外側のタスク開始
      //      Task Id: 1
      //     内側のタスク開始
      //      Task Id: 2
      //     外側のタスク終了
      //
      // と出力され、「内側のタスク終了」の出力がされないまま
      // メイン処理が終了したりする。
      //
      // これは、2つのタスクが親子関係を持っていないため
      // 別々で処理が行われているからである。
      //
      detachedTask.Wait();
    }
    
    void ChildTaskProc()
    {
      Console.WriteLine("内側のタスク開始");
      PrintTaskId();
      Thread.Sleep(TimeSpan.FromSeconds(2.0));
      Console.WriteLine("内側のタスク終了");
    }
    
    void PrintTaskId()
    {
      //
      // 現在実行中のタスクのIDを表示.
      //
      Console.WriteLine("\tTask Id: {0}", Task.CurrentId);
    }
  }
  #endregion
  
  #region TaskSamples-04
  /// <summary>
  /// タスク並列ライブラリについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// タスク並列ライブラリは、.NET 4.0から追加されたライブラリです。
  /// </remarks>
  public class TaskSamples04 : IExecutable
  {
    public void Execute()
    {
      //
      // 入れ子タスクの作成
      //
      // タスクは入れ子にすることも可能。
      //
      // 入れ子のタスクには、以下の2種類が存在する。
      //   ・単純な入れ子タスク（デタッチされた入れ子タスク）
      //   ・子タスク（親のタスクにアタッチされた入れ子タスク）
      //
      // 以下のサンプルでは、子タスクを作成して実行している。
      // 子タスクとは、単純な入れ子タスクと違い、親タスクと親子関係を
      // 持った状態でタスク処理が行われる。
      //
      // つまり、親のタスクは子のタスクの終了を待ってから、自身の処理を終了する。
      //
      // 親との関連を持つ入れ子のタスクは、「アタッチされた入れ子のタスク」と言う。
      //
      // アタッチされた入れ子タスクの作成は、タスクを生成する際に以下のTaskCreationOptionsを
      // 指定する。
      //   TaskCreationOptions.AttachedToParent
      //
      
      //
      // 親子関係を持つ子タスクを作成.
      //
      Console.WriteLine("親のタスク開始");
      Task t = new Task(ParentTaskProc);
      t.Start();
      t.Wait();
      Console.WriteLine("親のタスク終了");
    }
    
    void ParentTaskProc()
    {
      PrintTaskId();
      
      //
      // 明示的にTaskCreationOptionsを指定して
      // アタッチされた入れ子タスクを指定する。
      //
      Task childTask = new Task(ChildTaskProc, TaskCreationOptions.AttachedToParent);
      childTask.Start();
      
      //
      // 「デタッチされた入れ子タスク」と違い、親タスクにアタッチされた入れ子タスクは
      // 明示的にWaitをしなくても、親のタスクが子のタスクの終了を待ってくれる。
      //
    }
    
    void ChildTaskProc()
    {
      Console.WriteLine("子のタスク開始");
      PrintTaskId();
      Thread.Sleep(TimeSpan.FromSeconds(2.0));
      Console.WriteLine("子のタスク終了");
    }
    
    void PrintTaskId()
    {
      //
      // 現在実行中のタスクのIDを表示.
      //
      Console.WriteLine("\tTask Id: {0}", Task.CurrentId);
    }
  }
  #endregion
  
  #region TaskSamples-xx
  // Taskのエラー処理のサンプル。
  // 後でちゃんとしたサンプルに置き換えること。
  public class TaskSamplesXX : IExecutable
  {
    public void Execute()
    {
      Task<int> t = Task<int>.Factory.StartNew(() => { return 1; });
      Task<int> c = t.ContinueWith<int>((antecedent) => { throw new InvalidOperationException("Error in ContinuationTask"); });

      try
      {
        t.Wait();
        c.Wait();
      }
      catch (AggregateException aggEx)
      {
        foreach (Exception ex in aggEx.InnerExceptions)
        {
          Console.WriteLine(ex.Message);
        }
      }
    }
  }
  #endregion
  
  #region WindowsFormsSynchronizationContextSamples-01
  /// <summary>
  /// WindowsFormsSynchronizationContextクラスについてのサンプルです。
  /// </summary>
  /// <!-- <remarks>
  /// WindowsFormsSynchronizationContextは、SynchronizationContextクラスの派生クラスです。
  /// デフォルトでは、Windows Formsにて、最初のフォームが作成された際に自動的に設定されます。
  /// (AutoInstall静的プロパティにて、動作を変更可能。）
  /// </remakrs>
  public class WindowsFormsSynchronizationContextSamples01 : IExecutable
  {
    class SampleForm : WinFormsForm
    {
      public string ContextTypeName { get; set; }
    
      public SampleForm()
      {
        Load += (s, e) =>
        {
          //
          // UIスレッドのスレッドIDを表示.
          //
          PrintMessageAndThreadId("UI Thread");
          
          //
          // 現在の同期コンテキストを取得.
          //   Windows Formsの場合は、WinFormsSynchronizationContextとなる。
          //
          SynchronizationContext context = SynchronizationContext.Current;
          ContextTypeName = context.ToString();
          
          //
          // Sendは、同期コンテキストに対して同期メッセージを送る。
          // Postは、同期コンテキストに対して非同期メッセージを送る。
          //
          // つまり、SendMessageとPostMessageと同じ.
          //
          context.Send((obj) => { PrintMessageAndThreadId("Send"); }, null);
          context.Post((obj) => { PrintMessageAndThreadId("Post"); }, null);
          
          //
          // UIスレッドと関係ない別のスレッド.
          //
          Task.Factory.StartNew(() => { PrintMessageAndThreadId("Task.Factory"); });
          
          PrintMessageAndThreadId("Form.Load");
          Close();
        };
        
        FormClosing += (s, e) => 
        {
          //
          // SendとPostを呼び出し、どのタイミングで出力されるか確認.
          //
          SynchronizationContext context = SynchronizationContext.Current;
          context.Send((obj) => { PrintMessageAndThreadId("Send--2"); }, null);
          context.Post((obj) => { PrintMessageAndThreadId("Post--2"); }, null);
          
          //
          // UIスレッドと関係ない別のスレッド.
          //
          Task.Factory.StartNew(() => { PrintMessageAndThreadId("Task.Factory"); });
          
          PrintMessageAndThreadId("Form.FormClosing");
        };
        
        FormClosed += (s, e) =>
        {
          //
          // SendとPostを呼び出し、どのタイミングで出力されるか確認.
          //
          SynchronizationContext context = SynchronizationContext.Current;
          context.Send((obj) => { PrintMessageAndThreadId("Send--3"); }, null);
          context.Post((obj) => { PrintMessageAndThreadId("Post--3"); }, null);
  
          //
          // UIスレッドと関係ない別のスレッド.
          //
          Task.Factory.StartNew(() => { PrintMessageAndThreadId("Task.Factory"); });
          
          PrintMessageAndThreadId("Form.FormClosed");
        };
      }
      
      private void PrintMessageAndThreadId(string message)
      {
        Console.WriteLine("{0,-17}, スレッドID: {1}", message, Thread.CurrentThread.ManagedThreadId);
      }
    }
    
    [STAThread]
    public void Execute()
    {
      //
      // SynchronizationContextは、同期コンテキストを様々な同期モデルに反映させるための
      // 処理を提供するクラスである。
      //
      // 派生クラスとして以下のクラスが存在する。
      //   ・WindowsFormsSynchronizationContext   (WinForms用)
      //   ・DispatcherSynchronizationContext   (WPF用)
      //
      // 基本的に、WinFormsもしくはWPFを利用している状態で
      // UIスレッドとは別のスレッドから、UIを更新する際に裏で利用されているクラスである。
      // (BackgroundWorkerも、このクラスを利用してUIスレッドに更新をかけている。）
      //
      // 現在のスレッドのSynchronizationContextを取得するには、Current静的プロパティを利用する。
      // 特定のSynchronizationContextを強制的に設定するには、SetSynchronizationContextメソッドを利用する。
      //
      // デフォルトでは、独自に作成したスレッドの場合
      // SynchronizationContext.Currentの戻り値はnullとなる。
      //
      Console.WriteLine(
        "現在のスレッドでのSynchronizationContextの状態：{0}", 
        SynchronizationContext.Current == null
          ? "NULL"
          : SynchronizationContext.Current.ToString()
      );
      
      //
      // フォームを起動し、値を確認.
      //
      WinFormsApplication.EnableVisualStyles();
      
      SampleForm aForm = new SampleForm();
      WinFormsApplication.Run(aForm);
      
      Console.WriteLine("WinFormsでのSynchronizationContextの型名：{0}", aForm.ContextTypeName);
    }
  }
  #endregion
  
  #region MonitorSample-01
  /// <summary>
  /// Monitorクラスについてのサンプルです。
  /// </summary>
  public class MonitorSamples01 : IExecutable
  {
    object _lock = new object();
    bool   _go;
    
    public void Execute()
    {
      new Thread(Waiter).Start();
      
      Thread.Sleep(TimeSpan.FromSeconds(1));
      lock (_lock)
      {
        _go = true;
        //
        // ブロックしているスレッドに対して、通知を発行.
        //   Monitor.Pulseは、lock内でしか実行できない.
        //
        Monitor.Pulse(_lock);
      }
      
      Console.WriteLine("Main thread end.");
    }
    
    void Waiter()
    {
      lock (_lock)
      {
        while (!_go)
        {
          //
          // 通知が来るまで、スレッドをブロック.
          //   Monitor.Waitは、lock内でしか実行できない.
          //
          Monitor.Wait(_lock);
        }
      }
      
      Console.WriteLine("awake!!");
    }
  }
  #endregion
  
  #region ManualResetEventSlimSamples-01
  /// <summary>
  /// ManualResetEventSlimクラスについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// ManualResetEventSlimクラスは、.NET 4.0で追加されたクラスです。
  /// 元々存在していたManualResetEventクラスよりも軽量なクラスとなっています。
  /// 特徴しては、以下の点が挙げられます。
  ///   ・WaitメソッドにCancellationTokenを受け付けるオーバーロードが存在する。
  ///   ・非常に短い時間の待機の場合、このクラスは待機ハンドルではなくビジースピンを利用して待機する。
  /// </remarks>
  public class ManualResetEventSlimSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // 通常の使い方.
      //
      ManualResetEventSlim mres = new ManualResetEventSlim(false);
      
      ThreadPool.QueueUserWorkItem(DoProc, mres);
      
      Console.Write("メインスレッド待機中・・・");
      mres.Wait();
      Console.WriteLine("終了");
      
      //
      // WaitメソッドにCancellationTokenを受け付けるオーバーロードを使用。
      //
      mres.Reset();
      
      CancellationTokenSource tokenSource = new CancellationTokenSource();
      CancellationToken     token     = tokenSource.Token;
      
      Task task = Task.Factory.StartNew(DoProc, mres);
      
      //
      // キャンセル状態に設定.
      //
      tokenSource.Cancel();
      
      Console.Write("メインスレッド待機中・・・");
  
      try
      {
        //
        // CancellationTokenを指定して、Wait呼び出し。
        // この場合は、以下のどちらかの条件を満たした時点でWaitが解除される。
        //  ・別の場所にて、Setが呼ばれてシグナル状態となる。
        //  ・CancellationTokenがキャンセルされる。
        //
        // トークンがキャンセルされた場合、OperationCanceledExceptionが発生するので
        // CancellationTokenを指定するWaitを呼び出す場合は、try-catchが必須となる。
        //
        // 今回の例の場合は、予めCancellationTokenをキャンセルしているので
        // タスク処理でシグナル状態に設定されるよりも先に、キャンセル状態に設定される。
        // なので、実行結果には、「*** シグナル状態に設定 ***」という文言は出力されない。
        //
        mres.Wait(token);
      }
      catch (OperationCanceledException cancelEx)
      {
        Console.Write("*** {0} *** ", cancelEx.Message);
      }
  
      Console.WriteLine("終了");
    }
    
    void DoProc(object stateObj)
    {
      Thread.Sleep(TimeSpan.FromSeconds(1));
      Console.Write("*** シグナル状態に設定 *** ");
      (stateObj as ManualResetEventSlim).Set();
    }
  }
  #endregion
  
  #region CountDownEventSamples-01
  /// <summary>
  /// CountdownEventクラスについてのサンプルです。(1)
  /// </summary>
  /// <remarks>
  /// CountdownEventクラスは、.NET 4.0から追加されたクラスです。
  /// JavaのCountDownLatchクラスと同じ機能を持っています。
  /// </remarks>
  public class CountdownEventSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // 初期カウントが1のCountdownEventオブジェクトを作成.
      //
      // この場合、どこかの処理にてカウントを一つ減らす必要がある。
      // カウントが残っている状態でWaitをしていると、いつまでたってもWaitを
      // 抜けることが出来ない。
      //
      using (CountdownEvent cde = new CountdownEvent(1))
      {
        // 初期の状態を表示.
        Console.WriteLine("InitialCount={0}", cde.InitialCount);
        Console.WriteLine("CurrentCount={0}", cde.CurrentCount);
        Console.WriteLine("IsSet={0}", cde.IsSet);
        
        Task t = Task.Factory.StartNew(() => 
        {
          Thread.Sleep(TimeSpan.FromSeconds(1));
          
          //
          // カウントをデクリメント.
          //
          // Signalメソッドを引数なしで呼ぶと、１つカウントを減らすことが出来る。
          // (指定した数分、カウントをデクリメントするオーバーロードも存在する。)
          //
          // CountdownEvent.CurrentCountが0の状態で、さらにSignalメソッドを呼び出すと
          // InvalidOperationException (イベントのカウントを 0 より小さい値にデクリメントしようとしました。)が
          // 発生する。
          //
          cde.Signal();
          cde.Signal(); // このタイミングで例外が発生する.
        });
        
        try
        {
          t.Wait();
        }
        catch (AggregateException aggEx)
        {
          foreach (Exception innerEx in aggEx.Flatten().InnerExceptions)
          {
            Console.WriteLine("ERROR={0}", innerEx.Message);
          }
        }
  
        //
        // カウントが0になるまで待機.
        //
        cde.Wait();
        
        // 現在の状態を表示.
        Console.WriteLine("InitialCount={0}", cde.InitialCount);
        Console.WriteLine("CurrentCount={0}", cde.CurrentCount);
        Console.WriteLine("IsSet={0}", cde.IsSet);
      }
    }
  }
  #endregion
  
  #region CountdownEventSamples-02
  /// <summary>
  /// CountdownEventクラスについてのサンプルです。(2)
  /// </summary>
  /// <remarks>
  /// CountdownEventクラスは、.NET 4.0から追加されたクラスです。
  /// JavaのCountDownLatchクラスと同じ機能を持っています。
  /// </remarks>
  public class CountdownEventSamples02 : IExecutable
  {
    public void Execute()
    {
      const int LEAST_TASK_FINISH_COUNT = 3;
      
      //
      // 複数のスレッドから一つのCountdownEventをシグナルする.
      //
      // CountdownEventがよく利用されるパターンとなる。
      // N個の処理が規定数終了するまで、メインスレッドの続行を待機するイメージ.
      //
      // 以下の処理では、5つタスクを作成して、3つ終わった時点で
      // メインスレッドは処理を続行するようにする.
      //
      // N個の処理が全部終了するまで、メインスレッドの続行を待機する場合は
      // CountdownEventのカウントをタスクの処理数と同じにすれば良い。
      //
      using (CountdownEvent cde = new CountdownEvent(LEAST_TASK_FINISH_COUNT))
      {
        // 初期の状態を表示.
        Console.WriteLine("InitialCount={0}", cde.InitialCount);
        Console.WriteLine("CurrentCount={0}", cde.CurrentCount);
        Console.WriteLine("IsSet={0}", cde.IsSet);
        
        Task[] tasks = new Task[]
        {
          Task.Factory.StartNew(TaskProc, cde),
          Task.Factory.StartNew(TaskProc, cde),
          Task.Factory.StartNew(TaskProc, cde),
          Task.Factory.StartNew(TaskProc, cde),
          Task.Factory.StartNew(TaskProc, cde)
        };
        
        //
        // 3つ終わるまで待機.
        //
        cde.Wait();
        Console.WriteLine("5つのタスクの内、3つ終了");
        
        Console.WriteLine("メインスレッド 続行開始・・・");
        Thread.Sleep(TimeSpan.FromSeconds(1));
        
        //
        // 残りのタスクを待機.
        //
        Task.WaitAll(tasks);
        Console.WriteLine("全てのタスク終了");
        
        // 現在の状態を表示.
        Console.WriteLine("InitialCount={0}", cde.InitialCount);
        Console.WriteLine("CurrentCount={0}", cde.CurrentCount);
        Console.WriteLine("IsSet={0}", cde.IsSet);
      }
    }
    
    void TaskProc(object data)
    {
      Console.WriteLine("Task ID={0} 開始", Task.CurrentId);
      Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(10)));
      
      //
      // 既に3つ終了しているか否かを確認し、まだならシグナル.
      //
      CountdownEvent cde = data as CountdownEvent;
      if (!cde.IsSet)
      {
        cde.Signal();
        Console.WriteLine("＊＊＊カウントをデクリメント＊＊＊ Task ID={0} CountdownEvent.CurrentCount={1}", Task.CurrentId, cde.CurrentCount);
      }
      
      Console.WriteLine("Task ID={0} 終了", Task.CurrentId);
    }
  }
  #endregion
  
  #region CountdownEventSamples-03
  /// <summary>
  /// CountdownEventクラスについてのサンプルです。(3)
  /// </summary>
  /// <remarks>
  /// CountdownEventクラスは、.NET 4.0から追加されたクラスです。
  /// JavaのCountDownLatchクラスと同じ機能を持っています。
  /// </remarks>
  public class CountdownEventSamples03 : IExecutable
  {
    public void Execute()
    {
      //
      // CountdownEventには、CancellationTokenを受け付けるWaitメソッドが存在する.
      // 使い方は、ManualResetEventSlimクラスの場合と同じ。
      // 
      // 参考リソース:
      //   .NET クラスライブラリ探訪-042 (System.Threading.ManualResetEventSlim)
      //   http://d.hatena.ne.jp/gsf_zero1/20110323/p1
      //
      CancellationTokenSource tokenSource = new CancellationTokenSource();
      CancellationToken     token     = tokenSource.Token;
      
      using (CountdownEvent cde = new CountdownEvent(1))
      {
        // 初期の状態を表示.
        Console.WriteLine("InitialCount={0}", cde.InitialCount);
        Console.WriteLine("CurrentCount={0}", cde.CurrentCount);
        Console.WriteLine("IsSet={0}",    cde.IsSet);
        
        Task t = Task.Factory.StartNew(() =>
        {
          Thread.Sleep(TimeSpan.FromSeconds(2));
          
          token.ThrowIfCancellationRequested();
          cde.Signal();
        }, token);
        
        //
        // 処理をキャンセル.
        //
        tokenSource.Cancel();
        
        try
        {
          cde.Wait(token);
        }
        catch (OperationCanceledException cancelEx)
        {
          if (token == cancelEx.CancellationToken)
          {
            Console.WriteLine("＊＊＊CountdownEvent.Wait()がキャンセルされました＊＊＊");
          }
        }
        
        try
        {
          t.Wait();
        }
        catch (AggregateException aggEx)
        {
          aggEx.Handle(ex => 
          {
            if (ex is OperationCanceledException)
            {
              OperationCanceledException cancelEx = ex as OperationCanceledException;
              
              if (token == cancelEx.CancellationToken)
              {
                Console.WriteLine("＊＊＊タスクがキャンセルされました＊＊＊");
                return true;
              }
            }
            
            return false;
          });
        }
        
        // 現在の状態を表示.
        Console.WriteLine("InitialCount={0}", cde.InitialCount);
        Console.WriteLine("CurrentCount={0}", cde.CurrentCount);
        Console.WriteLine("IsSet={0}", cde.IsSet);
      }
    }
  }
  #endregion
  
  #region CountdownEventSamples-04
  /// <summary>
  /// CountdownEventクラスについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// CountdownEventクラスは、.NET 4.0から追加されたクラスです。
  /// </remarks>
  public class CountdownEventSamples04 : IExecutable
  {
    public void Execute()
    {
      //
      // CountdownEventクラスには、以下のメソッドが存在する。
      //   ・AddCountメソッド
      //   ・Resetメソッド
      // AddCountメソッドは、CountdownEventの内部カウントをインクリメントする。
      // Resetメソッドは、現在の内部カウントをリセットする。
      //
      // どちらのメソッドも、Int32を引数に取るオーバーロードが用意されており
      // 指定した数を設定することも出来る。
      //
      // 尚、AddCountメソッドを利用する際の注意点として
      //   既に内部カウントが0の状態でAddCountを実行すると例外が発生する。
      // つまり、既にIsSetがTrue（シグナル状態）でAddCountするとエラーとなる。
      //
      
      //
      // 内部カウントが0の状態で、AddCountしてみる.
      //
      using (CountdownEvent cde = new CountdownEvent(0))
      {
        // 初期の状態を表示.
        PrintCurrentCountdownEvent(cde);
        
        try
        {
          //
          // 既にシグナル状態の場合に、さらにAddCountしようとすると例外が発生する.
          //
          cde.AddCount();
        }
        catch (InvalidOperationException invalidEx)
        {
          Console.WriteLine("＊＊＊ {0} ＊＊＊", invalidEx.Message);
        }
        
        // 現在の状態を表示.
        PrintCurrentCountdownEvent(cde);
      }
      
      Console.WriteLine("");
      
      using (CountdownEvent cde = new CountdownEvent(1))
      {
        // 初期の状態を表示.
        PrintCurrentCountdownEvent(cde);
        
        //
        // 10個の別処理を実行する.
        // それぞれの内部処理にてランダムでSLEEPして、終了タイミングをバラバラに設定.
        //
        Console.WriteLine("別処理開始・・・");
        
        for (int i = 0; i < 10; i++)
        {
          Task.Factory.StartNew(TaskProc, cde);
        }
        
        do
        {
          // 現在の状態を表示.
          PrintCurrentCountdownEvent(cde, "t");
          
          Thread.Sleep(TimeSpan.FromSeconds(2));
        }
        while (cde.CurrentCount != 1);
        
        Console.WriteLine("・・・別処理終了");
        
        //
        // 待機.
        //
        Console.WriteLine("メインスレッドにて最後のカウントをデクリメント");
        cde.Signal();
        cde.Wait();
        
        // 現在の状態を表示.
        PrintCurrentCountdownEvent(cde);
  
        Console.WriteLine("");
        
        //
        // 内部カウントをリセット.
        //
        Console.WriteLine("内部カウントをリセット");
        cde.Reset();
        
        // 現在の状態を表示.
        PrintCurrentCountdownEvent(cde);
        
        //
        // 待機.
        //
        Console.WriteLine("メインスレッドにて最後のカウントをデクリメント");
        cde.Signal();
        cde.Wait();
        
        // 現在の状態を表示.
        PrintCurrentCountdownEvent(cde);
      }
    }
    
    void PrintCurrentCountdownEvent(CountdownEvent cde, string prefix = "")
    {
      Console.WriteLine("{0}InitialCount={1}", prefix, cde.InitialCount);
      Console.WriteLine("{0}CurrentCount={1}", prefix, cde.CurrentCount);
      Console.WriteLine("{0}IsSet={1}",    prefix, cde.IsSet);
    }
    
    void TaskProc(object data)
    {
      //
      // 処理開始と共に、CountdownEventの内部カウントをインクリメント.
      //
      CountdownEvent cde = data as CountdownEvent;
      cde.AddCount();
      
      Thread.Sleep(TimeSpan.FromSeconds(new Random().Next(10)));
      
      //
      // 内部カウントをデクリメント.
      //
      cde.Signal();
    }
  }
  #endregion
  
  #region BarrierSamples-01
  /// <summary>
  /// Barrierクラスについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// Barrierクラスは、.NET 4.0から追加されたクラスです。
  /// </remarks>
  public class BarrierSamples01 : IExecutable
  {
    // 計算値を保持する変数
    long _count;
    
    public void Execute()
    {
      //
      // Barrierクラスは、並行処理を複数のフェーズ毎に協調動作させる場合に利用する.
      // つまり、同時実行操作を同期する際に利用出来る。
      //
      // 例えば、論理的に3フェーズ存在する処理があったとして、並行して動作する処理が2つあるとする。
      // 各並行処理に対して、フェーズ毎に一旦結果を収集し、また平行して処理を行う事とする。
      // そのような場合に、Barrierクラスが役に立つ。
      //
      // Barrierクラスをインスタンス化する際に、対象となる並行処理の数をコンストラクタに指定する。
      // コンストラクタには、フェーズ毎に実行されるコールバックを設定することも出来る。
      //
      // 後は、Barrier.SignalAndWaitを、各並行処理が呼び出せば良い。
      // コンストラクタに指定した数分、SignalAndWaitが呼び出された時点で1フェーズ終了となり
      // 設定したコールバックが実行される。
      //
      // 各並行処理は、SignalAndWaitを呼び出した後、Barrierにて指定した処理数分のSignalAndWaitが
      // 呼び出されるまで、ブロックされる。
      //
      // 対象とする並行処理数は、以下のメソッドを利用することにより増減させることが出来る。
      //   ・AddParticipants
      //   ・RemoveParticipants
      //
      // CountdownEvent, ManualResetEventSlimと同じく、このクラスのSignalAndWaitメソッドも
      // CancellationTokenを受け付けるオーバーロードが存在する。
      //
      // CountdownEventと同じく、このクラスもIDisposableを実装しているのでusing可能。
      //
      
      //
      // 5つの処理を、特定のフェーズ毎に同期させながら実行.
      // さらに、フェーズ単位で途中結果を出力するようにする.
      //
      using (Barrier barrier = new Barrier(5, PostPhaseProc))
      {
        Parallel.Invoke(
          () => ParallelProc(barrier, 10, 123456, 2), 
          () => ParallelProc(barrier, 20, 678910, 3),
          () => ParallelProc(barrier, 30, 749827, 5),
          () => ParallelProc(barrier, 40, 847202, 7),
          () => ParallelProc(barrier, 50, 503295, 777)
        );
      }
      
      Console.WriteLine("最終値：{0}", _count);
    }
    
    //
    // 各並列処理用のアクション.
    //
    void ParallelProc(Barrier barrier, int randomMaxValue, int randomSeed, int modValue)
    {
      //
      // 第一フェーズ.
      //
      Calculate(barrier, randomMaxValue, randomSeed, modValue, 100);
      
      //
      // 第二フェーズ.
      //
      Calculate(barrier, randomMaxValue, randomSeed, modValue, 5000);
      
      //
      // 第三フェーズ.
      //
      Calculate(barrier, randomMaxValue, randomSeed, modValue, 10000);
    }
    
    //
    // 計算処理.
    //
    void Calculate(Barrier barrier, int randomMaxValue, int randomSeed, int modValue, int loopCountMaxValue)
    {
      Random  rnd   = new Random(randomSeed);
      Stopwatch watch = Stopwatch.StartNew();
      
      int loopCount = rnd.Next(loopCountMaxValue);
      Console.WriteLine("[Phase{0}] ループカウント：{1}, TASK:{2}", barrier.CurrentPhaseNumber, loopCount, Task.CurrentId);
      
      for (int i = 0; i < loopCount; i++)
      {
        // 適度に時間がかかるように調整.
        if (rnd.Next(10000) % modValue == 0)
        {
          Thread.Sleep(TimeSpan.FromMilliseconds(10));
        }
        
        Interlocked.Add(ref _count, (i + rnd.Next(randomMaxValue)));
      }
      
      watch.Stop();
      Console.WriteLine("[Phase{0}] SignalAndWait -- TASK:{1}, ELAPSED:{2}", barrier.CurrentPhaseNumber, Task.CurrentId, watch.Elapsed);
      
      try
      {
        //
        // シグナルを発行し、仲間のスレッドが揃うのを待つ.
        //
        barrier.SignalAndWait();
      }
      catch (BarrierPostPhaseException postPhaseEx)
      {
        //
        // Post Phaseアクションにてエラーが発生した場合はここに来る.
        // (本来であれば、キャンセルするなどのエラー処理が必要)
        //
        Console.WriteLine("*** {0} ***", postPhaseEx.Message);
        throw;
      }
    }
    
    //
    // Barrierにて、各フェーズ毎が完了した際に呼ばれるコールバック.
    // (Barrierクラスのコンストラクタにて設定する)
    //
    void PostPhaseProc(Barrier barrier)
    {
      //
      // Post Phaseアクションは、同時実行している処理が全てSignalAndWaitを
      // 呼ばなければ発生しない。
      //
      // つまり、この処理が走っている間、他の同時実行処理は全てブロックされている状態となる。
      //
      long current = Interlocked.Read(ref _count);
      
      Console.WriteLine("現在のフェーズ：{0}, 参加要素数：{1}", barrier.CurrentPhaseNumber, barrier.ParticipantCount);
      Console.WriteLine("t現在値：{0}", current);
      
      //
      // 以下のコメントを外すと、次のPost Phaseアクションにて
      // 全てのSignalAndWaitを呼び出している、処理にてBarrierPostPhaseExceptionが
      // 発生する。
      //
      //throw new InvalidOperationException("dummy");
    }
  }
  #endregion
  
  #region BarrierSamples-02
  /// <summary>
  /// Barrierクラスについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// Barrierクラスは、.NET 4.0から追加されたクラスです。
  /// </remarks>
  public class BarrierSamples02 : IExecutable
  {
    // 計算値を保持する変数
    long _count;
    // キャンセルトークンソース.
    CancellationTokenSource _tokenSource;
    // キャンセルトークン.
    CancellationToken _token;
    
    public void Execute()
    {
      _tokenSource = new CancellationTokenSource();
      _token     = _tokenSource.Token;
  
      //
      // 5つの処理を、特定のフェーズ毎に同期させながら実行.
      // さらに、フェーズ単位で途中結果を出力するようにするが
      // 5秒経過した時点でキャンセルを行う。
      //
      using (Barrier barrier = new Barrier(5, PostPhaseProc))
      {
        
        try
        {
          Parallel.Invoke(
            () => ParallelProc(barrier, 10, 123456, 2), 
            () => ParallelProc(barrier, 20, 678910, 3),
            () => ParallelProc(barrier, 30, 749827, 5),
            () => ParallelProc(barrier, 40, 847202, 7),
            () => ParallelProc(barrier, 50, 503295, 777),
            () => 
            {
              //
              // 5秒間待機した後にキャンセルを行う.
              //
              Thread.Sleep(TimeSpan.FromSeconds(5));
              Console.WriteLine("■■■■　キャンセル　■■■■");
              _tokenSource.Cancel();
            }
          );
        }
        catch (AggregateException aggEx)
        {
          aggEx.Handle(HandleAggregateException);
        }
      }
      
      _tokenSource.Dispose();
      
      Console.WriteLine("最終値：{0}", _count);
    }
    
    //
    // 各並列処理用のアクション.
    //
    void ParallelProc(Barrier barrier, int randomMaxValue, int randomSeed, int modValue)
    {
      //
      // 第一フェーズ.
      //
      Calculate(barrier, randomMaxValue, randomSeed, modValue, 100);
      
      //
      // 第二フェーズ.
      //
      Calculate(barrier, randomMaxValue, randomSeed, modValue, 5000);
      
      //
      // 第三フェーズ.
      //
      Calculate(barrier, randomMaxValue, randomSeed, modValue, 10000);
    }
    
    //
    // 計算処理.
    //
    void Calculate(Barrier barrier, int randomMaxValue, int randomSeed, int modValue, int loopCountMaxValue)
    {
      Random  rnd   = new Random(randomSeed);
      Stopwatch watch = Stopwatch.StartNew();
      
      int loopCount = rnd.Next(loopCountMaxValue);
      Console.WriteLine("[Phase{0}] ループカウント：{1}, TASK:{2}", barrier.CurrentPhaseNumber, loopCount, Task.CurrentId);
      
      for (int i = 0; i < loopCount; i++)
      {
        //
        // キャンセル状態をチェック.
        // 別の場所にてキャンセルが行われている場合は
        // OperationCanceledExceptionが発生する.
        //
        _token.ThrowIfCancellationRequested();
        
        // 適度に時間がかかるように調整.
        if (rnd.Next(10000) % modValue == 0)
        {
          Thread.Sleep(TimeSpan.FromMilliseconds(10));
        }
        
        Interlocked.Add(ref _count, (i + rnd.Next(randomMaxValue)));
      }
      
      watch.Stop();
      Console.WriteLine("[Phase{0}] SignalAndWait -- TASK:{1}, ELAPSED:{2}", barrier.CurrentPhaseNumber, Task.CurrentId, watch.Elapsed);
      
      try
      {
        //
        // シグナルを発行し、仲間のスレッドが揃うのを待つ.
        //
        barrier.SignalAndWait(_token);
      }
      catch (BarrierPostPhaseException postPhaseEx)
      {
        //
        // Post Phaseアクションにてエラーが発生した場合はここに来る.
        // (本来であれば、キャンセルするなどのエラー処理が必要)
        //
        Console.WriteLine("*** {0} ***", postPhaseEx.Message);
        throw;
      }
      catch (OperationCanceledException cancelEx)
      {
        //
        // 別の場所にてキャンセルが行われた.
        //
        // 既に処理が完了してSignalAndWaitを呼び、仲間のスレッドを
        // 待っている状態でキャンセルが発生した場合は
        //    「操作が取り消されました。」となる。
        //
        // SignalAndWaitを呼ぶ前に、既にキャンセル状態となっている状態で
        // SignalAndWaitを呼ぶと
        //    「操作がキャンセルされました。」となる。
        //
        Console.WriteLine("キャンセルされました -- MESSAGE:{0}, TASK:{1}", cancelEx.Message, Task.CurrentId);
        throw;
      }
    }
    
    //
    // Barrierにて、各フェーズ毎が完了した際に呼ばれるコールバック.
    // (Barrierクラスのコンストラクタにて設定する)
    //
    void PostPhaseProc(Barrier barrier)
    {
      //
      // Post Phaseアクションは、同時実行している処理が全てSignalAndWaitを
      // 呼ばなければ発生しない。
      //
      // つまり、この処理が走っている間、他の同時実行処理は全てブロックされている状態となる。
      //
      long current = Interlocked.Read(ref _count);
      
      Console.WriteLine("現在のフェーズ：{0}, 参加要素数：{1}", barrier.CurrentPhaseNumber, barrier.ParticipantCount);
      Console.WriteLine("t現在値：{0}", current);
      
      //
      // 以下のコメントを外すと、次のPost Phaseアクションにて
      // 全てのSignalAndWaitを呼び出している、処理にてBarrierPostPhaseExceptionが
      // 発生する。
      //
      //throw new InvalidOperationException("dummy");
    }
    
    //
    // AggregateException.Handleメソッドに設定されるコールバック.
    //
    bool HandleAggregateException(Exception ex)
    {
      if (ex is OperationCanceledException)
      {
        OperationCanceledException cancelEx = ex as OperationCanceledException;
        if (_token == cancelEx.CancellationToken)
        {
          Console.WriteLine("＊＊＊Barrier内の処理がキャンセルされた MESSAGE={0} ＊＊＊", cancelEx.Message);
          return true;
        }
      }
      
      return false;
    }
  }
  #endregion
  
  #region SemaphoreSlimSamples-01
  /// <summary>
  /// SemaphoreSlimクラスについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// SemaphoreSlimクラスは、.NET 4.0から追加されたクラスです。
  /// 従来から存在していたSemaphoreクラスの軽量版となります。
  /// </remarks>
  public class SemaphoreSlimSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // SemaphoreSlimクラスは、Semaphoreクラスの軽量版として
      // .NET 4.0から追加されたクラスである。
      //
      // Semaphoreは、リソースに同時にアクセス出来るスレッドの数を制限するために利用される。
      //
      // 機能的には、Semaphoreクラスと大差ないが以下の機能が追加されている。
      //   キャンセルトークンを受け付けるWaitメソッドのオーバーロードが存在する。
      // キャンセルトークンを受け付けるWaitメソッドに関しては、CountdownEventクラスやBarrierクラス
      // と利用方法は同じである。
      //
      // 尚、元々のSemaphoreクラスでは、WaitOneメソッドだったものが
      // SemaphoreSlimクラスでは、Waitメソッドという名前に変わっている。
      //
      
      //
      // Waitメソッドの利用.
      // 
      // Waitメソッドは、入ることが出来た場合はTrueを返す。
      // 既に上限までスレッドが入っている場合はFalseが返却される。
      // (つまりブロックされる。)
      //
      // 引数無しのWaitメソッドは、入ることが出来るまでブロックされるメソッドとなる。
      // 結果をboolで受け取る場合は、Int32を引数にとるWaitメソッドを利用する。
      // 0を指定すると即結果が返ってくる。-1を指定すると無制限に待つ。
      // (引数無しのWaitメソッドと同じ。)
      //
      // SemaphoreSlimでは、AvailableWaitHandleプロパティよりWaitHandleを取得することが出来る。
      // ただし、このWaitHandleは、SemaphoreSlim本体とは連携しているわけでは無い。
      // なので、このWaitHandle経由でWaitOneを実行しても、SemaphoreSlim側のカウントは変化しないので注意。
      //
      using (SemaphoreSlim semaphore = new SemaphoreSlim(2))
      {
        // 現在Semaphoreに入ることが可能なスレッド数を表示
        Console.WriteLine("CurrentCount={0}", semaphore.CurrentCount);
        
        // 1つ目
        Console.WriteLine("1つ目のWait={0}", semaphore.Wait(0));
        // 2つ目
        Console.WriteLine("2つ目のWait={0}", semaphore.Wait(0));
        
        // 現在Semaphoreに入ることが可能なスレッド数を表示
        Console.WriteLine("CurrentCount={0}", semaphore.CurrentCount);
        
        // 3つ目
        // 現在Releaseしている数は0なので、入ることが出来ない。
        // (Falseが返却される)
        Console.WriteLine("3つ目のWait={0}", semaphore.Wait(0));
        
        // １つリリースして、枠を空ける.
        semaphore.Release();
        
        // 現在Semaphoreに入ることが可能なスレッド数を表示
        Console.WriteLine("CurrentCount={0}", semaphore.CurrentCount);
        
        // 再度、3つ目
        // 今度は、枠が空いているので入ることが出来る。
        Console.WriteLine("3つ目のWait={0}", semaphore.Wait(0));
        
        // 現在Semaphoreに入ることが可能なスレッド数を表示
        Console.WriteLine("CurrentCount={0}", semaphore.CurrentCount);
        
        semaphore.Release();
        semaphore.Release();
  
        // 現在Semaphoreに入ることが可能なスレッド数を表示
        Console.WriteLine("CurrentCount={0}", semaphore.CurrentCount);
      }
    }
  }
  #endregion
  
  #region SemaphoreSlimSamples-02
  /// <summary>
  /// SemaphoreSlimクラスについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// SemaphoreSlimクラスは、.NET 4.0から追加されたクラスです。
  /// 従来から存在していたSemaphoreクラスの軽量版となります。
  /// </remarks>
  public class SemaphoreSlimSamples02 : IExecutable
  {
    public void Execute()
    {
      //
      // SemaphoreSlimのWaitメソッドにはキャンセルトークンを
      // 受け付けるオーバーロードが存在する。
      //
      // CountdownEventやBarrierの場合と同じく、Waitメソッドに
      // キャンセルトークンを指定した場合、別の場所にてキャンセルが
      // 行われると、OperationCanceledExceptionが発生する。
      //
      const int timeout = 2000;
      
      CancellationTokenSource tokenSource = new CancellationTokenSource();
      CancellationToken     token     = tokenSource.Token;
      
      using (SemaphoreSlim semaphore = new SemaphoreSlim(2))
      {
        //
        // あらかじめ、セマフォの上限までWaitしておき
        // 後のスレッドが入れないようにしておく.
        //
        semaphore.Wait();
        semaphore.Wait();
        
        //
        // ３つのタスクを作成する.
        //  １つ目のタスク：キャンセルトークンを指定して無制限待機.
        //  ２つ目のタスク：キャンセルトークンとタイムアウト値を指定して待機.
        //  ３つ目のタスク：特定時間待機した後、キャンセル処理を行う.
        //
        Parallel.Invoke
          (
            () => WaitProc1(semaphore, token),
            () => WaitProc2(semaphore, timeout, token),
            () => DoCancel(timeout, tokenSource)
          );
        
        semaphore.Release();
        semaphore.Release();
        Console.WriteLine("CurrentCount={0}", semaphore.CurrentCount);
      }
    }
    
    // キャンセルトークンを指定して無制限待機.
    void WaitProc1(SemaphoreSlim semaphore, CancellationToken token)
    {
      try
      {
        Console.WriteLine("WaitProc1=待機開始");
        semaphore.Wait(token);
      }
      catch (OperationCanceledException cancelEx)
      {
        Console.WriteLine("WaitProc1={0}", cancelEx.Message);
      }
      finally
      {
        Console.WriteLine("WaitProc1_CurrentCount={0}", semaphore.CurrentCount);
      }
    }
    
    // キャンセルトークンとタイムアウト値を指定して待機.
    void WaitProc2(SemaphoreSlim semaphore, int timeout, CancellationToken token)
    {
      try
      {
        bool isSuccess = semaphore.Wait(timeout, token);
        if (!isSuccess)
        {
          Console.WriteLine("WaitProc2={0}t★★タイムアウト★★", isSuccess);
        }
      }
      catch (OperationCanceledException cancelEx)
      {
        Console.WriteLine("WaitProc2={0}", cancelEx.Message);
      }
      finally
      {
        Console.WriteLine("WaitProc2_CurrentCount={0}", semaphore.CurrentCount);
      }
    }
    
    // 特定時間待機した後、キャンセル処理を行う.
    void DoCancel(int timeout, CancellationTokenSource tokenSource)
    {
      Console.WriteLine("待機開始：{0}msec", timeout + 1000);
      Thread.Sleep(timeout + 1000);
      
      Console.WriteLine("待機終了");
      Console.WriteLine("★★キャンセル発行★★");
      tokenSource.Cancel();
    }
  }
  #endregion
  
  #region ReaderWriterLockSlimSamples-01
  /// <summary>
  /// ReaderWriterLockSlimクラスについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// ReaderWriterLockSlimクラスは、.NET 4.0から追加されたクラスです。
  /// 従来から存在するReaderWriterLockクラスの軽量版という位置づけになっています。
  ///
  /// しかし、MSDNに以下のように記述されているように今後はこのクラスを利用する方がいいです。
  /// [MSDNから抜粋] (http://msdn.microsoft.com/ja-jp/library/system.threading.readerwriterlockslim.aspx)
  ///   ReaderWriterLockSlim は ReaderWriterLock と似ていますが、再帰の規則や
  ///   ロック状態のアップグレードおよびダウングレードの規則が簡素化されています。 
  ///   ReaderWriterLockSlim は、デッドロックの可能性を大幅に回避します。 
  ///   さらに、ReaderWriterLockSlim のパフォーマンスは ReaderWriterLock と比較して格段に優れています。 
  ///   すべての新規開発で、ReaderWriterLockSlim を使用することをお勧めします。 
  /// </remarks>
  public class ReaderWriterLockSlimSamples01 : IExecutable
  {
    public void Execute()
    {
    }
  }
  #endregion
  
  #region InterlockedSamples-01
  public class InterlockedSamples01 : IExecutable
  {
    public void Execute()
    {
    }
  }
  #endregion
  
  #region ConcurrentQueueSamples-01
  public class ConcurrentQueueSamples01 : IExecutable
  {
    public void Execute()
    {
    }
  }
  #endregion
  
  #region ConcurrentDictionarySamples-01
  public class ConcurrentDictionarySamples01 : IExecutable
  {
    public void Execute()
    {
    }
  }
  #endregion
  
  #region ConcurrentStackSamples-01
  public class ConcurrentStackSamples01 : IExecutable
  {
    public void Execute()
    {
    }
  }
  #endregion
  
  #region ConcurrentBagSamples-01
  public class ConcurrentBagSamples01 : IExecutable
  {
    public void Execute()
    {
    }
  }
  #endregion
  
  #region BlockingCollectionSamples-01
  public class BlockingCollectionSamples01 : IExecutable
  {
    public void Execute()
    {
    }
  }
  #endregion
  
  #region CancellationTokenSamples-01
  /// <summary>
  /// CancellationTokenとCancellationTokenSourceについてのサンプルです。
  /// </summary>
  public class CancellationTokenSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // CancellationTokenとCancellationTokenSourceは
      // .NET Framework 4.0から追加された型である。
      //
      // 非同期操作または長時間の同期処理などの際、汎用的なキャンセル処理を実装するために利用できる。
      // よくタスク (System.Threading.Tasks.Task)と一緒に利用されている
      // 例が多いが、別にタスクでなくても利用できる。（通常のThreadやManualResetEventSlimなど)
      //
      // CancellationTokenSourceとCancellationTokenは親子のような関係にあり
      //   ・CancellationTokenSourceはキャンセル操作を持つ。
      //   ・CancellationTokenは、キャンセルされた事を検知する。
      // となっている。
      //
      // CancellationTokenにて、キャンセルされたか否かを検知するには以下のプロパティまたはメソッドを利用する.
      //   ・IsCancellationRequested
      //   ・ThrowIfCancellationRequested
      // 上記の内、ThrowIfCancellationRequestedメソッドはキャンセルされていた場合に
      // OperationCanceledExceptionを発生させる。
      //
      // そのほかにも、CancellationTokenには以下のプロパティとメソッドが存在する。
      //   ・WaitHandle
      //   ・Register
      // WaitHandleプロパティは、該当トークンがキャンセルされた際に通知される待機ハンドルである。
      // この待機ハンドルを利用することで、トークンがキャンセルされた後に実行される処理などを記述出来る。
      // Registerメソッドは、トークンがキャンセルされた際に関連してキャンセル処理などを行いたいオブジェクトが存在する
      // 場合などに利用できる。CancellationTokenは操作のキャンセルを表すものであり、オブジェクトの状態をキャンセルしたい
      // 場合にこのメソッドを利用して登録しておく.
      //
      // また、CancellationTokenSourceには、以下のstaticメソッドが存在する。
      //   ・CreateLinkedTokenSource
      // CreateLinkedTokenSourceメソッドは、引数に複数のトークンを受け取り
      // それらのトークンを紐づけた状態のトークンソースを作成してくれる。
      // これを利用することにより、複数のトークン全てがキャンセルされた際にキャンセル扱いになる
      // CancellationTokenを生成する事が出来る。
      // 
      // 関連する全てのトークンがキャンセル状態となった際に行うキャンセル処理を記述する場合などに利用できる。
      //
      var cts = new CancellationTokenSource();
      
      ////////////////////////////////////////////////////////////////////
      //
      // Threadを利用してのキャンセル処理.
      //
      var t = new Thread(() => Work1(cts.Token));
      t.Start();
      
      Thread.Sleep(TimeSpan.FromSeconds(3));
      
      // キャンセル実行.
      cts.Cancel();
      
      ////////////////////////////////////////////////////////////////////
      //
      // ThreadPoolを利用してのキャンセル処理.
      //
      // CancellationTokenSourceは、一度キャンセルすると
      // 再利用できない構造となっている。（つまり、キャンセル後に取得したTokenを利用しても
      // 最初からキャンセルされた事になっている。）
      //
      cts = new CancellationTokenSource();
      ThreadPool.QueueUserWorkItem((obj) => Work2(cts.Token), null);
      
      Thread.Sleep(TimeSpan.FromSeconds(3));
      cts.Cancel();
      
      ////////////////////////////////////////////////////////////////////
      //
      // ManualResetEventSlimを利用してのキャンセル処理.
      //
      cts = new CancellationTokenSource();
      
      var waitHandle = new ManualResetEventSlim(false);
      Task.Factory.StartNew(() => Work3(cts.Token, waitHandle));
      
      Thread.Sleep(TimeSpan.FromSeconds(3));
      cts.Cancel();
      
      ////////////////////////////////////////////////////////////////////
      //
      // CancellationToken.WaitHandleを利用してのキャンセル待ち.
      //
      cts = new CancellationTokenSource();
      using (var countdown = new CountdownEvent(3))
      {
        var token = cts.Token;
        
        Parallel.Invoke
        (
          // 3秒後にキャンセル処理を実行.
          () => 
          {
            Thread.Sleep(TimeSpan.FromSeconds(3));
            cts.Cancel();
            countdown.Signal();
          },
          // トークンのWaitHandleを利用してキャンセル待ち.
          () => 
          {
            Console.WriteLine(">>> キャンセル待ち・・・");
            token.WaitHandle.WaitOne();
            Console.WriteLine(">>> 操作がキャンセルされたので、WaitHandleから通知されました。");
            countdown.Signal();
          },
          // キャンセルされるまで実行される処理.
          () =>
          {
            try
            {
              while (true)
              {
                token.ThrowIfCancellationRequested();
                Console.WriteLine(">>> wait...");
                Thread.Sleep(TimeSpan.FromMilliseconds(700));
              }
            }
            catch (OperationCanceledException ex)
            {
              Console.WriteLine(">>> {0}", ex.Message);
            }
            
            countdown.Signal();
          }
        );
        
        countdown.Wait();
      }

      ////////////////////////////////////////////////////////////////////
      //
      // CancellationToken.Registerを利用した関連オブジェクトのキャンセル操作.
      // CancellationToken.Registerメソッドには、キャンセルされた際に実行される
      // アクションを設定することが出来る。これを利用することで、トークンのキャンセル時に
      // 関連してキャンセル処理やキャンセル時にのみ実行する処理を記述することが出来る。
      //
      // 以下では、WebClientを利用して非同期処理を行っている最中にトークンをキャンセルし
      // さらに、WebClientもキャンセルするようにしている。（若干強引だが・・・・w）
      //
      cts = new CancellationTokenSource();
      
      var token2 = cts.Token;
      var client = new WebClient();
      
      client.DownloadStringCompleted += (s, e) => 
      {
        Console.WriteLine(">>> キャンセルされた？ == {0}", e.Cancelled);
      };
      
      token2.Register(() => 
        {
          Console.WriteLine(">>> 操作がキャンセルされたので、WebClient側もキャンセルします。");
          client.CancelAsync();
        }
      );
      
      Console.WriteLine(">>> WebClient.DownloadStringAsync...");
      client.DownloadStringAsync(new Uri(@"http://d.hatena.ne.jp/gsf_zero1/"));
      
      Thread.Sleep(TimeSpan.FromMilliseconds(200));
      cts.Cancel();
      
      ////////////////////////////////////////////////////////////////////
      //
      // CancellationTokenSourceには、複数のトークンを同期させるための
      // CreateLinkedTokenSourceメソッドが存在する。
      // このメソッドを利用することにより、複数のトークンのキャンセルを処理することが出来る。
      // 
      // 尚、CreateLinkedTokenSourceで作成したリンクトークンソースは
      // Disposeしないといけない事に注意。
      //
      var cts2 = new CancellationTokenSource();
      var cts3 = new CancellationTokenSource();
      
      var cts2Token = cts2.Token;
      var cts3Token = cts3.Token;
      
      using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts2Token, cts3Token))
      {
        var linkedCtsToken = linkedCts.Token;
        
        using (var countdown = new CountdownEvent(2))
        {
          Parallel.Invoke
          (
            // 1秒後にcts2をキャンセル
            () => 
            {
              Thread.Sleep(TimeSpan.FromSeconds(1));
              Console.WriteLine(">>> cts2.Canel()");
              cts2.Cancel();
              
              countdown.Signal();
            },
            // 2秒後にcts3をキャンセル.
            () =>
            {
              Thread.Sleep(TimeSpan.FromSeconds(2));
              Console.WriteLine(">>> cts3.Canel()");
              cts3.Cancel();
              
              countdown.Signal();
            }
          );
          
          countdown.Wait();
        }
        
        // 各トークンの状態をチェック.
        Console.WriteLine(">>>> cts2Token.IsCancellationRequested == {0}", cts2Token.IsCancellationRequested);
        Console.WriteLine(">>>> cts3Token.IsCancellationRequested == {0}", cts3Token.IsCancellationRequested);
        // リンクトークンなので、紐づくトークン全てがキャンセルになると自動的にキャンセル状態となる。
        Console.WriteLine(">>>> linkedCtsToken.IsCancellationRequested == {0}", linkedCtsToken.IsCancellationRequested);
      }
      
      Thread.Sleep(TimeSpan.FromSeconds(1));
    }
    
    void Work1(CancellationToken cancelToken)
    {
      //
      // キャンセル処理を実装する場合、try-catchを用意して
      // OperationCanceledExceptionを受け取るようにしておく.
      //
      try
      {
        while (true)
        {
          //
          // もし、外部でキャンセルされていた場合
          // このメソッドはOperationCanceledExceptionを発生させる。
          //
          cancelToken.ThrowIfCancellationRequested();
          
          Console.WriteLine(">> wait...");
          Thread.Sleep(TimeSpan.FromSeconds(1));
        }
      }
      catch (OperationCanceledException ex)
      {
        //
        // キャンセルされた.
        //
        Console.WriteLine(">>> {0}", ex.Message);
      }
    }
    
    void Work2(CancellationToken cancelToken)
    {
      //
      // IsCancellationRequestedプロパティを利用して
      // キャンセルを検知する.
      //
      while (true)
      {
        if (cancelToken.IsCancellationRequested)
        {
          // キャンセルされた.
          Console.WriteLine(">>> 操作はキャンセルされました。");
          break;
        }
        
        Console.WriteLine(">> wait...");
        Thread.Sleep(TimeSpan.FromSeconds(1));
      }
    }
    
    void Work3(CancellationToken cancelToken, ManualResetEventSlim waitHandle)
    {
      try
      {
        Console.WriteLine(">> waitHandle.Wait...");
        waitHandle.Wait(cancelToken);
        Console.WriteLine(">> awake!");
      }
      catch (OperationCanceledException ex)
      {
        // キャンセルされた.
        Console.WriteLine(">>> {0}", ex.Message);
      }
    }
  }
  #endregion
  
  #region LazySamples-01
  /// <summary>
  /// Lazy<T>, LazyInitializerクラスのサンプルです。
  /// </summary>
  public class LazySamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // Lazy<T>クラスは、遅延初期化 (Lazy Initialize)機能を付与するクラスである。
      //
      // 利用する際は、LazyクラスのコンストラクタにFunc<T>を指定することにより
      // 初期化処理を指定する。（たとえば、コストのかかるオブジェクトの構築などをFuncデリゲート内にて処理など）
      //
      // また、コンストラクタにはFunc<T>の他にも、第二引数としてスレッドセーフモードを指定出来る。
      // (System.Threading.LazyThreadSafetyMode)
      //
      // スレッドセーフモードは、Lazyクラスが遅延初期化処理を行う際にどのレベルのスレッドセーフ処理を適用するかを指定するもの。
      // スレッドセーフモードの指定は、LazyクラスのコンストラクタにてLazyThreadSafetyModeかboolで指定する。
      //   ・None:                    スレッドセーフ無し。速度が必要な場合、または、呼び元にてスレッドセーフが保証出来る場合に利用
      //   ・PublicationOnly:         複数のスレッドが同時に値の初期化を行う事を許可するが、最初に初期化に成功したスレッドが
      //                             Lazyインスタンスの値を設定するモード。（race-to initialize)
      //   ・ExecutionAndPublication: 完全スレッドセーフモード。一つのスレッドのみが初期化を行えるモード。
      //                             (double-checked locking)
      //
      // Lazyクラスのコンストラクタにて、スレッドセーフモードをbool型で指定する場合、以下のLazyThreadSafetyModeの値が指定された事と同じになる。
      //    ・true : LazyThreadSafetyMode.ExecutionAndPublicationと同じ。
      //    ・false: LazyThreadSafetyMode.Noneと同じ。
      //
      // Lazyクラスは、例外のキャッシュ機能を持っている。これは、Lazy.Valueを呼び出した際にコンストラクタで指定した
      // 初期化処理内で例外が発生した事を検知する際に利用する。Lazyクラスのコンストラクタにて、既定コンストラクタを使用するタイプの
      // 設定を行っている場合、例外のキャッシュは有効にならない。
      //
      // また、LazyThreadSafetyMode.PublicationOnlyを指定した場合も、例外のキャッシュは有効とならない。
      //
      // 排他モードで初期化処理を実行
      var lazy1 = new Lazy<HeavyObject>(() => new HeavyObject(TimeSpan.FromMilliseconds(100)), LazyThreadSafetyMode.ExecutionAndPublication);
      // 尚、上は以下のように第二引数をtrueで指定した場合と同じ事。
      // var lazy1 = new Lazy(() => new HeavyObject(TimeSpan.FromSeconds(1)), true);
      
      // 値が初期化済みであるかどうかは、IsValueCreatedで確認出来る。
      Console.WriteLine("値構築済み？ == {0}", lazy1.IsValueCreated);
      
      //
      // 複数のスレッドから同時に初期化を試みてみる。 (ExecutionAndPublication)
      //
      Parallel.Invoke
      (
        () => 
        {
          Console.WriteLine("[lambda1] 初期化処理実行 start.");
          
          if (lazy1.IsValueCreated)
          {
            Console.WriteLine("[lambda1] 既に値が作成されている。(IsValueCreated=true)");
          }
          else
          {
            Console.WriteLine("[lambda1] ThreadId={0}", Thread.CurrentThread.ManagedThreadId);
            var obj = lazy1.Value;
          }
          
          Console.WriteLine("[lambda1] 初期化処理実行 end.");
        },
        () => 
        {
          Console.WriteLine("[lambda2] 初期化処理実行 start.");
          
          if (lazy1.IsValueCreated)
          {
            Console.WriteLine("[lambda2] 既に値が作成されている。(IsValueCreated=true)");
          }
          else
          {
            Console.WriteLine("[lambda2] ThreadId={0}", Thread.CurrentThread.ManagedThreadId);
            var obj = lazy1.Value;
          }
          
          Console.WriteLine("[lambda2] 初期化処理実行 end.");
        }
      );
      
      Console.WriteLine("==========================================");
      
      //
      // 複数のスレッドにて同時に初期化処理の実行を許可するが、最初に初期化した値が設定されるモード。
      // (PublicationOnly)
      //
      var lazy2 = new Lazy<HeavyObject>(() => new HeavyObject(TimeSpan.FromMilliseconds(100)), LazyThreadSafetyMode.PublicationOnly);

      Parallel.Invoke
      (
        () => 
        {
          Console.WriteLine("[lambda1] 初期化処理実行 start.");
          
          if (lazy2.IsValueCreated)
          {
            Console.WriteLine("[lambda1] 既に値が作成されている。(IsValueCreated=true)");
          }
          else
          {
            Console.WriteLine("[lambda1] ThreadId={0}", Thread.CurrentThread.ManagedThreadId);
            var obj = lazy2.Value;
          }
          
          Console.WriteLine("[lambda1] 初期化処理実行 end.");
        },
        () => 
        {
          Console.WriteLine("[lambda2] 初期化処理実行 start.");
          
          if (lazy2.IsValueCreated)
          {
            Console.WriteLine("[lambda2] 既に値が作成されている。(IsValueCreated=true)");
          }
          else
          {
            Console.WriteLine("[lambda2] ThreadId={0}", Thread.CurrentThread.ManagedThreadId);
            var obj = lazy2.Value;
          }
          
          Console.WriteLine("[lambda2] 初期化処理実行 end.");
        }
      );
      
      Console.WriteLine("値構築済み？ == {0}", lazy1.IsValueCreated);
      Console.WriteLine("値構築済み？ == {0}", lazy2.IsValueCreated);
      
      Console.WriteLine("lazy1のスレッドID: {0}", lazy1.Value.CreatedThreadId);
      Console.WriteLine("lazy2のスレッドID: {0}", lazy2.Value.CreatedThreadId);
    }
    
    class HeavyObject
    {
      int _threadId;
      
      public HeavyObject(TimeSpan waitSpan)
      {
        Console.WriteLine(">>>>>> HeavyObjectのコンストラクタ start. [{0}]", Thread.CurrentThread.ManagedThreadId);
        Initialize(waitSpan);
        Console.WriteLine(">>>>>> HeavyObjectのコンストラクタ end.   [{0}]", Thread.CurrentThread.ManagedThreadId);
      }
      
      void Initialize(TimeSpan waitSpan)
      {
        Thread.Sleep(waitSpan);
        _threadId = Thread.CurrentThread.ManagedThreadId;
      }
      
      public int CreatedThreadId
      {
        get
        {
          return _threadId;
        }
      }
    }
  }
  #endregion
  
  #region LazyInitializerSamples-01
  public class LazyInitializerSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // LazyInitializerは、Lazyと同様に遅延初期化を行うための
      // クラスである。このクラスは、staticメソッドのみで構成され
      // Lazyでの記述を簡便化するために存在する。
      //
      // EnsureInitializedメソッドは
      // Lazyクラスにて、LazyThreadSafetyMode.PublicationOnlyを
      // 指定した場合と同じ動作となる。(race-to-initialize)
      //
      var hasHeavy = new HasHeavyData();
      
      Parallel.Invoke
      (
        () => 
        {
          Console.WriteLine("Created. [{0}]", hasHeavy.Heavy.CreatedThreadId);
        },
        () => 
        {
          Console.WriteLine("Created. [{0}]", hasHeavy.Heavy.CreatedThreadId);
        },
        // 少し待機してから、作成済みの値にアクセス.
        () =>
        {
          Thread.Sleep(TimeSpan.FromMilliseconds(2000));
          Console.WriteLine(">>少し待機してから、作成済みの値にアクセス.");
          Console.WriteLine(">>Created. [{0}]", hasHeavy.Heavy.CreatedThreadId);
        }
      );
    }
    
    class HasHeavyData
    {
      HeavyObject _heavy;
      
      public HeavyObject Heavy
      {
        get
        {
          //
          // LazyInitializerを利用して、遅延初期化.
          //
          Console.WriteLine("[ThreadId {0}] 値初期化処理開始. start", Thread.CurrentThread.ManagedThreadId);
          LazyInitializer.EnsureInitialized(ref _heavy, () => new HeavyObject(TimeSpan.FromMilliseconds(100)));
          Console.WriteLine("[ThreadId {0}] 値初期化処理開始. end", Thread.CurrentThread.ManagedThreadId);
          
          return _heavy;
        }
      }
    }
    
    class HeavyObject
    {
      int _threadId;
      
      public HeavyObject(TimeSpan waitSpan)
      {
        Console.WriteLine(">>>>>> HeavyObjectのコンストラクタ start. [{0}]", Thread.CurrentThread.ManagedThreadId);
        Initialize(waitSpan);
        Console.WriteLine(">>>>>> HeavyObjectのコンストラクタ end.   [{0}]", Thread.CurrentThread.ManagedThreadId);
      }
      
      void Initialize(TimeSpan waitSpan)
      {
        Thread.Sleep(waitSpan);
        _threadId = Thread.CurrentThread.ManagedThreadId;
      }
      
      public int CreatedThreadId
      {
        get
        {
          return _threadId;
        }
      }
    }
  }
  #endregion
  
  #region ThreadLocalSamples-01
  /// <summary>
  /// ThreadLocal<T>クラスのサンプルです。
  /// </summary>
  public class ThreadLocalSamples01 : IExecutable
  {
    #region Static Fields
    // ThreadStatic
    [ThreadStatic]
    static int count = 2;
    static ThreadLocal<int> count2 = new ThreadLocal<int>(() => 2);
    #endregion
    
    #region Fields
    [ThreadStatic]
    int count3 = 2;
    ThreadLocal<int> count4 = new ThreadLocal<int>(() => 4);
    #endregion
    
    public void Execute()
    {
      //
      // ThreadLocal<T>は、.NET 4.0から追加された型である。
      // ThreadStatic属性と同様に、スレッドローカルストレージ(TLS)を表現するための型である。
      //
      // 従来より存在していたThreadStatic属性には、以下の点が行えなかった。
      //   ・インスタンスフィールドには対応していない。（staticフィールドのみ)
      //    (インスタンスフィールドにも属性を付与することが出来るが、ちゃんと動作しない）
      //   ・フィールドの値は常に、その型のデフォルト値で初期化される。初期値を設定しても無視される。
      //
      // ThreadLocal<T>は、上記の点を解決している。つまり
      //   ・インスタンスフィールドに対応している。
      //   ・フィールドの値を初期値で初期化出来る。
      //
      // 利用方法は、System.Lazyと似ており、コンストラクタに初期化のためのデリゲートを渡す。
      //
      
      //
      // staticフィールドのThreadState属性の確認
      // ThreadStatic属性では、初期値を宣言時に設定していても無視され、強制的にデフォルト値が適用されるので
      // 出力される値は、全て0となる。
      //
      int numberOfParallels = 10;
      using (var countdown = new CountdownEvent(numberOfParallels))
      {
        for (var i = 0; i < numberOfParallels; i++)
        {
          int tmp = i;
          new Thread(() => { Console.WriteLine("ThreadStatic [count]>>> {0}", count++); countdown.Signal(); }).Start();
        }
        
        countdown.Wait();
      }
      
      //
      // staticフィールドのThreadLocal<T>の確認
      // ThreadLocal<T>は、初期値を設定できるので、出力される値は2となる。
      //
      using (var countdown = new CountdownEvent(numberOfParallels))
      {
        for (var i = 0; i < numberOfParallels; i++)
        {
          new Thread(() => { Console.WriteLine("ThreadLocal<T> [count2]>>> {0}", count2.Value++); countdown.Signal(); }).Start();
        }
        
        countdown.Wait();
      }
      
      //
      // インスタンスフィールドのThreadStatic属性の確認
      // ThreadStatic属性は、インスタンスフィールドに対しては効果が無い。
      // なので、出力される値は2,3,4,5,6...とインクリメントされていく.
      //
      using (var countdown = new CountdownEvent(numberOfParallels))
      {
        for (var i = 0; i < numberOfParallels; i++)
        {
          int tmp = i;
          new Thread(() => { Console.WriteLine("ThreadStatic [count3]>>> {0}", count3++); countdown.Signal(); }).Start();
        }
        
        countdown.Wait();
      }
      
      //
      // インスタンスフィールドのThreadLocal<T>の確認
      // ThreadLocal<T>は、インスタンスフィールドに対しても問題なく利用できる。
      // なので、出力される値は4となる。
      //
      using (var countdown = new CountdownEvent(numberOfParallels))
      {
        for (var i = 0; i < numberOfParallels; i++)
        {
          new Thread(() => { Console.WriteLine("ThreadLocal<T> [count4]>>> {0}", count4.Value++); countdown.Signal(); }).Start();
        }
        
        countdown.Wait();
      }
      
      count2.Dispose();
      count4.Dispose();
    }
  }
  #endregion
  
  #region PLinqSamples-01
  public class PLinqSamples01 : IExecutable
  {
    public void Execute()
    {
      byte[] numbers = GetRandomNumbers();
      
      Stopwatch watch = Stopwatch.StartNew();
      
      // 普通のLINQ
      // var query1 = from x in numbers
      // 並列LINQ（１）（ExecutionModeを付与していないので、並列で実行するか否かはTPLが決定する）
      // var query1 = from x in numbers.AsParallel()
      // 並列LINQ（２）（ExecutionModeを付与しているので、強制的に並列で実行するよう指示）
      var query1 = from x in numbers.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                   select Math.Pow(x, 2);
      
      foreach (var item in query1)
      {
        Console.WriteLine(item);
      }
      
      watch.Stop();
      Console.WriteLine(watch.Elapsed);
    }
    
    byte[] GetRandomNumbers()
    {
      byte[] result = new byte[10];
      Random rnd = new Random();
      
      rnd.NextBytes(result);
      
      return result;
    }
  }
  #endregion
  
  #region PLinqSamples-02
  public class PLinqSamples02 : IExecutable
  {
    public void Execute()
    {
      // 並列LINQにて、元の順序を保持して処理するにはAsOrderedを利用する。
      // AsOrderedを指定していない場合、どの順序で処理されていくのかは保証されない。
      var query1 = from x in Enumerable.Range(1, 20)
                   select Math.Pow(x, 2);
      
      foreach (var item in query1)
      {
        Console.WriteLine(item);
      }
      
      Console.WriteLine("===============");
      
      //
      // 以下のように、元のデータシーケンスをIEnumerable<T>と指定した上で並列LINQを行おうとしても
      // 並列化されない。何故なら、IEnumerable<T>では、LINQはシーケンス内にいくつ要素が存在するのかを
      // 判別することが出来ないためである。
      //
      // 並列LINQは、元のシーケンスをPartionerを利用して、一定のサイズのチャンクに分けて
      // 同時実行するための機能であるため、元の要素数が分からない場合はチャンクに分けることが出来ない。
      //
      // 並列処理を行う為には、ToListかToArrayなどを行い変換してから処理を進めるか
      // ParallelEnumerable.Rangeを利用したりするとうまくいく。
      //
      // 以下の例では並列処理が行われない。
      //var query2 = from x in Enumerable.Range(1, 20).AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
      // 以下の例ではLINQがリストの要素数を判別することが出来るので、並列処理が行われる。
      var query2 = from x in Enumerable.Range(1, 20).ToList().AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                   select Math.Pow(x, 2);
      
      foreach (var item in query2)
      {
        Console.WriteLine(item);
      }
      
      Console.WriteLine("===============");
      
      var query3 = from x in ParallelEnumerable.Range(1, 20).AsParallel().AsOrdered().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                   select Math.Pow(x, 2);
      
      foreach (var item in query3)
      {
        Console.WriteLine(item);
      }
      
      Console.WriteLine("===============");
    }
  }
  #endregion
  
  #region GetInvalidPathCharsAndGetInvalidFileNameCharsSamples-01
  /// <summary>
  /// PathクラスのGetInvalidPathCharsメソッドとGetInvalidFileNameCharsメソッドのサンプルです。
  /// </summary>
  public class GetInvalidPathCharsAndGetInvalidFileNameCharsSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // Pathクラスには、パス名及びファイル名に利用できない文字を取得するメソッドが存在する。
      //   パス名：GetInvalidPathChars
      // ファイル名：GetInvalidFileNameChars
      //
      // 引数などで渡されたパスやファイル名に対して不正な文字が利用されていないか
      // チェックする際などに利用できる。
      //
      // 戻り値は、どちらもcharの配列となっている。
      //
      char[] invalidPathChars   = Path.GetInvalidPathChars();
      char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
      
      string tmpPath   = @"c:usrlocaltmp_<path>_tmp";
      string tmpFileName = @"tmp_<filename>_tmp.|||";
      
      Console.WriteLine("不正なパス文字が存在してる？     = {0}", invalidPathChars.Any(ch => tmpPath.Contains(ch)));
      Console.WriteLine("不正なファイル名文字が存在してる？ = {0}", invalidFileNameChars.Any(ch => tmpFileName.Contains(ch)));
    }
  }
  #endregion
  
  #region NewLineDetectSample-01
  /// <summary>
  /// 文字列中の改行コードの数を算出するサンプルです。
  /// </summary>
  public class NewLineDetectSample01 : IExecutable
  {
    public void Execute()
    {
      string testStrings = string.Format("あt{0}いt{0}う{0}えt{0}お{0}", Environment.NewLine);
      
      Console.WriteLine("=== 元文字列 start ===");
      Console.WriteLine(testStrings);
      Console.WriteLine("=== 元文字列 end  ===");
      
      //
      // 改行コードを判定するための、比較元文字配列を構築.
      //
      char[] newLineChars = Environment.NewLine.ToCharArray();
      
      //
      // 改行コードのカウントを算出.
      //
      int  count  = 0;
      char prevChar = char.MaxValue;
      foreach (Char ch in testStrings)
      {
        //
        // プラットフォームによっては、改行コードが２文字の構成 (CRLF)となるため
        // 前後の文字のパターンが両方一致する場合に改行コードであるとみなす。
        //
        if (newLineChars.Contains(prevChar) && newLineChars.Contains(ch))
        {
          count++;
        }
        
        prevChar = ch;
      }
      
      Console.WriteLine("改行コードの数: {0}", count);
    }
  }
  #endregion
  
  #region WcfSample-01
  /// <summary>
  /// WCFのサンプルです。
  /// </summary>
  /// <remarks>
  /// 最も基本的なサービスとクライアントの応答を行うサンプルです。
  /// </remarks>
  public class WcfSamples01 : IExecutable
  {
    #region Constants
    /// <summary>
    /// サービスのURL
    /// </summary>
    const  string       SERVICE_URL   = "http://localhost:54321/HelloWorldService";
    /// <summary>
    /// エンドポイント名
    /// </summary>
    const  string       ENDPOINT_ADDR = "";
    /// <summary>
    /// バインディング
    /// </summary>
    readonly BasicHttpBinding BINDING     = new BasicHttpBinding();
    #endregion
    
    /// <summary>
    /// サービスインターフェース
    /// </summary>
    [ServiceContract]
    public interface IHelloWorldService
    {
      /// <summary>
      /// サービスメソッド
      /// </summary>
      [OperationContract]
      string SayHello();
    }
    
    /// <summary>
    /// サービスの実装
    /// </summary>
    public class HelloWorldService : IHelloWorldService
    {
      public string SayHello()
      {
        return "Hello World";
      }
    }
    
    public void Execute()
    {
      using (ServiceHost host = CreateService())
      {
        //
        // サービスを開始.
        //
        host.Open();
        
        //
        // クライアント側を構築.
        //
        using (ChannelFactory<IHelloWorldService> factory = CreateChannelFactory())
        {
          //
          // クライアントプロキシオブジェクトを取得.
          //
          IHelloWorldService proxy = factory.CreateChannel();
  
          //
          // サービスメソッドを呼び出し、結果を取得.
          //
          Console.WriteLine("サービスの呼び出し結果= {0}", proxy.SayHello());
        }
      }
    }
    
    private ServiceHost CreateService()
    {
      //
      // ホストを初期化
      //
      ServiceHost host = new ServiceHost(typeof(HelloWorldService), new Uri(SERVICE_URL));
      
      //
      // エンドポイントを追加.
      //
      host.AddServiceEndpoint(typeof(IHelloWorldService), BINDING, ENDPOINT_ADDR);
      
      return host;
    }
    
    private ChannelFactory<IHelloWorldService> CreateChannelFactory()
    {
      //
      // クライアント側からサービスに接続するためにChannelFactoryを構築.
      //
      ChannelFactory<IHelloWorldService> factory = 
        new ChannelFactory<IHelloWorldService>(BINDING, new EndpointAddress(SERVICE_URL));
      
      return factory;
    }
  }
  #endregion
  
  #region WcfSamples-02
  /// <summary>
  /// WCFのサンプルです。
  /// </summary>
  /// <remarks>
  /// 引数にカスタムオブジェクトを指定するサービスメソッドを定義しています。
  /// </remarks>
  public class WcfSamples02 : IExecutable
  {
    #region Constants
    /// <summary>
    /// サービスのURL
    /// </summary>
    const  string       SERVICE_URL   = "http://localhost:54321/NumberSumService";
    /// <summary>
    /// エンドポイント名
    /// </summary>
    const  string       ENDPOINT_ADDR = "";
    /// <summary>
    /// バインディング
    /// </summary>
    readonly BasicHttpBinding BINDING     = new BasicHttpBinding();
    #endregion
    
    /// <summary>
    /// サービスインターフェース
    /// </summary>
    [ServiceContract]
    public interface INumberSumService
    {
      /// <summary>
      /// サービスメソッド
      /// </summary>
      [OperationContract]
      int Sum(Data data);
    }
    
    /// <summary>
    /// サービスの実装クラス
    /// </summary>
    public class NumberSumService : INumberSumService
    {
      public int Sum(Data data)
      {
        return (data.X + data.Y);
      }
    }
    
    /// <summary>
    /// データコントラクトクラス
    /// </summary>
    [DataContract]
    public class Data
    {
      [DataMember]
      public int X
      {
        get;
        set;
      }
      
      [DataMember]
      public int Y
      {
        get;
        set;
      }
    }
    
    public void Execute()
    {
      using (ServiceHost host = CreateService())
      {
        //
        // サービスを開始.
        //
        host.Open();
        
        //
        // クライアント側を構築.
        //
        using (ChannelFactory<INumberSumService> factory = CreateChannelFactory())
        {
          //
          // クライアントプロキシオブジェクトを取得.
          //
          INumberSumService proxy = factory.CreateChannel();
  
          //
          // サービスメソッドを呼び出し、結果を取得.
          //
          Console.WriteLine("サービスの呼び出し結果= {0}", proxy.Sum(new Data { X = 300, Y = 200 }));
        }
      }
    }
    
    private ServiceHost CreateService()
    {
      //
      // ホストを初期化
      //
      ServiceHost host = new ServiceHost(typeof(NumberSumService), new Uri(SERVICE_URL));
      
      //
      // エンドポイントを追加.
      //
      host.AddServiceEndpoint(typeof(INumberSumService), BINDING, ENDPOINT_ADDR);
      
      return host;
    }
    
    private ChannelFactory<INumberSumService> CreateChannelFactory()
    {
      //
      // クライアント側からサービスに接続するためにChannelFactoryを構築.
      //
      ChannelFactory<INumberSumService> factory = 
        new ChannelFactory<INumberSumService>(BINDING, new EndpointAddress(SERVICE_URL));
      
      return factory;
    }
  }
  #endregion
  
  #region WcfSamples-03
  /// <summary>
  /// WCFのサンプルです。
  /// </summary>
  /// <remarks>
  /// 引数と戻り値にカスタムオブジェクトを指定するサービスメソッドを定義しています。
  /// </remarks>
  public class WcfSamples03 : IExecutable
  {
    #region Constants
    /// <summary>
    /// サービスのURL
    /// </summary>
    const  string       SERVICE_URL   = "http://localhost:54321/ReturnCustomDataService";
    /// <summary>
    /// エンドポイント名
    /// </summary>
    const  string       ENDPOINT_ADDR = "";
    /// <summary>
    /// バインディング
    /// </summary>
    readonly BasicHttpBinding BINDING     = new BasicHttpBinding();
    #endregion
  
    /// <summary>
    /// サービスインターフェース
    /// </summary>
    [ServiceContract]
    public interface IReturnCustomDataService
    {
      [OperationContract]
      ReturnData Execute(Data data);
    }
    
    /// <summary>
    /// サービス実装クラス
    /// </summary>
    public class ReturnCustomDataService : IReturnCustomDataService
    {
      public ReturnData Execute(Data data)
      {
        return new ReturnData { X = data.Y, Y = data.X };
      }
    }
    
    /// <summary>
    /// サービスメソッドの引数クラス
    /// </summary>
    [DataContract]
    public class Data
    {
      [DataMember]
      public int X
      {
        get;
        set;
      }
      
      [DataMember]
      public int Y
      {
        get;
        set;
      }
      
      public override string ToString()
      {
        return string.Format("X={0}, Y={1}", X, Y);
      }
    }
    
    /// <summary>
    /// サービスメソッドの戻り値クラス
    /// </summary>
    [DataContract]
    public class ReturnData
    {
      [DataMember]
      public int X
      {
        get;
        set;
      }
      
      [DataMember]
      public int Y
      {
        get;
        set;
      }
      
      public override string ToString()
      {
        return string.Format("X={0}, Y={1}", X, Y);
      }
    }
    
    public void Execute()
    {
      using (ServiceHost host = CreateService())
      {
        //
        // サービスを開始.
        //
        host.Open();
        
        //
        // クライアント側を構築.
        //
        using (ChannelFactory<IReturnCustomDataService> factory = CreateChannelFactory())
        {
          //
          // クライアントプロキシオブジェクトを取得.
          //
          IReturnCustomDataService proxy = factory.CreateChannel();
  
          //
          // サービスメソッドを呼び出し、結果を取得.
          //
          Data data = new Data { X = 300, Y = 200 };
          Console.WriteLine("サービスの呼び出し前= {0}",   data);
          Console.WriteLine("サービスの呼び出し結果= {0}", proxy.Execute(data));
        }
      }
    }
    
    private ServiceHost CreateService()
    {
      //
      // ホストを初期化
      //
      ServiceHost host = new ServiceHost(typeof(ReturnCustomDataService), new Uri(SERVICE_URL));
      
      //
      // エンドポイントを追加.
      //
      host.AddServiceEndpoint(typeof(IReturnCustomDataService), BINDING, ENDPOINT_ADDR);
      
      return host;
    }
    
    private ChannelFactory<IReturnCustomDataService> CreateChannelFactory()
    {
      //
      // クライアント側からサービスに接続するためにChannelFactoryを構築.
      //
      ChannelFactory<IReturnCustomDataService> factory = 
        new ChannelFactory<IReturnCustomDataService>(BINDING, new EndpointAddress(SERVICE_URL));
      
      return factory;
    }
  }
  #endregion
  
  #region 全角チェックと半角チェック
  /// <summary>
  /// 全角チェックと半角チェックのサンプルです。
  /// </summary>
  /// <remarks>
  /// 単純な全角チェックと半角チェックを定義しています。
  /// </remarks>
  public class ZenkakuHankakuCheckSample01 : IExecutable
  {
    public void Execute()
    {
      string zenkakuOnlyStrings     = "あいうえお";
      string hankakuOnlyStrings     = "ｱｲｳｴｵ";
      string zenkakuAndHankakuStrings = "あいうえおｱｲｳｴｵ";
      
      Console.WriteLine("IsZenkaku:zenkakuOnly:{0}",        IsZenkaku(zenkakuOnlyStrings));
      Console.WriteLine("IsZenkaku:hankakuOnlyStrings:{0}",     IsZenkaku(hankakuOnlyStrings));
      Console.WriteLine("IsZenkaku:zenkakuAndHankakuStrings:{0}", IsZenkaku(zenkakuAndHankakuStrings));
      Console.WriteLine("IsHankaku:zenkakuOnly:{0}",        IsHankaku(zenkakuOnlyStrings));
      Console.WriteLine("IsHankaku:hankakuOnlyStrings:{0}",     IsHankaku(hankakuOnlyStrings));
      Console.WriteLine("IsHankaku:zenkakuAndHankakuStrings:{0}", IsHankaku(zenkakuAndHankakuStrings));
    }
    
    bool IsZenkaku(string value)
    {
      //
      // 指定された文字列が全て全角文字で構成されているか否かは
      // 文字列を一旦SJISに変換し取得したバイト数と元文字列の文字数＊２が
      // 成り立つか否かで決定できる。
      //
      return (Encoding.GetEncoding("sjis").GetByteCount(value) == (value.Length * 2));
    }
    
    bool IsHankaku(string value)
    {
      //
      // 指定された文字列が全て半角文字で構成されているか否かは
      // 文字列を一旦SJISに変換し取得したバイト数と元文字列の文字数が
      // 成り立つか否かで決定できる。
      //
      return (Encoding.GetEncoding("sjis").GetByteCount(value) == value.Length);
    }
  }
  #endregion
  
  #region DateParseSamples-01
  public class DateParseSample01 : IExecutable
  {
    public void Execute()
    {
      //
      // ParseExactメソッドの場合は、値が2011, フォーマットがyyyy
      // の場合でも日付変換出来る。
      //
      try
      {
        var d = DateTime.ParseExact("2011", "yyyy", null);
        Console.WriteLine(d);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      
      //
      // TryParseメソッドの場合は、以下のどちらもFalseとなる。
      // 恐らく、IFormatProviderを設定しないと動かないと思われる。
      //
      DateTime d2;
      Console.WriteLine(DateTime.TryParse("2011", out d2));
      Console.WriteLine(DateTime.TryParse("2011", null, DateTimeStyles.None, out d2));
      
      //
      // TryParseExactメソッドの場合は、値が2011、フォーマットがyyyy
      // の場合でも日付変換出来る。
      //
      DateTime d3;
      Console.WriteLine(DateTime.TryParseExact("2011", "yyyy", null, DateTimeStyles.None, out d3));
      
      Console.WriteLine(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
      
      var d98 = DateTime.Now;
      var d99 = DateTime.ParseExact(d98.ToString("yyyyMMddHHmmssfff"), "yyyyMMddHHmmssfff", null);
      Console.WriteLine(d98 == d99);
      Console.WriteLine(d98.Ticks);
      Console.WriteLine(d98 == new DateTime(d98.Ticks));
      
      // 時分秒を指定していない場合は、00:00:00となる
      var d100 = new DateTime(2011, 11, 12);
      Console.WriteLine("{0}, {1}, {2}", d100.Hour, d100.Minute, d100.Second);
    }
  }
  #endregion
  
  #region RssSamples-01
  public class RssSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // RSSを作成には以下の手順で構築する.
      //
      // (1) SyndicationItemを作成し、リストに追加
      // (2) SyndicationFeedを作成し、(1)で作成したリストを追加
      // (3) XmlWriterを構築し、出力.
      //
      List<SyndicationItem> items = new List<SyndicationItem>();
      
      for (int i = 0; i < 10; i++)
      {
        var newItem = new SyndicationItem();
        
        newItem.Title = new TextSyndicationContent(string.Format("Test Title-{0}", i));
        newItem.Links.Add(new SyndicationLink(new Uri(@"http://www.google.co.jp/")));
        
        items.Add(newItem);
      }
      
      SyndicationFeed feed = new SyndicationFeed("Test Feed", "This is a test feed", new Uri(@"http://www.yahoo.co.jp/"), items);
      feed.LastUpdatedTime = DateTime.Now;
      
      StringBuilder sb = new StringBuilder();
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = true;
      
      using (XmlWriter writer = XmlWriter.Create(sb, settings))
      {
        //feed.SaveAsAtom10(writer);
        feed.SaveAsRss20(writer);
        writer.Close();
      }
      
      Console.WriteLine(sb.ToString());
    }
  }
  #endregion
  
  #region ImageConverterSamples-01
  /// <summary>
  /// ImageConverterクラスのサンプルです。
  /// </summary>
  public class ImageConverterSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // Imageオブジェクトを取得.
      //
      GDIImage image = GDIImage.FromFile("database.png");
      
      //
      // Imageをバイト配列に変換.
      //   Imageから別のオブジェクトに変換する場合はConvertToを利用する.
      //
      ImageConverter converter = new ImageConverter();
      byte[] imageBytes = (byte[]) converter.ConvertTo(image, typeof(byte[]));
      
      //
      // バイト配列をImageに変換.
      //   バイト配列からImageオブジェクトに変換する場合はConvertFromを利用する.
      //
      GDIImage image2 = (GDIImage) converter.ConvertFrom(imageBytes);
      
      // 確認.
      Debug.Assert(image != null);
      Debug.Assert(imageBytes != null && imageBytes.Length > 0);
      Debug.Assert(image2 != null);
      
      //
      // [補足]
      // Imageオブジェクトをファイルとして保存する場合は以下のようにする.
      //
      //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      //string fileName    = @"Sample.png";
      //string filePath    = Path.Combine(desktopPath, fileName);
      //
      //using (Stream stream = File.Create(filePath))
      //{
      //  image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
      //}
    }
  }
  #endregion
  
  #region RuntimeHelpersSamples-01
  /// <summary>
  /// RuntimeHelpersクラスのサンプルです。
  /// </summary>
  public class RuntimeHelpersSamples01 : IExecutable
  {
    class SampleClass
    {
      public int Id { get; set; }
      
      public override int GetHashCode()
      {
        return Id.GetHashCode();
      }
    }
    
    public void Execute()
    {
      //
      // RuntimeHelpersクラスのGetHashCodeは、他のクラスのGetHashCodeメソッド
      // と挙動が少し違う。以下、MSDN(http://msdn.microsoft.com/ja-jp/library/11tbk3h9.aspx)に
      // ある記述を引用。
      //
      // ・Object.GetHashCode は、オブジェクト値を考慮するシナリオで便利です。 同じ内容の 2 つの文字列は、Object.GetHashCode で同じ値を返します。
      // ・RuntimeHelpers.GetHashCode は、オブジェクト識別子を考慮するシナリオで便利です。 同じ内容の 2 つの文字列は、内容が同じでも異なる文字列オブジェクトであるため、RuntimeHelpers.GetHashCode で異なる値を返します。
      //
      // 以下では、サンプルとなるオブジェクトを2つ作成し、ハッシュコードを出力するようにしている。
      // サンプルクラスでは、GetHashCodeメソッドをオーバーライドしており、Idプロパティのハッシュコードを
      // 返すようにしている。
      //   (注意) このクラスのGetHashCodeメソッドの実装は、サンプルのために簡略化してあります。
      //         実際の実装で、このようなハッシュコードの算出はしてはいけません。
      //
      // 以下の場合、Object.GetHashCodeを呼び出している場合は当然ながら同じハッシュコードとなるが
      // RuntimeHelpers.GetHashCodeを呼び出している場合、違うハッシュコードとなる.
      //
      SampleClass sampleObj1 = new SampleClass{ Id = 100 };
      SampleClass sampleObj2 = new SampleClass{ Id = 100 };
      
      Console.WriteLine("[Object.GetHashCode]        sampleObj1 = {0}, sampleObj2 = {1}", sampleObj1.GetHashCode(), sampleObj2.GetHashCode());
      Console.WriteLine("[RuntimeHelper.GetHashCode] sampleObj1 = {0}, sampleObj2 = {1}", RuntimeHelpers.GetHashCode(sampleObj1), RuntimeHelpers.GetHashCode(sampleObj2));
  
      //
      // 文字列データで検証.
      // 以下は、文字列のハッシュコードが異なるか否かを検証.
      // 変数s1, s2を作成してから、連結して文字列値を作成している理由は
      // CLRによって、内部で文字列がインターン(Intern)されないようにするため.
      //
      // 文字列がInternされていない場合、RuntimeHelpers.GetHashCodeメソッドは
      // 違う値を返す。Object.GetHashCodeは同じハッシュコードを返す.
      //
      string s1    = "hello ";
      string s2    = "world";
      string test1 = s1 + s2;
      string test2 = s1 + s2;
      
      Console.WriteLine("[Object.GetHashCode]        test1 = {0}, test2 = {1}", test1.GetHashCode(), test2.GetHashCode());
      Console.WriteLine("[RuntimeHelper.GetHashCode] test1 = {0}, test2 = {1}", RuntimeHelpers.GetHashCode(test1), RuntimeHelpers.GetHashCode(test2));
      
      //
      // 文字列データで検証
      // 以下は、CLRによって文字列がインターンされる値に対してハッシュコードを取得している.
      //
      // この場合、RuntimeHelpers.GetHashCodeでも同じハッシュコードが返ってくる.
      // 尚、CLRによって値がインターンされるのはリテラルだけである.
      // 連結操作によって作成された文字列はインターンされない.
      // 無理矢理インターンするには、String.Internメソッドを利用する.
      //
      string test3 = "hello world";
      string test4 = "hello world";
      
      Console.WriteLine("[Object.GetHashCode]        test3 = {0}, test4 = {1}", test3.GetHashCode(), test4.GetHashCode());
      Console.WriteLine("[RuntimeHelper.GetHashCode] test3 = {0}, test4 = {1}", RuntimeHelpers.GetHashCode(test3), RuntimeHelpers.GetHashCode(test4));
    }
  }
  #endregion
  
  #region RuntimeHelpersSamples-02
  /// <summary>
  /// RuntimeHelpersクラスのサンプルです。
  /// </summary>
  public class RuntimeHelpersSamples02 : IExecutable
  {
    // サンプルクラス
    static class SampleClass
    {
      static SampleClass()
      {
        Console.WriteLine("SampleClass static ctor()");
      }
  
      //
      // このメソッドに対して、CER内で利用できるよう信頼性のコントラクトを付与.
      // ReliabilityContractAttributeおよびConsistencyやCerは
      // System.Runtime.ConstrainedExecution名前空間に存在する.
      //
      // RuntimeHelpers.PrepareConstrainedRegionsメソッドにて
      // 実行できるのは、Consistency.WillNotCorruptStateおよびMayCorruptInstanceの場合のみ.
      //
      // 尚、この属性はメソッドだけではなく、クラスやインターフェースにも付与できる。
      // その場合、クラス全体に対して信頼性のコントラクトを付与したことになる。
      //
      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      internal static void Print()
      {
        Console.WriteLine("SampleClass.Print()");
      }
    }
    
    public void Execute()
    {
      //
      // RuntimeHelpers.PrepareConstrainedRegionsを呼び出すと、コンパイラは
      // そのメソッド内のcatch, finallyブロックをCER（制約された実行領域）としてマークする。
      //
      // CERとしてマークされた領域から、コードを呼び出す場合、そのコードには信頼性のコントラクトが必要となる。
      // コードに対して、信頼性のコントラクトを付与するには、以下の属性を利用する。
      //  [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      //
      // CERでマークされた領域にて、コードに信頼性のコントラクトが付与されている場合、CLRは
      // try内の本処理が実行される前に、catch, finallyブロックのコードを事前コンパイルする。
      //
      // なので、例えばfinallyブロック内にて静的コンストラクタを持つクラスのメソッドを呼びだしていたり
      // すると、try内の本処理よりも先にfinallyブロック内の静的コンストラクタが呼ばれる事になる。
      // (事前コンパイルが行われると、アセンブリのロード、静的コンストラクタの実行などが発生するため)
      //
      RuntimeHelpers.PrepareConstrainedRegions();
      
      try
      {
        // 事前にRuntimeHelpers.PrepareConstrainedRegions()を呼び出している場合
        // 以下のメソッドが呼び出される前に、catch, finallyブロックが事前コンパイルされる.
        Calc();
      }
      finally
      {
        SampleClass.Print();
      }
    }
    
    void Calc()
    {
      for (int i = 0; i < 10; i++)
      {
        Console.Write("{0} ", (i + 1));
      }
      
      Console.WriteLine("");
    }
  }
  #endregion
  
  #region RuntimeHelpersSamples-03
  /// <summary>
  /// RuntimeHelpersクラスのサンプルです。
  /// </summary>
  public class RuntimeHelpersSamples03 : IExecutable
  {
    // サンプルクラス
    static class SampleClass
    {
      static SampleClass()
      {
        Console.WriteLine("SampleClass static ctor()");
      }
  
      //
      // このメソッドに対して、CER内で利用できるよう信頼性のコントラクトを付与.
      // ReliabilityContractAttributeおよびConsistencyやCerは
      // System.Runtime.ConstrainedExecution名前空間に存在する.
      //
      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      internal static void Print()
      {
        Console.WriteLine("SampleClass.Print()");
      }
    }
    
    public void Execute()
    {
      //
      // ExecuteCodeWithGuaranteedCleanupメソッドは, PrepareConstrainedRegionsメソッドと
      // 同様に、コードをCER（制約された実行環境）で実行するメソッドである。
      //
      // PrepareConstrainedRegionsメソッドが呼び出されたメソッドのcatch, finallyブロックを
      // CERとしてマークするのに対して、ExecuteCodeWithGuaranteedCleanupメソッドは
      // 明示的に実行コード部分とクリーンアップ部分 (バックアウトコード)を引数で渡す仕様となっている。
      //
      // ExecuteCodeWithGuaranteedCleanupメソッドは
      // TryCodeデリゲートとCleanupCodeデリゲート、及び、userDataを受け取る.
      //
      // public delegate void TryCode(object userData)
      // public delegate void CleanupCode(object userData, bool exceptionThrown)
      //
      // 前回のサンプルと同じ動作を行う.
      RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(Calc, Cleanup, null);
    }
    
    void Calc(object userData)
    {
      for (int i = 0; i < 10; i++)
      {
        Console.Write("{0} ", (i + 1));
      }
      
      Console.WriteLine("");
    }
    
    void Cleanup(object userData, bool exceptionThrown)
    {
      SampleClass.Print();
    }
  }
  #endregion
  
  #region CompareOptionsSamples-01
  /// <summary>
  /// 比較メソッドの結果値を変換するためのヘルパークラス.
  /// </summary>
  public static class CompareResultHelper
  {
    static readonly string[] CompResults = { "小さい", "等しい", "大きい" };
    
    // 比較結果の数値を文字列に変換.
    public static string ToStringResult(this int self)
    {
      return CompResults[self + 1];
    }
  }
  
  /// <summary>
  /// CompareOptions列挙型のサンプルです。
  /// </summary>
  public class CompareOptionsSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // string.Compareメソッドには、CultureInfoとCompareOptionsを
      // 引数にとるオーバーロードが定義されている。(他にもオーバーロードメソッドが存在します。)
      //
      // このオーバーロードを利用する際、CompareOptions.IgnoreKanaTypeを指定すると
      // 「ひらがな」と「カタカナ」の違いを無視して、文字列比較を行う事が出来る。
      //
      string ja1 = "はろーわーるど";
      string ja2 = "ハローワールド";
      
      CultureInfo ci = new CultureInfo("ja-JP");
      
      // 標準の比較方法で比較
      Console.WriteLine("{0}", string.Compare(ja1, ja2, ci, CompareOptions.None).ToStringResult());
      // 大文字小文字を無視して比較.
      Console.WriteLine("{0}", string.Compare(ja1, ja2, ci, CompareOptions.IgnoreCase).ToStringResult());
      // ひらがなとカタカナの違いを無視して比較
      // つまり、「はろーわーるど」と「ハローワールド」を同じ文字列として比較
      Console.WriteLine("{0}", string.Compare(ja1, ja2, ci, CompareOptions.IgnoreKanaType).ToStringResult());
      
      //
      // string.Compareメソッドは、内部でCutureInfoから、そのカルチャーに紐づく
      // CompareInfoを取り出して、比較処理を行っているので、自前で直接CompareInfoを
      // 用意して、Compareメソッドを呼び出しても同じ結果となる。
      //
      CompareInfo compInfo = CompareInfo.GetCompareInfo("ja-JP");
      Console.WriteLine("{0}", compInfo.Compare(ja1, ja2, CompareOptions.IgnoreKanaType).ToStringResult());
    }
  }
  #endregion
  
  #region CompareOptionsSamples-02
  /// <summary>
  /// CompareOptions列挙型のサンプルです。
  /// </summary>
  public class CompareOptionsSamples02 : IExecutable
  {
    public void Execute()
    {
      //
      // string.Compareメソッドには、CultureInfoとCompareOptionsを
      // 引数にとるオーバーロードが定義されている。(他にもオーバーロードメソッドが存在します。)
      //
      // このオーバーロードを利用する際、CompareOptions.IgnoreKanaTypeを指定すると
      // 「ひらがな」と「カタカナ」の違いを無視して、文字列比較を行う事が出来る。
      //
      // さらに、CompareOptionsには、IgnoreWidthという値も存在し
      // これを指定すると、全角と半角の違いを無視して、文字列比較を行う事が出来る。
      //
      string ja1 = "ハローワールド";
      string ja2 = "ﾊﾛｰﾜｰﾙﾄﾞ";
      string ja3 = "はろーわーるど";
      
      CultureInfo ci = new CultureInfo("ja-JP");
      
      // 全角半角の違いを無視して、「ハローワールド」と「ﾊﾛｰﾜｰﾙﾄﾞ」を比較
      Console.WriteLine("{0}", string.Compare(ja1, ja2, ci, CompareOptions.IgnoreWidth).ToStringResult());
      // 全角半角の違いを無視して、「はろーわーるど」と「ﾊﾛｰﾜｰﾙﾄﾞ」を比較
      Console.WriteLine("{0}", string.Compare(ja3, ja2, ci, CompareOptions.IgnoreWidth).ToStringResult());
      // 全角半角の違いを無視し、且つ、ひらがなとカタカナの違いを無視して、「はろーわーるど」と「ﾊﾛｰﾜｰﾙﾄﾞ」を比較
      Console.WriteLine("{0}", string.Compare(ja3, ja2, ci, (CompareOptions.IgnoreWidth | CompareOptions.IgnoreKanaType)).ToStringResult());
    }
  }
  #endregion
  
  #region AppDomainSamples-01
  /// <summary>
  /// AppDomainクラスのサンプルです。
  /// </summary>
  public class AppDomainSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // AppDomainには、.NET 4.0より以下のイベントが追加されている。
      //   ・FirstChanceExceptionイベント
      // このイベントは、例外が発生した際に文字通り最初に通知されるイベントである。
      // このイベントに通知されるタイミングは、catch節にて例外が補足されるよりも先となる。
      // 
      // 注意点として
      //   ・このイベントは、通知のみとなる。このイベントをハンドルしたからといって例外の発生が
      //    ここで止まるわけではない。例外は通常通りプログラムコード上のcatchに入ってくる。
      //   ・このイベントは、アプリケーションドメイン毎に定義できる。
      //   ・FirstChanceExceptionイベント内での例外は、絶対にハンドラ内でキャッチしないといけない。
      //    そうしないと、再帰的にFirstChanceExceptionが発生する。
      //   ・イベント引数であるFirstChanceExceptionEventArgsクラスは
      //    System.Runtime.ExceptionServices名前空間に存在する。
      //
      
      // 基底のAppDomainにて、FirstChanceExceptionイベントをハンドル.
      AppDomain.CurrentDomain.FirstChanceException += FirstChanceExHandler;
  
      try
      {
        // わざと例外発生.
        throw new InvalidOperationException("test Ex messsage");
      } 
      catch (InvalidOperationException ex)
      {
        // 本来のcatch処理.
        Console.WriteLine("Catch clause: {0}", ex.Message);
      }
      
      // イベントをアンバインド.
      AppDomain.CurrentDomain.FirstChanceException -= FirstChanceExHandler;
    }
    
    // イベントハンドラ.
    void FirstChanceExHandler(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
    {
      Console.WriteLine("FirstChanceException: {0}", e.Exception.Message);
    }
  }
  #endregion
  
  #region AppDomainSamples-02
  /// <summary>
  /// AppDomainクラスのサンプルです。
  /// </summary>
  public class AppDomainSamples02 : MarshalByRefObject, IExecutable
  {
    public void Execute()
    {
      AppDomain defaultDomain = AppDomain.CurrentDomain;
      AppDomain anotherDomain = AppDomain.CreateDomain("AnotherAppDomain");
      
      //
      // DomainUnloadイベントのハンドル.
      //
      // 既定のアプリケーションドメインでは、Unloadは登録できるが発行されることは
      // 無いので、設定する意味がない.
      //defaultDomain.DomainUnload += AppDomain_Unload;
      anotherDomain.DomainUnload += AppDomain_Unload;
      
      //
      // ProcessExitイベントのハンドル.
      //
      defaultDomain.ProcessExit += AppDomain_ProcessExit;
      anotherDomain.ProcessExit += AppDomain_ProcessExit;
      
      //
      // 既定のアプリケーションドメインをアンロードしようとするとエラーとなる.
      // ** appdomain をアンロード中にエラーが発生しました。 (HRESULT からの例外: 0x80131015) **
      //AppDomain.Unload(defaultDomain);
      
      //
      // AppDomain.Unloadを呼び出すと、DomainUnloadイベントが発生する.
      // AppDomain.Unloadを呼び出さずにプロセスが終了させようとすると
      // ProcessExitイベントが発生する。両方のイベントが同時に発生することは無い.
      //
      // 以下をコメントアウトすると、ProcessExitイベントが発生する.
      //
      //AppDomain.Unload(anotherDomain);
    }
    
    void AppDomain_Unload(object sender, EventArgs e)
    {
      AppDomain domain = sender as AppDomain;
      Console.WriteLine("AppDomain.Unload: {0}", domain.FriendlyName);
    }
    
    void AppDomain_ProcessExit(object sender, EventArgs e)
    {
      //
      // ProcessExitイベントには、タイムアウトが存在する。（既定は2秒）
      // 以下、MSDNの記述.
      // (http://msdn.microsoft.com/ja-jp/library/system.appdomain.processexit.aspx)
      //
      // 「プロセス シャットダウン時における全ファイナライザーの合計実行時間が限られているように、ProcessExit の
      // すべてのイベント ハンドラーに対して割り当てられる合計実行時間も限られています。 既定値は 2 秒です。」
      //
      // 以下のコメントを外して実行すると、タイムアウト時間を過ぎるので
      // イベントをハンドルしていても、後続の処理は実行されない。
      //
      // わざとタイムアウト時間が過ぎるように待機.
      //Console.WriteLine("AppDomain.ProcessExit Thread.Sleep()");
      //Thread.Sleep(TimeSpan.FromSeconds(3));
      
      AppDomain domain = sender as AppDomain;
      Console.WriteLine("AppDomain.ProcessExit: {0}", domain.FriendlyName);
    }
  }
  #endregion
  
  #region AppDomainSamples-03
  /// <summary>
  /// AppDomainクラスのサンプルです。
  /// </summary>
  public class AppDomainSamples03 : IExecutable
  {
    // AppDomainのモニタリングを担当するクラス
    class AppDomainMonitor : IDisposable
    {
      static AppDomainMonitor()
      {
        //
        // AppDomain.MonitoringIsEnabledは、特殊なプロパティで
        // 以下の特徴を持つ。
        //
        // ・一度True（監視ON）にしたら、false（監視OFF）に戻すことはできない。
        // ・値がTrue,False関係なく、Falseを設定しようとすると例外が発生する。
        // ・設定は、AppDomain共通設定となり、特定のAppDomainのみの監視は行えない.
        //
        if (!AppDomain.MonitoringIsEnabled)
        {
          AppDomain.MonitoringIsEnabled = true;
        }
      }
      
      public void Dispose()
      {
        // フルブロッキングコレクションを実行.
        GC.Collect();
        PrintMonitoringValues(AppDomain.CurrentDomain);
      }
      
      public void PrintMonitoringValues(AppDomain domain)
      {
        //
        // モニタリングをONにすると、以下のプロパティにアクセスして統計情報を取得することができるようになる。
        //
        // ・MonitoringSurvivedMemorySize
        //    最後の完全なブロッキング コレクションの実行後に残された、現在のアプリケーション ドメインによって参照されていることが判明しているバイト数
        // ・MonitoringSurvivedProcessMemorySize
        //    最後の完全なブロッキング コレクションの実行後に残された、プロセス内のすべてのアプリケーション ドメインにおける合計バイト数
        // ・MonitoringTotalAllocatedMemorySize
        //    アプリケーション ドメインが作成されてから、そのアプリケーション ドメインで実行されたすべてのメモリ割り当ての合計サイズ（バイト単位）
        //    収集されたメモリは差し引かれない。
        // ・MonitoringTotalProcessorTime
        //    プロセスが開始されてから、現在のアプリケーション ドメインでの実行中にすべてのスレッドで使用された合計プロセッサ時間
        //
        // 完全なブロッキングコレクション（フルブロッキングコレクション）は、GC.Collectメソッドで実行できる。
        //
        Console.WriteLine("============================================");
        Console.WriteLine("MonitoringSurvivedMemorySize        = {0:N0}", domain.MonitoringSurvivedMemorySize);
        Console.WriteLine("MonitoringSurvivedProcessMemorySize = {0:N0}", AppDomain.MonitoringSurvivedProcessMemorySize);
        Console.WriteLine("MonitoringTotalAllocatedMemorySize  = {0:N0}", domain.MonitoringTotalAllocatedMemorySize);
        Console.WriteLine("MonitoringTotalProcessorTime        = {0}",    domain.MonitoringTotalProcessorTime);
        Console.WriteLine("============================================");
      }
    }
    
    public void Execute()
    {
      using (AppDomainMonitor monitor = new AppDomainMonitor())
      {
        monitor.PrintMonitoringValues(AppDomain.CurrentDomain);
        
        List<string> aList = new List<string>();
        for (int i = 0; i < 1000; i++)
        {
          aList.Add(string.Format("hello world-{0:D2}", i));
        }
        
        monitor.PrintMonitoringValues(AppDomain.CurrentDomain);
        
        // CPUタイムを表示したいので、少しスピン.
        Thread.SpinWait(700000000);
      }
    }
  }
  #endregion
  
  #region AppDomainSamples-04
  public class AppDomainSamples04 : MarshalByRefObject, IExecutable
  {
    public void Execute()
    {
      //
      // AppDomainを利用して、別のAppDomainで処理を実行するための方法は、いくつか存在する。
      //
      // ・AppDomain.ExecuteAssemblyを利用する。
      // ・AppDomain.DoCallbackを利用する。
      // ・AppDomain.CreateInstanceAndUnwrapを利用して、プロキシを取得し実行.
      //
      var currentDomain = AppDomain.CurrentDomain;
      var anotherDomain = AppDomain.CreateDomain("AD No.2");
      
      //
      // AppDomain.ExecuteAssemblyを利用して実行.
      // 
      // ExecuteAssemblyメソッドには、アセンブリ名を指定する。
      // ここで指定するアセンブリは実行可能である必要があり、エントリポイントを持っている必要がある。
      //
      anotherDomain.ExecuteAssembly(@"AnotherAppDomain.exe");
      
      //
      // AppDomain.DoCallbackを利用する.
      //
      // DoCallbackは指定されたデリゲートを実行するためのメソッド.
      // 別のAppDomainのDoCallbackにデリゲートを渡す事により
      // 処理がそのアプリケーションドメインで実行される。
      //
      // 当然、値渡し(Serializable)と参照渡し(MarshalByRefObject)によって実行結果が異なる場合がある.
      //
      // Staticメソッド
      Console.WriteLine("----------[Static Method]--------");
      currentDomain.DoCallBack(CallbackMethod_S);
      anotherDomain.DoCallBack(CallbackMethod_S);
      Console.WriteLine("---------------------------------");
      
      // インスタンスメソッド.
      Console.WriteLine("---------[Instance Method]-------");
      currentDomain.DoCallBack(CallbackMethod);
      anotherDomain.DoCallBack(CallbackMethod);
      Console.WriteLine("---------------------------------");
      
      // 値渡し (Serializable)
      var byvalObj = new MarshalByVal();
      
      Console.WriteLine("---------[Serializable]----------");
      currentDomain.DoCallBack(byvalObj.CallbackMethod);
      anotherDomain.DoCallBack(byvalObj.CallbackMethod);
      Console.WriteLine("---------------------------------");
      
      // 参照渡し (MarshalByRefObject)
      // MarshalByRefObjectを継承しているため、以下の例では必ずデフォルトドメインで実行されることになる。
      var byrefObj = new MarshalByRef();
      
      Console.WriteLine("-------[MarshalByRefObject]------");
      currentDomain.DoCallBack(byrefObj.CallbackMethod);
      anotherDomain.DoCallBack(byrefObj.CallbackMethod);
      Console.WriteLine("---------------------------------");
      
      //
      // AppDomain.CreateInstanceAndUnwrapを利用する。
      // プロキシを取得して処理を実行する.
      //
      var asmName  = typeof(MarshalByRef).Assembly.FullName;
      var typeName = typeof(MarshalByRef).FullName;
      
      var obj = (MarshalByRef) anotherDomain.CreateInstanceAndUnwrap(asmName, typeName);
      
      Console.WriteLine("-------[CreateInstanceAndUnwrap]------");
      obj.CallbackMethod();
      Console.WriteLine("--------------------------------------");
      
      AppDomain.Unload(anotherDomain);
    }
    
    static void CallbackMethod_S()
    {
      Utils.PrintAsmName();
    }

    void CallbackMethod()
    {
      Utils.PrintAsmName();
    }
    
    [Serializable]
    public class MarshalByVal
    {
      public void CallbackMethod()
      {
        Utils.PrintAsmName();
      }
    }
    
    public class MarshalByRef : MarshalByRefObject
    {
      public void CallbackMethod()
      {
        Utils.PrintAsmName();
      }
    }
    
    static class Utils
    {
      public static void PrintAsmName()
      {
        var domain = AppDomain.CurrentDomain.FriendlyName;
        Console.WriteLine("Run on AppDomain:{0}", domain);
      }
    }
  }
  #endregion
  
  #region SerializationSurrogateSamples-01
  /// <summary>
  /// シリアライズに関するサンプルです。
  /// </summary>
  /// <remarks>
  /// シリアル化サロゲートについて。 (ISerializationSurrogate)
  /// </remarks>
  public class SerializationSurrogateSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // 普通のシリアライズ処理.
      //
      var obj = MakeSerializableObject();
      using (var stream = new MemoryStream())
      {
        var formatter = new BinaryFormatter();
        
        // 成功する.
        formatter.Serialize(stream, obj);
        
        stream.Position = 0;
        Console.WriteLine(formatter.Deserialize(stream));
      }
      
      //
      // シリアライズ不可 (Serializable属性をつけていない)
      //
      var obj2 = MakeNotSerializableObject();
      using (var stream = new MemoryStream())
      {
        var formatter = new BinaryFormatter();
        
        try
        {
          // 対象クラスにSerializable属性が付与されていないので
          // 以下を実行すると例外が発生する.
          formatter.Serialize(stream, obj2);
          
          stream.Position = 0;
          Console.WriteLine(formatter.Deserialize(stream));
        }
        catch (SerializationException ex)
        {
          Console.WriteLine("[ERROR]: {0}", ex.Message);
        }
      }
      
      //
      // シリアル化サロゲート. (SerializationSurrogate)
      //
      var obj3 = MakeNotSerializableObject();
      using (var stream = new MemoryStream())
      {
        var formatter = new BinaryFormatter();
        
        //
        // シリアル化サロゲートを行うために、以下の手順で設定を行う.
        //
        // 1.SurrogateSelectorオブジェクトを用意.
        // 2.自作Surrogateクラスを用意.
        // 3.SurrogateSelector.AddSurrogateでSurrogateオブジェクトを設定
        // 4.SurrogateSelectorをFormatterに設定.
        //
        // これにより、シリアライズ不可なオブジェクトをFormatterにてシリアライズ/デシリアライズ
        // する際にシリアル化サロゲートが行われるようになる。
        //
        var selector  = new SurrogateSelector();
        var surrogate = new CanNotSerializeSurrogate();
        var context   = new StreamingContext(StreamingContextStates.All);
        
        selector.AddSurrogate(typeof(CanNotSerialize), context, surrogate);
        
        formatter.SurrogateSelector = selector;
        
        try
        {
          // 通常、以下を実行すると例外が発生するが
          // シリアル化サロゲートを行うので、エラーとならずシリアライズが成功する.
          formatter.Serialize(stream, obj3);
          
          stream.Position = 0;
          Console.WriteLine(formatter.Deserialize(stream));
        }
        catch (SerializationException ex)
        {
          Console.WriteLine("[ERROR]: {0}", ex.Message);
        }
      }
    }
    
    IHasNameAndAge MakeSerializableObject()
    {
      return new CanSerialize 
                 { 
                    Name = "hoge"
                   ,Age = 99 
                 };
    }
    
    IHasNameAndAge MakeNotSerializableObject()
    {
      return new CanNotSerialize
                 {
                     Name = "hehe"
                    ,Age = 98
                 };
    }
    
    #region SampleInterfaceAndClasses
    interface IHasNameAndAge
    {
      string Name { get; set; }
      int    Age  { get; set; }
    }
    
    // シリアライズ可能なクラス
    [Serializable]
    class CanSerialize : IHasNameAndAge
    {
      string _name;
      int    _age;
      
      public string Name
      {
        get { return _name; }
        set { _name = value; }
      }
      
      public int Age 
      {
        get { return _age; }
        set { _age = value; }
      }
      
      public override string ToString() 
      { 
        return string.Format("[CanSerialize] Name={0}, Age={1}", Name, Age); 
      }
    }
    
    // シリアライズ不可なクラス
    class CanNotSerialize : IHasNameAndAge
    {
      string _name;
      int    _age;
      
      public string Name
      {
        get { return _name; }
        set { _name = value; }
      }
      
      public int Age 
      {
        get { return _age; }
        set { _age = value; }
      }
      
      public override string ToString() 
      { 
        return string.Format("[CanNotSerialize] Name={0}, Age={1}", Name, Age); 
      }
    }
    
    // CanNotSerializeクラスのためのサロゲートクラス.
    class CanNotSerializeSurrogate : ISerializationSurrogate
    {
      // シリアライズ時に呼び出されるメソッド
      public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
      {
        CanNotSerialize targetObj = obj as CanNotSerialize;
        
        //
        // シリアライズする項目と値を以下のようにinfoに設定していく.
        //
        info.AddValue("Name", targetObj.Name);
        info.AddValue("Age",  targetObj.Age);
      }
      
      // デシリアライズ時に呼び出されるメソッド.
      public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
      {
        CanNotSerialize targetObj = obj as CanNotSerialize;
        
        //
        // infoから値を取得し、対象となるオブジェクトに設定.
        //
        targetObj.Name = info.GetString("Name");
        targetObj.Age  = info.GetInt32("Age");
        
        // Formatterは, この戻り値を無視するので戻り値はnullで良い.
        return null;
      }
    }
    #endregion
  }
  #endregion
  
  #region CallContextSamples-01
  /// <summary>
  /// 実行コンテキスト(ExecutionContext)と論理呼び出しコンテキスト(CallContext)のサンプルです。
  /// </summary>
  public class CallContextSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // すべてのスレッドには、実行コンテキスト (Execution Context) が関連付けられている。
      // 実行コンテキストには
      //    ・そのスレッドのセキュリティ設定 (圧縮スタック、Thread.Principal, Windowsの認証情報)
      //    ・ホスト設定 (HostExecutionContextManager)
      //    ・論理呼び出しコンテキストデータ (CallContext)
      // が紐付いている。
      // 
      // その中でも、論理呼び出しコンテキスト (CallContext) は、LogicalSetDataメソッド、LogicalGetDataメソッドを
      // 利用することにより、同じ実行コンテキストを持つスレッド間でデータを共有することができる。
      // 既定では、CLRは起動元のスレッドの実行コンテキストが自動的に伝播されるようにしてくれる。
      //
      // 実行コンテキストの伝播方法は、ExecutionContextクラスを利用することにより変更することができる。
      // ExecutionContext.SuppressFlowメソッドにはSerucityCriticalAttribute
      // が付与されているので、環境によっては動作しなくなる可能性がある。
      // (SerucityCriticalAttributeは、完全信頼を要求する属性)
      //
      var numberOfThreads = 5;
      
      using (var cde = new CountdownEvent(numberOfThreads))
      {
        //
        // メインスレッド上にて、論理呼び出しコンテキストデータを設定.
        //
        CallContext.LogicalSetData("Message", "Hello World");
        
        //
        // 既定の設定のまま (親元のExecutionContextをそのまま継承） で、別スレッド生成.
        //
        ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("First Thread", cde));
        
        //
        // 実行コンテキストの伝播方法を変更.
        //   SuppressFlowメソッドは、実行コンテキストフローを抑制するメソッド.
        // SuppressFlowメソッドは、AsyncFlowControlを戻り値として返却する。
        // 抑制した実行コンテキストを復元するには、AsyncFlowControl.Undoを呼び出す。
        //
        AsyncFlowControl flowControl = ExecutionContext.SuppressFlow();
        
        //
        // 抑制された実行コンテストの状態で、別スレッド生成.
        //
        ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("Second Thread", cde));
        
        //
        // 実行コンテキストを復元.
        //
        flowControl.Undo();
        
        //
        // 再度、別スレッド生成.
        //
        ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("Third Thread", cde));
        
        //
        // 再度、実行コンテキストを抑制し、抑制されている間に論理呼び出しコンテキストデータを変更し
        // その後、実行コンテキストを復元する.
        //
        flowControl = ExecutionContext.SuppressFlow();
        CallContext.LogicalSetData("Message", "Modified....");
        
        ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("Fourth Thread", cde));
        flowControl.Undo();
        ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("Fifth Thread", cde));
        
        cde.Wait();
      }
    }
    
    void ShowCallContextLogicalData(object state)
    {
      var data = state as ThreadData;
      
      Console.WriteLine(
         "Thread: {0, -15}, Id: {1}, Message: {2}"
        ,data.Name
        ,Thread.CurrentThread.ManagedThreadId
        ,CallContext.LogicalGetData("Message")
      );
      
      data.Counter.Signal();
    }
    
    #region Inner Classes
    class ThreadData
    {
      public string         Name    { get; private set;}
      public CountdownEvent Counter { get; private set;}
      
      public ThreadData(string name, CountdownEvent cde)
      {
        Name    = name;
        Counter = cde;
      }
    }
    #endregion
  }
  #endregion
  
  #region IEquatableSamples-01
  /// <summary>
  /// IEquatable<T>のサンプルです。
  /// </summary> -->
  public class IEquatableSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // IEquatable<T>インターフェースは、2つのインスタンスが等しいか否かを判別するための
      // 型指定のEqualsメソッドを定義しているインターフェースである。
      //
      // このインターフェースを実装することで、通常のobject.Equals以外に型が指定された
      // Equalsメソッドを持つことができるようになる。
      // このインターフェースは、特に構造体を定義する上で重要であり、構造体の場合、object.Equalsで
      // 比較を行うと、ボックス化が発生してしまうため、IEquatable<T>を実装することが多い。
      // (ボックス化が発生しなくなるため。）
      //
      // また、厳密には必須ではないが、IEquatable<T>を実装する場合、同時に以下のメソッドもオーバーライドするのが普通である。
      //   ・object.Equals
      //   ・object.GetHashCode
      // object.Equalsをオーバーライドするのは、IEquatableを実装していてもクラスによっては、それを無視して強制的にobject.Equalsで
      // 比較する処理が存在するためである。
      //
      // IEquatable<T>インターフェースは、Dictionary<TKey, TValue>, List<T>などのジェネリックコレクションにて
      // Contains, IndexOf, LastIndexOf, Removeなどの各メソッドで等価性をテストする場合に利用される。
      // (ArrayのIndexOfメソッドなどでも同様に利用される。)
      // 同じインターフェースで、比較機能を提供するものとして、IComparable<T>インターフェースがある。
      //
      // object.GetHashCodeをオーバーライドするのは、上の理由によりobject.Equalsがオーバーライドされるため。
      //
      Data data1 = new Data(1, "Hello World");
      Data data2 = new Data(2, "Hello World2");
      Data data3 = new Data(3, "Hello World3");
      Data data4 = data3;
      Data data5 = new Data(1, "Hello World4");
      
      Console.WriteLine("data1 equals data2? ==> {0}", data1.Equals(data2));
      Console.WriteLine("data1 equals data3? ==> {0}", data1.Equals(data3));
      Console.WriteLine("data1 equals data4? ==> {0}", data1.Equals(data4));
      Console.WriteLine("data1 equals data5? ==> {0}", data1.Equals(data5));
      
      object d1 = data1;
      object d2 = data2;
      object d5 = data5;
      
      Console.WriteLine("data1 equals data2? ==> {0}", d1.Equals(d2));
      Console.WriteLine("data1 equals data5? ==> {0}", d1.Equals(d5));
      
      Data[] dataArray = { data1, data2, data3, data4, data5 };
      Console.WriteLine("IndexOf={0}", Array.IndexOf(dataArray, data3));
    }
    
    sealed class Data : IEquatable<Data>
    {
      public Data(int id, string name)
      {
        Id = id;
        Name = name;
      }
      
      public int Id
      {
        get;
        private set;
      }
      
      public string Name
      {
        get;
        private set;
      }
      
      // IEquatable<T>の実装.
      public bool Equals(Data other)
      {
        Console.WriteLine("\t→→Call IEquatable.Equals");
        
        if (other == null)
        {
          return false;
        }
        
        return Id == other.Id;
      }
      
      // object.Equals
      public override bool Equals(object other)
      {
        Console.WriteLine("\t→→Call object.Equals");
        
        Data data = other as Data;
        if (data == null)
        {
          return false;
        }
        
        return Equals(data);
      }
      
      // object.GetHashCode
      public override int GetHashCode()
      {
        return Id.GetHashCode();
      }
    }
  }
  #endregion
  
  #region EqualityComparerSamples-01
  public class EqualityComparerSamples01 : IExecutable
  {
    public void Execute()
    {
      var d1 = new Data("data1", "data1-value1");
      var d2 = new Data("data2", "data2-value1");
      var d3 = new Data("data3", "data3-value1");
      
      // d1と同じ値を持つ別のインスタンスを作成しておく.
      var d1_2 = new Data(d1.Name, d1.Value);
      
      /////////////////////////////////////////////////////////
      //
      // object.Equalsで比較.
      //
      Console.WriteLine("===== object.Equalsで比較. =====");
      Console.WriteLine("d1.Equals(d2) : {0}", d1.Equals(d2));
      Console.WriteLine("d1.Equals(d3) : {0}", d1.Equals(d3));
      Console.WriteLine("d1.Equals(d1_2) : {0}", d1.Equals(d1_2));
      
      /////////////////////////////////////////////////////////
      //
      // EqualityComparerで比較.
      //
      var comparer = new DataEqualityComparer();
      
      Console.WriteLine("===== EqualityComparerで比較. =====");
      Console.WriteLine("d1.Equals(d2) : {0}", comparer.Equals(d1, d2));
      Console.WriteLine("d1.Equals(d3) : {0}", comparer.Equals(d1, d3));
      Console.WriteLine("d1.Equals(d1_2) : {0}", comparer.Equals(d1, d1_2));
      
      /////////////////////////////////////////////////////////
      //
      // Dictionaryで一致するか否かを確認 (EqualityComparer無し)
      //
      var dict1 = new Dictionary<Data, string>();
      
      dict1[d1] = d1.Value;
      dict1[d2] = d2.Value;
      dict1[d3] = d3.Value;
      
      // 以下のコードでは、ちゃんと値が取得できる. (参照が同じため)
      Console.WriteLine("===== Dictionaryで一致するか否かを確認 (EqualityComparer無し). =====");
      Console.WriteLine("key:d1 ==> {0}", dict1[d1]);
      Console.WriteLine("key:d3 ==> {0}", dict1[d3]);
      
      // 以下のコードでは、ちゃんとtrueが取得できる. (参照が同じため)
      Console.WriteLine("contains-key: d1 ==> {0}", dict1.ContainsKey(d1));
      Console.WriteLine("contains-key: d2 ==> {0}", dict1.ContainsKey(d2));
      Console.WriteLine("contains-key: d3 ==> {0}", dict1.ContainsKey(d3));
      
      //
      // 同じ値を持つ、別インスタンスを作成し、EqualityComparerなしのDictionaryで試してみる.
      //
      var d4 = new Data(d1.Name, d1.Value);
      var d5 = new Data(d2.Name, d2.Value);
      var d6 = new Data(d3.Name, d3.Value);
      
      // 以下のコードを実行すると例外が発生する. (キーとして一致しないため)
      try
      {
        Console.WriteLine("===== 同じ値を持つ、別インスタンスを作成し、EqualityComparerなしのDictionaryで試してみる. =====");
        Console.WriteLine("key:d4 ==> {0}", dict1[d4]);
      }
      catch (KeyNotFoundException)
      {
        Console.WriteLine("キーとしてd4を指定しましたが、一致するキーが見つかりませんでした。");
      }
      
      // 当然、ContainsKeyメソッドもfalseを返す.
      Console.WriteLine("contains-key: d4 ==> {0}", dict1.ContainsKey(d4));
      
      
      /////////////////////////////////////////////////////////
      //
      // Dictionaryを作成する際に、EqualityComparerを指定して作成.
      //
      var dict2 = new Dictionary<Data, string>(comparer);
      
      dict2[d1] = d1.Value;
      dict2[d2] = d2.Value;
      dict2[d3] = d3.Value;

      // 以下のコードでは、ちゃんと値が取得できる. (EqualityComparerを指定しているため)
      Console.WriteLine("===== Dictionaryを作成する際に、EqualityComparerを指定して作成. =====");
      Console.WriteLine("key:d4 ==> {0}", dict2[d4]);
      Console.WriteLine("key:d6 ==> {0}", dict2[d6]);
      
      // 以下のコードでは、ちゃんとtrueが取得できる. (EqualityComparerを指定しているため)
      Console.WriteLine("contains-key: d4 ==> {0}", dict2.ContainsKey(d4));
      Console.WriteLine("contains-key: d5 ==> {0}", dict2.ContainsKey(d5));
      Console.WriteLine("contains-key: d6 ==> {0}", dict2.ContainsKey(d6));

      /////////////////////////////////////////////////////////
      //
      // EqualityComparer<T>には、Defaultという静的プロパティが存在する.
      // このプロパティは、Tに指定された型がIEquatable<T>を実装しているかどうかを
      // チェックし、実装している場合は、内部でIEquatable<T>の実装を利用する
      // EqualityComaparer<T>を作成して返してくれる.
      //
      // Tに指定された型が、IEquatable<T>を実装していない場合
      // object.Equals, object.GetHashCodeを利用する実装を返す.
      //
      // 本サンプルで利用するサンプルクラスは、以下のようになっている.
      //   Dataクラス： IEquatable<T>を実装していない.
      //   Data2クラス： IEquatable<T>を実装している.
      //
      // 上記のクラスに対して、それぞれEqualityComparer<T>.Defaultを呼び出すと以下の
      // クラスのインスタンスが返ってくる.
      //   Dataクラス：  ObjectEqualityComparer`1
      //   Data2クラス: GenericEqualityComparer`1
      // IEquatable<T>を実装している場合は、GenericEqualityComparerが
      // 実装していない場合は、ObjectEqualityComparerとなる。
      //
      var dataEqualityComparer  = EqualityComparer<Data>.Default;
      var data2EqualityComparer = EqualityComparer<Data2>.Default;
      
      // 生成された型を表示.
      Console.WriteLine("===== EqualityComparer<T>.Defaultの動作. =====");
      Console.WriteLine("Data={0}, Data2={1}", dataEqualityComparer.GetType().Name, data2EqualityComparer.GetType().Name);
      
      // それぞれサンプルデータを作成して、比較してみる.
      // 尚、どちらの場合も1番目のデータと3番目のデータのキーが同じになるようにしている.
      var data_1 = new Data("data_1", "value_1");
      var data_2 = new Data("data_2", "value_2");
      var data_3 = new Data("data_1", "value_3");
      
      var data2_1 = new Data2("data2_1", "value2_1");
      var data2_2 = new Data2("data2_2", "value2_2");
      var data2_3 = new Data2("data2_1", "value2_3");
      
      // DataクラスのEqualityComparerを使用して比較.
      Console.WriteLine("data_1.Equals(data_2) : {0}", dataEqualityComparer.Equals(data_1, data_2));
      Console.WriteLine("data_1.Equals(data_3) : {0}", dataEqualityComparer.Equals(data_1, data_3));
      
      // Data2クラスのEqualityComparerを使用して比較.
      Console.WriteLine("data2_1.Equals(data2_2) : {0}", data2EqualityComparer.Equals(data2_1, data2_2));
      Console.WriteLine("data2_1.Equals(data2_3) : {0}", data2EqualityComparer.Equals(data2_1, data2_3));
    }
    
    class Data
    {
      public Data(string name, string value)
      {
        Name  = name;
        Value = value;
      }
      
      public string Name
      {
        get;
        private set;
      }
      
      public string Value
      {
        get;
        private set;
      }
      
      public override string ToString()
      {
        return string.Format("Name={0}, Value={1}", Name, Value);
      }
    }
    
    class DataEqualityComparer : EqualityComparer<Data>
    {
      public override bool Equals(Data x, Data y)
      {
        if (x == null && y == null)
        {
          return true;
        }
        
        if (x == null || y == null)
        {
          return false;
        }
        
        return x.Name == y.Name;
      }
      
      public override int GetHashCode(Data x)
      {
        if (x == null || string.IsNullOrEmpty(x.Name))
        {
          return string.Empty.GetHashCode();
        }
        
        return x.Name.GetHashCode();
      }
    }
    
    class Data2 : IEquatable<Data2>
    {
      public Data2(string name, string value)
      {
        Name  = name;
        Value = value;
      }
      
      public string Name
      {
        get;
        private set;
      }
      
      public string Value
      {
        get;
        private set;
      }
      
      public bool Equals(Data2 other)
      {
        if (other == null)
        {
          return false;
        }
        
        return other.Name == Name;
      }
      
      public override bool Equals(object other)
      {
        Data2 data = other as Data2;
        if (data == null)
        {
          return false;
        }
        
        return Equals(data);
      }
      
      public override int GetHashCode()
      {
        return string.IsNullOrEmpty(Name) ? string.Empty.GetHashCode() : Name.GetHashCode();
      }
    }
  }
  #endregion
  
  #region ZipFileSamples-01
  /// <summary>
  /// System.IO.Compression.ZipFileクラスのサンプルです。
  /// </summary>
  /// <remarks>
  /// ZipFileクラスは、.NET Framework 4.5で追加されたクラスです。
  /// このクラスを利用するには、「System.IO.Compression.FileSystem.dll」を
  /// 参照設定に追加する必要があります。
  /// このクラスは、Metroアプリでは利用できません。
  /// Metroアプリでは、代わりにZipArchiveクラスを利用します。
  /// </remarks>
  public class ZipFileSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // ZipFileクラスは、ZIP形式のファイルを扱うためのクラスである。
      // 同じ事が出来るクラスとして、ZipArchiveクラスが存在するが
      // こちらは、きめ細かい処理が行えるクラスとなっており
      // ZipFileクラスは、ユーティリティクラスの扱いに近い。
      //
      // ZipFileクラスに定義されているメソッドは、全てstaticメソッドとなっている。
      //
      // 簡単に圧縮・解凍するためのメソッドとして
      //   ・CreateFromDirectory(string, string)
      //   ・ExtractToDirectory(string, string)
      // が用意されている。
      //
      // 尚、このクラスはMetroスタイルアプリ (新しい名前はWindows 8スタイルUI？)
      // では利用できないクラスである。Metroでは、ZipArchiveを利用することになる。
      // (http://msdn.microsoft.com/en-us/library/system.io.compression.zipfile)
      //
      
      //
      // 圧縮.
      //
      string srcDirectory = Environment.CurrentDirectory;
      string dstDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      string dstFilePath  = Path.Combine(dstDirectory, "ZipTest.zip");
      
      if (File.Exists(dstFilePath))
      {
        File.Delete(dstFilePath);
      }
      
      ZipFile.CreateFromDirectory(srcDirectory, dstFilePath);
      
      //
      // 解凍.
      //
      string extractDirectory = Path.Combine(dstDirectory, "ZipTest");
      if (Directory.Exists(extractDirectory))
      {
        Directory.Delete(extractDirectory, recursive: true);
        Directory.CreateDirectory(extractDirectory);
      }
      
      ZipFile.ExtractToDirectory(dstFilePath, extractDirectory);
    }
  }
  #endregion
  
  #region ZipFileSamples-02
  /// <summary>
  /// System.IO.Compression.ZipFileクラスのサンプルです。
  /// </summary>
  /// <remarks>
  /// ZipFileクラスは、.NET Framework 4.5で追加されたクラスです。
  /// このクラスを利用するには、「System.IO.Compression.FileSystem.dll」を
  /// 参照設定に追加する必要があります。
  /// このクラスは、Metroアプリでは利用できません。
  /// Metroアプリでは、代わりにZipArchiveクラスを利用します。
  /// </remarks>
  public class ZipFileSamples02 : IExecutable
  {
    string _zipFilePath;
    
    string DesktopPath
    {
      get
      {
        return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      }
    }
    
    public void Execute()
    {
      //
      // ZipFileクラスの以下のメソッドを利用すると、既存のZIPファイルを開く事が出来る。
      //   ・OpenRead
      //   ・Open(string, ZipArchiveMode)
      //   ・Open(string, ZipArchiveMode, Encoding)
      // どのメソッドも、戻り値としてZipArchiveクラスのインスタンスを返す。
      // 実際にZIPファイル内のエントリ取得は、ZipArchiveから行う.
      // ZipArchiveクラスは、IDisposableを実装しているのでusingブロックで
      // 利用するのが好ましい。
      //
      // 尚、ZipArchiveクラスを利用する場合、参照設定に
      //   System.IO.Compression.dll
      // を追加する必要がある。
      //
      Prepare();
      
      //
      // OpenRead
      //
      using (var archive = ZipFile.OpenRead(_zipFilePath))
      {
        archive.Entries.ToList().ForEach(PrintEntry);
      }
      
      //
      // Open(string, ZipArchiveMode)
      //
      using (var archive = ZipFile.Open(_zipFilePath, ZipArchiveMode.Read))
      {
        //
        // ZipArchive.Entriesプロパティからは、ReadOnlyCollection<ZipArchiveEntry>が取得できる。
        // 1エントリの情報は、ZipArchiveEntryから取得できる。
        //
        // ZipArchiveEntryには、Nameというプロパティが存在し、このプロパティから実際のファイル名を取得できる。
        // また、Lengthプロパティより圧縮前のファイルサイズが取得できる。圧縮後のサイズは、CompressedLengthから取得できる。
        // エントリの内容を読み出すには、ZipArchiveEntry.Openメソッドを利用する。
        //
        archive.Entries.ToList().ForEach(PrintEntry);
      }
      
      //
      // Open(string, ZipArchiveMode, Encoding)
      //   テキストファイルのみ、中身を読み出して出力.
      //
      using (var archive = ZipFile.Open(_zipFilePath, ZipArchiveMode.Read, Encoding.GetEncoding("sjis")))
      {
        archive.Entries.Where(entry => entry.Name.EndsWith("txt")).ToList().ForEach(PrintEntryContents);
      }
      
      File.Delete(_zipFilePath);
      Directory.Delete(Path.Combine(DesktopPath, "ZipTest"), recursive: true);
    }
    
    void Prepare()
    {
      //
      // サンプルZIPファイルを作成しておく.
      // (デスクトップ上にZipTest.zipという名称で出力される)
      //
      new ZipFileSamples01().Execute();
      _zipFilePath = Path.Combine(DesktopPath, "ZipTest.zip");
    }
    
    void PrintEntry(ZipArchiveEntry entry)
    {
      Console.WriteLine("[{0}, {1}]", entry.Name, entry.Length);
    }
    
    void PrintEntryContents(ZipArchiveEntry entry)
    {
      using (var reader = new StreamReader(entry.Open(), Encoding.GetEncoding("sjis")))
      {
        for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
        {
          Console.WriteLine(line);
        }
      }
    }
  }
  #endregion
  
  #region ZipFileSamples-03
  /// <summary>
  /// System.IO.Compression.ZipFileクラスのサンプルです。
  /// </summary>
  /// <remarks>
  /// ZipFileクラスは、.NET Framework 4.5で追加されたクラスです。
  /// このクラスを利用するには、「System.IO.Compression.FileSystem.dll」を
  /// 参照設定に追加する必要があります。
  /// このクラスは、Metroアプリでは利用できません。
  /// Metroアプリでは、代わりにZipArchiveクラスを利用します。
  ///
  /// 尚、ZipArchiveクラスを利用する場合
  ///   System.IO.Compression.dll
  /// を参照設定に追加する必要があります。
  /// </remarks>
  public class ZipFileSamples03 : IExecutable
  {
    string _zipFilePath;
    
    string DesktopPath
    {
      get
      {
        return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      }
    }
    
    public void Execute()
    {
      //
      // ZIPファイルの作成および更新.
      //   作成および更新の場合、ZipArchiveクラスを利用する.
      // 
      // ・エントリの追加： ZipArchive.CreateEntryFromFile OR ZipArchive.CreateEntry
      //
      // CreateEntryFromFileは、メソッドの名前が示す通り元ファイルがある場合に利用する。
      // 元となるファイルが存在する場合はこれが楽である。
      //
      // CreateEntryは、エントリのみを新規作成するメソッド。データは自前で流し込む必要がある。
      //
      Prepare();
      
      //
      // Zipファイルを新規作成.
      //
      using (var archive = ZipFile.Open(_zipFilePath, ZipArchiveMode.Create))
      {
        //
        // 元ファイルが存在している場合は、CreateEntryFromFileを利用するのが楽.
        //
        archive.CreateEntryFromFile("Persons.txt", "Persons.txt");
      }
      
      //
      // Zipファイルの内容を更新.
      //
      using (var archive = ZipFile.Open(_zipFilePath, ZipArchiveMode.Update))
      {
        //
        // 元ファイルは存在するが、今度はCreateEntryメソッドで新規エントリのみを作成しデータは、手動で流し込む.
        //
        using (var reader = new BinaryReader(File.Open("database.png", FileMode.Open)))
        {
          var newEntry = archive.CreateEntry("database.png");
          using (var writer = new BinaryWriter(newEntry.Open()))
          {
            WriteAllBytes(reader, writer);
          }
        }
      }
      
      File.Delete(_zipFilePath);
    }
    
    void Prepare()
    {
      _zipFilePath = Path.Combine(DesktopPath, "ZipTest2.zip");
      if (File.Exists(_zipFilePath))
      {
        File.Delete(_zipFilePath);
      }
    }
    
    void WriteAllBytes(BinaryReader reader, BinaryWriter writer)
    {
      try
      {
        for (;;)
        {
          writer.Write(reader.ReadByte());
        }
      }
      catch (EndOfStreamException)
      {
        writer.Flush();
      }
    }
  }
  #endregion
  
  #region ProgressSamples-01
  /// <summary>
  /// System.Progress<T>のサンプルです。
  /// </summary>
  /// <remarks>
  /// このクラスは、.NET Framework 4.5から追加された型です。
  /// </remarks>
  public class ProgressSamples01 : IExecutable
  {
    /// <summary>
    /// サンプル用ウィンドウ
    /// </summary>
    /// <remarks>
    /// このウィンドウには、ProgressBarが一つだけ配置されています。
    /// </remarks>
    class SampleWindow : Window
    {
      const double MIN = 0.0;
      const double MAX = 100.0;
      
      ProgressBar _bar;
      
      public SampleWindow()
      {
        InitializeControl();
        InitializeEvent();
      }
      
      void InitializeControl()
      {
        Width  = 400;
        Height = 80;
        
        _bar = new ProgressBar
               {
                  Minimum = MIN
                 ,Maximum = MAX
                 ,Value   = MIN
                 ,SmallChange = MIN
                };
        
        Content = _bar;
      }
      
      void InitializeEvent()
      {
        //
        // ロードイベント.
        //   やってることは、単純にプログレスバーの進捗を伸ばしていくだけ.
        //   進捗を伸ばす部分に、Progress<T>を利用している.
        //
        //   内部でawaitを使用しているのでラムダにasyncを指定.
        //
        Loaded += async (s, e) =>
        {
          //
          // .NET Framework 4.5より、CancellationTokenSourceのコンストラクタに
          // キャンセル状態になるまでのタイムアウト値を設定できるようになった。
          // 下記の場合だと、5秒後に自動的にキャンセル扱いになる.
          //
          var tokenSource = new CancellationTokenSource(5000);
          
          //
          // プログレスバーの進捗を伸ばすためのProgress<T>を構築.
          // コンストラクタにActionを渡しても、ProgressChangedイベントにハンドラを設定してもどちらでも良い。
          //
          // Progress<T>は、インスタンス生成時に現在のSynchronizationContextをキャプチャする。
          // なので、UIが絡む処理を行う場合は、必ずUIスレッド上でインスタンスを生成する必要がある。
          //
          var progress = new Progress<int>(SetProgress);
          // わざと、UIスレッドではない場所でインスタンスを生成している.
          // これを下のPerformStepメソッドに渡すと、SetProgressメソッドに入ってきた時点で
          // Control.InvokeRequiredがtrueとなる。つまり、UIスレッド以外からSetProgressが
          // 呼ばれているので、Invokeしないといけない状態となる。
          //var progress2 = Task.Run(() => new Progress<int>(SetProgress)).Result;

          //
          // 処理開始.
          //   awaitを指定しているので、処理はUIスレッドと切り離されて実行され
          //   プログレスバーの進捗設定の時のみUIスレッドで実行される. (IProgress<T>.ReportからのSetProgress呼び出し)
          //   await後のタイトル設定は、PerformStepメソッドが終了次第実行される。
          //
          // 第二引数に渡しているprogressを、progress2に変更して実行すると
          // 画面上のプログレスバーの進捗が一切進まなくなる。
          // これは、progress2の方が、UIスレッドではない場所で生成されたため
          // キャプチャされたSynchronizationContextがDispatcherSynchronizationContextでは無いためである。
          //
          await PerformStep(tokenSource.Token, progress);
          Title = "DONE.";
        };
      }
      
      async Task PerformStep(CancellationToken token, IProgress<int> progress)
      {
        for (;;)
        {
          foreach (var value in Enumerable.Range(0, 100))
          {
            if (token.IsCancellationRequested)
            {
              return;
            }
            
            await Task.Delay(10);
            
            //
            // Reportメソッドを呼び出すには
            // IProgress<T>にキャストして利用する必要がある。
            // (Progress<T>にて、明示的インターフェース実装されているため）
            //
            progress.Report(value);
          }
        }
      }
      
      void SetProgress(int newValue)
      {
        //
        // UIスレッドで実行されていない場合を視覚的に見たいので
        // UIスレッドで実行されていない場合はわざと何もしない.
        //
        if (!_bar.Dispatcher.CheckAccess())
        {
          return;
        }
        
        _bar.Value = newValue;
      }
    }
    
    [STAThread]
    public void Execute()
    {
      //
      // Progress<T>は、.NET Framework 4.5で追加された型である。
      // Progress<T>は、IProgress<T>インターフェースを実装しており
      // 文字通り、進捗状況を処理するために存在する。
      //
      // 利用する場合、コンストラクタにAction<T>を指定するか
      // ProgressChangedイベントをハンドルするかで処理できる。
      //
      // 尚、Progress<T>はインスタンスが作成される際に
      // 現在のSynchronizationContextをキャプチャし
      // ProgressChangedイベントをキャプチャしたSynchronizationContext上で
      // 実行してくれるため、イベントハンドラ内でコントロールを操作しても問題無い。
      // (インスタンスの作成自体をイベントスレッド以外で行っている場合は別）
      //
      // コンソールアプリのようにSynchronizationContextが紐づかない
      // コンテキストの場合は、ThreadPool上で実行される。
      //
      var app = new Application();
      app.Run(new SampleWindow());
    }
  }
  #endregion
  
  #region ProgressSamples-02
  /// <summary>
  /// System.Progress<T>のサンプルです。
  /// </summary>
  /// <remarks>
  /// このクラスは、.NET Framework 4.5から追加された型です。
  /// </remarks>
  public class ProgressSamples02 : IExecutable
  {
    class SampleForm : WinFormsForm
    {
      const int MIN = 0;
      const int MAX = 100;
      
      WinFormsLabel _label;
      WinFormsProgressBar   _bar;
      WinFormsButton        _btn;
      
      public SampleForm()
      {
        InitializeControl();
        InitializeEvent();
      }
      
      void InitializeControl()
      {
        SuspendLayout();
        
        Width  = 400;
        Height = 130;
        
        _label = new WinFormsLabel
        {
           Text     = string.Empty
          ,AutoSize = false
          ,Width    = 350
        };
        
        _bar = new WinFormsProgressBar
        {
           Minimum = MIN
          ,Maximum = MAX
          ,Width   = 350
          ,Value   = MIN
          ,Step    = 1
          ,Style   = WinFormsProgressBarStyle.Continuous
        };
        
        _btn = new WinFormsButton
        {
           Text  = "Cancel"
          ,Width = 120
        };
        
        var panel = new WinFormsFlowLayoutPanel
        {
           FlowDirection = WinFormsFlowDirection.TopDown
          ,Dock          = WinFormsDockStyle.Fill
        };
        
        panel.Controls.Add(_label);
        panel.Controls.Add(_bar);
        panel.Controls.Add(_btn);
        
        Controls.Add(panel);
        
        ResumeLayout();
      }
      
      void InitializeEvent()
      {
        Load += async (s, e) =>
        {
          var tokenSource = new CancellationTokenSource();
          var progress    = new Progress<ProgressMessage>(SetProgress);
          
          _btn.Tag     = tokenSource;
          _bar.Maximum = Directory.EnumerateFiles(".").Count();
          
          await Compress(tokenSource.Token, progress);
          
          Text = "DONE";
          if (tokenSource.IsCancellationRequested)
          {
            Text = "CANCEL";
          }
        };
        
        _btn.Click += (s, e) =>
        {
          (_btn.Tag as CancellationTokenSource).Cancel();
        };
      }
      
      async Task Compress(CancellationToken token, IProgress<ProgressMessage> progress)
      {
        string ZipFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ZipTest3.zip");
        string TmpFilePath = ZipFilePath + ".tmp";
        
        if (File.Exists(ZipFilePath))
        {
          File.Move(ZipFilePath, TmpFilePath);
        }
        
        using (var archive = ZipFile.Open(ZipFilePath, ZipArchiveMode.Create))
        {
          foreach (var filePath in Directory.EnumerateFiles("."))
          {
            if (token.IsCancellationRequested)
            {
              break;
            }
            
            progress.Report(new BeginMessage { Message = string.Format("{0}を圧縮しています...", filePath), Token = token });
            
            archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
            await Task.Delay(1000);
            
            progress.Report(new AfterMessage { Message = string.Format("{0}を圧縮完了", filePath), Token = token });
          }
        }
        
        if (token.IsCancellationRequested)
        {
          File.Delete(ZipFilePath);
          
          if (File.Exists(TmpFilePath))
          {
            File.Move(TmpFilePath, ZipFilePath);
          }
        }
        else
        {
          if (File.Exists(TmpFilePath))
          {
            File.Delete(TmpFilePath);
          }
        }
      }
      
      void SetProgress(ProgressMessage message)
      {
        if (message.Token.IsCancellationRequested)
        {
          _label.Text = "処理はキャンセルされました。";
          return;
        }
        
        _label.Text = message.Message;
        if (message is AfterMessage)
        {
          _bar.PerformStep();
        }
      }
      
      class ProgressMessage
      {
        public string Message
        {
          get;
          set;
        }
        
        public CancellationToken Token
        {
          get;
          set;
        }
      }
      
      class BeginMessage : ProgressMessage {}
      class AfterMessage : ProgressMessage {}
    }

    [STAThread]
    public void Execute()
    {
      WinFormsApplication.EnableVisualStyles();
      WinFormsApplication.Run(new SampleForm());
    }
  }
  #endregion
  
  #region ProgressSamples-03
  /// <summary>
  /// System.Progress<T>のサンプルです。
  /// </summary>
  /// <remarks>
  /// このクラスは、.NET Framework 4.5から追加された型です。
  /// </remarks>
  public class ProgressSamples03 : IExecutable
  {
    class SampleWindow : Window
    {
      TextBlock   _label;
      ProgressBar _bar;
      Button      _btn;
      
      public SampleWindow()
      {
        InitializeControl();
        InitializeEvent();
      }
      
      void InitializeControl()
      {
        Width  = 400;
        Height = 100;
        
        _label = new TextBlock
        {
          Text = string.Empty
        };
        
        _bar = new ProgressBar
        {
           Height = 20
          ,Minimum = 0
        };
        
        _btn = new Button
        {
           Content = "Cancel"
          ,Margin  = new Thickness(300, 0, 0, 0)
        };
        
        var panel = new StackPanel();
        
        panel.Children.Add(_label);
        panel.Children.Add(_bar);
        panel.Children.Add(_btn);
        
        Content = panel;
      }
      
      void InitializeEvent()
      {
        Loaded += async (s, e) =>
        {
          var tokenSource = new CancellationTokenSource();
          var progress    = new Progress<ProgressMessage>(SetProgress);
          
          _btn.Tag     = tokenSource;
          _bar.Maximum = Directory.EnumerateFiles(".").Count();
          
          await Compress(tokenSource.Token, progress);
          
          Title = "DONE";
          if (tokenSource.IsCancellationRequested)
          {
            Title = "CANCEL";
          }
        };
        
        _btn.Click += (s, e) =>
        {
          (_btn.Tag as CancellationTokenSource).Cancel();
        };
      }
      
      async Task Compress(CancellationToken token, IProgress<ProgressMessage> progress)
      {
        string ZipFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ZipTest3.zip");
        string TmpFilePath = ZipFilePath + ".tmp";
        
        if (File.Exists(ZipFilePath))
        {
          File.Move(ZipFilePath, TmpFilePath);
        }
        
        using (var archive = ZipFile.Open(ZipFilePath, ZipArchiveMode.Create))
        {
          foreach (var filePath in Directory.EnumerateFiles("."))
          {
            if (token.IsCancellationRequested)
            {
              break;
            }
            
            progress.Report(new BeginMessage { Message = string.Format("{0}を圧縮しています...", filePath), Token = token });
            
            archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
            await Task.Delay(1000);
            
            progress.Report(new AfterMessage { Message = string.Format("{0}を圧縮完了", filePath), Token = token });
          }
        }
        
        if (token.IsCancellationRequested)
        {
          File.Delete(ZipFilePath);
          if (File.Exists(TmpFilePath))
          {
            File.Move(TmpFilePath, ZipFilePath);
          }
        }
        else
        {
          if (File.Exists(TmpFilePath))
          {
            File.Delete(TmpFilePath);
          }
        }
      }
      
      void SetProgress(ProgressMessage message)
      {
        if (message.Token.IsCancellationRequested)
        {
          _label.Text = "処理はキャンセルされました。";
          return;
        }
        
        _label.Text = message.Message;
        if (message is AfterMessage)
        {
          _bar.Value++;
        }
      }
      
      class ProgressMessage
      {
        public string Message
        {
          get;
          set;
        }
        
        public CancellationToken Token
        {
          get;
          set;
        }
      }
      
      class BeginMessage : ProgressMessage {}
      class AfterMessage : ProgressMessage {}
    }
    
    public void Execute()
    {
      var app = new Application();
      app.Run(new SampleWindow());
    }
  }
  #endregion
  
  #region ListForEachDiffSamples-01
  /// <summary>
  /// Listをforeachでループする場合と、List.ForEachする場合の速度差をテスト
  /// </summary>
  public class ListForEachDiffSamples01 : IExecutable
  {
    public void Execute()
    {
      Prepare();
      
      //
      // Listをforeachで処理するか、List.ForEachで処理するかで
      // どちらの方が速いのかを計測。
      //
      // ILレベルで見ると
      //   foreachの場合： callが2つ
      //   List.ForEachの場合： callvirtが1つ
      // となる。
      //
      foreach (var elementCount in new []{1000, 3000, 5000, 10000, 50000, 100000, 150000, 500000, 700000, 1000000})
      {
        Console.WriteLine("===== [Count:{0}] =====", elementCount);
        
        var theList = new List<int>(Enumerable.Range(1, elementCount));

        var watch = Stopwatch.StartNew();
        Sum_foreach(theList);
        watch.Stop();
        Console.WriteLine("foreach:      {0}", watch.Elapsed);

        watch = Stopwatch.StartNew();
        Sum_List_ForEach(theList);
        watch.Stop();
        Console.WriteLine("List.ForEach: {0}", watch.Elapsed);
      }
    }
    
    void Prepare()
    {
      int result = 0;
      foreach (var x in new List<int>(Enumerable.Range(1, 1000)))
      {
        result += x;
      }
      
      result = 0;
      new List<int>(Enumerable.Range(1, 1000)).ForEach(x => result += x);
    }
    
    int Sum_foreach(List<int> theList)
    {
      int result = 0;
      foreach (var x in theList)
      {
        result += x;
      }
      return result;
    }
    
    int Sum_List_ForEach(List<int> theList)
    {
      int result = 0;
      theList.ForEach(x => result += x);
      return result;
    }
  }
  #endregion
  
  #region DisposableSamples-01
  /// <summary>
  /// IDisposableのサンプルです。
  /// </summary>
  /// <remarks>
  /// 以下の記事を見て作成したサンプル。
  ///   http://www.codeproject.com/Tips/458846/Using-using-Statements-DisposalAccumulator
  /// </remarks>
  public class DisposableSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // 通常パターン.
      //
      using (var disposable1 = new Disposable1())
      {
        using (var disposable2 = new Disposable2())
        {
          using (var disposable3 = new Disposable3())
          {
            Console.WriteLine("Dispose Start..");
          }
        }
      }
      
      //
      // 通常パターン: DisposableManager利用.
      //
      using (var manager = new DisposableManager())
      {
        var d1 = manager.Add(new Disposable1());
        var d2 = manager.Add(new Disposable2());
        var d3 = manager.Add(new Disposable3());
        
        Console.WriteLine("Dispose Start..");
      }
      
      //
      // 条件が存在し、作成されないオブジェクトが存在する可能性がある場合.
      //
      Disposable1 dispose1 = null;
      Disposable2 dispose2 = null;
      Disposable3 dispose3 = null;
      
      bool isDispose2Create = false;
      try
      {
        dispose1 = new Disposable1();
        
        if (isDispose2Create)
        {
          dispose2 = new Disposable2();
        }
        
        dispose3 = new Disposable3();
      }
      finally
      {
        Console.WriteLine("Dispose Start..");
        DisposeIfNotNull(dispose1);
        DisposeIfNotNull(dispose2);
        DisposeIfNotNull(dispose3);
      }
      
      
      //
      // 条件あり: DisposableManager利用.
      //
      dispose1 = null;
      dispose2 = null;
      dispose3 = null;
      
      using (var manager = new DisposableManager())
      {
        dispose1 = manager.Add(new Disposable1());
        
        if (isDispose2Create)
        {
          dispose2 = manager.Add(new Disposable2());
        }
        
        dispose3 = manager.Add(new Disposable3());
        
        Console.WriteLine("Dispose Start..");
      }
    }
    
    void DisposeIfNotNull(IDisposable disposableObject)
    {
      if (disposableObject == null)
      {
        return;
      }
      
      disposableObject.Dispose();
    }
    
    class DisposableManager : IDisposable
    {
      Stack<IDisposable> _disposables;
      bool               _isDisposed;
      
      public DisposableManager()
      {
        _disposables = new Stack<IDisposable>();
        _isDisposed  = false;
      }
      
      public T Add<T>(T disposableObject) where T : IDisposable
      {
        Defence();
        
        if (disposableObject != null)
        {
          _disposables.Push(disposableObject);
        }
        
        return disposableObject;
      }
      
      public void Dispose()
      {
        _disposables.ToList().ForEach(disposable => disposable.Dispose());
        _disposables.Clear();
        
        _isDisposed = true;
      }
      
      void Defence()
      {
        if (_isDisposed)
        {
          throw new ObjectDisposedException("Cannot access a disposed object.");
        }
      }
    }
    
    class Base : IDisposable
    {
      public void Dispose()
      {
        Console.WriteLine("[{0}] Disposed...", GetType().Name);
      }
    }
    
    class Disposable1 : Base {}
    class Disposable2 : Base {}
    class Disposable3 : Base {}
    
  }
  #endregion
  
  #region MulticoreJITSamples-01
  /// <summary>
  /// マルチコアJITのサンプルです.
  /// </summary>
  public class MulticoreJITSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // .NET 4.5よりマルチコアJITが搭載されている.
      // 文字通り、マルチコア構成の環境にて並列でJITを行う機能である。
      // これにより、アプリケーションの動きに先行して、必要となるメソッドのJITが
      // 行われる可能性が高くなり、結果的にアプリケーションのパフォーマンスが上がるとのこと。
      //
      // マルチコアJITは、ASP.NET 4.5とSilverlight5では
      // 既定で有効となっているが、デスクトップアプリケーションでは
      // デフォルトで有効になっていない。
      //
      // 有効になっていない理由は、この機能を利用するためには
      // プロファイリング処理が必須であり、プロファイルデータを保存
      // することが条件であるため。デスクトップアプリケーションでは
      // フレームワーク側が、プロファイルデータをどこに保存するべきなのかを
      // 判断できないため、手動で実行するようになっている。
      //
      // 参考URL:
      //  http://blogs.msdn.com/b/dotnet/archive/2012/10/18/an-easy-solution-for-improving-app-launch-performance.aspx
      //  http://stackoverflow.com/questions/12965606/why-is-multicore-jit-not-on-by-default-in-net-4-5
      //  http://msdn.microsoft.com/ja-jp/magazine/hh882452.aspx
      //
      // マルチコアJITを有効にするには、System.Runtime.ProfileOptimizationクラスの
      // 以下のstaticメソッドを呼び出すだけである。
      //   ・SetProfileRoot
      //   ・StartProfile
      // 上記メソッドは、アプリケーションのエントリポイントで呼び出す方がよい。
      //
      
      //
      // マルチコアJITを有効にする.
      //  プロファイルデータ格納場所は、アプリ実行フォルダ.
      //  プロファイルデータのファイル名は、App.JIT.Profileとする。
      //
      ProfileOptimization.SetProfileRoot(Environment.CurrentDirectory);
      ProfileOptimization.StartProfile("App.JIT.Profile");
    }
  }
  #endregion
  
  #region MyTest
  public class MyTmpTest : IExecutable
  {
    public void Execute()
    {
    }
  }
  #endregion
}
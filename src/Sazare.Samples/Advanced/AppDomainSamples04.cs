namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region AppDomainSamples-04
  /// <summary>
  /// AppDomainクラスのサンプルです。
  /// </summary>
  [Sample]
  public class AppDomainSamples04 : MarshalByRefObject, Sazare.Common.IExecutable
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
      anotherDomain.ExecuteAssembly(@"resources/AnotherAppDomain.exe");

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
      var asmName = typeof(MarshalByRef).Assembly.FullName;
      var typeName = typeof(MarshalByRef).FullName;

      var obj = (MarshalByRef)anotherDomain.CreateInstanceAndUnwrap(asmName, typeName);

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
}

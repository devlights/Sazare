namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;

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
}

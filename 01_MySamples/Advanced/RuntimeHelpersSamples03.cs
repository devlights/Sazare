namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.CompilerServices;
  using System.Runtime.ConstrainedExecution;

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
}

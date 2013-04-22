namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Reflection;

  #region ReflectionSample-03
  /// <summary>
  /// リフレクションのサンプルです。
  /// </summary>
  /// <remarks>
  /// リフレクション実行時のパフォーマンスをアップさせる方法について記述しています。
  /// </remarks>
  [Sample]
  public class ReflectionSample03 : Sazare.Common.IExecutable
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
      StringToString s2s = (StringToString)Delegate.CreateDelegate(typeof(StringToString), mi);
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
}

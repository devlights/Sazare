namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  using Sazare.Common;

  /// <summary>
  /// Debugクラスについてのサンプルです。
  /// </summary>
  [Sample]
  public class DebugSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // Debugクラスは文字通りデバッグ情報を出力するために利用されるクラスである.
      // このクラスの全てのメソッドは、Conditional("DEBUG")属性が付与されている.
      // なので、DEBUG定数を指定していない場合、コンパイラの段階で呼び出しが削除される.
      //
      // Visual Studioではデフォルトで
      //   デバッグビルドの場合は、DEBUGとTRACE
      //   リリースビルドの場合は、TRACE
      // が付与されて、コンパイルされる.
      //
      // Traceクラスと同様にDebugクラスもListenerの考えを持っていて
      // リスナーを追加することでいろいろな出力が行える様になっている.
      //
      // 注意点として、ListenerはTraceとDebugで共有されており
      // どちらかに追加すると両方で有効となる.
      //
      bool output = true;

      Debug.WriteLine("デバッグメッセージ");
      Debug.WriteLineIf(output, "条件付きデバッグメッセージ");
    }
  }
}

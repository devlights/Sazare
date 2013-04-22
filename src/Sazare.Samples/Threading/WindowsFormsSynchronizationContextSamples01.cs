namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using WinFormsApplication = System.Windows.Forms.Application;
  using WinFormsForm = System.Windows.Forms.Form;

  #region WindowsFormsSynchronizationContextSamples-01
  /// <summary>
  /// WindowsFormsSynchronizationContextクラスについてのサンプルです。
  /// </summary>
  /// <!-- <remarks>
  /// WindowsFormsSynchronizationContextは、SynchronizationContextクラスの派生クラスです。
  /// デフォルトでは、Windows Formsにて、最初のフォームが作成された際に自動的に設定されます。
  /// (AutoInstall静的プロパティにて、動作を変更可能。）
  /// </remakrs>
  [Sample]
  public class WindowsFormsSynchronizationContextSamples01 : Sazare.Common.IExecutable
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
}

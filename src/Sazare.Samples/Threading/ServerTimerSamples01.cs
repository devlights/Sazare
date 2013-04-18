namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  //
  // Alias設定.
  //
  using WinFormsApplication = System.Windows.Forms.Application;
  using WinFormsDockStyle = System.Windows.Forms.DockStyle;
  using WinFormsForm = System.Windows.Forms.Form;
  using WinFormsFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
  using WinFormsListBox = System.Windows.Forms.ListBox;

  #region System.Timers.Timerのサンプル
  /// <summary>
  /// System.Timers.Timerクラスについてのサンプルです。
  /// </summary>
  [Sample]
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
      _timer.Interval = 1000;
      _timer.AutoReset = true;

      //
      // タイマを開始.
      //
      _timer.Enabled = true;
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
}

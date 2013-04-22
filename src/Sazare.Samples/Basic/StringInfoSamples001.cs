namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Drawing;
  using System.Globalization;
  using System.Linq;

  //
  // Alias設定.
  //
  using GDISize = System.Drawing.Size;
  using WinFormsApplication = System.Windows.Forms.Application;
  using WinFormsButton = System.Windows.Forms.Button;
  using WinFormsControl = System.Windows.Forms.Control;
  using WinFormsDockStyle = System.Windows.Forms.DockStyle;
  using WinFormsFlowDirection = System.Windows.Forms.FlowDirection;
  using WinFormsFlowLayoutPanel = System.Windows.Forms.FlowLayoutPanel;
  using WinFormsForm = System.Windows.Forms.Form;
  using WinFormsFormStartPosition = System.Windows.Forms.FormStartPosition;
  using WinFormsMessageBox = System.Windows.Forms.MessageBox;
  using WinFormsTextBox = System.Windows.Forms.TextBox;

  using Sazare.Common;
  
  #region StringInfoSamples-001
  /// <summary>
  /// StringInfoについてのサンプルです。
  /// </summary>
  /// <remarks>
  /// サロゲートペアについて記述しています。
  /// </remarks>
  [Sample]
  public class StringInfoSamples001 : Sazare.Common.IExecutable
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

        WinFormsButton b = new WinFormsButton { Text = "Exec" };
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

        WinFormsFlowLayoutPanel contentPane = new WinFormsFlowLayoutPanel { FlowDirection = WinFormsFlowDirection.TopDown, WrapContents = true };
        contentPane.Controls.AddRange(new WinFormsControl[] { t, b });
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
}

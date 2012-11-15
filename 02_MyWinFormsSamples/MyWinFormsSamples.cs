// vim:set ts=2 sw=2 et ws is nowrap ft=cs:

namespace Gsf.Samples.WinForms
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.Data;
  using System.Data.Common;
  using System.Data.Linq;
  using System.Drawing;
  using System.Diagnostics;
  using System.Globalization;
  using System.Linq;
  using System.Net;
  using System.Net.NetworkInformation;
  using System.Reflection;
  using System.Reflection.Emit;
  using System.Runtime.InteropServices;
  using System.Runtime.Remoting;
  using System.Runtime.Remoting.Messaging;
  using System.Security;
  using System.Text;
  using System.Threading;
  using System.Windows.Forms;
  using System.Xml;
  using System.Xml.Linq;

  #region 共通インターフェース定義
  interface IExecutable
  {
      void Execute();
  }
  #endregion

  #region ダミークラス
  class Dummy : Form, IExecutable
  {
      
      public Dummy()
      {
          Text = "THIS IS DUMMY FORM.";
      }
      
      public void Execute()
      {
          Application.EnableVisualStyles();
          Application.Run(new Dummy());
      }
  }
  #endregion
  
  #region 共通拡張クラス
  public static class StringExtensions
  {

      public static int ToInt(this string self)
      {
          return int.Parse(self);
      }
  }
  #endregion
  
  #region 各サンプルの起動を担当するクラス.
  public class SampleLauncher
  {

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
  
  #region "進行状況ダイアログのサンプル"
  class ProgressDialogSample : IExecutable
  {
      private class Dialog : Form
      {
          public Dialog()
          {
              InitializeComponent();
          }

          protected void InitializeComponent()
          {
              SuspendLayout();

              Text = "処理中・・・・・";
              Size = new Size(100, 100);
              TopMost = true;
              ControlBox = false;
              MaximizeBox = false;
              MinimizeBox = false;
              ShowIcon = false;
              ShowInTaskbar = false;

              ResumeLayout();
          }
      }

      private class ThreadInfo
      {
          public Form Owner
          {
              get;
              set;
          }

          public Form Dialog
          {
              get;
              set;
          }

          public ManualResetEvent WaitHandle
          {
              get;
              set;
          }
      }

      private class ParentForm : Form
      {
          public ParentForm()
          {
              InitializeComponent();
          }

          protected void InitializeComponent()
          {
              SuspendLayout();

              Text = "親フォーム";
              Size = new Size(500, 500);

              Button btnShowDialog = new Button();
              btnShowDialog.Text = "ダイアログ表示";
              btnShowDialog.Width = 100;
              btnShowDialog.Dock = DockStyle.Fill;
              btnShowDialog.Click += (s, e) =>
              {
                  Enabled = false;

                  ThreadInfo info = new ThreadInfo
                  {
                      Owner = this,
                      WaitHandle = new ManualResetEvent(false)
                  };
                  
                  Thread thread = new Thread((val) =>
                  {
                      ThreadInfo tinfo = val as ThreadInfo;
                      Form owner = tinfo.Owner;

                      Dialog dialog = new Dialog();

                      dialog.StartPosition = FormStartPosition.Manual;
                      dialog.Left = owner.Left + (owner.Width - dialog.Width) / 2;
                      dialog.Top = owner.Top + (owner.Height - dialog.Height) / 2;

                      dialog.Activated += (s2, e2) =>
                      {
                          tinfo.WaitHandle.Set();
                      };

                      tinfo.Dialog = dialog;

                      dialog.ShowDialog();
                  });

                  thread.IsBackground = true;
                  thread.Start(info);

                  info.WaitHandle.WaitOne();

                  for (int i = 0; i < 10; i++)
                  {
                      string caption = string.Format("{0}%完了", ((i + 1) * 10));
                      info.Dialog.Invoke(new MethodInvoker(() =>
                      {
                          info.Dialog.Text = caption;
                      }));

                      Thread.Sleep(1000);
                  }

                  info.Dialog.Invoke(new MethodInvoker(info.Dialog.Close));

                  Enabled = true;
                  Activate();

              };

              Controls.Add(btnShowDialog);

              ResumeLayout();
          }
      }

      [STAThread]
      public void Execute() {
      	// アプリケーション起動
          Application.Run(new ParentForm());
      }
  }
  #endregion
  
  #region MDISamples-01
  public class MDISamples01 : IExecutable
  {

      class ParentForm : Form
      {
          public ParentForm()
              : base()
              {
              InitializeControl();
              InitializeEvent();
          }

          protected void InitializeControl()
          {
              SuspendLayout();

              Text = "Parent Form.";
              IsMdiContainer = true;
              Size = new Size(800, 600);
              StartPosition = FormStartPosition.CenterScreen;

              ResumeLayout();
          }

          protected void InitializeEvent()
          {
              Load += (s, e) =>
              {
                  if (MdiChildren == null)
                  {
                      return;
                  }

                  foreach (Form child in MdiChildren)
                  {
                      child.Show();
                  }

                  LayoutMdi(MdiLayout.TileHorizontal);
              };
          }
      }

      class ChildForm : Form
      {
          public ChildForm()
              : base()
              {
              InitializeControl();
          }

          protected void InitializeControl()
          {
              SuspendLayout();

              Size = new Size(200, 100);

              ResumeLayout();
          }
      }

      [STAThread]
      public void Execute()
      {
          ParentForm parent = new ParentForm();

          for (int i = 0; i < 10; i++)
          {
              ChildForm child = new ChildForm
              {
                  Text = string.Format("Child-{0}", i.ToString()),
                  MdiParent = parent
              };
          }

          Application.Run(parent);
      }
  }
  #endregion
  
  #region SetWindowsPosSamples
  public class SetWindowPosSamples : IExecutable
  {

      static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

      const uint SWP_NOSIZE = 0x0001;
      const uint SWP_NOMOVE = 0x0002;

      const uint TOPMOST_FLAGS = (SWP_NOSIZE | SWP_NOMOVE);

      [DllImport("user32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

      public class SampleForm : Form
      {
          public SampleForm()
          {
              InitializeComponent();
              InitializeEvents();
          }

          void InitializeComponent()
          {
              SuspendLayout();

              Size = new Size(300, 100);
              Text = "SetWindowPos Sample";

              ResumeLayout();
          }

          void InitializeEvents()
          {
              Load += (s, e) => 
                  {
                      //
                      // SetWindowPos関数を用いることで、常に最前面にアプリケーションが表示される。
                      //
                      SetWindowPos(Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
                  };
          }
      }

      public void Execute()
      {
          Application.EnableVisualStyles();
          Application.Run(new SampleForm());
      }
  }
  #endregion
}
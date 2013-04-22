namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.IO.Compression;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Windows;
  using System.Windows.Controls;

  using Sazare.Common;
  
  #region ProgressSamples-03
  /// <summary>
  /// System.Progress<T>のサンプルです。
  /// </summary>
  /// <remarks>
  /// このクラスは、.NET Framework 4.5から追加された型です。
  /// </remarks>
  [Sample]
  public class ProgressSamples03 : Sazare.Common.IExecutable
  {
    class SampleWindow : Window
    {
      TextBlock _label;
      ProgressBar _bar;
      Button _btn;

      public SampleWindow()
      {
        InitializeControl();
        InitializeEvent();
      }

      void InitializeControl()
      {
        Width = 400;
        Height = 100;

        _label = new TextBlock
        {
          Text = string.Empty
        };

        _bar = new ProgressBar
        {
          Height = 20
          ,
          Minimum = 0
        };

        _btn = new Button
        {
          Content = "Cancel"
          ,
          Margin = new Thickness(300, 0, 0, 0)
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
          var progress = new Progress<ProgressMessage>(SetProgress);

          _btn.Tag = tokenSource;
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

      class BeginMessage : ProgressMessage { }
      class AfterMessage : ProgressMessage { }
    }

    public void Execute()
    {
      var app = new Application();
      app.Run(new SampleWindow());
    }
  }
  #endregion
}

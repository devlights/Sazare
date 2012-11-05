// vim:set ts=4 sw=4 et ws is nowrap ft=cs:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;

//
// WPFをXAML付きで勉強する際に利用するサンプル.
// コードのみを利用してWPFを勉強する際は、MySamples.csを利用する.
//
namespace Gsf.Samples.Wpf
{
  #region CommonInterfaces
  internal interface IExecutable
  {
    void Execute();
  }
  
  internal interface IWpfSampleInitialize
  {
    void Setup(Window window);
  }
  
  internal interface IWpfAppInitialize
  {
    Application GetApplication();
  }
  #endregion
  
  #region DummyClass
  internal class Dummy : IExecutable
  {
    public void Execute()
    {
      MessageBox.Show("This is Dummy Class.");
    }
  }
  #endregion
  
  #region LauncherClass
  public class SampleLauncher
  {
    //
    // WPFの場合、STAThreadをつけていないとウィンドウが起動できない.
    // STAThreadを付与していない場合、以下のエラーが発生する.
    //
    // 「'指定されたバインディング制約に一致する型 'System.Windows.Window' のコンストラクターの呼び出しで例外がスローされました。」
    //
    [STAThread]
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

      Application app = null;
      try
      {
        Assembly     assembly = Assembly.GetExecutingAssembly();
        ObjectHandle handle   = Activator.CreateInstance(assembly.FullName, className);
        if (handle != null)
        {
          object clazz = handle.Unwrap();

          if (clazz != null)
          {
            if (clazz is IWpfSampleInitialize)
            {
              IWpfAppInitialize    appInitializer    = clazz as IWpfAppInitialize;
              IWpfSampleInitialize sampleInitializer = clazz as IWpfSampleInitialize;
              
              app = (appInitializer == null)
                      ? new Application()
                      : appInitializer.GetApplication();
              
              Window window = ReadXaml(className.Split('.').Last());
              sampleInitializer.Setup(window);
              
              window.Show();
              app.Run(window);
            }
            else if (clazz is IExecutable)
            {
              (clazz as IExecutable).Execute();
            }
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      finally
      {
        if (app != null)
        {
          app.Shutdown(0);
        }
      }
    }
    
    static Window ReadXaml(string xamlName)
    {
      Window window = null;
      
      string path = Path.Combine("Xaml", string.Format("{0}.xaml", xamlName));
      using (FileStream fs = File.OpenRead(path))
      {
        window = XamlReader.Load(fs) as Window;
      }
      
      return window;
    }
  }
  #endregion
  
  #region Interface Test
  internal class InterfaceTest : IWpfSampleInitialize, IWpfAppInitialize
  {
    class MyApp : Application
    {
      protected override void OnStartup(StartupEventArgs e)
      {
        base.OnStartup(e);
        MessageBox.Show("IWpfAppInitialize.GetApplication");
      }
    }
    
    public void Setup(Window window)
    {
      window.Loaded += (s, e) =>
      {
        MessageBox.Show("IWpfSampleInitialize.Setup");
      };
    }
    
    public Application GetApplication()
    {
      return new MyApp();
    }
  }
  #endregion
  
  #region HelloWpf
  internal class HelloWpf : IWpfSampleInitialize
  {
    public void Setup(Window window)
    {
    }
  }
  #endregion
  
  #region ShowMessageBox
  internal class ShowMessageBox : IWpfSampleInitialize
  {
    public void Setup(Window window)
    {
      Button btn = window.FindName("btnMessage") as Button;
      btn.Click += btnMessage_Click;
    }
    
    void btnMessage_Click(object sender, RoutedEventArgs e)
    {
      MessageBox.Show("Hello WPF!!");
    }
  }
  #endregion
  
  #region ShowTextBox
  internal class ShowTextBox : IWpfSampleInitialize
  {
    public void Setup(Window window)
    {
      TextBox txt = window.FindName("txt") as TextBox;
      txt.Text = "Hello WPF!!";
    }
  }
  #endregion
  
  #region ShowStackPanel
  internal class ShowStackPanel : IWpfSampleInitialize
  {
    public void Setup(Window window)
    {
    }
  }
  #endregion
  
  #region ShowDockPanel
  internal class ShowDockPanel : IWpfSampleInitialize
  {
    public void Setup(Window window)
    {
      MessageBox.Show("WPF Study");
    }
  }
  #endregion
}
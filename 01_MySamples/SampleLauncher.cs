
namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using System.Runtime.Remoting;
  
  #region サンプルの起動を担当するクラス
  /// <summary>
  /// サンプルの起動を担当するクラスです。
  /// </summary>
  /// <remarks>
  /// 本クラスがエントリポイントとなります。
  /// </remarks>
  public class SampleLauncher
  {
    /// <summary>
    /// エントリポイントメソッド
    /// </summary>
    /// <param name="args">起動時引数</param>
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

      //
      // 指定されたクラスを起動.
      //
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
  
}
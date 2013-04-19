namespace Sazare.Tools.Cui
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;

  using Sazare.Samples;
  
  class Program
  {
    private readonly static Type   DummyType;
    private          static string ClassName;

    static Program()
    {
      DummyType = typeof(Dummy);
      ClassName = string.Empty;
    }

    static void Main()
    {
      try
      {
        var handle = Activator.CreateInstance(GetAssembly().FullName, GetFqdnName());
        if (handle != null)
        {
          var clazz = handle.Unwrap();
          if (clazz != null)
          {
            Execute(clazz as IExecutable);
          }
        }
      }
      catch (TypeLoadException)
      {
        Console.WriteLine("指定されたサンプルが見つかりません...[{0}]", ClassName);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      finally
      {
        Console.WriteLine("\n\nPress any key to exit...");
        Console.ReadKey();
      }
    }

    internal static Assembly GetAssembly()
    {
      return DummyType.Assembly;
    }

    internal static string GetClassName()
    {
      return Environment.GetCommandLineArgs().Skip(1).FirstOrDefault();
    }

    internal static string WithNamespace(string className)
    {
      var parts = className.Split(new char[] { '.' });
      if (parts.Length > 1)
      {
        return className;
      }

      return string.Format("{0}.{1}", DummyType.Namespace, className);
    }

    internal static string GetFqdnName()
    {
      var className = GetClassName();
      if (string.IsNullOrWhiteSpace(className))
      {
        Console.Write("ENTER CLASS NAME: ");
        return WithNamespace((ClassName = Console.ReadLine()));
      }

      return WithNamespace((ClassName = className));
    }

    internal static void Execute(IExecutable executable)
    {
      Console.WriteLine("\n");
      Console.WriteLine("================== START ==================");
      executable.Execute();
      Console.WriteLine("==================  END  ==================");
    }
  }
}

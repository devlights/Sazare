namespace Sazare.Tools.Cui
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;

  using Sazare.Common;
  using Sazare.Samples;

  class Program
  {
    const           string EXIT_PHASE = "exit";
    static readonly Type   DummyType;
    static          string ClassName;

    static Program()
    {
      DummyType = typeof(Dummy);
      ClassName = string.Empty;
    }

    static void Main()
    {
      try
      {
        Output.SetOutputManager(new CuiOutputManager());

        for (;;)
        {
          try
          {
            Console.Write("\nENTER CLASS NAME: ");

            var userInput = Console.ReadLine();
            if (userInput.ToLower() == EXIT_PHASE)
            {
              break;
            }

            var handle = Activator.CreateInstance(GetAssembly().FullName, GetFqdnName(userInput));
            if (handle != null)
            {
              var clazz = handle.Unwrap();
              if (clazz != null)
              {
                var executor = new CuiAppProcessExecutor();
                executor.Execute(clazz as IExecutable);
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
        }
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

    internal static string GetInitialClassName()
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

    internal static string GetFqdnName(string value)
    {
      var className = GetInitialClassName();
      if (!string.IsNullOrWhiteSpace(className))
      {
        return WithNamespace((ClassName = className));
      }

      return WithNamespace((ClassName = value));
    }
  }
}

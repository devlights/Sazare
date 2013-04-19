namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Runtime.CompilerServices;

  public class CallerInformationSamples01 : IExecutable
  {
    public void Execute()
    {
      var manager = new CallerInfoManager();

      Console.WriteLine(manager.Snap());
      Console.WriteLine(MethodA(manager));
      Console.WriteLine(MethodB(manager));
    }

    CallerInfoManager MethodA(CallerInfoManager manager)
    {
      return manager.Snap();
    }

    CallerInfoManager MethodB(CallerInfoManager manager)
    {
      return manager.Snap();
    }
  }

  class CallerInfoManager
  {
    public string FilePath   { get; private set; }
    public int    LineNumber { get; private set; }
    public string MemberName { get; private set; }

    public CallerInfoManager Snap([CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
      FilePath   = file;
      LineNumber = line;
      MemberName = member;

      return this;
    }

    public override string ToString()
    {
      return string.Format("File:{0}\nLine:{1}\nMember:{2}", Path.GetFileName(FilePath), LineNumber, MemberName);
    }
  }
}

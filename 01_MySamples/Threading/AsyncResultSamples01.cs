namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Remoting.Messaging;
  using System.Threading;

  #region AsyncResultSamples-01
  /// <summary>
  /// ”ñ“¯Šúˆ— (IAsyncResult) ‚ÌƒTƒ“ƒvƒ‹‚P‚Å‚·B
  /// </summary>
  public class AsyncResultSamples01 : IExecutable
  {

    AutoResetEvent _are = new AutoResetEvent(false);

    public void Execute()
    {
      Func<DateTime, string> func = CallerMethod;

      IAsyncResult result = func.BeginInvoke(DateTime.Now, CallbackMethod, _are);
      _are.WaitOne();
    }

    string CallerMethod(DateTime d)
    {
      return d.ToString("yyyy/MM/dd HH:mm:ss");
    }

    void CallbackMethod(IAsyncResult ar)
    {
      AsyncResult result = ar as AsyncResult;
      Func<DateTime, string> caller = result.AsyncDelegate as Func<DateTime, string>;
      EventWaitHandle handle = result.AsyncState as EventWaitHandle;

      if (!result.EndInvokeCalled)
      {
        Console.WriteLine(caller.EndInvoke(result));
        handle.Set();
      }
    }
  }
  #endregion
}

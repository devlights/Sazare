namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Remoting.Messaging;
  using System.Threading;

  using Sazare.Common;
  
  #region AsyncResultSamples-01
  /// <summary>
  /// 非同期処理 (IAsyncResult) のサンプル１です。
  /// </summary>
  [Sample]
  public class AsyncResultSamples01 : Sazare.Common.IExecutable
  {

    AutoResetEvent _are = new AutoResetEvent(false);

    public void Execute()
    {
      Func<DateTime, string> func = CallerMethod;

      IAsyncResult result = func.BeginInvoke(DateTime.Now, CallbackMethod, _are);
      _are.WaitOne();
      func.EndInvoke(result);
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
        Output.WriteLine(caller.EndInvoke(result));
        handle.Set();
      }
    }
  }
  #endregion
}

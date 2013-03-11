namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;

  #region EventSettingSamples-01
  /// <summary>
  /// 手動でイベントを制御する方法に関してのサンプルです。(EventHandlerList)
  /// </summary>
  public class EventSettingSamples01 : IExecutable
  {
    class Sample
    {
      object _eventTarget = new object();

      public Sample()
      {
        Events = new EventHandlerList();
      }

      public EventHandlerList Events
      {
        get;
        set;
      }

      public event EventHandler TestEvent;

      public void FireEvents()
      {
        if (TestEvent != null)
        {
          TestEvent(this, EventArgs.Empty);
        }
      }
    }

    public void Execute()
    {
      Sample obj = new Sample();

      EventHandler handler = (s, e) =>
      {
        Console.WriteLine("event raised.");
      };

      obj.TestEvent += handler;
      obj.FireEvents();
      obj.TestEvent -= handler;
      obj.FireEvents();

    }
  }
  #endregion
}

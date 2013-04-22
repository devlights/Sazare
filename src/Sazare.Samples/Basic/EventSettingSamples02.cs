namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;

  using Sazare.Common;
  
  #region EventSettingSamples-02
  /// <summary>
  /// 手動でイベントを制御する方法に関してのサンプルです。(EventHandlerList)
  /// </summary>
  [Sample]
  public class EventSettingSamples02 : Sazare.Common.IExecutable
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

      public event EventHandler TestEvent
      {
        add
        {
          Output.WriteLine("add handler.");
          Events.AddHandler(_eventTarget, value);
        }
        remove
        {
          Output.WriteLine("remove handler.");
          Events.RemoveHandler(_eventTarget, value);
        }
      }

      public void FireEvents()
      {
        EventHandler handler = Events[_eventTarget] as EventHandler;

        if (handler != null)
        {
          handler(this, EventArgs.Empty);
        }
      }
    }

    public void Execute()
    {
      Sample obj = new Sample();

      EventHandler handler = (s, e) =>
      {
        Output.WriteLine("event raised.");
      };

      obj.TestEvent += handler;
      obj.FireEvents();
      obj.TestEvent -= handler;
      obj.FireEvents();

    }
  }
  #endregion
}

using System;
using System.ComponentModel;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{
#pragma warning disable 414

    #region EventSettingSamples-01

    /// <summary>
    ///     手動でイベントを制御する方法に関してのサンプルです。(EventHandlerList)
    /// </summary>
    [Sample]
    public class EventSettingSamples01 : IExecutable
    {
        public void Execute()
        {
            var obj = new Sample();

            EventHandler handler = (s, e) => { Output.WriteLine("event raised."); };

            obj.TestEvent += handler;
            obj.FireEvents();
            obj.TestEvent -= handler;
            obj.FireEvents();
        }

        private class Sample
        {
            private object _eventTarget = new object();

            public Sample()
            {
                Events = new EventHandlerList();
            }

            public EventHandlerList Events { get; set; }

            public event EventHandler TestEvent;

            public void FireEvents()
            {
                if (TestEvent != null)
                {
                    TestEvent(this, EventArgs.Empty);
                }
            }
        }
    }

    #endregion
}
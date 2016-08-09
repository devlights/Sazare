using System;
using System.Dynamic;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{

    #region ExpandoObjectクラスのサンプル-02

    /// <summary>
    ///     ExpandoObjectクラスについてのサンプルです。
    /// </summary>
    /// <remarks>
    ///     .NET 4.0から追加されたクラスです。
    /// </remarks>
    [Sample]
    public class ExpandoObjectSamples02 : IExecutable
    {
        public void Execute()
        {
            ///////////////////////////////////////////////
            //
            // ExpandoObjectにイベントを追加.
            //
            dynamic obj = new ExpandoObject();

            //
            // イベント定義
            //   ExpandoObjectに対してイベントを定義するには
            //   まず、イベントフィールドを定義して、それをnullで初期化
            //   する必要がある。
            //
            obj.MyEvent = null;

            //
            // イベントハンドラを設定.
            //
            obj.MyEvent += new EventHandler((sender, e) => { Output.WriteLine("sender={0}", sender); });

            // イベント着火.
            obj.MyEvent(obj, EventArgs.Empty);
        }
    }

    #endregion
}
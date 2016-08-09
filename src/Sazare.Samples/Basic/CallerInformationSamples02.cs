﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sazare.Common;

// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{
    public class CallerInformationSamples02 : IExecutable
    {
        public void Execute()
        {
            //
            // 普通にINotifyPropertyChangedの実装クラスを使って値を変更して確認.
            //
            var notifyObj = new NotityPropertyChangedImpl();

            notifyObj.PropertyChanged += (s, e) => { Output.WriteLine("[{0}] changed to [{1}]", e.PropertyName, (s as dynamic).Name); };

            notifyObj.Name = "hello world";
            notifyObj.Name = "goobye world";
        }

        // INotifyPropertyChangedインターフェースの実装
        private class NotityPropertyChangedImpl : INotifyPropertyChanged
        {
            private string _name;

            public string Name
            {
                get { return _name; }
                set
                {
                    if (_name == value)
                        return;

                    _name = value;

                    //
                    // メソッドに[CallerMemberName]を付与しているのでプロパティ名を指定する必要なし.
                    //
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            #region OnPropertyChanged

            public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            #endregion
        }
    }
}
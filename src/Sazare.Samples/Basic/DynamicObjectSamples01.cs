﻿namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Dynamic;
  using System.Linq;
  
  using Sazare.Common;

  /// <summary>
  /// System.Dynamic.DynamicObjectについてのサンプルです。
  /// </summary>
  [Sample]
  class DynamicObjectSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // DynamicObjectクラスのサブクラスを作成する
      // ことで、簡単にdynamic対応のクラスを作成することが
      // できる。実際、DynamicObjectの中には多くのメソッドが
      // 定義されているが、動的な値を格納するようにするだけであれば
      //   TryGetMember
      //   TrySetMember
      // メソッドをオーバーライドすれば事足りる。
      //
      dynamic obj = new MyDynamicObject();

      obj.age  = 100;
      obj.name = "name1";

      Output.WriteLine(obj.GetType().Name);
      Output.WriteLine(obj.AGE);
      Output.WriteLine(obj.NAME);
    }

    class MyDynamicObject : DynamicObject
    {
      readonly Dictionary<string, object> _memberMappings;

      public MyDynamicObject()
      {
        _memberMappings = new Dictionary<string, object>();
      }

      public override bool TryGetMember(GetMemberBinder binder, out object result)
      {
        result = null;

        var name = binder.Name.ToUpper();
        if (_memberMappings.ContainsKey(name))
        {
          result = _memberMappings[name];
          return true;
        }

        return false;
      }

      public override bool TrySetMember(SetMemberBinder binder, object value)
      {
        var name = binder.Name.ToUpper();
        if (_memberMappings.ContainsKey(name))
        {
          _memberMappings[name] = value;
        }
        else
        {
          _memberMappings.Add(name, value);
        }

        return true;
      }
    }
  }
}
namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// サンプルを表す属性です.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
  public sealed class SampleAttribute : Attribute
  {
  }
}

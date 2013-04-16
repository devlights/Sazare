namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  #region DefaultValuesSamples-01
  /// <summary>
  /// 各型のデフォルト値についてのサンプルです。
  /// </summary>
  [Sample]
  public class DefaultValuesSamples01 : IExecutable
  {
    class SampleClass { }
    struct SampleStruct { }

    public void Execute()
    {
      Console.WriteLine("byte   のデフォルト:    {0}", default(byte));
      Console.WriteLine("char   のデフォルト:    {0}", default(char) == 0x00);
      Console.WriteLine("short  のデフォルト:    {0}", default(short));
      Console.WriteLine("ushort のデフォルト:    {0}", default(ushort));
      Console.WriteLine("int  のデフォルト:    {0}", default(int));
      Console.WriteLine("uint   のデフォルト:    {0}", default(uint));
      Console.WriteLine("long   のデフォルト:    {0}", default(long));
      Console.WriteLine("ulong  のデフォルト:    {0}", default(ulong));
      Console.WriteLine("float  のデフォルト:    {0}", default(float));
      Console.WriteLine("double のデフォルト:    {0}", default(double));
      Console.WriteLine("decimalのデフォルト:    {0}", default(decimal));
      Console.WriteLine("string のデフォルト:    NULL = {0}", default(string) == null);
      Console.WriteLine("byte[] のデフォルト:    NULL = {0}", default(byte[]) == null);
      Console.WriteLine("List<string>のデフォルト: NULL = {0}", default(List<string>) == null);
      Console.WriteLine("自前クラスのデフォルト:   NULL = {0}", default(SampleClass) == null);
      Console.WriteLine("自前構造体のデフォルト:   {0}", default(SampleStruct));
    }
  }
  #endregion
}

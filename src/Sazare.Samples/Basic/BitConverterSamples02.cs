// ReSharper disable once CheckNamespace
namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sazare.Common;
  using Sazare.Samples.BitConverterSamples02_Extensions;

  #region BitConverterSamples-02

  public class BitConverterSamples02 : IExecutable
  {
    public void Execute()
    {
      var hexConverter = new Func<byte, string>(i => i.ToString("x"));

      decimal.GetBits(decimal.MaxValue)
        .SelectMany(BitConverter.GetBytes)
        .JoinAndPrint(Output.WriteLine, hexConverter);

      Output.WriteLine();

      BitConverter.GetBytes(DateTime.MaxValue.ToBinary())
        .JoinAndPrint(Output.WriteLine, hexConverter);
    }
  }

  namespace BitConverterSamples02_Extensions
  {
    public static class ListExtensions
    {
      public static void JoinAndPrint<T>(this IEnumerable<T> self, Action<object> action, Func<T, string> hexConverter, string prefix = "0x")
      {
        var enumerable = self as T[] ?? self.ToArray();

        var queryHex = enumerable.Select(hexConverter);
        action(string.Join(" ", enumerable));
        action(string.Join(" ", queryHex));
      }
    }
  }

  #endregion

}

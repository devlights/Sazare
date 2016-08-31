// ReSharper disable CheckNamespace

using System;
using Sazare.Common;

namespace Sazare.Samples
{
    #region LinqSamples-18 AND LinqSamples-19 AND 拡張メソッド解決

    /// <summary>
    /// <see cref="Person"/>の拡張メソッドが定義されています。
    /// </summary>
    public static class PersonExtension
    {
        /// <summary>
        /// 指定された条件に合致する<see cref="Person"/>を取得します。
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <param name="predicate">抽出条件</param>
        /// <returns>絞込結果</returns>
        public static Persons Where(this Persons self, Func<Person, bool> predicate)
        {
            var result = new Persons();

            Output.WriteLine("========= WHERE ========");
            foreach (var aPerson in self)
            {
                if (predicate(aPerson))
                {
                    result.Add(aPerson);
                }
            }

            return result;
        }
    }

    #endregion
}
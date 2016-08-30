// ReSharper disable CheckNamespace

using System.Collections.Generic;
using System.Linq;
using Sazare.Common;

namespace Sazare.Samples
{

    #region LinqSamples-41

    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples41 : IExecutable
    {
        public void Execute()
        {
            //
            // Emptyメソッドは、文字通り空のシーケンスを作成するメソッドである。
            // Unionする際や、Aggregateする際の中間値として利用されることが多い。
            //
            Output.WriteLine("COUNT = {0}", Enumerable.Empty<string>().Count());

            //
            // 指定されたシーケンスから合計値が100を超えているシーケンスのみを抽出.
            // Aggregateのseed値として、空のシーケンスを渡すためにEnumerable.Emptyを
            // 使用している。
            //
            var sequences = new List<IEnumerable<int>>
            {
                Enumerable.Range(1, 10),
                Enumerable.Range(30, 3),
                Enumerable.Range(50, 2),
                Enumerable.Range(200, 1)
            };

            var query =
                sequences.Aggregate(
                    Enumerable.Empty<int>(),
                    (current, next) => next.Sum() > 100 ? current.Union(next) : current
                );

            foreach (var item in query)
            {
                Output.WriteLine(item);
            }
        }
    }

    #endregion
}
// ReSharper disable CheckNamespace

using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using Sazare.Common;

namespace Sazare.Samples
{

    #region WcfSamples-03

    /// <summary>
    ///     WCFのサンプルです。
    /// </summary>
    /// <remarks>
    ///     引数と戻り値にカスタムオブジェクトを指定するサービスメソッドを定義しています。
    /// </remarks>
    [Sample]
    public class WcfSamples03 : IExecutable
    {
        public void Execute()
        {
            using (var host = CreateService())
            {
                //
                // サービスを開始.
                //
                host.Open();

                //
                // クライアント側を構築.
                //
                using (var factory = CreateChannelFactory())
                {
                    //
                    // クライアントプロキシオブジェクトを取得.
                    //
                    var proxy = factory.CreateChannel();

                    //
                    // サービスメソッドを呼び出し、結果を取得.
                    //
                    var data = new Data {X = 300, Y = 200};
                    Output.WriteLine("サービスの呼び出し前= {0}", data);
                    Output.WriteLine("サービスの呼び出し結果= {0}", proxy.Execute(data));
                }
            }
        }

        private ServiceHost CreateService()
        {
            //
            // ホストを初期化
            //
            var host = new ServiceHost(typeof(ReturnCustomDataService), new Uri(SERVICE_URL));

            //
            // エンドポイントを追加.
            //
            host.AddServiceEndpoint(typeof(IReturnCustomDataService), BINDING, ENDPOINT_ADDR);

            return host;
        }

        private ChannelFactory<IReturnCustomDataService> CreateChannelFactory()
        {
            //
            // クライアント側からサービスに接続するためにChannelFactoryを構築.
            //
            var factory =
                new ChannelFactory<IReturnCustomDataService>(BINDING, new EndpointAddress(SERVICE_URL));

            return factory;
        }

        /// <summary>
        ///     サービスインターフェース
        /// </summary>
        [ServiceContract]
        public interface IReturnCustomDataService
        {
            [OperationContract]
            ReturnData Execute(Data data);
        }

        /// <summary>
        ///     サービス実装クラス
        /// </summary>
        public class ReturnCustomDataService : IReturnCustomDataService
        {
            public ReturnData Execute(Data data)
            {
                return new ReturnData {X = data.Y, Y = data.X};
            }
        }

        /// <summary>
        ///     サービスメソッドの引数クラス
        /// </summary>
        [DataContract]
        public class Data
        {
            [DataMember]
            public int X { get; set; }

            [DataMember]
            public int Y { get; set; }

            public override string ToString()
            {
                return string.Format("X={0}, Y={1}", X, Y);
            }
        }

        /// <summary>
        ///     サービスメソッドの戻り値クラス
        /// </summary>
        [DataContract]
        public class ReturnData
        {
            [DataMember]
            public int X { get; set; }

            [DataMember]
            public int Y { get; set; }

            public override string ToString()
            {
                return string.Format("X={0}, Y={1}", X, Y);
            }
        }

        #region Constants

        /// <summary>
        ///     サービスのURL
        /// </summary>
        private const string SERVICE_URL = "http://localhost:54321/ReturnCustomDataService";

        /// <summary>
        ///     エンドポイント名
        /// </summary>
        private const string ENDPOINT_ADDR = "";

        /// <summary>
        ///     バインディング
        /// </summary>
        private readonly BasicHttpBinding BINDING = new BasicHttpBinding();

        #endregion
    }

    #endregion
}
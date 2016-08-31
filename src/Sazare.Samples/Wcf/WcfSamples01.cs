// ReSharper disable CheckNamespace

using System;
using System.ServiceModel;
using Sazare.Common;

namespace Sazare.Samples
{

    #region WcfSample-01

    /// <summary>
    ///     WCFのサンプルです。
    /// </summary>
    /// <remarks>
    ///     最も基本的なサービスとクライアントの応答を行うサンプルです。
    /// </remarks>
    [Sample]
    public class WcfSamples01 : IExecutable
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
                    Output.WriteLine("サービスの呼び出し結果= {0}", proxy.SayHello());
                }
            }
        }

        private ServiceHost CreateService()
        {
            //
            // ホストを初期化
            //
            var host = new ServiceHost(typeof(HelloWorldService), new Uri(SERVICE_URL));

            //
            // エンドポイントを追加.
            //
            host.AddServiceEndpoint(typeof(IHelloWorldService), BINDING, ENDPOINT_ADDR);

            return host;
        }

        private ChannelFactory<IHelloWorldService> CreateChannelFactory()
        {
            //
            // クライアント側からサービスに接続するためにChannelFactoryを構築.
            //
            var factory =
                new ChannelFactory<IHelloWorldService>(BINDING, new EndpointAddress(SERVICE_URL));

            return factory;
        }

        /// <summary>
        ///     サービスインターフェース
        /// </summary>
        [ServiceContract]
        public interface IHelloWorldService
        {
            /// <summary>
            ///     サービスメソッド
            /// </summary>
            [OperationContract]
            string SayHello();
        }

        /// <summary>
        ///     サービスの実装
        /// </summary>
        public class HelloWorldService : IHelloWorldService
        {
            public string SayHello()
            {
                return "Hello World";
            }
        }

        #region Constants

        /// <summary>
        ///     サービスのURL
        /// </summary>
        private const string SERVICE_URL = "http://localhost:54321/HelloWorldService";

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
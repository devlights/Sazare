namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.ServiceModel;

  #region WcfSample-01
  /// <summary>
  /// WCFのサンプルです。
  /// </summary>
  /// <remarks>
  /// 最も基本的なサービスとクライアントの応答を行うサンプルです。
  /// </remarks>
  public class WcfSamples01 : IExecutable
  {
    #region Constants
    /// <summary>
    /// サービスのURL
    /// </summary>
    const string SERVICE_URL = "http://localhost:54321/HelloWorldService";
    /// <summary>
    /// エンドポイント名
    /// </summary>
    const string ENDPOINT_ADDR = "";
    /// <summary>
    /// バインディング
    /// </summary>
    readonly BasicHttpBinding BINDING = new BasicHttpBinding();
    #endregion

    /// <summary>
    /// サービスインターフェース
    /// </summary>
    [ServiceContract]
    public interface IHelloWorldService
    {
      /// <summary>
      /// サービスメソッド
      /// </summary>
      [OperationContract]
      string SayHello();
    }

    /// <summary>
    /// サービスの実装
    /// </summary>
    public class HelloWorldService : IHelloWorldService
    {
      public string SayHello()
      {
        return "Hello World";
      }
    }

    public void Execute()
    {
      using (ServiceHost host = CreateService())
      {
        //
        // サービスを開始.
        //
        host.Open();

        //
        // クライアント側を構築.
        //
        using (ChannelFactory<IHelloWorldService> factory = CreateChannelFactory())
        {
          //
          // クライアントプロキシオブジェクトを取得.
          //
          IHelloWorldService proxy = factory.CreateChannel();

          //
          // サービスメソッドを呼び出し、結果を取得.
          //
          Console.WriteLine("サービスの呼び出し結果= {0}", proxy.SayHello());
        }
      }
    }

    private ServiceHost CreateService()
    {
      //
      // ホストを初期化
      //
      ServiceHost host = new ServiceHost(typeof(HelloWorldService), new Uri(SERVICE_URL));

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
      ChannelFactory<IHelloWorldService> factory =
        new ChannelFactory<IHelloWorldService>(BINDING, new EndpointAddress(SERVICE_URL));

      return factory;
    }
  }
  #endregion
}

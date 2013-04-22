namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Serialization;
  using System.ServiceModel;

  using Sazare.Common;
  
  #region WcfSamples-02
  /// <summary>
  /// WCFのサンプルです。
  /// </summary>
  /// <remarks>
  /// 引数にカスタムオブジェクトを指定するサービスメソッドを定義しています。
  /// </remarks>
  [Sample]
  public class WcfSamples02 : Sazare.Common.IExecutable
  {
  #region Constants
    /// <summary>
    /// サービスのURL
    /// </summary>
    const string SERVICE_URL = "http://localhost:54321/NumberSumService";
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
    public interface INumberSumService
    {
      /// <summary>
      /// サービスメソッド
      /// </summary>
      [OperationContract]
      int Sum(Data data);
    }

    /// <summary>
    /// サービスの実装クラス
    /// </summary>
    public class NumberSumService : INumberSumService
    {
      public int Sum(Data data)
      {
        return (data.X + data.Y);
      }
    }

    /// <summary>
    /// データコントラクトクラス
    /// </summary>
    [DataContract]
    public class Data
    {
      [DataMember]
      public int X
      {
        get;
        set;
      }

      [DataMember]
      public int Y
      {
        get;
        set;
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
        using (ChannelFactory<INumberSumService> factory = CreateChannelFactory())
        {
          //
          // クライアントプロキシオブジェクトを取得.
          //
          INumberSumService proxy = factory.CreateChannel();

          //
          // サービスメソッドを呼び出し、結果を取得.
          //
          Output.WriteLine("サービスの呼び出し結果= {0}", proxy.Sum(new Data { X = 300, Y = 200 }));
        }
      }
    }

    private ServiceHost CreateService()
    {
      //
      // ホストを初期化
      //
      ServiceHost host = new ServiceHost(typeof(NumberSumService), new Uri(SERVICE_URL));

      //
      // エンドポイントを追加.
      //
      host.AddServiceEndpoint(typeof(INumberSumService), BINDING, ENDPOINT_ADDR);

      return host;
    }

    private ChannelFactory<INumberSumService> CreateChannelFactory()
    {
      //
      // クライアント側からサービスに接続するためにChannelFactoryを構築.
      //
      ChannelFactory<INumberSumService> factory =
        new ChannelFactory<INumberSumService>(BINDING, new EndpointAddress(SERVICE_URL));

      return factory;
    }
  }
  #endregion
}

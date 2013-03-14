namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Serialization;
  using System.ServiceModel;

  #region WcfSamples-03
  /// <summary>
  /// WCFのサンプルです。
  /// </summary>
  /// <remarks>
  /// 引数と戻り値にカスタムオブジェクトを指定するサービスメソッドを定義しています。
  /// </remarks>
  public class WcfSamples03 : IExecutable
  {
    #region Constants
    /// <summary>
    /// サービスのURL
    /// </summary>
    const string SERVICE_URL = "http://localhost:54321/ReturnCustomDataService";
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
    public interface IReturnCustomDataService
    {
      [OperationContract]
      ReturnData Execute(Data data);
    }

    /// <summary>
    /// サービス実装クラス
    /// </summary>
    public class ReturnCustomDataService : IReturnCustomDataService
    {
      public ReturnData Execute(Data data)
      {
        return new ReturnData { X = data.Y, Y = data.X };
      }
    }

    /// <summary>
    /// サービスメソッドの引数クラス
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

      public override string ToString()
      {
        return string.Format("X={0}, Y={1}", X, Y);
      }
    }

    /// <summary>
    /// サービスメソッドの戻り値クラス
    /// </summary>
    [DataContract]
    public class ReturnData
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

      public override string ToString()
      {
        return string.Format("X={0}, Y={1}", X, Y);
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
        using (ChannelFactory<IReturnCustomDataService> factory = CreateChannelFactory())
        {
          //
          // クライアントプロキシオブジェクトを取得.
          //
          IReturnCustomDataService proxy = factory.CreateChannel();

          //
          // サービスメソッドを呼び出し、結果を取得.
          //
          Data data = new Data { X = 300, Y = 200 };
          Console.WriteLine("サービスの呼び出し前= {0}", data);
          Console.WriteLine("サービスの呼び出し結果= {0}", proxy.Execute(data));
        }
      }
    }

    private ServiceHost CreateService()
    {
      //
      // ホストを初期化
      //
      ServiceHost host = new ServiceHost(typeof(ReturnCustomDataService), new Uri(SERVICE_URL));

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
      ChannelFactory<IReturnCustomDataService> factory =
        new ChannelFactory<IReturnCustomDataService>(BINDING, new EndpointAddress(SERVICE_URL));

      return factory;
    }
  }
  #endregion
}

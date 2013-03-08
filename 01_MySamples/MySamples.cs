// vim:set ts=2 sw=2 et ws is nowrap ft=cs:
//////////////////////////////////////////////////////////////////////
//
// 基本的なクラスライブラリに関するサンプルを集めたファイル.
//
//////////////////////////////////////////////////////////////////////
namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Data.Common;
  using System.Diagnostics;
  using System.Drawing;
  using System.Globalization;
  using System.IO;
  using System.IO.Compression;
  using System.Linq;
  using System.Net;
  using System.Reflection;
  using System.Runtime;
  using System.Runtime.CompilerServices;
  using System.Runtime.ConstrainedExecution;
  using System.Runtime.InteropServices;
  using System.Runtime.Remoting;
  using System.Runtime.Remoting.Messaging;
  using System.Runtime.Serialization;
  using System.Runtime.Serialization.Formatters.Binary;
  using System.ServiceModel;
  using System.ServiceModel.Syndication;
  using System.Text;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Windows;
  using System.Windows.Controls;
  //using System.Windows.Forms;
  using System.Xml;
  using System.Xml.Linq;
  //
  // Alias設定.
  //
  using GDIImage = System.Drawing.Image;
  using WinFormsApplication = System.Windows.Forms.Application;
  using WinFormsButton = System.Windows.Forms.Button;
  using WinFormsDockStyle = System.Windows.Forms.DockStyle;
  using WinFormsFlowDirection = System.Windows.Forms.FlowDirection;
  using WinFormsFlowLayoutPanel = System.Windows.Forms.FlowLayoutPanel;
  using WinFormsForm = System.Windows.Forms.Form;
  using WinFormsLabel = System.Windows.Forms.Label;
  using WinFormsProgressBar = System.Windows.Forms.ProgressBar;
  using WinFormsProgressBarStyle = System.Windows.Forms.ProgressBarStyle;

  #region ダミークラス
  /// <summary>
  /// ダミークラス
  /// </summary>
  class Dummy : IExecutable
  {
    /// <summary>
    /// 処理を実行します。
    /// </summary>
    public void Execute()
    {
      Console.WriteLine("THIS IS DUMMY CLASS.");
    }
  }
  #endregion
  
  #region サンプルの起動を担当するクラス
  /// <summary>
  /// サンプルの起動を担当するクラスです。
  /// </summary>
  /// <remarks>
  /// 本クラスがエントリポイントとなります。
  /// </remarks>
  public class SampleLauncher
  {
    /// <summary>
    /// エントリポイントメソッド
    /// </summary>
    /// <param name="args">起動時引数</param>
    [STAThread]
    static void Main(string[] args)
    {
      string className = typeof(Dummy).Name;
      if (args.Length != 0)
      {
        className = args[0];
      }

      if (!string.IsNullOrEmpty(className))
      {
        className = string.Format("{0}.{1}", typeof(SampleLauncher).Namespace, className);
      }

      //
      // 指定されたクラスを起動.
      //
      try
      {
        Assembly assembly = Assembly.GetExecutingAssembly();
        ObjectHandle handle = Activator.CreateInstance(assembly.FullName, className);
        if (handle != null)
        {
          object clazz = handle.Unwrap();

          if (clazz != null)
          {
            (clazz as IExecutable).Execute();
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }
  }
  #endregion
  
  #region ThreadLocalSamples-01
  /// <summary>
  /// ThreadLocal<T>クラスのサンプルです。
  /// </summary>
  public class ThreadLocalSamples01 : IExecutable
  {
    #region Static Fields
    // ThreadStatic
    [ThreadStatic]
    static int count = 2;
    static ThreadLocal<int> count2 = new ThreadLocal<int>(() => 2);
    #endregion
    
    #region Fields
    [ThreadStatic]
    int count3 = 2;
    ThreadLocal<int> count4 = new ThreadLocal<int>(() => 4);
    #endregion
    
    public void Execute()
    {
      //
      // ThreadLocal<T>は、.NET 4.0から追加された型である。
      // ThreadStatic属性と同様に、スレッドローカルストレージ(TLS)を表現するための型である。
      //
      // 従来より存在していたThreadStatic属性には、以下の点が行えなかった。
      //   ・インスタンスフィールドには対応していない。（staticフィールドのみ)
      //    (インスタンスフィールドにも属性を付与することが出来るが、ちゃんと動作しない）
      //   ・フィールドの値は常に、その型のデフォルト値で初期化される。初期値を設定しても無視される。
      //
      // ThreadLocal<T>は、上記の点を解決している。つまり
      //   ・インスタンスフィールドに対応している。
      //   ・フィールドの値を初期値で初期化出来る。
      //
      // 利用方法は、System.Lazyと似ており、コンストラクタに初期化のためのデリゲートを渡す。
      //
      
      //
      // staticフィールドのThreadState属性の確認
      // ThreadStatic属性では、初期値を宣言時に設定していても無視され、強制的にデフォルト値が適用されるので
      // 出力される値は、全て0となる。
      //
      int numberOfParallels = 10;
      using (var countdown = new CountdownEvent(numberOfParallels))
      {
        for (var i = 0; i < numberOfParallels; i++)
        {
          int tmp = i;
          new Thread(() => { Console.WriteLine("ThreadStatic [count]>>> {0}", count++); countdown.Signal(); }).Start();
        }
        
        countdown.Wait();
      }
      
      //
      // staticフィールドのThreadLocal<T>の確認
      // ThreadLocal<T>は、初期値を設定できるので、出力される値は2となる。
      //
      using (var countdown = new CountdownEvent(numberOfParallels))
      {
        for (var i = 0; i < numberOfParallels; i++)
        {
          new Thread(() => { Console.WriteLine("ThreadLocal<T> [count2]>>> {0}", count2.Value++); countdown.Signal(); }).Start();
        }
        
        countdown.Wait();
      }
      
      //
      // インスタンスフィールドのThreadStatic属性の確認
      // ThreadStatic属性は、インスタンスフィールドに対しては効果が無い。
      // なので、出力される値は2,3,4,5,6...とインクリメントされていく.
      //
      using (var countdown = new CountdownEvent(numberOfParallels))
      {
        for (var i = 0; i < numberOfParallels; i++)
        {
          int tmp = i;
          new Thread(() => { Console.WriteLine("ThreadStatic [count3]>>> {0}", count3++); countdown.Signal(); }).Start();
        }
        
        countdown.Wait();
      }
      
      //
      // インスタンスフィールドのThreadLocal<T>の確認
      // ThreadLocal<T>は、インスタンスフィールドに対しても問題なく利用できる。
      // なので、出力される値は4となる。
      //
      using (var countdown = new CountdownEvent(numberOfParallels))
      {
        for (var i = 0; i < numberOfParallels; i++)
        {
          new Thread(() => { Console.WriteLine("ThreadLocal<T> [count4]>>> {0}", count4.Value++); countdown.Signal(); }).Start();
        }
        
        countdown.Wait();
      }
      
      count2.Dispose();
      count4.Dispose();
    }
  }
  #endregion
  
  #region PLinqSamples-01
  public class PLinqSamples01 : IExecutable
  {
    public void Execute()
    {
      byte[] numbers = GetRandomNumbers();
      
      Stopwatch watch = Stopwatch.StartNew();
      
      // 普通のLINQ
      // var query1 = from x in numbers
      // 並列LINQ（１）（ExecutionModeを付与していないので、並列で実行するか否かはTPLが決定する）
      // var query1 = from x in numbers.AsParallel()
      // 並列LINQ（２）（ExecutionModeを付与しているので、強制的に並列で実行するよう指示）
      var query1 = from x in numbers.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                   select Math.Pow(x, 2);
      
      foreach (var item in query1)
      {
        Console.WriteLine(item);
      }
      
      watch.Stop();
      Console.WriteLine(watch.Elapsed);
    }
    
    byte[] GetRandomNumbers()
    {
      byte[] result = new byte[10];
      Random rnd = new Random();
      
      rnd.NextBytes(result);
      
      return result;
    }
  }
  #endregion
  
  #region PLinqSamples-02
  public class PLinqSamples02 : IExecutable
  {
    public void Execute()
    {
      // 並列LINQにて、元の順序を保持して処理するにはAsOrderedを利用する。
      // AsOrderedを指定していない場合、どの順序で処理されていくのかは保証されない。
      var query1 = from x in Enumerable.Range(1, 20)
                   select Math.Pow(x, 2);
      
      foreach (var item in query1)
      {
        Console.WriteLine(item);
      }
      
      Console.WriteLine("===============");
      
      //
      // 以下のように、元のデータシーケンスをIEnumerable<T>と指定した上で並列LINQを行おうとしても
      // 並列化されない。何故なら、IEnumerable<T>では、LINQはシーケンス内にいくつ要素が存在するのかを
      // 判別することが出来ないためである。
      //
      // 並列LINQは、元のシーケンスをPartionerを利用して、一定のサイズのチャンクに分けて
      // 同時実行するための機能であるため、元の要素数が分からない場合はチャンクに分けることが出来ない。
      //
      // 並列処理を行う為には、ToListかToArrayなどを行い変換してから処理を進めるか
      // ParallelEnumerable.Rangeを利用したりするとうまくいく。
      //
      // 以下の例では並列処理が行われない。
      //var query2 = from x in Enumerable.Range(1, 20).AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
      // 以下の例ではLINQがリストの要素数を判別することが出来るので、並列処理が行われる。
      var query2 = from x in Enumerable.Range(1, 20).ToList().AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                   select Math.Pow(x, 2);
      
      foreach (var item in query2)
      {
        Console.WriteLine(item);
      }
      
      Console.WriteLine("===============");
      
      var query3 = from x in ParallelEnumerable.Range(1, 20).AsParallel().AsOrdered().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                   select Math.Pow(x, 2);
      
      foreach (var item in query3)
      {
        Console.WriteLine(item);
      }
      
      Console.WriteLine("===============");
    }
  }
  #endregion
  
  #region GetInvalidPathCharsAndGetInvalidFileNameCharsSamples-01
  /// <summary>
  /// PathクラスのGetInvalidPathCharsメソッドとGetInvalidFileNameCharsメソッドのサンプルです。
  /// </summary>
  public class GetInvalidPathCharsAndGetInvalidFileNameCharsSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // Pathクラスには、パス名及びファイル名に利用できない文字を取得するメソッドが存在する。
      //   パス名：GetInvalidPathChars
      // ファイル名：GetInvalidFileNameChars
      //
      // 引数などで渡されたパスやファイル名に対して不正な文字が利用されていないか
      // チェックする際などに利用できる。
      //
      // 戻り値は、どちらもcharの配列となっている。
      //
      char[] invalidPathChars   = Path.GetInvalidPathChars();
      char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
      
      string tmpPath   = @"c:usrlocaltmp_<path>_tmp";
      string tmpFileName = @"tmp_<filename>_tmp.|||";
      
      Console.WriteLine("不正なパス文字が存在してる？     = {0}", invalidPathChars.Any(ch => tmpPath.Contains(ch)));
      Console.WriteLine("不正なファイル名文字が存在してる？ = {0}", invalidFileNameChars.Any(ch => tmpFileName.Contains(ch)));
    }
  }
  #endregion
  
  #region NewLineDetectSample-01
  /// <summary>
  /// 文字列中の改行コードの数を算出するサンプルです。
  /// </summary>
  public class NewLineDetectSample01 : IExecutable
  {
    public void Execute()
    {
      string testStrings = string.Format("あt{0}いt{0}う{0}えt{0}お{0}", Environment.NewLine);
      
      Console.WriteLine("=== 元文字列 start ===");
      Console.WriteLine(testStrings);
      Console.WriteLine("=== 元文字列 end  ===");
      
      //
      // 改行コードを判定するための、比較元文字配列を構築.
      //
      char[] newLineChars = Environment.NewLine.ToCharArray();
      
      //
      // 改行コードのカウントを算出.
      //
      int  count  = 0;
      char prevChar = char.MaxValue;
      foreach (Char ch in testStrings)
      {
        //
        // プラットフォームによっては、改行コードが２文字の構成 (CRLF)となるため
        // 前後の文字のパターンが両方一致する場合に改行コードであるとみなす。
        //
        if (newLineChars.Contains(prevChar) && newLineChars.Contains(ch))
        {
          count++;
        }
        
        prevChar = ch;
      }
      
      Console.WriteLine("改行コードの数: {0}", count);
    }
  }
  #endregion
  
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
    const  string       SERVICE_URL   = "http://localhost:54321/HelloWorldService";
    /// <summary>
    /// エンドポイント名
    /// </summary>
    const  string       ENDPOINT_ADDR = "";
    /// <summary>
    /// バインディング
    /// </summary>
    readonly BasicHttpBinding BINDING     = new BasicHttpBinding();
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
  
  #region WcfSamples-02
  /// <summary>
  /// WCFのサンプルです。
  /// </summary>
  /// <remarks>
  /// 引数にカスタムオブジェクトを指定するサービスメソッドを定義しています。
  /// </remarks>
  public class WcfSamples02 : IExecutable
  {
    #region Constants
    /// <summary>
    /// サービスのURL
    /// </summary>
    const  string       SERVICE_URL   = "http://localhost:54321/NumberSumService";
    /// <summary>
    /// エンドポイント名
    /// </summary>
    const  string       ENDPOINT_ADDR = "";
    /// <summary>
    /// バインディング
    /// </summary>
    readonly BasicHttpBinding BINDING     = new BasicHttpBinding();
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
          Console.WriteLine("サービスの呼び出し結果= {0}", proxy.Sum(new Data { X = 300, Y = 200 }));
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
    const  string       SERVICE_URL   = "http://localhost:54321/ReturnCustomDataService";
    /// <summary>
    /// エンドポイント名
    /// </summary>
    const  string       ENDPOINT_ADDR = "";
    /// <summary>
    /// バインディング
    /// </summary>
    readonly BasicHttpBinding BINDING     = new BasicHttpBinding();
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
          Console.WriteLine("サービスの呼び出し前= {0}",   data);
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
  
  #region 全角チェックと半角チェック
  /// <summary>
  /// 全角チェックと半角チェックのサンプルです。
  /// </summary>
  /// <remarks>
  /// 単純な全角チェックと半角チェックを定義しています。
  /// </remarks>
  public class ZenkakuHankakuCheckSample01 : IExecutable
  {
    public void Execute()
    {
      string zenkakuOnlyStrings     = "あいうえお";
      string hankakuOnlyStrings     = "ｱｲｳｴｵ";
      string zenkakuAndHankakuStrings = "あいうえおｱｲｳｴｵ";
      
      Console.WriteLine("IsZenkaku:zenkakuOnly:{0}",        IsZenkaku(zenkakuOnlyStrings));
      Console.WriteLine("IsZenkaku:hankakuOnlyStrings:{0}",     IsZenkaku(hankakuOnlyStrings));
      Console.WriteLine("IsZenkaku:zenkakuAndHankakuStrings:{0}", IsZenkaku(zenkakuAndHankakuStrings));
      Console.WriteLine("IsHankaku:zenkakuOnly:{0}",        IsHankaku(zenkakuOnlyStrings));
      Console.WriteLine("IsHankaku:hankakuOnlyStrings:{0}",     IsHankaku(hankakuOnlyStrings));
      Console.WriteLine("IsHankaku:zenkakuAndHankakuStrings:{0}", IsHankaku(zenkakuAndHankakuStrings));
    }
    
    bool IsZenkaku(string value)
    {
      //
      // 指定された文字列が全て全角文字で構成されているか否かは
      // 文字列を一旦SJISに変換し取得したバイト数と元文字列の文字数＊２が
      // 成り立つか否かで決定できる。
      //
      return (Encoding.GetEncoding("sjis").GetByteCount(value) == (value.Length * 2));
    }
    
    bool IsHankaku(string value)
    {
      //
      // 指定された文字列が全て半角文字で構成されているか否かは
      // 文字列を一旦SJISに変換し取得したバイト数と元文字列の文字数が
      // 成り立つか否かで決定できる。
      //
      return (Encoding.GetEncoding("sjis").GetByteCount(value) == value.Length);
    }
  }
  #endregion
  
  #region DateParseSamples-01
  public class DateParseSample01 : IExecutable
  {
    public void Execute()
    {
      //
      // ParseExactメソッドの場合は、値が2011, フォーマットがyyyy
      // の場合でも日付変換出来る。
      //
      try
      {
        var d = DateTime.ParseExact("2011", "yyyy", null);
        Console.WriteLine(d);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      
      //
      // TryParseメソッドの場合は、以下のどちらもFalseとなる。
      // 恐らく、IFormatProviderを設定しないと動かないと思われる。
      //
      DateTime d2;
      Console.WriteLine(DateTime.TryParse("2011", out d2));
      Console.WriteLine(DateTime.TryParse("2011", null, DateTimeStyles.None, out d2));
      
      //
      // TryParseExactメソッドの場合は、値が2011、フォーマットがyyyy
      // の場合でも日付変換出来る。
      //
      DateTime d3;
      Console.WriteLine(DateTime.TryParseExact("2011", "yyyy", null, DateTimeStyles.None, out d3));
      
      Console.WriteLine(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
      
      var d98 = DateTime.Now;
      var d99 = DateTime.ParseExact(d98.ToString("yyyyMMddHHmmssfff"), "yyyyMMddHHmmssfff", null);
      Console.WriteLine(d98 == d99);
      Console.WriteLine(d98.Ticks);
      Console.WriteLine(d98 == new DateTime(d98.Ticks));
      
      // 時分秒を指定していない場合は、00:00:00となる
      var d100 = new DateTime(2011, 11, 12);
      Console.WriteLine("{0}, {1}, {2}", d100.Hour, d100.Minute, d100.Second);
    }
  }
  #endregion
  
  #region RssSamples-01
  public class RssSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // RSSを作成には以下の手順で構築する.
      //
      // (1) SyndicationItemを作成し、リストに追加
      // (2) SyndicationFeedを作成し、(1)で作成したリストを追加
      // (3) XmlWriterを構築し、出力.
      //
      List<SyndicationItem> items = new List<SyndicationItem>();
      
      for (int i = 0; i < 10; i++)
      {
        var newItem = new SyndicationItem();
        
        newItem.Title = new TextSyndicationContent(string.Format("Test Title-{0}", i));
        newItem.Links.Add(new SyndicationLink(new Uri(@"http://www.google.co.jp/")));
        
        items.Add(newItem);
      }
      
      SyndicationFeed feed = new SyndicationFeed("Test Feed", "This is a test feed", new Uri(@"http://www.yahoo.co.jp/"), items);
      feed.LastUpdatedTime = DateTime.Now;
      
      StringBuilder sb = new StringBuilder();
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = true;
      
      using (XmlWriter writer = XmlWriter.Create(sb, settings))
      {
        //feed.SaveAsAtom10(writer);
        feed.SaveAsRss20(writer);
        writer.Close();
      }
      
      Console.WriteLine(sb.ToString());
    }
  }
  #endregion
  
  #region ImageConverterSamples-01
  /// <summary>
  /// ImageConverterクラスのサンプルです。
  /// </summary>
  public class ImageConverterSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // Imageオブジェクトを取得.
      //
      GDIImage image = GDIImage.FromFile("database.png");
      
      //
      // Imageをバイト配列に変換.
      //   Imageから別のオブジェクトに変換する場合はConvertToを利用する.
      //
      ImageConverter converter = new ImageConverter();
      byte[] imageBytes = (byte[]) converter.ConvertTo(image, typeof(byte[]));
      
      //
      // バイト配列をImageに変換.
      //   バイト配列からImageオブジェクトに変換する場合はConvertFromを利用する.
      //
      GDIImage image2 = (GDIImage) converter.ConvertFrom(imageBytes);
      
      // 確認.
      Debug.Assert(image != null);
      Debug.Assert(imageBytes != null && imageBytes.Length > 0);
      Debug.Assert(image2 != null);
      
      //
      // [補足]
      // Imageオブジェクトをファイルとして保存する場合は以下のようにする.
      //
      //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      //string fileName    = @"Sample.png";
      //string filePath    = Path.Combine(desktopPath, fileName);
      //
      //using (Stream stream = File.Create(filePath))
      //{
      //  image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
      //}
    }
  }
  #endregion
  
  #region RuntimeHelpersSamples-01
  /// <summary>
  /// RuntimeHelpersクラスのサンプルです。
  /// </summary>
  public class RuntimeHelpersSamples01 : IExecutable
  {
    class SampleClass
    {
      public int Id { get; set; }
      
      public override int GetHashCode()
      {
        return Id.GetHashCode();
      }
    }
    
    public void Execute()
    {
      //
      // RuntimeHelpersクラスのGetHashCodeは、他のクラスのGetHashCodeメソッド
      // と挙動が少し違う。以下、MSDN(http://msdn.microsoft.com/ja-jp/library/11tbk3h9.aspx)に
      // ある記述を引用。
      //
      // ・Object.GetHashCode は、オブジェクト値を考慮するシナリオで便利です。 同じ内容の 2 つの文字列は、Object.GetHashCode で同じ値を返します。
      // ・RuntimeHelpers.GetHashCode は、オブジェクト識別子を考慮するシナリオで便利です。 同じ内容の 2 つの文字列は、内容が同じでも異なる文字列オブジェクトであるため、RuntimeHelpers.GetHashCode で異なる値を返します。
      //
      // 以下では、サンプルとなるオブジェクトを2つ作成し、ハッシュコードを出力するようにしている。
      // サンプルクラスでは、GetHashCodeメソッドをオーバーライドしており、Idプロパティのハッシュコードを
      // 返すようにしている。
      //   (注意) このクラスのGetHashCodeメソッドの実装は、サンプルのために簡略化してあります。
      //         実際の実装で、このようなハッシュコードの算出はしてはいけません。
      //
      // 以下の場合、Object.GetHashCodeを呼び出している場合は当然ながら同じハッシュコードとなるが
      // RuntimeHelpers.GetHashCodeを呼び出している場合、違うハッシュコードとなる.
      //
      SampleClass sampleObj1 = new SampleClass{ Id = 100 };
      SampleClass sampleObj2 = new SampleClass{ Id = 100 };
      
      Console.WriteLine("[Object.GetHashCode]        sampleObj1 = {0}, sampleObj2 = {1}", sampleObj1.GetHashCode(), sampleObj2.GetHashCode());
      Console.WriteLine("[RuntimeHelper.GetHashCode] sampleObj1 = {0}, sampleObj2 = {1}", RuntimeHelpers.GetHashCode(sampleObj1), RuntimeHelpers.GetHashCode(sampleObj2));
  
      //
      // 文字列データで検証.
      // 以下は、文字列のハッシュコードが異なるか否かを検証.
      // 変数s1, s2を作成してから、連結して文字列値を作成している理由は
      // CLRによって、内部で文字列がインターン(Intern)されないようにするため.
      //
      // 文字列がInternされていない場合、RuntimeHelpers.GetHashCodeメソッドは
      // 違う値を返す。Object.GetHashCodeは同じハッシュコードを返す.
      //
      string s1    = "hello ";
      string s2    = "world";
      string test1 = s1 + s2;
      string test2 = s1 + s2;
      
      Console.WriteLine("[Object.GetHashCode]        test1 = {0}, test2 = {1}", test1.GetHashCode(), test2.GetHashCode());
      Console.WriteLine("[RuntimeHelper.GetHashCode] test1 = {0}, test2 = {1}", RuntimeHelpers.GetHashCode(test1), RuntimeHelpers.GetHashCode(test2));
      
      //
      // 文字列データで検証
      // 以下は、CLRによって文字列がインターンされる値に対してハッシュコードを取得している.
      //
      // この場合、RuntimeHelpers.GetHashCodeでも同じハッシュコードが返ってくる.
      // 尚、CLRによって値がインターンされるのはリテラルだけである.
      // 連結操作によって作成された文字列はインターンされない.
      // 無理矢理インターンするには、String.Internメソッドを利用する.
      //
      string test3 = "hello world";
      string test4 = "hello world";
      
      Console.WriteLine("[Object.GetHashCode]        test3 = {0}, test4 = {1}", test3.GetHashCode(), test4.GetHashCode());
      Console.WriteLine("[RuntimeHelper.GetHashCode] test3 = {0}, test4 = {1}", RuntimeHelpers.GetHashCode(test3), RuntimeHelpers.GetHashCode(test4));
    }
  }
  #endregion
  
  #region RuntimeHelpersSamples-02
  /// <summary>
  /// RuntimeHelpersクラスのサンプルです。
  /// </summary>
  public class RuntimeHelpersSamples02 : IExecutable
  {
    // サンプルクラス
    static class SampleClass
    {
      static SampleClass()
      {
        Console.WriteLine("SampleClass static ctor()");
      }
  
      //
      // このメソッドに対して、CER内で利用できるよう信頼性のコントラクトを付与.
      // ReliabilityContractAttributeおよびConsistencyやCerは
      // System.Runtime.ConstrainedExecution名前空間に存在する.
      //
      // RuntimeHelpers.PrepareConstrainedRegionsメソッドにて
      // 実行できるのは、Consistency.WillNotCorruptStateおよびMayCorruptInstanceの場合のみ.
      //
      // 尚、この属性はメソッドだけではなく、クラスやインターフェースにも付与できる。
      // その場合、クラス全体に対して信頼性のコントラクトを付与したことになる。
      //
      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      internal static void Print()
      {
        Console.WriteLine("SampleClass.Print()");
      }
    }
    
    public void Execute()
    {
      //
      // RuntimeHelpers.PrepareConstrainedRegionsを呼び出すと、コンパイラは
      // そのメソッド内のcatch, finallyブロックをCER（制約された実行領域）としてマークする。
      //
      // CERとしてマークされた領域から、コードを呼び出す場合、そのコードには信頼性のコントラクトが必要となる。
      // コードに対して、信頼性のコントラクトを付与するには、以下の属性を利用する。
      //  [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      //
      // CERでマークされた領域にて、コードに信頼性のコントラクトが付与されている場合、CLRは
      // try内の本処理が実行される前に、catch, finallyブロックのコードを事前コンパイルする。
      //
      // なので、例えばfinallyブロック内にて静的コンストラクタを持つクラスのメソッドを呼びだしていたり
      // すると、try内の本処理よりも先にfinallyブロック内の静的コンストラクタが呼ばれる事になる。
      // (事前コンパイルが行われると、アセンブリのロード、静的コンストラクタの実行などが発生するため)
      //
      RuntimeHelpers.PrepareConstrainedRegions();
      
      try
      {
        // 事前にRuntimeHelpers.PrepareConstrainedRegions()を呼び出している場合
        // 以下のメソッドが呼び出される前に、catch, finallyブロックが事前コンパイルされる.
        Calc();
      }
      finally
      {
        SampleClass.Print();
      }
    }
    
    void Calc()
    {
      for (int i = 0; i < 10; i++)
      {
        Console.Write("{0} ", (i + 1));
      }
      
      Console.WriteLine("");
    }
  }
  #endregion
  
  #region RuntimeHelpersSamples-03
  /// <summary>
  /// RuntimeHelpersクラスのサンプルです。
  /// </summary>
  public class RuntimeHelpersSamples03 : IExecutable
  {
    // サンプルクラス
    static class SampleClass
    {
      static SampleClass()
      {
        Console.WriteLine("SampleClass static ctor()");
      }
  
      //
      // このメソッドに対して、CER内で利用できるよう信頼性のコントラクトを付与.
      // ReliabilityContractAttributeおよびConsistencyやCerは
      // System.Runtime.ConstrainedExecution名前空間に存在する.
      //
      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
      internal static void Print()
      {
        Console.WriteLine("SampleClass.Print()");
      }
    }
    
    public void Execute()
    {
      //
      // ExecuteCodeWithGuaranteedCleanupメソッドは, PrepareConstrainedRegionsメソッドと
      // 同様に、コードをCER（制約された実行環境）で実行するメソッドである。
      //
      // PrepareConstrainedRegionsメソッドが呼び出されたメソッドのcatch, finallyブロックを
      // CERとしてマークするのに対して、ExecuteCodeWithGuaranteedCleanupメソッドは
      // 明示的に実行コード部分とクリーンアップ部分 (バックアウトコード)を引数で渡す仕様となっている。
      //
      // ExecuteCodeWithGuaranteedCleanupメソッドは
      // TryCodeデリゲートとCleanupCodeデリゲート、及び、userDataを受け取る.
      //
      // public delegate void TryCode(object userData)
      // public delegate void CleanupCode(object userData, bool exceptionThrown)
      //
      // 前回のサンプルと同じ動作を行う.
      RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(Calc, Cleanup, null);
    }
    
    void Calc(object userData)
    {
      for (int i = 0; i < 10; i++)
      {
        Console.Write("{0} ", (i + 1));
      }
      
      Console.WriteLine("");
    }
    
    void Cleanup(object userData, bool exceptionThrown)
    {
      SampleClass.Print();
    }
  }
  #endregion
  
  #region CompareOptionsSamples-01
  /// <summary>
  /// 比較メソッドの結果値を変換するためのヘルパークラス.
  /// </summary>
  public static class CompareResultHelper
  {
    static readonly string[] CompResults = { "小さい", "等しい", "大きい" };
    
    // 比較結果の数値を文字列に変換.
    public static string ToStringResult(this int self)
    {
      return CompResults[self + 1];
    }
  }
  
  /// <summary>
  /// CompareOptions列挙型のサンプルです。
  /// </summary>
  public class CompareOptionsSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // string.Compareメソッドには、CultureInfoとCompareOptionsを
      // 引数にとるオーバーロードが定義されている。(他にもオーバーロードメソッドが存在します。)
      //
      // このオーバーロードを利用する際、CompareOptions.IgnoreKanaTypeを指定すると
      // 「ひらがな」と「カタカナ」の違いを無視して、文字列比較を行う事が出来る。
      //
      string ja1 = "はろーわーるど";
      string ja2 = "ハローワールド";
      
      CultureInfo ci = new CultureInfo("ja-JP");
      
      // 標準の比較方法で比較
      Console.WriteLine("{0}", string.Compare(ja1, ja2, ci, CompareOptions.None).ToStringResult());
      // 大文字小文字を無視して比較.
      Console.WriteLine("{0}", string.Compare(ja1, ja2, ci, CompareOptions.IgnoreCase).ToStringResult());
      // ひらがなとカタカナの違いを無視して比較
      // つまり、「はろーわーるど」と「ハローワールド」を同じ文字列として比較
      Console.WriteLine("{0}", string.Compare(ja1, ja2, ci, CompareOptions.IgnoreKanaType).ToStringResult());
      
      //
      // string.Compareメソッドは、内部でCutureInfoから、そのカルチャーに紐づく
      // CompareInfoを取り出して、比較処理を行っているので、自前で直接CompareInfoを
      // 用意して、Compareメソッドを呼び出しても同じ結果となる。
      //
      CompareInfo compInfo = CompareInfo.GetCompareInfo("ja-JP");
      Console.WriteLine("{0}", compInfo.Compare(ja1, ja2, CompareOptions.IgnoreKanaType).ToStringResult());
    }
  }
  #endregion
  
  #region CompareOptionsSamples-02
  /// <summary>
  /// CompareOptions列挙型のサンプルです。
  /// </summary>
  public class CompareOptionsSamples02 : IExecutable
  {
    public void Execute()
    {
      //
      // string.Compareメソッドには、CultureInfoとCompareOptionsを
      // 引数にとるオーバーロードが定義されている。(他にもオーバーロードメソッドが存在します。)
      //
      // このオーバーロードを利用する際、CompareOptions.IgnoreKanaTypeを指定すると
      // 「ひらがな」と「カタカナ」の違いを無視して、文字列比較を行う事が出来る。
      //
      // さらに、CompareOptionsには、IgnoreWidthという値も存在し
      // これを指定すると、全角と半角の違いを無視して、文字列比較を行う事が出来る。
      //
      string ja1 = "ハローワールド";
      string ja2 = "ﾊﾛｰﾜｰﾙﾄﾞ";
      string ja3 = "はろーわーるど";
      
      CultureInfo ci = new CultureInfo("ja-JP");
      
      // 全角半角の違いを無視して、「ハローワールド」と「ﾊﾛｰﾜｰﾙﾄﾞ」を比較
      Console.WriteLine("{0}", string.Compare(ja1, ja2, ci, CompareOptions.IgnoreWidth).ToStringResult());
      // 全角半角の違いを無視して、「はろーわーるど」と「ﾊﾛｰﾜｰﾙﾄﾞ」を比較
      Console.WriteLine("{0}", string.Compare(ja3, ja2, ci, CompareOptions.IgnoreWidth).ToStringResult());
      // 全角半角の違いを無視し、且つ、ひらがなとカタカナの違いを無視して、「はろーわーるど」と「ﾊﾛｰﾜｰﾙﾄﾞ」を比較
      Console.WriteLine("{0}", string.Compare(ja3, ja2, ci, (CompareOptions.IgnoreWidth | CompareOptions.IgnoreKanaType)).ToStringResult());
    }
  }
  #endregion
  
  #region AppDomainSamples-01
  /// <summary>
  /// AppDomainクラスのサンプルです。
  /// </summary>
  public class AppDomainSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // AppDomainには、.NET 4.0より以下のイベントが追加されている。
      //   ・FirstChanceExceptionイベント
      // このイベントは、例外が発生した際に文字通り最初に通知されるイベントである。
      // このイベントに通知されるタイミングは、catch節にて例外が補足されるよりも先となる。
      // 
      // 注意点として
      //   ・このイベントは、通知のみとなる。このイベントをハンドルしたからといって例外の発生が
      //    ここで止まるわけではない。例外は通常通りプログラムコード上のcatchに入ってくる。
      //   ・このイベントは、アプリケーションドメイン毎に定義できる。
      //   ・FirstChanceExceptionイベント内での例外は、絶対にハンドラ内でキャッチしないといけない。
      //    そうしないと、再帰的にFirstChanceExceptionが発生する。
      //   ・イベント引数であるFirstChanceExceptionEventArgsクラスは
      //    System.Runtime.ExceptionServices名前空間に存在する。
      //
      
      // 基底のAppDomainにて、FirstChanceExceptionイベントをハンドル.
      AppDomain.CurrentDomain.FirstChanceException += FirstChanceExHandler;
  
      try
      {
        // わざと例外発生.
        throw new InvalidOperationException("test Ex messsage");
      } 
      catch (InvalidOperationException ex)
      {
        // 本来のcatch処理.
        Console.WriteLine("Catch clause: {0}", ex.Message);
      }
      
      // イベントをアンバインド.
      AppDomain.CurrentDomain.FirstChanceException -= FirstChanceExHandler;
    }
    
    // イベントハンドラ.
    void FirstChanceExHandler(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
    {
      Console.WriteLine("FirstChanceException: {0}", e.Exception.Message);
    }
  }
  #endregion
  
  #region AppDomainSamples-02
  /// <summary>
  /// AppDomainクラスのサンプルです。
  /// </summary>
  public class AppDomainSamples02 : MarshalByRefObject, IExecutable
  {
    public void Execute()
    {
      AppDomain defaultDomain = AppDomain.CurrentDomain;
      AppDomain anotherDomain = AppDomain.CreateDomain("AnotherAppDomain");
      
      //
      // DomainUnloadイベントのハンドル.
      //
      // 既定のアプリケーションドメインでは、Unloadは登録できるが発行されることは
      // 無いので、設定する意味がない.
      //defaultDomain.DomainUnload += AppDomain_Unload;
      anotherDomain.DomainUnload += AppDomain_Unload;
      
      //
      // ProcessExitイベントのハンドル.
      //
      defaultDomain.ProcessExit += AppDomain_ProcessExit;
      anotherDomain.ProcessExit += AppDomain_ProcessExit;
      
      //
      // 既定のアプリケーションドメインをアンロードしようとするとエラーとなる.
      // ** appdomain をアンロード中にエラーが発生しました。 (HRESULT からの例外: 0x80131015) **
      //AppDomain.Unload(defaultDomain);
      
      //
      // AppDomain.Unloadを呼び出すと、DomainUnloadイベントが発生する.
      // AppDomain.Unloadを呼び出さずにプロセスが終了させようとすると
      // ProcessExitイベントが発生する。両方のイベントが同時に発生することは無い.
      //
      // 以下をコメントアウトすると、ProcessExitイベントが発生する.
      //
      //AppDomain.Unload(anotherDomain);
    }
    
    void AppDomain_Unload(object sender, EventArgs e)
    {
      AppDomain domain = sender as AppDomain;
      Console.WriteLine("AppDomain.Unload: {0}", domain.FriendlyName);
    }
    
    void AppDomain_ProcessExit(object sender, EventArgs e)
    {
      //
      // ProcessExitイベントには、タイムアウトが存在する。（既定は2秒）
      // 以下、MSDNの記述.
      // (http://msdn.microsoft.com/ja-jp/library/system.appdomain.processexit.aspx)
      //
      // 「プロセス シャットダウン時における全ファイナライザーの合計実行時間が限られているように、ProcessExit の
      // すべてのイベント ハンドラーに対して割り当てられる合計実行時間も限られています。 既定値は 2 秒です。」
      //
      // 以下のコメントを外して実行すると、タイムアウト時間を過ぎるので
      // イベントをハンドルしていても、後続の処理は実行されない。
      //
      // わざとタイムアウト時間が過ぎるように待機.
      //Console.WriteLine("AppDomain.ProcessExit Thread.Sleep()");
      //Thread.Sleep(TimeSpan.FromSeconds(3));
      
      AppDomain domain = sender as AppDomain;
      Console.WriteLine("AppDomain.ProcessExit: {0}", domain.FriendlyName);
    }
  }
  #endregion
  
  #region AppDomainSamples-03
  /// <summary>
  /// AppDomainクラスのサンプルです。
  /// </summary>
  public class AppDomainSamples03 : IExecutable
  {
    // AppDomainのモニタリングを担当するクラス
    class AppDomainMonitor : IDisposable
    {
      static AppDomainMonitor()
      {
        //
        // AppDomain.MonitoringIsEnabledは、特殊なプロパティで
        // 以下の特徴を持つ。
        //
        // ・一度True（監視ON）にしたら、false（監視OFF）に戻すことはできない。
        // ・値がTrue,False関係なく、Falseを設定しようとすると例外が発生する。
        // ・設定は、AppDomain共通設定となり、特定のAppDomainのみの監視は行えない.
        //
        if (!AppDomain.MonitoringIsEnabled)
        {
          AppDomain.MonitoringIsEnabled = true;
        }
      }
      
      public void Dispose()
      {
        // フルブロッキングコレクションを実行.
        GC.Collect();
        PrintMonitoringValues(AppDomain.CurrentDomain);
      }
      
      public void PrintMonitoringValues(AppDomain domain)
      {
        //
        // モニタリングをONにすると、以下のプロパティにアクセスして統計情報を取得することができるようになる。
        //
        // ・MonitoringSurvivedMemorySize
        //    最後の完全なブロッキング コレクションの実行後に残された、現在のアプリケーション ドメインによって参照されていることが判明しているバイト数
        // ・MonitoringSurvivedProcessMemorySize
        //    最後の完全なブロッキング コレクションの実行後に残された、プロセス内のすべてのアプリケーション ドメインにおける合計バイト数
        // ・MonitoringTotalAllocatedMemorySize
        //    アプリケーション ドメインが作成されてから、そのアプリケーション ドメインで実行されたすべてのメモリ割り当ての合計サイズ（バイト単位）
        //    収集されたメモリは差し引かれない。
        // ・MonitoringTotalProcessorTime
        //    プロセスが開始されてから、現在のアプリケーション ドメインでの実行中にすべてのスレッドで使用された合計プロセッサ時間
        //
        // 完全なブロッキングコレクション（フルブロッキングコレクション）は、GC.Collectメソッドで実行できる。
        //
        Console.WriteLine("============================================");
        Console.WriteLine("MonitoringSurvivedMemorySize        = {0:N0}", domain.MonitoringSurvivedMemorySize);
        Console.WriteLine("MonitoringSurvivedProcessMemorySize = {0:N0}", AppDomain.MonitoringSurvivedProcessMemorySize);
        Console.WriteLine("MonitoringTotalAllocatedMemorySize  = {0:N0}", domain.MonitoringTotalAllocatedMemorySize);
        Console.WriteLine("MonitoringTotalProcessorTime        = {0}",    domain.MonitoringTotalProcessorTime);
        Console.WriteLine("============================================");
      }
    }
    
    public void Execute()
    {
      using (AppDomainMonitor monitor = new AppDomainMonitor())
      {
        monitor.PrintMonitoringValues(AppDomain.CurrentDomain);
        
        List<string> aList = new List<string>();
        for (int i = 0; i < 1000; i++)
        {
          aList.Add(string.Format("hello world-{0:D2}", i));
        }
        
        monitor.PrintMonitoringValues(AppDomain.CurrentDomain);
        
        // CPUタイムを表示したいので、少しスピン.
        Thread.SpinWait(700000000);
      }
    }
  }
  #endregion
  
  #region AppDomainSamples-04
  public class AppDomainSamples04 : MarshalByRefObject, IExecutable
  {
    public void Execute()
    {
      //
      // AppDomainを利用して、別のAppDomainで処理を実行するための方法は、いくつか存在する。
      //
      // ・AppDomain.ExecuteAssemblyを利用する。
      // ・AppDomain.DoCallbackを利用する。
      // ・AppDomain.CreateInstanceAndUnwrapを利用して、プロキシを取得し実行.
      //
      var currentDomain = AppDomain.CurrentDomain;
      var anotherDomain = AppDomain.CreateDomain("AD No.2");
      
      //
      // AppDomain.ExecuteAssemblyを利用して実行.
      // 
      // ExecuteAssemblyメソッドには、アセンブリ名を指定する。
      // ここで指定するアセンブリは実行可能である必要があり、エントリポイントを持っている必要がある。
      //
      anotherDomain.ExecuteAssembly(@"AnotherAppDomain.exe");
      
      //
      // AppDomain.DoCallbackを利用する.
      //
      // DoCallbackは指定されたデリゲートを実行するためのメソッド.
      // 別のAppDomainのDoCallbackにデリゲートを渡す事により
      // 処理がそのアプリケーションドメインで実行される。
      //
      // 当然、値渡し(Serializable)と参照渡し(MarshalByRefObject)によって実行結果が異なる場合がある.
      //
      // Staticメソッド
      Console.WriteLine("----------[Static Method]--------");
      currentDomain.DoCallBack(CallbackMethod_S);
      anotherDomain.DoCallBack(CallbackMethod_S);
      Console.WriteLine("---------------------------------");
      
      // インスタンスメソッド.
      Console.WriteLine("---------[Instance Method]-------");
      currentDomain.DoCallBack(CallbackMethod);
      anotherDomain.DoCallBack(CallbackMethod);
      Console.WriteLine("---------------------------------");
      
      // 値渡し (Serializable)
      var byvalObj = new MarshalByVal();
      
      Console.WriteLine("---------[Serializable]----------");
      currentDomain.DoCallBack(byvalObj.CallbackMethod);
      anotherDomain.DoCallBack(byvalObj.CallbackMethod);
      Console.WriteLine("---------------------------------");
      
      // 参照渡し (MarshalByRefObject)
      // MarshalByRefObjectを継承しているため、以下の例では必ずデフォルトドメインで実行されることになる。
      var byrefObj = new MarshalByRef();
      
      Console.WriteLine("-------[MarshalByRefObject]------");
      currentDomain.DoCallBack(byrefObj.CallbackMethod);
      anotherDomain.DoCallBack(byrefObj.CallbackMethod);
      Console.WriteLine("---------------------------------");
      
      //
      // AppDomain.CreateInstanceAndUnwrapを利用する。
      // プロキシを取得して処理を実行する.
      //
      var asmName  = typeof(MarshalByRef).Assembly.FullName;
      var typeName = typeof(MarshalByRef).FullName;
      
      var obj = (MarshalByRef) anotherDomain.CreateInstanceAndUnwrap(asmName, typeName);
      
      Console.WriteLine("-------[CreateInstanceAndUnwrap]------");
      obj.CallbackMethod();
      Console.WriteLine("--------------------------------------");
      
      AppDomain.Unload(anotherDomain);
    }
    
    static void CallbackMethod_S()
    {
      Utils.PrintAsmName();
    }

    void CallbackMethod()
    {
      Utils.PrintAsmName();
    }
    
    [Serializable]
    public class MarshalByVal
    {
      public void CallbackMethod()
      {
        Utils.PrintAsmName();
      }
    }
    
    public class MarshalByRef : MarshalByRefObject
    {
      public void CallbackMethod()
      {
        Utils.PrintAsmName();
      }
    }
    
    static class Utils
    {
      public static void PrintAsmName()
      {
        var domain = AppDomain.CurrentDomain.FriendlyName;
        Console.WriteLine("Run on AppDomain:{0}", domain);
      }
    }
  }
  #endregion
  
  #region SerializationSurrogateSamples-01
  /// <summary>
  /// シリアライズに関するサンプルです。
  /// </summary>
  /// <remarks>
  /// シリアル化サロゲートについて。 (ISerializationSurrogate)
  /// </remarks>
  public class SerializationSurrogateSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // 普通のシリアライズ処理.
      //
      var obj = MakeSerializableObject();
      using (var stream = new MemoryStream())
      {
        var formatter = new BinaryFormatter();
        
        // 成功する.
        formatter.Serialize(stream, obj);
        
        stream.Position = 0;
        Console.WriteLine(formatter.Deserialize(stream));
      }
      
      //
      // シリアライズ不可 (Serializable属性をつけていない)
      //
      var obj2 = MakeNotSerializableObject();
      using (var stream = new MemoryStream())
      {
        var formatter = new BinaryFormatter();
        
        try
        {
          // 対象クラスにSerializable属性が付与されていないので
          // 以下を実行すると例外が発生する.
          formatter.Serialize(stream, obj2);
          
          stream.Position = 0;
          Console.WriteLine(formatter.Deserialize(stream));
        }
        catch (SerializationException ex)
        {
          Console.WriteLine("[ERROR]: {0}", ex.Message);
        }
      }
      
      //
      // シリアル化サロゲート. (SerializationSurrogate)
      //
      var obj3 = MakeNotSerializableObject();
      using (var stream = new MemoryStream())
      {
        var formatter = new BinaryFormatter();
        
        //
        // シリアル化サロゲートを行うために、以下の手順で設定を行う.
        //
        // 1.SurrogateSelectorオブジェクトを用意.
        // 2.自作Surrogateクラスを用意.
        // 3.SurrogateSelector.AddSurrogateでSurrogateオブジェクトを設定
        // 4.SurrogateSelectorをFormatterに設定.
        //
        // これにより、シリアライズ不可なオブジェクトをFormatterにてシリアライズ/デシリアライズ
        // する際にシリアル化サロゲートが行われるようになる。
        //
        var selector  = new SurrogateSelector();
        var surrogate = new CanNotSerializeSurrogate();
        var context   = new StreamingContext(StreamingContextStates.All);
        
        selector.AddSurrogate(typeof(CanNotSerialize), context, surrogate);
        
        formatter.SurrogateSelector = selector;
        
        try
        {
          // 通常、以下を実行すると例外が発生するが
          // シリアル化サロゲートを行うので、エラーとならずシリアライズが成功する.
          formatter.Serialize(stream, obj3);
          
          stream.Position = 0;
          Console.WriteLine(formatter.Deserialize(stream));
        }
        catch (SerializationException ex)
        {
          Console.WriteLine("[ERROR]: {0}", ex.Message);
        }
      }
    }
    
    IHasNameAndAge MakeSerializableObject()
    {
      return new CanSerialize 
                 { 
                    Name = "hoge"
                   ,Age = 99 
                 };
    }
    
    IHasNameAndAge MakeNotSerializableObject()
    {
      return new CanNotSerialize
                 {
                     Name = "hehe"
                    ,Age = 98
                 };
    }
    
    #region SampleInterfaceAndClasses
    interface IHasNameAndAge
    {
      string Name { get; set; }
      int    Age  { get; set; }
    }
    
    // シリアライズ可能なクラス
    [Serializable]
    class CanSerialize : IHasNameAndAge
    {
      string _name;
      int    _age;
      
      public string Name
      {
        get { return _name; }
        set { _name = value; }
      }
      
      public int Age 
      {
        get { return _age; }
        set { _age = value; }
      }
      
      public override string ToString() 
      { 
        return string.Format("[CanSerialize] Name={0}, Age={1}", Name, Age); 
      }
    }
    
    // シリアライズ不可なクラス
    class CanNotSerialize : IHasNameAndAge
    {
      string _name;
      int    _age;
      
      public string Name
      {
        get { return _name; }
        set { _name = value; }
      }
      
      public int Age 
      {
        get { return _age; }
        set { _age = value; }
      }
      
      public override string ToString() 
      { 
        return string.Format("[CanNotSerialize] Name={0}, Age={1}", Name, Age); 
      }
    }
    
    // CanNotSerializeクラスのためのサロゲートクラス.
    class CanNotSerializeSurrogate : ISerializationSurrogate
    {
      // シリアライズ時に呼び出されるメソッド
      public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
      {
        CanNotSerialize targetObj = obj as CanNotSerialize;
        
        //
        // シリアライズする項目と値を以下のようにinfoに設定していく.
        //
        info.AddValue("Name", targetObj.Name);
        info.AddValue("Age",  targetObj.Age);
      }
      
      // デシリアライズ時に呼び出されるメソッド.
      public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
      {
        CanNotSerialize targetObj = obj as CanNotSerialize;
        
        //
        // infoから値を取得し、対象となるオブジェクトに設定.
        //
        targetObj.Name = info.GetString("Name");
        targetObj.Age  = info.GetInt32("Age");
        
        // Formatterは, この戻り値を無視するので戻り値はnullで良い.
        return null;
      }
    }
    #endregion
  }
  #endregion
  
  #region CallContextSamples-01
  /// <summary>
  /// 実行コンテキスト(ExecutionContext)と論理呼び出しコンテキスト(CallContext)のサンプルです。
  /// </summary>
  public class CallContextSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // すべてのスレッドには、実行コンテキスト (Execution Context) が関連付けられている。
      // 実行コンテキストには
      //    ・そのスレッドのセキュリティ設定 (圧縮スタック、Thread.Principal, Windowsの認証情報)
      //    ・ホスト設定 (HostExecutionContextManager)
      //    ・論理呼び出しコンテキストデータ (CallContext)
      // が紐付いている。
      // 
      // その中でも、論理呼び出しコンテキスト (CallContext) は、LogicalSetDataメソッド、LogicalGetDataメソッドを
      // 利用することにより、同じ実行コンテキストを持つスレッド間でデータを共有することができる。
      // 既定では、CLRは起動元のスレッドの実行コンテキストが自動的に伝播されるようにしてくれる。
      //
      // 実行コンテキストの伝播方法は、ExecutionContextクラスを利用することにより変更することができる。
      // ExecutionContext.SuppressFlowメソッドにはSerucityCriticalAttribute
      // が付与されているので、環境によっては動作しなくなる可能性がある。
      // (SerucityCriticalAttributeは、完全信頼を要求する属性)
      //
      var numberOfThreads = 5;
      
      using (var cde = new CountdownEvent(numberOfThreads))
      {
        //
        // メインスレッド上にて、論理呼び出しコンテキストデータを設定.
        //
        CallContext.LogicalSetData("Message", "Hello World");
        
        //
        // 既定の設定のまま (親元のExecutionContextをそのまま継承） で、別スレッド生成.
        //
        ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("First Thread", cde));
        
        //
        // 実行コンテキストの伝播方法を変更.
        //   SuppressFlowメソッドは、実行コンテキストフローを抑制するメソッド.
        // SuppressFlowメソッドは、AsyncFlowControlを戻り値として返却する。
        // 抑制した実行コンテキストを復元するには、AsyncFlowControl.Undoを呼び出す。
        //
        AsyncFlowControl flowControl = ExecutionContext.SuppressFlow();
        
        //
        // 抑制された実行コンテストの状態で、別スレッド生成.
        //
        ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("Second Thread", cde));
        
        //
        // 実行コンテキストを復元.
        //
        flowControl.Undo();
        
        //
        // 再度、別スレッド生成.
        //
        ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("Third Thread", cde));
        
        //
        // 再度、実行コンテキストを抑制し、抑制されている間に論理呼び出しコンテキストデータを変更し
        // その後、実行コンテキストを復元する.
        //
        flowControl = ExecutionContext.SuppressFlow();
        CallContext.LogicalSetData("Message", "Modified....");
        
        ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("Fourth Thread", cde));
        flowControl.Undo();
        ThreadPool.QueueUserWorkItem(ShowCallContextLogicalData, new ThreadData("Fifth Thread", cde));
        
        cde.Wait();
      }
    }
    
    void ShowCallContextLogicalData(object state)
    {
      var data = state as ThreadData;
      
      Console.WriteLine(
         "Thread: {0, -15}, Id: {1}, Message: {2}"
        ,data.Name
        ,Thread.CurrentThread.ManagedThreadId
        ,CallContext.LogicalGetData("Message")
      );
      
      data.Counter.Signal();
    }
    
    #region Inner Classes
    class ThreadData
    {
      public string         Name    { get; private set;}
      public CountdownEvent Counter { get; private set;}
      
      public ThreadData(string name, CountdownEvent cde)
      {
        Name    = name;
        Counter = cde;
      }
    }
    #endregion
  }
  #endregion
  
  #region IEquatableSamples-01
  /// <summary>
  /// IEquatable<T>のサンプルです。
  /// </summary> -->
  public class IEquatableSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // IEquatable<T>インターフェースは、2つのインスタンスが等しいか否かを判別するための
      // 型指定のEqualsメソッドを定義しているインターフェースである。
      //
      // このインターフェースを実装することで、通常のobject.Equals以外に型が指定された
      // Equalsメソッドを持つことができるようになる。
      // このインターフェースは、特に構造体を定義する上で重要であり、構造体の場合、object.Equalsで
      // 比較を行うと、ボックス化が発生してしまうため、IEquatable<T>を実装することが多い。
      // (ボックス化が発生しなくなるため。）
      //
      // また、厳密には必須ではないが、IEquatable<T>を実装する場合、同時に以下のメソッドもオーバーライドするのが普通である。
      //   ・object.Equals
      //   ・object.GetHashCode
      // object.Equalsをオーバーライドするのは、IEquatableを実装していてもクラスによっては、それを無視して強制的にobject.Equalsで
      // 比較する処理が存在するためである。
      //
      // IEquatable<T>インターフェースは、Dictionary<TKey, TValue>, List<T>などのジェネリックコレクションにて
      // Contains, IndexOf, LastIndexOf, Removeなどの各メソッドで等価性をテストする場合に利用される。
      // (ArrayのIndexOfメソッドなどでも同様に利用される。)
      // 同じインターフェースで、比較機能を提供するものとして、IComparable<T>インターフェースがある。
      //
      // object.GetHashCodeをオーバーライドするのは、上の理由によりobject.Equalsがオーバーライドされるため。
      //
      Data data1 = new Data(1, "Hello World");
      Data data2 = new Data(2, "Hello World2");
      Data data3 = new Data(3, "Hello World3");
      Data data4 = data3;
      Data data5 = new Data(1, "Hello World4");
      
      Console.WriteLine("data1 equals data2? ==> {0}", data1.Equals(data2));
      Console.WriteLine("data1 equals data3? ==> {0}", data1.Equals(data3));
      Console.WriteLine("data1 equals data4? ==> {0}", data1.Equals(data4));
      Console.WriteLine("data1 equals data5? ==> {0}", data1.Equals(data5));
      
      object d1 = data1;
      object d2 = data2;
      object d5 = data5;
      
      Console.WriteLine("data1 equals data2? ==> {0}", d1.Equals(d2));
      Console.WriteLine("data1 equals data5? ==> {0}", d1.Equals(d5));
      
      Data[] dataArray = { data1, data2, data3, data4, data5 };
      Console.WriteLine("IndexOf={0}", Array.IndexOf(dataArray, data3));
    }
    
    sealed class Data : IEquatable<Data>
    {
      public Data(int id, string name)
      {
        Id = id;
        Name = name;
      }
      
      public int Id
      {
        get;
        private set;
      }
      
      public string Name
      {
        get;
        private set;
      }
      
      // IEquatable<T>の実装.
      public bool Equals(Data other)
      {
        Console.WriteLine("\t→→Call IEquatable.Equals");
        
        if (other == null)
        {
          return false;
        }
        
        return Id == other.Id;
      }
      
      // object.Equals
      public override bool Equals(object other)
      {
        Console.WriteLine("\t→→Call object.Equals");
        
        Data data = other as Data;
        if (data == null)
        {
          return false;
        }
        
        return Equals(data);
      }
      
      // object.GetHashCode
      public override int GetHashCode()
      {
        return Id.GetHashCode();
      }
    }
  }
  #endregion
  
  #region EqualityComparerSamples-01
  public class EqualityComparerSamples01 : IExecutable
  {
    public void Execute()
    {
      var d1 = new Data("data1", "data1-value1");
      var d2 = new Data("data2", "data2-value1");
      var d3 = new Data("data3", "data3-value1");
      
      // d1と同じ値を持つ別のインスタンスを作成しておく.
      var d1_2 = new Data(d1.Name, d1.Value);
      
      /////////////////////////////////////////////////////////
      //
      // object.Equalsで比較.
      //
      Console.WriteLine("===== object.Equalsで比較. =====");
      Console.WriteLine("d1.Equals(d2) : {0}", d1.Equals(d2));
      Console.WriteLine("d1.Equals(d3) : {0}", d1.Equals(d3));
      Console.WriteLine("d1.Equals(d1_2) : {0}", d1.Equals(d1_2));
      
      /////////////////////////////////////////////////////////
      //
      // EqualityComparerで比較.
      //
      var comparer = new DataEqualityComparer();
      
      Console.WriteLine("===== EqualityComparerで比較. =====");
      Console.WriteLine("d1.Equals(d2) : {0}", comparer.Equals(d1, d2));
      Console.WriteLine("d1.Equals(d3) : {0}", comparer.Equals(d1, d3));
      Console.WriteLine("d1.Equals(d1_2) : {0}", comparer.Equals(d1, d1_2));
      
      /////////////////////////////////////////////////////////
      //
      // Dictionaryで一致するか否かを確認 (EqualityComparer無し)
      //
      var dict1 = new Dictionary<Data, string>();
      
      dict1[d1] = d1.Value;
      dict1[d2] = d2.Value;
      dict1[d3] = d3.Value;
      
      // 以下のコードでは、ちゃんと値が取得できる. (参照が同じため)
      Console.WriteLine("===== Dictionaryで一致するか否かを確認 (EqualityComparer無し). =====");
      Console.WriteLine("key:d1 ==> {0}", dict1[d1]);
      Console.WriteLine("key:d3 ==> {0}", dict1[d3]);
      
      // 以下のコードでは、ちゃんとtrueが取得できる. (参照が同じため)
      Console.WriteLine("contains-key: d1 ==> {0}", dict1.ContainsKey(d1));
      Console.WriteLine("contains-key: d2 ==> {0}", dict1.ContainsKey(d2));
      Console.WriteLine("contains-key: d3 ==> {0}", dict1.ContainsKey(d3));
      
      //
      // 同じ値を持つ、別インスタンスを作成し、EqualityComparerなしのDictionaryで試してみる.
      //
      var d4 = new Data(d1.Name, d1.Value);
      var d5 = new Data(d2.Name, d2.Value);
      var d6 = new Data(d3.Name, d3.Value);
      
      // 以下のコードを実行すると例外が発生する. (キーとして一致しないため)
      try
      {
        Console.WriteLine("===== 同じ値を持つ、別インスタンスを作成し、EqualityComparerなしのDictionaryで試してみる. =====");
        Console.WriteLine("key:d4 ==> {0}", dict1[d4]);
      }
      catch (KeyNotFoundException)
      {
        Console.WriteLine("キーとしてd4を指定しましたが、一致するキーが見つかりませんでした。");
      }
      
      // 当然、ContainsKeyメソッドもfalseを返す.
      Console.WriteLine("contains-key: d4 ==> {0}", dict1.ContainsKey(d4));
      
      
      /////////////////////////////////////////////////////////
      //
      // Dictionaryを作成する際に、EqualityComparerを指定して作成.
      //
      var dict2 = new Dictionary<Data, string>(comparer);
      
      dict2[d1] = d1.Value;
      dict2[d2] = d2.Value;
      dict2[d3] = d3.Value;

      // 以下のコードでは、ちゃんと値が取得できる. (EqualityComparerを指定しているため)
      Console.WriteLine("===== Dictionaryを作成する際に、EqualityComparerを指定して作成. =====");
      Console.WriteLine("key:d4 ==> {0}", dict2[d4]);
      Console.WriteLine("key:d6 ==> {0}", dict2[d6]);
      
      // 以下のコードでは、ちゃんとtrueが取得できる. (EqualityComparerを指定しているため)
      Console.WriteLine("contains-key: d4 ==> {0}", dict2.ContainsKey(d4));
      Console.WriteLine("contains-key: d5 ==> {0}", dict2.ContainsKey(d5));
      Console.WriteLine("contains-key: d6 ==> {0}", dict2.ContainsKey(d6));

      /////////////////////////////////////////////////////////
      //
      // EqualityComparer<T>には、Defaultという静的プロパティが存在する.
      // このプロパティは、Tに指定された型がIEquatable<T>を実装しているかどうかを
      // チェックし、実装している場合は、内部でIEquatable<T>の実装を利用する
      // EqualityComaparer<T>を作成して返してくれる.
      //
      // Tに指定された型が、IEquatable<T>を実装していない場合
      // object.Equals, object.GetHashCodeを利用する実装を返す.
      //
      // 本サンプルで利用するサンプルクラスは、以下のようになっている.
      //   Dataクラス： IEquatable<T>を実装していない.
      //   Data2クラス： IEquatable<T>を実装している.
      //
      // 上記のクラスに対して、それぞれEqualityComparer<T>.Defaultを呼び出すと以下の
      // クラスのインスタンスが返ってくる.
      //   Dataクラス：  ObjectEqualityComparer`1
      //   Data2クラス: GenericEqualityComparer`1
      // IEquatable<T>を実装している場合は、GenericEqualityComparerが
      // 実装していない場合は、ObjectEqualityComparerとなる。
      //
      var dataEqualityComparer  = EqualityComparer<Data>.Default;
      var data2EqualityComparer = EqualityComparer<Data2>.Default;
      
      // 生成された型を表示.
      Console.WriteLine("===== EqualityComparer<T>.Defaultの動作. =====");
      Console.WriteLine("Data={0}, Data2={1}", dataEqualityComparer.GetType().Name, data2EqualityComparer.GetType().Name);
      
      // それぞれサンプルデータを作成して、比較してみる.
      // 尚、どちらの場合も1番目のデータと3番目のデータのキーが同じになるようにしている.
      var data_1 = new Data("data_1", "value_1");
      var data_2 = new Data("data_2", "value_2");
      var data_3 = new Data("data_1", "value_3");
      
      var data2_1 = new Data2("data2_1", "value2_1");
      var data2_2 = new Data2("data2_2", "value2_2");
      var data2_3 = new Data2("data2_1", "value2_3");
      
      // DataクラスのEqualityComparerを使用して比較.
      Console.WriteLine("data_1.Equals(data_2) : {0}", dataEqualityComparer.Equals(data_1, data_2));
      Console.WriteLine("data_1.Equals(data_3) : {0}", dataEqualityComparer.Equals(data_1, data_3));
      
      // Data2クラスのEqualityComparerを使用して比較.
      Console.WriteLine("data2_1.Equals(data2_2) : {0}", data2EqualityComparer.Equals(data2_1, data2_2));
      Console.WriteLine("data2_1.Equals(data2_3) : {0}", data2EqualityComparer.Equals(data2_1, data2_3));
    }
    
    class Data
    {
      public Data(string name, string value)
      {
        Name  = name;
        Value = value;
      }
      
      public string Name
      {
        get;
        private set;
      }
      
      public string Value
      {
        get;
        private set;
      }
      
      public override string ToString()
      {
        return string.Format("Name={0}, Value={1}", Name, Value);
      }
    }
    
    class DataEqualityComparer : EqualityComparer<Data>
    {
      public override bool Equals(Data x, Data y)
      {
        if (x == null && y == null)
        {
          return true;
        }
        
        if (x == null || y == null)
        {
          return false;
        }
        
        return x.Name == y.Name;
      }
      
      public override int GetHashCode(Data x)
      {
        if (x == null || string.IsNullOrEmpty(x.Name))
        {
          return string.Empty.GetHashCode();
        }
        
        return x.Name.GetHashCode();
      }
    }
    
    class Data2 : IEquatable<Data2>
    {
      public Data2(string name, string value)
      {
        Name  = name;
        Value = value;
      }
      
      public string Name
      {
        get;
        private set;
      }
      
      public string Value
      {
        get;
        private set;
      }
      
      public bool Equals(Data2 other)
      {
        if (other == null)
        {
          return false;
        }
        
        return other.Name == Name;
      }
      
      public override bool Equals(object other)
      {
        Data2 data = other as Data2;
        if (data == null)
        {
          return false;
        }
        
        return Equals(data);
      }
      
      public override int GetHashCode()
      {
        return string.IsNullOrEmpty(Name) ? string.Empty.GetHashCode() : Name.GetHashCode();
      }
    }
  }
  #endregion
  
  #region ZipFileSamples-01
  /// <summary>
  /// System.IO.Compression.ZipFileクラスのサンプルです。
  /// </summary>
  /// <remarks>
  /// ZipFileクラスは、.NET Framework 4.5で追加されたクラスです。
  /// このクラスを利用するには、「System.IO.Compression.FileSystem.dll」を
  /// 参照設定に追加する必要があります。
  /// このクラスは、Metroアプリでは利用できません。
  /// Metroアプリでは、代わりにZipArchiveクラスを利用します。
  /// </remarks>
  public class ZipFileSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // ZipFileクラスは、ZIP形式のファイルを扱うためのクラスである。
      // 同じ事が出来るクラスとして、ZipArchiveクラスが存在するが
      // こちらは、きめ細かい処理が行えるクラスとなっており
      // ZipFileクラスは、ユーティリティクラスの扱いに近い。
      //
      // ZipFileクラスに定義されているメソッドは、全てstaticメソッドとなっている。
      //
      // 簡単に圧縮・解凍するためのメソッドとして
      //   ・CreateFromDirectory(string, string)
      //   ・ExtractToDirectory(string, string)
      // が用意されている。
      //
      // 尚、このクラスはMetroスタイルアプリ (新しい名前はWindows 8スタイルUI？)
      // では利用できないクラスである。Metroでは、ZipArchiveを利用することになる。
      // (http://msdn.microsoft.com/en-us/library/system.io.compression.zipfile)
      //
      
      //
      // 圧縮.
      //
      string srcDirectory = Environment.CurrentDirectory;
      string dstDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      string dstFilePath  = Path.Combine(dstDirectory, "ZipTest.zip");
      
      if (File.Exists(dstFilePath))
      {
        File.Delete(dstFilePath);
      }
      
      ZipFile.CreateFromDirectory(srcDirectory, dstFilePath);
      
      //
      // 解凍.
      //
      string extractDirectory = Path.Combine(dstDirectory, "ZipTest");
      if (Directory.Exists(extractDirectory))
      {
        Directory.Delete(extractDirectory, recursive: true);
        Directory.CreateDirectory(extractDirectory);
      }
      
      ZipFile.ExtractToDirectory(dstFilePath, extractDirectory);
    }
  }
  #endregion
  
  #region ZipFileSamples-02
  /// <summary>
  /// System.IO.Compression.ZipFileクラスのサンプルです。
  /// </summary>
  /// <remarks>
  /// ZipFileクラスは、.NET Framework 4.5で追加されたクラスです。
  /// このクラスを利用するには、「System.IO.Compression.FileSystem.dll」を
  /// 参照設定に追加する必要があります。
  /// このクラスは、Metroアプリでは利用できません。
  /// Metroアプリでは、代わりにZipArchiveクラスを利用します。
  /// </remarks>
  public class ZipFileSamples02 : IExecutable
  {
    string _zipFilePath;
    
    string DesktopPath
    {
      get
      {
        return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      }
    }
    
    public void Execute()
    {
      //
      // ZipFileクラスの以下のメソッドを利用すると、既存のZIPファイルを開く事が出来る。
      //   ・OpenRead
      //   ・Open(string, ZipArchiveMode)
      //   ・Open(string, ZipArchiveMode, Encoding)
      // どのメソッドも、戻り値としてZipArchiveクラスのインスタンスを返す。
      // 実際にZIPファイル内のエントリ取得は、ZipArchiveから行う.
      // ZipArchiveクラスは、IDisposableを実装しているのでusingブロックで
      // 利用するのが好ましい。
      //
      // 尚、ZipArchiveクラスを利用する場合、参照設定に
      //   System.IO.Compression.dll
      // を追加する必要がある。
      //
      Prepare();
      
      //
      // OpenRead
      //
      using (var archive = ZipFile.OpenRead(_zipFilePath))
      {
        archive.Entries.ToList().ForEach(PrintEntry);
      }
      
      //
      // Open(string, ZipArchiveMode)
      //
      using (var archive = ZipFile.Open(_zipFilePath, ZipArchiveMode.Read))
      {
        //
        // ZipArchive.Entriesプロパティからは、ReadOnlyCollection<ZipArchiveEntry>が取得できる。
        // 1エントリの情報は、ZipArchiveEntryから取得できる。
        //
        // ZipArchiveEntryには、Nameというプロパティが存在し、このプロパティから実際のファイル名を取得できる。
        // また、Lengthプロパティより圧縮前のファイルサイズが取得できる。圧縮後のサイズは、CompressedLengthから取得できる。
        // エントリの内容を読み出すには、ZipArchiveEntry.Openメソッドを利用する。
        //
        archive.Entries.ToList().ForEach(PrintEntry);
      }
      
      //
      // Open(string, ZipArchiveMode, Encoding)
      //   テキストファイルのみ、中身を読み出して出力.
      //
      using (var archive = ZipFile.Open(_zipFilePath, ZipArchiveMode.Read, Encoding.GetEncoding("sjis")))
      {
        archive.Entries.Where(entry => entry.Name.EndsWith("txt")).ToList().ForEach(PrintEntryContents);
      }
      
      File.Delete(_zipFilePath);
      Directory.Delete(Path.Combine(DesktopPath, "ZipTest"), recursive: true);
    }
    
    void Prepare()
    {
      //
      // サンプルZIPファイルを作成しておく.
      // (デスクトップ上にZipTest.zipという名称で出力される)
      //
      new ZipFileSamples01().Execute();
      _zipFilePath = Path.Combine(DesktopPath, "ZipTest.zip");
    }
    
    void PrintEntry(ZipArchiveEntry entry)
    {
      Console.WriteLine("[{0}, {1}]", entry.Name, entry.Length);
    }
    
    void PrintEntryContents(ZipArchiveEntry entry)
    {
      using (var reader = new StreamReader(entry.Open(), Encoding.GetEncoding("sjis")))
      {
        for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
        {
          Console.WriteLine(line);
        }
      }
    }
  }
  #endregion
  
  #region ZipFileSamples-03
  /// <summary>
  /// System.IO.Compression.ZipFileクラスのサンプルです。
  /// </summary>
  /// <remarks>
  /// ZipFileクラスは、.NET Framework 4.5で追加されたクラスです。
  /// このクラスを利用するには、「System.IO.Compression.FileSystem.dll」を
  /// 参照設定に追加する必要があります。
  /// このクラスは、Metroアプリでは利用できません。
  /// Metroアプリでは、代わりにZipArchiveクラスを利用します。
  ///
  /// 尚、ZipArchiveクラスを利用する場合
  ///   System.IO.Compression.dll
  /// を参照設定に追加する必要があります。
  /// </remarks>
  public class ZipFileSamples03 : IExecutable
  {
    string _zipFilePath;
    
    string DesktopPath
    {
      get
      {
        return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      }
    }
    
    public void Execute()
    {
      //
      // ZIPファイルの作成および更新.
      //   作成および更新の場合、ZipArchiveクラスを利用する.
      // 
      // ・エントリの追加： ZipArchive.CreateEntryFromFile OR ZipArchive.CreateEntry
      //
      // CreateEntryFromFileは、メソッドの名前が示す通り元ファイルがある場合に利用する。
      // 元となるファイルが存在する場合はこれが楽である。
      //
      // CreateEntryは、エントリのみを新規作成するメソッド。データは自前で流し込む必要がある。
      //
      Prepare();
      
      //
      // Zipファイルを新規作成.
      //
      using (var archive = ZipFile.Open(_zipFilePath, ZipArchiveMode.Create))
      {
        //
        // 元ファイルが存在している場合は、CreateEntryFromFileを利用するのが楽.
        //
        archive.CreateEntryFromFile("Persons.txt", "Persons.txt");
      }
      
      //
      // Zipファイルの内容を更新.
      //
      using (var archive = ZipFile.Open(_zipFilePath, ZipArchiveMode.Update))
      {
        //
        // 元ファイルは存在するが、今度はCreateEntryメソッドで新規エントリのみを作成しデータは、手動で流し込む.
        //
        using (var reader = new BinaryReader(File.Open("database.png", FileMode.Open)))
        {
          var newEntry = archive.CreateEntry("database.png");
          using (var writer = new BinaryWriter(newEntry.Open()))
          {
            WriteAllBytes(reader, writer);
          }
        }
      }
      
      File.Delete(_zipFilePath);
    }
    
    void Prepare()
    {
      _zipFilePath = Path.Combine(DesktopPath, "ZipTest2.zip");
      if (File.Exists(_zipFilePath))
      {
        File.Delete(_zipFilePath);
      }
    }
    
    void WriteAllBytes(BinaryReader reader, BinaryWriter writer)
    {
      try
      {
        for (;;)
        {
          writer.Write(reader.ReadByte());
        }
      }
      catch (EndOfStreamException)
      {
        writer.Flush();
      }
    }
  }
  #endregion
  
  #region ProgressSamples-01
  /// <summary>
  /// System.Progress<T>のサンプルです。
  /// </summary>
  /// <remarks>
  /// このクラスは、.NET Framework 4.5から追加された型です。
  /// </remarks>
  public class ProgressSamples01 : IExecutable
  {
    /// <summary>
    /// サンプル用ウィンドウ
    /// </summary>
    /// <remarks>
    /// このウィンドウには、ProgressBarが一つだけ配置されています。
    /// </remarks>
    class SampleWindow : Window
    {
      const double MIN = 0.0;
      const double MAX = 100.0;
      
      ProgressBar _bar;
      
      public SampleWindow()
      {
        InitializeControl();
        InitializeEvent();
      }
      
      void InitializeControl()
      {
        Width  = 400;
        Height = 80;
        
        _bar = new ProgressBar
               {
                  Minimum = MIN
                 ,Maximum = MAX
                 ,Value   = MIN
                 ,SmallChange = MIN
                };
        
        Content = _bar;
      }
      
      void InitializeEvent()
      {
        //
        // ロードイベント.
        //   やってることは、単純にプログレスバーの進捗を伸ばしていくだけ.
        //   進捗を伸ばす部分に、Progress<T>を利用している.
        //
        //   内部でawaitを使用しているのでラムダにasyncを指定.
        //
        Loaded += async (s, e) =>
        {
          //
          // .NET Framework 4.5より、CancellationTokenSourceのコンストラクタに
          // キャンセル状態になるまでのタイムアウト値を設定できるようになった。
          // 下記の場合だと、5秒後に自動的にキャンセル扱いになる.
          //
          var tokenSource = new CancellationTokenSource(5000);
          
          //
          // プログレスバーの進捗を伸ばすためのProgress<T>を構築.
          // コンストラクタにActionを渡しても、ProgressChangedイベントにハンドラを設定してもどちらでも良い。
          //
          // Progress<T>は、インスタンス生成時に現在のSynchronizationContextをキャプチャする。
          // なので、UIが絡む処理を行う場合は、必ずUIスレッド上でインスタンスを生成する必要がある。
          //
          var progress = new Progress<int>(SetProgress);
          // わざと、UIスレッドではない場所でインスタンスを生成している.
          // これを下のPerformStepメソッドに渡すと、SetProgressメソッドに入ってきた時点で
          // Control.InvokeRequiredがtrueとなる。つまり、UIスレッド以外からSetProgressが
          // 呼ばれているので、Invokeしないといけない状態となる。
          //var progress2 = Task.Run(() => new Progress<int>(SetProgress)).Result;

          //
          // 処理開始.
          //   awaitを指定しているので、処理はUIスレッドと切り離されて実行され
          //   プログレスバーの進捗設定の時のみUIスレッドで実行される. (IProgress<T>.ReportからのSetProgress呼び出し)
          //   await後のタイトル設定は、PerformStepメソッドが終了次第実行される。
          //
          // 第二引数に渡しているprogressを、progress2に変更して実行すると
          // 画面上のプログレスバーの進捗が一切進まなくなる。
          // これは、progress2の方が、UIスレッドではない場所で生成されたため
          // キャプチャされたSynchronizationContextがDispatcherSynchronizationContextでは無いためである。
          //
          await PerformStep(tokenSource.Token, progress);
          Title = "DONE.";
        };
      }
      
      async Task PerformStep(CancellationToken token, IProgress<int> progress)
      {
        for (;;)
        {
          foreach (var value in Enumerable.Range(0, 100))
          {
            if (token.IsCancellationRequested)
            {
              return;
            }
            
            await Task.Delay(10);
            
            //
            // Reportメソッドを呼び出すには
            // IProgress<T>にキャストして利用する必要がある。
            // (Progress<T>にて、明示的インターフェース実装されているため）
            //
            progress.Report(value);
          }
        }
      }
      
      void SetProgress(int newValue)
      {
        //
        // UIスレッドで実行されていない場合を視覚的に見たいので
        // UIスレッドで実行されていない場合はわざと何もしない.
        //
        if (!_bar.Dispatcher.CheckAccess())
        {
          return;
        }
        
        _bar.Value = newValue;
      }
    }
    
    [STAThread]
    public void Execute()
    {
      //
      // Progress<T>は、.NET Framework 4.5で追加された型である。
      // Progress<T>は、IProgress<T>インターフェースを実装しており
      // 文字通り、進捗状況を処理するために存在する。
      //
      // 利用する場合、コンストラクタにAction<T>を指定するか
      // ProgressChangedイベントをハンドルするかで処理できる。
      //
      // 尚、Progress<T>はインスタンスが作成される際に
      // 現在のSynchronizationContextをキャプチャし
      // ProgressChangedイベントをキャプチャしたSynchronizationContext上で
      // 実行してくれるため、イベントハンドラ内でコントロールを操作しても問題無い。
      // (インスタンスの作成自体をイベントスレッド以外で行っている場合は別）
      //
      // コンソールアプリのようにSynchronizationContextが紐づかない
      // コンテキストの場合は、ThreadPool上で実行される。
      //
      var app = new Application();
      app.Run(new SampleWindow());
    }
  }
  #endregion
  
  #region ProgressSamples-02
  /// <summary>
  /// System.Progress<T>のサンプルです。
  /// </summary>
  /// <remarks>
  /// このクラスは、.NET Framework 4.5から追加された型です。
  /// </remarks>
  public class ProgressSamples02 : IExecutable
  {
    class SampleForm : WinFormsForm
    {
      const int MIN = 0;
      const int MAX = 100;
      
      WinFormsLabel _label;
      WinFormsProgressBar   _bar;
      WinFormsButton        _btn;
      
      public SampleForm()
      {
        InitializeControl();
        InitializeEvent();
      }
      
      void InitializeControl()
      {
        SuspendLayout();
        
        Width  = 400;
        Height = 130;
        
        _label = new WinFormsLabel
        {
           Text     = string.Empty
          ,AutoSize = false
          ,Width    = 350
        };
        
        _bar = new WinFormsProgressBar
        {
           Minimum = MIN
          ,Maximum = MAX
          ,Width   = 350
          ,Value   = MIN
          ,Step    = 1
          ,Style   = WinFormsProgressBarStyle.Continuous
        };
        
        _btn = new WinFormsButton
        {
           Text  = "Cancel"
          ,Width = 120
        };
        
        var panel = new WinFormsFlowLayoutPanel
        {
           FlowDirection = WinFormsFlowDirection.TopDown
          ,Dock          = WinFormsDockStyle.Fill
        };
        
        panel.Controls.Add(_label);
        panel.Controls.Add(_bar);
        panel.Controls.Add(_btn);
        
        Controls.Add(panel);
        
        ResumeLayout();
      }
      
      void InitializeEvent()
      {
        Load += async (s, e) =>
        {
          var tokenSource = new CancellationTokenSource();
          var progress    = new Progress<ProgressMessage>(SetProgress);
          
          _btn.Tag     = tokenSource;
          _bar.Maximum = Directory.EnumerateFiles(".").Count();
          
          await Compress(tokenSource.Token, progress);
          
          Text = "DONE";
          if (tokenSource.IsCancellationRequested)
          {
            Text = "CANCEL";
          }
        };
        
        _btn.Click += (s, e) =>
        {
          (_btn.Tag as CancellationTokenSource).Cancel();
        };
      }
      
      async Task Compress(CancellationToken token, IProgress<ProgressMessage> progress)
      {
        string ZipFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ZipTest3.zip");
        string TmpFilePath = ZipFilePath + ".tmp";
        
        if (File.Exists(ZipFilePath))
        {
          File.Move(ZipFilePath, TmpFilePath);
        }
        
        using (var archive = ZipFile.Open(ZipFilePath, ZipArchiveMode.Create))
        {
          foreach (var filePath in Directory.EnumerateFiles("."))
          {
            if (token.IsCancellationRequested)
            {
              break;
            }
            
            progress.Report(new BeginMessage { Message = string.Format("{0}を圧縮しています...", filePath), Token = token });
            
            archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
            await Task.Delay(1000);
            
            progress.Report(new AfterMessage { Message = string.Format("{0}を圧縮完了", filePath), Token = token });
          }
        }
        
        if (token.IsCancellationRequested)
        {
          File.Delete(ZipFilePath);
          
          if (File.Exists(TmpFilePath))
          {
            File.Move(TmpFilePath, ZipFilePath);
          }
        }
        else
        {
          if (File.Exists(TmpFilePath))
          {
            File.Delete(TmpFilePath);
          }
        }
      }
      
      void SetProgress(ProgressMessage message)
      {
        if (message.Token.IsCancellationRequested)
        {
          _label.Text = "処理はキャンセルされました。";
          return;
        }
        
        _label.Text = message.Message;
        if (message is AfterMessage)
        {
          _bar.PerformStep();
        }
      }
      
      class ProgressMessage
      {
        public string Message
        {
          get;
          set;
        }
        
        public CancellationToken Token
        {
          get;
          set;
        }
      }
      
      class BeginMessage : ProgressMessage {}
      class AfterMessage : ProgressMessage {}
    }

    [STAThread]
    public void Execute()
    {
      WinFormsApplication.EnableVisualStyles();
      WinFormsApplication.Run(new SampleForm());
    }
  }
  #endregion
  
  #region ProgressSamples-03
  /// <summary>
  /// System.Progress<T>のサンプルです。
  /// </summary>
  /// <remarks>
  /// このクラスは、.NET Framework 4.5から追加された型です。
  /// </remarks>
  public class ProgressSamples03 : IExecutable
  {
    class SampleWindow : Window
    {
      TextBlock   _label;
      ProgressBar _bar;
      Button      _btn;
      
      public SampleWindow()
      {
        InitializeControl();
        InitializeEvent();
      }
      
      void InitializeControl()
      {
        Width  = 400;
        Height = 100;
        
        _label = new TextBlock
        {
          Text = string.Empty
        };
        
        _bar = new ProgressBar
        {
           Height = 20
          ,Minimum = 0
        };
        
        _btn = new Button
        {
           Content = "Cancel"
          ,Margin  = new Thickness(300, 0, 0, 0)
        };
        
        var panel = new StackPanel();
        
        panel.Children.Add(_label);
        panel.Children.Add(_bar);
        panel.Children.Add(_btn);
        
        Content = panel;
      }
      
      void InitializeEvent()
      {
        Loaded += async (s, e) =>
        {
          var tokenSource = new CancellationTokenSource();
          var progress    = new Progress<ProgressMessage>(SetProgress);
          
          _btn.Tag     = tokenSource;
          _bar.Maximum = Directory.EnumerateFiles(".").Count();
          
          await Compress(tokenSource.Token, progress);
          
          Title = "DONE";
          if (tokenSource.IsCancellationRequested)
          {
            Title = "CANCEL";
          }
        };
        
        _btn.Click += (s, e) =>
        {
          (_btn.Tag as CancellationTokenSource).Cancel();
        };
      }
      
      async Task Compress(CancellationToken token, IProgress<ProgressMessage> progress)
      {
        string ZipFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ZipTest3.zip");
        string TmpFilePath = ZipFilePath + ".tmp";
        
        if (File.Exists(ZipFilePath))
        {
          File.Move(ZipFilePath, TmpFilePath);
        }
        
        using (var archive = ZipFile.Open(ZipFilePath, ZipArchiveMode.Create))
        {
          foreach (var filePath in Directory.EnumerateFiles("."))
          {
            if (token.IsCancellationRequested)
            {
              break;
            }
            
            progress.Report(new BeginMessage { Message = string.Format("{0}を圧縮しています...", filePath), Token = token });
            
            archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
            await Task.Delay(1000);
            
            progress.Report(new AfterMessage { Message = string.Format("{0}を圧縮完了", filePath), Token = token });
          }
        }
        
        if (token.IsCancellationRequested)
        {
          File.Delete(ZipFilePath);
          if (File.Exists(TmpFilePath))
          {
            File.Move(TmpFilePath, ZipFilePath);
          }
        }
        else
        {
          if (File.Exists(TmpFilePath))
          {
            File.Delete(TmpFilePath);
          }
        }
      }
      
      void SetProgress(ProgressMessage message)
      {
        if (message.Token.IsCancellationRequested)
        {
          _label.Text = "処理はキャンセルされました。";
          return;
        }
        
        _label.Text = message.Message;
        if (message is AfterMessage)
        {
          _bar.Value++;
        }
      }
      
      class ProgressMessage
      {
        public string Message
        {
          get;
          set;
        }
        
        public CancellationToken Token
        {
          get;
          set;
        }
      }
      
      class BeginMessage : ProgressMessage {}
      class AfterMessage : ProgressMessage {}
    }
    
    public void Execute()
    {
      var app = new Application();
      app.Run(new SampleWindow());
    }
  }
  #endregion
  
  #region ListForEachDiffSamples-01
  /// <summary>
  /// Listをforeachでループする場合と、List.ForEachする場合の速度差をテスト
  /// </summary>
  public class ListForEachDiffSamples01 : IExecutable
  {
    public void Execute()
    {
      Prepare();
      
      //
      // Listをforeachで処理するか、List.ForEachで処理するかで
      // どちらの方が速いのかを計測。
      //
      // ILレベルで見ると
      //   foreachの場合： callが2つ
      //   List.ForEachの場合： callvirtが1つ
      // となる。
      //
      foreach (var elementCount in new []{1000, 3000, 5000, 10000, 50000, 100000, 150000, 500000, 700000, 1000000})
      {
        Console.WriteLine("===== [Count:{0}] =====", elementCount);
        
        var theList = new List<int>(Enumerable.Range(1, elementCount));

        var watch = Stopwatch.StartNew();
        Sum_foreach(theList);
        watch.Stop();
        Console.WriteLine("foreach:      {0}", watch.Elapsed);

        watch = Stopwatch.StartNew();
        Sum_List_ForEach(theList);
        watch.Stop();
        Console.WriteLine("List.ForEach: {0}", watch.Elapsed);
      }
    }
    
    void Prepare()
    {
      int result = 0;
      foreach (var x in new List<int>(Enumerable.Range(1, 1000)))
      {
        result += x;
      }
      
      result = 0;
      new List<int>(Enumerable.Range(1, 1000)).ForEach(x => result += x);
    }
    
    int Sum_foreach(List<int> theList)
    {
      int result = 0;
      foreach (var x in theList)
      {
        result += x;
      }
      return result;
    }
    
    int Sum_List_ForEach(List<int> theList)
    {
      int result = 0;
      theList.ForEach(x => result += x);
      return result;
    }
  }
  #endregion
  
  #region DisposableSamples-01
  /// <summary>
  /// IDisposableのサンプルです。
  /// </summary>
  /// <remarks>
  /// 以下の記事を見て作成したサンプル。
  ///   http://www.codeproject.com/Tips/458846/Using-using-Statements-DisposalAccumulator
  /// </remarks>
  public class DisposableSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // 通常パターン.
      //
      using (var disposable1 = new Disposable1())
      {
        using (var disposable2 = new Disposable2())
        {
          using (var disposable3 = new Disposable3())
          {
            Console.WriteLine("Dispose Start..");
          }
        }
      }
      
      //
      // 通常パターン: DisposableManager利用.
      //
      using (var manager = new DisposableManager())
      {
        var d1 = manager.Add(new Disposable1());
        var d2 = manager.Add(new Disposable2());
        var d3 = manager.Add(new Disposable3());
        
        Console.WriteLine("Dispose Start..");
      }
      
      //
      // 条件が存在し、作成されないオブジェクトが存在する可能性がある場合.
      //
      Disposable1 dispose1 = null;
      Disposable2 dispose2 = null;
      Disposable3 dispose3 = null;
      
      bool isDispose2Create = false;
      try
      {
        dispose1 = new Disposable1();
        
        if (isDispose2Create)
        {
          dispose2 = new Disposable2();
        }
        
        dispose3 = new Disposable3();
      }
      finally
      {
        Console.WriteLine("Dispose Start..");
        DisposeIfNotNull(dispose1);
        DisposeIfNotNull(dispose2);
        DisposeIfNotNull(dispose3);
      }
      
      
      //
      // 条件あり: DisposableManager利用.
      //
      dispose1 = null;
      dispose2 = null;
      dispose3 = null;
      
      using (var manager = new DisposableManager())
      {
        dispose1 = manager.Add(new Disposable1());
        
        if (isDispose2Create)
        {
          dispose2 = manager.Add(new Disposable2());
        }
        
        dispose3 = manager.Add(new Disposable3());
        
        Console.WriteLine("Dispose Start..");
      }
    }
    
    void DisposeIfNotNull(IDisposable disposableObject)
    {
      if (disposableObject == null)
      {
        return;
      }
      
      disposableObject.Dispose();
    }
    
    class DisposableManager : IDisposable
    {
      Stack<IDisposable> _disposables;
      bool               _isDisposed;
      
      public DisposableManager()
      {
        _disposables = new Stack<IDisposable>();
        _isDisposed  = false;
      }
      
      public T Add<T>(T disposableObject) where T : IDisposable
      {
        Defence();
        
        if (disposableObject != null)
        {
          _disposables.Push(disposableObject);
        }
        
        return disposableObject;
      }
      
      public void Dispose()
      {
        _disposables.ToList().ForEach(disposable => disposable.Dispose());
        _disposables.Clear();
        
        _isDisposed = true;
      }
      
      void Defence()
      {
        if (_isDisposed)
        {
          throw new ObjectDisposedException("Cannot access a disposed object.");
        }
      }
    }
    
    class Base : IDisposable
    {
      public void Dispose()
      {
        Console.WriteLine("[{0}] Disposed...", GetType().Name);
      }
    }
    
    class Disposable1 : Base {}
    class Disposable2 : Base {}
    class Disposable3 : Base {}
    
  }
  #endregion
  
  #region MulticoreJITSamples-01
  /// <summary>
  /// マルチコアJITのサンプルです.
  /// </summary>
  public class MulticoreJITSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // .NET 4.5よりマルチコアJITが搭載されている.
      // 文字通り、マルチコア構成の環境にて並列でJITを行う機能である。
      // これにより、アプリケーションの動きに先行して、必要となるメソッドのJITが
      // 行われる可能性が高くなり、結果的にアプリケーションのパフォーマンスが上がるとのこと。
      //
      // マルチコアJITは、ASP.NET 4.5とSilverlight5では
      // 既定で有効となっているが、デスクトップアプリケーションでは
      // デフォルトで有効になっていない。
      //
      // 有効になっていない理由は、この機能を利用するためには
      // プロファイリング処理が必須であり、プロファイルデータを保存
      // することが条件であるため。デスクトップアプリケーションでは
      // フレームワーク側が、プロファイルデータをどこに保存するべきなのかを
      // 判断できないため、手動で実行するようになっている。
      //
      // 参考URL:
      //  http://blogs.msdn.com/b/dotnet/archive/2012/10/18/an-easy-solution-for-improving-app-launch-performance.aspx
      //  http://stackoverflow.com/questions/12965606/why-is-multicore-jit-not-on-by-default-in-net-4-5
      //  http://msdn.microsoft.com/ja-jp/magazine/hh882452.aspx
      //
      // マルチコアJITを有効にするには、System.Runtime.ProfileOptimizationクラスの
      // 以下のstaticメソッドを呼び出すだけである。
      //   ・SetProfileRoot
      //   ・StartProfile
      // 上記メソッドは、アプリケーションのエントリポイントで呼び出す方がよい。
      //
      
      //
      // マルチコアJITを有効にする.
      //  プロファイルデータ格納場所は、アプリ実行フォルダ.
      //  プロファイルデータのファイル名は、App.JIT.Profileとする。
      //
      ProfileOptimization.SetProfileRoot(Environment.CurrentDirectory);
      ProfileOptimization.StartProfile("App.JIT.Profile");
    }
  }
  #endregion
  
  #region MyTest
  public class MyTmpTest : IExecutable
  {
    public void Execute()
    {
    }
  }
  #endregion
}
namespace Gsf.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.ServiceModel.Syndication;
  using System.Text;
  using System.Xml;

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
}

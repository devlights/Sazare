namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Drawing;
  using System.Linq;

  using GDIImage = System.Drawing.Image;

  #region ImageConverterSamples-01
  /// <summary>
  /// ImageConverterクラスのサンプルです。
  /// </summary>
  [Sample]
  public class ImageConverterSamples01 : IExecutable
  {
    public void Execute()
    {
      //
      // Imageオブジェクトを取得.
      //
      GDIImage image = GDIImage.FromFile("resources/database.png");

      //
      // Imageをバイト配列に変換.
      //   Imageから別のオブジェクトに変換する場合はConvertToを利用する.
      //
      ImageConverter converter = new ImageConverter();
      byte[] imageBytes = (byte[])converter.ConvertTo(image, typeof(byte[]));

      //
      // バイト配列をImageに変換.
      //   バイト配列からImageオブジェクトに変換する場合はConvertFromを利用する.
      //
      GDIImage image2 = (GDIImage)converter.ConvertFrom(imageBytes);

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
}

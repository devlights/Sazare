namespace Sazare.Samples
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  #region GetInvalidPathCharsAndGetInvalidFileNameCharsSamples-01
  /// <summary>
  /// PathクラスのGetInvalidPathCharsメソッドとGetInvalidFileNameCharsメソッドのサンプルです。
  /// </summary>
  [Sample]
  public class GetInvalidPathCharsAndGetInvalidFileNameCharsSamples01 : Sazare.Common.IExecutable
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
      char[] invalidPathChars = Path.GetInvalidPathChars();
      char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

      string tmpPath = @"c:usrlocaltmp_<path>_tmp";
      string tmpFileName = @"tmp_<filename>_tmp.|||";

      Console.WriteLine("不正なパス文字が存在してる？     = {0}", invalidPathChars.Any(ch => tmpPath.Contains(ch)));
      Console.WriteLine("不正なファイル名文字が存在してる？ = {0}", invalidFileNameChars.Any(ch => tmpFileName.Contains(ch)));
    }
  }
  #endregion
}

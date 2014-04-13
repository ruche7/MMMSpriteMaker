using MMMSpriteMaker.Properties;
using System;
using System.IO;
using System.Windows.Markup;
using System.Xml;

namespace MMMSpriteMaker
{
    /// <summary>
    /// アプリケーション設定に関する処理を行う静的ユーティリティクラス。
    /// </summary>
    public static class SettingsUtil
    {
        /// <summary>
        /// アプリケーション設定をXAMLファイルへ書き出す。
        /// </summary>
        /// <param name="self">アプリケーション設定。</param>
        /// <param name="filePath">出力先ファイルパス。</param>
        /// <remarks>
        /// ApplicationSettingsBase クラス標準の Save メソッドは即時保存ではなく、
        /// 呼び出し直後にアプリケーションを終了させると正常に保存されない。
        /// 
        /// このメソッドは呼び出したタイミングで書き出しを完了させる。
        /// </remarks>
        public static void SaveToXaml(this Settings self, string filePath)
        {
            // 親ディレクトリ作成
            var parentDirPath = Path.GetDirectoryName(Path.GetFullPath(filePath));
            if (!Directory.Exists(parentDirPath))
            {
                Directory.CreateDirectory(parentDirPath);
            }

            // XAMLファイル書き出し
            var xmlSettings = new XmlWriterSettings { Indent = true };
            using (var xmlWriter = XmlWriter.Create(filePath, xmlSettings))
            {
                XamlWriter.Save(self, xmlWriter);
            }
        }

        /// <summary>
        /// XAMLファイルからアプリケーション設定を読み取る。
        /// </summary>
        /// <param name="filePath">入力元ファイルパス。</param>
        /// <returns>アプリケーション設定。読み取れなかった場合は null 。</returns>
        public static Settings LoadFromXaml(string filePath)
        {
            // ファイルが存在しないなら読み取れないので null を返す
            if (!File.Exists(filePath))
            {
                return null;
            }

            // 読み取り試行
            try
            {
                using (var stream = File.OpenRead(filePath))
                {
                    return XamlReader.Load(stream) as Settings;
                }
            }
            catch { }

            return null;
        }
    }
}

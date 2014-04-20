using ruche.mmm.tools.spriteMaker.resources;
using System;
using System.IO;
using System.Text;
using System.Windows.Media;

namespace ruche.mmm.tools.spriteMaker
{
    /// <summary>
    /// アクセサリファイルを作成するクラス。
    /// </summary>
    public class AccessoryFileMaker
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public AccessoryFileMaker()
        {
            Config = new AccessoryFileConfig();
        }

        /// <summary>
        /// アクセサリファイル設定を取得または設定する。
        /// </summary>
        public AccessoryFileConfig Config { get; set; }

        /// <summary>
        /// テクスチャファイル名を取得または設定する。
        /// </summary>
        /// <remarks>
        /// ここで指定した文字列がそのままアクセサリファイルに書き出される。
        /// </remarks>
        [TemplateReplaceId]
        public string TextureFileName { get; set; }

        /// <summary>
        /// アクセサリファイルを作成する。
        /// </summary>
        /// <param name="filePath">ファイルパス。</param>
        /// <remarks>
        /// 事前に下記のプロパティに有効な値を設定しておく必要がある。
        /// 
        /// <list type="bullet">
        /// <item><description>Config</description></item>
        /// <item><description>TextureFileName</description></item>
        /// </list>
        /// </remarks>
        public void Make(string filePath)
        {
            if (!MakerUtil.IsValidPath(filePath))
            {
                throw new ArgumentException("Invalid file path.", "filePath");
            }
            if (Config == null)
            {
                throw new InvalidOperationException("Config is null.");
            }
            if (!MakerUtil.IsValidPath(TextureFileName))
            {
                throw new InvalidOperationException("TextureFileName is invalid.");
            }

            // 雛形文字列取得
            var code = Resources.SpriteAccessory;

            // 置換
            code = TemplateReplaceIdAttribute.Replace(this, code);

            // 書き出し
            File.WriteAllText(filePath, code, Encoding.GetEncoding(932));
        }

        #region 文字列置換用プロパティ

        /// <summary>
        /// アクセサリファイル上で float 値を表す文字列を作成する。
        /// </summary>
        /// <param name="value">float 値。</param>
        /// <returns>float 値を表す文字列。</returns>
        private static string MakeFloatString(float value)
        {
            return string.Format("{0:0.000000}", value);
        }

        private static string MakeColorString(Color color, bool withAlpha)
        {
            return
                MakeFloatString(color.ScR) + ";" +
                MakeFloatString(color.ScG) + ";" +
                MakeFloatString(color.ScB) + ";" +
                (withAlpha ? (MakeFloatString(color.ScA) + ";") : "");
        }

        [TemplateReplaceId]
        private string MaterialFaceColor
        {
            get { return MakeColorString(Config.FaceColor, true); }
        }

        [TemplateReplaceId]
        private string MaterialSpecularPower
        {
            get { return MakeFloatString(Config.SpecularPower); }
        }

        [TemplateReplaceId]
        private string MaterialSpecularColor
        {
            get { return MakeColorString(Config.SpecularColor, false); }
        }

        [TemplateReplaceId]
        private string MaterialEmissiveColor
        {
            get { return MakeColorString(Config.EmissiveColor, false); }
        }

        #endregion
    }
}

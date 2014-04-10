using System;
using System.IO;
using System.Text;
using ruche.mmm.tools.spriteMaker.resources;

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
        public AccessoryFileMaker() { }

        /// <summary>
        /// テクスチャファイル名を取得または設定する。
        /// </summary>
        [TemplateReplaceId]
        public string TextureFileName { get; set; }

        /// <summary>
        /// アクセサリファイルを作成する。
        /// </summary>
        /// <param name="filePath">ファイルパス。</param>
        public void Make(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Invalid file path.", "filePath");
            }
            if (string.IsNullOrWhiteSpace(TextureFileName))
            {
                throw new InvalidOperationException("Invalid texture file name.");
            }

            // 雛形文字列取得
            var code = Resources.SpriteAccessory;

            // 置換
            code = TemplateReplaceIdAttribute.Replace(this, code);

            // 書き出し
            File.WriteAllText(filePath, code, Encoding.GetEncoding(932));
        }
    }
}

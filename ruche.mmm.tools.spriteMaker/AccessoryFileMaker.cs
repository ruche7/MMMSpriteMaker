using ruche.mmm.tools.spriteMaker.resources;
using System;
using System.IO;
using System.Text;

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
        /// 事前に TextureFileName に有効な文字列を設定しておく必要がある。
        /// </remarks>
        public void Make(string filePath)
        {
            if (!MakerUtil.IsValidPath(filePath))
            {
                throw new ArgumentException("Invalid file path.", "filePath");
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
    }
}

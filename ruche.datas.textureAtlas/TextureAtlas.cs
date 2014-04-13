using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ruche.datas.textureAtlas
{
    /// <summary>
    /// テクスチャアトラスを定義するクラス。
    /// </summary>
    public class TextureAtlas
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="imageFileName">画像ファイル名。</param>
        /// <param name="imageSize">画像サイズ。</param>
        /// <param name="frames">フレーム列挙。</param>
        public TextureAtlas(
            string imageFileName,
            Size imageSize,
            IEnumerable<TextureAtlasFrame> frames)
        {
            // 引数チェック
            if (imageFileName == null)
            {
                throw new ArgumentNullException("imageFileName");
            }
            if (imageSize.Width <= 0 || imageSize.Height <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    "imageSize",
                    imageSize,
                    "Invalid image size.");
            }
            if (frames == null)
            {
                throw new ArgumentNullException("frames");
            }
            var frms = frames.ToList().AsReadOnly();
            if (frms.Count <= 0)
            {
                throw new ArgumentException("An empty frames.", "frames");
            }
            if (frms.Any(f => f == null))
            {
                throw new ArgumentException("Some frames are null.", "frames");
            }

            // 設定
            ImageFileName = imageFileName;
            ImageSize = imageSize;
            Frames = frms;
        }

        /// <summary>
        /// 画像ファイル名を取得する。
        /// </summary>
        public string ImageFileName { get; private set; }

        /// <summary>
        /// 画像サイズを取得する。
        /// </summary>
        public Size ImageSize { get; private set; }

        /// <summary>
        /// フレームのコレクションを取得する。
        /// </summary>
        public ReadOnlyCollection<TextureAtlasFrame> Frames { get; private set; }
    }
}

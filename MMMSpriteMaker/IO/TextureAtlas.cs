using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MMMSpriteMaker.IO
{
    /// <summary>
    /// テクスチャアトラスを定義するクラス。
    /// </summary>
    public partial class TextureAtlas
    {
        /// <summary>
        /// テクスチャアトラスのフレームを定義するクラス。
        /// </summary>
        public class Frame
        {
            /// <summary>
            /// コンストラクタ。
            /// </summary>
            /// <param name="leftTop">画像上での左上端位置。</param>
            /// <param name="size">フレームサイズ。</param>
            /// <param name="rotated">回転フラグ。</param>
            public Frame(Point leftTop, Size size, bool rotated)
            {
                // 引数チェック
                if (leftTop.X < 0 || leftTop.Y < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "leftTop",
                        leftTop,
                        "Invalid frame position.");
                }
                if (size.Width <= 0 || size.Height <= 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "size",
                        size,
                        "Invalid frame size.");
                }

                // 設定
                LeftTop = leftTop;
                Size = size;
                Rotated = rotated;
            }

            /// <summary>
            /// 画像上での左上端位置を取得する。
            /// 回転している場合でも画像上の左上端位置となる。
            /// </summary>
            public Point LeftTop { get; private set; }

            /// <summary>
            /// フレームサイズを取得する。
            /// 回転している場合、画像上とは幅と高さが逆転する。
            /// </summary>
            public Size Size { get; private set; }

            /// <summary>
            /// 回転フラグを取得する。
            /// </summary>
            public bool Rotated { get; private set; }

            /// <summary>
            /// 回転を考慮したテクスチャの左上端位置を取得する。
            /// </summary>
            public Point TextureLeftTop
            {
                get
                {
                    return Rotated ?
                        new Point(LeftTop.X + Size.Height, LeftTop.Y) :
                        LeftTop;
                }
            }

            /// <summary>
            /// 回転を考慮したテクスチャのU幅を取得する。負数になりうる。
            /// </summary>
            public double TextureSizeU
            {
                get { return Rotated ? -Size.Height : Size.Width; }
            }

            /// <summary>
            /// 回転を考慮したテクスチャのV幅を取得する。
            /// </summary>
            public double TextureSizeV
            {
                get { return Rotated ? Size.Width : Size.Height; }
            }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        protected TextureAtlas(
            string imageFileName,
            Size imageSize,
            IEnumerable<Frame> frames)
        {
            // 引数チェック
            if (string.IsNullOrWhiteSpace(imageFileName))
            {
                throw new ArgumentException("Invalid file name.", "imageFileName");
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
        public ReadOnlyCollection<Frame> Frames { get; private set; }
    }
}

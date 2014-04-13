using System;
using System.Windows;

namespace ruche.datas.textureAtlas
{
    /// <summary>
    /// テクスチャアトラスのフレーム単体を定義するクラス。
    /// </summary>
    public class TextureAtlasFrame
    {
        /// <summary>
        /// TextureAtlasFrame インスタンスを作成する。
        /// </summary>
        /// <param name="size">フレームサイズ。</param>
        /// <param name="leftTop">画像上の左上端位置。</param>
        /// <param name="rotated">回転フラグ。</param>
        /// <returns>TextureAtlasFrame インスタンス。</returns>
        /// <remarks>
        /// cocos2d形式などで定義されているパラメータをそのまま渡して初期化できる。
        /// </remarks>
        public static TextureAtlasFrame Create(Size size, Point leftTop, bool rotated)
        {
            double baseU = rotated ? (leftTop.X + size.Height) : leftTop.X;
            double baseV = leftTop.Y;
            double sizeU = rotated ? -size.Height : size.Width;
            double sizeV = rotated ? size.Width : size.Height;

            return
                new TextureAtlasFrame(
                    size,
                    new Point(baseU, baseV),
                    rotated ?
                        new Point(baseU, baseV + sizeV) :
                        new Point(baseU + sizeU, baseV),
                    new Point(baseU + sizeU, baseV + sizeV));
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="size">フレームサイズ。</param>
        /// <param name="leftTopUVPoint">
        /// 3D空間表示時に向かって左上端のテクスチャ座標となる画像上の位置。
        /// </param>
        /// <param name="rightTopUVPoint">
        /// 3D空間表示時に向かって右上端のテクスチャ座標となる画像上の位置。
        /// </param>
        /// <param name="rightBottomUVPoint">
        /// 3D空間表示時に向かって右下端のテクスチャ座標となる画像上の位置。
        /// </param>
        /// <param name="leftBottomUVPoint">
        /// 3D空間表示時に向かって左下端のテクスチャ座標となる画像上の位置。
        /// </param>
        public TextureAtlasFrame(
            Size size,
            Point leftTopUVPoint,
            Point rightTopUVPoint,
            Point rightBottomUVPoint,
            Point leftBottomUVPoint)
        {
            Size = size;
            LeftTopUVPoint = leftTopUVPoint;
            RightTopUVPoint = rightTopUVPoint;
            RightBottomUVPoint = rightBottomUVPoint;
            LeftBottomUVPoint = leftBottomUVPoint;
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="size">フレームサイズ。</param>
        /// <param name="leftTopUVPoint">
        /// 3D空間表示時に向かって左上端のテクスチャ座標となる画像上の位置。
        /// </param>
        /// <param name="rightTopUVPoint">
        /// 3D空間表示時に向かって右上端のテクスチャ座標となる画像上の位置。
        /// </param>
        /// <param name="rightBottomUVPoint">
        /// 3D空間表示時に向かって右下端のテクスチャ座標となる画像上の位置。
        /// </param>
        /// <remarks>
        /// 向かって左下端のテクスチャ座標は他の3点から自動的に求められる。
        /// </remarks>
        public TextureAtlasFrame(
            Size size,
            Point leftTopUVPoint,
            Point rightTopUVPoint,
            Point rightBottomUVPoint)
            :
            this(
                size,
                leftTopUVPoint,
                rightTopUVPoint,
                rightBottomUVPoint,
                rightBottomUVPoint + (leftTopUVPoint - rightTopUVPoint))
        {
        }

        /// <summary>
        /// フレームサイズを取得する。
        /// </summary>
        public Size Size { get; private set; }

        /// <summary>
        /// 3D空間表示時に向かって左上端のテクスチャ座標となる画像上の位置を取得する。
        /// </summary>
        public Point LeftTopUVPoint { get; private set; }

        /// <summary>
        /// 3D空間表示時に向かって右上端のテクスチャ座標となる画像上の位置を取得する。
        /// </summary>
        public Point RightTopUVPoint { get; private set; }

        /// <summary>
        /// 3D空間表示時に向かって右下端のテクスチャ座標となる画像上の位置を取得する。
        /// </summary>
        public Point RightBottomUVPoint { get; private set; }

        /// <summary>
        /// 3D空間表示時に向かって左下端のテクスチャ座標となる画像上の位置を取得する。
        /// </summary>
        public Point LeftBottomUVPoint { get; private set; }
    }
}

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
        /// <param name="leftTopOnImage">画像上の左上端位置。</param>
        /// <param name="rotated">回転フラグ。</param>
        /// <param name="originalSize">オリジナル画像サイズ。</param>
        /// <param name="leftTopTrimPoint">
        /// オリジナル画像をトリミングした際の左上端トリミング位置。
        /// </param>
        /// <returns>TextureAtlasFrame インスタンス。</returns>
        /// <remarks>
        /// cocos2d形式などで定義されているパラメータをそのまま渡して初期化できる。
        /// </remarks>
        public static TextureAtlasFrame Create(
            Size size,
            Point leftTopOnImage,
            bool rotated,
            Size originalSize,
            Point leftTopTrimPoint)
        {
            double baseU = rotated ? (leftTopOnImage.X + size.Height) : leftTopOnImage.X;
            double baseV = leftTopOnImage.Y;
            double sizeU = rotated ? -size.Height : size.Width;
            double sizeV = rotated ? size.Width : size.Height;

            return
                new TextureAtlasFrame(
                    size,
                    new Point(baseU, baseV),
                    rotated ?
                        new Point(baseU, baseV + sizeV) :
                        new Point(baseU + sizeU, baseV),
                    new Point(baseU + sizeU, baseV + sizeV),
                    originalSize,
                    leftTopTrimPoint);
        }

        /// <summary>
        /// TextureAtlasFrame インスタンスを作成する。
        /// </summary>
        /// <param name="size">フレームサイズ。オリジナル画像サイズにもなる。</param>
        /// <param name="leftTopOnImage">画像上の左上端位置。</param>
        /// <param name="rotated">回転フラグ。</param>
        /// <returns>TextureAtlasFrame インスタンス。</returns>
        /// <remarks>
        /// cocos2d形式などで定義されているパラメータをそのまま渡して初期化できる。
        /// </remarks>
        public static TextureAtlasFrame Create(
            Size size,
            Point leftTopOnImage,
            bool rotated)
        {
            return Create(size, leftTopOnImage, rotated, size, new Point(0, 0));
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
        /// <param name="originalSize">オリジナル画像サイズ。</param>
        /// <param name="leftTopTrimPoint">
        /// オリジナル画像をトリミングした際の左上端トリミング位置。
        /// </param>
        public TextureAtlasFrame(
            Size size,
            Point leftTopUVPoint,
            Point rightTopUVPoint,
            Point rightBottomUVPoint,
            Point leftBottomUVPoint,
            Size originalSize,
            Point leftTopTrimPoint)
        {
            Size = size;
            LeftTopUVPoint = leftTopUVPoint;
            RightTopUVPoint = rightTopUVPoint;
            RightBottomUVPoint = rightBottomUVPoint;
            LeftBottomUVPoint = leftBottomUVPoint;
            OriginalSize = originalSize;
            LeftTopTrimPoint = leftTopTrimPoint;
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
        /// <param name="originalSize">オリジナル画像サイズ。</param>
        /// <param name="leftTopTrimPoint">
        /// オリジナル画像をトリミングした際の左上端トリミング位置。
        /// </param>
        /// <remarks>
        /// 向かって左下端のテクスチャ座標は他の3点から自動的に求められる。
        /// </remarks>
        public TextureAtlasFrame(
            Size size,
            Point leftTopUVPoint,
            Point rightTopUVPoint,
            Point rightBottomUVPoint,
            Size originalSize,
            Point leftTopTrimPoint)
            :
            this(
                size,
                leftTopUVPoint,
                rightTopUVPoint,
                rightBottomUVPoint,
                rightBottomUVPoint + (leftTopUVPoint - rightTopUVPoint),
                originalSize,
                leftTopTrimPoint)
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="size">フレームサイズ。オリジナル画像サイズにもなる。</param>
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
            :
            this(
                size,
                leftTopUVPoint,
                rightTopUVPoint,
                rightBottomUVPoint,
                leftBottomUVPoint,
                size,
                new Point(0, 0))
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="size">フレームサイズ。オリジナル画像サイズにもなる。</param>
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
                size,
                new Point(0, 0))
        {
        }

        /// <summary>
        /// フレームサイズを取得する。
        /// </summary>
        /// <remarks>
        /// 実際のテクスチャ画像上でこのフレームのために確保されている領域サイズを表す。
        /// 画像を回転させて格納している場合は本来の(回転前の)サイズとなる。
        /// そのため、実際のテクスチャ画像とは横幅と縦幅が逆転している場合がある。
        /// </remarks>
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

        /// <summary>
        /// オリジナル画像サイズを取得する。
        /// </summary>
        /// <remarks>
        /// テクスチャアトラス作成時にオリジナル画像がトリミングされた場合、
        /// オリジナル画像のサイズを表す。
        /// トリミングされていないならば Size プロパティと同じ値を返す。
        /// </remarks>
        public Size OriginalSize { get; private set; }

        /// <summary>
        /// オリジナル画像をトリミングした際の左上端トリミング位置を取得する。
        /// </summary>
        /// <remarks>
        /// テクスチャアトラス作成時にオリジナル画像がトリミングされた場合、
        /// オリジナル画像上のこの位置がトリミング領域の左上端となる。
        /// トリミングされていないならば位置 0,0 を返す。
        /// </remarks>
        public Point LeftTopTrimPoint { get; private set; }
    }
}

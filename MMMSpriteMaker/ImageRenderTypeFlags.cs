using System;

namespace MMMSpriteMaker
{
    /// <summary>
    /// ImageRenderType 列挙値に紐付くメタ情報の組み合わせを定義する列挙。
    /// </summary>
    [Flags]
    public enum ImageRenderTypeFlags
    {
        /// <summary>
        /// 単一値として指定されることで、すべてのメタ情報が無効であることを示す。
        /// </summary>
        None = 0,

        /// <summary>
        /// 背面を描画できることを示す。
        /// </summary>
        CanRenderBack = 1 << 0,

        /// <summary>
        /// ライトとセルフシャドウを有効にできることを示す。
        /// </summary>
        CanUseLight = 1 << 1,

        /// <summary>
        /// ピクセル倍率設定を利用することを示す。
        /// </summary>
        UsePixelRatio = 1 << 2,

        /// <summary>
        /// ビューポート幅設定を利用することを示す。
        /// </summary>
        UseViewportWidth = 1 << 3,

        /// <summary>
        /// Zオーダー範囲設定を利用することを示す。
        /// </summary>
        UseZRange = 1 << 4,
    }
}

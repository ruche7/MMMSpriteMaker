using ruche.mmm.tools.spriteMaker.resources;
using System.ComponentModel.DataAnnotations;

namespace ruche.mmm.tools.spriteMaker
{
    /// <summary>
    /// イメージの描画方法を表す列挙。
    /// </summary>
    public enum ImageRenderType
    {
        /// <summary>
        /// スプライト。
        /// </summary>
        [Display(
            Name = "Enum_ImageRenderType_Sprite",
            ResourceType = typeof(Resources))]
        [ImageRenderTypeFlags(
            ImageRenderTypeFlags.CanRenderPost |
            ImageRenderTypeFlags.CanRenderBack |
            ImageRenderTypeFlags.UsePixelRatio |
            ImageRenderTypeFlags.UseViewportWidth |
            ImageRenderTypeFlags.UseZRange)]
        Sprite = 0,

        /// <summary>
        /// ドットバイドットのスプライト。
        /// </summary>
        [Display(
            Name = "Enum_ImageRenderType_DotByDotSprite",
            ResourceType = typeof(Resources))]
        [ImageRenderTypeFlags(
            ImageRenderTypeFlags.CanRenderPost |
            ImageRenderTypeFlags.CanRenderBack |
            ImageRenderTypeFlags.UseZRange)]
        DotByDotSprite = 1,

        /// <summary>
        /// ビルボード。
        /// </summary>
        [Display(
            Name = "Enum_ImageRenderType_Billboard",
            ResourceType = typeof(Resources))]
        [ImageRenderTypeFlags(
            ImageRenderTypeFlags.CanRenderBack |
            ImageRenderTypeFlags.CanUseLight |
            ImageRenderTypeFlags.UsePixelRatio)]
        Billboard = 2,

        /// <summary>
        /// 板ポリゴン。
        /// </summary>
        [Display(
            Name = "Enum_ImageRenderType_Polygon",
            ResourceType = typeof(Resources))]
        [ImageRenderTypeFlags(
            ImageRenderTypeFlags.CanRenderBack |
            ImageRenderTypeFlags.CanUseLight |
            ImageRenderTypeFlags.UsePixelRatio)]
        Polygon = 3,
    }

    /// <summary>
    /// ImageRenderType の拡張メソッドを提供する静的クラス。
    /// </summary>
    public static class ImageRenderTypeExtension
    {
        /// <summary>
        /// ImageRenderType 列挙値に紐付く
        /// ImageRenderTypeFlags 列挙値の組み合わせを取得する。
        /// </summary>
        /// <param name="self">ImageRenderType 列挙値。</param>
        /// <returns>ImageRenderTypeFlags 列挙値の組み合わせ。</returns>
        public static ImageRenderTypeFlags GetFlags(this ImageRenderType self)
        {
            return ImageRenderTypeFlagsAttribute.GetFlags(self);
        }

        /// <summary>
        /// ImageRenderType 列挙値に、
        /// 指定した ImageRenderTypeFlags 列挙値の組み合わせが
        /// 1 つ以上紐付いているか否かを取得する。
        /// </summary>
        /// <param name="self">ImageRenderType 列挙値。</param>
        /// <param name="flags">ImageRenderTypeFlags 列挙値の組み合わせ。</param>
        /// <returns>1 つ以上紐付いているならば true 。そうでなければ false 。</returns>
        public static bool HasAnyFlags(
            this ImageRenderType self,
            ImageRenderTypeFlags flags)
        {
            return ((self.GetFlags() & flags) != 0);
        }
    }
}

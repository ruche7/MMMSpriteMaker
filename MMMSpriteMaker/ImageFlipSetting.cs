using System.ComponentModel.DataAnnotations;
using Prop = MMMSpriteMaker.Properties;

namespace MMMSpriteMaker
{
    /// <summary>
    /// イメージの反転設定を表す列挙。
    /// </summary>
    public enum ImageFlipSetting
    {
        /// <summary>
        /// 反転しない。
        /// </summary>
        [Display(
            Name = "Enum_ImageFlipSetting_NotFlip",
            ResourceType = typeof(Prop.Resources))]
        NotFlip = 0,

        /// <summary>
        /// 反転する。
        /// </summary>
        [Display(
            Name = "Enum_ImageFlipSetting_Flip",
            ResourceType = typeof(Prop.Resources))]
        Flip = 1,

        /// <summary>
        /// 選択可能にする。
        /// </summary>
        [Display(
            Name = "Enum_ImageFlipSetting_Selectable",
            ResourceType = typeof(Prop.Resources))]
        Selectable = 2,
    }
}

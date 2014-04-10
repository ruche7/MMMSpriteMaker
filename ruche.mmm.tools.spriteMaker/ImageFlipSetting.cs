using System.ComponentModel.DataAnnotations;
using ruche.mmm.tools.spriteMaker.resources;

namespace ruche.mmm.tools.spriteMaker
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
            ResourceType = typeof(Resources))]
        NotFlip = 0,

        /// <summary>
        /// 反転する。
        /// </summary>
        [Display(
            Name = "Enum_ImageFlipSetting_Flip",
            ResourceType = typeof(Resources))]
        Flip = 1,

        /// <summary>
        /// 選択可能にする。
        /// </summary>
        [Display(
            Name = "Enum_ImageFlipSetting_Selectable",
            ResourceType = typeof(Resources))]
        Selectable = 2,
    }
}

using System.ComponentModel.DataAnnotations;
using ruche.mmm.tools.spriteMaker.resources;

namespace ruche.mmm.tools.spriteMaker
{
    /// <summary>
    /// ライトとセルフシャドウの有効設定を表す列挙。
    /// </summary>
    public enum LightSetting
    {
        /// <summary>
        /// 無効にする。
        /// </summary>
        [Display(
            Name = "Enum_LightSetting_Disabled",
            ResourceType = typeof(Resources))]
        Disabled = 0,

        /// <summary>
        /// 有効にする。
        /// </summary>
        [Display(
            Name = "Enum_LightSetting_Enabled",
            ResourceType = typeof(Resources))]
        Enabled = 1,

        /// <summary>
        /// 選択可能にする。
        /// </summary>
        [Display(
            Name = "Enum_LightSetting_Selectable",
            ResourceType = typeof(Resources))]
        Selectable = 2,
    }
}

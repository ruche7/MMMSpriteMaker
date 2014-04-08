using System.ComponentModel.DataAnnotations;
using Prop = MMMSpriteMaker.Properties;

namespace MMMSpriteMaker
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
            ResourceType = typeof(Prop.Resources))]
        Disabled = 0,

        /// <summary>
        /// 有効にする。
        /// </summary>
        [Display(
            Name = "Enum_LightSetting_Enabled",
            ResourceType = typeof(Prop.Resources))]
        Enabled = 1,

        /// <summary>
        /// 選択可能にする。
        /// </summary>
        [Display(
            Name = "Enum_LightSetting_Selectable",
            ResourceType = typeof(Prop.Resources))]
        Selectable = 2,
    }
}

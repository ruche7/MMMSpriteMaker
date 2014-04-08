using System.ComponentModel.DataAnnotations;
using Prop = MMMSpriteMaker.Properties;

namespace MMMSpriteMaker
{
    /// <summary>
    /// イメージの基準点位置を表す列挙。
    /// </summary>
    public enum ImageBasePoint
    {
        /// <summary>
        /// 左上。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_LeftTop",
            ResourceType = typeof(Prop.Resources))]
        LeftTop = 0,

        /// <summary>
        /// 中央上。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_MiddleTop",
            ResourceType = typeof(Prop.Resources))]
        MiddleTop = 1,

        /// <summary>
        /// 右上。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_RightTop",
            ResourceType = typeof(Prop.Resources))]
        RightTop = 2,

        /// <summary>
        /// 左中央。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_LeftMiddle",
            ResourceType = typeof(Prop.Resources))]
        LeftMiddle = 3,

        /// <summary>
        /// 中心。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_Center",
            ResourceType = typeof(Prop.Resources))]
        Center = 4,

        /// <summary>
        /// 右中央。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_RightMiddle",
            ResourceType = typeof(Prop.Resources))]
        RightMiddle = 5,

        /// <summary>
        /// 左下。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_LeftBottom",
            ResourceType = typeof(Prop.Resources))]
        LeftBottom = 6,

        /// <summary>
        /// 中央下。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_MiddleBottom",
            ResourceType = typeof(Prop.Resources))]
        MiddleBottom = 7,

        /// <summary>
        /// 右下。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_RightBottom",
            ResourceType = typeof(Prop.Resources))]
        RightBottom = 8,
    }
}

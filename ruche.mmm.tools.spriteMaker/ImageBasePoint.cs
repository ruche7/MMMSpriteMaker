using ruche.mmm.tools.spriteMaker.resources;
using System.ComponentModel.DataAnnotations;

namespace ruche.mmm.tools.spriteMaker
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
            ResourceType = typeof(Resources))]
        LeftTop = 0,

        /// <summary>
        /// 中央上。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_MiddleTop",
            ResourceType = typeof(Resources))]
        MiddleTop = 1,

        /// <summary>
        /// 右上。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_RightTop",
            ResourceType = typeof(Resources))]
        RightTop = 2,

        /// <summary>
        /// 左中央。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_LeftMiddle",
            ResourceType = typeof(Resources))]
        LeftMiddle = 3,

        /// <summary>
        /// 中心。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_Center",
            ResourceType = typeof(Resources))]
        Center = 4,

        /// <summary>
        /// 右中央。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_RightMiddle",
            ResourceType = typeof(Resources))]
        RightMiddle = 5,

        /// <summary>
        /// 左下。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_LeftBottom",
            ResourceType = typeof(Resources))]
        LeftBottom = 6,

        /// <summary>
        /// 中央下。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_MiddleBottom",
            ResourceType = typeof(Resources))]
        MiddleBottom = 7,

        /// <summary>
        /// 右下。
        /// </summary>
        [Display(
            Name = "Enum_ImageBasePoint_RightBottom",
            ResourceType = typeof(Resources))]
        RightBottom = 8,
    }
}

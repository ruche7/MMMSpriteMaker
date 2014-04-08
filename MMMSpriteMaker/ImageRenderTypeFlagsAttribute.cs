using System;
using System.Linq;

namespace MMMSpriteMaker
{
    /// <summary>
    /// ImageRenderType 列挙値に紐付くメタ情報の組み合わせを指定する属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class ImageRenderTypeFlagsAttribute : Attribute
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="flags">
        /// ImageRenderTypeFlags 列挙値の組み合わせ。
        /// </param>
        public ImageRenderTypeFlagsAttribute(ImageRenderTypeFlags flags)
        {
            Flags = flags;
        }

        /// <summary>
        /// ImageRenderTypeFlags 列挙値の組み合わせを取得または設定する。
        /// </summary>
        public ImageRenderTypeFlags Flags { get; set; }

        /// <summary>
        /// ImageRenderType 列挙値に紐付く
        /// ImageRenderTypeFlags 列挙値の組み合わせを取得する。
        /// </summary>
        /// <param name="self">ImageRenderType 列挙値。</param>
        /// <returns>ImageRenderTypeFlags 列挙値の組み合わせ。</returns>
        public static ImageRenderTypeFlags GetFlags(ImageRenderType self)
        {
            var info = typeof(ImageRenderType).GetField(self.ToString());
            if (info == null)
            {
                return ImageRenderTypeFlags.None;
            }

            var attrs =
                info.GetCustomAttributes(typeof(ImageRenderTypeFlagsAttribute), false)
                    as ImageRenderTypeFlagsAttribute[];
            if (attrs == null || attrs.Length <= 0)
            {
                return ImageRenderTypeFlags.None;
            }

            return
                attrs.Aggregate(
                    ImageRenderTypeFlags.None,
                    (flags, attr) => (flags | attr.Flags));
        }
    }
}

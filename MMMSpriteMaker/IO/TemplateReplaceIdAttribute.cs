using System;
using System.Reflection;
using System.Text;

namespace MMMSpriteMaker.IO
{
    /// <summary>
    /// 雛形文字列の置換用プロパティであることを示す属性。
    /// </summary>
    /// <remarks>
    /// 例えばプロパティ名が SomeProp である場合、
    /// 雛形文字列中の "[[SomeProp]]" という文字列をプロパティの値で置換する。
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class TemplateReplaceIdAttribute : Attribute
    {
        /// <summary>
        /// 文字列の置換処理を実行する。
        /// </summary>
        /// <typeparam name="T">
        /// TemplateReplaceIdAttribute 属性を持つプロパティが1つ以上定義されている型。
        /// </typeparam>
        /// <param name="instance">置換用インスタンス。</param>
        /// <param name="text">置換対象文字列。</param>
        /// <returns>置換処理結果の文字列。</returns>
        public static string Replace<T>(T instance, string text)
        {
            var s = new StringBuilder(text);

            Array.ForEach(
                instance.GetType().GetProperties(
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance),
                pi =>
                {
                    if (Attribute.IsDefined(pi, typeof(TemplateReplaceIdAttribute)))
                    {
                        s.Replace(
                            "[[" + pi.Name + "]]",
                            pi.GetValue(instance, null).ToString());
                    }
                });

            return s.ToString();
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public TemplateReplaceIdAttribute() { }
    }
}

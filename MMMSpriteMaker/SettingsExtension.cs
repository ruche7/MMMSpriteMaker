using MMMSpriteMaker.Properties;
using System;
using System.Configuration;

namespace MMMSpriteMaker
{
    /// <summary>
    /// Properties.Settings クラスの拡張メソッドを提供する静的クラス。
    /// </summary>
    public static class SettingsExtension
    {
        /// <summary>
        /// 指定した設定の既定値を取得する。
        /// </summary>
        /// <typeparam name="T">値型。</typeparam>
        /// <param name="name">設定名。</param>
        /// <param name="converter">型コンバータ。</param>
        /// <returns>既定値。</returns>
        private static T GetDefaultValue<T>(string name, Func<string, T> converter)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            var info = typeof(Settings).GetProperty(name);
            if (info == null)
            {
                throw new ArgumentException(
                    "'" + name + "' is not found on Settings.",
                    "name");
            }

            var attrs =
                info.GetCustomAttributes(typeof(DefaultSettingValueAttribute), false)
                    as DefaultSettingValueAttribute[];
            if (attrs == null || attrs.Length <= 0)
            {
                throw new ArgumentException(
                    "'" + name + "' is not have DefaultSettingValueAttribute.",
                    "name");
            }

            return converter(attrs[0].Value);
        }

        /// <summary>
        /// 実際に背面を描画するか否かを取得する。
        /// </summary>
        /// <param name="self">アプリケーション設定。</param>
        /// <returns>背面を描画するならば true 。そうでなければ false 。</returns>
        public static bool IsRenderingBack(this Settings self)
        {
            return
                self.RenderingBack &&
                self.RenderType.HasAnyFlags(ImageRenderTypeFlags.CanRenderBack);
        }

        /// <summary>
        /// 実際のライトとセルフシャドウの有効設定値を取得する。
        /// </summary>
        /// <param name="self">アプリケーション設定。</param>
        /// <returns>ライトとセルフシャドウの有効設定値。</returns>
        public static LightSetting GetLightSetting(this Settings self)
        {
            return
                self.RenderType.HasAnyFlags(ImageRenderTypeFlags.CanUseLight) ?
                    self.LightSetting :
                    LightSetting.Disabled;
        }

        /// <summary>
        /// 実際に書き出すピクセル倍率値を取得する。
        /// </summary>
        /// <param name="self">アプリケーション設定。</param>
        /// <returns>ピクセル倍率値。</returns>
        public static float GetPixelRatio(this Settings self)
        {
            return
                self.RenderType.HasAnyFlags(ImageRenderTypeFlags.UsePixelRatio) ?
                    self.PixelRatio :
                    GetDefaultValue<float>("PixelRatio", s => float.Parse(s));
        }

        /// <summary>
        /// 実際に書き出すビューポート幅を取得する。
        /// </summary>
        /// <param name="self">アプリケーション設定。</param>
        /// <returns>ビューポート幅。</returns>
        public static float GetSpriteViewportWidth(this Settings self)
        {
            return
                self.RenderType.HasAnyFlags(ImageRenderTypeFlags.UseViewportWidth) ?
                    self.SpriteViewportWidth :
                    GetDefaultValue<float>("SpriteViewportWidth", s => float.Parse(s));
        }

        /// <summary>
        /// 実際に書き出すZオーダー範囲値を取得する。
        /// </summary>
        /// <param name="self">アプリケーション設定。</param>
        /// <returns>Zオーダー範囲値。</returns>
        public static float GetSpriteZRange(this Settings self)
        {
            return
                self.RenderType.HasAnyFlags(ImageRenderTypeFlags.UseZRange) ?
                    self.SpriteZRange :
                    GetDefaultValue<float>("SpriteZRange", s => float.Parse(s));
        }
    }
}

using System;
using System.ComponentModel;

namespace ruche.mmm.tools.spriteMaker
{
    /// <summary>
    /// エフェクトファイル設定クラス。
    /// </summary>
    [Serializable]
    public sealed class EffectFileConfig : ConfigBase, ICloneable
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public EffectFileConfig()
        {
            Reset();
        }

        /// <summary>
        /// イメージの描画方法を取得または設定する。
        /// </summary>
        [DefaultValue(typeof(ImageRenderType), "Sprite")]
        public ImageRenderType RenderType
        {
            get { return (ImageRenderType)this["RenderType"]; }
            set { this["RenderType"] = value; }
        }

        /// <summary>
        /// 背面を描画するか否かを取得または設定する。
        /// </summary>
        /// <remarks>
        /// 実際に背景を描画するか否かは IsRenderingBack メソッドで取得すること。
        /// </remarks>
        [DefaultValue(false)]
        public bool RenderingBack
        {
            get { return (bool)this["RenderingBack"]; }
            set { this["RenderingBack"] = value; }
        }

        /// <summary>
        /// 実際に背面を描画するか否かを取得する。
        /// </summary>
        /// <returns>背面を描画するならば true 。そうでなければ false 。</returns>
        public bool IsRenderingBack()
        {
            return
                RenderType.HasAnyFlags(ImageRenderTypeFlags.CanRenderBack) ?
                    RenderingBack :
                    false;
        }

        /// <summary>
        /// ライトの有効設定値を取得または設定する。
        /// </summary>
        /// <remarks>
        /// 実際に利用される設定値は GetLightSetting メソッドで取得すること。
        /// </remarks>
        [DefaultValue(typeof(LightSetting), "Disabled")]
        public LightSetting LightSetting
        {
            get { return (LightSetting)this["LightSetting"]; }
            set { this["LightSetting"] = value; }
        }

        /// <summary>
        /// 実際に利用されるライトの有効設定値を取得または設定する。
        /// </summary>
        /// <returns>ライトの有効設定値。</returns>
        public LightSetting GetLightSetting()
        {
            return
                RenderType.HasAnyFlags(ImageRenderTypeFlags.CanUseLight) ?
                    LightSetting :
                    LightSetting.Disabled;
        }

        /// <summary>
        /// ピクセル倍率の最小値。
        /// </summary>
        public static readonly float MinPixelRatio = 0.000001f;

        /// <summary>
        /// ピクセル倍率値を取得または設定する。
        /// </summary>
        /// <remarks>
        /// 実際に利用される値は GetPixelRatio メソッドで取得すること。
        /// </remarks>
        [DefaultValue(0.1f)]
        public float PixelRatio
        {
            get { return (float)this["PixelRatio"]; }
            set { this["PixelRatio"] = Math.Max(value, MinPixelRatio); }
        }

        /// <summary>
        /// 実際に利用されるピクセル倍率値を取得する。
        /// </summary>
        /// <returns>ピクセル倍率値。</returns>
        public float GetPixelRatio()
        {
            return
                RenderType.HasAnyFlags(ImageRenderTypeFlags.UsePixelRatio) ?
                    PixelRatio :
                    (float)GetDefaultValue("PixelRatio");
        }

        /// <summary>
        /// ビューポート幅の最小値。
        /// </summary>
        public static readonly float MinSpriteViewportWidth = 0.000001f;

        /// <summary>
        /// ビューポート幅を取得または設定する。
        /// </summary>
        /// <remarks>
        /// 実際に利用される値は GetSpriteViewportWidth メソッドで取得すること。
        /// </remarks>
        [DefaultValue(64.0f)]
        public float SpriteViewportWidth
        {
            get { return (float)this["SpriteViewportWidth"]; }
            set { this["SpriteViewportWidth"] = Math.Max(value, MinSpriteViewportWidth); }
        }

        /// <summary>
        /// 実際に利用されるビューポート幅を取得する。
        /// </summary>
        /// <returns>ビューポート幅。</returns>
        public float GetSpriteViewportWidth()
        {
            return
                RenderType.HasAnyFlags(ImageRenderTypeFlags.UseViewportWidth) ?
                    SpriteViewportWidth :
                    (float)GetDefaultValue("SpriteViewportWidth");
        }

        /// <summary>
        /// Zオーダー範囲の最小値。
        /// </summary>
        public static readonly float MinSpriteZRange = 0.000001f;

        /// <summary>
        /// Zオーダー範囲値を取得または設定する。
        /// </summary>
        /// <remarks>
        /// 実際に利用される値は GetSpriteZRange メソッドで取得すること。
        /// </remarks>
        [DefaultValue(10.0f)]
        public float SpriteZRange
        {
            get { return (float)this["SpriteZRange"]; }
            set { this["SpriteZRange"] = Math.Max(value, MinSpriteZRange); }
        }

        /// <summary>
        /// 実際に利用されるZオーダー範囲値を取得する。
        /// </summary>
        /// <returns>Zオーダー範囲値。</returns>
        public float GetSpriteZRange()
        {
            return
                RenderType.HasAnyFlags(ImageRenderTypeFlags.UseZRange) ?
                    SpriteZRange :
                    (float)GetDefaultValue("SpriteZRange");
        }

        /// <summary>
        /// イメージの基準点位置を取得または設定する。
        /// </summary>
        [DefaultValue(typeof(ImageBasePoint), "Center")]
        public ImageBasePoint BasePoint
        {
            get { return (ImageBasePoint)this["BasePoint"]; }
            set { this["BasePoint"] = value; }
        }

        /// <summary>
        /// イメージの左右反転設定値を取得または設定する。
        /// </summary>
        [DefaultValue(typeof(ImageFlipSetting), "Selectable")]
        public ImageFlipSetting HorizontalFlipSetting
        {
            get { return (ImageFlipSetting)this["HorizontalFlipSetting"]; }
            set { this["HorizontalFlipSetting"] = value; }
        }

        /// <summary>
        /// イメージの上下反転設定値を取得または設定する。
        /// </summary>
        [DefaultValue(typeof(ImageFlipSetting), "Selectable")]
        public ImageFlipSetting VerticalFlipSetting
        {
            get { return (ImageFlipSetting)this["VerticalFlipSetting"]; }
            set { this["VerticalFlipSetting"] = value; }
        }

        /// <summary>
        /// 自身の設定値で初期化されたクローンを作成する。
        /// </summary>
        /// <returns>自身の設定値で初期化されたクローン。</returns>
        /// <remarks>
        /// 設定値のみがコピーされる。イベントはコピーされない。
        /// </remarks>
        public EffectFileConfig Clone()
        {
            return CloneCore<EffectFileConfig>();
        }

        #region ICloneable 明示的実装

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion
    }
}

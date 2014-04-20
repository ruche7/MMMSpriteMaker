using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ruche.mmm.tools.spriteMaker
{
    /// <summary>
    /// アクセサリファイル設定クラス。
    /// </summary>
    [Serializable]
    public sealed class AccessoryFileConfig : ConfigBase, ICloneable
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public AccessoryFileConfig()
        {
            Reset();
        }

        /// <summary>
        /// 面の色を取得または設定する。
        /// </summary>
        [DefaultValue(typeof(Color), "sc# 1.0,0.7,0.7,0.7")]
        public Color FaceColor
        {
            get { return (Color)this["FaceColor"]; }
            set { this["FaceColor"] = value; }
        }

        /// <summary>
        /// 発散光の色成分を取得または設定する。α成分は無視される。
        /// </summary>
        [DefaultValue(typeof(Color), "sc# 1.0,0.3,0.3,0.3")]
        public Color EmissiveColor
        {
            get { return (Color)this["EmissiveColor"]; }
            set { this["EmissiveColor"] = value; }
        }

        /// <summary>
        /// 鏡面反射光の色成分を取得または設定する。α成分は無視される。
        /// </summary>
        [DefaultValue(typeof(Color), "sc# 1.0,0.2,0.2,0.2")]
        public Color SpecularColor
        {
            get { return (Color)this["SpecularColor"]; }
            set { this["SpecularColor"] = value; }
        }

        /// <summary>
        /// 鏡面反射強度の最小値。
        /// </summary>
        public static readonly float MinSpecularPower = 0;

        /// <summary>
        /// 鏡面反射強度値を取得または設定する。
        /// </summary>
        [DefaultValue(1.0f)]
        public float SpecularPower
        {
            get { return (float)this["SpecularPower"]; }
            set { this["SpecularPower"] = Math.Max(value, MinSpecularPower); }
        }

        /// <summary>
        /// 自身の設定値で初期化されたクローンを作成する。
        /// </summary>
        /// <returns>自身の設定値で初期化されたクローン。</returns>
        /// <remarks>
        /// 設定値のみがコピーされる。イベントはコピーされない。
        /// </remarks>
        public AccessoryFileConfig Clone()
        {
            return CloneCore<AccessoryFileConfig>();
        }

        #region ICloneable 明示的実装

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion
    }
}

using System;
using System.IO;
using System.Linq;
using System.Text;
using Prop = MMMSpriteMaker.Properties;

namespace MMMSpriteMaker.IO
{
    /// <summary>
    /// エフェクトファイルを作成するクラス。
    /// </summary>
    public class EffectFileMaker
    {
        /// <summary>
        /// ピクセル倍率の最小値。
        /// </summary>
        public static readonly float MinPixelRatio = 0.000001f;

        /// <summary>
        /// ピクセル倍率の最大値。
        /// </summary>
        public static readonly float MaxPixelRatio = 100000.0f;

        /// <summary>
        /// ビューポート幅の最小値。
        /// </summary>
        public static readonly float MinSpriteViewportWidth = 0.000001f;

        /// <summary>
        /// ビューポート幅の最大値。
        /// </summary>
        public static readonly float MaxSpriteViewportWidth = 100000.0f;

        /// <summary>
        /// Zオーダー範囲の最小値。
        /// </summary>
        public static readonly float MinSpriteZRange = 0.000001f;

        /// <summary>
        /// Zオーダー範囲の最大値。
        /// </summary>
        public static readonly float MaxSpriteZRange = 100000.0f;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public EffectFileMaker()
        {
            Config = Prop.Settings.Default;
        }

        /// <summary>
        /// テクスチャアトラスを取得または設定する。
        /// </summary>
        public TextureAtlas TextureAtlas { get; set; }

        /// <summary>
        /// アプリケーション設定を取得または設定する。
        /// </summary>
        public Prop.Settings Config { get; set; }

        /// <summary>
        /// 実際に背面を描画するか否かを取得する。
        /// </summary>
        /// <returns>背面を描画するならば true 。そうでなければ false 。</returns>
        public bool IsRenderingBack()
        {
            return
                Config.RenderingBack &&
                Config.RenderType.HasAnyFlags(ImageRenderTypeFlags.CanRenderBack);
        }

        /// <summary>
        /// エフェクトファイルを作成する。
        /// </summary>
        /// <param name="filePath">ファイルパス。</param>
        public void Make(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Invalid file path.", "filePath");
            }
            if (TextureAtlas == null)
            {
                throw new InvalidOperationException("TextureAtlas is null.");
            }
            if (Config == null)
            {
                throw new InvalidOperationException("Config is null.");
            }

            // 雛形文字列取得
            var code = Prop.Resources.SpriteEffect;

            // 置換
            code = TemplateReplaceIdAttribute.Replace(this, code);

            // 書き出し
            File.WriteAllText(filePath, code, Encoding.GetEncoding(932));
        }

        #region 文字列置換用プロパティ

        [TemplateReplaceId]
        private int ConfigRenderType
        {
            get { return (int)Config.RenderType; }
        }

        [TemplateReplaceId]
        private int ConfigRenderingBack
        {
            get { return Config.IsRenderingBack() ? 1 : 0; }
        }

        [TemplateReplaceId]
        private int ConfigLightSetting
        {
            get { return (int)Config.GetLightSetting(); }
        }

        [TemplateReplaceId]
        private string ConfigPixelRatio
        {
            get { return string.Format("{0:0.#########}f", Config.GetPixelRatio()); }
        }

        [TemplateReplaceId]
        private string ConfigSpriteViewportWidth
        {
            get
            {
                return string.Format("{0:0.#########}f", Config.GetSpriteViewportWidth());
            }
        }

        [TemplateReplaceId]
        private string ConfigSpriteZRange
        {
            get { return string.Format("{0:0.#########}f", Config.GetSpriteZRange()); }
        }

        [TemplateReplaceId]
        private int ConfigBasePoint
        {
            get { return (int)Config.BasePoint; }
        }

        [TemplateReplaceId]
        private int ConfigHorizontalFlipSetting
        {
            get { return (int)Config.HorizontalFlipSetting; }
        }

        [TemplateReplaceId]
        private int ConfigVerticalFlipSetting
        {
            get { return (int)Config.VerticalFlipSetting; }
        }

        [TemplateReplaceId]
        private int AtlasFrameCount
        {
            get { return TextureAtlas.Frames.Count; }
        }

        [TemplateReplaceId]
        private int AtlasImageWidth
        {
            get { return (int)TextureAtlas.ImageSize.Width; }
        }

        [TemplateReplaceId]
        private int AtlasImageHeight
        {
            get { return (int)TextureAtlas.ImageSize.Height; }
        }

        [TemplateReplaceId]
        private string AtlasSizes
        {
            get
            {
                return
                    string.Join(
                        "\n",
                        from f in TextureAtlas.Frames
                        select
                            "        float2(" +
                            (int)f.Size.Width + ", " + (int)f.Size.Height +
                            ") * SprMake_AtlasSizeMul,");
            }
        }

        [TemplateReplaceId]
        private string AtlasUVLeftTops
        {
            get
            {
                return
                    string.Join(
                        "\n",
                        from f in TextureAtlas.Frames
                        let x = f.Rotated ? (f.LeftTop.X + f.Size.Height) : f.LeftTop.X
                        select
                            "        float2(" + (int)x + ", " + (int)f.LeftTop.Y +
                            ") * SprMake_AtlasUVMul,");
            }
        }

        [TemplateReplaceId]
        private string AtlasUVSizes
        {
            get
            {
                return
                    string.Join(
                        "\n",
                        from f in TextureAtlas.Frames
                        let w = f.Rotated ? -f.Size.Height : f.Size.Width
                        let h = f.Rotated ? f.Size.Width : f.Size.Height
                        select
                            "        float2(" + (int)w + ", " + (int)h +
                            ") * SprMake_AtlasUVMul,");
            }
        }

        [TemplateReplaceId]
        private string SelectAtlasCode
        {
            get
            {
                return
                    string.Join(
                        "\n",
                        from i in Enumerable.Range(0, TextureAtlas.Frames.Count)
                        let ri = TextureAtlas.Frames.Count - i - 1
                        let else_ = (i > 0) ? "else " : ""
                        let if_ = (ri > 0) ? ("if (SprMake_AtlasIndex >= " + ri + ")") : ""
                        select
                            "    " + else_ + if_ + "\n" +
                            "    {\n" +
                            "        Out.Size = SprMake_AtlasSizes[" + ri + "];\n" +
                            "        Out.LeftTopUV = SprMake_AtlasUVLeftTops[" + ri + "];\n" +
                            "        Out.UVSize = SprMake_AtlasUVSizes[" + ri + "];\n" +
                            "    }");
            }
        }

        #endregion
    }
}

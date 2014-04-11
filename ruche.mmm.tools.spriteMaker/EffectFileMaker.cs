using ruche.mmm.tools.spriteMaker.resources;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ruche.mmm.tools.spriteMaker
{
    /// <summary>
    /// エフェクトファイルを作成するクラス。
    /// </summary>
    public class EffectFileMaker
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public EffectFileMaker()
        {
            Config = new EffectFileConfig();
        }

        /// <summary>
        /// テクスチャアトラスを取得または設定する。
        /// </summary>
        public TextureAtlas TextureAtlas { get; set; }

        /// <summary>
        /// エフェクトファイル設定を取得または設定する。
        /// </summary>
        public EffectFileConfig Config { get; set; }

        /// <summary>
        /// エフェクトファイルを作成する。
        /// </summary>
        /// <param name="filePath">ファイルパス。</param>
        /// <remarks>
        /// 事前に TextureAtlas および Config に有効な値を設定しておく必要がある。
        /// </remarks>
        public void Make(string filePath)
        {
            if (!MakerUtil.IsValidPath(filePath))
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
            var code = Resources.SpriteEffect;

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
            get { return string.Format("{0:0.0########}f", Config.GetPixelRatio()); }
        }

        [TemplateReplaceId]
        private string ConfigSpriteViewportWidth
        {
            get
            {
                return string.Format("{0:0.0########}f", Config.GetSpriteViewportWidth());
            }
        }

        [TemplateReplaceId]
        private string ConfigSpriteZRange
        {
            get { return string.Format("{0:0.0########}f", Config.GetSpriteZRange()); }
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
                        Environment.NewLine,
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
                        Environment.NewLine,
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
                        Environment.NewLine,
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
                        Environment.NewLine,
                        from i in Enumerable.Range(0, TextureAtlas.Frames.Count)
                        let ri = TextureAtlas.Frames.Count - i - 1
                        let else_ = (i > 0) ? "else " : ""
                        let if_ = (ri > 0) ? ("if (SprMake_AtlasIndex >= " + ri + ")") : ""
                        select
                            string.Join(
                                Environment.NewLine + "    ",
                                "    " + else_ + if_,
                                "{",
                                "    Out.Size = SprMake_AtlasSizes[" + ri + "];",
                                "    Out.LeftTopUV = SprMake_AtlasUVLeftTops[" + ri + "];",
                                "    Out.UVSize = SprMake_AtlasUVSizes[" + ri + "];",
                                "}"));
            }
        }

        #endregion
    }
}

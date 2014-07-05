using ruche.datas.textureAtlas;
using ruche.mmm.tools.spriteMaker.resources;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

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
        /// エフェクトファイル設定を取得または設定する。
        /// </summary>
        public EffectFileConfig Config { get; set; }

        /// <summary>
        /// テクスチャアトラスを取得または設定する。
        /// </summary>
        public TextureAtlas TextureAtlas { get; set; }

        /// <summary>
        /// エフェクトファイルを作成する。
        /// </summary>
        /// <param name="filePath">ファイルパス。</param>
        /// <remarks>
        /// 事前に下記のプロパティに有効な値を設定しておく必要がある。
        /// 
        /// <list type="bullet">
        /// <item><description>Config</description></item>
        /// <item><description>TextureAtlas</description></item>
        /// </list>
        /// </remarks>
        public void Make(string filePath)
        {
            if (!MakerUtil.IsValidPath(filePath))
            {
                throw new ArgumentException("Invalid file path.", "filePath");
            }
            if (Config == null)
            {
                throw new InvalidOperationException("Config is null.");
            }
            if (TextureAtlas == null)
            {
                throw new InvalidOperationException("TextureAtlas is null.");
            }

            // 雛形文字列取得
            var code = Resources.SpriteEffect;

            // 置換
            code = TemplateReplaceIdAttribute.Replace(this, code);

            // 書き出し
            File.WriteAllText(filePath, code, Encoding.GetEncoding(932));
        }

        #region 文字列置換用プロパティ

        /// <summary>
        /// エフェクトファイル上で float 値を表す文字列を作成する。
        /// </summary>
        /// <param name="value">float 値。</param>
        /// <returns>float 値を表す文字列。</returns>
        private static string MakeFloatString(float value)
        {
            return string.Format("{0:0.0########}f", value);
        }

        [TemplateReplaceId]
        private int ConfigRenderType
        {
            get { return (int)Config.RenderType; }
        }

        [TemplateReplaceId]
        private int ConfigPostEffect
        {
            get { return Config.IsPostEffect() ? 1 : 0; }
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
            get { return MakeFloatString(Config.GetPixelRatio()); }
        }

        [TemplateReplaceId]
        private string ConfigSpriteViewportWidth
        {
            get { return MakeFloatString(Config.GetSpriteViewportWidth()); }
        }

        [TemplateReplaceId]
        private string ConfigSpriteZRange
        {
            get { return MakeFloatString(Config.GetSpriteZRange()); }
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
                            MakeFloatString((float)f.Size.Width) + ", " +
                            MakeFloatString((float)f.Size.Height) +
                            ") * SprMake_AtlasSizeMul,");
            }
        }

        [TemplateReplaceId]
        private string AtlasPosLeftBottoms
        {
            get
            {
                return
                    string.Join(
                        Environment.NewLine,
                        from f in TextureAtlas.Frames
                        let trimL = f.LeftTopTrimPoint.X
                        let trimB =
                            f.OriginalSize.Height - (f.LeftTopTrimPoint.Y + f.Size.Height)
                        select
                            "        (float2(" +
                            MakeFloatString((float)f.OriginalSize.Width) + ", " +
                            MakeFloatString((float)f.OriginalSize.Height) +
                            ") * SprMake_AtlasBasePointMul + float2(" +
                            MakeFloatString((float)trimL) + ", " +
                            MakeFloatString((float)trimB) +
                            ")) * SprMake_AtlasSizeMul,");
            }
        }

        private string MakeUVString(Func<TextureAtlasFrame, Point> uvPointSelector)
        {
            return
                string.Join(
                    Environment.NewLine,
                    from f in TextureAtlas.Frames
                    let pt = uvPointSelector(f)
                    select
                        "        float2(" +
                        MakeFloatString((float)pt.X) + ", " +
                        MakeFloatString((float)pt.Y) +
                        ") * SprMake_AtlasUVMul,");
        }

        [TemplateReplaceId]
        private string AtlasUVLeftTops
        {
            get { return MakeUVString(f => f.LeftTopUVPoint); }
        }

        [TemplateReplaceId]
        private string AtlasUVRightTops
        {
            get { return MakeUVString(f => f.RightTopUVPoint); }
        }

        [TemplateReplaceId]
        private string AtlasUVRightBottoms
        {
            get { return MakeUVString(f => f.RightBottomUVPoint); }
        }

        [TemplateReplaceId]
        private string AtlasUVLeftBottoms
        {
            get { return MakeUVString(f => f.LeftBottomUVPoint); }
        }

        #endregion
    }
}

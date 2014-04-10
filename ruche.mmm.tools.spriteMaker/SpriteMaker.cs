using ruche.mmm.tools.spriteMaker.resources;
using System;
using System.IO;

namespace ruche.mmm.tools.spriteMaker
{
    /// <summary>
    /// スプライトファイル群を作成するクラス。
    /// </summary>
    public class SpriteMaker
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="effectFileConfig">エフェクトファイル設定。</param>
        /// <param name="textureAtlasFilePath">テクスチャアトラスファイルパス。</param>
        public SpriteMaker(
            EffectFileConfig effectFileConfig,
            string textureAtlasFilePath)
        {
            if (effectFileConfig == null)
            {
                throw new ArgumentNullException("effectFileConfig");
            }
            if (textureAtlasFilePath == null)
            {
                throw new ArgumentNullException("textureAtlasFilePath");
            }

            EffectFileConfig = effectFileConfig;

            var path = Path.GetFullPath(textureAtlasFilePath);
            BaseDirectoryPath = Path.GetDirectoryName(path);
            TextureAtlasFileName = Path.GetFileName(path);
            AccessoryFileName = Path.ChangeExtension(TextureAtlasFileName, "x");
            EffectFileName = Path.ChangeExtension(TextureAtlasFileName, "fx");
        }

        /// <summary>
        /// エフェクトファイル設定を取得する。
        /// </summary>
        public EffectFileConfig EffectFileConfig { get; private set; }

        /// <summary>
        /// ベースディレクトリパスを取得する。
        /// </summary>
        public string BaseDirectoryPath { get; private set; }

        /// <summary>
        /// テクスチャアトラスファイル名を取得する。
        /// </summary>
        public string TextureAtlasFileName { get; private set; }

        /// <summary>
        /// アクセサリファイル名を取得する。
        /// </summary>
        public string AccessoryFileName { get; private set; }

        /// <summary>
        /// エフェクトファイル名を取得する。
        /// </summary>
        public string EffectFileName { get; private set; }

        /// <summary>
        /// アクセサリファイルとエフェクトファイルを作成する。
        /// </summary>
        public void Make()
        {
            var atlasFilePath = Path.Combine(BaseDirectoryPath, TextureAtlasFileName);
            var accFilePath = Path.Combine(BaseDirectoryPath, AccessoryFileName);
            var effectFilePath = Path.Combine(BaseDirectoryPath, EffectFileName);

            // テクスチャアトラス読み込み
            var atlas = TextureAtlas.FromPlist(atlasFilePath);
            if (atlas == null)
            {
                throw new InvalidDataException(
                    string.Format(
                        Resources.SpriteMakerFormat_BadAtlasFile,
                        TextureAtlasFileName));
            }

            // アクセサリファイル書き出し
            var accMaker = new AccessoryFileMaker();
            accMaker.TextureFileName = atlas.ImageFileName;
            try
            {
                accMaker.Make(accFilePath);
            }
            catch
            {
                throw new Exception(
                    string.Format(
                        Resources.SpriteMakerFormat_AccessoryFileFail,
                        AccessoryFileName));
            }

            // エフェクトファイル書き出し
            var effectMaker = new EffectFileMaker();
            effectMaker.Config = EffectFileConfig;
            effectMaker.TextureAtlas = atlas;
            try
            {
                effectMaker.Make(effectFilePath);
            }
            catch
            {
                throw new Exception(
                    string.Format(
                        Resources.SpriteMakerFormat_EffectFileFail,
                        EffectFileName));
            }
        }
    }
}

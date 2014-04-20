using ruche.datas.textureAtlas;
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
        public SpriteMaker()
        {
            AccessoryFileConfig = new AccessoryFileConfig();
            EffectFileConfig = new EffectFileConfig();
        }

        /// <summary>
        /// テクスチャアトラスローダを取得または設定する。
        /// </summary>
        public Func<string, TextureAtlas> TextureAtlasLoader { get; set; }

        /// <summary>
        /// テクスチャアトラスの入力元パスを取得または設定する。
        /// </summary>
        public string InputTextureAtlasPath { get; set; }

        /// <summary>
        /// 実際に使われるテクスチャアトラスの入力元パスを取得する。
        /// </summary>
        /// <returns>
        /// テクスチャアトラスの入力元パス。
        /// TextureAtlasFilePath に無効な値が入っている場合は null 。
        /// </returns>
        public string GetInputTextureAtlasPath()
        {
            return
                MakerUtil.IsValidPath(InputTextureAtlasPath) ?
                    Path.GetFullPath(InputTextureAtlasPath) : null;
        }

        /// <summary>
        /// アクセサリファイル設定を取得または設定する。
        /// </summary>
        public AccessoryFileConfig AccessoryFileConfig { get; set; }

        /// <summary>
        /// エフェクトファイル設定を取得または設定する。
        /// </summary>
        public EffectFileConfig EffectFileConfig { get; set; }

        /// <summary>
        /// アクセサリファイルの出力先パスを取得または設定する。
        /// </summary>
        /// <remarks>
        /// null ならばテクスチャアトラスの入力元パスの拡張子を
        /// ".x" に書き換えたパスを用いる。
        /// </remarks>
        public string OutputAccessoryFilePath { get; set; }

        /// <summary>
        /// 実際に使われるアクセサリファイルの出力先パスを取得する。
        /// </summary>
        /// <returns>
        /// アクセサリファイルの出力先パス。
        /// OutputAccessoryFilePath に null 以外の無効な値が入っている場合や、
        /// OutputAccessoryFilePath が null かつ
        /// TextureAtlasFilePath に無効な値が入っている場合は null 。
        /// </returns>
        public string GetOutputAccessoryFilePath()
        {
            return MakeOutputFilePath(OutputAccessoryFilePath, ".x");
        }

        /// <summary>
        /// エフェクトファイルの出力先パスを取得または設定する。
        /// </summary>
        /// <remarks>
        /// null ならばテクスチャアトラスの入力元パスの拡張子を
        /// ".fx" に書き換えたパスを用いる。
        /// </remarks>
        public string OutputEffectFilePath { get; set; }

        /// <summary>
        /// 実際に使われるエフェクトファイルの出力先パスを取得する。
        /// </summary>
        /// <returns>
        /// エフェクトファイルの出力先パス。
        /// OutputEffectFilePath に null 以外の無効な値が入っている場合や、
        /// OutputEffectFilePath が null かつ
        /// TextureAtlasFilePath に無効な値が入っている場合は null 。
        /// </returns>
        public string GetOutputEffectFilePath()
        {
            return MakeOutputFilePath(OutputEffectFilePath, ".fx");
        }

        /// <summary>
        /// アクセサリファイルとエフェクトファイルを作成する。
        /// </summary>
        /// <remarks>
        /// 事前に下記のプロパティに有効な値を設定しておく必要がある。
        /// 
        /// <list type="bullet">
        /// <item><description>TextureAtlasLoader</description></item>
        /// <item><description>InputTextureAtlasPath</description></item>
        /// <item><description>AccessoryFileConfig</description></item>
        /// <item><description>EffectFileConfig</description></item>
        /// </list>
        /// </remarks>
        public void Make()
        {
            if (TextureAtlasLoader == null)
            {
                throw new InvalidOperationException("TextureAtlasLoader is null.");
            }
            var atlasPath = GetInputTextureAtlasPath();
            if (atlasPath == null)
            {
                throw new InvalidOperationException("InputTextureAtlasPath is invalid.");
            }
            if (AccessoryFileConfig == null)
            {
                throw new InvalidOperationException("AccessoryFileConfig is null.");
            }
            if (EffectFileConfig == null)
            {
                throw new InvalidOperationException("EffectFileConfig is null.");
            }
            var accFilePath = GetOutputAccessoryFilePath();
            if (accFilePath == null)
            {
                throw new InvalidOperationException("OutputAccessoryFilePath is invalid.");
            }
            var effectFilePath = GetOutputEffectFilePath();
            if (effectFilePath == null)
            {
                throw new InvalidOperationException("OutputEffectFilePath is invalid.");
            }

            // テクスチャアトラスファイル読み込み
            TextureAtlas atlas = LoadTextureAtlas(atlasPath);

            // アクセサリファイル書き出し
            var accMaker = new AccessoryFileMaker();
            accMaker.Config = AccessoryFileConfig;
            accMaker.TextureFileName = atlas.ImageFileName;
            try
            {
                accMaker.Make(accFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(
                        Resources.SpriteMakerFormat_AccessoryFileFail,
                        Path.GetFileName(accFilePath)),
                    ex);
            }

            // エフェクトファイル書き出し
            var effectMaker = new EffectFileMaker();
            effectMaker.Config = EffectFileConfig;
            effectMaker.TextureAtlas = atlas;
            try
            {
                effectMaker.Make(effectFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(
                        Resources.SpriteMakerFormat_EffectFileFail,
                        Path.GetFileName(effectFilePath)),
                    ex);
            }
        }

        /// <summary>
        /// テクスチャアトラスをロードする。
        /// </summary>
        /// <param name="atlasPath">入力元パス。</param>
        /// <returns>テクスチャアトラス。</returns>
        private TextureAtlas LoadTextureAtlas(string atlasPath)
        {
            try
            {
                var atlas = TextureAtlasLoader(atlasPath);
                if (atlas == null)
                {
                    throw new FileFormatException(
                        string.Format(
                            Resources.SpriteMakerFormat_BadAtlasFile,
                            Path.GetFileName(atlasPath)));
                }

                return atlas;
            }
            catch (FileFormatException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FileFormatException(
                    string.Format(
                        Resources.SpriteMakerFormat_BadAtlasFile,
                        Path.GetFileName(atlasPath)),
                    ex);
            }
        }

        /// <summary>
        /// 出力ファイルパスを作成する。
        /// </summary>
        /// <param name="filePath">
        /// 出力ファイルパス指定値。
        /// null ならばテクスチャアトラスファイルパスを基にする。
        /// </param>
        /// <param name="extension">拡張子。</param>
        /// <returns>出力ファイルパス。作成できなかった場合は null 。</returns>
        private string MakeOutputFilePath(string filePath, string extension)
        {
            if (filePath == null)
            {
                var atlasFilePath = GetInputTextureAtlasPath();
                if (atlasFilePath != null)
                {
                    return Path.ChangeExtension(atlasFilePath, extension);
                }
            }
            else if (MakerUtil.IsValidPath(filePath))
            {
                return Path.GetFullPath(filePath);
            }

            return null;
        }
    }
}

using ruche.datas.textureAtlas;
using ruche.datas.textureAtlas.loaders;
using ruche.mmm.tools.spriteMaker;
using System;
using System.IO;

namespace testApp
{
    class Program
    {
        /// <summary>
        /// テクスチャアトラスローダを取得または設定する。
        /// </summary>
        private static Func<string, TextureAtlas> TextureAtlasLoader { get; set; }

        /// <summary>
        /// テクスチャアトラスローダを取得する。
        /// </summary>
        /// <returns>テクスチャアトラスローダ。</returns>
        private static Func<string, TextureAtlas> GetTextureAtlasLoader()
        {
            // 初回呼び出し時に作成
            if (TextureAtlasLoader == null)
            {
                var loader = new Cocos2dTextureAtlasLoader();
                TextureAtlasLoader = loader.Load;
            }

            return TextureAtlasLoader;
        }

        /// <summary>
        /// テスト処理用の SpriteMaker インスタンスを作成する。
        /// </summary>
        /// <param name="testIndex">テスト番号。</param>
        /// <param name="textureAtlasPath">テクスチャアトラスパス。</param>
        /// <returns>SpriteMaker インスタンス。テスト処理範囲外ならば null 。</returns>
        private static SpriteMaker CreateSpriteMaker(
            int testIndex,
            string textureAtlasPath)
        {
            var renderTypes =
                Enum.GetValues(typeof(ImageRenderType)) as ImageRenderType[];
            var basePoints = Enum.GetValues(typeof(ImageBasePoint)) as ImageBasePoint[];
            var lightSettings = Enum.GetValues(typeof(LightSetting)) as LightSetting[];

            var effectFilePath = Path.ChangeExtension(textureAtlasPath, null);

            var renderType = renderTypes[testIndex % renderTypes.Length];
            testIndex /= renderTypes.Length;
            effectFilePath += "-rt" + (int)renderType;

            var basePoint = basePoints[testIndex % basePoints.Length];
            testIndex /= basePoints.Length;
            effectFilePath += "-bp" + (int)basePoint;

            var lightSetting = lightSettings[testIndex % lightSettings.Length];
            testIndex /= lightSettings.Length;
            effectFilePath += "-li" + (int)lightSetting;

            bool renderingBack = (testIndex % 2 != 0);
            testIndex /= 2;
            effectFilePath += renderingBack ? "-bk" : "";

            bool postEffect = (testIndex % 2 == 0);
            testIndex /= 2;
            effectFilePath += postEffect ? "-P" : "";

            if (testIndex > 0)
            {
                return null;
            }

            effectFilePath = Path.ChangeExtension(effectFilePath, ".fx");

            // ポストエフェクトなら対応するXファイルも生成
            string accFilePath =
                postEffect ? Path.ChangeExtension(effectFilePath, ".x") : null;

            return
                new SpriteMaker
                {
                    EffectFileConfig =
                        new EffectFileConfig
                        {
                            RenderType = renderType,
                            PostEffect = postEffect,
                            RenderingBack = renderingBack,
                            LightSetting = lightSetting,
                            BasePoint = basePoint,
                        },
                    TextureAtlasLoader = GetTextureAtlasLoader(),
                    InputTextureAtlasPath = textureAtlasPath,
                    OutputAccessoryFilePath = accFilePath,
                    OutputEffectFilePath = effectFilePath,
                };
        }

        /// <summary>
        /// エフェクトファイル設定が指定そのままに適用されるか否かを取得する。
        /// </summary>
        /// <param name="config">エフェクトファイル設定。</param>
        /// <returns>
        /// 指定そのままに適用されるならば true 。そうでなければ false 。
        /// </returns>
        private static bool IsEffectFileConfigApplied(EffectFileConfig config)
        {
            // RenderType 次第ではそのまま適用されない値をチェック
            return (
                config.LightSetting == config.GetLightSetting() &&
                config.RenderingBack == config.IsRenderingBack() &&
                config.PostEffect == config.IsPostEffect());
        }

        /// <summary>
        /// メインメソッド。
        /// </summary>
        static int Main(string[] args)
        {
            // 引数チェック
            if (args.Length <= 0)
            {
                Console.WriteLine("Usage: testApp textureAtlasFile");
                return 1;
            }

            // テスト処理
            for (int testIndex = 0; ; ++testIndex)
            {
                // SpriteMaker 作成
                var maker = CreateSpriteMaker(testIndex, args[0]);
                if (maker == null)
                {
                    // null が返ってきたらテスト処理終了
                    break;
                }

                // エフェクトファイル設定がそのまま適用されないならばスキップ
                if (!IsEffectFileConfigApplied(maker.EffectFileConfig))
                {
                    continue;
                }

                // ファイル出力
                var outName =
                    Path.GetFileName(
                        Path.ChangeExtension(maker.OutputEffectFilePath, null));
                Console.WriteLine("MAKE: {0}", outName);
                maker.Make();
                Console.WriteLine("  -> Done!");
            }

            return 0;
        }
    }
}

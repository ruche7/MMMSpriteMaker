using MMMSpriteMaker.resources;
using ruche.datas.textureAtlas;
using ruche.mmm.tools.spriteMaker;
using System;
using System.IO;
using System.Reflection;

namespace MMMSpriteMaker
{
    /// <summary>
    /// テクスチャアトラスローダを生成する静的クラス。
    /// </summary>
    public static class TextureAtlasLoaderFactory
    {
        /// <summary>
        /// テクスチャアトラスローダを生成する。
        /// </summary>
        /// <returns>テクスチャアトラスローダ。</returns>
        public static Func<string, TextureAtlas> Create()
        {
            if (Loader == null)
            {
                // プラグイン読み取り元パス
                // 1. 実行ファイルと同じ場所にある plugins ディレクトリ
                // 2. 実行ファイルと同じ場所
                var exeDir =
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var pluginsDir = Path.Combine(exeDir, "plugins");

                // ローダデリゲート作成
                Loader = TextureAtlasPluginLoader.CreateDelegate(pluginsDir, exeDir);
                if (Loader == null)
                {
                    throw new Exception(
                        Resources.TextureAtlasLoaderFactory_PluginNotFound);
                }
            }

            return Loader;
        }

        /// <summary>
        /// ローダデリゲートを取得または設定する。
        /// </summary>
        private static Func<string, TextureAtlas> Loader { get; set; }
    }
}

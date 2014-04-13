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
            if (InnerLoader == null)
            {
                // プラグイン読み取り元パス
                // 1. 実行ファイルと同じ場所にある plugins ディレクトリ
                // 2. 実行ファイルと同じ場所
                var exeDir =
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var pluginsDir = Path.Combine(exeDir, "plugins");

                // ローダ作成
                InnerLoader = TextureAtlasPluginLoader.Create(pluginsDir, exeDir);
                if (InnerLoader == null)
                {
                    throw new Exception(
                        Resources.TextureAtlasLoaderFactory_PluginNotFound);
                }
            }

            // デリゲートにして返す
            return (path => InnerLoader.Load(path));
        }

        /// <summary>
        /// 実処理を行うローダインスタンスを取得または設定する。
        /// </summary>
        private static TextureAtlasPluginLoader InnerLoader { get; set; }
    }
}

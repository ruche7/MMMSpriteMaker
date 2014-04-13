using ruche.datas.textureAtlas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ruche.mmm.tools.spriteMaker
{
    /// <summary>
    /// プラグインアセンブリ群を用いてテクスチャアトラスを作成するクラス。
    /// </summary>
    public class TextureAtlasPluginLoader
    {
        /// <summary>
        /// ITextureAtlasLoader 実装クラス情報クラス。
        /// </summary>
        protected class LoaderInfo
        {
            /// <summary>
            /// コンストラクタ。
            /// </summary>
            /// <param name="loaderType">ITextureAtlasLoader 実装クラス型。</param>
            public LoaderInfo(Type loaderType)
            {
                if (loaderType == null)
                {
                    throw new ArgumentNullException("loaderType");
                }

                LoaderType = loaderType;
            }

            /// <summary>
            /// ITextureAtlasLoader 実装クラス型を取得する。
            /// </summary>
            public Type LoaderType { get; private set; }

            /// <summary>
            /// ITextureAtlasLoader 実装クラスインスタンスを生成済みか否かを取得する。
            /// </summary>
            /// <remarks>
            /// ITextureAtlasLoader 実装クラスインスタンスは
            /// GetLoader メソッドの初回呼び出し時に生成される。
            /// </remarks>
            public bool IsLoaderCreated
            {
                get { return Loader != null; }
            }

            /// <summary>
            /// ITextureAtlasLoader 実装クラスインスタンスを取得する。
            /// </summary>
            /// <returns>ITextureAtlasLoader 実装クラスインスタンス。</returns>
            public ITextureAtlasLoader GetLoader()
            {
                // 初回呼び出し時に生成
                if (Loader == null)
                {
                    Loader = (ITextureAtlasLoader)Activator.CreateInstance(LoaderType);

                    // 型名デバッグ表示
                    Debug.WriteLine(
                        "CreateInstance: " +
                        ((Loader == null) ? "(null)" : Loader.GetType().FullName));
                }

                return Loader;
            }

            /// <summary>
            /// ITextureAtlasLoader 実装クラスインスタンスを取得または設定する。
            /// </summary>
            private ITextureAtlasLoader Loader { get; set; }
        }

        /// <summary>
        /// TextureAtlasPluginLoader インスタンスを作成する。
        /// </summary>
        /// <returns>
        /// TextureAtlasPluginLoader インスタンス。
        /// プラグインが1つも見つからなかった場合は null 。
        /// </returns>
        /// <remarks>
        /// 実行ファイルと同じ位置にあるプラグインファイルを検索する。
        /// </remarks>
        public static TextureAtlasPluginLoader Create()
        {
            return Create((IEnumerable<string>)null);
        }

        /// <summary>
        /// TextureAtlasPluginLoader インスタンスを作成する。
        /// </summary>
        /// <returns>
        /// TextureAtlasPluginLoader インスタンス。
        /// プラグインが1つも見つからなかった場合は null 。
        /// </returns>
        /// <param name="pluginDirectoryPathes">
        /// プラグインファイルの検索ディレクトリ一覧。
        /// </param>
        public static TextureAtlasPluginLoader Create(
            params string[] pluginDirectoryPathes)
        {
            return Create((IEnumerable<string>)pluginDirectoryPathes);
        }

        /// <summary>
        /// TextureAtlasPluginLoader インスタンスを作成する。
        /// </summary>
        /// <returns>
        /// TextureAtlasPluginLoader インスタンス。
        /// プラグインが1つも見つからなかった場合は null 。
        /// </returns>
        /// <param name="pluginDirectoryPathes">
        /// プラグインファイルの検索ディレクトリ列挙。
        /// null を渡した場合は実行ファイルと同じ位置にあるプラグインファイルを検索する。
        /// </param>
        public static TextureAtlasPluginLoader Create(
            IEnumerable<string> pluginDirectoryPathes)
        {
            // 検索先ディレクトリ列挙
            var dirPathes =
                pluginDirectoryPathes ??
                new string[]
                {
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                };

            // ローダ情報一覧作成
            var infos =
                dirPathes
                    // ディレクトリ内の *.dll ファイル一覧取得
                    .Where(path => Directory.Exists(path))
                    .SelectMany(path => Directory.GetFiles(path, "*.dll"))
                    // アセンブリ一覧作成
                    .Select(
                        file =>
                        {
                            try { return Assembly.LoadFrom(file); }
                            catch { }
                            return null;
                        })
                    .Where(asm => asm != null)
                    // ローダ情報一覧作成
                    .SelectMany(
                        asm =>
                            from type in asm.GetExportedTypes()
                            where
                                !type.IsInterface &&
                                !type.IsAbstract &&
                                typeof(ITextureAtlasLoader).IsAssignableFrom(type)
                            select new LoaderInfo(type));

            // 対象が1つも無ければ null を返す
            if (!infos.Any())
            {
                return null;
            }

            // インスタンス作成
            return new TextureAtlasPluginLoader(infos);
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="loaderInfos">LoaderInfo 列挙。</param>
        protected TextureAtlasPluginLoader(IEnumerable<LoaderInfo> loaderInfos)
        {
            if (loaderInfos == null)
            {
                throw new ArgumentNullException("loaderInfos");
            }

            LoaderInfos = loaderInfos;
        }

        /// <summary>
        /// ファイルまたはディレクトリからテクスチャアトラスを作成する。
        /// </summary>
        /// <param name="path">ファイルまたはディレクトリのパス。</param>
        /// <returns>テクスチャアトラス。作成できない場合は null 。</returns>
        public TextureAtlas Load(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            // ファイルかディレクトリか
            bool pathIsFile = File.Exists(path);
            bool pathIsDir = Directory.Exists(path);
            if (!pathIsFile && !pathIsDir)
            {
                // どちらでもない
                return null;
            }

            // テクスチャアトラス生成デリゲート作成
            Func<IEnumerable<LoaderInfo>, TextureAtlas> creater =
                infos => LoadByLoaderInfos(infos, path, pathIsFile, pathIsDir);

            // まずローダ生成済みのローダ情報を使って作成試行
            var atlas =
                creater(
                    from info in LoaderInfos
                    where info.IsLoaderCreated
                    let loader = info.GetLoader()
                    where
                        (pathIsFile && loader.CanLoadFile) ||
                        (pathIsDir && loader.CanLoadDirectory)
                    orderby loader.IsTypicalSourcePath(path) ? 0 : 1
                    select info);
            if (atlas != null)
            {
                return atlas;
            }

            // ローダ未生成のローダ情報を使って作成試行
            atlas =
                creater(
                    from info in LoaderInfos
                    where !info.IsLoaderCreated
                    select info);
            if (atlas != null)
            {
                return atlas;
            }

            return null;
        }

        /// <summary>
        /// ローダ情報列挙を用いてテクスチャアトラスを作成する。
        /// </summary>
        /// <param name="loaderInfos">ローダ情報列挙。</param>
        /// <param name="path">ファイルまたはディレクトリのパス。</param>
        /// <param name="pathIsFile">path がファイルならば true 。</param>
        /// <param name="pathIsDirectory">path がディレクトリならば true 。</param>
        /// <returns>テクスチャアトラス。作成できない場合は null 。</returns>
        private static TextureAtlas LoadByLoaderInfos(
            IEnumerable<LoaderInfo> loaderInfos,
            string path,
            bool pathIsFile,
            bool pathIsDirectory)
        {
            return
                loaderInfos
                    // ローダ取得
                    .Select(
                        info =>
                        {
                            try { return info.GetLoader(); }
                            catch { }
                            return null;
                        })
                    .Where(loader => loader != null)
                    // 条件チェック
                    .Where(
                        loader =>
                            (pathIsFile && loader.CanLoadFile) ||
                            (pathIsDirectory && loader.CanLoadDirectory))
                    // テクスチャアトラス作成
                    .Select(
                        loader =>
                        {
                            try { return loader.Load(path); }
                            catch { }
                            return null;
                        })
                    // 最初に作成成功したものを返す
                    .FirstOrDefault(atlas => atlas != null);
        }

        /// <summary>
        /// ITextureAtlasLoader 実装クラス情報列挙を取得または設定する。
        /// </summary>
        private IEnumerable<LoaderInfo> LoaderInfos { get; set; }
    }
}

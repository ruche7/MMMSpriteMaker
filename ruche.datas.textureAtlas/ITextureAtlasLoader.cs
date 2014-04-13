using System;

namespace ruche.datas.textureAtlas
{
    /// <summary>
    /// テクスチャアトラスを作成する機能を提供するインタフェース。
    /// </summary>
    public interface ITextureAtlasLoader
    {
        /// <summary>
        /// ファイルからのテクスチャアトラス作成をサポートするか否かを取得する。
        /// </summary>
        bool CanLoadFile { get; }

        /// <summary>
        /// ディレクトリからのテクスチャアトラス作成をサポートするか否かを取得する。
        /// </summary>
        bool CanLoadDirectory { get; }

        /// <summary>
        /// 実装クラスのソースとして一般的なパスであるか否かを取得する。
        /// </summary>
        /// <param name="path">ファイルまたはディレクトリのパス。</param>
        /// <returns>一般的なパスであるならば true 。そうでなければ false 。</returns>
        /// <remarks>
        /// 例えば特定の拡張子を持つなど、
        /// パスが実装クラスのソースとして一般的であるかどうかを示す。
        /// 
        /// 複数の実装クラスを使い分けてテクスチャアトラスを作成する場合に、
        /// このメソッドが true を返す実装クラスを優先的に呼び出すことで
        /// より効率的に作成できる場合がある。
        /// </remarks>
        bool IsTypicalSourcePath(string path);

        /// <summary>
        /// ファイルまたはディレクトリからテクスチャアトラスを作成する。
        /// </summary>
        /// <param name="path">ファイルまたはディレクトリのパス。</param>
        /// <returns>テクスチャアトラス。作成できない場合は null 。</returns>
        TextureAtlas Load(string path);
    }
}

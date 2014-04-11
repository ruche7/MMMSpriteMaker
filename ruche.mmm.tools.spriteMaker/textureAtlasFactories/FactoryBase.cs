using System;
using System.IO;

namespace ruche.mmm.tools.spriteMaker.textureAtlasFactories
{
    /// <summary>
    /// テクスチャアトラスを作成するクラスの基底クラス。
    /// </summary>
    public abstract class FactoryBase
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        protected FactoryBase() { }

        /// <summary>
        /// ストリームからテクスチャアトラスを作成する。
        /// </summary>
        /// <param name="stream">ストリーム。</param>
        /// <returns>テクスチャアトラス。</returns>
        public abstract TextureAtlas Load(Stream stream);

        /// <summary>
        /// ファイルからテクスチャアトラスを作成する。
        /// </summary>
        /// <param name="filePath">ファイルパス。</param>
        /// <returns>テクスチャアトラス。</returns>
        public TextureAtlas Load(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                return Load(stream);
            }
        }
    }
}

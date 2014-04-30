using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codeplex.Data;
using System.Windows;

namespace ruche.datas.textureAtlas.loaders
{
    /// <summary>
    /// Unity等で使われるJSON形式ファイルからテクスチャアトラスを作成するクラス。
    /// </summary>
    public sealed class JsonHashTextureAtlasLoader : ITextureAtlasLoader
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public JsonHashTextureAtlasLoader() { }

        #region ITextureAtlasLoader 実装

        /// <summary>
        /// ファイルからのテクスチャアトラス作成をサポートするか否かを取得する。
        /// </summary>
        /// <remarks>常に true を返す。</remarks>
        public bool CanLoadFile
        {
            get { return true; }
        }

        /// <summary>
        /// ディレクトリからのテクスチャアトラス作成をサポートするか否かを取得する。
        /// </summary>
        /// <remarks>常に false を返す。</remarks>
        public bool CanLoadDirectory
        {
            get { return false; }
        }

        /// <summary>
        /// ソースとして一般的なパスであるか否かを取得する。
        /// </summary>
        /// <param name="path">ファイルまたはディレクトリのパス。</param>
        /// <returns>
        /// 拡張子が ".json" または ".txt" であるならば true 。そうでなければ false 。
        /// </returns>
        public bool IsTypicalSourcePath(string path)
        {
            if (path != null)
            {
                var ext = Path.GetExtension(path).ToLower();
                return (ext == ".json" || ext == ".txt");
            }
            return false;
        }

        /// <summary>
        /// JSON形式ファイルからテクスチャアトラスを作成する。
        /// </summary>
        /// <param name="path">ファイルパス。</param>
        /// <returns>テクスチャアトラス。作成できない場合は null 。</returns>
        public TextureAtlas Load(string path)
        {
            try
            {
                var jsonText = File.ReadAllText(path, Encoding.Default);
                var json = DynamicJson.Parse(jsonText);

                string imgFileName = json.meta.image;
                Size imgSize = new Size(json.meta.size.w, json.meta.size.h);

                var frames = new List<TextureAtlasFrame>();
                foreach (KeyValuePair<string, dynamic> kv in json.frames)
                {
                    var f = kv.Value;
                    var frameRect =
                        new Rect(f.frame.x, f.frame.y, f.frame.w, f.frame.h);
                    var rotated = (bool)f.rotated;
                    var srcSize = new Size(f.sourceSize.w, f.sourceSize.h);
                    var trimPos =
                        new Point(f.spriteSourceSize.x, f.spriteSourceSize.y);
                    var frame =
                        TextureAtlasFrame.Create(
                            frameRect.Size,
                            frameRect.Location,
                            rotated,
                            srcSize,
                            trimPos);
                    frames.Add(frame);
                }

                return new TextureAtlas(imgFileName, imgSize, frames);
            }
            catch { }

            return null;
        }

        #endregion
    }
}

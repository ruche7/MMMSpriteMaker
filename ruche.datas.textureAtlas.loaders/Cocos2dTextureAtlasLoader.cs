using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;

namespace ruche.datas.textureAtlas.loaders
{
    /// <summary>
    /// Cocos2d形式ファイルからテクスチャアトラスを作成するクラス。
    /// </summary>
    /// <remarks>
    /// format 1 ～ 3 に対応している。 format 0 には対応しない。
    /// </remarks>
    public sealed class Cocos2dTextureAtlasLoader : ITextureAtlasLoader
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Cocos2dTextureAtlasLoader() { }

        /// <summary>
        /// サイズを表す plist 要素文字列値にマッチする正規表現。
        /// </summary>
        private static readonly Regex regexPlistSize =
            new Regex(@"^\s*\{\s*(\d+)\s*,\s*(\d+)\s*\}\s*$");

        /// <summary>
        /// 領域を表す plist 要素文字列値にマッチする正規表現。
        /// </summary>
        private static readonly Regex regexPlistRect =
            new Regex(
                @"^\s*{\s*" +
                @"\{\s*(\d+)\s*,\s*(\d+)\s*\}" +
                @"\s*,\s*" +
                @"\{\s*(\d+)\s*,\s*(\d+)\s*\}" +
                @"\s*\}\s*$");

        /// <summary>
        /// 子要素を取得する。
        /// </summary>
        /// <param name="owner">子要素のコンテナ。</param>
        /// <param name="name">子要素の名前。</param>
        /// <returns>子要素。</returns>
        /// <exception cref="ArgumentException">
        /// 指定した子要素が存在しない場合。
        /// </exception>
        private static XElement Get(XContainer owner, string name)
        {
            var child = owner.Element(name);
            if (child == null)
            {
                throw new ArgumentException("'<" + name + ">' is not found.", "name");
            }
            return child;
        }

        /// <summary>
        /// plist の各要素をパースしてディクショナリを作成する。
        /// </summary>
        /// <param name="elements">plist 要素の列挙。</param>
        /// <returns>パース結果のディクショナリ。</returns>
        private static Dictionary<string, dynamic> ParseElements(
            IEnumerable<XElement> elements)
        {
            int index = 0;
            return
                elements
                    .GroupBy(_ => index++ / 2)
                    .ToDictionary(
                        x => x.ElementAt(0).Value,
                        x => ParseValue(x.ElementAt(1)));
        }

        /// <summary>
        /// plist の各要素をパースして配列を作成する。
        /// </summary>
        /// <param name="elements">plist 要素の列挙。</param>
        /// <returns>パース結果の配列(リスト)。</returns>
        private static List<dynamic> ParseArray(IEnumerable<XElement> elements)
        {
            return elements.Select(e => ParseValue(e)).ToList();
        }

        /// <summary>
        /// plist の各要素値をパースする。
        /// </summary>
        /// <param name="value">plist 要素値。</param>
        /// <returns>パース結果値。</returns>
        private static dynamic ParseValue(XElement value)
        {
            switch (value.Name.ToString())
            {
            case "string":
                return value.Value;

            case "integer":
                return int.Parse(value.Value);

            case "real":
                return float.Parse(value.Value);

            case "true":
                return true;

            case "false":
                return false;

            case "dict":
                return ParseElements(value.Elements());

            case "array":
                return ParseArray(value.Elements());
            }

            throw new NotSupportedException(
                "Plist value type '" + value.Name + "' is not supported.");
        }

        /// <summary>
        /// サイズを表す plist 要素文字列値をパースする。
        /// </summary>
        /// <param name="value">plist 要素文字列値。</param>
        /// <returns>パース結果のサイズ値。</returns>
        private static Size ParseSizeString(string value)
        {
            var m = regexPlistSize.Match(value);
            if (!m.Success)
            {
                throw new ArgumentException(
                    "Cannot parse '" + value + "' to size.",
                    "value");
            }
            return new Size(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
        }

        /// <summary>
        /// 領域を表す plist 要素文字列値をパースする。
        /// </summary>
        /// <param name="value">plist 要素文字列値。</param>
        /// <returns>パース結果の領域値。</returns>
        private static Rect ParseRectString(string value)
        {
            var m = regexPlistRect.Match(value);
            if (!m.Success)
            {
                throw new ArgumentException(
                    "Cannot parse '" + value + "' to rectangle.",
                    "value");
            }
            return
                new Rect(
                    int.Parse(m.Groups[1].Value),
                    int.Parse(m.Groups[2].Value),
                    int.Parse(m.Groups[3].Value),
                    int.Parse(m.Groups[4].Value));
        }

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
        /// 拡張子が ".plist" であるならば true 。そうでなければ false 。
        /// </returns>
        public bool IsTypicalSourcePath(string path)
        {
            return (path != null && Path.GetExtension(path).ToLower() == ".plist");
        }

        /// <summary>
        /// Cocos2d形式ファイルからテクスチャアトラスを作成する。
        /// </summary>
        /// <param name="path">ファイルパス。</param>
        /// <returns>テクスチャアトラス。作成できない場合は null 。</returns>
        public TextureAtlas Load(string path)
        {
            try
            {
                var doc = XDocument.Load(path);
                var root = Get(Get(doc, "plist"), "dict");
                var dict = ParseElements(root.Elements());

                IDictionary<string, dynamic> meta = dict["metadata"];
                string imgFileName =
                    meta["textureFileName"] ?? meta["realTextureFileName"];
                Size imgSize = ParseSizeString(meta["size"]);

                // frames のフォーマットは format 値により異なる
                // format 値が無い場合は format=3 として扱う
                IEnumerable<TextureAtlasFrame> frames = null;
                if (meta.ContainsKey("format") && (int)meta["format"] < 3)
                {
                    // format=1, format=2
                    frames =
                        from kv in (dict["frames"] as IDictionary<string, dynamic>)
                        let f = kv.Value as IDictionary<string, dynamic>
                        let frameRect = ParseRectString(f["frame"] as string)
                        let rotated =
                            f.ContainsKey("rotated") ? (bool)f["rotated"] : false
                        let srcSize = ParseSizeString(f["sourceSize"] as string)
                        let trimRect = ParseRectString(f["sourceColorRect"] as string)
                        select
                            TextureAtlasFrame.Create(
                                frameRect.Size,
                                frameRect.Location,
                                rotated,
                                srcSize,
                                trimRect.Location);
                }
                else
                {
                    // format=3
                    frames =
                        from kv in (dict["frames"] as IDictionary<string, dynamic>)
                        let f = kv.Value as IDictionary<string, dynamic>
                        let frameSize = ParseSizeString(f["spriteSize"] as string)
                        let texRect = ParseRectString(f["textureRect"] as string)
                        let rotated = (bool)f["textureRotated"]
                        let srcSize = ParseSizeString(f["spriteSourceSize"] as string)
                        let trimRect = ParseRectString(f["spriteColorRect"] as string)
                        select
                            TextureAtlasFrame.Create(
                                frameSize,
                                texRect.Location,
                                rotated,
                                srcSize,
                                trimRect.Location);
                }

                return new TextureAtlas(imgFileName, imgSize, frames);
            }
            catch { }

            return null;
        }

        #endregion
    }
}

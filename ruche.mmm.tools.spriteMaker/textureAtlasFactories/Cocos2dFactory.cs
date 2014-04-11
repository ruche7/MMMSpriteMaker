﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;

namespace ruche.mmm.tools.spriteMaker.textureAtlasFactories
{
    /// <summary>
    /// Cocos2d形式ファイル(*.plist)からテクスチャアトラスを作成するクラス。
    /// </summary>
    public sealed class Cocos2dFactory : FactoryBase
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Cocos2dFactory() { }

        /// <summary>
        /// ストリームからテクスチャアトラスを作成する。
        /// </summary>
        /// <param name="stream">ストリーム。</param>
        /// <returns>テクスチャアトラス。</returns>
        public override TextureAtlas Load(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            try
            {
                var doc = XDocument.Load(stream);
                var root = Get(Get(doc, "plist"), "dict");
                var dict = ParseElements(root.Elements());

                Dictionary<string, dynamic> meta = dict["metadata"];
                string imgFileName =
                    meta["textureFileName"] ?? meta["realTextureFileName"];
                Size imgSize = ParseSizeString(meta["size"]);

                var frames =
                    from kv in (dict["frames"] as Dictionary<string, dynamic>)
                    let r = ParseRectString(kv.Value["frame"])
                    select new TextureAtlas.Frame(r.Location, r.Size, kv.Value["rotated"]);

                return new TextureAtlas(imgFileName, imgSize, frames);
            }
            catch (Exception ex)
            {
                throw new FileFormatException("Invalid file format.", ex);
            }
        }

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
                // not supported
                break;
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
    }
}

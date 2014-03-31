using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;

namespace MMMSpriteMaker
{
    partial class TextureAtlas
    {
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
		/// plist ファイルからテクスチャアトラスを構築する。
		/// </summary>
		/// <param name="filePath">plist ファイルパス。</param>
		/// <returns>テクスチャアトラス。失敗した場合は null 。</returns>
		public static TextureAtlas FromPlist(string filePath)
        {
            try
            {
                var doc = XDocument.Load(filePath);
                var root = doc.Element("plist").Element("dict");
                var dict = ParsePlistElements(root.Elements());

                Dictionary<string, dynamic> meta = dict["metadata"];
                string imgFileName =
                    meta["textureFileName"] ?? meta["realTextureFileName"];
                Size imgSize = ParsePlistSizeString(meta["size"]);

                var frames =
                    from kv in (dict["frames"] as Dictionary<string, dynamic>)
                    let r = ParsePlistRectString(kv.Value["frame"])
                    select new TextureAtlas.Frame(r.Location, r.Size, kv.Value["rotated"]);

                return new TextureAtlas(imgFileName, imgSize, frames);
            }
            catch { }

            return null;
        }

		/// <summary>
		/// plist の各要素をパースしてディクショナリを作成する。
		/// </summary>
		/// <param name="elements">plist 要素の列挙。</param>
		/// <returns>パース結果のディクショナリ。</returns>
        private static Dictionary<string, dynamic> ParsePlistElements(
			IEnumerable<XElement> elements)
        {
			int index = 0;
			return
				elements
					.GroupBy(e => index++ / 2)
					.ToDictionary(
						x => x.ElementAt(0).Value,
						x => ParsePlistValue(x.ElementAt(1)));
        }

		/// <summary>
		/// plist の各要素値をパースする。
		/// </summary>
		/// <param name="value">plist 要素値。</param>
		/// <returns>パース結果値。</returns>
		private static dynamic ParsePlistValue(XElement value)
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
                return ParsePlistElements(value.Elements());

            case "array":
				// not supported
                break;
            }

            throw new ArgumentException(
                "Plist value type '" + value.Name + "' is not supported.",
                "value");
        }

		/// <summary>
		/// サイズを表す plist 要素文字列値をパースする。
		/// </summary>
        /// <param name="value">plist 要素文字列値。</param>
		/// <returns>パース結果のサイズ値。</returns>
        private static Size ParsePlistSizeString(string value)
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
        private static Rect ParsePlistRectString(string value)
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

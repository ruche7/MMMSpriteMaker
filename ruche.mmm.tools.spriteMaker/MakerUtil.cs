using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ruche.mmm.tools.spriteMaker
{
    /// <summary>
    /// 静的ユーティリティクラス。
    /// </summary>
    internal static class MakerUtil
    {
        /// <summary>
        /// 有効なパス文字列であるか否かを取得する。
        /// </summary>
        /// <param name="path">パス文字列。</param>
        /// <returns>有効ならば true 。そうでなければ false 。</returns>
        public static bool IsValidPath(string path)
        {
            return
                !string.IsNullOrWhiteSpace(path) &&
                path.IndexOfAny(Path.GetInvalidPathChars()) < 0;
        }
    }
}

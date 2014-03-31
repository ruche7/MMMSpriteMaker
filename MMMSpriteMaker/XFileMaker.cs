using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using Prop = MMMSpriteMaker.Properties;

namespace MMMSpriteMaker
{
    /// <summary>
    /// Xファイルを作成するクラス。
    /// </summary>
    public class XFileMaker
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public XFileMaker() { }

        /// <summary>
        /// 背面を描画するか否かを取得または設定する。
        /// </summary>
        public bool IsDrawBack { get; set; }

        /// <summary>
        /// テクスチャファイル名を取得または設定する。
        /// </summary>
        [TemplateReplaceId]
        public string TextureFileName { get; set; }

        /// <summary>
        /// 面の数を取得する。
        /// </summary>
        [TemplateReplaceId]
        private int MeshFaceCount
        {
            get { return IsDrawBack ? 2 : 1; }
        }

        /// <summary>
        /// 面の頂点リスト文字列を取得する。
        /// </summary>
        [TemplateReplaceId]
        private string MeshFaces
        {
            get
            {
                string value = " 4;0,1,2,3;";
                if (IsDrawBack)
                {
                    value += ",\n 4;1,0,3,2;";
                }
                return value;
            }
        }

        /// <summary>
        /// 面のマテリアルリスト文字列を取得する。
        /// </summary>
        [TemplateReplaceId]
        private string MeshFaceMaterials
        {
            get
            {
                string value = "  0";
                if (IsDrawBack)
                {
                    value += ",\n  0";
                }
                return value;
            }
        }

        /// <summary>
        /// 面の法線リスト文字列を取得する。
        /// </summary>
        [TemplateReplaceId]
        private string MeshFaceNormals
        {
            get
            {
                string value = "  4;0,0,0,0;";
                if (IsDrawBack)
                {
                    value += ",\n  4;1,1,1,1;";
                }
                return value;
            }
        }

        /// <summary>
        /// Xファイルを作成する。
        /// </summary>
        /// <param name="filePath">ファイルパス。</param>
        public void Make(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Invalid file path.", "filePath");
            }
            if (string.IsNullOrWhiteSpace(TextureFileName))
            {
                throw new InvalidOperationException("Invalid texture file name.");
            }

            // 雛形文字列取得
            var code = Prop.Resources.SpriteAccessory;

            // 置換
            code = TemplateReplaceIdAttribute.Replace(this, code);

            // 書き出し
            File.WriteAllText(filePath, code, Encoding.GetEncoding(932));
        }
    }
}

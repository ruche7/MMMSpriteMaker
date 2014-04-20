using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ruche.mmm.tools.spriteMaker
{
    /// <summary>
    /// 設定値を保持するクラスの基底クラス。
    /// </summary>
    /// <remarks>
    /// 派生クラスで System.ComponentModel.DefaultValueAttribute 属性を持つ
    /// 公開プロパティを定義することでそれが設定値となる。
    /// 
    /// 設定値プロパティでは、プロパティと同じ名前を添字とするインデクサに対する
    /// 値の取得および設定を実装すること。
    /// 
    /// コンストラクタでは Reset メソッドを呼び出してすべての設定を初期値にすること。
    /// </remarks>
    [Serializable]
    public abstract class ConfigBase : INotifyPropertyChanged
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        protected ConfigBase()
        {
        }

        /// <summary>
        /// すべての設定を既定値に戻す。
        /// </summary>
        public void Reset()
        {
            foreach (var info in ConfigPropertyInfos)
            {
                var attrs =
                    info.GetCustomAttributes(typeof(DefaultValueAttribute), false)
                        as DefaultValueAttribute[];
                this[info.Name] = attrs[0].Value;
            }
        }

        /// <summary>
        /// すべての設定名のコレクションを取得する。
        /// </summary>
        /// <returns>すべての設定名の列挙。</returns>
        public ReadOnlyCollection<string> ConfigNames
        {
            get { return InfoCache.GetConfigNames(this.GetType()); }
        }

        /// <summary>
        /// すべての設定のプロパティ情報コレクションを取得する。
        /// </summary>
        public ReadOnlyCollection<PropertyInfo> ConfigPropertyInfos
        {
            get { return InfoCache.GetConfigPropertyInfos(this.GetType()); }
        }

        /// <summary>
        /// 指定した設定の既定値を取得する。
        /// </summary>
        /// <param name="name">設定名。</param>
        /// <returns>既定値。</returns>
        public dynamic GetDefaultValue(string name)
        {
            try
            {
                var info = this.GetType().GetProperty(name);
                var attrs =
                    info.GetCustomAttributes(typeof(DefaultValueAttribute), false)
                        as DefaultValueAttribute[];
                return attrs[0].Value;
            }
            catch
            {
                throw new ArgumentException("Invalid config name.", "name");
            }
        }

        /// <summary>
        /// 別のインスタンスとすべての設定値が等価であるか否かを取得する。
        /// </summary>
        /// <param name="other">比較対象。</param>
        /// <returns>
        /// すべての設定値が等価であるならば true 。そうでなければ false 。
        /// </returns>
        /// <remarks>
        /// すべての設定名が同じであれば型が異なる場合であっても比較を行う。
        /// </remarks>
        public bool EqualsAllValues(ConfigBase other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return
                ConfigNames.All(
                    name =>
                    {
                        dynamic a, b;
                        return
                            this.Source.TryGetValue(name, out a) &&
                            other.Source.TryGetValue(name, out b) &&
                            object.Equals(a, b);
                    });
        }

        /// <summary>
        /// 設定値を他のインスタンスにコピーする。
        /// </summary>
        /// <param name="dest">コピー先。</param>
        /// <remarks>
        /// 自身とコピー先の両方に存在する設定値についてコピーを行う。
        /// プロパティ名が同じであっても型が異なる場合はコピーを行わない。
        /// </remarks>
        public void CopyValuesTo(ConfigBase dest)
        {
            if (object.ReferenceEquals(dest, null))
            {
                throw new ArgumentNullException("dest");
            }

            if (object.ReferenceEquals(this, dest))
            {
                return;
            }

            foreach (var si in ConfigPropertyInfos)
            {
                if (
                    dest.ConfigPropertyInfos.Any(
                        di =>
                            di.Name == si.Name &&
                            di.PropertyType.Equals(si.PropertyType)))
                {
                    dynamic value;
                    if (this.Source.TryGetValue(si.Name, out value))
                    {
                        dest[si.Name] = value;
                    }
                }
            }
        }

        /// <summary>
        /// このインスタンスの文字列表現を取得する。
        /// </summary>
        /// <returns>このインスタンスの文字列表現。</returns>
        public override string ToString()
        {
            return
                string.Join(
                    Environment.NewLine,
                    ConfigNames.Select(
                        name =>
                        {
                            var value = (object)this[name];
                            string valueText =
                                object.ReferenceEquals(value, null) ?
                                    "(null)" : value.ToString();
                            return (name + "=" + valueText);
                        }));
        }

        /// <summary>
        /// 設定値が変更された時に呼び出されるイベント。
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 設定値を取得または設定するインデクサ。
        /// </summary>
        /// <param name="name">設定名。</param>
        /// <returns>設定値。</returns>
        protected dynamic this[string name]
        {
            get
            {
                dynamic value;
                return Source.TryGetValue(name, out value) ? value : null;
            }
            set
            {
                if (!ConfigNames.Contains(name))
                {
                    throw new ArgumentException("Invalid config name.", "name");
                }

                dynamic current;
                if (!Source.TryGetValue(name, out current) || !current.Equals(value))
                {
                    Source[name] = value;
                    NotifyPropertyChanged(name);
                }
            }
        }

        /// <summary>
        /// 設定値の変更を通知する。
        /// </summary>
        /// <param name="name">設定名。</param>
        /// <remarks>
        /// インデクサから呼び出されるため、通常は個別に呼び出す必要はない。
        /// インデクサに値を設定する以外の方法で設定値を変更できるようにする場合は
        /// このメソッドを呼び出して変更を通知する必要がある。
        /// </remarks>
        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// 派生クラスで ICloneable インタフェースを実装するためのヘルパメソッド。
        /// </summary>
        /// <typeparam name="Derived">派生クラス型。</typeparam>
        /// <returns>自身の設定値で初期化されたクローン。</returns>
        /// <remarks>
        /// 設定値のみがコピーされる。イベントはコピーされない。
        /// </remarks>
        protected Derived CloneCore<Derived>()
            where Derived : ConfigBase, new()
        {
            var dest = new Derived();
            CopyValuesTo(dest);
            return dest;
        }

        /// <summary>
        /// 設定値の保持先となるディクショナリを取得または設定する。
        /// </summary>
        private Dictionary<string, dynamic> Source
        {
            get { return source; }
            set { source = value ?? new Dictionary<string, dynamic>(); }
        }
        private Dictionary<string, dynamic> source = new Dictionary<string, dynamic>();

        #region ConfigBase 派生クラス設定コレクションキャッシュ

        /// <summary>
        /// ConfigBase 派生クラスの設定コレクションをキャッシュする静的クラス。
        /// </summary>
        private static class InfoCache
        {
            /// <summary>
            /// ConfigBase 派生クラスの設定値プロパティ情報コレクションを取得する。
            /// </summary>
            /// <param name="configType">ConfigBase 派生クラス型。</param>
            /// <returns>設定値プロパティ情報コレクション。</returns>
            public static ReadOnlyCollection<PropertyInfo> GetConfigPropertyInfos(
                Type configType)
            {
                return GetCacheData(configType).PropertyInfos;
            }

            /// <summary>
            /// ConfigBase 派生クラスの設定名コレクションを取得する。
            /// </summary>
            /// <param name="configType">ConfigBase 派生クラス型。</param>
            /// <returns>設定名コレクション。</returns>
            public static ReadOnlyCollection<string> GetConfigNames(Type configType)
            {
                return GetCacheData(configType).Names;
            }

            /// <summary>
            /// キャッシュデータクラス。
            /// </summary>
            private class CacheData
            {
                /// <summary>
                /// 設定値プロパティ情報コレクションを取得または設定する。
                /// </summary>
                public ReadOnlyCollection<PropertyInfo> PropertyInfos { get; set; }

                /// <summary>
                /// 設定名コレクションを取得または設定する。
                /// </summary>
                public ReadOnlyCollection<string> Names { get; set; }
            }

            /// <summary>
            /// キャッシュデータディクショナリ。
            /// </summary>
            private static readonly Dictionary<Type, CacheData> cacheDatas =
                new Dictionary<Type, CacheData>();

            /// <summary>
            /// キャッシュデータを取得する。
            /// </summary>
            /// <param name="configType">ConfigBase 派生クラス型。</param>
            /// <returns>キャッシュデータ。</returns>
            private static CacheData GetCacheData(Type configType)
            {
                CacheData data;
                if (!cacheDatas.TryGetValue(configType, out data))
                {
                    // 見つからなければ新規作成
                    data = new CacheData();
                    data.PropertyInfos =
                        configType.GetProperties()
                            .Where(
                                pi => pi.IsDefined(typeof(DefaultValueAttribute), false))
                            .ToList()
                            .AsReadOnly();
                    data.Names =
                        data.PropertyInfos.Select(pi => pi.Name).ToList().AsReadOnly();

                    // 追加
                    cacheDatas[configType] = data;
                }

                return data;
            }
        }

        #endregion
    }
}

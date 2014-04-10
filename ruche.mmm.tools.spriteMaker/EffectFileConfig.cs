using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ruche.mmm.tools.spriteMaker
{
    [Serializable]
    public sealed class EffectFileConfig
        : INotifyPropertyChanged, IEquatable<EffectFileConfig>
    {
        public static readonly float MinPixelRatio = 0.000001f;

        public static readonly float MinSpriteViewportWidth = 0.000001f;

        public static readonly float MinSpriteZRange = 0.000001f;

        public EffectFileConfig()
        {
            Reset();
        }

        [DefaultValue(typeof(ImageRenderType), "Sprite")]
        public ImageRenderType RenderType
        {
            get { return (ImageRenderType)this["RenderType"]; }
            set { this["RenderType"] = value; }
        }

        [DefaultValue(false)]
        public bool RenderingBack
        {
            get { return (bool)this["RenderingBack"]; }
            set { this["RenderingBack"] = value; }
        }

        public bool IsRenderingBack()
        {
            return
                RenderType.HasAnyFlags(ImageRenderTypeFlags.CanRenderBack) ?
                    RenderingBack :
                    false;
        }

        [DefaultValue(typeof(ImageBasePoint), "Center")]
        public ImageBasePoint BasePoint
        {
            get { return (ImageBasePoint)this["BasePoint"]; }
            set { this["BasePoint"] = value; }
        }

        [DefaultValue(typeof(ImageFlipSetting), "NotFlip")]
        public ImageFlipSetting HorizontalFlipSetting
        {
            get { return (ImageFlipSetting)this["HorizontalFlipSetting"]; }
            set { this["HorizontalFlipSetting"] = value; }
        }

        [DefaultValue(typeof(ImageFlipSetting), "NotFlip")]
        public ImageFlipSetting VerticalFlipSetting
        {
            get { return (ImageFlipSetting)this["VerticalFlipSetting"]; }
            set { this["VerticalFlipSetting"] = value; }
        }

        [DefaultValue(typeof(LightSetting), "Disabled")]
        public LightSetting LightSetting
        {
            get { return (LightSetting)this["LightSetting"]; }
            set { this["LightSetting"] = value; }
        }

        public LightSetting GetLightSetting()
        {
            return
                RenderType.HasAnyFlags(ImageRenderTypeFlags.CanUseLight) ?
                    LightSetting :
                    LightSetting.Disabled;
        }

        [DefaultValue(0.1f)]
        public float PixelRatio
        {
            get { return (float)this["PixelRatio"]; }
            set { this["PixelRatio"] = Math.Max(value, MinPixelRatio); }
        }

        public float GetPixelRatio()
        {
            return
                RenderType.HasAnyFlags(ImageRenderTypeFlags.UsePixelRatio) ?
                    PixelRatio :
                    (float)GetDefaultValue("PixelRatio");
        }

        [DefaultValue(45.0f)]
        public float SpriteViewportWidth
        {
            get { return (float)this["SpriteViewportWidth"]; }
            set { this["SpriteViewportWidth"] = Math.Max(value, MinSpriteViewportWidth); }
        }

        public float GetSpriteViewportWidth()
        {
            return
                RenderType.HasAnyFlags(ImageRenderTypeFlags.UseViewportWidth) ?
                    SpriteViewportWidth :
                    (float)GetDefaultValue("SpriteViewportWidth");
        }

        [DefaultValue(100.0f)]
        public float SpriteZRange
        {
            get { return (float)this["SpriteZRange"]; }
            set { this["SpriteZRange"] = Math.Max(value, MinSpriteZRange); }
        }

        public float GetSpriteZRange()
        {
            return
                RenderType.HasAnyFlags(ImageRenderTypeFlags.UseZRange) ?
                    SpriteZRange :
                    (float)GetDefaultValue("SpriteZRange");
        }

        public void Reset()
        {
            foreach (var info in GetConfigPropertyInfos())
            {
                var attrs =
                    info.GetCustomAttributes(typeof(DefaultValueAttribute), false)
                        as DefaultValueAttribute[];
                this[info.Name] = attrs[0].Value;
            }
        }

        public IEnumerable<string> GetConfigNames()
        {
            return GetConfigPropertyInfos().Select(info => info.Name);
        }

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

        public bool Equals(EffectFileConfig other)
        {
            if (
                object.ReferenceEquals(other, null) ||
                Source.Count != other.Source.Count)
            {
                return false;
            }

            return
                Source.All(
                    kv =>
                    {
                        dynamic value;
                        return
                            other.Source.TryGetValue(kv.Key, out value) &&
                            kv.Value.Equals(value);
                    });
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EffectFileConfig);
        }

        public override int GetHashCode()
        {
            return
                (int)RenderType ^
                (RenderingBack ? -1 : 0) ^
                ((int)BasePoint * 8) ^
                ((int)HorizontalFlipSetting * 16) ^
                ((int)VerticalFlipSetting * 32) ^
                ((int)LightSetting * 64) ^
                PixelRatio.GetHashCode() ^
                SpriteViewportWidth.GetHashCode() ^
                SpriteZRange.GetHashCode();
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private Dictionary<string, dynamic> Source
        {
            get { return source; }
        }
        private Dictionary<string, dynamic> source = new Dictionary<string, dynamic>();

        private dynamic this[string name]
        {
            get
            {
                dynamic value;
                return Source.TryGetValue(name, out value) ? value : null;
            }
            set
            {
                if (!Source.ContainsKey(name) || !Source[name].Equals(value))
                {
                    Source[name] = value;
                    NotifyPropertyChanged(name);
                }
            }
        }

        private IEnumerable<PropertyInfo> GetConfigPropertyInfos()
        {
            return
                from info in this.GetType().GetProperties()
                where info.IsDefined(typeof(DefaultValueAttribute), false)
                select info;
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}

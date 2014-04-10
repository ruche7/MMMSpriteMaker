using ruche.mmm.tools.spriteMaker;
using ruche.wpf.viewModel;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace MMMSpriteMaker.ViewModel
{
    /// <summary>
    /// 設定画面の ViewModel クラス。
    /// </summary>
    public class ConfigViewModel : ViewModelBase
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public ConfigViewModel() : this(new EffectFileConfig())
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="config">エフェクトファイル設定。</param>
        public ConfigViewModel(EffectFileConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            // エフェクトファイル設定を設定
            Config = config;

            // コマンド作成
            ResetCommand = new DelegateCommand(_ => Config.Reset());

            // エフェクトファイル設定変更時の処理を登録
            Config.PropertyChanged += Config_PropertyChanged;

            // RenderType 関連プロパティ初期化
            ChangeRenderTypeStatus(Config.RenderType);
        }

        /// <summary>
        /// エフェクトファイル設定を取得する。
        /// </summary>
        public EffectFileConfig Config { get; private set; }

        /// <summary>
        /// イメージの描画方法を取得または設定する。
        /// </summary>
        public ImageRenderType RenderType
        {
            get { return Config.RenderType; }
            set { Config.RenderType = value; }
        }

        /// <summary>
        /// イメージの描画方法に対して背面描画を指定可能か否かを取得する。
        /// </summary>
        public bool CanRenderBack
        {
            get { return canRenderBack; }
            private set
            {
                if (value != canRenderBack)
                {
                    canRenderBack = value;
                    NotifyPropertyChanged("CanRenderBack");
                }
            }
        }
        private bool canRenderBack = false;

        /// <summary>
        /// 背面を描画するか否かを取得または設定する。
        /// </summary>
        public bool RenderingBack
        {
            get { return Config.RenderingBack; }
            set { Config.RenderingBack = value; }
        }

        /// <summary>
        /// イメージの描画方法に対してライトを有効化可能か否かを取得する。
        /// </summary>
        public bool CanUseLight
        {
            get { return canUseLight; }
            private set
            {
                if (value != canUseLight)
                {
                    canUseLight = value;
                    NotifyPropertyChanged("CanUseLight");
                }
            }
        }
        private bool canUseLight = false;

        /// <summary>
        /// ライトとセルフシャドウの有効設定を取得または設定する。
        /// </summary>
        public LightSetting LightSetting
        {
            get { return Config.LightSetting; }
            set { Config.LightSetting = value; }
        }

        /// <summary>
        /// イメージの描画方法に対してピクセル倍率値を利用するか否かを取得する。
        /// </summary>
        public bool UsePixelRatio
        {
            get { return usePixelRatio; }
            private set
            {
                if (value != usePixelRatio)
                {
                    usePixelRatio = value;
                    NotifyPropertyChanged("UsePixelRatio");
                }
            }
        }
        private bool usePixelRatio = false;

        /// <summary>
        /// ピクセル倍率の最小値を取得する。
        /// </summary>
        public static float MinPixelRatio
        {
            get { return EffectFileConfig.MinPixelRatio; }
        }

        /// <summary>
        /// ピクセル倍率値を取得または設定する。
        /// </summary>
        public float PixelRatio
        {
            get { return Config.PixelRatio; }
            set { Config.PixelRatio = value; }
        }

        /// <summary>
        /// イメージの描画方法に対してビューポート幅を利用するか否かを取得する。
        /// </summary>
        public bool UseSpriteViewportWidth
        {
            get { return useSpriteViewportWidth; }
            private set
            {
                if (value != useSpriteViewportWidth)
                {
                    useSpriteViewportWidth = value;
                    NotifyPropertyChanged("UseSpriteViewportWidth");
                }
            }
        }
        private bool useSpriteViewportWidth = false;

        /// <summary>
        /// ビューポート幅の最小値を取得する。
        /// </summary>
        public static float MinSpriteViewportWidth
        {
            get { return EffectFileConfig.MinSpriteViewportWidth; }
        }

        /// <summary>
        /// ビューポート幅を取得または設定する。
        /// </summary>
        public float SpriteViewportWidth
        {
            get { return Config.SpriteViewportWidth; }
            set { Config.SpriteViewportWidth = value; }
        }

        /// <summary>
        /// イメージの描画方法に対してZオーダー範囲値を利用するか否かを取得する。
        /// </summary>
        public bool UseSpriteZRange
        {
            get { return useSpriteZRange; }
            private set
            {
                if (value != useSpriteZRange)
                {
                    useSpriteZRange = value;
                    NotifyPropertyChanged("UseSpriteZRange");
                }
            }
        }
        private bool useSpriteZRange = false;

        /// <summary>
        /// Zオーダー範囲の最小値を取得する。
        /// </summary>
        public static float MinSpriteZRange
        {
            get { return EffectFileConfig.MinSpriteZRange; }
        }

        /// <summary>
        /// Zオーダー範囲値を取得または設定する。
        /// </summary>
        public float SpriteZRange
        {
            get { return Config.SpriteZRange; }
            set { Config.SpriteZRange = value; }
        }

        /// <summary>
        /// イメージの基準点位置を取得または設定する。
        /// </summary>
        public ImageBasePoint BasePoint
        {
            get { return Config.BasePoint; }
            set { Config.BasePoint = value; }
        }

        /// <summary>
        /// イメージの左右反転設定値を取得または設定する。
        /// </summary>
        public ImageFlipSetting HorizontalFlipSetting
        {
            get { return Config.HorizontalFlipSetting; }
            set { Config.HorizontalFlipSetting = value; }
        }

        /// <summary>
        /// イメージの上下反転設定値を取得または設定する。
        /// </summary>
        public ImageFlipSetting VerticalFlipSetting
        {
            get { return Config.VerticalFlipSetting; }
            set { Config.VerticalFlipSetting = value; }
        }

        /// <summary>
        /// すべての設定を既定値に戻すコマンドを取得する。
        /// </summary>
        public ICommand ResetCommand { get; private set; }

        /// <summary>
        /// アプリケーション設定の変更時に呼び出される。
        /// </summary>
        private void Config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // 自身に通知
            NotifyPropertyChanged(e.PropertyName);

            // RenderType なら関連プロパティ変更
            if (e.PropertyName == "RenderType")
            {
                ChangeRenderTypeStatus(Config.RenderType);
            }
        }

        /// <summary>
        /// イメージの描画方法に応じて変化するプロパティを設定する。
        /// </summary>
        /// <param name="renderType">イメージの描画方法。</param>
        private void ChangeRenderTypeStatus(ImageRenderType renderType)
        {
            CanRenderBack = renderType.HasAnyFlags(ImageRenderTypeFlags.CanRenderBack);
            CanUseLight = renderType.HasAnyFlags(ImageRenderTypeFlags.CanUseLight);
            UsePixelRatio = renderType.HasAnyFlags(ImageRenderTypeFlags.UsePixelRatio);
            UseSpriteViewportWidth =
                renderType.HasAnyFlags(ImageRenderTypeFlags.UseViewportWidth);
            UseSpriteZRange = renderType.HasAnyFlags(ImageRenderTypeFlags.UseZRange);
        }
    }
}

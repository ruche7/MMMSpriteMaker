using MMMSpriteMaker.Properties;
using ruche.mmm.tools.spriteMaker;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using res = MMMSpriteMaker.resources;

namespace MMMSpriteMaker
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// アプリケーション設定保存ファイルパス。
        /// </summary>
        private static readonly string SettingsXamlFilePath =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                @"ruche-home\" + typeof(App).Namespace + @"\Settings.xaml");

        /// <summary>
        /// アプリケーション設定を取得または設定する。
        /// </summary>
        public static Settings Settings { get; set; }

        /// <summary>
        /// エフェクトファイル設定を取得または設定する。
        /// </summary>
        private static EffectFileConfig EffectFileConfig
        {
            get { return Settings.EffectFileConfig; }
            set { Settings.EffectFileConfig = value; }
        }

        /// <summary>
        /// アプリケーション設定を読み込む。
        /// </summary>
        private static void LoadSettings()
        {
            var settings =
                SettingsUtil.LoadFromXaml(SettingsXamlFilePath) ??
                new Settings();

            Settings = Settings.Synchronized(settings) as Settings;
        }

        /// <summary>
        /// アプリケーション設定を保存する。
        /// </summary>
        private static void SaveSettings()
        {
            Settings.SaveToXaml(SettingsXamlFilePath);
        }

        /// <summary>
        /// OKボタン付きのダイアログを表示する。
        /// </summary>
        /// <param name="message">表示メッセージ。</param>
        /// <param name="image">表示アイコン。</param>
        public static void ShowAlert(string message, MessageBoxImage image)
        {
            MessageBox.Show(
                message,
                res.Resources.Dialog_Caption,
                MessageBoxButton.OK,
                image);
        }

        /// <summary>
        /// アプリケーション設定ファイルの排他保存用ミューテクス。
        /// </summary>
        private Mutex mutexForSettingFile =
            new Mutex(false, "{888E7C5E-D045-4537-8609-815A19AB268C}");

        /// <summary>
        /// 設定ウィンドウの多重起動防止用のミューテクス。
        /// </summary>
        private Mutex mutexForWindow =
            new Mutex(false, "{876286C1-80D7-44FB-B3AF-54AE29125810}");

        /// <summary>
        /// アプリケーション設定をプロセス間排他で初期化する。
        /// </summary>
        private void InitializeSettings()
        {
            try
            {
                mutexForSettingFile.WaitOne();

                // 読み込み
                LoadSettings();

                bool needSave = false;

                // エフェクトファイル設定が null ならば初期化
                if (EffectFileConfig == null)
                {
                    EffectFileConfig = new EffectFileConfig();
                    needSave = true;
                }

                // 必要であれば保存
                if (needSave)
                {
                    SaveSettings();
                }
            }
            finally
            {
                mutexForSettingFile.ReleaseMutex();
            }

            // 設定値が変更されたら即保存するようにする
            Settings.EffectFileConfig.PropertyChanged += OnSettingsPropertyChanged;
            Settings.PropertyChanged += OnSettingsPropertyChanged;
        }

        /// <summary>
        /// Settings および Settings.EffectFileConfig の内容が変更された時に呼び出される。
        /// </summary>
        private void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // プロセス間排他で保存
            try
            {
                mutexForSettingFile.WaitOne();

                SaveSettings();
            }
            finally
            {
                mutexForSettingFile.ReleaseMutex();
            }

            // イベント内容デバッグ表示
            Debug.WriteLine(
                "PropertyChanged: {0}.{1}",
                sender.GetType().Name,
                e.PropertyName);
        }

        /// <summary>
        /// プログラム引数を処理する。
        /// </summary>
        /// <param name="args"></param>
        private void ProcessArgs(string[] args)
        {
            // 引数チェック
            if (args == null || args.Length <= 0)
            {
                ShowAlert(res.Resources.Dialog_ArgsFail, MessageBoxImage.Error);
                Shutdown(1);
                return;
            }

            try
            {
                // テクスチャアトラスローダ作成
                var loader = TextureAtlasLoaderFactory.Create();

                // 作成ウィンドウ起動
                var makerWindow =
                    new View.MakerWindow(
                        new ViewModel.MakerViewModel(EffectFileConfig, loader, args));
                makerWindow.Show();
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message, MessageBoxImage.Error);
                Shutdown(1);
                return;
            }
        }

        /// <summary>
        /// アプリケーション起動時に呼び出される。
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // アプリケーション設定初期化
            InitializeSettings();

            // 引数があればそれらを処理して終了
            if (e.Args.Length > 0)
            {
                ProcessArgs(e.Args);
                return;
            }

            // 設定ウィンドウの多重起動防止
            if (!mutexForWindow.WaitOne(0, false))
            {
                ShowAlert(
                    res.Resources.Dialog_ConfigWindowMultiBootFail,
                    MessageBoxImage.Warning);
                Shutdown(1);
                return;
            }

            // 設定ウィンドウ起動
            var configWindow =
                new View.ConfigWindow(new ViewModel.ConfigViewModel(EffectFileConfig));
            configWindow.Show();
        }

        /// <summary>
        /// アプリケーション終了時に呼び出される。
        /// </summary>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // 念のためイベントを破棄
            Settings.PropertyChanged -= OnSettingsPropertyChanged;
            Settings.EffectFileConfig.PropertyChanged -= OnSettingsPropertyChanged;

            // ミューテクス破棄
            if (mutexForWindow != null)
            {
                mutexForWindow.Dispose();
                mutexForWindow = null;
            }
            if (mutexForSettingFile != null)
            {
                mutexForSettingFile.Dispose();
                mutexForSettingFile = null;
            }
        }
    }
}

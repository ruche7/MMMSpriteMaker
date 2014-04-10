using ruche.mmm.tools.spriteMaker;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using Prop = MMMSpriteMaker.Properties;

namespace MMMSpriteMaker
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// OKボタン付きのダイアログを表示する。
        /// </summary>
        /// <param name="message">表示メッセージ。</param>
        /// <param name="image">表示アイコン。</param>
        private static void ShowAlert(string message, MessageBoxImage image)
        {
            MessageBox.Show(
                message,
                Prop.Resources.Dialog_Caption,
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
        /// アプリケーション設定を取得する。
        /// </summary>
        private Prop.Settings Settings
        {
            get { return Prop.Settings.Default; }
        }

        /// <summary>
        /// エフェクトファイル設定を取得する。
        /// </summary>
        private EffectFileConfig EffectFileConfig
        {
            get { return Settings.EffectFileConfig; }
        }

        /// <summary>
        /// アプリケーション設定をプロセス間排他で初期化する。
        /// </summary>
        private void InitializeSettings()
        {
            try
            {
                mutexForSettingFile.WaitOne();

                // 読み込み
                Settings.Reload();

                bool needSave = false;

                // 未アップグレードの場合のみアップグレードする
                if (!Settings.IsUpgraded)
                {
                    Settings.Upgrade();
                    Settings.IsUpgraded = true;
                    needSave = true;
                }

                // エフェクトファイル設定が null ならば初期化
                if (Settings.EffectFileConfig == null)
                {
                    Settings.EffectFileConfig = new EffectFileConfig();
                    needSave = true;
                }

                // 必要であれば保存
                if (needSave)
                {
                    Settings.Save();
                }
            }
            finally
            {
                mutexForSettingFile.ReleaseMutex();
            }

            // 設定値が変更されたら即保存するようにする
            Settings.PropertyChanged += (sender, e) => SaveSettings();
            Settings.EffectFileConfig.PropertyChanged += (sender, e) => SaveSettings();
        }

        /// <summary>
        /// アプリケーション設定をプロセス間排他で保存する。
        /// </summary>
        private void SaveSettings()
        {
            try
            {
                mutexForSettingFile.WaitOne();

                // 保存
                Settings.Save();
            }
            finally
            {
                mutexForSettingFile.ReleaseMutex();
            }
        }

        /// <summary>
        /// プログラム引数を処理する。
        /// </summary>
        /// <param name="args"></param>
        private void ProcessArgs(string[] args)
        {
            // 引数からファイルを抜き出す
            var files = args.Where(arg => File.Exists(arg)).ToArray();
            if (files.Length <= 0)
            {
                ShowAlert(Prop.Resources.Dialog_ArgsFail, MessageBoxImage.Error);
                Shutdown(1);
                return;
            }

            // 作成ウィンドウ起動
            var makerWindow =
                new View.MakerWindow(
                    new ViewModel.MakerViewModel(EffectFileConfig, files));
            makerWindow.Show();
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
                    Prop.Resources.Dialog_ConfigWindowMultiBootFail,
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

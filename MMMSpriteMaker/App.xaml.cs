using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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
        /// アプリケーション名。
        /// </summary>
        public static readonly string Name = GetAssemblyTitle() ?? "MMMSpriteMaker";

        /// <summary>
        /// アプリケーション設定を取得する。
        /// </summary>
        internal static Prop.Settings Settings
        {
            get { return Prop.Settings.Default; }
        }

        /// <summary>
        /// アセンブリタイトルを取得する。
        /// </summary>
        /// <returns>アセンブリタイトル。いわゆるアプリケーション名。</returns>
        private static string GetAssemblyTitle()
        {
            var attr =
                Attribute.GetCustomAttribute(
                    Assembly.GetExecutingAssembly(),
                    typeof(AssemblyTitleAttribute)) as AssemblyTitleAttribute;
            return (attr == null) ? null : attr.Title;
        }

        /// <summary>
        /// アプリケーション設定ファイルの排他保存用ミューテクス。
        /// </summary>
        private Mutex mutexForSettingFile =
            new Mutex(false, "{888E7C5E-D045-4537-8609-815A19AB268C}");

        /// <summary>
        /// メインウィンドウの多重起動防止用のミューテクス。
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
                Settings.Reload();

                // 未アップグレードの場合のみアップグレードする
                if (!Settings.IsUpgraded)
                {
                    Settings.Upgrade();
                    Settings.IsUpgraded = true;
                    Settings.Save();
                }
            }
            finally
            {
                mutexForSettingFile.ReleaseMutex();
            }

            // 設定値が変更されたら即保存するようにする
            Settings.PropertyChanged += Settings_PropertyChanged;
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
        /// アプリケーション設定値が変更された時に呼び出される。
        /// </summary>
        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // 即ファイルへ保存
            SaveSettings();
        }

        /// <summary>
        /// アプリケーション起動時に呼び出される。
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 設定初期化
            InitializeSettings();

            // 引数があればそれらを処理して終了
            if (e.Args.Length > 0)
            {
                // TODO: 引数のファイルを処理

                Shutdown(0);
                return;
            }

            // メインウィンドウの多重起動防止
            if (!mutexForWindow.WaitOne(0, false))
            {
                MessageBox.Show(
                    Prop.Resources.Dialog_MainWindowMultiBootFail,
                    Name,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                Shutdown(1);
                return;
            }

            // メインウィンドウ起動
            (new MainWindow()).Show();
        }

        /// <summary>
        /// アプリケーション終了時に呼び出される。
        /// </summary>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // 設定値変更時の処理をやめる
            Settings.PropertyChanged -= Settings_PropertyChanged;

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

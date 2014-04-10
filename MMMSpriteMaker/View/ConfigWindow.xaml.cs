using MMMSpriteMaker.ViewModel;
using ruche.mmm.tools.spriteMaker;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MMMSpriteMaker.View
{
    /// <summary>
    /// ConfigWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ConfigWindow : Window
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="viewModel">ビューモデル。</param>
        public ConfigWindow(ConfigViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            InitializeComponent();

            // ビューモデル設定
            DataContext = viewModel;

            // エフェクトファイル設定を保持しておく
            EffectFileConfig = viewModel.Config;
        }

        /// <summary>
        /// エフェクトファイル設定を取得または設定する。
        /// </summary>
        private EffectFileConfig EffectFileConfig { get; set; }

        /// <summary>
        /// Close コマンドの Executed イベント発生時に呼び出される。
        /// </summary>
        private void OnCloseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// アイテムドラッグの前処理イベント発生時に呼び出される。
        /// </summary>
        private void Window_PreviewDragOver(object sender, DragEventArgs e)
        {
            var items = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (
                items != null &&
                items.Length > 0 &&
                items.Any(item => File.Exists(item)))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        /// <summary>
        /// アイテムドロップイベント発生時に呼び出される。
        /// </summary>
        private void Window_Drop(object sender, DragEventArgs e)
        {
            var items = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (items != null && items.Length > 0)
            {
                var files = items.Where(item => File.Exists(item)).ToArray();
                if (files.Length > 0)
                {
                    // ファイルを処理
                    var makerWindow =
                        new View.MakerWindow(
                            new ViewModel.MakerViewModel(EffectFileConfig, files));
                    makerWindow.Show();

                    e.Handled = true;
                }
            }
        }
    }
}

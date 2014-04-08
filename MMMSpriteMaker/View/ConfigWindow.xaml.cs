using MMMSpriteMaker.Properties;
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
        public ConfigWindow()
        {
            InitializeComponent();
        }

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
                            new ViewModel.MakerViewModel(Settings.Default, files));
                    makerWindow.Show();

                    e.Handled = true;
                }
            }
        }
    }
}

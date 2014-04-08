using MMMSpriteMaker.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace MMMSpriteMaker.View
{
    /// <summary>
    /// MakerWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MakerWindow : Window
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="viewModel">ビューモデル。</param>
        public MakerWindow(MakerViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            InitializeComponent();

            // ビューモデル設定
            DataContext = viewModel;
        }

        /// <summary>
        /// Close コマンドの Executed イベント発生時に呼び出される。
        /// </summary>
        private void OnCloseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}

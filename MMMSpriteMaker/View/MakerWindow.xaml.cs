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

            // 処理終了時に呼び出されるイベントを設定
            viewModel.RunFinished += OnRunFinished;
        }

        /// <summary>
        /// 処理終了時に呼び出される。
        /// </summary>
        private void OnRunFinished(object sender, EventArgs e)
        {
            var viewModel = DataContext as MakerViewModel;
            if (viewModel != null)
            {
                // イベント削除
                viewModel.RunFinished -= OnRunFinished;

                // 成功時に自動で閉じる かつ すべて成功
                if (
                    App.Settings.AutoCloseAtSucceeded &&
                    viewModel.SucceededCount >= viewModel.Makers.Count)
                {
                    // 閉じる
                    Close();
                }
            }
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

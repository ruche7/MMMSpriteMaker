﻿using MMMSpriteMaker.ViewModel;
using ruche.mmm.tools.spriteMaker;
using System;
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

            // ファイル設定を保持しておく
            AccessoryFileConfig = viewModel.AccessoryFileConfig;
            EffectFileConfig = viewModel.EffectFileConfig;
        }

        /// <summary>
        /// アクセサリファイル設定を取得または設定する。
        /// </summary>
        private AccessoryFileConfig AccessoryFileConfig { get; set; }

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
            if (items != null && items.Length > 0)
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
                try
                {
                    // テクスチャアトラスローダ作成
                    var loader = TextureAtlasLoaderFactory.Create();

                    // 作成ウィンドウ起動
                    var makerWindow =
                        new View.MakerWindow(
                            new ViewModel.MakerViewModel(
                                AccessoryFileConfig,
                                EffectFileConfig,
                                loader,
                                items));
                    makerWindow.Show();
                }
                catch (Exception ex)
                {
                    App.ShowAlert(ex.Message, MessageBoxImage.Error);
                }

                e.Handled = true;
            }
        }
    }
}

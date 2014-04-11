using MMMSpriteMaker.Properties;
using ruche.mmm.tools.spriteMaker;
using ruche.wpf.viewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MMMSpriteMaker.ViewModel
{
    /// <summary>
    /// 作成画面の ViewModel クラス。
    /// </summary>
    public class MakerViewModel : ViewModelBase
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="effectFileConfig">エフェクトファイル設定。</param>
        /// <param name="textureAtlasFilePathes">
        /// テクスチャアトラスファイルパス列挙。
        /// </param>
        public MakerViewModel(
            EffectFileConfig effectFileConfig,
            IEnumerable<string> textureAtlasFilePathes)
        {
            if (effectFileConfig == null)
            {
                throw new ArgumentNullException("effectFileConfig");
            }
            if (textureAtlasFilePathes == null)
            {
                throw new ArgumentNullException("textureAtlasFilePathes");
            }

            // SpriteMaker コレクション作成
            Makers =
                textureAtlasFilePathes
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .Select(p => Path.GetFullPath(p))
                    .Distinct() // あまりアテにならないが一応重複を弾く
                    .Select(
                        p =>
                            new SpriteMaker
                            {
                                TextureAtlasFilePath = p,
                                EffectFileConfig = effectFileConfig,
                            })
                    .ToList()
                    .AsReadOnly();
            if (Makers.Count <= 0)
            {
                throw new ArgumentException(
                    "textureAtlasFilePathes is invalid.",
                    "textureAtlasFilePathes");
            }

            // コマンド作成
            RunCommand =
                new DelegateCommand(
                    _ => ExecuteRunCommand(),
                    _ => !Running && !Finished);
            CancelCommand =
                new DelegateCommand(
                    _ => CancelTokenSource.Cancel(),
                    _ => CancelTokenSource != null);
            CopyLogCommand =
                new DelegateCommand(
                    _ =>
                        Clipboard.SetText(
                            string.Join(Environment.NewLine, LogLines) +
                            Environment.NewLine));

            // キャプション更新
            UpdateViewCaption();
        }

        /// <summary>
        /// SpriteMaker コレクションを取得する。
        /// </summary>
        public ReadOnlyCollection<SpriteMaker> Makers { get; private set; }

        /// <summary>
        /// ログ行コレクションを取得する。
        /// </summary>
        public ObservableCollection<string> LogLines
        {
            get { return logLines; }
        }
        private ObservableCollection<string> logLines =
            new ObservableCollection<string>();

        /// <summary>
        /// 処理成功カウンタ値を取得する。
        /// </summary>
        public int SucceededCount
        {
            get { return succeededCount; }
            private set
            {
                var v = Math.Max(value, 0);
                if (v != succeededCount)
                {
                    succeededCount = v;
                    NotifyPropertyChanged("SucceededCount");
                    UpdatePassedCount();
                }
            }
        }
        private int succeededCount = 0;

        /// <summary>
        /// 処理失敗カウンタ値を取得する。
        /// </summary>
        public int FailedCount
        {
            get { return failedCount; }
            private set
            {
                var v = Math.Max(value, 0);
                if (v != failedCount)
                {
                    failedCount = v;
                    NotifyPropertyChanged("FailedCount");
                    UpdatePassedCount();
                }
            }
        }
        private int failedCount = 0;

        /// <summary>
        /// 処理済みカウンタ値を取得する。
        /// </summary>
        public int PassedCount
        {
            get { return passedCount; }
            private set
            {
                var v = Math.Min(Math.Max(0, value), Makers.Count);
                if (v != passedCount)
                {
                    passedCount = v;
                    NotifyPropertyChanged("PassedCount");
                    UpdateViewCaption();
                }
            }
        }
        private int passedCount = 0;

        /// <summary>
        /// PassedCount を更新する。
        /// </summary>
        private void UpdatePassedCount()
        {
            PassedCount = SucceededCount + FailedCount;
        }

        /// <summary>
        /// ビューのキャプションとして利用可能な文字列を取得する。
        /// </summary>
        public string ViewCaption
        {
            get { return viewCaption; }
            private set
            {
                var v = value ?? "";
                if (v != viewCaption)
                {
                    viewCaption = v;
                    NotifyPropertyChanged("ViewCaption");
                }
            }
        }
        private string viewCaption = "";

        /// <summary>
        /// ViewCaption を更新する。
        /// </summary>
        private void UpdateViewCaption()
        {
            ViewCaption =
                string.Format(
                    "[{0}/{1}] {2}",
                    PassedCount,
                    Makers.Count,
                    Resources.MakerWindow_Caption);
        }

        /// <summary>
        /// 処理中であるか否かを取得する。
        /// </summary>
        public bool Running
        {
            get { return running; }
            private set
            {
                if (value != running)
                {
                    running = value;
                    NotifyPropertyChanged("Running");
                }
            }
        }
        private bool running = false;

        /// <summary>
        /// 処理終了済みであるか否かを取得する。
        /// </summary>
        public bool Finished
        {
            get { return finished; }
            private set
            {
                if (value != finished)
                {
                    finished = value;
                    NotifyPropertyChanged("Finished");
                }
            }
        }
        private bool finished = false;

        /// <summary>
        /// 処理開始コマンドを取得する。
        /// </summary>
        public ICommand RunCommand { get; private set; }

        /// <summary>
        /// 処理中断コマンドを取得する。
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// コピーログコマンドを取得する。
        /// </summary>
        public ICommand CopyLogCommand { get; private set; }

        /// <summary>
        /// 処理終了時に呼び出されるイベント。
        /// </summary>
        public event EventHandler RunFinished;

        /// <summary>
        /// UIスレッドでタスク処理を行うためのスケジューラ。
        /// </summary>
        private readonly TaskScheduler uiSyncScheduler =
            TaskScheduler.FromCurrentSynchronizationContext();

        /// <summary>
        /// 処理中断用トークンソースを取得または設定する。
        /// </summary>
        private CancellationTokenSource CancelTokenSource { get; set; }

        /// <summary>
        /// RunCommand の実行処理を行う。
        /// </summary>
        private void ExecuteRunCommand()
        {
            // パラメータ初期化
            LogLines.Clear();
            SucceededCount = 0;
            FailedCount = 0;
            Finished = false;
            Running = true;

            // タスク開始
            CancelTokenSource = new CancellationTokenSource();
            var runTask = Task.Factory.StartNew(Run, CancelTokenSource.Token);

            // 終了時タスク登録
            runTask.ContinueWith(
                _ => OnRunFinished(false),
                new CancellationToken(),
                TaskContinuationOptions.OnlyOnRanToCompletion,
                uiSyncScheduler);
            runTask.ContinueWith(
                _ => OnRunFinished(true),
                new CancellationToken(),
                TaskContinuationOptions.OnlyOnCanceled,
                uiSyncScheduler);
        }

        /// <summary>
        /// RunCommand の実処理を行う。
        /// </summary>
        private void Run()
        {
            foreach (var maker in Makers)
            {
                // 中断されていたら抜ける
                ThrowIfCanceled();

                try
                {
                    // 開始ログ追加
                    var atlasFilePath = maker.GetTextureAtlasFilePath();
                    var baseDirPath = Path.GetDirectoryName(atlasFilePath);
                    var atlasFileName = Path.GetFileName(atlasFilePath);
                    AddLogLines(
                        string.Format(
                            Resources.LogFormat_Begin,
                            PassedCount + 1,
                            Makers.Count,
                            baseDirPath,
                            atlasFileName));

                    // 作成処理実行
                    maker.Make();

                    // 成功ログ追加
                    var accessoryFileName =
                        Path.GetFileName(maker.GetOutputAccessoryFilePath());
                    var effectFileName =
                        Path.GetFileName(maker.GetOutputEffectFilePath());
                    AddLogLines(
                        string.Format(
                            Resources.LogFormat_Success,
                            accessoryFileName,
                            effectFileName));
                    IncrementPassedCount(true);
                }
                catch (Exception ex)
                {
                    // 失敗ログ追加
                    AddLogLines(string.Format(Resources.LogFormat_Fail, ex.Message));
                    IncrementPassedCount(false);
                }

                AddLogLines("--------------------");
            }
        }

        /// <summary>
        /// 処理が中断されている場合は例外を送出する。
        /// </summary>
        private void ThrowIfCanceled()
        {
            if (CancelTokenSource != null)
            {
                CancelTokenSource.Token.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// 処理終了時に呼び出される。
        /// </summary>
        /// <param name="canceled">処理を中断したならば true 。</param>
        private void OnRunFinished(bool canceled)
        {
            if (CancelTokenSource != null)
            {
                CancelTokenSource.Dispose();
                CancelTokenSource = null;
            }

            // ログ追加
            AddLogLines(
                string.Format(
                    canceled ?
                        Resources.LogFormat_Canceled :
                        Resources.LogFormat_Completed,
                    SucceededCount,
                    FailedCount,
                    Makers.Count - PassedCount));

            Finished = true;
            Running = false;

            // RunFinished イベント呼び出し
            CallRunFinishedEvent();
        }

        /// <summary>
        /// UIスレッド上でアクションを実行し、完了を待機する。
        /// </summary>
        /// <param name="action">実行するアクション。</param>
        private void DoTaskOnUIThread(Action action)
        {
            if (TaskScheduler.Current.Id == uiSyncScheduler.Id)
            {
                action();
            }
            else
            {
                var task = new Task(action);
                task.Start(uiSyncScheduler);
                task.Wait();
            }
        }

        /// <summary>
        /// UIスレッド上でログ行を追加する。
        /// </summary>
        /// <param name="lines">追加するログ文字列。改行で区切られる。</param>
        private void AddLogLines(string lines)
        {
            DoTaskOnUIThread(
                () =>
                {
                    using (var sr = new StringReader(lines))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            LogLines.Add(line);
                        }
                    }
                });
        }

        /// <summary>
        /// UIスレッド上で処理済みカウンタ値をインクリメントする。
        /// </summary>
        /// <param name="succeeded">
        /// 処理成功したならば true 。処理失敗したならば false 。
        /// </param>
        private void IncrementPassedCount(bool succeeded)
        {
            DoTaskOnUIThread(
                () =>
                {
                    if (succeeded)
                    {
                        ++SucceededCount;
                    }
                    else
                    {
                        ++FailedCount;
                    }
                });
        }

        /// <summary>
        /// UIスレッド上で RunFinished イベントを呼び出す。
        /// </summary>
        private void CallRunFinishedEvent()
        {
            DoTaskOnUIThread(
                () =>
                {
                    if (RunFinished != null)
                    {
                        RunFinished(this, EventArgs.Empty);
                    }
                });
        }
    }
}

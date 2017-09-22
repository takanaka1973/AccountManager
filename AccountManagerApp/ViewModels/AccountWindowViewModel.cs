using System.ComponentModel;

namespace AccountManagerApp
{
    /// <summary>
    /// アカウントウィンドウのビューモデル。
    /// </summary>
    /// <remarks>新規アカウントウィンドウ、アカウント編集ウィンドウのベースクラス。</remarks>
    public class AccountWindowViewModel : ViewModel
    {
        private readonly WindowManager _windowManager;
        private readonly AccountManager _accountManager;

        private readonly Account _targetAccount;    // may be null
        private readonly Account _editingAccount;

        private bool _canFinish;

        /// <summary>
        /// 編集中のアカウント。
        /// </summary>
        public Account EditingAccount
        {
            get
            {
                return _editingAccount;
            }
        }

        /// <summary>
        /// ウィンドウのタイトル。
        /// </summary>
        public string WindowTitle { get; }

        /// <summary>
        /// 編集完了コマンド。
        /// </summary>
        public DelegateCommand FinishCommand { get; }

        /// <summary>
        /// 編集キャンセルコマンド。
        /// </summary>
        public DelegateCommand CancelCommand { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <remarks>XAMLデザイナ用。</remarks>
        public AccountWindowViewModel()
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="windowManager">ウィンドウマネージャ(null指定不可)</param>
        /// <param name="accountManager">アカウントマネージャ(null指定不可)</param>
        /// <param name="targetAccount">編集対象のアカウント(新規アカウント作成の場合はnull)</param>
        /// <param name="windowTitle">ウィンドウのタイトル(null指定不可)</param>
        public AccountWindowViewModel(WindowManager windowManager,
            AccountManager accountManager, Account targetAccount, string windowTitle)
        {
            Utilities.RejectNull(windowManager, nameof(windowManager));
            Utilities.RejectNull(accountManager, nameof(accountManager));
            Utilities.RejectNull(windowTitle, nameof(windowTitle));

            _windowManager = windowManager;
            _accountManager = accountManager;

            _targetAccount = targetAccount;

            if (_targetAccount == null)
            {
                _editingAccount = new Account();
            }
            else
            {
                _editingAccount = _targetAccount.Clone();
            }

            _editingAccount.PropertyChanged += EditingAccount_PropertyChanged;

            _canFinish = CanFinish(_targetAccount, _editingAccount);

            FinishCommand = new DelegateCommand(CanExecuteFinishCommand, ExecuteFinishCommand);
            CancelCommand = new DelegateCommand(null, ExecuteCancelCommand);

            WindowTitle = windowTitle;
        }

        private void EditingAccount_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bool oldCanFinish = _canFinish;
            _canFinish = CanFinish(_targetAccount, _editingAccount);

            if (_canFinish != oldCanFinish)
            {
                FinishCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanExecuteFinishCommand(object parameter)
        {
            return _canFinish;
        }

        private void ExecuteFinishCommand(object parameter)
        {
            if (_canFinish)
            {
                DoFinish(_accountManager, _targetAccount, _editingAccount);
                _windowManager.CloseWindow(this);
            }
        }

        /// <summary>
        /// 編集完了が可能かどうかを取得する。
        /// </summary>
        /// <param name="targetAccount">編集対象のアカウント(新規アカウント作成の場合のみnull)</param>
        /// <param name="editingAccount">編集中のアカウント(not null)</param>
        /// <returns>編集完了が可能な場合はtrue、そうでない場合はfalse</returns>
        /// <remarks>サブクラスでオーバライドして判定を行うこと。</remarks>
        protected virtual bool CanFinish(Account targetAccount, Account editingAccount)
        {
            return true;
        }

        /// <summary>
        /// 編集完了時の処理を行う。
        /// </summary>
        /// <param name="accountManager">アカウントマネージャ(not null)</param>
        /// <param name="targetAccount">編集対象のアカウント(新規アカウント作成の場合のみnull)</param>
        /// <param name="editingAccount">編集中のアカウント(not null)</param>
        /// <remarks>サブクラスでオーバライドして必要な処理を行うこと。</remarks>
        protected virtual void DoFinish(AccountManager accountManager, Account targetAccount, Account editingAccount)
        {
        }

        private void ExecuteCancelCommand(object parameter)
        {
            _windowManager.CloseWindow(this);
        }

        /// <summary>
        /// <see cref="ViewModel.OnWindowClosing"/>の実装。
        /// </summary>
        public bool OnWindowClosing()
        {
            return true;
        }

        /// <summary>
        /// <see cref="ViewModel.OnWindowClosed"/>の実装。
        /// </summary>
        public void OnWindowClosed()
        {
            _editingAccount.PropertyChanged -= EditingAccount_PropertyChanged;
        }

    }
}

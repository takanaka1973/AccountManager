using System.Collections.Generic;
using System.ComponentModel;

// TODO: 未保存の変更がある状態でウィンドウを閉じようとした場合に確認メッセージを表示する。

namespace AccountManagerApp
{
    /// <summary>
    /// メインウィンドウのビューモデル。
    /// </summary>
    public class MainWindowViewModel : ObservableObject, ViewModel
    {
        private readonly WindowManager _windowManager;
        private readonly AccountManager _accountManager;

        private Account _selectedAccount = null;

        /// <summary>
        /// 新規アカウントコマンド。
        /// </summary>
        public DelegateCommand NewAccountCommand { get; }

        /// <summary>
        /// アカウント編集コマンド。
        /// </summary>
        public DelegateCommand EditAccountCommand { get; }

        /// <summary>
        /// アカウント編集コマンド(対象指定あり)。
        /// </summary>
        /// <remarks>コマンドのパラメータで編集対象アカウントが渡される。</remarks>
        public DelegateCommand EditTargetAccountCommand { get; }

        /// <summary>
        /// アカウント削除コマンド。
        /// </summary>
        public DelegateCommand DeleteAccountCommand { get; }

        /// <summary>
        /// アカウント保存コマンド。
        /// </summary>
        public DelegateCommand SaveAccountsCommand { get; }

        /// <summary>
        /// アカウントのリスト。
        /// </summary>
        public IReadOnlyList<Account> AccountList
        {
            get
            {
                return _accountManager.AccountList;
            }
        }

        /// <summary>
        /// 保存されていない変更があるかどうか。
        /// </summary>
        /// <value>保存されていない変更がある場合はtrue、無い場合はfalse</value>
        public bool HasUnsavedChanges
        {
            get
            {
                return _accountManager.HasUnsavedChanges;
            }
        }

        /// <summary>
        /// 選択されているアカウント。
        /// </summary>
        public Account SelectedAccount
        {
            get
            {
                return _selectedAccount;
            }
            set
            {
                Account oldSelectedAccount = _selectedAccount;
                _selectedAccount = value;

                if (oldSelectedAccount == null && _selectedAccount != null
                    || oldSelectedAccount != null && _selectedAccount == null)
                {
                    EditAccountCommand.NotifyCanExecuteChanged();
                    DeleteAccountCommand.NotifyCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <remarks>XAMLデザイナ用。</remarks>
        public MainWindowViewModel()
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="windowManager">ウィンドウマネージャ(null指定不可)</param>
        /// <param name="accountManager">アカウントマネージャ(null指定不可)</param>
        public MainWindowViewModel(WindowManager windowManager, AccountManager accountManager)
        {
            Utilities.RejectNull(windowManager, nameof(windowManager));
            Utilities.RejectNull(accountManager, nameof(accountManager));

            _windowManager = windowManager;

            _accountManager = accountManager;
            _accountManager.PropertyChanged += AccountManager_PropertyChanged;

            NewAccountCommand = new DelegateCommand(null, ExecuteNewAccountCommand);
            EditAccountCommand = new DelegateCommand(CanExecuteEditAccountCommand, ExecuteEditAccountCommand);
            EditTargetAccountCommand = new DelegateCommand(null, ExecuteEditTargetAccountCommand);
            DeleteAccountCommand = new DelegateCommand(CanExecuteDeleteAccountCommand, ExecuteDeleteAccountCommand);
            SaveAccountsCommand = new DelegateCommand(CanExecuteSaveAccountsCommand, ExecuteSaveAccountsCommand);
        }

        private void AccountManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AccountManager.HasUnsavedChanges))
            {
                NotifyPropertyChanged(nameof(HasUnsavedChanges));
                SaveAccountsCommand.NotifyCanExecuteChanged();
            }
        }

        private void ExecuteNewAccountCommand(object parameter)
        {
            _windowManager.ShowNewAccountWindow(this, _accountManager);
        }

        private bool CanExecuteEditAccountCommand(object parameter)
        {
            if (_selectedAccount != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ExecuteEditAccountCommand(object parameter)
        {
            if (_selectedAccount != null)
            {
                _windowManager.ShowEditAccountWindow(this, _accountManager, _selectedAccount);
            }
        }

        private void ExecuteEditTargetAccountCommand(object parameter)
        {
            Account targetAccount = parameter as Account;

            if (targetAccount != null)
            {
                _windowManager.ShowEditAccountWindow(this, _accountManager, targetAccount);
            }
        }

        private bool CanExecuteDeleteAccountCommand(object parameter)
        {
            if (_selectedAccount != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ExecuteDeleteAccountCommand(object parameter)
        {
            if (_selectedAccount != null)
            {
                _accountManager.RemoveAccount(_selectedAccount);
            }
        }

        private bool CanExecuteSaveAccountsCommand(object parameter)
        {
            return HasUnsavedChanges;
        }

        private void ExecuteSaveAccountsCommand(object parameter)
        {
            if (!CanExecuteSaveAccountsCommand(parameter))
            {
                return;
            }

            try
            {
                _accountManager.SaveAccounts();
            }
            catch (AccountStorageException)
            {
                _windowManager.ShowError(this, Messages.FailedToSaveAccounts);
            }
        }

        /// <summary>
        /// <see cref="ViewModel.OnWindowClosed"/>の実装。
        /// </summary>
        public void OnWindowClosed()
        {
            _accountManager.PropertyChanged -= AccountManager_PropertyChanged;
        }

    }
}

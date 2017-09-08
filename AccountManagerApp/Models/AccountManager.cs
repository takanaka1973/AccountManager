using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AccountManagerApp
{
    /// <summary>
    /// アカウントマネージャ。
    /// </summary>
    public class AccountManager : ObservableObject
    {
        private readonly ObservableCollection<Account> _accountList = new ObservableCollection<Account>();
        private readonly AccountStorage _accountStorage;

        private bool _hasUnsavedChanges = false;

        /// <summary>
        /// 管理しているアカウントのリスト。
        /// </summary>
        public IReadOnlyList<Account> AccountList
        {
            get
            {
                return _accountList;
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
                return _hasUnsavedChanges;
            }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="accountStorage">アカウントのストレージ(null指定不可)</param>
        public AccountManager(AccountStorage accountStorage)
        {
            Utilities.RejectNull(accountStorage, nameof(accountStorage));

            _accountStorage = accountStorage;
        }

        /// <summary>
        /// アカウントを登録する。
        /// </summary>
        /// <param name="account">登録するアカウント(null指定不可)</param>
        public void RegisterAccount(Account account)
        {
            Utilities.RejectNull(account, nameof(account));

            account.PropertyChanged += Account_PropertyChanged;

            _accountList.Add(account);
            MarkHasUnsavedChanges();
        }

        /// <summary>
        /// アカウントを登録解除する。
        /// </summary>
        /// <param name="account">登録解除するアカウント(null指定不可)</param>
        /// <remarks><paramref name="account"/>に登録されていないアカウントを指定した場合は何もしない。</remarks>
        public void RemoveAccount(Account account)
        {
            Utilities.RejectNull(account, nameof(account));

            if (_accountList.Contains(account))
            {
                account.PropertyChanged -= Account_PropertyChanged;

                _accountList.Remove(account);
                MarkHasUnsavedChanges();
            }
        }

        /// <summary>
        /// 全アカウントを保存する。
        /// </summary>
        /// <exception cref="AccountStorageException">保存に失敗した場合</exception>
        public void SaveAccounts()
        {
            _accountStorage.Save(_accountList);

            ClearHasUnsavedChanges();
        }

        /// <summary>
        /// アカウントを読み込む。
        /// </summary>
        /// <exception cref="AccountStorageException">読み込みに失敗した場合</exception>
        /// <remarks>読み込みに成功した場合、以前登録されていたアカウントは全て登録解除される。</remarks>
        public void LoadAccounts()
        {
            IList<Account> loadedAccountList = _accountStorage.Load();

            foreach (Account oldAccount in _accountList)
            {
                oldAccount.PropertyChanged -= Account_PropertyChanged;
            }

            _accountList.Clear();

            foreach (Account newAccount in loadedAccountList)
            {
                newAccount.PropertyChanged += Account_PropertyChanged;
                _accountList.Add(newAccount);
            }

            ClearHasUnsavedChanges();
        }

        private void Account_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MarkHasUnsavedChanges();
        }

        private void MarkHasUnsavedChanges()
        {
            if (!_hasUnsavedChanges)
            {
                _hasUnsavedChanges = true;
                NotifyPropertyChanged(nameof(HasUnsavedChanges));
            }
        }

        private void ClearHasUnsavedChanges()
        {
            if (_hasUnsavedChanges)
            {
                _hasUnsavedChanges = false;
                NotifyPropertyChanged(nameof(HasUnsavedChanges));
            }
        }

    }
}

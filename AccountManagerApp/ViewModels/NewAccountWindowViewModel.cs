namespace AccountManagerApp
{
    /// <summary>
    /// 新規アカウントウィンドウのビューモデル。
    /// </summary>
    public class NewAccountWindowViewModel : AccountWindowViewModel
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <remarks>XAMLデザイナ用。</remarks>
        public NewAccountWindowViewModel()
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="windowManager">ウィンドウマネージャ(null指定不可)</param>
        /// <param name="accountManager">アカウントマネージャ(null指定不可)</param>
        public NewAccountWindowViewModel(WindowManager windowManager, AccountManager accountManager)
            : base(windowManager, accountManager, null, "New Account")
        {
        }

        /// <summary>
        /// <see cref="AccountWindowViewModel.CanFinish(Account, Account)"/>の実装。
        /// </summary>
        protected override bool CanFinish(Account targetAccount, Account editingAccount)
        {
            if (editingAccount.AccountName == "")
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// <see cref="AccountWindowViewModel.DoFinish(AccountManager, Account, Account)"/>の実装。
        /// </summary>
        protected override void DoFinish(AccountManager accountManager, Account targetAccount, Account editingAccount)
        {
            accountManager.RegisterAccount(editingAccount);
        }

    }
}

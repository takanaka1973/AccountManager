namespace AccountManagerApp
{
    /// <summary>
    /// アカウント編集ウィンドウのビューモデル。
    /// </summary>
    public class EditAccountWindowViewModel : AccountWindowViewModel
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <remarks>XAMLデザイナ用。</remarks>
        public EditAccountWindowViewModel()
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="windowManager">ウィンドウマネージャ(null指定不可)</param>
        /// <param name="accountManager">アカウントマネージャ(null指定不可)</param>
        /// <param name="targetAccount">編集対象のアカウント(null指定不可)</param>
        public EditAccountWindowViewModel(WindowManager windowManager, AccountManager accountManager, Account targetAccount)
            : base(windowManager, accountManager, targetAccount, "Edit Account")
        {
            Utilities.RejectNull(targetAccount, nameof(targetAccount));
        }

        /// <summary>
        /// <see cref="AccountWindowViewModel.DoFinish(AccountManager, Account, Account)"/>の実装。
        /// </summary>
        protected override void DoFinish(AccountManager accountManager, Account targetAccount, Account editingAccount)
        {
            targetAccount.CopyFrom(editingAccount);
        }

    }
}

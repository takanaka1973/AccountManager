namespace AccountManagerApp
{
    /// <summary>
    /// ウィンドウマネージャ。
    /// </summary>
    public interface WindowManager
    {
        /// <summary>
        /// メインウィンドウを開く。
        /// </summary>
        /// <param name="accountManager">アカウントマネージャ(null指定不可)</param>
        void ShowMainWindow(AccountManager accountManager);

        /// <summary>
        /// 新規アカウントウィンドウを開く。
        /// </summary>
        /// <param name="parentViewModel">親ウィンドウのビューモデル(null指定不可)</param>
        /// <param name="accountManager">アカウントマネージャ(null指定不可)</param>
        /// <remarks>このメソッドは新規アカウントウィンドウが閉じられるまで制御を返さない。</remarks>
        void ShowNewAccountWindow(ViewModel parentViewModel, AccountManager accountManager);

        /// <summary>
        /// アカウント編集ウィンドウを開く。
        /// </summary>
        /// <param name="parentViewModel">親ウィンドウのビューモデル(null指定不可)</param>
        /// <param name="accountManager">アカウントマネージャ(null指定不可)</param>
        /// <param name="targetAccount">編集対象のアカウント(null指定不可)</param>
        /// <remarks>このメソッドはアカウント編集ウィンドウが閉じられるまで制御を返さない。</remarks>
        void ShowEditAccountWindow(ViewModel parentViewModel, AccountManager accountManager, Account targetAccount);

        /// <summary>
        /// ウィンドウを閉じる。
        /// </summary>
        /// <param name="viewModel">閉じたいウィンドウのビューモデル(null指定不可)</param>
        /// <remarks><paramref name="viewModel"/>に関連付いたウィンドウが無い場合は何もしない。</remarks>
        void CloseWindow(ViewModel viewModel);

        /// <summary>
        /// エラーメッセージを表示する。
        /// </summary>
        /// <param name="viewModel">エラーメッセージを表示するビューモデル(null指定可)</param>
        /// <param name="message">エラーメッセージ(null指定不可)</param>
        /// <remarks>
        /// <para>このメソッドはエラーメッセージが確認されるまで制御を返さない。</para>
        /// <para><paramref name="viewModel"/>に関連付いたウィンドウが無い場合は何もしない。</para>
        /// <para><paramref name="viewModel"/>にnullを指定した場合は親ウィンドウ無しでメッセージを表示する。</para>
        /// </remarks>
        void ShowError(ViewModel viewModel, string message);

    }
}

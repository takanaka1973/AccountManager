namespace AccountManagerApp
{
    /// <summary>
    /// 表示用メッセージの定義。
    /// </summary>
    public static class Messages
    {
        /// <summary>
        /// アカウント保存失敗。
        /// </summary>
        public const string FailedToSaveAccounts = "Failed to save accounts.";

        /// <summary>
        /// アカウント読み込み失敗。
        /// </summary>
        public const string FailedToLoadAccounts = "Failed to load accounts.";

        /// <summary>
        /// 未保存の変更がある場合の終了確認。
        /// </summary>
        public const string ConfirmToQuitWhenHavingUnsavedChanges = "There are some unsaved changes.\n"
            + "Do you really want to quit without saving your changes?";

        /// <summary>
        /// アカウントの削除確認。
        /// </summary>
        public const string ConfirmToDeleteAccount = "Do you really want to delete this account?";

    }
}

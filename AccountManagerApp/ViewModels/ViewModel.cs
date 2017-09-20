namespace AccountManagerApp
{
    /// <summary>
    /// ビューモデル。
    /// </summary>
    public interface ViewModel
    {
        /// <summary>
        /// ウィンドウが閉じられようとしている際に呼び出される。
        /// </summary>
        /// <returns>ウィンドウを閉じてよい場合はtrue、閉じてはいけない場合はfalse</returns>
        bool OnWindowClosing();

        /// <summary>
        /// ウィンドウが閉じられた際に呼び出される。
        /// </summary>
        void OnWindowClosed();
    }
}

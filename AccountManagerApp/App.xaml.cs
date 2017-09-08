using System.Windows;

namespace AccountManagerApp
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private AccountStorage _accountStorage;
        private AccountManager _accountManager;
        private WindowManager _windowManager;

        /// <summary>
        /// アプリケーション開始時に呼び出される。
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _accountStorage = new FileAccountStorage();
            _accountManager = new AccountManager(_accountStorage);
            _windowManager = new WindowManagerImpl();

            try
            {
                _accountManager.LoadAccounts();
            }
            catch (AccountStorageException)
            {
                _windowManager.ShowError(null, Messages.FailedToLoadAccounts);
            }

            _windowManager.ShowMainWindow(_accountManager);
        }
    }
}

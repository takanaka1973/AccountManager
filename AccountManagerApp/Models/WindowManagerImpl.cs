using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace AccountManagerApp
{
    /// <summary>
    /// <see cref="WindowManager"/>の実装クラス。
    /// </summary>
    public class WindowManagerImpl : WindowManager
    {
        private const string MessageBoxTitle = "Account Manager";

        private readonly IDictionary<ViewModel, Window> _windowMap = new Dictionary<ViewModel, Window>();

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public WindowManagerImpl()
        {
        }

        /// <summary>
        /// <see cref="WindowManager.ShowMainWindow(AccountManager)"/>の実装。
        /// </summary>
        public void ShowMainWindow(AccountManager accountManager)
        {
            Utilities.RejectNull(accountManager, nameof(accountManager));

            var window = new MainWindow();
            var viewModel = new MainWindowViewModel(this, accountManager);

            ShowWindow(window, viewModel);
        }

        /// <summary>
        /// <see cref="WindowManager.ShowNewAccountWindow(ViewModel, AccountManager)"/>の実装。
        /// </summary>
        public void ShowNewAccountWindow(ViewModel parentViewModel, AccountManager accountManager)
        {
            Utilities.RejectNull(parentViewModel, nameof(parentViewModel));
            Utilities.RejectNull(accountManager, nameof(accountManager));

            var window = new AccountWindow();
            var viewModel = new NewAccountWindowViewModel(this, accountManager);

            ShowDialog(window, viewModel, parentViewModel);
        }

        /// <summary>
        /// <see cref="WindowManager.ShowEditAccountWindow(ViewModel, AccountManager, Account)"/>の実装。
        /// </summary>
        public void ShowEditAccountWindow(ViewModel parentViewModel, AccountManager accountManager, Account targetAccount)
        {
            Utilities.RejectNull(parentViewModel, nameof(parentViewModel));
            Utilities.RejectNull(accountManager, nameof(accountManager));
            Utilities.RejectNull(targetAccount, nameof(targetAccount));

            var window = new AccountWindow();
            var viewModel = new EditAccountWindowViewModel(this, accountManager, targetAccount);

            ShowDialog(window, viewModel, parentViewModel);
        }

        private void ShowWindow(Window window, ViewModel viewModel)
        {
            window.Closing += Window_Closing;
            window.Closed += Window_Closed;
            window.DataContext = viewModel;

            _windowMap[viewModel] = window;

            window.Show();
        }

        private void ShowDialog(Window window, ViewModel viewModel, ViewModel parentViewModel)
        {
            window.Closing += Window_Closing;
            window.Closed += Window_Closed;
            window.DataContext = viewModel;

            if (_windowMap.ContainsKey(parentViewModel))
            {
                Window parentWindow = _windowMap[parentViewModel];
                window.Owner = parentWindow;
            }

            _windowMap[viewModel] = window;

            window.ShowDialog();
        }

        /// <summary>
        /// <see cref="WindowManager.CloseWindow(ViewModel)"/>の実装。
        /// </summary>
        public void CloseWindow(ViewModel viewModel)
        {
            Utilities.RejectNull(viewModel, nameof(viewModel));

            if (_windowMap.ContainsKey(viewModel))
            {
                Window window = _windowMap[viewModel];
                window.Close();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Window window = (Window)sender;
            ViewModel viewModel = (ViewModel)window.DataContext;

            if (_windowMap.ContainsKey(viewModel))
            {
                bool allowClose = viewModel.OnWindowClosing();

                if (!allowClose)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            ViewModel viewModel = (ViewModel)window.DataContext;

            window.Closing -= Window_Closing;
            window.Closed -= Window_Closed;

            if (_windowMap.ContainsKey(viewModel))
            {
                viewModel.OnWindowClosed();
                _windowMap.Remove(viewModel);
            }
        }

        /// <summary>
        /// <see cref="WindowManager.ShowError(ViewModel, string)"/>の実装。
        /// </summary>
        public void ShowError(ViewModel viewModel, string message)
        {
            Utilities.RejectNull(message, nameof(message));

            if (viewModel == null)
            {
                MessageBox.Show(message, MessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (_windowMap.ContainsKey(viewModel))
            {
                Window window = _windowMap[viewModel];
                MessageBox.Show(window, message, MessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

﻿namespace AccountManagerApp.Tests
{
    public class WindowManagerMock : WindowManager
    {
        public bool IsShowMainWindowCalled { get; private set; }
        public bool IsShowNewAccountWindowCalled { get; private set; }

        public bool IsShowEditAccountWindowCalled { get; private set; }
        public Account ShowEditAccountWindowParamTargetAccount { get; private set; }

        public bool IsCloseWindowCalled { get; private set; }

        public bool IsShowErrorCalled { get; private set; }
        public ViewModel ShowErrorParamViewModel { get; private set; }
        public string ShowErrorParamMessage { get; private set; }

        public WindowManagerMock()
        {
            Clear();
        }

        public void Clear()
        {
            IsShowMainWindowCalled = false;
            IsShowNewAccountWindowCalled = false;

            IsShowEditAccountWindowCalled = false;
            ShowEditAccountWindowParamTargetAccount = null;

            IsCloseWindowCalled = false;

            IsShowErrorCalled = false;
            ShowErrorParamViewModel = null;
            ShowErrorParamMessage = null;
        }

        public void ShowMainWindow(AccountManager accountManager)
        {
            IsShowMainWindowCalled = true;
        }

        public void ShowNewAccountWindow(ViewModel parentViewModel, AccountManager accountManager)
        {
            IsShowNewAccountWindowCalled = true;
        }

        public void ShowEditAccountWindow(ViewModel parentViewModel, AccountManager accountManager, Account targetAccount)
        {
            IsShowEditAccountWindowCalled = true;
            ShowEditAccountWindowParamTargetAccount = targetAccount;
        }

        public void CloseWindow(ViewModel viewModel)
        {
            IsCloseWindowCalled = true;
        }

        public void ShowError(ViewModel viewModel, string message)
        {
            IsShowErrorCalled = true;
            ShowErrorParamViewModel = viewModel;
            ShowErrorParamMessage = message;
        }

    }
}

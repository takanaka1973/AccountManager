using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Windows.Input;

namespace AccountManagerApp.Tests
{
    [TestClass]
    public class MainWindowViewModelTests
    {
        private AccountStorageMock _accountStorage;
        private AccountManager _accountManager;
        private WindowManagerMock _windowManager;
        private MainWindowViewModel _mainWindowViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _accountStorage = new AccountStorageMock();
            _accountManager = new AccountManager(_accountStorage);
            _windowManager = new WindowManagerMock();
            _mainWindowViewModel = new MainWindowViewModel(_windowManager, _accountManager);
        }

        [TestMethod]
        public void AccountListプロパティはAccountManagerのAccountListを返す()
        {
            Assert.AreSame(_accountManager.AccountList, _mainWindowViewModel.AccountList);
        }

        [TestMethod]
        public void HasUnsavedChangesプロパティはAccountManagerのHasUnsavedChangesを返す()
        {
            Assert.AreEqual(_accountManager.HasUnsavedChanges, _mainWindowViewModel.HasUnsavedChanges);
            Assert.IsFalse(_mainWindowViewModel.HasUnsavedChanges);

            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            _accountManager.RegisterAccount(account1);

            Assert.AreEqual(_accountManager.HasUnsavedChanges, _mainWindowViewModel.HasUnsavedChanges);
            Assert.IsTrue(_mainWindowViewModel.HasUnsavedChanges);
        }

        [TestMethod]
        public void HasUnsavedChangesプロパティがtrueに変わる際に通知が発生する()
        {
            Assert.IsFalse(_mainWindowViewModel.HasUnsavedChanges);

            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            bool eventFired = false;

            _mainWindowViewModel.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual(nameof(MainWindowViewModel.HasUnsavedChanges), e.PropertyName);
            };

            _accountManager.RegisterAccount(account1);
            Assert.IsTrue(eventFired);
            Assert.IsTrue(_mainWindowViewModel.HasUnsavedChanges);

            _accountManager.SaveAccounts();
            Assert.IsFalse(_mainWindowViewModel.HasUnsavedChanges);

            eventFired = false;
            account1.AccountName = "z";
            Assert.IsTrue(eventFired);
            Assert.IsTrue(_mainWindowViewModel.HasUnsavedChanges);
        }

        [TestMethod]
        public void HasUnsavedChangesプロパティがfalseに変わる際に通知が発生する()
        {
            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            _accountManager.RegisterAccount(account1);
            Assert.IsTrue(_mainWindowViewModel.HasUnsavedChanges);

            bool eventFired = false;

            _mainWindowViewModel.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual(nameof(MainWindowViewModel.HasUnsavedChanges), e.PropertyName);
            };

            _accountManager.SaveAccounts();
            Assert.IsTrue(eventFired);
            Assert.IsFalse(_mainWindowViewModel.HasUnsavedChanges);
        }

        [TestMethod]
        public void ウィンドウが閉じられるとAccountManagerの監視を止める()
        {
            bool eventFired = false;

            _mainWindowViewModel.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
            };

            Assert.IsFalse(_accountManager.HasUnsavedChanges);

            _mainWindowViewModel.OnWindowClosed();

            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            _accountManager.RegisterAccount(account1);

            Assert.IsTrue(_accountManager.HasUnsavedChanges);
            Assert.IsFalse(eventFired);
        }

        [TestMethod]
        public void NewAccountCommandを実行するとWindowManagerのShowNewAccountWindowが呼び出される()
        {
            Assert.IsFalse(_windowManager.IsShowNewAccountWindowCalled);

            _mainWindowViewModel.NewAccountCommand.Execute(null);

            Assert.IsTrue(_windowManager.IsShowNewAccountWindowCalled);
        }

        [TestMethod]
        public void EditAccountCommandを実行するとWindowManagerのShowEditAccountWindowが呼び出される()
        {
            ICommand editAccountCommand = _mainWindowViewModel.EditAccountCommand;

            _mainWindowViewModel.SelectedAccount = new Account();

            Assert.IsTrue(editAccountCommand.CanExecute(null));
            Assert.IsFalse(_windowManager.IsShowEditAccountWindowCalled);

            editAccountCommand.Execute(null);

            Assert.IsTrue(_windowManager.IsShowEditAccountWindowCalled);
        }

        [TestMethod]
        public void EditAccountCommandのCanExecuteはSelectedAccountがnullでない場合のみtrueを返す()
        {
            ICommand editAccountCommand = _mainWindowViewModel.EditAccountCommand;

            _mainWindowViewModel.SelectedAccount = new Account();
            Assert.IsTrue(editAccountCommand.CanExecute(null));

            _mainWindowViewModel.SelectedAccount = null;
            Assert.IsFalse(editAccountCommand.CanExecute(null));

            _mainWindowViewModel.SelectedAccount = new Account();
            Assert.IsTrue(editAccountCommand.CanExecute(null));
        }

        [TestMethod]
        public void EditAccountCommandのCanExecuteの戻り値が変わる場合のみCanExecuteChangedイベントが発火される()
        {
            ICommand editAccountCommand = _mainWindowViewModel.EditAccountCommand;
            bool eventFired = false;

            editAccountCommand.CanExecuteChanged += (sender, e) =>
            {
                eventFired = true;
            };

            _mainWindowViewModel.SelectedAccount = new Account();
            Assert.IsTrue(eventFired);

            eventFired = false;
            _mainWindowViewModel.SelectedAccount = new Account();
            Assert.IsFalse(eventFired);

            eventFired = false;
            _mainWindowViewModel.SelectedAccount = null;
            Assert.IsTrue(eventFired);

            eventFired = false;
            _mainWindowViewModel.SelectedAccount = null;
            Assert.IsFalse(eventFired);

            eventFired = false;
            _mainWindowViewModel.SelectedAccount = new Account();
            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void SaveAccountsCommandのCanExecuteはHasUnsavedChangesがtrueの場合のみtrueを返す()
        {
            ICommand saveAccountsCommand = _mainWindowViewModel.SaveAccountsCommand;

            Assert.IsFalse(_mainWindowViewModel.HasUnsavedChanges);
            Assert.IsFalse(saveAccountsCommand.CanExecute(null));

            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            _accountManager.RegisterAccount(account1);

            Assert.IsTrue(_mainWindowViewModel.HasUnsavedChanges);
            Assert.IsTrue(saveAccountsCommand.CanExecute(null));

            _accountManager.SaveAccounts();

            Assert.IsFalse(_mainWindowViewModel.HasUnsavedChanges);
            Assert.IsFalse(saveAccountsCommand.CanExecute(null));
        }

        [TestMethod]
        public void SaveAccountsCommandのCanExecuteの戻り値が変わる場合のみCanExecuteChangedイベントが発火される()
        {
            ICommand saveAccountsCommand = _mainWindowViewModel.SaveAccountsCommand;

            Assert.IsFalse(saveAccountsCommand.CanExecute(null));

            bool eventFired = false;

            saveAccountsCommand.CanExecuteChanged += (sender, e) =>
            {
                eventFired = true;
            };

            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            _accountManager.RegisterAccount(account1);
            Assert.IsTrue(saveAccountsCommand.CanExecute(null));
            Assert.IsTrue(eventFired);

            eventFired = false;
            account1.AccountName = "x";
            Assert.IsTrue(saveAccountsCommand.CanExecute(null));
            Assert.IsFalse(eventFired);

            eventFired = false;
            _accountManager.SaveAccounts();
            Assert.IsFalse(saveAccountsCommand.CanExecute(null));
            Assert.IsTrue(eventFired);

            eventFired = false;
            _accountManager.SaveAccounts();
            Assert.IsFalse(saveAccountsCommand.CanExecute(null));
            Assert.IsFalse(eventFired);

            eventFired = false;
            account1.AccountName = "y";
            Assert.IsTrue(saveAccountsCommand.CanExecute(null));
            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void SaveAccountsCommandを実行すると全アカウントが保存される()
        {
            ICommand saveAccountsCommand = _mainWindowViewModel.SaveAccountsCommand;

            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            Account account2 = new Account()
            {
                AccountName = "v",
                UserId = "w",
                Password = "x",
                Url = "y",
                Remarks = "z",
            };

            _accountManager.RegisterAccount(account1);
            _accountManager.RegisterAccount(account2);

            Assert.IsTrue(saveAccountsCommand.CanExecute(null));

            saveAccountsCommand.Execute(null);

            IList<Account> savedAccounts = _accountStorage.SavedAccounts;

            Assert.AreEqual(2, savedAccounts.Count);
            Assert.IsTrue(savedAccounts[0].Equals(account1));
            Assert.IsTrue(savedAccounts[1].Equals(account2));
        }

        [TestMethod]
        public void SaveAccountsCommandの実行でアカウント保存に失敗した場合はHasUnsavedChangesはtrueのまま()
        {
            ICommand saveAccountsCommand = _mainWindowViewModel.SaveAccountsCommand;

            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            _accountManager.RegisterAccount(account1);

            Assert.IsTrue(_mainWindowViewModel.HasUnsavedChanges);
            Assert.IsTrue(saveAccountsCommand.CanExecute(null));

            _accountStorage.ThrowAccountStorageExceptionOnSave = true;
            saveAccountsCommand.Execute(null);

            Assert.IsTrue(_mainWindowViewModel.HasUnsavedChanges);
            Assert.IsTrue(saveAccountsCommand.CanExecute(null));

            Assert.AreEqual(0, _accountStorage.SavedAccounts.Count);
        }

        [TestMethod]
        public void SaveAccountsCommandの実行でアカウント保存に失敗した場合はエラーメッセージが表示される()
        {
            ICommand saveAccountsCommand = _mainWindowViewModel.SaveAccountsCommand;

            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            _accountManager.RegisterAccount(account1);

            Assert.IsTrue(saveAccountsCommand.CanExecute(null));

            _accountStorage.ThrowAccountStorageExceptionOnSave = true;
            saveAccountsCommand.Execute(null);

            Assert.IsTrue(_windowManager.IsShowErrorCalled);
            Assert.AreEqual(_mainWindowViewModel, _windowManager.ShowErrorParamViewModel);
            Assert.AreEqual(Messages.FailedToSaveAccounts, _windowManager.ShowErrorParamMessage);
        }

        [TestMethod]
        public void EditTargetAccountCommandのCanExecuteは常にtrueを返す()
        {
            ICommand editTargetAccountCommand = _mainWindowViewModel.EditTargetAccountCommand;

            Assert.IsTrue(editTargetAccountCommand.CanExecute(null));

            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            _accountManager.RegisterAccount(account1);

            Assert.IsTrue(editTargetAccountCommand.CanExecute(null));

            _accountManager.SaveAccounts();

            Assert.IsTrue(editTargetAccountCommand.CanExecute(null));
        }

        [TestMethod]
        public void EditTargetAccountCommandを実行するとWindowManagerのShowEditAccountWindowが呼び出される()
        {
            ICommand editTargetAccountCommand = _mainWindowViewModel.EditTargetAccountCommand;

            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            editTargetAccountCommand.Execute(account1);

            Assert.IsTrue(_windowManager.IsShowEditAccountWindowCalled);
            Assert.AreSame(account1, _windowManager.ShowEditAccountWindowParamTargetAccount);
        }

        [TestMethod]
        public void DeleteAccountCommandのCanExecuteはSelectedAccountがnullでない場合のみtrueを返す()
        {
            ICommand deleteAccountCommand = _mainWindowViewModel.DeleteAccountCommand;
            bool eventFired = false;

            deleteAccountCommand.CanExecuteChanged += (sender, e) =>
            {
                eventFired = true;
            };

            _mainWindowViewModel.SelectedAccount = new Account();
            Assert.IsTrue(deleteAccountCommand.CanExecute(null));
            Assert.IsTrue(eventFired);

            eventFired = false;
            _mainWindowViewModel.SelectedAccount = new Account();
            Assert.IsTrue(deleteAccountCommand.CanExecute(null));
            Assert.IsFalse(eventFired);

            eventFired = false;
            _mainWindowViewModel.SelectedAccount = null;
            Assert.IsFalse(deleteAccountCommand.CanExecute(null));
            Assert.IsTrue(eventFired);

            eventFired = false;
            _mainWindowViewModel.SelectedAccount = null;
            Assert.IsFalse(deleteAccountCommand.CanExecute(null));
            Assert.IsFalse(eventFired);

            eventFired = false;
            _mainWindowViewModel.SelectedAccount = new Account();
            Assert.IsTrue(deleteAccountCommand.CanExecute(null));
            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void DeleteAccountCommandを実行すると選択されているアカウントが削除される()
        {
            ICommand deleteAccountCommand = _mainWindowViewModel.DeleteAccountCommand;

            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            Account account2 = new Account()
            {
                AccountName = "v",
                UserId = "w",
                Password = "x",
                Url = "y",
                Remarks = "z",
            };

            _accountManager.RegisterAccount(account1);
            _accountManager.RegisterAccount(account2);

            Assert.AreEqual(2, _accountManager.AccountList.Count);
            Assert.AreSame(account1, _accountManager.AccountList[0]);
            Assert.AreSame(account2, _accountManager.AccountList[1]);

            _mainWindowViewModel.SelectedAccount = account1;
            Assert.IsTrue(deleteAccountCommand.CanExecute(null));

            deleteAccountCommand.Execute(null);

            Assert.AreEqual(1, _accountManager.AccountList.Count);
            Assert.AreSame(account2, _accountManager.AccountList[0]);
        }

    }
}

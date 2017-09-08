using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Input;

namespace AccountManagerApp.Tests
{
    [TestClass]
    public class AccountWindowViewModelTests
    {
        private AccountStorageMock _accountStorage;
        private AccountManager _accountManager;
        private WindowManagerMock _windowManager;
        private AccountWindowViewModel _accountWindowViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            var targetAccount = new Account();
            _accountStorage = new AccountStorageMock();
            _accountManager = new AccountManager(_accountStorage);
            _windowManager = new WindowManagerMock();
            _accountWindowViewModel = new AccountWindowViewModel(_windowManager, _accountManager, targetAccount, "title");
        }

        [TestMethod]
        public void EditingAccountのAccountNameが空文字列でない場合のみFinishCommandのCanExecuteはtrueを返す()
        {
            ICommand finishCommand = _accountWindowViewModel.FinishCommand;
            Account editingAccount = _accountWindowViewModel.EditingAccount;

            editingAccount.AccountName = "a";
            Assert.IsTrue(finishCommand.CanExecute(null));

            editingAccount.AccountName = "";
            Assert.IsFalse(finishCommand.CanExecute(null));

            editingAccount.UserId = "x";
            Assert.IsFalse(finishCommand.CanExecute(null));
        }

        [TestMethod]
        public void FinishCommandのCanExecuteの戻り値が変わる場合のみCanExecuteChangedイベントが発火される()
        {
            ICommand finishCommand = _accountWindowViewModel.FinishCommand;
            Account editingAccount = _accountWindowViewModel.EditingAccount;

            editingAccount.AccountName = "";

            bool eventFired = false;

            finishCommand.CanExecuteChanged += (sender, e) =>
            {
                eventFired = true;
            };

            editingAccount.AccountName = "";
            Assert.IsFalse(eventFired);

            eventFired = false;
            editingAccount.AccountName = "abc";
            Assert.IsTrue(eventFired);

            eventFired = false;
            editingAccount.AccountName = "xyz";
            Assert.IsFalse(eventFired);

            eventFired = false;
            editingAccount.AccountName = "";
            Assert.IsTrue(eventFired);

            eventFired = false;
            editingAccount.UserId = "zzz";
            Assert.IsFalse(eventFired);
        }

        [TestMethod]
        public void CancelCommandのCanExecuteは常にtrueを返す()
        {
            ICommand cancelCommand = _accountWindowViewModel.CancelCommand;
            Account editingAccount = _accountWindowViewModel.EditingAccount;

            editingAccount.AccountName = "";
            Assert.IsTrue(cancelCommand.CanExecute(null));

            editingAccount.AccountName = "xyz";
            Assert.IsTrue(cancelCommand.CanExecute(null));
        }

        [TestMethod]
        public void ウィンドウが閉じられるとEditingAccountに対するイベント監視を止める()
        {
            ICommand finishCommand = _accountWindowViewModel.FinishCommand;
            Account editingAccount = _accountWindowViewModel.EditingAccount;

            editingAccount.AccountName = "";

            bool eventFired = false;

            finishCommand.CanExecuteChanged += (sender, e) =>
            {
                eventFired = true;
            };

            _accountWindowViewModel.OnWindowClosed();

            editingAccount.AccountName = "abc";
            Assert.IsFalse(eventFired);
        }

    }
}

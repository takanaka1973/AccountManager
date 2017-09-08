using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Input;

namespace AccountManagerApp.Tests
{
    [TestClass]
    public class NewAccountWindowViewModelTests
    {
        private AccountStorageMock _accountStorage;
        private AccountManager _accountManager;
        private WindowManagerMock _windowManager;
        private NewAccountWindowViewModel _newAccountWindowViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _accountStorage = new AccountStorageMock();
            _accountManager = new AccountManager(_accountStorage);
            _windowManager = new WindowManagerMock();
            _newAccountWindowViewModel = new NewAccountWindowViewModel(_windowManager, _accountManager);
        }

        [TestMethod]
        public void EditingAccountの初期値は全フィールドが空文字列()
        {
            Account editingAccount = _newAccountWindowViewModel.EditingAccount;

            Assert.AreEqual("", editingAccount.AccountName);
            Assert.AreEqual("", editingAccount.UserId);
            Assert.AreEqual("", editingAccount.Password);
            Assert.AreEqual("", editingAccount.Url);
            Assert.AreEqual("", editingAccount.Remarks);
        }

        [TestMethod]
        public void FinishCommandのCanExecuteの初期値はfalse()
        {
            Assert.IsFalse(_newAccountWindowViewModel.FinishCommand.CanExecute(null));
        }

        [TestMethod]
        public void FinishCommandを実行するとアカウントが登録される()
        {
            Assert.AreEqual(0, _accountManager.AccountList.Count);

            ICommand finishCommand = _newAccountWindowViewModel.FinishCommand;
            Account editingAccount = _newAccountWindowViewModel.EditingAccount;

            editingAccount.AccountName = "a";
            editingAccount.UserId = "b";
            editingAccount.Password = "c";
            editingAccount.Url = "d";
            editingAccount.Remarks = "e";

            Assert.IsTrue(finishCommand.CanExecute(null));

            finishCommand.Execute(null);

            Assert.AreEqual(1, _accountManager.AccountList.Count);
            Assert.AreSame(editingAccount, _accountManager.AccountList[0]);
        }

        [TestMethod]
        public void AccountNameが空文字列の場合はFinishCommandを実行してもアカウントは登録されない()
        {
            Assert.AreEqual(0, _accountManager.AccountList.Count);

            ICommand finishCommand = _newAccountWindowViewModel.FinishCommand;
            Account editingAccount = _newAccountWindowViewModel.EditingAccount;

            editingAccount.AccountName = "";
            editingAccount.UserId = "b";
            editingAccount.Password = "c";
            editingAccount.Url = "d";
            editingAccount.Remarks = "e";

            Assert.IsFalse(finishCommand.CanExecute(null));

            finishCommand.Execute(null);

            Assert.AreEqual(0, _accountManager.AccountList.Count);
        }

    }
}

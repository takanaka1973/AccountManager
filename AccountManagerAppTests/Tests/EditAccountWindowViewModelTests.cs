using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Input;

namespace AccountManagerApp.Tests
{
    [TestClass]
    public class EditAccountWindowViewModelTests
    {
        private Account _targetAccount;
        private AccountStorageMock _accountStorage;
        private AccountManager _accountManager;
        private WindowManagerMock _windowManager;
        private EditAccountWindowViewModel _editAccountWindowViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _targetAccount = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            _accountStorage = new AccountStorageMock();
            _accountManager = new AccountManager(_accountStorage);
            _windowManager = new WindowManagerMock();
            _editAccountWindowViewModel = new EditAccountWindowViewModel(_windowManager, _accountManager, _targetAccount);
        }

        [TestMethod]
        public void EditingAccountの各フィールドの初期値はコンストラクタに渡したtargetAccountと同じ()
        {
            Account editingAccount = _editAccountWindowViewModel.EditingAccount;

            Assert.AreEqual(_targetAccount.AccountName, editingAccount.AccountName);
            Assert.AreEqual(_targetAccount.UserId, editingAccount.UserId);
            Assert.AreEqual(_targetAccount.Password, editingAccount.Password);
            Assert.AreEqual(_targetAccount.Url, editingAccount.Url);
            Assert.AreEqual(_targetAccount.Remarks, editingAccount.Remarks);

            Assert.AreNotSame(_targetAccount, editingAccount);
        }

        [TestMethod]
        public void targetAccountのAccountNameが空文字列でない場合はFinishCommandのCanExecuteの初期値はtrue()
        {
            Assert.AreNotEqual("", _targetAccount.AccountName);

            Assert.IsTrue(_editAccountWindowViewModel.FinishCommand.CanExecute(null));
        }

        [TestMethod]
        public void targetAccountのAccountNameが空文字列の場合はFinishCommandのCanExecuteの初期値はfalse()
        {
            var targetAccount1 = new Account()
            {
                AccountName = "",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            var editAccountWindowViewModel1 = new EditAccountWindowViewModel(_windowManager, _accountManager, targetAccount1);

            Assert.IsFalse(editAccountWindowViewModel1.FinishCommand.CanExecute(null));
        }

        [TestMethod]
        public void FinishCommandを実行するとtargetAccountが更新される()
        {
            ICommand finishCommand = _editAccountWindowViewModel.FinishCommand;
            Account editingAccount = _editAccountWindowViewModel.EditingAccount;

            editingAccount.AccountName = "v";
            editingAccount.UserId = "w";
            editingAccount.Password = "x";
            editingAccount.Url = "y";
            editingAccount.Remarks = "z";

            Assert.IsTrue(finishCommand.CanExecute(null));

            Assert.AreNotEqual(editingAccount.AccountName, _targetAccount.AccountName);
            Assert.AreNotEqual(editingAccount.UserId, _targetAccount.UserId);
            Assert.AreNotEqual(editingAccount.Password, _targetAccount.Password);
            Assert.AreNotEqual(editingAccount.Url, _targetAccount.Url);
            Assert.AreNotEqual(editingAccount.Remarks, _targetAccount.Remarks);

            finishCommand.Execute(null);

            Assert.AreEqual(editingAccount.AccountName, _targetAccount.AccountName);
            Assert.AreEqual(editingAccount.UserId, _targetAccount.UserId);
            Assert.AreEqual(editingAccount.Password, _targetAccount.Password);
            Assert.AreEqual(editingAccount.Url, _targetAccount.Url);
            Assert.AreEqual(editingAccount.Remarks, _targetAccount.Remarks);

            Assert.AreNotSame(editingAccount, _targetAccount);
        }

        [TestMethod]
        public void AccountNameが空文字列の場合はFinishCommandを実行してもtargetAccountは更新されない()
        {
            ICommand finishCommand = _editAccountWindowViewModel.FinishCommand;
            Account editingAccount = _editAccountWindowViewModel.EditingAccount;

            editingAccount.AccountName = "";
            editingAccount.UserId = "w";
            editingAccount.Password = "x";
            editingAccount.Url = "y";
            editingAccount.Remarks = "z";

            Assert.IsFalse(finishCommand.CanExecute(null));

            Assert.AreNotEqual(editingAccount.AccountName, _targetAccount.AccountName);
            Assert.AreNotEqual(editingAccount.UserId, _targetAccount.UserId);
            Assert.AreNotEqual(editingAccount.Password, _targetAccount.Password);
            Assert.AreNotEqual(editingAccount.Url, _targetAccount.Url);
            Assert.AreNotEqual(editingAccount.Remarks, _targetAccount.Remarks);

            finishCommand.Execute(null);

            Assert.AreNotEqual(editingAccount.AccountName, _targetAccount.AccountName);
            Assert.AreNotEqual(editingAccount.UserId, _targetAccount.UserId);
            Assert.AreNotEqual(editingAccount.Password, _targetAccount.Password);
            Assert.AreNotEqual(editingAccount.Url, _targetAccount.Url);
            Assert.AreNotEqual(editingAccount.Remarks, _targetAccount.Remarks);
        }

    }
}

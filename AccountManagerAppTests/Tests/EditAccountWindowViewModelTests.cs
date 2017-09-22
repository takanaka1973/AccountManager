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
        public void targetAccountのAccountNameが空文字列でない場合のFinishCommandのCanExecuteの初期値はfalse()
        {
            Assert.AreNotEqual("", _targetAccount.AccountName);

            Assert.IsFalse(_editAccountWindowViewModel.FinishCommand.CanExecute(null));
        }

        [TestMethod]
        public void targetAccountのAccountNameが空文字列の場合のFinishCommandのCanExecuteの初期値はfalse()
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
        public void 編集対象アカウントと異なる点がある場合はFinishCommandのCanExecuteはtrueを返す()
        {
            ICommand finishCommand = _editAccountWindowViewModel.FinishCommand;
            Account editingAccount = _editAccountWindowViewModel.EditingAccount;

            Assert.IsFalse(finishCommand.CanExecute(null));

            editingAccount.AccountName = "v";
            Assert.IsTrue(finishCommand.CanExecute(null));

            editingAccount.AccountName = _targetAccount.AccountName;
            editingAccount.UserId = "w";
            Assert.IsTrue(finishCommand.CanExecute(null));

            editingAccount.UserId = _targetAccount.UserId;
            editingAccount.Password = "x";
            Assert.IsTrue(finishCommand.CanExecute(null));

            editingAccount.Password = _targetAccount.Password;
            editingAccount.Url = "y";
            Assert.IsTrue(finishCommand.CanExecute(null));

            editingAccount.Url = _targetAccount.Url;
            editingAccount.Remarks = "z";
            Assert.IsTrue(finishCommand.CanExecute(null));

            editingAccount.Remarks = _targetAccount.Remarks;
            Assert.IsFalse(finishCommand.CanExecute(null));
        }

        [TestMethod]
        public void 編集対象アカウントと異なる点があってもAccountNameが空文字列の場合はFinishCommandのCanExecuteはfalseを返す()
        {
            ICommand finishCommand = _editAccountWindowViewModel.FinishCommand;
            Account editingAccount = _editAccountWindowViewModel.EditingAccount;

            editingAccount.AccountName = "v";
            editingAccount.UserId = "w";
            editingAccount.Password = "x";
            editingAccount.Url = "y";
            editingAccount.Remarks = "z";
            Assert.IsTrue(finishCommand.CanExecute(null));

            editingAccount.AccountName = "";
            Assert.IsFalse(finishCommand.CanExecute(null));
        }

        [TestMethod]
        public void 編集対象アカウントと異なる点が無い場合はFinishCommandのCanExecuteはfalseを返す()
        {
            ICommand finishCommand = _editAccountWindowViewModel.FinishCommand;
            Account editingAccount = _editAccountWindowViewModel.EditingAccount;

            editingAccount.AccountName = "v";
            editingAccount.UserId = "w";
            editingAccount.Password = "x";
            editingAccount.Url = "y";
            editingAccount.Remarks = "z";

            Assert.IsTrue(finishCommand.CanExecute(null));

            editingAccount.AccountName = _targetAccount.AccountName;
            editingAccount.UserId = _targetAccount.UserId;
            editingAccount.Password = _targetAccount.Password;
            editingAccount.Url = _targetAccount.Url;
            editingAccount.Remarks = _targetAccount.Remarks;

            Assert.IsFalse(finishCommand.CanExecute(null));
        }

        [TestMethod]
        public void FinishCommandのCanExecuteの戻り値が変わる場合のみCanExecuteChangedイベントが発火される()
        {
            ICommand finishCommand = _editAccountWindowViewModel.FinishCommand;
            Account editingAccount = _editAccountWindowViewModel.EditingAccount;

            Assert.IsFalse(finishCommand.CanExecute(null));

            bool eventFired = false;

            finishCommand.CanExecuteChanged += (sender, e) =>
            {
                eventFired = true;
            };

            editingAccount.AccountName = "v";
            Assert.IsTrue(finishCommand.CanExecute(null));
            Assert.IsTrue(eventFired);

            eventFired = false;
            editingAccount.UserId = "w";
            Assert.IsTrue(finishCommand.CanExecute(null));
            Assert.IsFalse(eventFired);

            eventFired = false;
            editingAccount.AccountName = _targetAccount.AccountName;
            editingAccount.UserId = _targetAccount.UserId;
            Assert.IsFalse(finishCommand.CanExecute(null));
            Assert.IsTrue(eventFired);

            eventFired = false;
            editingAccount.AccountName = "";
            Assert.IsFalse(finishCommand.CanExecute(null));
            Assert.IsFalse(eventFired);
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

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AccountManagerApp.Tests
{
    [TestClass]
    public class AccountManagerTests
    {
        private AccountStorageMock _accountStorage;
        private AccountManager _accountManager;

        [TestInitialize]
        public void TestInitialize()
        {
            _accountStorage = new AccountStorageMock();
            _accountManager = new AccountManager(_accountStorage);
        }

        [TestMethod]
        public void AccountListの初期値は空のリスト()
        {
            IReadOnlyList<Account> accountList = _accountManager.AccountList;
            Assert.AreEqual(0, accountList.Count);
        }

        [TestMethod]
        public void HasUnsavedChangesの初期値はfalse()
        {
            Assert.IsFalse(_accountManager.HasUnsavedChanges);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterAccountはnullを受け付けない()
        {
            _accountManager.RegisterAccount(null);
        }

        [TestMethod]
        public void RegisterAccountでアカウントを登録できる()
        {
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

            Assert.AreSame(account1, _accountManager.AccountList[0]);
            Assert.AreSame(account2, _accountManager.AccountList[1]);
        }

        [TestMethod]
        public void アカウントを登録するとHasUnsavedChangesがtrueに変わる()
        {
            Assert.IsFalse(_accountManager.HasUnsavedChanges);

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
        }

        [TestMethod]
        public void 登録されているアカウントを変更するとHasUnsavedChangesがtrueになる()
        {
            Assert.IsFalse(_accountManager.HasUnsavedChanges);

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

            _accountManager.SaveAccounts();
            Assert.IsFalse(_accountManager.HasUnsavedChanges);

            account1.AccountName = "a";
            Assert.IsFalse(_accountManager.HasUnsavedChanges);

            account1.AccountName = "z";
            Assert.IsTrue(_accountManager.HasUnsavedChanges);

            _accountManager.SaveAccounts();
            Assert.IsFalse(_accountManager.HasUnsavedChanges);

            account1.AccountName = "z";
            Assert.IsFalse(_accountManager.HasUnsavedChanges);

            account1.Remarks = "z";
            Assert.IsTrue(_accountManager.HasUnsavedChanges);
        }

        [TestMethod]
        public void HasUnsavedChangesがtrueに変わるとPropertyChangedイベントが発火される()
        {
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

            bool eventFired = false;

            _accountManager.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual(nameof(AccountManager.HasUnsavedChanges), e.PropertyName);
            };

            _accountManager.RegisterAccount(account1);
            Assert.IsTrue(eventFired);

            eventFired = false;
            _accountManager.RegisterAccount(account2);
            Assert.IsFalse(eventFired);
        }

        [TestMethod]
        public void HasUnsavedChangesがfalseに変わるとPropertyChangedイベントが発火される()
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
            Assert.IsTrue(_accountManager.HasUnsavedChanges);

            bool eventFired = false;

            _accountManager.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual(nameof(AccountManager.HasUnsavedChanges), e.PropertyName);
            };

            _accountManager.SaveAccounts();
            Assert.IsTrue(eventFired);
            Assert.IsFalse(_accountManager.HasUnsavedChanges);

            eventFired = false;
            _accountManager.SaveAccounts();
            Assert.IsFalse(eventFired);
            Assert.IsFalse(_accountManager.HasUnsavedChanges);
        }

        [TestMethod]
        public void SaveAccountsを呼び出すと全アカウントが保存される()
        {
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

            _accountManager.SaveAccounts();

            IList<Account> savedAccounts = _accountStorage.SavedAccounts;

            Assert.AreEqual(2, savedAccounts.Count);
            Assert.IsTrue(savedAccounts[0].Equals(account1));
            Assert.IsTrue(savedAccounts[1].Equals(account2));
        }

        [TestMethod]
        public void SaveAccountsを呼び出すとHasUnsavedChangesがfalseになる()
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
            Assert.IsTrue(_accountManager.HasUnsavedChanges);

            _accountManager.SaveAccounts();
            Assert.IsFalse(_accountManager.HasUnsavedChanges);
        }

        [TestMethod]
        public void RemoveAccountでアカウントを登録解除できる()
        {
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

            _accountManager.RemoveAccount(account1);

            Assert.AreEqual(1, _accountManager.AccountList.Count);
            Assert.AreSame(account2, _accountManager.AccountList[0]);

            _accountManager.RemoveAccount(account1);

            Assert.AreEqual(1, _accountManager.AccountList.Count);
            Assert.AreSame(account2, _accountManager.AccountList[0]);

            _accountManager.RemoveAccount(account2);

            Assert.AreEqual(0, _accountManager.AccountList.Count);
        }

        [TestMethod]
        public void アカウントを登録解除するとHasUnsavedChangesがtrueになる()
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
            _accountManager.SaveAccounts();
            Assert.IsFalse(_accountManager.HasUnsavedChanges);

            bool eventFired = false;

            _accountManager.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual(nameof(AccountManager.HasUnsavedChanges), e.PropertyName);
            };

            _accountManager.RemoveAccount(account1);

            Assert.IsTrue(_accountManager.HasUnsavedChanges);
            Assert.IsTrue(eventFired);

            _accountManager.SaveAccounts();
            Assert.IsFalse(_accountManager.HasUnsavedChanges);

            eventFired = false;
            _accountManager.RemoveAccount(account1);

            Assert.IsFalse(_accountManager.HasUnsavedChanges);
            Assert.IsFalse(eventFired);
        }

        [TestMethod]
        public void LoadAccounts_初期状態から_AccountStorageが返した全アカウントがリストに設定される_空リストが返された場合()
        {
            _accountManager.LoadAccounts();
            Assert.AreEqual(0, _accountManager.AccountList.Count);
        }

        [TestMethod]
        public void LoadAccounts_初期状態から_AccountStorageが返した全アカウントがリストに設定される_空でないリストが返された場合()
        {
            Account account1 = new Account();

            Account account2 = new Account()
            {
                AccountName = "x<y>z",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "あ",
            };

            _accountStorage.AccountsToBeLoaded.Add(account1);
            _accountStorage.AccountsToBeLoaded.Add(account2);

            _accountManager.LoadAccounts();

            Assert.AreEqual(2, _accountManager.AccountList.Count);
            Assert.IsTrue(_accountManager.AccountList[0].Equals(account1));
            Assert.IsTrue(_accountManager.AccountList[1].Equals(account2));
        }

        [TestMethod]
        public void LoadAccounts_アカウントが登録済みの状態から_AccountStorageが返した全アカウントがリストに設定される_空リストが返された場合()
        {
            Account account1 = new Account();

            Account account2 = new Account()
            {
                AccountName = "x<y>z",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "あ",
            };

            _accountManager.RegisterAccount(account1);
            _accountManager.RegisterAccount(account2);

            _accountManager.LoadAccounts();

            Assert.AreEqual(0, _accountManager.AccountList.Count);
        }

        [TestMethod]
        public void LoadAccounts_アカウントが登録済みの状態から_AccountStorageが返した全アカウントがリストに設定される_空でないリストが返された場合()
        {
            Account account1 = new Account()
            {
                AccountName = "あ",
                UserId = "い",
                Password = "う",
                Url = "え",
                Remarks = "お",
            };

            Account account2 = new Account()
            {
                AccountName = "か",
                UserId = "き",
                Password = "く",
                Url = "け",
                Remarks = "こ",
            };

            _accountManager.RegisterAccount(account1);
            _accountManager.RegisterAccount(account2);

            Account account3 = new Account();

            Account account4 = new Account()
            {
                AccountName = "x<y>z",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "あ",
            };

            _accountStorage.AccountsToBeLoaded.Add(account3);
            _accountStorage.AccountsToBeLoaded.Add(account4);

            _accountManager.LoadAccounts();

            Assert.AreEqual(2, _accountManager.AccountList.Count);
            Assert.IsTrue(_accountManager.AccountList[0].Equals(account3));
            Assert.IsTrue(_accountManager.AccountList[1].Equals(account4));
        }

        [TestMethod]
        public void LoadAccounts_ロードによりリストから排除された古いアカウントのプロパティ監視は解除される()
        {
            Account account1 = new Account();

            Account account2 = new Account()
            {
                AccountName = "x<y>z",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "あ",
            };

            _accountManager.RegisterAccount(account1);
            _accountManager.RegisterAccount(account2);
            _accountManager.SaveAccounts();

            _accountManager.LoadAccounts();

            Assert.AreEqual(0, _accountManager.AccountList.Count);
            Assert.IsFalse(_accountManager.HasUnsavedChanges);

            account1.AccountName = "new1";
            account2.AccountName = "new2";

            Assert.IsFalse(_accountManager.HasUnsavedChanges);
        }

        [TestMethod]
        public void LoadAccounts_ロードによりリストに追加された新しいアカウントのプロパティ監視が行われる()
        {
            Account account1 = new Account();

            Account account2 = new Account()
            {
                AccountName = "x<y>z",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "あ",
            };

            _accountStorage.AccountsToBeLoaded.Add(account1);
            _accountStorage.AccountsToBeLoaded.Add(account2);

            _accountManager.LoadAccounts();

            Assert.AreEqual(2, _accountManager.AccountList.Count);
            Assert.IsFalse(_accountManager.HasUnsavedChanges);

            account1.AccountName = "new1";

            Assert.IsTrue(_accountManager.HasUnsavedChanges);
        }

        [TestMethod]
        public void LoadAccounts_HasUnsavedChangesがfalseになる_元がfalseの場合()
        {
            Account account1 = new Account()
            {
                AccountName = "あ",
                UserId = "い",
                Password = "う",
                Url = "え",
                Remarks = "お",
            };

            Account account2 = new Account()
            {
                AccountName = "か",
                UserId = "き",
                Password = "く",
                Url = "け",
                Remarks = "こ",
            };

            _accountManager.RegisterAccount(account1);
            _accountManager.RegisterAccount(account2);

            _accountManager.SaveAccounts();
            Assert.IsFalse(_accountManager.HasUnsavedChanges);

            Account account3 = new Account();

            Account account4 = new Account()
            {
                AccountName = "x<y>z",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "あ",
            };

            _accountStorage.AccountsToBeLoaded.Add(account3);
            _accountStorage.AccountsToBeLoaded.Add(account4);

            bool eventFired = false;

            _accountManager.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
            };

            _accountManager.LoadAccounts();

            Assert.AreEqual(2, _accountManager.AccountList.Count);
            Assert.IsFalse(_accountManager.HasUnsavedChanges);
            Assert.IsFalse(eventFired);
        }

        [TestMethod]
        public void LoadAccounts_HasUnsavedChangesがfalseになる_元がtrueの場合()
        {
            Account account1 = new Account()
            {
                AccountName = "あ",
                UserId = "い",
                Password = "う",
                Url = "え",
                Remarks = "お",
            };

            Account account2 = new Account()
            {
                AccountName = "か",
                UserId = "き",
                Password = "く",
                Url = "け",
                Remarks = "こ",
            };

            _accountManager.RegisterAccount(account1);
            _accountManager.RegisterAccount(account2);

            Assert.IsTrue(_accountManager.HasUnsavedChanges);

            Account account3 = new Account();

            Account account4 = new Account()
            {
                AccountName = "x<y>z",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "あ",
            };

            _accountStorage.AccountsToBeLoaded.Add(account3);
            _accountStorage.AccountsToBeLoaded.Add(account4);

            bool eventFired = false;

            _accountManager.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual(nameof(AccountManager.HasUnsavedChanges), e.PropertyName);
            };

            _accountManager.LoadAccounts();

            Assert.AreEqual(2, _accountManager.AccountList.Count);
            Assert.IsFalse(_accountManager.HasUnsavedChanges);
            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        [ExpectedException(typeof(AccountStorageException))]
        public void LoadAccounts_AccountStorageがAccountStorageExceptionを投げた場合にAccountStorageExceptionを投げる()
        {
            _accountStorage.ThrowAccountStorageExceptionOnLoad = true;
            _accountManager.LoadAccounts();
        }

        [TestMethod]
        public void LoadAccounts_AccountStorageがAccountStorageExceptionを投げた場合にリストの内容は変わらない()
        {
            Account account1 = new Account()
            {
                AccountName = "あ",
                UserId = "い",
                Password = "う",
                Url = "え",
                Remarks = "お",
            };

            Account account2 = new Account()
            {
                AccountName = "か",
                UserId = "き",
                Password = "く",
                Url = "け",
                Remarks = "こ",
            };

            _accountManager.RegisterAccount(account1);
            _accountManager.RegisterAccount(account2);
            Assert.AreEqual(2, _accountManager.AccountList.Count);

            try
            {
                _accountStorage.ThrowAccountStorageExceptionOnLoad = true;
                _accountManager.LoadAccounts();

                Assert.Fail();
            }
            catch (AccountStorageException)
            {
                // OK
            }

            Assert.AreEqual(2, _accountManager.AccountList.Count);
            Assert.IsTrue(_accountManager.AccountList[0].Equals(account1));
            Assert.IsTrue(_accountManager.AccountList[1].Equals(account2));
        }

        [TestMethod]
        public void LoadAccounts_AccountStorageがAccountStorageExceptionを投げた場合にHasUnsavedChangesは変わらない()
        {
            Account account1 = new Account()
            {
                AccountName = "あ",
                UserId = "い",
                Password = "う",
                Url = "え",
                Remarks = "お",
            };

            Account account2 = new Account()
            {
                AccountName = "か",
                UserId = "き",
                Password = "く",
                Url = "け",
                Remarks = "こ",
            };

            _accountManager.RegisterAccount(account1);
            _accountManager.RegisterAccount(account2);
            Assert.IsTrue(_accountManager.HasUnsavedChanges);

            try
            {
                _accountStorage.ThrowAccountStorageExceptionOnLoad = true;
                _accountManager.LoadAccounts();

                Assert.Fail();
            }
            catch (AccountStorageException)
            {
                // OK
            }

            Assert.AreEqual(2, _accountManager.AccountList.Count);
            Assert.IsTrue(_accountManager.HasUnsavedChanges);
        }

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace AccountManagerApp.Tests
{
    [TestClass]
    public class FileAccountStorageTests
    {
        private const string TestDataFolderPath = @"C:\temp\AccountManagerTest";
        private static readonly string DataFilePath = Path.Combine(TestDataFolderPath, "AccountList.xml");
        private static readonly string BackupFolderPath = Path.Combine(TestDataFolderPath, "backup");

        private FileAccountStorage _fileAccountStorage;

        private static void DeleteTestDataFolder()
        {
            if (Directory.Exists(TestDataFolderPath))
            {
                Directory.Delete(TestDataFolderPath, true);
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            DeleteTestDataFolder();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            DeleteTestDataFolder();
            _fileAccountStorage = new FileAccountStorage(TestDataFolderPath);
        }

        [TestMethod]
        public void Saveのテスト_空のアカウントリスト()
        {
            Assert.IsFalse(File.Exists(DataFilePath));

            List<Account> accountList = new List<Account>();
            _fileAccountStorage.Save(accountList);

            Assert.IsTrue(File.Exists(DataFilePath));

            XmlDocument doc = new XmlDocument();
            doc.Load(DataFilePath);

            XmlElement rootElement = doc.DocumentElement;

            Assert.AreEqual("AccountList", rootElement.Name);
            Assert.AreEqual("1.0", rootElement.Attributes["formatVersion"].Value);
            Assert.AreEqual("false", rootElement.Attributes["encrypted"].Value);

            Assert.AreEqual(0, rootElement.ChildNodes.Count);
        }

        [TestMethod]
        public void Saveのテスト_空でないアカウントリスト()
        {
            Assert.IsFalse(File.Exists(DataFilePath));

            Account account1 = new Account();

            Account account2 = new Account()
            {
                AccountName = "x<y>z",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "あ",
            };

            List<Account> accountList = new List<Account>();
            accountList.Add(account1);
            accountList.Add(account2);

            _fileAccountStorage.Save(accountList);

            Assert.IsTrue(File.Exists(DataFilePath));

            XmlDocument doc = new XmlDocument();
            doc.Load(DataFilePath);

            XmlElement rootElement = doc.DocumentElement;

            Assert.AreEqual("AccountList", rootElement.Name);
            Assert.AreEqual("1.0", rootElement.Attributes["formatVersion"].Value);
            Assert.AreEqual("false", rootElement.Attributes["encrypted"].Value);

            Assert.AreEqual(2, rootElement.ChildNodes.Count);

            XmlElement accountElement1 = (XmlElement)rootElement.ChildNodes[0];
            Assert.AreEqual("Account", accountElement1.Name);

            Assert.AreEqual("AccountName", accountElement1.ChildNodes[0].Name);
            Assert.AreEqual("", accountElement1.ChildNodes[0].InnerText);

            Assert.AreEqual("UserId", accountElement1.ChildNodes[1].Name);
            Assert.AreEqual("", accountElement1.ChildNodes[1].InnerText);

            Assert.AreEqual("Password", accountElement1.ChildNodes[2].Name);
            Assert.AreEqual("", accountElement1.ChildNodes[2].InnerText);

            Assert.AreEqual("Url", accountElement1.ChildNodes[3].Name);
            Assert.AreEqual("", accountElement1.ChildNodes[3].InnerText);

            Assert.AreEqual("Remarks", accountElement1.ChildNodes[4].Name);
            Assert.AreEqual("", accountElement1.ChildNodes[4].InnerText);

            XmlElement accountElement2 = (XmlElement)rootElement.ChildNodes[1];
            Assert.AreEqual("Account", accountElement2.Name);

            Assert.AreEqual("AccountName", accountElement2.ChildNodes[0].Name);
            Assert.AreEqual("x<y>z", accountElement2.ChildNodes[0].InnerText);

            Assert.AreEqual("UserId", accountElement2.ChildNodes[1].Name);
            Assert.AreEqual("b", accountElement2.ChildNodes[1].InnerText);

            Assert.AreEqual("Password", accountElement2.ChildNodes[2].Name);
            Assert.AreEqual("c", accountElement2.ChildNodes[2].InnerText);

            Assert.AreEqual("Url", accountElement2.ChildNodes[3].Name);
            Assert.AreEqual("d", accountElement2.ChildNodes[3].InnerText);

            Assert.AreEqual("Remarks", accountElement2.ChildNodes[4].Name);
            Assert.AreEqual("あ", accountElement2.ChildNodes[4].InnerText);
        }

        [TestMethod]
        [ExpectedException(typeof(AccountStorageException))]
        public void Saveのテスト_IOException発生()
        {
            Assert.IsFalse(File.Exists(DataFilePath));

            Directory.CreateDirectory(TestDataFolderPath);

            using (FileStream fs = new FileStream(DataFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                List<Account> accountList = new List<Account>();
                _fileAccountStorage.Save(accountList);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AccountStorageException))]
        public void Saveのテスト_UnauthorizedAccessException発生()
        {
            Assert.IsFalse(File.Exists(DataFilePath));

            Directory.CreateDirectory(DataFilePath);    // データファイルと同名のフォルダ

            List<Account> accountList = new List<Account>();
            _fileAccountStorage.Save(accountList);
        }

        [TestMethod]
        public void Saveのテスト_バックアップファイルの作成()
        {
            Assert.IsFalse(Directory.Exists(BackupFolderPath));

            DateTime currentDateTime = new DateTime(2017, 1, 2, 3, 4, 5);
            _fileAccountStorage.GetCurrentDateTime = () => currentDateTime;

            string backupFilePath = Path.Combine(BackupFolderPath, "AccountList_20170102030405.xml");

            Account account1 = new Account();

            Account account2 = new Account()
            {
                AccountName = "x<y>z",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "あ",
            };

            List<Account> accountList = new List<Account>();
            accountList.Add(account1);
            accountList.Add(account2);

            _fileAccountStorage.Save(accountList);

            Assert.IsTrue(Directory.Exists(BackupFolderPath));
            Assert.IsFalse(File.Exists(backupFilePath));

            byte[] oldDataFileContent = ReadFile(DataFilePath);

            account2.AccountName = "modified";
            _fileAccountStorage.Save(accountList);

            Assert.IsTrue(File.Exists(backupFilePath));

            byte[] backupFileContent = ReadFile(backupFilePath);
            Assert.IsTrue(backupFileContent.SequenceEqual(oldDataFileContent));
        }

        private byte[] ReadFile(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] fileContent = new byte[fs.Length];
                fs.Read(fileContent, 0, fileContent.Length);

                return fileContent;
            }
        }

        [TestMethod]
        public void Saveのテスト_同名ファイルが存在するためにバックアップファイルの作成に失敗()
        {
            DateTime currentDateTime = new DateTime(2017, 1, 2, 3, 4, 5);
            _fileAccountStorage.GetCurrentDateTime = () => currentDateTime;

            string backupFilePath = Path.Combine(BackupFolderPath, "AccountList_20170102030405.xml");

            Account account1 = new Account()
            {
                AccountName = "x<y>z",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "あ",
            };

            List<Account> accountList = new List<Account>();
            accountList.Add(account1);

            try
            {
                _fileAccountStorage.Save(accountList);
            }
            catch (AccountStorageException)
            {
                Assert.Fail();
            }

            Assert.IsFalse(File.Exists(backupFilePath));

            try
            {
                account1.AccountName = "modified1";
                _fileAccountStorage.Save(accountList);
            }
            catch (AccountStorageException)
            {
                Assert.Fail();
            }

            Assert.IsTrue(File.Exists(backupFilePath));

            try
            {
                account1.AccountName = "modified2";
                _fileAccountStorage.Save(accountList);

                Assert.Fail();
            }
            catch (AccountStorageException)
            {
                // OK
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AccountStorageException))]
        public void Saveのテスト_同名ファイルが存在するためにバックアップファイル用フォルダの作成に失敗()
        {
            DateTime currentDateTime = new DateTime(2017, 1, 2, 3, 4, 5);
            _fileAccountStorage.GetCurrentDateTime = () => currentDateTime;

            Account account1 = new Account()
            {
                AccountName = "x<y>z",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "あ",
            };

            List<Account> accountList = new List<Account>();
            accountList.Add(account1);

            Directory.CreateDirectory(TestDataFolderPath);

            using (FileStream fs = File.Create(BackupFolderPath))
            {
            }

            _fileAccountStorage.Save(accountList);
        }

        [TestMethod]
        public void Loadはデータフォルダが存在しない場合に空のアカウントリストを返す()
        {
            Assert.IsFalse((Directory.Exists(TestDataFolderPath)));

            IList<Account> accountList = _fileAccountStorage.Load();
            Assert.AreEqual(0, accountList.Count);
        }

        [TestMethod]
        public void Loadはデータフォルダは存在するがデータファイルが存在しない場合に空のアカウントリストを返す()
        {
            Assert.IsFalse((Directory.Exists(TestDataFolderPath)));
            Assert.IsFalse(File.Exists(DataFilePath));

            Directory.CreateDirectory(TestDataFolderPath);

            IList<Account> accountList = _fileAccountStorage.Load();
            Assert.AreEqual(0, accountList.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(AccountStorageException))]
        public void Loadはデータファイルが空ファイルの場合にAccountStorageExceptionを投げる()
        {
            Directory.CreateDirectory(TestDataFolderPath);

            using (FileStream fs = File.Create(DataFilePath))
            {
            }

            IList<Account> accountList = _fileAccountStorage.Load();
        }

        [TestMethod]
        [ExpectedException(typeof(AccountStorageException))]
        public void Loadはデータファイルが不正なXMLの場合にAccountStorageExceptionを投げる()
        {
            Directory.CreateDirectory(TestDataFolderPath);

            using (StreamWriter sw = new StreamWriter(File.Create(DataFilePath)))
            {
                sw.Write("dummy");
            }

            IList<Account> accountList = _fileAccountStorage.Load();
        }

        [TestMethod]
        [ExpectedException(typeof(AccountStorageException))]
        public void Loadはデータファイルが不正なAccountListXmlの場合にAccountStorageExceptionを投げる()
        {
            Directory.CreateDirectory(TestDataFolderPath);

            XmlDocument accountListXmlDocument = new XmlDocument();

            XmlElement accountListElement = accountListXmlDocument.CreateElement("AccountList");
            accountListElement.SetAttribute("formatVersion", "invalid-version");
            accountListElement.SetAttribute("encrypted", "false");

            accountListXmlDocument.AppendChild(accountListElement);
            accountListXmlDocument.Save(DataFilePath);

            IList<Account> accountList = _fileAccountStorage.Load();
        }

        [TestMethod]
        public void Loadはデータファイルが空のAccountListXmlの場合に空のアカウントリストを返す()
        {
            Directory.CreateDirectory(TestDataFolderPath);

            XmlDocument accountListXmlDocument = new XmlDocument();

            XmlElement accountListElement = accountListXmlDocument.CreateElement("AccountList");
            accountListElement.SetAttribute("formatVersion", "1.0");
            accountListElement.SetAttribute("encrypted", "false");

            accountListXmlDocument.AppendChild(accountListElement);
            accountListXmlDocument.Save(DataFilePath);

            IList<Account> accountList = _fileAccountStorage.Load();
            Assert.AreEqual(0, accountList.Count);
        }

        [TestMethod]
        public void Loadはデータファイルが空でないAccountListXmlの場合にその内容に従ったアカウントリストを返す()
        {
            Directory.CreateDirectory(TestDataFolderPath);

            Account account1 = new Account();

            Account account2 = new Account()
            {
                AccountName = "x<y>z",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "あ",
            };

            List<Account> orgAccountList = new List<Account>();
            orgAccountList.Add(account1);
            orgAccountList.Add(account2);

            XmlDocument accountListXmlDocument = new XmlDocument();

            XmlElement accountListElement = accountListXmlDocument.CreateElement("AccountList");
            accountListElement.SetAttribute("formatVersion", "1.0");
            accountListElement.SetAttribute("encrypted", "false");

            foreach (Account account in orgAccountList)
            {
                accountListElement.AppendChild(account.ToXmlElement(accountListXmlDocument));
            }

            accountListXmlDocument.AppendChild(accountListElement);
            accountListXmlDocument.Save(DataFilePath);

            IList<Account> returnedAccountList = _fileAccountStorage.Load();

            Assert.AreEqual(orgAccountList.Count, returnedAccountList.Count);

            for (int i = 0; i < returnedAccountList.Count; i++)
            {
                Assert.IsTrue(returnedAccountList[i].Equals(orgAccountList[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AccountStorageException))]
        public void Loadはデータファイルの読み込みに失敗した場合にAccountStorageExceptionを投げる_IOException発生()
        {
            Directory.CreateDirectory(TestDataFolderPath);

            using (FileStream fs = new FileStream(DataFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                IList<Account> accountList = _fileAccountStorage.Load();
            }
        }

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Xml;

namespace AccountManagerApp.Tests
{
    [TestClass]
    public class AccountListXmlProcessorTests
    {
        private AccountListXmlProcessor _accountListXmlProcessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _accountListXmlProcessor = new AccountListXmlProcessor();
        }

        [TestMethod]
        public void CreateAccountListXmlDocumentのテスト_空のアカウントリスト()
        {
            List<Account> accountList = new List<Account>();

            XmlDocument doc = _accountListXmlProcessor.CreateAccountListXmlDocument(accountList);

            XmlElement rootElement = doc.DocumentElement;

            Assert.AreEqual("AccountList", rootElement.Name);
            Assert.AreEqual("1.0", rootElement.Attributes["formatVersion"].Value);
            Assert.AreEqual("false", rootElement.Attributes["encrypted"].Value);

            Assert.AreEqual(0, rootElement.ChildNodes.Count);
        }

        [TestMethod]
        public void CreateAccountListXmlDocumentのテスト_空でないアカウントリスト()
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

            List<Account> accountList = new List<Account>();
            accountList.Add(account1);
            accountList.Add(account2);

            XmlDocument doc = _accountListXmlProcessor.CreateAccountListXmlDocument(accountList);

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
        public void ParseAccountListXmlDocumentはaccountListXmlDocumentが不正な場合にInvalidXmlExceptionを投げる()
        {
            XmlDocument accountListXmlDocument;
            IList<Account> accountList;

            try
            {
                accountListXmlDocument = new XmlDocument();

                accountList = _accountListXmlProcessor.ParseAccountListXmlDocument(accountListXmlDocument);
                Assert.Fail();
            }
            catch (InvalidXmlException)
            {
                // OK
            }

            try
            {
                accountListXmlDocument = new XmlDocument();

                XmlElement accountListElement = accountListXmlDocument.CreateElement("AccountList");
                accountListXmlDocument.AppendChild(accountListElement);

                accountList = _accountListXmlProcessor.ParseAccountListXmlDocument(accountListXmlDocument);
                Assert.Fail();
            }
            catch (InvalidXmlException)
            {
                // OK
            }

            try
            {
                accountListXmlDocument = new XmlDocument();

                XmlElement accountListElement = accountListXmlDocument.CreateElement("AccountList");
                accountListElement.SetAttribute("formatVersion", "1.0");
                accountListXmlDocument.AppendChild(accountListElement);

                accountList = _accountListXmlProcessor.ParseAccountListXmlDocument(accountListXmlDocument);
                Assert.Fail();
            }
            catch (InvalidXmlException)
            {
                // OK
            }

            try
            {
                accountListXmlDocument = new XmlDocument();

                XmlElement accountListElement = accountListXmlDocument.CreateElement("AccountList");
                accountListElement.SetAttribute("formatVersion", "1.1");
                accountListElement.SetAttribute("encrypted", "false");
                accountListXmlDocument.AppendChild(accountListElement);

                accountList = _accountListXmlProcessor.ParseAccountListXmlDocument(accountListXmlDocument);
                Assert.Fail();
            }
            catch (InvalidXmlException)
            {
                // OK
            }

            try
            {
                accountListXmlDocument = new XmlDocument();

                XmlElement accountListElement = accountListXmlDocument.CreateElement("AccountList");
                accountListElement.SetAttribute("formatVersion", "1.0");
                accountListElement.SetAttribute("encrypted", "true");
                accountListXmlDocument.AppendChild(accountListElement);

                accountList = _accountListXmlProcessor.ParseAccountListXmlDocument(accountListXmlDocument);
                Assert.Fail();
            }
            catch (InvalidXmlException)
            {
                // OK
            }

            try
            {
                accountListXmlDocument = new XmlDocument();

                XmlElement accountListElement = accountListXmlDocument.CreateElement("AccountListX");
                accountListElement.SetAttribute("formatVersion", "1.0");
                accountListElement.SetAttribute("encrypted", "false");
                accountListXmlDocument.AppendChild(accountListElement);

                accountList = _accountListXmlProcessor.ParseAccountListXmlDocument(accountListXmlDocument);
                Assert.Fail();
            }
            catch (InvalidXmlException)
            {
                // OK
            }

            try
            {
                accountListXmlDocument = new XmlDocument();

                XmlElement accountListElement = accountListXmlDocument.CreateElement("AccountList");
                accountListElement.SetAttribute("formatVersion", "1.0");
                accountListElement.SetAttribute("encrypted", "false");
                accountListXmlDocument.AppendChild(accountListElement);

                accountList = _accountListXmlProcessor.ParseAccountListXmlDocument(accountListXmlDocument);
                // OK

                Assert.AreEqual(0, accountList.Count);
            }
            catch (InvalidXmlException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ParseAccountListXmlDocumentはaccountListXmlDocument内の要素が不正な場合にInvalidXmlExceptionを投げる()
        {
            XmlDocument accountListXmlDocument;
            IList<Account> accountList;

            try
            {
                accountListXmlDocument = new XmlDocument();

                XmlElement accountListElement = accountListXmlDocument.CreateElement("AccountList");
                accountListElement.SetAttribute("formatVersion", "1.0");
                accountListElement.SetAttribute("encrypted", "false");

                XmlElement accountElement = accountListXmlDocument.CreateElement("Account");
                accountListElement.AppendChild(accountElement);

                accountListXmlDocument.AppendChild(accountListElement);

                accountList = _accountListXmlProcessor.ParseAccountListXmlDocument(accountListXmlDocument);
                // OK

                Assert.AreEqual(1, accountList.Count);
            }
            catch (InvalidXmlException)
            {
                Assert.Fail();
            }

            try
            {
                accountListXmlDocument = new XmlDocument();

                XmlElement accountListElement = accountListXmlDocument.CreateElement("AccountList");
                accountListElement.SetAttribute("formatVersion", "1.0");
                accountListElement.SetAttribute("encrypted", "false");

                XmlElement accountElement = accountListXmlDocument.CreateElement("AccountX");
                accountListElement.AppendChild(accountElement);

                accountListXmlDocument.AppendChild(accountListElement);

                accountList = _accountListXmlProcessor.ParseAccountListXmlDocument(accountListXmlDocument);

                Assert.Fail();
            }
            catch (InvalidXmlException)
            {
                // OK
            }
        }

        [TestMethod]
        public void ParseAccountListXmlDocumentはaccountListXmlDocument内の全アカウントのリストを返す()
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

            IList<Account> returnedAccountList = _accountListXmlProcessor.ParseAccountListXmlDocument(accountListXmlDocument);

            Assert.AreEqual(orgAccountList.Count, returnedAccountList.Count);

            for (int i = 0; i < returnedAccountList.Count; i++)
            {
                Assert.IsTrue(returnedAccountList[i].Equals(orgAccountList[i]));
            }
        }

        [TestMethod]
        public void ParseAccountListXmlDocumentは空のaccountListXmlDocumentに対して空のリストを返す()
        {
            XmlDocument accountListXmlDocument = new XmlDocument();

            XmlElement accountListElement = accountListXmlDocument.CreateElement("AccountList");
            accountListElement.SetAttribute("formatVersion", "1.0");
            accountListElement.SetAttribute("encrypted", "false");

            accountListXmlDocument.AppendChild(accountListElement);

            IList<Account> returnedAccountList = _accountListXmlProcessor.ParseAccountListXmlDocument(accountListXmlDocument);

            Assert.AreEqual(0, returnedAccountList.Count);
        }

        [TestMethod]
        public void ParseAccountListXmlDocumentはaccountListXmlDocument内のXmlElement以外のノードは無視する()
        {
            Account account1 = new Account()
            {
                AccountName = "x<y>z",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "あ",
            };

            XmlDocument accountListXmlDocument = new XmlDocument();

            XmlElement accountListElement = accountListXmlDocument.CreateElement("AccountList");
            accountListElement.SetAttribute("formatVersion", "1.0");
            accountListElement.SetAttribute("encrypted", "false");

            XmlText textNode = accountListXmlDocument.CreateTextNode("text node");
            accountListElement.AppendChild(textNode);

            accountListElement.AppendChild(account1.ToXmlElement(accountListXmlDocument));

            XmlComment commentNode = accountListXmlDocument.CreateComment("comment node");
            accountListElement.AppendChild(commentNode);

            accountListXmlDocument.AppendChild(accountListElement);

            IList<Account> returnedAccountList = _accountListXmlProcessor.ParseAccountListXmlDocument(accountListXmlDocument);

            Assert.AreEqual(1, returnedAccountList.Count);
            Assert.IsTrue(returnedAccountList[0].Equals(account1));
        }

    }
}

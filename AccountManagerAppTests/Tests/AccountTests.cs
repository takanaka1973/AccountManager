using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;

namespace AccountManagerApp.Tests
{
    [TestClass]
    public class AccountTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public void 各プロパティの初期値は空文字列()
        {
            Account account = new Account();

            Assert.AreEqual("", account.AccountName);
            Assert.AreEqual("", account.UserId);
            Assert.AreEqual("", account.Password);
            Assert.AreEqual("", account.Url);
            Assert.AreEqual("", account.Remarks);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AccountNameにはnullを設定できない()
        {
            Account account = new Account();
            account.AccountName = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserIdにはnullを設定できない()
        {
            Account account = new Account();
            account.UserId = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Passwordにはnullを設定できない()
        {
            Account account = new Account();
            account.Password = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Urlにはnullを設定できない()
        {
            Account account = new Account();
            account.Url = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Remarksにはnullを設定できない()
        {
            Account account = new Account();
            account.Remarks = null;
        }

        [TestMethod]
        public void AccountNameを変更するとPropertyChangedイベントが発火される()
        {
            Account account = new Account();
            bool eventFired = false;

            account.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual("AccountName", e.PropertyName);
            };

            account.AccountName = "abc";

            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void AccountNameに同じ値を設定してもPropertyChangedイベントは発火されない()
        {
            Account account = new Account();
            bool eventFired = false;

            account.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual("AccountName", e.PropertyName);
            };

            account.AccountName = "";
            Assert.IsFalse(eventFired);

            eventFired = false;
            account.AccountName = "abc";
            Assert.IsTrue(eventFired);

            eventFired = false;
            account.AccountName = "abc";
            Assert.IsFalse(eventFired);
        }

        [TestMethod]
        public void UserIdを変更するとPropertyChangedイベントが発火される()
        {
            Account account = new Account();
            bool eventFired = false;

            account.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual("UserId", e.PropertyName);
            };

            account.UserId = "abc";

            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void UserIdに同じ値を設定してもPropertyChangedイベントは発火されない()
        {
            Account account = new Account();
            bool eventFired = false;

            account.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual("UserId", e.PropertyName);
            };

            account.UserId = "";
            Assert.IsFalse(eventFired);

            eventFired = false;
            account.UserId = "abc";
            Assert.IsTrue(eventFired);

            eventFired = false;
            account.UserId = "abc";
            Assert.IsFalse(eventFired);
        }

        [TestMethod]
        public void Passwordを変更するとPropertyChangedイベントが発火される()
        {
            Account account = new Account();
            bool eventFired = false;

            account.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual("Password", e.PropertyName);
            };

            account.Password = "abc";

            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void Passwordに同じ値を設定してもPropertyChangedイベントは発火されない()
        {
            Account account = new Account();
            bool eventFired = false;

            account.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual("Password", e.PropertyName);
            };

            account.Password = "";
            Assert.IsFalse(eventFired);

            eventFired = false;
            account.Password = "abc";
            Assert.IsTrue(eventFired);

            eventFired = false;
            account.Password = "abc";
            Assert.IsFalse(eventFired);
        }

        [TestMethod]
        public void Urlを変更するとPropertyChangedイベントが発火される()
        {
            Account account = new Account();
            bool eventFired = false;

            account.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual("Url", e.PropertyName);
            };

            account.Url = "abc";

            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void Urlに同じ値を設定してもPropertyChangedイベントは発火されない()
        {
            Account account = new Account();
            bool eventFired = false;

            account.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual("Url", e.PropertyName);
            };

            account.Url = "";
            Assert.IsFalse(eventFired);

            eventFired = false;
            account.Url = "abc";
            Assert.IsTrue(eventFired);

            eventFired = false;
            account.Url = "abc";
            Assert.IsFalse(eventFired);
        }

        [TestMethod]
        public void Remarksを変更するとPropertyChangedイベントが発火される()
        {
            Account account = new Account();
            bool eventFired = false;

            account.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual("Remarks", e.PropertyName);
            };

            account.Remarks = "abc";

            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void Remarksに同じ値を設定してもPropertyChangedイベントは発火されない()
        {
            Account account = new Account();
            bool eventFired = false;

            account.PropertyChanged += (sender, e) =>
            {
                eventFired = true;
                Assert.AreEqual("Remarks", e.PropertyName);
            };

            account.Remarks = "";
            Assert.IsFalse(eventFired);

            eventFired = false;
            account.Remarks = "abc";
            Assert.IsTrue(eventFired);

            eventFired = false;
            account.Remarks = "abc";
            Assert.IsFalse(eventFired);
        }

        [TestMethod]
        public void Cloneは複製を返す()
        {
            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            Account account2 = account1.Clone();

            Assert.AreNotSame(account1, account2);

            Assert.AreEqual(account1.AccountName, account2.AccountName);
            Assert.AreEqual(account1.UserId, account2.UserId);
            Assert.AreEqual(account1.Password, account2.Password);
            Assert.AreEqual(account1.Url, account2.Url);
            Assert.AreEqual(account1.Remarks, account2.Remarks);
        }

        [TestMethod]
        public void ICloneable_Cloneは複製を返す()
        {
            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            ICloneable cloneable = account1;

            Account account2 = (Account)cloneable.Clone();

            Assert.AreNotSame(account1, account2);

            Assert.AreEqual(account1.AccountName, account2.AccountName);
            Assert.AreEqual(account1.UserId, account2.UserId);
            Assert.AreEqual(account1.Password, account2.Password);
            Assert.AreEqual(account1.Url, account2.Url);
            Assert.AreEqual(account1.Remarks, account2.Remarks);
        }

        [TestMethod]
        public void CopyFromは渡されたAccountの全プロパティを自分に設定する()
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

            account2.CopyFrom(account1);

            Assert.AreEqual(account1.AccountName, account2.AccountName);
            Assert.AreEqual(account1.UserId, account2.UserId);
            Assert.AreEqual(account1.Password, account2.Password);
            Assert.AreEqual(account1.Url, account2.Url);
            Assert.AreEqual(account1.Remarks, account2.Remarks);
        }

        [TestMethod]
        public void Equalsのテスト()
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
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            Account account3 = new Account()
            {
                AccountName = "a*",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            Account account4 = new Account()
            {
                AccountName = "a",
                UserId = "b*",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            Account account5 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c*",
                Url = "d",
                Remarks = "e",
            };

            Account account6 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d*",
                Remarks = "e",
            };

            Account account7 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e*",
            };

            Assert.IsFalse(account1.Equals(null));
            Assert.IsTrue(account1.Equals(account1));

            Assert.IsTrue(account1.Equals(account2));

            Assert.IsFalse(account1.Equals(account3));
            Assert.IsFalse(account1.Equals(account4));
            Assert.IsFalse(account1.Equals(account5));
            Assert.IsFalse(account1.Equals(account6));
            Assert.IsFalse(account1.Equals(account7));
        }

        [TestMethod]
        public void ToXmlElementのテスト_初期値の場合()
        {
            Account account1 = new Account();

            XmlDocument doc = new XmlDocument();
            XmlElement xmlElement1 = account1.ToXmlElement(doc);

            string expectedXmlText1 = "<Account><AccountName></AccountName><UserId></UserId><Password></Password><Url></Url><Remarks></Remarks></Account>";
            Assert.AreEqual(expectedXmlText1, xmlElement1.OuterXml);
        }

        [TestMethod]
        public void ToXmlElementのテスト_値がある場合()
        {
            Account account1 = new Account()
            {
                AccountName = "a",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            XmlDocument doc = new XmlDocument();
            XmlElement xmlElement1 = account1.ToXmlElement(doc);

            string expectedXmlText1 = "<Account><AccountName>a</AccountName><UserId>b</UserId><Password>c</Password><Url>d</Url><Remarks>e</Remarks></Account>";
            Assert.AreEqual(expectedXmlText1, xmlElement1.OuterXml);
        }

        [TestMethod]
        public void ToXmlElementのテスト_値が日本語の場合()
        {
            Account account1 = new Account()
            {
                AccountName = "あ",
                UserId = "い",
                Password = "う",
                Url = "え",
                Remarks = "お",
            };

            XmlDocument doc = new XmlDocument();
            XmlElement xmlElement1 = account1.ToXmlElement(doc);

            string expectedXmlText1 = "<Account><AccountName>あ</AccountName><UserId>い</UserId><Password>う</Password><Url>え</Url><Remarks>お</Remarks></Account>";
            Assert.AreEqual(expectedXmlText1, xmlElement1.OuterXml);
        }

        [TestMethod]
        public void ToXmlElementのテスト_値にエスケープが必要な文字を含む場合()
        {
            Account account1 = new Account()
            {
                AccountName = "x<y>z",
                UserId = "b",
                Password = "c",
                Url = "d",
                Remarks = "e",
            };

            XmlDocument doc = new XmlDocument();
            XmlElement xmlElement1 = account1.ToXmlElement(doc);

            string expectedXmlText1 = "<Account><AccountName>x&lt;y&gt;z</AccountName><UserId>b</UserId><Password>c</Password><Url>d</Url><Remarks>e</Remarks></Account>";
            Assert.AreEqual(expectedXmlText1, xmlElement1.OuterXml);
        }

        [TestMethod]
        public void FromXmlElementはaccountXmlElementが子要素を持たない場合に全フィールドが空文字列のアカウントを返す()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement accountXmlElement = xmlDocument.CreateElement("Account");

            Account account = Account.FromXmlElement(accountXmlElement);

            Assert.AreEqual("", account.AccountName);
            Assert.AreEqual("", account.UserId);
            Assert.AreEqual("", account.Password);
            Assert.AreEqual("", account.Url);
            Assert.AreEqual("", account.Remarks);
        }

        [TestMethod]
        public void FromXmlElementは各フィールドの値がaccountXmlElementの子要素通りのアカウントを返す()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement accountXmlElement = xmlDocument.CreateElement("Account");

            XmlElement accountNameElement = xmlDocument.CreateElement("AccountName");
            accountNameElement.InnerText = "x<y>z";
            accountXmlElement.AppendChild(accountNameElement);

            XmlElement userIdElement = xmlDocument.CreateElement("UserId");
            userIdElement.InnerText = "あ";
            accountXmlElement.AppendChild(userIdElement);

            XmlElement passwordElement = xmlDocument.CreateElement("Password");
            passwordElement.InnerText = "い";
            accountXmlElement.AppendChild(passwordElement);

            XmlElement urlElement = xmlDocument.CreateElement("Url");
            urlElement.InnerText = "う";
            accountXmlElement.AppendChild(urlElement);

            XmlElement remarksElement = xmlDocument.CreateElement("Remarks");
            remarksElement.InnerText = "え";
            accountXmlElement.AppendChild(remarksElement);

            Account account = Account.FromXmlElement(accountXmlElement);

            Assert.AreEqual("x<y>z", account.AccountName);
            Assert.AreEqual("あ", account.UserId);
            Assert.AreEqual("い", account.Password);
            Assert.AreEqual("う", account.Url);
            Assert.AreEqual("え", account.Remarks);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidXmlException))]
        public void FromXmlElementはaccountXmlElementの名前が正しくない場合にInvalidXmlExceptionを投げる()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement accountXmlElement = xmlDocument.CreateElement("AccountX");

            Account account = Account.FromXmlElement(accountXmlElement);
        }

        [TestMethod]
        public void FromXmlElementはaccountXmlElement内の特定の子要素が無い場合にアカウントのそのフィールドに空文字列を設定して返す()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement accountXmlElement = xmlDocument.CreateElement("Account");

            XmlElement userIdElement = xmlDocument.CreateElement("UserId");
            userIdElement.InnerText = "あ";
            accountXmlElement.AppendChild(userIdElement);

            XmlElement passwordElement = xmlDocument.CreateElement("Password");
            passwordElement.InnerText = "い";
            accountXmlElement.AppendChild(passwordElement);

            XmlElement urlElement = xmlDocument.CreateElement("Url");
            urlElement.InnerText = "う";
            accountXmlElement.AppendChild(urlElement);

            XmlElement remarksElement = xmlDocument.CreateElement("Remarks");
            remarksElement.InnerText = "え";
            accountXmlElement.AppendChild(remarksElement);

            Account account = Account.FromXmlElement(accountXmlElement);

            Assert.AreEqual("", account.AccountName);
            Assert.AreEqual("あ", account.UserId);
            Assert.AreEqual("い", account.Password);
            Assert.AreEqual("う", account.Url);
            Assert.AreEqual("え", account.Remarks);

            accountXmlElement.RemoveChild(userIdElement);
            account = Account.FromXmlElement(accountXmlElement);

            Assert.AreEqual("", account.AccountName);
            Assert.AreEqual("", account.UserId);
            Assert.AreEqual("い", account.Password);
            Assert.AreEqual("う", account.Url);
            Assert.AreEqual("え", account.Remarks);

            accountXmlElement.RemoveChild(passwordElement);
            account = Account.FromXmlElement(accountXmlElement);

            Assert.AreEqual("", account.AccountName);
            Assert.AreEqual("", account.UserId);
            Assert.AreEqual("", account.Password);
            Assert.AreEqual("う", account.Url);
            Assert.AreEqual("え", account.Remarks);

            accountXmlElement.RemoveChild(urlElement);
            account = Account.FromXmlElement(accountXmlElement);

            Assert.AreEqual("", account.AccountName);
            Assert.AreEqual("", account.UserId);
            Assert.AreEqual("", account.Password);
            Assert.AreEqual("", account.Url);
            Assert.AreEqual("え", account.Remarks);

            accountXmlElement.RemoveChild(remarksElement);
            account = Account.FromXmlElement(accountXmlElement);

            Assert.AreEqual("", account.AccountName);
            Assert.AreEqual("", account.UserId);
            Assert.AreEqual("", account.Password);
            Assert.AreEqual("", account.Url);
            Assert.AreEqual("", account.Remarks);
        }

        [TestMethod]
        public void FromXmlElementはaccountXmlElement内のXmlElement以外のノードは無視する()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement accountXmlElement = xmlDocument.CreateElement("Account");

            XmlText textNode = xmlDocument.CreateTextNode("text node");
            accountXmlElement.AppendChild(textNode);

            XmlElement accountNameElement = xmlDocument.CreateElement("AccountName");
            accountNameElement.InnerText = "x<y>z";
            accountXmlElement.AppendChild(accountNameElement);

            XmlElement userIdElement = xmlDocument.CreateElement("UserId");
            userIdElement.InnerText = "あ";
            accountXmlElement.AppendChild(userIdElement);

            XmlElement passwordElement = xmlDocument.CreateElement("Password");
            passwordElement.InnerText = "い";
            accountXmlElement.AppendChild(passwordElement);

            XmlElement urlElement = xmlDocument.CreateElement("Url");
            urlElement.InnerText = "う";
            accountXmlElement.AppendChild(urlElement);

            XmlElement remarksElement = xmlDocument.CreateElement("Remarks");
            remarksElement.InnerText = "え";
            accountXmlElement.AppendChild(remarksElement);

            XmlComment commentNode = xmlDocument.CreateComment("comment node");
            accountXmlElement.AppendChild(commentNode);

            Account account = Account.FromXmlElement(accountXmlElement);

            Assert.AreEqual("x<y>z", account.AccountName);
            Assert.AreEqual("あ", account.UserId);
            Assert.AreEqual("い", account.Password);
            Assert.AreEqual("う", account.Url);
            Assert.AreEqual("え", account.Remarks);
        }

    }
}

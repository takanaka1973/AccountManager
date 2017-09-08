using System;
using System.Xml;

namespace AccountManagerApp
{
    /// <summary>
    /// 一つのアカウント。
    /// </summary>
    public class Account : ObservableObject, ICloneable
    {
        private const string AccountXmlElementName = "Account";

        private const string AccountNameXmlElementName = "AccountName";
        private const string UserIdXmlElementName = "UserId";
        private const string PasswordXmlElementName = "Password";
        private const string UrlXmlElementName = "Url";
        private const string RemarksXmlElementName = "Remarks";

        private string _accountName = "";
        private string _userId = "";
        private string _password = "";
        private string _url = "";
        private string _remarks = "";

        /// <summary>
        /// アカウント名。
        /// </summary>
        /// <remarks>nullの設定は不可。</remarks>
        public string AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                SetNonNullStringProperty(ref _accountName, nameof(AccountName), value);
            }
        }

        /// <summary>
        /// ユーザID。
        /// </summary>
        /// <remarks>nullの設定は不可。</remarks>
        public string UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                SetNonNullStringProperty(ref _userId, nameof(UserId), value);
            }
        }

        /// <summary>
        /// パスワード。
        /// </summary>
        /// <remarks>nullの設定は不可。</remarks>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                SetNonNullStringProperty(ref _password, nameof(Password), value);
            }
        }

        /// <summary>
        /// URL。
        /// </summary>
        /// <remarks>nullの設定は不可。</remarks>
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                SetNonNullStringProperty(ref _url, nameof(Url), value);
            }
        }

        /// <summary>
        /// 備考。
        /// </summary>
        /// <remarks>nullの設定は不可。</remarks>
        public string Remarks
        {
            get
            {
                return _remarks;
            }
            set
            {
                SetNonNullStringProperty(ref _remarks, nameof(Remarks), value);
            }
        }

        /// <summary>
        /// このアカウントの複製を作成する。
        /// </summary>
        /// <returns>このアカウントの複製</returns>
        public Account Clone()
        {
            var newAccount = new Account()
            {
                AccountName = this.AccountName,
                UserId = this.UserId,
                Password = this.Password,
                Url = this.Url,
                Remarks = this.Remarks,
            };

            return newAccount;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// 指定されたアカウントの内容をこのアカウントにコピーする。
        /// </summary>
        /// <param name="sourceAccount">コピー元のアカウント(null指定不可)</param>
        public void CopyFrom(Account sourceAccount)
        {
            Utilities.RejectNull(sourceAccount, nameof(sourceAccount));

            AccountName = sourceAccount.AccountName;
            UserId = sourceAccount.UserId;
            Password = sourceAccount.Password;
            Url = sourceAccount.Url;
            Remarks = sourceAccount.Remarks;
        }

        /// <summary>
        /// このアカウントと同内容か比較する。
        /// </summary>
        /// <param name="theOther">比較対象のアカウント</param>
        /// <returns>同内容の場合はtrue、それ以外の場合はfalse</returns>
        public bool Equals(Account theOther)
        {
            if (theOther == null)
            {
                return false;
            }

            if (AccountName == theOther.AccountName && UserId == theOther.UserId
                && Password == theOther.Password && Url == theOther.Url && Remarks == theOther.Remarks)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// このアカウントの内容を持つXmlElementを生成する。
        /// </summary>
        /// <param name="xmlDocument">XmlElementの生成に利用するXmlDocument(null指定不可)</param>
        /// <returns>生成したXmlElement</returns>
        /// <remarks>このメソッドは生成したXmlElementの<paramref name="xmlDocument"/>への追加は行わない。</remarks>
        public XmlElement ToXmlElement(XmlDocument xmlDocument)
        {
            Utilities.RejectNull(xmlDocument, nameof(xmlDocument));

            XmlElement accountElement = xmlDocument.CreateElement(AccountXmlElementName);

            XmlElement accountNameElement = xmlDocument.CreateElement(AccountNameXmlElementName);
            accountNameElement.InnerText = _accountName;
            accountElement.AppendChild(accountNameElement);

            XmlElement userIdElement = xmlDocument.CreateElement(UserIdXmlElementName);
            userIdElement.InnerText = _userId;
            accountElement.AppendChild(userIdElement);

            XmlElement passwordElement = xmlDocument.CreateElement(PasswordXmlElementName);
            passwordElement.InnerText = _password;
            accountElement.AppendChild(passwordElement);

            XmlElement urlElement = xmlDocument.CreateElement(UrlXmlElementName);
            urlElement.InnerText = _url;
            accountElement.AppendChild(urlElement);

            XmlElement remarksElement = xmlDocument.CreateElement(RemarksXmlElementName);
            remarksElement.InnerText = _remarks;
            accountElement.AppendChild(remarksElement);

            return accountElement;
        }

        /// <summary>
        /// アカウントのXmlElementをパースしてアカウントを生成する。
        /// </summary>
        /// <param name="accountXmlElement">アカウントのXmlElement(null指定不可)</param>
        /// <returns>生成されたアカウント</returns>
        /// <exception cref="InvalidXmlException"><paramref name="accountXmlElement"/>が不正な場合</exception>
        public static Account FromXmlElement(XmlElement accountXmlElement)
        {
            Utilities.RejectNull(accountXmlElement, nameof(accountXmlElement));

            if (accountXmlElement.Name != AccountXmlElementName)
            {
                throw new InvalidXmlException("Invalid element name.");
            }

            Account account = new Account();

            foreach (XmlNode childXmlNode in accountXmlElement.ChildNodes)
            {
                if (childXmlNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                XmlElement childXmlElement = (XmlElement)childXmlNode;

                switch (childXmlElement.Name)
                {
                    case AccountNameXmlElementName:
                        account.AccountName = childXmlElement.InnerText;
                        break;
                    case UserIdXmlElementName:
                        account.UserId = childXmlElement.InnerText;
                        break;
                    case PasswordXmlElementName:
                        account.Password = childXmlElement.InnerText;
                        break;
                    case UrlXmlElementName:
                        account.Url = childXmlElement.InnerText;
                        break;
                    case RemarksXmlElementName:
                        account.Remarks = childXmlElement.InnerText;
                        break;
                }
            }

            return account;
        }

    }
}

using System.Collections.Generic;
using System.Xml;

namespace AccountManagerApp
{
    /// <summary>
    /// アカウントリストのXMLを処理するクラス。
    /// </summary>
    public class AccountListXmlProcessor
    {
        private const string AccountListElementName = "AccountList";

        private const string FormatVersionAttrName = "formatVersion";
        private const string CurrentFormatVersion = "1.0";

        private const string EncryptedAttrName = "encrypted";
        private const string CurrentEncryptedAttrValue = "false";

        /// <summary>
        /// アカウントのリストを保持するXmlDocumentを生成する。
        /// </summary>
        /// <param name="accountList">アカウントのリスト(null指定不可)</param>
        /// <returns>アカウントのリストを保持するXmlDocument</returns>
        public XmlDocument CreateAccountListXmlDocument(IEnumerable<Account> accountList)
        {
            Utilities.RejectNull(accountList, nameof(accountList));

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null));

            XmlElement accountListElement = xmlDocument.CreateElement(AccountListElementName);
            accountListElement.SetAttribute(FormatVersionAttrName, CurrentFormatVersion);
            accountListElement.SetAttribute(EncryptedAttrName, CurrentEncryptedAttrValue);

            foreach (Account account in accountList)
            {
                accountListElement.AppendChild(account.ToXmlElement(xmlDocument));
            }

            xmlDocument.AppendChild(accountListElement);

            return xmlDocument;
        }

        /// <summary>
        /// アカウントリストのXmlDocumentをパースする。
        /// </summary>
        /// <param name="accountListXmlDocument">アカウントリストのXmlDocument(null指定不可)</param>
        /// <returns>アカウントリスト</returns>
        /// <exception cref="InvalidXmlException"><paramref name="accountListXmlDocument"/>が不正な場合</exception>
        public IList<Account> ParseAccountListXmlDocument(XmlDocument accountListXmlDocument)
        {
            Utilities.RejectNull(accountListXmlDocument, nameof(accountListXmlDocument));

            ValidateAccountListXmlDocument(accountListXmlDocument);

            List<Account> accountList = new List<Account>();

            foreach (XmlNode xmlNode in accountListXmlDocument.DocumentElement.ChildNodes)
            {
                if (xmlNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                XmlElement accountXmlElement = (XmlElement)xmlNode;
                accountList.Add(Account.FromXmlElement(accountXmlElement));
            }

            return accountList;
        }

        private void ValidateAccountListXmlDocument(XmlDocument accountListXmlDocument)
        {
            XmlElement accountListElement = accountListXmlDocument.DocumentElement;

            if (accountListElement == null)
            {
                throw new InvalidXmlException("No root element.");
            }

            if (accountListElement.Name != AccountListElementName)
            {
                throw new InvalidXmlException("The root element must be an account list element.");
            }

            if (!accountListElement.HasAttribute(FormatVersionAttrName))
            {
                throw new InvalidXmlException("No format version attribute.");
            }

            if (accountListElement.Attributes[FormatVersionAttrName].Value != CurrentFormatVersion)
            {
                throw new InvalidXmlException("Invalid format version.");
            }

            if (!accountListElement.HasAttribute(EncryptedAttrName))
            {
                throw new InvalidXmlException("No encrypted attribute.");
            }

            if (accountListElement.Attributes[EncryptedAttrName].Value != CurrentEncryptedAttrValue)
            {
                throw new InvalidXmlException("Invalid encrypted attribute value.");
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace AccountManagerApp
{
    /// <summary>
    /// ファイルベースのアカウントストレージ。
    /// </summary>
    public class FileAccountStorage : AccountStorage
    {
        private const string DefaultDataFolderName = "Account Manager";
        private const string DataFileName = "AccountList.xml";

        private const string BackupFolderName = "backup";
        private const string BackupFileNameFormat = "AccountList_{0}.xml";

        private readonly AccountListXmlProcessor _accountListXmlProcessor = new AccountListXmlProcessor();
        private readonly string _dataFolderPath;

        public delegate DateTime GetCurrentDateTimeDelegate();
        public GetCurrentDateTimeDelegate GetCurrentDateTime { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <remarks>このコンストラクタを用いた場合、データ格納フォルダはデフォルトの場所になる。</remarks>
        public FileAccountStorage() : this(GetDefaultDataFolderPath())
        {
        }

        private static string GetDefaultDataFolderPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DefaultDataFolderName);
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dataFolderPath">データを格納するフォルダのパス(null指定不可)</param>
        public FileAccountStorage(string dataFolderPath)
        {
            Utilities.RejectNull(dataFolderPath, nameof(dataFolderPath));

            _dataFolderPath = dataFolderPath;
            GetCurrentDateTime = GetCurrentDateTimeImpl;
        }

        private DateTime GetCurrentDateTimeImpl()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// <see cref="AccountStorage.Save(IEnumerable{Account})"/>の実装。
        /// </summary>
        public void Save(IEnumerable<Account> accountList)
        {
            Utilities.RejectNull(accountList, nameof(accountList));

            XmlDocument xmlDocument = _accountListXmlProcessor.CreateAccountListXmlDocument(accountList);

            string dataFilePath = Path.Combine(_dataFolderPath, DataFileName);
            string backupFolderPath = Path.Combine(_dataFolderPath, BackupFolderName);

            try
            {
                Directory.CreateDirectory(_dataFolderPath);
                Directory.CreateDirectory(backupFolderPath);

                if (File.Exists(dataFilePath))
                {
                    File.Copy(dataFilePath, GetBackupFilePath(backupFolderPath), false);
                }

                xmlDocument.Save(dataFilePath);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new AccountStorageException("Saving accounts failed.", ex);
            }
        }

        private string GetBackupFilePath(string backupFolderPath)
        {
            string currentDateTimeStr = GetCurrentDateTime().ToString("yyyyMMddHHmmss");
            string backupFileName = string.Format(BackupFileNameFormat, currentDateTimeStr);

            return Path.Combine(backupFolderPath, backupFileName);
        }

        /// <summary>
        /// <see cref="AccountStorage.Load"/>の実装。
        /// </summary>
        public IList<Account> Load()
        {
            if (!Directory.Exists(_dataFolderPath))
            {
                return new List<Account>();
            }

            string dataFilePath = Path.Combine(_dataFolderPath, DataFileName);

            if (!File.Exists(dataFilePath))
            {
                return new List<Account>();
            }

            try
            {
                XmlDocument accountListXmlDocument = new XmlDocument();
                accountListXmlDocument.Load(dataFilePath);

                return _accountListXmlProcessor.ParseAccountListXmlDocument(accountListXmlDocument);
            }
            catch (Exception ex) when (ex is XmlException || ex is InvalidXmlException
                || ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new AccountStorageException("Failed to load accounts.", ex);
            }
        }

    }
}

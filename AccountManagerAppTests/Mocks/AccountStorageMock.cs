using System.Collections.Generic;

namespace AccountManagerApp.Tests
{
    public class AccountStorageMock : AccountStorage
    {
        public IList<Account> SavedAccounts { get; } = new List<Account>();
        public bool ThrowAccountStorageExceptionOnSave { get; set; } = false;

        public IList<Account> AccountsToBeLoaded { get; } = new List<Account>();
        public bool ThrowAccountStorageExceptionOnLoad { get; set; } = false;

        public void Save(IEnumerable<Account> accountList)
        {
            if (ThrowAccountStorageExceptionOnSave)
            {
                throw new AccountStorageException();
            }

            SavedAccounts.Clear();

            foreach (Account account in accountList)
            {
                SavedAccounts.Add(account.Clone());
            }
        }

        public IList<Account> Load()
        {
            if (ThrowAccountStorageExceptionOnLoad)
            {
                throw new AccountStorageException();
            }

            return AccountsToBeLoaded;
        }

    }
}

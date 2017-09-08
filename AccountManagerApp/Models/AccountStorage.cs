using System.Collections.Generic;

namespace AccountManagerApp
{
    /// <summary>
    /// アカウントのストレージ。
    /// </summary>
    public interface AccountStorage
    {
        /// <summary>
        /// アカウントを保存する。
        /// </summary>
        /// <param name="accountList">保存対象の全アカウント(null指定不可)</param>
        /// <exception cref="AccountStorageException">保存に失敗した場合</exception>
        void Save(IEnumerable<Account> accountList);

        /// <summary>
        /// アカウントを読み込む。
        /// </summary>
        /// <returns>読み込んだアカウントのリスト</returns>
        /// <exception cref="AccountStorageException">読み込みに失敗した場合</exception>
        IList<Account> Load();

    }
}

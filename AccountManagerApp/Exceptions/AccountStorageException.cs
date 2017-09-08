using System;

namespace AccountManagerApp
{
    /// <summary>
    /// アカウントのストレージへのアクセス失敗を表す例外。
    /// </summary>
    [Serializable]
    public class AccountStorageException : Exception
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public AccountStorageException()
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="message">例外の説明</param>
        public AccountStorageException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="message">例外の説明</param>
        /// <param name="innerException">元となった例外</param>
        public AccountStorageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

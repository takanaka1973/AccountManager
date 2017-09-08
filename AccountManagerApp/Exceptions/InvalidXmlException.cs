using System;

namespace AccountManagerApp
{
    /// <summary>
    /// XMLが不正であることを表す例外。
    /// </summary>
    [Serializable]
    public class InvalidXmlException : Exception
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public InvalidXmlException()
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="message">例外の説明</param>
        public InvalidXmlException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="message">例外の説明</param>
        /// <param name="innerException">元となった例外</param>
        public InvalidXmlException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

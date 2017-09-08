using System;

namespace AccountManagerApp
{
    /// <summary>
    /// 各種ユーティリティ。
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// 引数のnullチェックを行う。
        /// </summary>
        /// <param name="paramValue">チェック対象の引数の値</param>
        /// <param name="paramName">チェック対象の引数の名前</param>
        /// <exception cref="ArgumentNullException"><paramref name="paramValue"/>がnullの場合</exception>
        public static void RejectNull(object paramValue, string paramName)
        {
            if (paramValue == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

    }
}

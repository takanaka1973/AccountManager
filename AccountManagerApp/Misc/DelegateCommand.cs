using System;
using System.Windows.Input;

namespace AccountManagerApp
{
    /// <summary>
    /// デリゲートを用いて定義するコマンド。
    /// </summary>
    /// <remarks><see cref="ICommand"/>の実装を支援する。</remarks>
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> _canExecuteDelegate;
        private readonly Action<object> _executeDelegate;

        /// <summary>
        /// コマンドの実行可否が変化したことを通知するイベント。
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="canExecuteDelegate">
        /// コマンドの実行可否を返すデリゲート(nullを指定した場合は常に実行可能とみなす)
        /// </param>
        /// <param name="executeDelegate">
        /// コマンドを実行するデリゲート(null指定不可)
        /// </param>
        public DelegateCommand(Predicate<object> canExecuteDelegate, Action<object> executeDelegate)
        {
            Utilities.RejectNull(executeDelegate, nameof(executeDelegate));

            _canExecuteDelegate = canExecuteDelegate;
            _executeDelegate = executeDelegate;
        }

        /// <summary>
        /// <see cref="ICommand.CanExecute(object)"/>の実装。
        /// </summary>
        public bool CanExecute(object parameter)
        {
            if (_canExecuteDelegate == null)
            {
                return true;
            }

            return _canExecuteDelegate(parameter);
        }

        /// <summary>
        /// <see cref="ICommand.Execute(object)"/>の実装。
        /// </summary>
        public void Execute(object parameter)
        {
            // ここではコマンドの実行可否はチェックしない。
            _executeDelegate(parameter);
        }

        /// <summary>
        /// CanExecuteChangedイベントを発行する。
        /// </summary>
        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}

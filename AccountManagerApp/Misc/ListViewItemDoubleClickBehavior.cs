using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace AccountManagerApp
{
    /// <summary>
    /// リストビュー項目のダブルクリックを処理するビヘイビア。
    /// </summary>
    public class ListViewItemDoubleClickBehavior : Behavior<ListView>
    {
        /// <summary>
        /// このビヘイビアの依存関係プロパティ「Command」の定義。
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(ListViewItemDoubleClickBehavior), new PropertyMetadata(null));

        /// <summary>
        /// リストビュー項目がダブルクリックされた際に実行するコマンド。
        /// </summary>
        /// <remarks>
        /// コマンドのパラメータには、ダブルクリックされた項目のDataContextに設定されているオブジェクトを渡す。
        /// </remarks>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseDoubleClick += AssociatedObject_MouseDoubleClick;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseDoubleClick -= AssociatedObject_MouseDoubleClick;
            base.OnDetaching();
        }

        private void AssociatedObject_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListView listView = (ListView)sender;
            ListViewItem listViewItem = listView.ContainerFromElement((DependencyObject)e.OriginalSource) as ListViewItem;

            if (listViewItem == null)
            {
                return;
            }

            object targetObject = listViewItem.DataContext;

            if (targetObject == null)
            {
                return;
            }

            ICommand boundCommand = Command;

            if (boundCommand != null && boundCommand.CanExecute(targetObject))
            {
                boundCommand.Execute(targetObject);
                e.Handled = true;
            }
        }

    }
}

using System;
using System.ComponentModel;

namespace AccountManagerApp
{
    /// <summary>
    /// プロパティの変化を観察可能なオブジェクト。
    /// </summary>
    /// <remarks><see cref="INotifyPropertyChanged"/>の実装を支援する。</remarks>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// プロパティが変化したことを通知するイベント。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティが変化したことを通知する。
        /// </summary>
        /// <param name="propertyName">プロパティ名(null指定不可)</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            Utilities.RejectNull(propertyName, nameof(propertyName));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// nullを許容しないstring型のプロパティに値を設定する。
        /// </summary>
        /// <param name="property">設定対象のプロパティ</param>
        /// <param name="propertyName">プロパティ名(null指定不可)</param>
        /// <param name="newValue">プロパティの新しい値(null指定不可)</param>
        /// <remarks>プロパティの値が変化した場合はPropertyChangedイベントを発行する。</remarks>
        protected void SetNonNullStringProperty(ref string property, string propertyName, string newValue)
        {
            Utilities.RejectNull(propertyName, nameof(propertyName));

            if (newValue == null)
            {
                throw new ArgumentNullException(propertyName);
            }

            bool isPropertyChanged = (property != newValue) ? true : false;

            property = newValue;

            if (isPropertyChanged)
            {
                NotifyPropertyChanged(propertyName);
            }
        }

    }
}

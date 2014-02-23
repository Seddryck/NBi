using System.Collections.Generic;
using System.ComponentModel;

namespace NBi.UI.Genbi.Presenter
{
    /// <summary>
    /// Base abstract class for presenter implementations
    /// </summary>
    public abstract class PresenterBase : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> propertyValues = new Dictionary<string, object>();
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Gets the value of the property with the specified <paramref name="propertyName"/>.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The value of the property with the specified <paramref name="propertyName"/>.</returns>
        protected T GetValue<T>(string propertyName)
        {
            return this.propertyValues.ContainsKey(propertyName) ? (T)this.propertyValues[propertyName] : default(T);
        }

        /// <summary>
        /// Sets the value of the property with the specified <paramref name="propertyName"/>.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">The value of the property.</param>
        protected void SetValue<T>(string propertyName, T propertyValue)
        {
            if (this.propertyValues.ContainsKey(propertyName))
            {
                var oldValue = this.propertyValues[propertyName];
                if (!Equals(oldValue, propertyValue))
                {
                    this.propertyValues[propertyName] = propertyValue;
                    this.OnPropertyChanged(propertyName);
                }
            }
            else
            {
                this.propertyValues.Add(propertyName, propertyValue);
                this.OnPropertyChanged(propertyName);
            }
        }


        /// <summary>
        /// Called when the value of some property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

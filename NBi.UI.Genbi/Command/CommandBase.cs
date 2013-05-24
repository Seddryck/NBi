using System.ComponentModel;
using NBi.UI.Genbi.Command;

namespace Greg.XmlEditor.Presentation.Commands
{
    /// <summary>
    /// Base abstract class for command implementation
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        private bool isEnabled;

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public virtual string Name
        {
            get { return this.GetType().Name.Replace("Command", string.Empty); }
        }

        /// <summary>
        /// Gets a value indicating whether the command is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            protected set
            {
                this.isEnabled = value;
                this.OnEnabledChanged();
            }
        }

        /// <summary>
        /// Executes the command logics.
        /// </summary>
        public abstract void Invoke();

        /// <summary>
        /// Refreshes the command state.
        /// </summary>
        public abstract void Refresh();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Called when the IsEnabled property changes.
        /// </summary>
        protected void OnEnabledChanged()
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs("IsEnabled"));
        }
    }
}
using System.ComponentModel;
using System.Windows.Forms;

namespace NBi.UI.Genbi.Command
{
    /// <summary>
    /// Base abstract class for command implementation
    /// </summary>
    abstract class CommandBase : ICommand
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

        public void ShowException(string text)
        {
            MessageBox.Show(text, "Genbi", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        public void ShowInform(string text)
        {
            MessageBox.Show(text, "Genbi", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
    }
}
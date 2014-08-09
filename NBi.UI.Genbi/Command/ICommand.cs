using System;
using System.ComponentModel;
using System.Linq;

namespace NBi.UI.Genbi.Command
{
    interface ICommand : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the command is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Executes the command logics.
        /// </summary>
        void Invoke();

        /// <summary>
        /// Refreshes the command state.
        /// </summary>
        void Refresh();
    }
}

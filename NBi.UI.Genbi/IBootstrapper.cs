using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi
{

    /// <summary>
    /// Generic application starter interface.
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// Boots the application.
        /// </summary>
        /// <param name="args">
        /// Parameters for the application startup.
        /// </param>
        void Boot(params string[] args);
    }

}

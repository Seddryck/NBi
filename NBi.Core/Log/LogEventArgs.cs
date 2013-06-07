using System;
using System.Linq;

namespace NBi.Core.Log
{
    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(NBi.Core.ResultSet.ResultSet resultSet) : base()
        {
            this.ResultSet = resultSet;
        }
        


        public NBi.Core.ResultSet.ResultSet ResultSet { get; private set; }
    }
}

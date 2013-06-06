using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Log;

namespace NBi.Core.Query
{
    public abstract class AbstractQueryEngine : IQueryEnginable
     {
        private ICollection<ILogger> loggers;
        public IEnumerable<ILogger> Loggers { get { return loggers; } }

        public AbstractQueryEngine()
	    {
            loggers = new List<ILogger>();
	    }

        protected internal void AddLogs(ICollection<ILogger> loggers)
        {
            this.loggers = loggers;
        }
    }

}

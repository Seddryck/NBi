using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Log;

namespace NBi.Core.Query
{
	public abstract class AbstractQueryEngine : IQueryEnginable
	 {
		private readonly ICollection<ILogger> loggers;
		public IEnumerable<ILogger> Loggers { get { return loggers; } }

		public AbstractQueryEngine(ICollection<ILogger> loggers)
		{
			this.loggers = new List<ILogger>(loggers);
		}

		internal void SendToLog(Content content, string message)
		{
			var effectiveLogs = Loggers.Where(log => log.Content == content);
			foreach (var log in effectiveLogs)
				log.Write(message);
		}
	}

}

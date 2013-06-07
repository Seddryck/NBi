using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Log
{
    public class LoggerFactory
    {
        public virtual ILogger Instantiate(Content content, string fullPath, Condition condition)
        {
            ILogger log = null;
            switch (content)
            {
                case Content.ResultSet:
                    log = new ResultSetLogger(fullPath, condition);
                    break;
                case Content.Query:
                    break;
                case Content.Statistics:
                    break;
                default:
                    break;
            }

            if (log == null)
                throw new ArgumentException();

            return log;
        }
    }
}

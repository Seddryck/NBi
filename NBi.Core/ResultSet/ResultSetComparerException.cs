using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet
{
    public class ResultSetComparerException : ArgumentException
    {
        public ResultSetComparerException(string message) : base(message)
        {

        }
    }
}

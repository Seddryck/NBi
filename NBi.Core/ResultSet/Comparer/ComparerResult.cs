using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet.Comparer
{
    public class ComparerResult
    {
        public string Message { get; private set; }
        public bool AreEqual { get; private set; }

        private ComparerResult(bool result)
        {
            AreEqual = result;
        }

        public ComparerResult(string message)
        {
            AreEqual = false;
            Message = message;
        }

        public static ComparerResult Equality 
        { 
            get
            {
                return new ComparerResult(true);
            }
        }
    }
}

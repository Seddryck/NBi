using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Report
{
    public class ParserFactory
    {

        public static IParser GetParser(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return new FileParser();
            else
                return new DatabaseParser();

        }
    }
}

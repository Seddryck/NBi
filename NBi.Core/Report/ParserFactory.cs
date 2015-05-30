using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Report
{
    public class ParserFactory
    {

        public static IParser GetParser(string source, string path, string name, string dataset)
        {
            if (string.IsNullOrWhiteSpace(source))
                return new FileParser();
            else
                return new DatabaseParser();

        }

        public static IQueryRequest GetRequest(string source, string basePath, string path, string name, string dataset)
        {
            if (string.IsNullOrWhiteSpace(source))
                return new FileRequest(basePath + path, name, dataset);
            else
                return new DatabaseRequest(source, path, name, dataset);

        }
    }
}

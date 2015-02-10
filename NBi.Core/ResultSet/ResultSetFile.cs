using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    public class ResultSetFile
    {
        public string Path { get; private set; }
        public ResultSetFileType Type { get; private set; }

        public ResultSetFile(string path, ResultSetFileType type)
        {
            this.Path = path;
            this.Type = type;
        }
    }
}

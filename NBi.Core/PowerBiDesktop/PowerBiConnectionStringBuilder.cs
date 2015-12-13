using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.PowerBiDesktop
{
    class PowerBiConnectionStringBuilder
    {
        private bool isBuilt = false;
        private string connectionString;

        public void Build(string connectionString)
        {

            isBuilt = true;
        }

        public string ConnectionString
        {
            get
            {
                if (!isBuilt)
                    throw new InvalidOperationException();
                return connectionString;
            }
        }

        private string GetWorkspace(string cmdLine)
        {
            var rex = new System.Text.RegularExpressions.Regex("-s\\s\"(?<path>.*)\"");
            var m = rex.Matches(cmdLine);
            if (m.Count == 0)
                throw new ArgumentException();

            return m[0].Groups["path"].Captures[0].Value;
        }

        private int GetPort(string portFile)
        {
            if (!System.IO.File.Exists(portFile))
                throw new ArgumentException();

            string sPort = System.IO.File.ReadAllText(portFile, Encoding.Unicode);
            var port = int.Parse(sPort);
            return port;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NBi.Core.PowerBiDesktop
{
    public class PowerBiDesktopConnectionStringBuilder
    {
        private bool isBuilt = false;
        private string connectionString;

        public void Build(string mainWindowTitle)
        {
            connectionString = BuildLocalConnectionString(mainWindowTitle);
            isBuilt = true;
        }

        public string GetConnectionString()
        {
            if (!isBuilt)
                throw new InvalidOperationException();
            return connectionString;
        }

        protected virtual string BuildLocalConnectionString(string name)
        {
            var processes = System.Diagnostics.Process.GetProcessesByName("msmdsrv");
            var process = processes.FirstOrDefault(p => p.GetParent().MainWindowTitle == string.Format("{0} - Power BI Desktop", name));
            var cmdLine = process.GetCommandLine();
            var workspace = GetWorkspace(cmdLine);
            var portFile = string.Format("{0}\\msmdsrv.port.txt", workspace);
            var port = GetPort(portFile);

            return string.Format("Data Source=localhost:{0};", port);
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

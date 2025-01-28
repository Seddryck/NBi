using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NBi.Core.PowerBiDesktop;

public class PowerBiDesktopConnectionStringBuilder
{
    private bool isBuilt = false;
    private string connectionString = string.Empty;

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

    private static async Task<System.Diagnostics.Process[]> DelayUntilServerIsRunningAsync(int delay = 2000, int timeout = 10000)
    {
        var end = DateTime.Now.AddMilliseconds(timeout);
        var processes = System.Diagnostics.Process.GetProcessesByName("msmdsrv");
        while (!processes.Any() && DateTime.Now < end)
        {
            await Task.Delay(delay);
            processes = System.Diagnostics.Process.GetProcessesByName("msmdsrv");
        }
        return processes;
    }

    protected virtual string BuildLocalConnectionString(string name)
    {
        var task = Task.Run(async() => await DelayUntilServerIsRunningAsync());
        var processes = task.Result;
        if (!processes.Any())
        {
            throw new ConnectionException
                    (
                        new InvalidOperationException("No process found with the name 'msmdsrv'. Are you sure your Power BI desktop solution is running?")
                        , $"PBIX = {name}"
                    );
        }
            
        var process = processes.FirstOrDefault(p => p.GetParent().MainWindowTitle == $"{name} - Power BI Desktop");
        if (process==null)
        {
            var existingParentNameString = new StringBuilder();
            foreach (var p in processes)
		            existingParentNameString.Append($"'{p.GetParent().MainWindowTitle}', '");
	            existingParentNameString = existingParentNameString.Remove(existingParentNameString.Length - 2, 2);
            
            throw new ConnectionException
                (
                    new NullReferenceException($"No parent process found with the name '{name}'. Existing parent names were {existingParentNameString}.")
                    , $"PBIX = {name}"
                );
        }

        var cmdLine = process.GetCommandLine();
        var workspace = GetWorkspace(cmdLine);
        var port = GetPort($"{workspace}\\msmdsrv.port.txt");

        return $"Data Source=localhost:{port};";
    }


    private string GetWorkspace(string cmdLine)
    {
        var rex = new Regex("-s\\s\"(?<path>.*)\"");
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

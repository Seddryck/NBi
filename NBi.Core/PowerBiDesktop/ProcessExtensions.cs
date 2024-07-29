using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace NBi.Core;

public static class ProcessExtensions
{
    public static int GetParentId(this Process process)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException();

        if (process == null)
            throw new ArgumentNullException(nameof(process), "Process can't be null.");

        // query the management system objects
        string queryText = string.Format("select parentprocessid from win32_process where processid = {0}", process.Id);
        using (var searcher = new ManagementObjectSearcher(queryText))
        {
            foreach (var obj in searcher.Get())
            {
                object data = obj.Properties["parentprocessid"].Value;
                if (data != null)
                    return Convert.ToInt32(data);
            }
        }
        throw new Exception(string.Format("No parent found for process {0} ({1})", process.Id, process.ProcessName));
    }

    public static Process GetParent(this Process process)
    {
        var parentId = process.GetParentId();
        var parent = Process.GetProcessById(parentId);
        return parent;
    }

    public static string GetCommandLine(this Process process)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException();

        if (process == null)
            throw new ArgumentNullException(nameof(process), "Process can't be null.");

        var commandLine = new StringBuilder();

        using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
        {
            foreach (var @object in searcher.Get())
            {
                commandLine.Append(@object["CommandLine"]);
                commandLine.Append(' ');
            }
        }

        return commandLine.ToString();
    }
}
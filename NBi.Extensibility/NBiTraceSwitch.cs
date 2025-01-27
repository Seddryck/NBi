using System;
using System.Diagnostics;
using System.Linq;

namespace NBi.Extensibility;

public class NBiTraceSwitch
{
    private static volatile TraceSwitch? instance;
    private readonly static object syncRoot = new();

    private NBiTraceSwitch() { }

    private static TraceSwitch Instance
    {
        get
        {
            if (instance == null)
                lock (syncRoot)
                {
                    instance ??= new TraceSwitch("NBi", "NBi trace", "3");
                }

            return instance;
        }
    }

    public static TraceLevel Level
    {
        get => Instance.Level;
        set => Instance.Level = value;
    }

    public static bool TraceError => Instance.TraceError;

    public static bool TraceWarning => Instance.TraceWarning;

    public static bool TraceInfo => Instance.TraceInfo;

    public static bool TraceVerbose => Instance.TraceVerbose;
}

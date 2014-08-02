using System;
using System.Diagnostics;
using System.Linq;

namespace NBi.Core
{
    public class NBiTraceSwitch
    {
        private static volatile TraceSwitch instance;
        private readonly static object syncRoot = new Object();

        private NBiTraceSwitch() { }

        private static TraceSwitch Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new TraceSwitch("NBi", "NBi trace", "3");
                    }
                }

                return instance;
            }
        }

        public static TraceLevel Level
        {
            get
            {
                return Instance.Level;
            }
            set
            {
                Instance.Level = value;
            }
        }

        public static bool TraceError
        {
            get
            {
                return Instance.TraceError;
            }
        }

        public static bool TraceWarning
        {
            get
            {
                return Instance.TraceWarning;
            }
        }

        public static bool TraceInfo
        {
            get
            {
                return Instance.TraceInfo;
            }
        }

        public static bool TraceVerbose
        {
            get
            {
                return Instance.TraceVerbose;
            }
        }

    }
}

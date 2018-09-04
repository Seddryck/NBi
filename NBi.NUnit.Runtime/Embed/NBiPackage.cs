using NUnit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Embed
{

    [Serializable]
    class NBiPackage : TestPackage
    {
        public NBiPackage(string configFile)
            : this(string.Empty, configFile)
        { }

        public NBiPackage(string binPath, string configFile)
            : base($@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}{binPath}\NBi.NUnit.Runtime.dll")
        {
            ConfigurationFile = configFile;
        }
    }
}

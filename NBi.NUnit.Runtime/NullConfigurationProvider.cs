using NBi.NUnit.Runtime.Configuration;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;

namespace NBi.NUnit.Runtime
{
    public class NullConfigurationProvider : ConfigurationProvider
    {
        public override NBiSection GetSection() => new NBiSection();
    }
}

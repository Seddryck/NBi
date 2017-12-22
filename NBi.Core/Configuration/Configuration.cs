using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Configuration
{
    class Configuration : IConfiguration
    {
        public IReadOnlyDictionary<string, string> Providers { get; }
        public IReadOnlyCollection<Type> Extensions { get; }

        public Configuration(IReadOnlyDictionary<string, string> providers, IReadOnlyCollection<Type> extensions)
	    {
            Providers = providers;
            Extensions = extensions;
	    }
    }
}

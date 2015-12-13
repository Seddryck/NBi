using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Configuration
{
    class Configuration : IConfiguration
    {
        private readonly IReadOnlyDictionary<string, string> providers;
        public IReadOnlyDictionary<string, string> Providers
        {
            get { return providers;}
        }

        public Configuration(IReadOnlyDictionary<string, string> providers)
	    {
            this.providers = providers;
	    }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Configuration
{
    class Configuration : IProvidersConfiguration, IExtensionsConfiguration
    {
        public IReadOnlyDictionary<string, string> Providers { get; }
        public IReadOnlyCollection<Type> Extensions { get; }

        public Configuration(IReadOnlyDictionary<string, string> providers, IReadOnlyCollection<Type> extensions)
	    {
            Providers = providers;
            Extensions = extensions;
	    }

        public Configuration()
        {
            Providers = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            Extensions = new ReadOnlyCollection<Type>(new List<Type>());
        }
    }
}

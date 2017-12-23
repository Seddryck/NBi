using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Configuration
{
    public class ConfigurationManager
    {
        private static Configuration configuration = new Configuration(
            new Dictionary<string, string>()
            , new Collection<Type>()
            );

        public static void Initialize(Dictionary<string, string> providers, List<Type> extensions)
        {
            configuration  = new Configuration(providers, extensions);
        }

        internal static Configuration GetConfiguration()
        {
            return configuration;
        }
    }
}

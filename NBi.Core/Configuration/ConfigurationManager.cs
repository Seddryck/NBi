using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Configuration
{
    public class ConfigurationManager
    {
        private static Configuration configuration = new Configuration(new Dictionary<string, string>());

        public static void Initialize(Dictionary<string, string> providers)
        {
            configuration  = new Configuration(providers);
        }

        internal static IConfiguration GetConfiguration()
        {
            return configuration;
        }
    }
}

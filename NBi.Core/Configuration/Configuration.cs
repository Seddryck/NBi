using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Configuration
{
    public class Configuration : IConfiguration
    {
        public IReadOnlyDictionary<string, string>? Providers { get; private set; }
        public IReadOnlyDictionary<Type, IDictionary<string, string>>? Extensions { get; private set; }
        public IFailureReportProfile? FailureReportProfile { get; private set; }

        public Configuration()
        { }

        public void LoadProviders(Dictionary<string, string> providers)
        {
            Providers = new ReadOnlyDictionary<string, string>(providers);
        }

        public void LoadExtensions(IDictionary<Type, IDictionary<string, string>> extensions)
        {
            Extensions = new ReadOnlyDictionary<Type, IDictionary<string, string>>(extensions);
        }

        public void LoadFailureReportProfile(IFailureReportProfile profile)
        {
            FailureReportProfile = profile;
        }

        private static IConfiguration? @default;
        public static IConfiguration Default
        {
            get
            {
                @default ??= BuildDefaultConfiguration();
                return @default;
            }
        }

        protected static IConfiguration BuildDefaultConfiguration()
        {
            return new Configuration()
            {
                FailureReportProfile = FailureReport.FailureReportProfile.Default,
                Providers = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()),
                Extensions = new ReadOnlyDictionary<Type, IDictionary<string, string>>(new Dictionary<Type, IDictionary<string, string>>())
            };
        }
    }
}

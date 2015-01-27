using NBi.GenbiL.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful
{
    public class SettingsState
    {

        private const string DefaultSutName = "Default - System-under-test";
        private const string DefaultAssertName = "Default - Assert";
        private const string DefaultDecorationName = "Default - Setup/Cleanup";
        private const string DefaultEverywhereName = "Default - Everywhere";
        private const string ReferenceFormatName = "Reference - {0}";

        private readonly Dictionary<DefaultType, string> defaults;
        private readonly Dictionary<string, string> references;

        public IDictionary<DefaultType, string> Defaults
        {
            get
            {
                return defaults;
            }
        }
        public IDictionary<string, string> References
        {
            get
            {
                return references;
            }
        }

        public SettingsState()
        {
            defaults = new Dictionary<DefaultType, string>();
            references = new Dictionary<string, string>();
            defaults.Add(DefaultType.SystemUnderTest, "");
            defaults.Add(DefaultType.Assert, "");
            defaults.Add(DefaultType.SetupCleanup, "");
            defaults.Add(DefaultType.Everywhere, "");
            ParallelizeQueries = true;
        }

        public bool ParallelizeQueries { get; set; }
    }
}

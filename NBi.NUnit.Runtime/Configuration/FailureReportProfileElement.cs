using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Framework;
using NBi.Framework.FailureMessage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Configuration
{
    public class FailureReportProfileElement : ConfigurationElement, IFailureReportProfile
    {
        public FailureReportProfileElement()
        { }

        [ConfigurationProperty("max-sample-items", IsRequired = false, DefaultValue = 10)]
        public int MaxSampleItem
        {
            get => (int)this["max-sample-items"];
            set => this["max-sample-items"] = value;
        }

        [ConfigurationProperty("threshold-sample-items", IsRequired = false, DefaultValue = 15)]
        public int ThresholdSampleItem
        {
            get => (int)this["threshold-sample-items"];
            set => this["threshold-sample-items"] = value;
        }

        [ConfigurationProperty("expected-set", IsRequired = false, DefaultValue = FailureReportSetType.Sample)]
        public FailureReportSetType ExpectedSet
        {
            get => (FailureReportSetType)this["expected-set"];
            set => this["expected-set"] = value;
        }

        [ConfigurationProperty("actual-set", IsRequired = false, DefaultValue = FailureReportSetType.Sample)]
        public FailureReportSetType ActualSet
        {
            get => (FailureReportSetType)this["actual-set"];
            set => this["actual-set"] = value;
        }

        [ConfigurationProperty("analysis-set", IsRequired = false, DefaultValue = FailureReportSetType.Sample)]
        public FailureReportSetType AnalysisSet
        {
            get => (FailureReportSetType)this["analysis-set"];
            set => this["analysis-set"] = value;
        }

        [ConfigurationProperty("format", IsRequired = false, DefaultValue = FailureReportFormat.Markdown)]
        public FailureReportFormat Format
        {
            get => (FailureReportFormat)this["format"];
            set => this["format"] = value;
        }

        [ConfigurationProperty("mode", IsRequired = false, DefaultValue = FailureReportMode.OnFailure)]
        public FailureReportMode Mode
        {
            get => (FailureReportMode)this["mode"];
            set => this["mode"] = value;
        }
    }
}

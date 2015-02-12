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
        {
        }
            

        [ConfigurationProperty("max-sample-item", IsRequired = false, DefaultValue = 10)]
        public int MaxSampleItem
        {
            get
            {
                return (int)this["max-sample-item"];
            }
            set
            {
                this["max-sample-item"] = value;
            }
        }

        [ConfigurationProperty("threshold-sample-item", IsRequired = false, DefaultValue = 15)]
        public int ThresholdSampleItem
        {
            get
            {
                return (int)this["threshold-sample-item"];
            }
            set
            {
                this["threshold-sample-item"] = value;
            }
        }

        [ConfigurationProperty("expected-set", IsRequired = false, DefaultValue = FailureReportSetType.Sample)]
        public FailureReportSetType ExpectedSet
        {
            get
            {
                return (FailureReportSetType)this["expected-set"];
            }
            set
            {
                this["expected-set"] = value;
            }
        }

        [ConfigurationProperty("actual-set", IsRequired = false, DefaultValue = FailureReportSetType.Sample)]
        public FailureReportSetType ActualSet
        {
            get
            {
                return (FailureReportSetType)this["actual-set"];
            }
            set
            {
                this["actual-set"] = value;
            }
        }

        [ConfigurationProperty("sample-set", IsRequired = false, DefaultValue = FailureReportSetType.Sample)]
        public FailureReportSetType AnalysisSet
        {
            get
            {
                return (FailureReportSetType)this["sample-set"];
            }
            set
            {
                this["sample-set"] = value;
            }
        }
    }
}

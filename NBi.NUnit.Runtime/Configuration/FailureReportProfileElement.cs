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
            

        [ConfigurationProperty("maxSampleItem", IsRequired = false, DefaultValue = 10)]
        public int MaxSampleItem
        {
            get
            {
                return (int)this["maxSampleItem"];
            }
            set
            {
                this["maxSampleItem"] = value;
            }
        }

        [ConfigurationProperty("thresholdSampleItem", IsRequired = false, DefaultValue = 15)]
        public int ThresholdSampleItem
        {
            get
            {
                return (int)this["thresholdSampleItem"];
            }
            set
            {
                this["thresholdSampleItem"] = value;
            }
        }

        [ConfigurationProperty("expectedSet", IsRequired = false, DefaultValue = FailureReportSetType.Sample)]
        public FailureReportSetType ExpectedSet
        {
            get
            {
                return (SetType)this["expectedSet"];
            }
            set
            {
                this["expectedSet"] = value;
            }
        }

        [ConfigurationProperty("actualSet", IsRequired = false, DefaultValue = FailureReportSetType.Sample)]
        public FailureReportSetType ActualSet
        {
            get
            {
                return (SetType)this["actualSet"];
            }
            set
            {
                this["actualSet"] = value;
            }
        }

        [ConfigurationProperty("sampleSet", IsRequired = false, DefaultValue = FailureReportSetType.Sample)]
        public FailureReportSetType SampleSet
        {
            get
            {
                return (SetType)this["sampleSet"];
            }
            set
            {
                this["sampleSet"] = value;
            }
        }
    }
}

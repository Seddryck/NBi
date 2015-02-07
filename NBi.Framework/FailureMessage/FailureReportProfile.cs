using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage
{
    public class FailureReportProfile : IFailureReportProfile
    {
        protected FailureReportProfile ()
	    {

	    }

        private static IFailureReportProfile @default;
        public static IFailureReportProfile Default
        {
            get
            {
                if (@default == null)
                    @default = new FailureReportProfile()
                            {
                                MaxSampleItem = 10,
                                ThresholdSampleItem = 15,
                                ExpectedSet = FailureReportSetType.Sample,
                                ActualSet = FailureReportSetType.Sample,
                                AnalysisSet = FailureReportSetType.Sample
                            };
                return @default;
            }
        }

        public int MaxSampleItem {get; set;}
        
        public int ThresholdSampleItem {get; set;}
        
        public FailureReportSetType ExpectedSet {get; set;}
        
        public FailureReportSetType ActualSet {get; set;}
        
        public FailureReportSetType AnalysisSet {get; set;}
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage
{
    public class FailureReportProfile : IFailureReportProfile
    {
        protected internal FailureReportProfile ()
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

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is FailureReportProfile))
                return false;

            return (((FailureReportProfile)obj).ExpectedSet == this.ExpectedSet
                        && ((FailureReportProfile)obj).ActualSet == this.ActualSet
                        && ((FailureReportProfile)obj).AnalysisSet == this.AnalysisSet
                        && ((FailureReportProfile)obj).MaxSampleItem == this.MaxSampleItem
                        && ((FailureReportProfile)obj).ThresholdSampleItem == this.ThresholdSampleItem);
        }

        public override int GetHashCode()
        {
            return ExpectedSet.GetHashCode()^139 
                        * ActualSet.GetHashCode()^79 
                        * AnalysisSet.GetHashCode()^58 
                        * MaxSampleItem.GetHashCode() ^ 17 
                        * ThresholdSampleItem.GetHashCode();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Markdown
{
    public abstract class SampledFailureMessageMarkdown<T> : FailureMessageMarkdown
    {
        private readonly IFailureReportProfile profile;
        protected IFailureReportProfile Profile
        {
            get { return profile; }
        }

        public SampledFailureMessageMarkdown(IFailureReportProfile profile)
            : base()
        {
            this.profile = profile;
        }

        protected IEnumerable<T> Sample(IEnumerable<T> fullSet, FailureReportSetType sampling)
        {
            if (sampling == FailureReportSetType.Sample)
                return fullSet.Take(fullSet.Count() > Profile.ThresholdSampleItem ? Profile.MaxSampleItem : fullSet.Count());
            else
                return fullSet.ToArray();
        }

        protected bool IsSampled(IEnumerable<T> fullSet, FailureReportSetType sampling)
        {
            return sampling==FailureReportSetType.Sample && fullSet.Count() > Profile.ThresholdSampleItem;
        }

        protected int CountExcludedRows(IEnumerable<T> fullSet)
        {
            if (IsSampled(fullSet, FailureReportSetType.Sample))
                return fullSet.Count() - Profile.MaxSampleItem;
            else
                return 0;
        }

        public override string RenderExpected()
        {
            if (Profile.ExpectedSet != FailureReportSetType.None)
                return base.RenderExpected();
            else
                return "Display skipped.";
        }

        public override string RenderActual()
        {
            if (Profile.ActualSet != FailureReportSetType.None)
                return base.RenderActual();
            else
                return "Display skipped.";
        }

        public override string RenderAnalysis()
        {
            if (Profile.AnalysisSet != FailureReportSetType.None)
                return base.RenderAnalysis();
            else
                return "Display skipped.";
        }


    }
}

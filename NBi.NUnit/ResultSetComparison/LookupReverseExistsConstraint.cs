using NBi.Core;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Framework.FailureMessage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.ResultSetComparison
{
    public class LookupReverseExistsConstraint : LookupExistsConstraint
    {
        public LookupReverseExistsConstraint(IResultSetService reference)
            : base(reference)
        { }

        public new LookupReverseExistsConstraint Using(ColumnMappingCollection mappings)
            => base.Using(mappings) as LookupReverseExistsConstraint;

        public override bool ProcessParallel(IResultSetService actual)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, string.Format("Queries exectued in parallel."));

            Parallel.Invoke(
                () => { rsReference = actual.Execute(); },
                () => { rsCandidate = referenceService.Execute(); }
            );

            return Matches(rsReference);
        }

        protected override bool doMatch(ResultSet actual)
        {
            violations = Engine.Execute(rsCandidate, actual);
            var output = violations.Count() == 0;

            if (output && Configuration?.FailureReportProfile.Mode == FailureReportMode.Always)
                Assert.Pass(Failure.RenderMessage());

            return output;
        }
    }
}

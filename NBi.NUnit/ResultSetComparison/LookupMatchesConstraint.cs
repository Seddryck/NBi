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
    public class LookupMatchesConstraint : LookupExistsConstraint
    {
        protected internal override ILookupAnalyzer Engine
        {
            get => engine ?? (engine = new LookupMatchesAnalyzer(
                        keyMappings ?? ColumnMappingCollection.Default
                        , valueMappings ?? throw new ArgumentNullException()
                    ));
            set => engine = value ?? throw new ArgumentNullException();
        }

        protected override ILookupViolationsMessageFormatter BuildFailure()
        {
            var factory = new LookupViolationsMessageFormatterFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile);
            msg.Generate(rsReference.Rows.Cast<DataRow>(), rsCandidate.Rows.Cast<DataRow>(), violations, keyMappings, valueMappings);
            return msg;
        }

        public LookupMatchesConstraint(IResultSetService reference)
        : base(reference) { }

        private ColumnMappingCollection keyMappings;
        private ColumnMappingCollection valueMappings;
        public LookupExistsConstraint Using(ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings)
        {
            this.keyMappings = keyMappings;
            this.valueMappings = valueMappings;
            return this;
        }
    }
}

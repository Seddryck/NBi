using NBi.Core;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.Scalar.Comparer;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
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
                        , tolerances 
                    ));
            set => engine = value ?? throw new ArgumentNullException();
        }

        protected override ILookupViolationMessageFormatter BuildFailure()
        {
            var factory = new LookupMatchesViolationsMessageFormatterFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile);
            msg.Generate(rsReference.Rows, rsCandidate.Rows, violations, keyMappings, valueMappings);
            return msg;
        }

        public LookupMatchesConstraint(IResultSetResolver reference)
        : base(reference) { }

        private ColumnMappingCollection keyMappings;
        private ColumnMappingCollection valueMappings;
        private IDictionary<IColumnIdentifier, Tolerance> tolerances;
        public LookupExistsConstraint Using(ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings, IDictionary<IColumnIdentifier, Tolerance> tolerances)
        {
            this.keyMappings = keyMappings;
            this.valueMappings = valueMappings;
            this.tolerances = tolerances;
            return this;
        }
    }
}

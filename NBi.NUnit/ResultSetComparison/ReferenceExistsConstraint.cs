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
    public class ReferenceExistsConstraint : NBiConstraint
    {
        protected IResultSetService parentService;

        protected bool parallelizeQueries = false;

        protected ResultSet rsParent;
        protected ResultSet rsChild;
        private ReferenceViolations violations;

        private IReferenceViolationsMessageFormatter failure;
        protected IReferenceViolationsMessageFormatter Failure
        {
            get
            {
                if (failure == null)
                    failure = BuildFailure();
                return failure;
            }
        }

        protected virtual IReferenceViolationsMessageFormatter BuildFailure()
        {
            var factory = new ReferenceViolationsMessageFormatterFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile);
            msg.Generate(rsParent.Rows.Cast<DataRow>(), rsChild.Rows.Cast<DataRow>(), violations);
            return msg;
        }
        
        protected ReferenceAnalyzer engine;
        protected internal virtual ReferenceAnalyzer Engine
        {
            get
            {
                if (engine == null)
                    engine = new ReferenceAnalyzer(mappings ?? ColumnMappingCollection.Default);
                return engine;
            }
            set
            {
                engine = value ?? throw new ArgumentNullException();
            }
        }

        public ReferenceExistsConstraint(IResultSetService parent)
        {
            parentService = parent;
        }

        private ColumnMappingCollection mappings;
        public ReferenceExistsConstraint Using(ColumnMappingCollection mappings)
        {
            this.mappings = mappings;
            return this;
        }

        public override bool Matches(object actual)
        {
            if (actual is IResultSetService)
                return ProcessParallel((IResultSetService)actual);
            else if (actual is ResultSet)
                return doMatch((ResultSet)actual);
            else
                throw new ArgumentException($"The type of the actual object is '{actual.GetType().Name}' and is not supported for a constraint of type '{this.GetType().Name}'. Use a ResultSet or a ResultSetService.", nameof(actual));
        }

        public bool ProcessParallel(IResultSetService actual)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, string.Format("Queries exectued in parallel."));

            Parallel.Invoke(
                () => { rsChild = actual.Execute(); },
                () => { rsParent = parentService.Execute(); }
            );

            return Matches(rsChild);
        }

        protected bool doMatch(ResultSet actual)
        {
            violations = Engine.Execute(actual, rsParent);
            var output = violations.Count == 0;

            if (output && Configuration?.FailureReportProfile.Mode == FailureReportMode.Always)
                Assert.Pass(Failure.RenderMessage());

            return output;
        }

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;

            writer.WriteLine();
            writer.WriteLine(Failure.RenderExpected());
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;

            writer.WriteLine();
            writer.WriteLine(Failure.RenderActual());
        }

        public override void WriteMessageTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                writer.Write(Failure.RenderMessage());
            else
            {
                writer.WritePredicate(Failure.RenderPredicate());
                writer.WriteLine();
                writer.WriteLine();
                base.WriteMessageTo(writer);
                writer.WriteLine();
                writer.WriteLine(Failure.RenderAnalysis());
            }
        }

    }
}

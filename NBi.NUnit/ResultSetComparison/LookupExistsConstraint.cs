﻿using NBi.Core;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
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
    public class LookupExistsConstraint : NBiConstraint
    {
        protected IResultSetResolver referenceResolver;

        protected bool parallelizeQueries = false;

        protected IResultSet rsReference;
        protected IResultSet rsCandidate;
        protected LookupViolationCollection violations;

        private ILookupViolationMessageFormatter failure;
        protected ILookupViolationMessageFormatter Failure
        {
            get => failure ?? (failure = BuildFailure());
        }

        protected virtual ILookupViolationMessageFormatter BuildFailure()
        {
            var factory = new LookupExistsViolationsMessageFormatterFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile);
            msg.Generate(rsReference.Rows, rsCandidate.Rows, violations, mappings, null);
            return msg;
        }
        
        protected ILookupAnalyzer engine;
        protected internal virtual ILookupAnalyzer Engine
        {
            get => engine ?? (engine = new LookupExistsAnalyzer(mappings ?? ColumnMappingCollection.Default));
            set => engine = value ?? throw new ArgumentNullException();
        }

        public LookupExistsConstraint(IResultSetResolver reference)
            => referenceResolver = reference;

        protected ColumnMappingCollection mappings;
        public LookupExistsConstraint Using(ColumnMappingCollection mappings)
        {
            this.mappings = mappings;
            return this;
        }

        public override bool Matches(object actual)
        {
            if (actual is IResultSetResolver)
                return ProcessParallel((IResultSetResolver)actual);
            else if (actual is IResultSet)
                return doMatch((IResultSet)actual);
            else
                throw new ArgumentException($"The type of the actual object is '{actual.GetType().Name}' and is not supported for a constraint of type '{this.GetType().Name}'. Use a ResultSet or a ResultSetService.", nameof(actual));
        }

        public virtual bool ProcessParallel(IResultSetResolver actual)
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("Queries exectued in parallel."));

            Parallel.Invoke(
                () => { rsCandidate = actual.Execute(); },
                () => { rsReference = referenceResolver.Execute(); }
            );

            return Matches(rsCandidate);
        }

        protected virtual bool doMatch(IResultSet actual)
        {
            violations = Engine.Execute(actual, rsReference);
            var output = violations.Count() == 0;

            if (output && Configuration?.FailureReportProfile.Mode == FailureReportMode.Always)
                Assert.Pass(Failure.RenderMessage());

            return output;
        }

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;

            writer.WriteLine();
            writer.WriteLine(Failure.RenderReference());
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;

            writer.WriteLine();
            writer.WriteLine(Failure.RenderCandidate());
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

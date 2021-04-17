using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core.ResultSet;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework.FailureMessage;
using NBi.Core.ResultSet.Analyzer;
using NBi.Core.ResultSet.Equivalence;
using NUnit.Framework;
using NBi.Core.Configuration.FailureReport;
using NBi.Extensibility;

namespace NBi.NUnit.ResultSetComparison
{
    public abstract class BaseResultSetComparisonConstraint : NBiConstraint
    {
        protected IResultSetService expect;

        protected bool parallelizeQueries = false;

        protected IResultSet expectedResultSet;
        protected IResultSet actualResultSet;

        protected ResultResultSet result;
        private IDataRowsMessageFormatter failure;
        protected IDataRowsMessageFormatter Failure { get => failure ?? (failure = BuildFailure()); }
        
        protected virtual IDataRowsMessageFormatter BuildFailure()
        {
            var factory = new DataRowsMessageFormatterFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile, Engine.Style);
            msg.BuildComparaison(expectedResultSet.Rows.Cast<DataRow>(), actualResultSet.Rows.Cast<DataRow>(), result);
            return msg;
        }
     
        /// <summary>
        /// Engine dedicated to ResultSet comparaison
        /// </summary>
        protected IEquivaler engine;
        protected internal virtual IEquivaler Engine
        {
            get => engine ?? (engine = new OrdinalEquivaler(AnalyzersFactory.EqualTo(), null));
            set => engine = value ?? throw new ArgumentNullException();
        }
        
        public BaseResultSetComparisonConstraint(IResultSetService value)
        {
            this.expect = value;
        }

        
        public BaseResultSetComparisonConstraint Using(IEquivaler engine)
        {
            this.Engine = engine;
            return this;
        }

        public BaseResultSetComparisonConstraint Using(ISettingsResultSet settings)
        {
            this.Engine.Settings = settings;
            return this;
        }
        
        public BaseResultSetComparisonConstraint Parallel()
        {
            this.parallelizeQueries = true;
            return this;
        }

        public BaseResultSetComparisonConstraint Sequential()
        {
            this.parallelizeQueries = false;
            return this;
        }
        
        /// <summary>
        /// Handle an IDbCommand and compare it to a predefined resultset
        /// </summary>
        /// <param name="actual">An IResultSetService or ResultSet</param>
        /// <returns>true, if the execution of the actual IResultSetService returns a ResultSet identical to the content of the expected ResultSet</returns>
        public override bool Matches(object actual)
        {
            switch (actual)
            {
                case IResultSetService rss: return Process(rss);
                case IResultSet rs: return doMatch(rs);
                default: throw new ArgumentException($"The type of the actual object is '{actual.GetType().Name}' and is not supported for a constraint of type '{this.GetType().Name}'. Use a ResultSet or a ResultSetService.", nameof(actual));
            }
        }

        protected bool doMatch(IResultSet actual)
        {
            actualResultSet = actual;

            //This is needed if we don't use //ism
            expectedResultSet = expectedResultSet ?? expect.Execute();

            result = Engine.Compare(actualResultSet, expectedResultSet);
            var output = result.Difference == ResultSetDifferenceType.None;

            if (output && Configuration?.FailureReportProfile.Mode==FailureReportMode.Always)
                Assert.Pass(Failure.RenderMessage());

            return output;
        }

        /// <summary>
        /// Handle an IDbCommand (Query and ConnectionString) and check it with the expectation (Another IDbCommand or a ResultSet)
        /// </summary>
        /// <param name="actual">IDbCommand</param>
        /// <returns></returns>
        public bool Process(IResultSetService actual)
        {
            IResultSet rsActual = null;
            if (parallelizeQueries)
                rsActual = ProcessParallel(actual);
            else
                rsActual = actual.Execute();
            
            return this.Matches(rsActual);
        }

        public IResultSet ProcessParallel(IResultSetService actual)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, string.Format("Queries exectued in parallel."));
            
            IResultSet rsActual = null;
            System.Threading.Tasks.Parallel.Invoke(
                () => {
                        rsActual = actual.Execute();
                      },
                () => {
                        expectedResultSet = expect.Execute();
                }
            );
            
            return rsActual;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
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
                writer.WritePredicate("Execution of the query doesn't match the expected result");
                writer.WriteLine();
                writer.WriteLine();
                base.WriteMessageTo(writer);
                writer.WriteLine();
                writer.WriteLine(Failure.RenderAnalysis());
            }
        }
        
        internal bool IsParallelizeQueries()
        {
            return parallelizeQueries;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework.FailureMessage;
using NBi.Framework;
using NBi.Core.Xml;
using NBi.Core.Transformation;
using NBi.Core.ResultSet.Analyzer;
using NBi.Core.ResultSet.Loading;

namespace NBi.NUnit.ResultSetComparison
{
    public abstract class BaseResultSetComparisonConstraint : NBiConstraint
    {
        protected IResultSetService expect;

        protected bool parallelizeQueries = false;
        protected CsvProfile csvProfile;

        protected ResultSet expectedResultSet;
        protected ResultSet actualResultSet;

        protected ResultSetCompareResult result;
        private DataRowsMessage failure;
        protected DataRowsMessage Failure
        {
            get
            {
                if (failure == null)
                    failure = BuildFailure();
                return failure;
            }
        }

        protected virtual DataRowsMessage BuildFailure()
        {
            var msg = new DataRowsMessage(Engine.Style, Configuration.FailureReportProfile);
            msg.BuildComparaison(expectedResultSet.Rows.Cast<DataRow>(), actualResultSet.Rows.Cast<DataRow>(), result);
            return msg;
        }
     
        /// <summary>
        /// Engine dedicated to ResultSet comparaison
        /// </summary>
        protected IResultSetComparer _engine;
        protected internal virtual IResultSetComparer Engine
        {
            get
            {
                if(_engine==null)
                    _engine = new ResultSetComparerByIndex(AnalyzersFactory.EqualTo(), null);
                return _engine;
            }
            set
            {
                if(value==null)
                    throw new ArgumentNullException();
                _engine = value;
            }
        }

        public TransformationProvider TransformationProvider { get; protected set; }

                
        
        public BaseResultSetComparisonConstraint(IResultSetService value)
        {
            this.expect = value;
        }

        
        public BaseResultSetComparisonConstraint Using(IResultSetComparer engine)
        {
            this.Engine = engine;
            return this;
        }

        public BaseResultSetComparisonConstraint Using(ISettingsResultSetComparison settings)
        {
            this.Engine.Settings = settings;
            return this;
        }

        public BaseResultSetComparisonConstraint Using(TransformationProvider transformationProvider)
        {
            this.TransformationProvider = transformationProvider;
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

        public BaseResultSetComparisonConstraint CsvProfile(CsvProfile profile)
        {
            this.csvProfile = profile;
            return this;
        }


        /// <summary>
        /// Handle an IDbCommand and compare it to a predefined resultset
        /// </summary>
        /// <param name="actual">An IResultSetService or ResultSet</param>
        /// <returns>true, if the execution of the actual IResultSetService returns a ResultSet identical to the content of the expected ResultSet</returns>
        public override bool Matches(object actual)
        {
            if (actual is IResultSetService)
                return Process((IResultSetService)actual);
            else if (actual is ResultSet)
                return doMatch((ResultSet)actual);
            else
                throw new ArgumentException($"The type of the actual object is '{actual.GetType().Name}' and is not supported for a constraint of type '{this.GetType().Name}'. Use a ResultSet or a ResultSetService.", nameof(actual));
        }

        protected bool doMatch(ResultSet actual)
        {
            actualResultSet = actual;

            //This is needed if we don't use //ism
            if (expectedResultSet ==  null)
                expectedResultSet = expect.Execute();

            if (TransformationProvider != null)
                TransformationProvider.Transform(expectedResultSet);

            result = Engine.Compare(actualResultSet, expectedResultSet);

            return result.Difference == ResultSetDifferenceType.None;
        }

        /// <summary>
        /// Handle an IDbCommand (Query and ConnectionString) and check it with the expectation (Another IDbCommand or a ResultSet)
        /// </summary>
        /// <param name="actual">IDbCommand</param>
        /// <returns></returns>
        public bool Process(IResultSetService actual)
        {
            ResultSet rsActual = null;
            if (parallelizeQueries)
            {
                rsActual = ProcessParallel(actual);
            }
            else
                rsActual = actual.Execute();
            
            return this.Matches(rsActual);
        }

        public ResultSet ProcessParallel(IResultSetService actual)
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("Queries exectued in parallel."));
            
            ResultSet rsActual = null;
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
            writer.WriteLine();
            writer.WriteLine(Failure.RenderExpected());
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine(Failure.RenderActual());
        }

        public override void WriteMessageTo(NUnitCtr.MessageWriter writer)
        {
            writer.WritePredicate("Execution of the query doesn't match the expected result");
            writer.WriteLine();
            writer.WriteLine();
            base.WriteMessageTo(writer);
            writer.WriteLine();
            writer.WriteLine(Failure.RenderCompared());
        }
        
        internal bool IsParallelizeQueries()
        {
            return parallelizeQueries;
        }
    }
}

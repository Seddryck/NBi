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

namespace NBi.NUnit.Query
{
    public class EqualToConstraint : NBiConstraint
    {
        
        
        protected Object expect;

        protected bool parallelizeQueries = false;
        protected CsvProfile csvProfile;

        protected ResultSet expectedResultSet;
        protected ResultSet actualResultSet;

        [Flags]
        public enum PersistanceItems
        {
            actual =1,
            expected =2
        }
        protected PersistanceItems persistanceItems;
        protected PersistanceChoice persistanceChoice;
        protected string filename;

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

        protected DataRowsMessage BuildFailure()
        {
            var msg = new DataRowsMessage(Engine.Style, Configuration.FailureReportProfile);
            msg.BuildComparaison(expectedResultSet.Rows.Cast<DataRow>(), actualResultSet.Rows.Cast<DataRow>(), result);
            return msg;
        }
     
        /// <summary>
        /// Engine dedicated to ResultSet comparaison
        /// </summary>
        protected IResultSetComparer _engine;
        protected internal IResultSetComparer Engine
        {
            get
            {
                if(_engine==null)
                    _engine = new ResultSetComparerByIndex(null);
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

        /// <summary>
        /// Engine dedicated to ResultSet acquisition
        /// </summary>
        protected IResultSetBuilder _resultSetBuilder;
        protected internal IResultSetBuilder ResultSetBuilder
        {
            get
            {
                if(_resultSetBuilder==null)
                {
                    if (csvProfile==null)
                        _resultSetBuilder = new ResultSetBuilder();
                    else
                        _resultSetBuilder = new ResultSetBuilder(csvProfile);
                }
                    
                return _resultSetBuilder;
            }
            set
            {
                if(value==null)
                    throw new ArgumentNullException();
                _resultSetBuilder = value;
            }
        }
        
        public EqualToConstraint (string value)
        {
            this.expect = value;
        }

        public EqualToConstraint (ResultSet value)
        {
            this.expect = value;
        }

        public EqualToConstraint(IContent value)
        {
            this.expect = value;
        }

        public EqualToConstraint (IDbCommand value)
        {
            this.expect = value;
        }

        public EqualToConstraint(XPathEngine xpath)
        {
            this.expect = xpath;
        }

        public EqualToConstraint Using(IResultSetComparer engine)
        {
            this.Engine = engine;
            return this;
        }

        public EqualToConstraint Using(ISettingsResultSetComparison settings)
        {
            this.Engine.Settings = settings;
            return this;
        }

        public EqualToConstraint Using(TransformationProvider transformationProvider)
        {
            this.TransformationProvider = transformationProvider;
            return this;
        }

        public EqualToConstraint Persist(PersistanceChoice choice, PersistanceItems items, string filename)
        {
            this.persistanceChoice = choice;
            this.filename=filename;
            this.persistanceItems = items;
            return this;
        }

        public EqualToConstraint Parallel()
        {
            this.parallelizeQueries = true;
            return this;
        }

        public EqualToConstraint Sequential()
        {
            this.parallelizeQueries = false;
            return this;
        }

        public EqualToConstraint CsvProfile(CsvProfile profile)
        {
            this.csvProfile = profile;
            return this;
        }


        /// <summary>
        /// Handle an IDbCommand and compare it to a predefined resultset
        /// </summary>
        /// <param name="actual">An OleDbCommand, SqlCommand or AdomdCommand</param>
        /// <returns>true, if the result of query execution is exactly identical to the content of the resultset</returns>
        public override bool Matches(object actual)
        {
            if (actual is IDbCommand)
                return Process((IDbCommand)actual);
            else if (actual is ResultSet)
                return doMatch((ResultSet)actual);
            else
                return false;

        }

        protected bool doMatch(ResultSet actual)
        {
            actualResultSet = actual;

            //This is needed if we don't use //ism
            if (expectedResultSet ==  null)
                expectedResultSet = GetResultSet(expect);

            if (TransformationProvider != null)
                TransformationProvider.Transform(expectedResultSet);

            result = Engine.Compare(actualResultSet, expectedResultSet);

            //  Math.Min for result.Difference limits the value to 1 if we've two non matching resultsets
            if ((int)persistanceChoice + Math.Min(1,(int)result.Difference) > 1)
            {
                if ((persistanceItems & PersistanceItems.expected) == PersistanceItems.expected)
                    doPersist(expectedResultSet, GetPersistancePath("Expect"));
                if ((persistanceItems & PersistanceItems.actual) == PersistanceItems.actual)
                    doPersist(actualResultSet, GetPersistancePath("Actual"));
            }

            return result.Difference == ResultSetDifferenceType.None;
        }

        protected string GetPersistancePath(string folder)
        {
            return string.Format(@"{0}\{1}", folder, filename);
        }
        /// <summary>
        /// Handle an IDbCommand (Query and ConnectionString) and check it with the expectation (Another IDbCommand or a ResultSet)
        /// </summary>
        /// <param name="actual">IDbCommand</param>
        /// <returns></returns>
        public bool Process(IDbCommand actual)
        {
            ResultSet rsActual = null;
            if (parallelizeQueries)
            {
                rsActual = ProcessParallel(actual);
            }
            else
                rsActual = GetResultSet(actual);
            
            return this.Matches(rsActual);
        }

        public ResultSet ProcessParallel(IDbCommand actual)
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("Queries exectued in parallel."));
            
            ResultSet rsActual = null;
            System.Threading.Tasks.Parallel.Invoke(
                () => {
                        rsActual = GetResultSet(actual);
                      },
                () => {
                        expectedResultSet = GetResultSet(expect);
                      }
            );
            
            return rsActual;
        }

        protected ResultSet GetResultSet(Object obj)
        {
            if (obj is XPathEngine)
            {
                var xpath = obj as XPathEngine;
                var rs = xpath.Execute();
                return rs;
            }
            else
                return ResultSetBuilder.Build(obj);
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

        private void doPersist(ResultSet resultSet, string path)
        {
            var writer = new ResultSetCsvWriter(System.IO.Path.GetDirectoryName(path));
            writer.Write(System.IO.Path.GetFileName(path), resultSet);
        }

        internal bool IsParallelizeQueries()
        {
            return parallelizeQueries;
        }
    }
}

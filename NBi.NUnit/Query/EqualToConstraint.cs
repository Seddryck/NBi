using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Query
{
    public class EqualToConstraint : NUnitCtr.Constraint
    {
        
        
        protected Object expect;

        protected bool parallelQueryExecution = false;

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
     
        /// <summary>
        /// Engine dedicated to ResultSet comparaison
        /// </summary>
        protected IResultSetComparer _engine;
        protected internal IResultSetComparer Engine
        {
            get
            {
                if(_engine==null)
                    _engine = new DataRowBasedResultSetComparer();
                return _engine;
            }
            set
            {
                if(value==null)
                    throw new ArgumentNullException();
                _engine = value;
            }
        }
        
        /// <summary>
        /// Engine dedicated to ResultSet acquisition
        /// </summary>
        protected IResultSetBuilder _resultSetBuilder;
        protected internal IResultSetBuilder ResultSetBuilder
        {
            get
            {
                if(_resultSetBuilder==null)
                    _resultSetBuilder = new ResultSetBuilder();
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

        public EqualToConstraint(IEnumerable<IRow> value)
        {
            this.expect = value;
        }

        public EqualToConstraint (IDbCommand value)
        {
            this.expect = value;
        }

        public EqualToConstraint Using(ResultSetComparisonSettings settings)
        {
            this.Engine.Settings = settings;
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
            this.parallelQueryExecution = true;
            return this;
        }

        public EqualToConstraint Sequential()
        {
            this.parallelQueryExecution = false;
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
            if (parallelQueryExecution)
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
            return ResultSetBuilder.Build(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            FormatResultSet(writer, expectedResultSet);
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            FormatResultSet(writer, actualResultSet);
        }

        public override void WriteMessageTo(NUnitCtr.MessageWriter writer)
        {
            writer.WritePredicate("Execution of the query doesn't match the expected result");
            writer.WriteLine();
            writer.WriteLine();
            base.WriteMessageTo(writer);
            DisplayDifferences(writer, result);
        }

        protected void DisplayDifferences(NUnitCtr.MessageWriter writer, ResultSetCompareResult compareResult)
        {
            if (compareResult.Unexpected.Count > 0)
            {
                writer.WriteLine("  Unexpected rows:");
                writer.WriteLine();
                FormatResultSet(writer, compareResult.Unexpected, false);
                writer.WriteLine();
            }

            if (compareResult.Missing.Count > 0)
            {
                writer.WriteLine("  Missing rows:");
                writer.WriteLine();
                FormatResultSet(writer, compareResult.Missing, false);
                writer.WriteLine();
            }

            if (compareResult.Duplicated.Count > 0)
            {
                writer.WriteLine("  Duplicated rows:");
                writer.WriteLine();
                FormatResultSet(writer, compareResult.Duplicated, true);
                writer.WriteLine();
            }

            if (compareResult.NonMatchingValue.Count > 0)
            {
                writer.WriteLine("  Non matching value rows:");
                writer.WriteLine();
                FormatResultSet(writer, compareResult.NonMatchingValue, true);
                writer.WriteLine();
            }

        }
       

        

        protected virtual void FormatResultSet(NUnitCtr.MessageWriter writer, ResultSetCompareResult.Sample sample, bool compare)
        {
            var textCreator = new ResultSetTextWriter();
            var output = textCreator.BuildContent(sample.Rows, sample.Count, compare);
            foreach (var line in output)
                writer.WriteLine(line);                
        }


        

        protected virtual ColumnRole GetColumnRole(IEnumerable<DataRow> rows, int columnIndex)
        {
            return (ColumnRole)rows.ElementAt(0).Table.Columns[columnIndex].ExtendedProperties["NBi::Role"];
        }

        protected virtual void FormatResultSet(NUnitCtr.MessageWriter writer, ResultSet resultSet)
        {
            var rows = resultSet.Rows.Cast<DataRow>().ToList();
            var textCreator = new ResultSetTextWriter();
            var output = textCreator.BuildContent(rows, rows.Count(), false);
            foreach (var line in output)
                writer.WriteLine(line);
        }

        private void doPersist(ResultSet resultSet, string path)
        {
            var writer = new ResultSetCsvWriter(System.IO.Path.GetDirectoryName(path));
            writer.Write(System.IO.Path.GetFileName(path), resultSet);
        }
    }
}

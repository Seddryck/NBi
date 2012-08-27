using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using NBi.Core.ResultSet;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class EqualToConstraint : NUnitCtr.Constraint
    {
        protected Object expect;
        protected string persistenceExpectedResultSetFullPath;
        protected string persistenceActualResultSetFullPath;

        protected ResultSet expectedResultSet;
        protected ResultSet actualResultSet;

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

        public EqualToConstraint PersistExpectation(string path, string filename)
        {
            this.persistenceExpectedResultSetFullPath = Path.Combine(path, filename);
            return this;
        }


        public EqualToConstraint PersistActual(string path, string filename)
        {
            this.persistenceActualResultSetFullPath = Path.Combine(path, filename);
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
            //Persist actual (if needed)
            if (!string.IsNullOrEmpty(persistenceActualResultSetFullPath))
                doPersist(actualResultSet, persistenceActualResultSetFullPath);
            
            expectedResultSet = GetResultSet(expect);
            //Persist expected (if requested)
            if (!string.IsNullOrEmpty(persistenceExpectedResultSetFullPath))
                doPersist(expectedResultSet, persistenceExpectedResultSetFullPath);

            result = Engine.Compare(actualResultSet, expectedResultSet);

            return result.Difference == ResultSetDifferenceType.None;
        }

        /// <summary>
        /// Handle an IDbCommand (Query and ConnectionString) and check it with the expectation (Another IDbCommand or a ResultSet)
        /// </summary>
        /// <param name="actual">IDbCommand</param>
        /// <returns></returns>
        public bool Process(IDbCommand actual)
        {
            var rsActual = GetResultSet(actual);
            return this.Matches(rsActual);
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

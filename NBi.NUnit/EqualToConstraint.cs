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

        public EqualToConstraint Using(ResultSetComparaisonSettings settings)
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
            writer.WriteExpectedValue(FormatResultSet(expectedResultSet));
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            writer.WriteActualValue(FormatResultSet(actualResultSet));
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
                writer.WriteLine(FormatResultSet(compareResult.Unexpected));
                writer.WriteLine();
            }

            if (compareResult.Missing.Count > 0)
            {
                writer.WriteLine("  Missing rows:");
                writer.WriteLine();
                writer.WriteLine(FormatResultSet(compareResult.Missing));
                writer.WriteLine();
            }

            if (compareResult.NonMatchingValue.Count > 0)
            {
                writer.WriteLine("  Non matching value rows:");
                writer.WriteLine();
                writer.WriteLine(FormatResultSet(compareResult.NonMatchingValue));
                writer.WriteLine();
            }
        }
       

        protected const int MAX_ROWS_DISPLAYED = 10;

        protected virtual string FormatResultSet(ResultSetCompareResult.Sample sample)
        {
            if (sample.References == null)
                return FormatResultSet(sample.Rows, sample.Count);
            else
                return FormatResultSetComparaison(sample.Rows, sample.References, sample.Count);
        }

        protected virtual string FormatResultSet(IEnumerable<DataRow> rows, int rowCount)
        {
            var sb = new System.Text.StringBuilder();

            //calculate row count to diplay
            int maxRows = (rowCount <= MAX_ROWS_DISPLAYED) ? rowCount : MAX_ROWS_DISPLAYED;
            var subsetRows = rows.Take(maxRows);

            IList<int> fieldLength = GetFieldsLength(subsetRows);

            var sbTable = new System.Text.StringBuilder();

            int maxRowLength = 0;
            for (int i = 0; i < subsetRows.Count(); i++)
            {
                var sbRow = new System.Text.StringBuilder();
                sbRow.Append(' ', 4);
                for (int j = 0; j < subsetRows.ElementAt(i).ItemArray.Count(); j++)
			    {
                    var cell = subsetRows.ElementAt(i).ItemArray[j];
                    sbRow.AppendFormat("| {0}", cell.ToString());
                    sbRow.Append(' ', fieldLength[j]-cell.ToString().Length);
			    }
                sbRow.AppendLine("|");
                if (sbRow.Length > maxRowLength)
                    maxRowLength = sbRow.Length;
                sbTable.Append(sbRow);
            }
            //information about #Rows and #Cols
            sb.AppendFormat("ResultSet with {0} row", rowCount);
            if (rowCount > 1)
                sb.Append('s');               
            sb.AppendLine();
            sb.AppendLine();

            //header
            sb.Append(' ', 4);
            sb.Append('-', fieldLength.Sum() + fieldLength.Count * 2 + 1);
            sb.AppendLine();

            //Table
            sb.Append(sbTable);

            //footer
            sb.Append(' ', 4);
            sb.Append('-', fieldLength.Sum() + fieldLength.Count * 2 + 1);
            sb.AppendLine();

            //If needed display # skipped rows
            if (rowCount > MAX_ROWS_DISPLAYED)
            {
                sb.Append(' ', 4);
                sb.Append(new string('.', 3));
                sb.Append(new string(' ', 3));
                sb.AppendFormat("{0} (of {1}) rows skipped for display purpose", rowCount - MAX_ROWS_DISPLAYED, rowCount);
                sb.Append(new string(' ', 3));
                sb.Append(new string('.', 3));
                sb.AppendLine();
            }

            sb.AppendLine();

            return sb.ToString();
        }

        //protected virtual IList<int> GetFieldsLength(IEnumerable<DataRow> rows)
        //{
        //    var fieldsLength = new List<int>();
            
        //    foreach (var r in rows)
        //    {
        //        if (fieldsLength.Count==0)
        //            foreach (object cell in r.ItemArray)
        //                fieldsLength.Add(Math.Max(10, cell.ToString().Length));
        //        else
        //        {
        //            for (int i = 0; i < r.ItemArray.Count(); i++)
        //                fieldsLength[i]= Math.Max(fieldsLength[i], r.ItemArray[i].ToString().Length);
        //        }
        //    }

        //    return fieldsLength;
        //}

        protected virtual IList<int> GetFieldsLength(IEnumerable<DataRow> rows)
        {
            var fieldsLength = new List<int>();

            for (int i = 0; i < rows.ElementAt(0).ItemArray.Count(); i++)
			{
			    var cells = new List<EqualToConstraintDisplay>();
                //Header
                var displayHeader = EqualToConstraintDisplay.BuildHeader(rows.ElementAt(0).Table, i);
                cells.Add(displayHeader);

                //For each row
                foreach (var r in rows)
                {
                    var display = EqualToConstraintDisplay.Build(r, i);
                    cells.Add(display);
                }
                fieldsLength.Add(cells.Max(f => f.GetCellLength()));
            }

            return fieldsLength;
        }

        protected virtual ColumnRole GetColumnRole(IEnumerable<DataRow> rows, int columnIndex)
        {
            return (ColumnRole)rows.ElementAt(0).Table.Columns[columnIndex].ExtendedProperties["NBi::Role"];
        }

        protected virtual string FormatResultSetComparaison(IEnumerable<DataRow> rows, IEnumerable<DataRow> refs, int rowCount)
        {
            var sb = new System.Text.StringBuilder();

            //calculate row count to diplay
            int maxRows = (rowCount <= MAX_ROWS_DISPLAYED) ? rowCount : MAX_ROWS_DISPLAYED;
            var subsetRows = rows.Take(maxRows);
            IList<int> fieldLength = GetFieldsLength(subsetRows);

            //information about #Rows and #Cols
            sb.AppendFormat("ResultSet with {0} row", rowCount);
            if (rowCount > 1)
                sb.Append('s');
            sb.AppendLine();

            //header
            sb.Append(' ', 4);
            sb.Append('-', fieldLength.Sum() + fieldLength.Count + 1); //Cells'length + "|" for each cell + the ending "|"
            sb.AppendLine();
            for (int i = 0; i < fieldLength.Count; i++)
            {
                var displayHeader = EqualToConstraintDisplay.BuildHeader(rows.ElementAt(0).Table, i);
                sb.AppendFormat("|{0}", displayHeader.GetText(fieldLength[i]));
            }
            sb.AppendLine("|");

            //Content
            for (int i = 0; i < maxRows; i++)
            {
                var sbRow = new System.Text.StringBuilder();

                sbRow.Append(' ', 4);
                for (int j = 0; j < subsetRows.ElementAt(i).ItemArray.Count(); j++)
                {
                    var display = EqualToConstraintDisplay.Build(rows.ElementAt(i), j);
                    sb.AppendFormat("|{0}", display.GetText(fieldLength[j]));
                }
                sbRow.AppendLine("|");
                sb.Append(sbRow);
            }
            //footer
            sb.Append(' ', 4);
            sb.Append('-', fieldLength.Sum() + fieldLength.Count + 1); //Cells'length + "|" for each cell + the ending "|"
            sb.AppendLine();

            //If needed display # skipped rows
            if (rowCount > MAX_ROWS_DISPLAYED)
            {
                sb.Append(new string('.', 3));
                sb.Append(new string(' ', 3));
                sb.AppendFormat("{0} (of {1}) rows skipped for display purpose", rowCount - MAX_ROWS_DISPLAYED, rowCount);
                sb.Append(new string(' ', 3));
                sb.Append(new string('.', 3));
                sb.AppendLine();
            }

            sb.AppendLine();

            return sb.ToString();
        }

        //protected virtual string FormatResultSetComparaison(IEnumerable<DataRow> rows, IEnumerable<DataRow> refs, int rowCount)
        //{
        //    var sb = new System.Text.StringBuilder();

        //    //calculate row count to diplay
        //    int maxRows = (rowCount <= MAX_ROWS_DISPLAYED) ? rowCount : MAX_ROWS_DISPLAYED;
        //    var subsetRows = rows.Take(maxRows);
        //    IList<int> fieldLength = GetFieldsLength(subsetRows);

        //    var sbTable = new System.Text.StringBuilder();

        //    int maxRowLength = 0;
        //    for (int i = 0; i < maxRows; i++)
        //    {
        //        var sbRow = new System.Text.StringBuilder();

        //        sbRow.Append(' ', 4);
        //        for (int j = 0; j < subsetRows.ElementAt(i).ItemArray.Count(); j++)
        //        {
        //            //Key Comparaison
        //            if (GetColumnRole(subsetRows, j) == ColumnRole.Key)
        //            {
        //                sbRow.AppendFormat("| {0}", subsetRows.ElementAt(i).ItemArray[j].ToString());
        //                sbRow.Append(' ', fieldLength[j] - subsetRows.ElementAt(i).ItemArray[j].ToString().Length);
        //            }
        //            //Value comparaison
        //            else if (GetColumnRole(subsetRows, j) == ColumnRole.Value)
        //            {
        //                sbRow.AppendFormat("| {0}", subsetRows.ElementAt(i).ItemArray[j].ToString());
        //                sbRow.Append(' ', fieldLength[j] - subsetRows.ElementAt(i).ItemArray[j].ToString().Length);

        //                //Comparaison settings
        //                if (String.IsNullOrEmpty(subsetRows.ElementAt(i).GetColumnError(j)))
        //                    sbRow.AppendFormat(" == ");
        //                else
        //                    sbRow.AppendFormat(" <> ");

        //                sbRow.AppendFormat("{0}", refs.ElementAt(i).ItemArray[j].ToString());
        //                sbRow.Append(' ', fieldLength[j] - refs.ElementAt(i).ItemArray[j].ToString().Length);
        //            }
        //        }
        //        sbRow.AppendLine("|");
        //        if (sbRow.Length > maxRowLength)
        //            maxRowLength = sbRow.Length;
        //        sbTable.Append(sbRow);
        //    }
        //    //information about #Rows and #Cols
        //    sb.AppendFormat("ResultSet with {0} row", rowCount);
        //    if (rowCount > 1)
        //        sb.Append('s');
        //    sb.AppendLine();

        //    //header
        //    sb.Append(' ', 4);
        //    sb.Append('-', fieldLength.Sum() + fieldLength.Count * 2 + 1);
        //    sb.AppendLine();

        //    //Table
        //    sb.Append(sbTable);

        //    //footer
        //    sb.Append(' ', 4);
        //    sb.Append('-', fieldLength.Sum() + fieldLength.Count * 2 + 1);
        //    sb.AppendLine();

        //    //If needed display # skipped rows
        //    if (rowCount > MAX_ROWS_DISPLAYED)
        //    {
        //        sb.Append(new string('.', 3));
        //        sb.Append(new string(' ', 3));
        //        sb.AppendFormat("{0} (of {1}) rows skipped for display purpose", rowCount - MAX_ROWS_DISPLAYED, rowCount);
        //        sb.Append(new string(' ', 3));
        //        sb.Append(new string('.', 3));
        //        sb.AppendLine();
        //    }

        //    sb.AppendLine();

        //    return sb.ToString();


        //}
        
        protected virtual string FormatResultSet(ResultSet resultSet)
        {
            var rows = resultSet.Rows.Cast<DataRow>().ToList();
            var columnCount = resultSet.Columns.Count;
            return FormatResultSet(rows, rows.Count);            
        }

        private void doPersist(ResultSet resultSet, string path)
        {
            var writer = new ResultSetCsvWriter(System.IO.Path.GetDirectoryName(path));
            writer.Write(System.IO.Path.GetFileName(path), resultSet);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using NBi.Core;
using NBi.Core.Query;

namespace NBi.Service
{
    public class TestCaseManager
    {
        private Func<string, string, bool> compare;

        internal TestCaseManager()
        {
            variables = new List<string>();
            content = new DataTable();
        }

        public void ReadFromCsv(string filename)
        {
            var csvReader = new CsvReader();
            content = csvReader.Read(filename, true);

            variables.Clear();
            foreach (DataColumn col in Content.Columns)
                variables.Add(col.ColumnName);
        }

        public string GetQueryFileContent(string queryFile)
        {
            var query = System.IO.File.ReadAllText(queryFile);
            return query;
        }

        public void ReadFromQueryFile(string queryFile, string connectionString)
        {
            var query = System.IO.File.ReadAllText(queryFile);
            ReadFromQuery(query, connectionString);
        }

        public void ReadFromQuery(string query, string connectionString)
        {
            var queryEngineFactory = new QueryEngineFactory();
            var queryEngine = queryEngineFactory.GetExecutor(query, connectionString);
            var ds = queryEngine.Execute();
            content = ds.Tables[0];

            variables.Clear();
            foreach (DataColumn col in Content.Columns)
                variables.Add(col.ColumnName);
        }

        private DataTable content;
        public DataTable Content
        {
            get
            {
                return content;
            }
        }

        private readonly List<string> variables;
        public IList<string> Variables
        {
            get
            {
                return variables;
            }
        }

        public void RenameVariable(int index, string newName)
        {
            if (variables.Count <= index)
                throw new ArgumentOutOfRangeException("index");
            //Rename the variable
            variables[index] = newName;
            //Rename the column
            content.Columns[index].ColumnName = newName;
        }

        public void MoveVariable(string variableName, int newPosition)
        {
            if (!variables.Contains(variableName))
                throw new ArgumentOutOfRangeException("variableName");
            //Move the variable
            var oldPosition = variables.IndexOf(variableName);
            variables.RemoveAt(oldPosition);
            variables.Insert(newPosition, variableName);
            //move the column
            content.Columns[oldPosition].SetOrdinal(newPosition);
        }

        public void Filter(string variableName, Operator @operator, bool negation, string text)
        {
            if (!variables.Contains(variableName))
                throw new ArgumentOutOfRangeException("variableName");

            AssignCompare(@operator);

            var index = variables.IndexOf(variableName);

            DataTableReader dataReader = null;
            var filteredRows = Content.AsEnumerable().Where(row => compare(row[index].ToString(), text) != negation);
            if (filteredRows.Count() > 0)
            {
                var filteredTable = filteredRows.CopyToDataTable();
                dataReader = filteredTable.CreateDataReader();
            }

            Content.Clear();
            if (dataReader!=null)
                Content.Load(dataReader, LoadOption.PreserveChanges);
            
            Content.AcceptChanges();
        }

        private void AssignCompare(Operator @operator)
        {
            switch (@operator)
            {
                case Operator.Equal:
                    compare = (a, b) => a == b;
                    break;
                case Operator.Like:
                    compare = Like;
                    break;
                default:
                    break;
            }
        }

        private bool Like(string value, string pattern)
        {
            //Turn a SQL-like-pattern into regex, by turning '%' into '.*'
            //Doesn't handle SQL's underscore into single character wild card '.{1,1}',
            //        or the way SQL uses square brackets for escaping.
            //(Note the same concept could work for DOS-style wildcards (* and ?)
            var regex = new Regex("^" + pattern
                           .Replace(".", "\\.")
                           .Replace("%", ".*")
                           .Replace("\\.*", "\\%")
                           + "$");

            return regex.IsMatch(value);
        }


        public void FilterDistinct()
        {
            DataTableReader dataReader = null;
            var distinctRows = Content.AsEnumerable().Distinct(System.Data.DataRowComparer.Default);

            if (distinctRows.Count() > 0)
            {
                var distinctTable = distinctRows.CopyToDataTable();
                dataReader = distinctTable.CreateDataReader();
            }
            Content.Clear();
            if(dataReader!=null)
                Content.Load(dataReader, LoadOption.PreserveChanges);
            Content.AcceptChanges();
        }

        public void Save(string filename)
        {
            var csvWriter = new CsvWriter(true);
            csvWriter.Write(Content, filename);
        }

    }
}

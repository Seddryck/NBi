using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using NBi.Core;
using NBi.Core.Query;
using NBi.Service.Dto;

namespace NBi.Service
{
    public class TestCasesManager
    {
        private Func<string, string, bool> compare;

        public TestCasesManager()
        {
            variables = new List<VariableInfo>();
            content = new DataTable();
            connectionStrings = new Dictionary<string, string>();
        }

        public void ReadFromCsv(string filename)
        {
            var csvReader = new CsvReader(filename, true);
            content = csvReader.Read();          

            variables.Clear();
            foreach (DataColumn col in Content.Columns)
            {
                var v = new VariableInfo(col.ColumnName);
                var uniqueValues = content.DefaultView.ToTable(true, col.ColumnName).AsEnumerable().Select(x => x[0].ToString().ToLower()).ToList();
                if (IsBooleanColumn(uniqueValues))
                    v.Type = VariableType.Boolean;
                variables.Add(v);
            }
        }

        protected virtual bool IsBooleanColumn(List<string> values)
        {
            var booleans = new string[] { "true", "false", "(none)" };
            var withoutNonBoolean = values.Except(booleans).Count() == 0;
            var withBoolean = values.Intersect(booleans).Count() > 0;

            return withBoolean && withoutNonBoolean;
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
            {
                var v = new VariableInfo(col.ColumnName);
                if (col.DataType is Boolean)
                    v.Type = VariableType.Boolean;
                variables.Add(v);
            }
        }

        private DataTable content;
        public DataTable Content
        {
            get
            {
                return content;
            }
        }

        private readonly List<VariableInfo> variables;
        public IList<VariableInfo> Variables
        {
            get
            {
                return variables;
            }
        }

        private readonly Dictionary<string,string> connectionStrings;
        public Dictionary<string, string> ConnectionStrings
        {
            get
            {
                return connectionStrings;
            }
        }

        public List<string> ConnectionStringNames
        {
            get
            {
                return ConnectionStrings.Keys.ToList();
            }
        }

        public void RenameVariable(int index, string newName)
        {
            if (variables.Count<=index)
                throw new ArgumentOutOfRangeException("index");
            //Rename the variable
            variables[index]=newName;
            //Rename the column
            content.Columns[index].ColumnName=newName;
        }

        public void MoveVariable(string variableName, int newPosition)
        {
            if(!variables.Contains(variableName))
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

            foreach (DataRow row in content.Rows)
            {
                if (compare(row[index].ToString(), text)==negation)
                    row.Delete();
            }
            content.AcceptChanges();
        }

        private void AssignCompare(Operator @operator)
        {
            switch (@operator)
            {
                case Operator.Equal:
                    compare = (a,b) => a==b;
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

        public void AddConnectionStrings(string name, string value)
        {
            if (connectionStrings.Keys.Contains(name))
                throw new ArgumentException("name");

            connectionStrings.Add(name, value);
        }

        public void RemoveConnectionStrings(string name)
        {
            if (!connectionStrings.Keys.Contains(name))
                throw new ArgumentException("name");

            connectionStrings.Remove(name);
        }

        public void EditConnectionStrings(string name, string newValue)
        {
            if (!connectionStrings.Keys.Contains(name))
                throw new ArgumentException("name");

            connectionStrings[name] = newValue;
        }
    }
}

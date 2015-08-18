using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;

namespace NBi.Core.ResultSet
{
    public class ResultSetBuilder : IResultSetBuilder
    {
        private readonly CsvProfile profile;
        public ResultSetBuilder()
            : this(CsvProfile.SemiColumnDoubleQuote)
        {
        }

        public ResultSetBuilder(CsvProfile profile)
        {
            this.profile = profile;
        }

        public virtual ResultSet Build(Object obj)
        {
            //Console.WriteLine("Debug: {0} {1}", obj.GetType(), obj.ToString()); 
            
            if (obj is ResultSet)
                return Build((ResultSet)obj);
            else if (obj is IList<IRow>)
                return Build((IList<IRow>)obj);
            else if (obj is IDbCommand)
                return Build((IDbCommand)obj);
            else if (obj is string)
                return Build((string)obj);
            else if (obj is object[])
                return Build((object[])obj);

            throw new ArgumentOutOfRangeException(string.Format("Type '{0}' is not expected when building a ResultSet", obj.GetType()));
        }
        
        public virtual ResultSet Build(ResultSet resultSet)
        {
            return resultSet;
        }

        public virtual ResultSet Build(IList<IRow> rows)
        {
            var rs = new ResultSet();
            rs.Load(rows);
            return rs;
        }
        
        public virtual ResultSet Build(IDbCommand cmd)
        {
            var qe = new QueryEngineFactory().GetExecutor(cmd);
            var ds = qe.Execute();
            var rs = new ResultSet();
            rs.Load(ds);
            return rs;
        }

        public virtual ResultSet Build(string path)
        {
            var reader = new CsvReader(profile);
            var dataTable = reader.Read(path, false);

            var rs = new ResultSet();
            rs.Load(dataTable);
            return rs;
        }

        public virtual ResultSet Build(object[] objects)
        {
            var resultSet = new ResultSet();
            resultSet.table = new DataTable();
            foreach (var obj in objects)
            {
                var items = obj as List<object>;
                var row = new Row();
                foreach (var item in items)
                {
                    var cell = new Cell();
                    cell.Value = item.ToString();
                    row.Cells.Add(cell);
                }
                resultSet.Rows.Add(row);
            }
            return resultSet;
        }

        private class Row : IRow
        {
            private readonly IList<ICell> cells = new List<ICell>();

            public IList<ICell> Cells
            {
                get { return cells; }
            }
        }

        private class Cell : ICell
        {
            private string value;
            public string Value
            {
                get { return value; }
                set { this.value = value; }
            }
        }

    }
}

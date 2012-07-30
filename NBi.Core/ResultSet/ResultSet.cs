using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public class ResultSet
    {
        protected DataTable _table;

        internal DataTable Table { get { return _table; } }
        
        public DataColumnCollection Columns
        {
            get { return _table.Columns; }
        }


        public DataRowCollection Rows
        {
            get { return _table.Rows; }
        }

        public ResultSet()
        {
        }

        public void Load(DataTable table)
        {
            _table = table;
        }
        
        public void Load (IEnumerable<DataRow> rows)
        {
            _table = new DataTable();
            rows.CopyToDataTable<DataRow>(_table, LoadOption.OverwriteChanges);
        }

        public void Load(IEnumerable<object[]> objects)
        {
            _table = new DataTable();
            
            //Build structure
            for (int i = 0; i < objects.First().Length; i++)
                Columns.Add(string.Format("Column{0}", i));

            //load each row one by one
            _table.BeginLoadData();
            foreach (var obj in objects)
                _table.LoadDataRow(obj, LoadOption.OverwriteChanges);
            _table.EndLoadData();
        }

    }
}
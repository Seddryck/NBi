using System;
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

        public void Load(DataSet dataSet)
        {
            Load(dataSet.Tables[0]);
        }

        public void Load(DataTable table)
        {
            _table = table;

            //display for debug
            ConsoleDisplay();
        }

        public void Load(IEnumerable<DataRow> rows)
        {
            _table = new DataTable();
            rows.CopyToDataTable<DataRow>(_table, LoadOption.OverwriteChanges);

            //display for debug
            ConsoleDisplay();
        }

        public void Load(IEnumerable<object[]> objects)
        {
            _table = new DataTable();
            
            //Build structure
            for (int i = 0; i < objects.First().Length; i++)
                Columns.Add(string.Format("Column{0}", i), objects.First().ElementAt(i).GetType());

            //load each row one by one
            _table.BeginLoadData();
            foreach (var obj in objects)
                _table.LoadDataRow(obj, LoadOption.OverwriteChanges);
            _table.EndLoadData();

            //display for debug
            ConsoleDisplay();
        }

        public void Load(IList<IRow> rows)
        {
            var objs = new List<object[]>();

            foreach (var row in rows)
            {
                var cells = row.Cells.ToArray<ICell>();
                var contentCells = new List<Object>();
                foreach (var cell in cells)
                    contentCells.Add(cell.Value);

                objs.Add(contentCells.ToArray());
            }

            this.Load(objs);
        }

        protected void ConsoleDisplay()
        {
            Console.WriteLine(new string('-', 30));
            foreach (DataRow row in Rows)
            {
                foreach (object cell in row.ItemArray)
                    Console.Write("| {0}\t", cell.ToString());
                Console.WriteLine("|");
            }
            Console.WriteLine(new string('-', 30));
            Console.WriteLine();
        }

    }
}
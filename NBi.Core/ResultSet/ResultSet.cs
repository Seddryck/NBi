using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public class ResultSet
    {
        protected DataTable table;

        internal DataTable Table { get { return table; } }
        
        public DataColumnCollection Columns
        {
            get { return table.Columns; }
        }


        public DataRowCollection Rows
        {
            get { return table.Rows; }
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
            this.table = table;

            //display for debug
            ConsoleDisplay();
        }

        public void Load(IEnumerable<DataRow> rows)
        {
            table = new DataTable();
            rows.CopyToDataTable<DataRow>(table, LoadOption.OverwriteChanges);

            //display for debug
            ConsoleDisplay();
        }

        public void Load(IEnumerable<object[]> objects)
        {
            table = new DataTable();

            //if > 0 row
            if (objects.Count() > 0)
            {

                //Build structure
                for (int i = 0; i < objects.First().Length; i++)
                {
                    if (objects.First().ElementAt(i) == null)
                        Columns.Add(string.Format("Column{0}", i), typeof(string));
                    else
                        Columns.Add(string.Format("Column{0}", i), objects.First().ElementAt(i).GetType());
                }

                //load each row one by one
                table.BeginLoadData();
                foreach (var obj in objects)
                {
                    //Transform (null) [string] into null
                    for (int i = 0; i < obj.Count(); i++)
                    {
                        if (obj[i] != null && obj[i].ToString().ToLower() == "(null)".ToLower())
                            obj[i] = null;
                    }

                    table.LoadDataRow(obj, LoadOption.OverwriteChanges);
                }
                table.EndLoadData();
            }

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
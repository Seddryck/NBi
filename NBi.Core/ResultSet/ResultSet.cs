using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public class ResultSet
    {
        protected internal DataTable Table { get; protected set; }
        
        public DataColumnCollection Columns
        {
            get { return Table.Columns; }
        }


        public DataRowCollection Rows
        {
            get { return Table.Rows; }
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
            this.Table = table;
        }

        public void Load(IEnumerable<DataRow> rows)
        {
            rows.CopyToDataTable(Table, LoadOption.OverwriteChanges);

            //display for debug
            ConsoleDisplay();
        }

        public void AddRange(IEnumerable<DataRow> rows)
        {
            rows.CopyToDataTable(Table, LoadOption.OverwriteChanges);
        }

        public ResultSet Clone()
        {
            var newRs = new ResultSet
            {
                Table = Table.Clone()
            };
            return newRs;
        }

        public void Load(string record)
        {
            Table = new DataTable();
            var fields = record.Split(';');

            //if > 0 row
            if (fields.Count() > 0)
            {
                //Build structure
                for (int i = 0; i < fields.Length; i++)
                    Columns.Add(string.Format("Column{0}", i), typeof(string));

                //load each row one by one
                Table.BeginLoadData();
                //Transform (null) [string] into null
                for (int i = 0; i < fields.Count(); i++)
                {
                    if (fields[i] != null && fields[i].ToString().ToLower() == "(null)".ToLower())
                        fields[i] = null;
                }
                Table.LoadDataRow(fields, LoadOption.OverwriteChanges);
                Table.EndLoadData();  
            }
        }

        public void Load(IEnumerable<object[]> objects)
        {
            Table = new DataTable();

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
                Table.BeginLoadData();
                foreach (var obj in objects)
                {
                    //Transform (null) [string] into null
                    for (int i = 0; i < obj.Count(); i++)
                    {
                        if (obj[i] != null && obj[i].ToString().ToLower() == "(null)".ToLower())
                            obj[i] = null;
                    }

                    Table.LoadDataRow(obj, LoadOption.OverwriteChanges);
                }
                Table.EndLoadData();
            }

            //display for debug
            ConsoleDisplay();
        }

        public void Load(IEnumerable<IRow> rows)
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
            if (!NBiTraceSwitch.TraceVerbose)
                return;

            Trace.WriteLine(string.Format(new string('-', 30)));
            foreach (DataRow row in Rows)
            {
                foreach (object cell in row.ItemArray)
                    Trace.Write(string.Format("| {0}\t", cell.ToString()));
                Trace.WriteLine(string.Format("|"));
            }
            Trace.WriteLine(string.Format(new string('-', 30)));
            Trace.WriteLine(string.Format(""));
        }

    }
}
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace NBi.Core.ResultSet
{
    internal class DataTableResultSet : IResultSet, IDisposable
    {
        private bool disposedValue;

        private DataTable Table { get; set; }
        
        public DataColumnCollection Columns
        {
            get => Table.Columns;
        }

        public DataRowCollection Rows
        {
            get => Table.Rows;
        }

        public DataColumn GetColumn(IColumnIdentifier columnIdentifier) 
            => columnIdentifier.GetColumn(this);

        public DataTableResultSet()
            => Table = new DataTable();

        public DataTableResultSet(DataTable table)
            => Table = table;

        public void AddRange(IEnumerable<DataRow> rows)
            => rows.CopyToDataTable(Table, LoadOption.OverwriteChanges);
        
        public void Add(DataRow row)
            => Table.ImportRow(row);

        public void ImportRow(DataRow row)
            => Table.ImportRow(row);

        public DataRow NewRow()
            => Table.NewRow();

        public void AcceptChanges()
            => Table.AcceptChanges();

        public DataTableReader CreateDataReader()
            => Table.CreateDataReader();



        public IResultSet Clone()
        {
            var newRs = new DataTableResultSet
            {
                Table = Table.Clone()
            };
            return newRs;
        }

        public void Clear()
            => Table.Clear();

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
            if (!Extensibility.NBiTraceSwitch.TraceVerbose)
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Table.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
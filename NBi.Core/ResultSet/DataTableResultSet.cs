﻿using NBi.Extensibility;
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

        public IEnumerable<IResultColumn> Columns
        {
            get { foreach (DataColumn column in Table.Columns) { yield return new DataColumnResultSet(column); } }
        }

        public IEnumerable<IResultRow> Rows
        {
            get { foreach (DataRow row in Table.Rows) { yield return new DataRowResultSet(row); } }
        }

        public int RowCount { get => Table.Rows.Count; }

        public IResultColumn AddColumn(string name)
            => AddColumn(name, typeof(object));

        public IResultColumn AddColumn(string name, Type type)
        {
            if (Table.Columns.Contains(name))
                throw new NBiException($"Can't add the column '{name}' because this column is already existing in the result-set.");

            var column = Table.Columns.Add(name, type);
            column.DefaultValue = DBNull.Value;
            return new DataColumnResultSet(column);
        }

        public IResultColumn AddColumn(string name, int ordinal, Type type)
        {
            AddColumn(name, type);
            Table.Columns[name].SetOrdinal(ordinal);
            return new DataColumnResultSet(Table.Columns[name]);
        }


        public int ColumnCount { get => Table.Columns.Count; }
        public bool ContainsColumn(string name)
            => Table.Columns.Contains(name);

        public IResultColumn GetColumn(IColumnIdentifier columnIdentifier)
            => columnIdentifier.GetColumn(this);

        public IResultColumn GetColumn(string name)
            => new DataColumnResultSet(Table.Columns[name]);
        public IResultColumn GetColumn(int ordinal)
            => new DataColumnResultSet(Table.Columns[ordinal]);

        public DataTableResultSet()
            => Table = new DataTable();

        public DataTableResultSet(DataTable table)
            => Table = table;

        public IResultRow Add(IResultRow row)
        {
            var newRow = Table.NewRow();
            newRow.ItemArray = row.ItemArray;
            Table.Rows.Add(newRow);
            return new DataRowResultSet(newRow);
        }

        public void AddRange(IEnumerable<IResultRow> rows)
        { foreach (var row in rows) { Add(row); } }

        public IResultRow this[int index]
        {
            get => new DataRowResultSet(Table.Rows[index]);
        }

        public IResultRow NewRow()
            => new DataRowResultSet(Table.NewRow());

        public void AcceptChanges()
            => Table.AcceptChanges();

        public DataTableReader CreateDataReader()
            => Table.CreateDataReader();

        public IResultSet Clone()
            => new DataTableResultSet(Table.Clone());

        public void Clear()
            => Table.Clear();

        public void InsertAt(IResultRow row, int index)
        {
            switch (row)
            {
                case DataRow r: Table.Rows.InsertAt(r, index); break;
                default:
                    throw new NotImplementedException();
            }
        }
        public void RemoveAt(int index)
            => Table.Rows.RemoveAt(index);


        public void Load(string record)
        {
            Table = new DataTable();
            var fields = record.Split(';');

            if (fields.Count() > 0)
            {
                //Build structure
                for (int i = 0; i < fields.Length; i++)
                    Table.Columns.Add(string.Format("Column{0}", i), typeof(string));

                Table.BeginLoadData();
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
                        Table.Columns.Add($"Column{i}", typeof(string));
                    else
                        Table.Columns.Add($"Column{i}", objects.First().ElementAt(i).GetType());
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
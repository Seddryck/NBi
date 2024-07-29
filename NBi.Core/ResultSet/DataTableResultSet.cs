using Expressif.Values;
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
            Table.Columns[name]!.SetOrdinal(ordinal);
            return new DataColumnResultSet(Table.Columns[name]!);
        }
        
        public int ColumnCount { get => Table.Columns.Count; }

        public bool ContainsColumn(string name)
            => Table.Columns.Contains(name);

        public IResultColumn? GetColumn(IColumnIdentifier columnIdentifier)
            => columnIdentifier.GetColumn(this);

        public IResultColumn? GetColumn(string name)
        {
            var column = Table.Columns[name];
            if (column is not null)
                return new DataColumnResultSet(column);
            return null;
        }

        public IResultColumn? GetColumn(int ordinal)
        {
            var column = Table.Columns[ordinal];
            if (column is not null)
                return new DataColumnResultSet(column);
            return null;
        }

        public DataTableResultSet()
            => Table = new DataTable();

        public DataTableResultSet(DataTable table)
            => Table = table;

        public IResultRow AddRow(IResultRow row)
            => AddRow(row.ItemArray);

        public IResultRow AddRow(object?[] itemArray)
        {
            var newRow = Table.NewRow();
            newRow.ItemArray = itemArray;
            Table.Rows.Add(newRow);
            return new DataRowResultSet(newRow);
        }

        public void AddRange(IEnumerable<IResultRow> rows)
        { foreach (var row in rows) { AddRow(row); } }

        public IResultRow this[int index]
        {
            get => new DataRowResultSet(Table.Rows[index]);
        }

        IResultRow IResultSet.NewRow()
            => new DataRowResultSet(Table.NewRow());

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

        public void Load(string record)
        {
            Table = new DataTable();
            string?[] fields = record.Split(';');

            if (fields.Length > 0)
            {
                //Build structure
                for (int i = 0; i < fields.Length; i++)
                    Table.Columns.Add(string.Format("Column{0}", i), typeof(string));

                Table.BeginLoadData();
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i] is not null && fields[i]!.ToString().Equals("(null)", StringComparison.CurrentCultureIgnoreCase))
                        fields[i] = null;
                }
                Table.LoadDataRow(fields, LoadOption.OverwriteChanges);
                Table.EndLoadData();
            }
        }

        public void Load(IEnumerable<object?[]> objects)
        {
            Table = new DataTable();

            //if > 0 row
            if (objects.Any())
            {

                //Build structure
                for (int i = 0; i < objects.First().Length; i++)
                {
                    if (objects.First().ElementAt(i) == null)
                        Table.Columns.Add($"Column{i}", typeof(string));
                    else
                        Table.Columns.Add($"Column{i}", objects.First()?.ElementAt(i)?.GetType() ?? throw new NullReferenceException());
                }

                 //load each row one by one
                Table.BeginLoadData();
                foreach (var obj in objects)
                {
                    //Transform (null) [string] into null
                    for (int i = 0; i < obj.Length; i++)
                    {
                        if (obj[i] is not null && obj[i]!.ToString()!.Equals("(null)", StringComparison.CurrentCultureIgnoreCase))
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
                var contentCells = new List<object>();
                foreach (var cell in cells)
                    contentCells.Add(cell.Value);

                objs.Add([.. contentCells]);
            }

            this.Load(objs);
        }

        protected void ConsoleDisplay()
        {
            if (!Extensibility.NBiTraceSwitch.TraceVerbose)
                return;

            Trace.WriteLine(string.Format(new string('-', 30)));
            foreach (var row in Rows)
            {
                foreach (object? cell in row.ItemArray)
                    Trace.Write($"| {cell?.ToString() ?? "(null)"}\t");
                Trace.WriteLine("|");
            }
            Trace.WriteLine(new string('-', 30));
            Trace.WriteLine("");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    Table.Dispose();
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
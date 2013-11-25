using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet.Formatter
{
    public class ClassicalFormatter : IFormatter
    {
        private readonly StringBuilder textBuilder;
        protected StringBuilder TextBuilder 
        { 
            get 
            { return textBuilder; }
        }

        private readonly List<int> columnsLength;
        protected IList<int> ColumnsLength
        {
            get
            { return columnsLength; }
        }

        protected int AbsoluteMaxRows { get; private set; }
        protected int MaxRows {get; private set;}

        public ClassicalFormatter() 
            : this(10,15)
        {
        }

        public ClassicalFormatter(int maxRows)
            : this(maxRows, maxRows)
        {
        }

        public ClassicalFormatter(int maxRows, int absoluteMaxRows)
        {
            textBuilder = new StringBuilder();
            columnsLength = new List<int>();
            MaxRows = maxRows;
            AbsoluteMaxRows = absoluteMaxRows;
        }

        public string Tabulize(IEnumerable<DataRow> rows)
        {
            if (rows.Count() == 0)
                TextBuilder.Append("This result set is empty.");
            else
            {
                BuildRowCount(rows.Count());
                DefineRowsToDisplay(ref rows);

                var columns = BuildColumns(rows);
                var formatter = BuildFormatter();
                var columnTexts = new List<IList<string>>();
                foreach (var column in columns)
                    columnTexts.Add(formatter.Tabulize(column));

                var i = 0;
                var rowEnumerator = columnTexts[0].GetEnumerator();
                while (rowEnumerator.MoveNext())
                {
                    foreach (var text in columnTexts)
                    {
                        textBuilder.Append("| ");
                        textBuilder.Append(text[i]);
                        textBuilder.Append(" ");
                    }
                    i += 1;
                    textBuilder.Append(formatter.NewLine);
                }

            }

            return TextBuilder.ToString();
        }
  
        private ColumnFormatter BuildFormatter()
        {
            return new ColumnFormatter(new HeaderFormatter(), new CellFormatter());
        }
  
        private IEnumerable<Column> BuildColumns(IEnumerable<DataRow> rows)
        {
            var columns = new List<Column>();
            foreach (DataColumn col in rows.First().Table.Columns)
            {
                var column = new Column();
                      
                column.Header.Load(rows.First().Table.Columns[col.Ordinal]);
                      
                foreach (DataRow row in rows)
                    column.Values.Add(row[col.Ordinal]); 
                    
                columns.Add(column);
            }

            return columns;
        }

        protected void DefineRowsToDisplay(ref IEnumerable<DataRow> rows)
        {
            if (rows.Count() > AbsoluteMaxRows)
                rows = rows.Take(MaxRows);
        }

        private void BuildRowCount(int rowsCount)
        {
            TextBuilder.AppendFormat("ResultSet with {0} row", rowsCount);
            TextBuilder.Append('s', Math.Max(Math.Min(rowsCount, 1)-1, 0));
            TextBuilder.AppendLine();
        }
    }
}

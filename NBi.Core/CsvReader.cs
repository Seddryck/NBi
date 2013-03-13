using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace NBi.Core
{
    public class CsvReader
    {
        public event ProgressStatusHandler ProgressStatusChanged;

        public CsvDefinition Definition { get; private set; }
        public string Filename { get; private set; }
        public bool FirstLineIsColumnName { get; private set; }

        public CsvReader(string filename, bool firstLineIsColumnName)
        {
            Filename = filename;
            Definition = CsvDefinition.SemiColumnDoubleQuote();
            FirstLineIsColumnName = firstLineIsColumnName;
        }

        public void RaiseProgressStatus(string status)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(status));
        }

        public void RaiseProgressStatus(string status, int current, int total)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs(string.Format(status, current, total), current, total));
        }

        public DataTable Read()
        {
            var table = new DataTable();

            RaiseProgressStatus("Processing CSV file");
            int i = 0;

            int count = 0;
            //Count the rows
            using (StreamReader r = new StreamReader(Filename, Encoding.Default))
            {
                while (r.ReadLine() != null)
                    count++;
            }
            count -= Convert.ToInt16(FirstLineIsColumnName);
            
            //Get first row to know the coutn of columns
            var columnCount = 0;
            var columnNames=new List<string>();
            using (StreamReader sr = new StreamReader(Filename, Encoding.Default))
            {
                var firstLine = sr.ReadLine();
                columnCount = firstLine.Split(Definition.FieldSeparator).Length;
                if (FirstLineIsColumnName)
                    columnNames.AddRange(SplitLine(firstLine));
            }

            //Correctly define the columns for the table
            for (int c = 0; c < columnCount; c++)
            {
                if (columnNames.Count == 0)
                    table.Columns.Add(string.Format("No name {0}", c.ToString()), typeof(string));
                else
                    table.Columns.Add(columnNames[c], typeof(string));
            }

            using (StreamReader sr = new StreamReader(Filename, Encoding.Default))
            {
                while (sr.Peek() >= 0)
                {
                    var line = sr.ReadLine();
                    if (i >= Convert.ToInt16(FirstLineIsColumnName))
                    {
                        RaiseProgressStatus("Loading row {0} of {1}", i, count);
                        var cells = SplitLine(line);
                        var row = table.NewRow();
                        row.ItemArray = cells;
                        table.Rows.Add(row);
                    }
                    i++;

                }
            }
            RaiseProgressStatus("CSV file processed");

            return table;
        }

        protected string[] SplitLine(string row)
        {
            var items = new List<string>();
            var list = new List<string>(row.Split(Definition.FieldSeparator));
            list.ForEach(item => items.Add(item.Replace(Definition.TextQualifier.ToString(), "")));
            return items.ToArray();
        }

    }
}

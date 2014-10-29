﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace NBi.Core
{
    public class CsvWriter
    {
        public event ProgressStatusHandler ProgressStatusChanged;

        public CsvProfile Definition { get; private set; }
        public bool FirstLineIsColumnName { get; private set; }

        public CsvWriter(bool firstLineIsColumnName)
        {
            Definition = CsvProfile.SemiColumnDoubleQuote;
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

        public void Write (DataTable table, string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename, false, Encoding.UTF8))
            {
                Write(table, writer);
            }
        }

        protected internal void Write(DataTable table, TextWriter writer)
        {
            RaiseProgressStatus("Writing CSV file");
            
            if (FirstLineIsColumnName)
                WriteHeader(table, writer);

            WriteContent(table, writer);
            writer.Flush();
            RaiseProgressStatus("CSV file written");
        }

        protected void WriteContent(DataTable table, TextWriter writer)
        {
            foreach (DataRow row in table.Rows)
            {
                int rowCount = 0;
                int count = table.Rows.Count;
                RaiseProgressStatus("writing row {0} of {1}", rowCount, count);

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    var content = row[i].ToString();
                    if (content.Contains(Definition.FieldSeparator) || content.Contains(Definition.RecordSeparator))
                        content = Definition.TextQualifier + content + Definition.TextQualifier;
                    
                    writer.Write(content);
                    writer.Write(i == table.Columns.Count - 1 ? Definition.RecordSeparator : Definition.FieldSeparator.ToString());
                }
            }
        }

        protected void WriteHeader(DataTable table, TextWriter writer)
        {
            RaiseProgressStatus("Writing header", 0, 0);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                writer.Write(table.Columns[i].ColumnName);
                writer.Write(i == table.Columns.Count - 1 ? Definition.RecordSeparator : Definition.FieldSeparator.ToString());
            }
        }
    }
}

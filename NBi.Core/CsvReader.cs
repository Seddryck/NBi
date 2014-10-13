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
        public int BufferSize { get; private set; }

        public CsvReader()
            : this(CsvDefinition.SemiColumnDoubleQuote(), 512)
        {
        }

        public CsvReader(CsvDefinition csvDefinition)
            : this(csvDefinition, 512)
        {
        }

        public CsvReader(int bufferSize)
            : this(CsvDefinition.SemiColumnDoubleQuote(), bufferSize)
        {
        }

        public CsvReader(CsvDefinition csvDefinition, int bufferSize)
        {
            Definition = csvDefinition;
            BufferSize = bufferSize;
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

        public DataTable Read(string filename, bool firstLineIsColumnName)
        {
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                return Read(stream, firstLineIsColumnName);
            }
        }

        protected internal DataTable Read(Stream stream, bool firstLineIsColumnName)
        {
            var table = new DataTable();

            RaiseProgressStatus("Processing CSV file");
            int i = 0;

            RaiseProgressStatus("Counting records");
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true))
            {
                var count = CountRecordSeparator(reader, Definition.RecordSeparator, BufferSize);
                count -= Convert.ToInt16(firstLineIsColumnName);
                stream.Position = 0;
                reader.DiscardBufferedData();

                //Get first row to know the count of columns
                RaiseProgressStatus("Defining fields");
                var columnCount = 0;
                var columnNames = new List<string>();
                var firstLine = GetFirstRecord(reader, Definition.RecordSeparator, BufferSize);
                columnCount = firstLine.Split(Definition.FieldSeparator).Length;
                if (firstLineIsColumnName)
                    columnNames.AddRange(SplitLine(firstLine));
                
                //Reset at 0, if this line is not the header
                if(!firstLineIsColumnName)
                {
                    stream.Position = 0;
                    reader.DiscardBufferedData();
                }
                    

                //Correctly define the columns for the table
                for (int c = 0; c < columnCount; c++)
                {
                    if (columnNames.Count == 0)
                        table.Columns.Add(string.Format("No name {0}", c.ToString()), typeof(string));
                    else
                        table.Columns.Add(columnNames[c], typeof(string));
                }

                //Parse the whole file

                bool isLastRecord = false;
                i = 1;
                var pos = 0;

                while (!isLastRecord)
                {
                    RaiseProgressStatus("Loading row {0} of {1}", i, count);
                    var record = GetNextRecord(reader, Definition.RecordSeparator, BufferSize);
                    pos += record.Length;
                    reader.BaseStream.Seek(pos, SeekOrigin.Begin);
                    isLastRecord = IsLastRecord(record);

                    record = CleanRecord(record, Definition.RecordSeparator);

                    var cells = SplitLine(record);
                    var row = table.NewRow();
                    row.ItemArray = cells;
                    table.Rows.Add(row);
                }
            }
            RaiseProgressStatus("CSV file processed");

            return table;
        }

        protected string[] SplitLine(string row)
        {
            var items = new List<string>();
            var list = new List<string>(row.Split(Definition.FieldSeparator));
            list.ForEach(item => items.Add(RemoveTextQualifier(item)));
            return items.ToArray();
        }

        protected internal string RemoveTextQualifier(string item)
        {
            if (string.IsNullOrEmpty(item))
                return string.Empty;

            if (item.Length == 1)
                return item;

            if (item[0] == Definition.TextQualifier && item[item.Length - 1] == Definition.TextQualifier)
                return item.Substring(1, item.Length - 2);

            return item;
        }

        protected internal int CountRecordSeparator(StreamReader reader, string recordSeparator, int bufferSize)
        {
            int i = 0;
            int n = 0;
            int j = 0;
            bool separatorAtEnd = false;

            do
            {
                char[] buffer = new char[bufferSize];
                n = reader.Read(buffer, 0, bufferSize);
                if (n > 0 && i == 0)
                    i = 1;


                foreach (var c in buffer)
                {
                    if (c != '\0')
                    {
                        separatorAtEnd = false;
                        if (c == recordSeparator[j])
                        {
                            j++;
                            if (j == recordSeparator.Length)
                            {
                                i++;
                                j = 0;
                                separatorAtEnd = true;
                            }
                        }
                        else
                            j = 0;
                    }
                }
            } while (n > 0);

            if (separatorAtEnd)
                i -= 1;

            return i;
        }

        protected internal string GetFirstRecord(StreamReader reader, string recordSeparator, int bufferSize)
        {
            var stringBuilder = new StringBuilder();
            int n = 0;
            int j = 0;

            while (true)
            {
                char[] buffer = new char[bufferSize];
                n = reader.Read(buffer, 0, bufferSize);

                foreach (var c in buffer)
                {

                    if (c != '\0')
                    {
                        stringBuilder.Append(c);
                        if (c == recordSeparator[j])
                        {
                            j++;
                            if (j == recordSeparator.Length)
                            {
                                return stringBuilder.ToString();
                                j = 0;
                            }
                        }
                        else
                            j = 0;
                    }
                    else
                        return stringBuilder.ToString();
                }
            }
        }

        protected internal string GetNextRecord(StreamReader reader, string recordSeparator, int bufferSize)
        {
            int n = 0;
            int j = 0;
            var stringBuilder = new StringBuilder();

            do
            {
                var buffer = new char[bufferSize];
                n = reader.Read(buffer, 0, bufferSize);
                foreach (var c in buffer)
                {
                    stringBuilder.Append(c);

                    if (c == recordSeparator[j])
                    {
                        j++;
                        if (j == recordSeparator.Length)
                        {
                            reader.DiscardBufferedData();
                            return stringBuilder.ToString();
                        }
                            
                    }
                    else
                        j = 0;
                }
            } while (n>0);
            return stringBuilder.ToString();
        }

        protected internal string CleanRecord(string record, string recordSeparator)
        {
            int i = 0;
            while (record.Length > i && record[record.Length - 1 - i] == '\0')
                i++;

            if (i > 0)
                record = record.Remove(record.Length - i, i);

            if (record.EndsWith(recordSeparator))
                return record.Remove(record.Length - recordSeparator.Length, recordSeparator.Length);

            return record;
        }

        protected internal bool IsLastRecord(string record)
        {
            return (String.IsNullOrEmpty(record) || record.EndsWith("\0"));
        }
    }
}

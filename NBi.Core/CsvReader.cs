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

        public CsvProfile Definition { get; private set; }
        public int BufferSize { get; private set; }

        public CsvReader()
            : this(CsvProfile.SemiColumnDoubleQuote, 512)
        {
        }

        public CsvReader(CsvProfile csvDefinition)
            : this(csvDefinition, 512)
        {
        }

        public CsvReader(int bufferSize)
            : this(CsvProfile.SemiColumnDoubleQuote, bufferSize)
        {
        }

        public CsvReader(CsvProfile csvDefinition, int bufferSize)
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
            if (!File.Exists(filename))
                throw new ExternalDependencyNotFoundException(filename);

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

                ////Check if the first byte is BOM or not
                var encodingBytesCount = 0;
                var buffer = new char[4];
                reader.Read(buffer, 0, 4);
                var startingString = new string(buffer);
                if (startingString[0] == 65279)
                    encodingBytesCount = 1;

                stream.Position = 0;
                reader.DiscardBufferedData();

                //Get first row to know the count of columns
                RaiseProgressStatus("Defining fields");
                var columnCount = 0;
                var columnNames = new List<string>();
                var firstLine = GetFirstRecord(reader, Definition.RecordSeparator, BufferSize);
                if (encodingBytesCount>0)
                    firstLine = firstLine.Substring(encodingBytesCount, firstLine.Length - encodingBytesCount);
                if (firstLine.EndsWith(Definition.RecordSeparator))
                    firstLine = firstLine.Substring(0, firstLine.Length - Definition.RecordSeparator.Length);
                columnCount = firstLine.Split(Definition.FieldSeparator).Length;
                if (firstLineIsColumnName)
                    columnNames.AddRange(SplitLine(firstLine));
                

                //Correctly define the columns for the table
                for (int c = 0; c < columnCount; c++)
                {
                    if (columnNames.Count == 0)
                        table.Columns.Add(string.Format("No name {0}", c.ToString()), typeof(string));
                    else
                        table.Columns.Add(columnNames[c], typeof(string));
                }

                //Parse the whole file
                stream.Position = 0;
                reader.DiscardBufferedData();

                bool isLastRecord = false;
                i = 0;
                var alreadyRead = string.Empty;
                var extraRead = string.Empty;

                while (!isLastRecord)
                {
                    RaiseProgressStatus("Loading row {0} of {1}", i, count);
                    var records = GetNextRecords(reader, Definition.RecordSeparator, BufferSize, alreadyRead, out extraRead);
                    foreach (var record in records)
                    {
                        var recordToParse = record;
                        
                        if (i==0 && encodingBytesCount>0)
                            recordToParse = recordToParse.Substring(encodingBytesCount, recordToParse.Length - encodingBytesCount);
                        
                        i++;
                        if (i!=1 || !firstLineIsColumnName)
                        { 
                            isLastRecord = IsLastRecord(recordToParse);
                            var cleanRecord = CleanRecord(recordToParse, Definition.RecordSeparator);
                            var cells = SplitLine(cleanRecord);
                            var row = table.NewRow();
                            if (row.ItemArray.Length<cells.Length)
                                throw new InvalidDataException
                                (
                                    string.Format
                                    (
                                        "The row {0} contains {1} more field{2} than expected."
                                        , table.Rows.Count + 1 + Convert.ToInt32(firstLineIsColumnName)
                                        , cells.Length - row.ItemArray.Length
                                        , cells.Length - row.ItemArray.Length>1 ? "s" : string.Empty
                                    )
                                );
                            row.ItemArray = cells;
                            table.Rows.Add(row);
                        }
                    }
                    alreadyRead = extraRead;
                    isLastRecord |= records.Count() == 0;                       
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
                                return stringBuilder.ToString();
                        }
                        else
                            j = 0;
                    }
                    else
                        return stringBuilder.ToString();
                }
            }
        }

        protected internal IEnumerable<string> GetNextRecords(StreamReader reader, string recordSeparator, int bufferSize, string alreadyRead, out string extraRead)
        {
            int n = 0;
            int j = 0;
            var stringBuilder = new StringBuilder();
            var records = new List<string>();
            var eof = false;
            
            extraRead = string.Empty;
            stringBuilder.Append(alreadyRead);
            j = IdentifyPartialRecordSeparator(alreadyRead, recordSeparator);

            do
            {
                var buffer = new char[bufferSize];
                n = reader.Read(buffer, 0, bufferSize);
                

                if (n > 0)
                {
                    foreach (var c in buffer)
                    {
                        stringBuilder.Append(c);

                        if (c == '\0')
                        {
                            eof = true;
                            break;
                        }


                        if (c == recordSeparator[j])
                        {
                            j++;
                            if (j == recordSeparator.Length)
                            {
                                records.Add(stringBuilder.ToString());
                                stringBuilder.Clear();
                                j = 0;
                            }

                        }
                        else
                            j = 0;
                    }
                }
                else
                {
                    eof = true;
                    stringBuilder.Append('\0');
                }
                    

            } while (records.Count==0 && !eof);
            
            if (eof && stringBuilder.Length>0 && stringBuilder[0]!='\0')
                records.Add(stringBuilder.ToString());

            if (stringBuilder.Length > 0)
                extraRead = stringBuilder.ToString();
            
            return records;
        }

        internal int IdentifyPartialRecordSeparator(string text, string recordSeparator)
        {
            int i = Math.Min(recordSeparator.Length - 1, text.Length);
            while(i>0)
            {
                if (text.EndsWith(recordSeparator.Substring(0, i)))
                    return i;
                i--;
            }
            return 0;
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

using NBi.Extensibility;
using NBi.Extensibility.FlatFile;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace NBi.Core.FlatFile
{
    public class CsvReader : IFlatFileReader
    {
        public event ProgressStatusHandler ProgressStatusChanged;

        protected CsvProfile Profile { get; private set; }
        protected int BufferSize { get; private set; }

        public CsvReader()
            : this(CsvProfile.SemiColumnDoubleQuote, 512)
        {
        }

        public CsvReader(CsvProfile profile)
            : this(profile, 512)
        {
        }

        public CsvReader(int bufferSize)
            : this(CsvProfile.SemiColumnDoubleQuote, bufferSize)
        {
        }

        public CsvReader(CsvProfile profile, int bufferSize)
        {
            this.Profile = profile;
            BufferSize = bufferSize;
        }

        protected void RaiseProgressStatus(string status)
            => ProgressStatusChanged?.Invoke(this, new ProgressStatusEventArgs(status));

        protected void RaiseProgressStatus(string status, int current, int total)
            => ProgressStatusChanged?.Invoke(this, new ProgressStatusEventArgs(string.Format(status, current, total), current, total));

        /// <summary>
        /// Read the CSV file and returns the corresponding DataTable
        /// </summary>
        /// <param name="filename">Name of the CSV file</param>
        /// <returns>A DataTable containing all the records (rows) and fields (columns) available in the CSV file</returns>
        public DataTable ToDataTable(string filename)
        {
            CheckFileExists(filename);
            var encoding = GetFileEncoding(filename);

            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                return Read(stream, encoding, Profile.FirstRowHeader, Profile.RecordSeparator, Profile.FieldSeparator, Profile.TextQualifier, Profile.EmptyCell, Profile.MissingCell);
        }

        /// <summary>
        /// Read the CSV file, overriding the value of isFirstRowHeader defined in the profile.
        /// </summary>
        /// <param name="filename">Name of the CSV file</param>
        /// <param name="isFirstRowHeader">Overrides the value of isFirstRowHeader defined in the profile</param>
        /// <returns>A DataTable containing all the records (rows) and fields (columns) available in the CSV file</returns>
        public DataTable ToDataTable(string filename, bool isFirstRowHeader)
        {
            CheckFileExists(filename);
            var encoding = GetFileEncoding(filename);

            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                return Read(stream, encoding, isFirstRowHeader, Profile.RecordSeparator, Profile.FieldSeparator, Profile.TextQualifier, Profile.EmptyCell, Profile.MissingCell);
        }

        protected virtual void CheckFileExists(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"The file {filename} was not found.", filename);
        }

        protected internal DataTable Read(Stream stream)
            => this.Read(stream, Encoding.UTF8, Profile.FirstRowHeader, Profile.RecordSeparator, Profile.FieldSeparator, Profile.TextQualifier, Profile.EmptyCell, Profile.MissingCell);

        protected internal DataTable Read(Stream stream, Encoding encoding, bool isFirstRowHeader, string recordSeparator, char fieldSeparator, char textQualifier, string emptyCell, string missingCell)
        {
            RaiseProgressStatus("Starting to process the CSV file ...");
            int i = 0;

            using (StreamReader reader = new StreamReader(stream, encoding, false))
            {
                var count = CountRecords(reader, Profile.RecordSeparator, isFirstRowHeader, Profile.PerformanceOptmized);
                var table = DefineFields(reader, recordSeparator, fieldSeparator, textQualifier, isFirstRowHeader);

                bool isLastRecord = false;
                i = 0;
                var alreadyRead = string.Empty;
                var extraRead = string.Empty;

                while (!isLastRecord)
                {
                    if (count.HasValue)
                        RaiseProgressStatus($"Loading row {i} of {count} ...", i, count.Value);
                    else
                        RaiseProgressStatus($"Loading row {i}{(count.HasValue ? $" of {count}" : string.Empty)} ...");

                    var records = GetNextRecords(reader, recordSeparator, BufferSize, alreadyRead, out extraRead);
                    foreach (var record in records)
                    {
                        var recordToParse = record;

                        //if (i == 0 && encodingBytesCount > 0)
                        //    recordToParse = recordToParse.Substring(encodingBytesCount, recordToParse.Length - encodingBytesCount);

                        i++;
                        if (i != 1 || !isFirstRowHeader)
                        {
                            isLastRecord = IsLastRecord(recordToParse);
                            var cleanRecord = CleanRecord(recordToParse, recordSeparator);
                            var cells = SplitLine(cleanRecord, fieldSeparator, textQualifier, emptyCell).ToList();
                            var row = table.NewRow();
                            if (row.ItemArray.Length < cells.Count)
                                throw new InvalidDataException
                                (
                                    string.Format
                                    (
                                        "The record {0} contains {1} more field{2} than expected."
                                        , table.Rows.Count + 1 + Convert.ToInt32(isFirstRowHeader)
                                        , cells.Count - row.ItemArray.Length
                                        , cells.Count - row.ItemArray.Length > 1 ? "s" : string.Empty
                                    )
                                );

                            //fill the missing cells
                            while (row.ItemArray.Length > cells.Count)
                                cells.Add(missingCell);

                            row.ItemArray = cells.ToArray();
                            table.Rows.Add(row);
                        }
                    }
                    alreadyRead = extraRead;
                    isLastRecord |= records.Count() == 0;
                }
                RaiseProgressStatus("CSV file fully processed.");

                return table;
            }
        }

        protected virtual DataTable DefineFields(StreamReader reader, string recordSeparator, char fieldSeparator, char textQualifier, bool isFirstRowHeader)
        {
            //Get first record to know the count of fields
            RaiseProgressStatus("Defining fields");
            var columnCount = 0;
            var columnNames = new List<string>();
            var firstLine = GetFirstRecord(reader, recordSeparator, BufferSize);
            //if (encodingBytesCount > 0)
            //    firstLine = firstLine.Substring(encodingBytesCount, firstLine.Length - encodingBytesCount);
            if (firstLine.EndsWith(recordSeparator))
                firstLine = firstLine.Substring(0, firstLine.Length - recordSeparator.Length);
            columnCount = firstLine.Split(fieldSeparator).Length;
            if (isFirstRowHeader)
                columnNames.AddRange(SplitLine(firstLine, fieldSeparator, textQualifier, string.Empty));


            //Correctly define the columns for the table
            var table = new DataTable();
            for (int c = 0; c < columnCount; c++)
            {
                if (columnNames.Count == 0)
                    table.Columns.Add(string.Format("No name {0}", c.ToString()), typeof(string));
                else
                    table.Columns.Add(columnNames[c], typeof(string));
            }
            RaiseProgressStatus($"{table.Columns.Count} field{(table.Columns.Count > 1 ? "s were" : " was")}  identified.");

            //Parse the whole file
            reader.BaseStream.Position = 0;
            reader.DiscardBufferedData();

            return table;
        }

        protected virtual int CalculateEncodingBytes(StreamReader reader)
        {
            //Check if the first byte is BOM or not
            var buffer = new char[4];
            reader.Read(buffer, 0, 4);
            var encodingBytesCount = (new string(buffer)[0] == 65279) ? 1 : 0;
            RaiseProgressStatus($"Encoding bytes was set to {encodingBytesCount}.");

            reader.BaseStream.Position = 0;
            reader.DiscardBufferedData();
            return encodingBytesCount;
        }

        /// <summary>
        /// Detects the byte order mark of a file and returns
        /// an appropriate encoding for the file.
        /// </summary>
        /// <param name="srcFile"></param>
        /// <returns></returns>
        protected virtual Encoding GetFileEncoding(string srcFile)
        {
            // Default  = Ansi CodePage
            var encoding = Encoding.Default;

            // Detect byte order mark if any - otherwise assume default
            var buffer = new byte[5];
            var file = new FileStream(srcFile, FileMode.Open);
            file.Read(buffer, 0, 5);
            file.Close();

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                encoding = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                encoding = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                encoding = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                encoding = Encoding.UTF7;

            return encoding;
        }

        protected virtual int? CountRecords(StreamReader reader, string recordSeparator, bool isFirstRowHeader, bool isPerformanceOptimized)
        {
            if (isPerformanceOptimized)
                return null;

            RaiseProgressStatus("Counting records ...");
            var count = CountRecordSeparators(reader, recordSeparator, BufferSize);
            count -= Convert.ToInt16(isFirstRowHeader);
            RaiseProgressStatus($"{count} record{(count > 1 ? "s were" : " was")} identified.");

            reader.BaseStream.Position = 0;
            reader.DiscardBufferedData();
            return count;
        }

        protected virtual int CountRecordSeparators(StreamReader reader, string recordSeparator, int bufferSize)
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

        protected virtual IEnumerable<string> SplitLine(string row, char fieldSeparator, char textQualifier, string emptyCell)
        {
            var list = new List<string>(row.Split(fieldSeparator));

            foreach (var item in list)
            {
                var value = RemoveTextQualifier(item, textQualifier);
                if (string.IsNullOrEmpty(value) && value != null)
                    yield return emptyCell;
                else
                    yield return value;
            }
        }

        protected virtual string RemoveTextQualifier(string item, char textQualifier)
        {
            if (string.IsNullOrEmpty(item))
                return string.Empty;

            if (item == "(null)")
                return null;

            if (item.Length == 1)
                return item;

            if (item[0] == textQualifier && item[item.Length - 1] == textQualifier)
                return item.Substring(1, item.Length - 2);

            return item;
        }

        protected virtual string GetFirstRecord(StreamReader reader, string recordSeparator, int bufferSize)
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

        protected virtual IEnumerable<string> GetNextRecords(StreamReader reader, string recordSeparator, int bufferSize, string alreadyRead, out string extraRead)
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


            } while (records.Count == 0 && !eof);

            if (eof && stringBuilder.Length > 0 && stringBuilder[0] != '\0')
                records.Add(stringBuilder.ToString());

            if (stringBuilder.Length > 0)
                extraRead = stringBuilder.ToString();

            return records;
        }

        protected virtual int IdentifyPartialRecordSeparator(string text, string recordSeparator)
        {
            int i = Math.Min(recordSeparator.Length - 1, text.Length);
            while (i > 0)
            {
                if (text.EndsWith(recordSeparator.Substring(0, i)))
                    return i;
                i--;
            }
            return 0;
        }

        protected virtual string CleanRecord(string record, string recordSeparator)
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

        protected virtual bool IsLastRecord(string record) => string.IsNullOrEmpty(record) || record.EndsWith("\0");
    }
}

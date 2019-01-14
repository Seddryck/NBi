using NBi.Extensibility;
using NBi.Extensibility.FlatFile;
using PocketCsvReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Acceptance.Resources
{
    public class TsvReader : CsvReader, IFlatFileReader
    {
        public bool IsFirstLine { get; set; } = true;

        public TsvReader()
            : base(new CsvProfile('\t', '\"', "\r\n", true, true, "(empty)", "(null)"))
        {
            base.ProgressStatusChanged += (s, e) 
                => ProgressStatusChanged?.Invoke(this
                , new NBi.Extensibility.ProgressStatusEventArgs(e.Status, e.Progress.Current, e.Progress.Total));
        }

        public new event NBi.Extensibility.ProgressStatusHandler ProgressStatusChanged;

        public new DataTable ToDataTable(string filename) => base.ToDataTable(filename);

        protected override IEnumerable<string> GetNextRecords(StreamReader reader, string recordSeparator, int bufferSize, string alreadyRead, out string extraRead)
        {
            extraRead = string.Empty;
            while (!reader.EndOfStream)
            {
                var value = reader.ReadLine();
                
                //The first line should also be submitted because it will be skipped in the base class implementation
                if (((!string.IsNullOrEmpty(value)) && value.Contains("10YBE")) || IsFirstLine)
                {
                    IsFirstLine = false;
                    return new List<string>() { value };
                }
            }
            return new List<string>();
        }

        protected override IEnumerable<string> SplitLine(string row, char fieldSeparator, char textQualifier, string emptyCell)
            => row.Split(new[] { fieldSeparator }).Select(x => x == string.Empty ? emptyCell : x);

        protected override string CleanRecord(string record, string recordSeparator)
            => record;
    }
}

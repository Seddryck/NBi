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

namespace Entsoe.FileManagement
{
    public class TsvReader : CsvReader, IFlatFileReader
    {
        public string Pattern { get; private set; }
        public bool IsFirstLine { get; set; } = true;

        public TsvReader()
            : this(null) { }

        public TsvReader(string pattern)
            : base(new CsvProfile('\t', '\"', "\r\n", true, true, "(empty)", "(null)"))
        {
            Pattern = pattern;
            base.ProgressStatusChanged += (s, e) 
                => ProgressStatusChanged?.Invoke(this
                , new NBi.Extensibility.ProgressStatusEventArgs(e.Status, e.Progress.Current, e.Progress.Total));
        }

        public new event NBi.Extensibility.ProgressStatusHandler ProgressStatusChanged;

        public new DataTable ToDataTable(string filename) => base.ToDataTable(filename);

        protected override IEnumerable<string> GetNextRecords(StreamReader reader, string recordSeparator, int bufferSize, string alreadyRead, out string extraRead)
        {
            extraRead = string.Empty;
            string value = string.Empty;
            do
            {
                value = reader.ReadLine();
                if ((!string.IsNullOrEmpty(value) && (string.IsNullOrEmpty(Pattern) || value.Contains(Pattern))) || IsFirstLine)
                {
                    IsFirstLine = false;
                    return new List<string>() { value };
                }
                    
            }
            while (!string.IsNullOrEmpty(value));
                return new List<string>();
        }

        protected override IEnumerable<string> SplitLine(string row, char fieldSeparator, char textQualifier, string emptyCell)
            => row.Split(new[] { fieldSeparator }).Select(x => x == string.Empty ? emptyCell : x);

        protected override string CleanRecord(string record, string recordSeparator)
            => record;
    }
}

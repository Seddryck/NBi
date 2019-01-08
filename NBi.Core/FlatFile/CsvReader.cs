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
    public class CsvReader : PocketCsvReader.CsvReader, IFlatFileReader
    {
        public new event ProgressStatusHandler ProgressStatusChanged;

        public CsvReader()
            : base(PocketCsvReader.CsvProfile.SemiColumnDoubleQuote, 512)
        {
            base.ProgressStatusChanged += (s, e) => ProgressStatusChanged?.Invoke(this, new ProgressStatusEventArgs(e.Status, e.Progress.Current, e.Progress.Total));
        }

        internal CsvReader(CsvProfile profile)
            : base(profile) { }

        internal CsvReader(CsvProfile profile, int bufferSize)
            : base(profile, bufferSize) { }
    }
}

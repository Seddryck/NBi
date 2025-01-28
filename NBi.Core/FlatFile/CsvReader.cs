using NBi.Extensibility;
using NBi.Extensibility.FlatFile;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace NBi.Core.FlatFile;

public class CsvReader : PocketCsvReader.CsvReader, IFlatFileReader
{
    public new event ProgressStatusHandler? ProgressStatusChanged;

    public CsvReader()
        : this(CsvProfile.SemiColumnDoubleQuote, 512) { }

    public CsvReader(CsvProfile profile)
        : this(profile, 512) { }

    public CsvReader(CsvProfile profile, int bufferSize)
        : base(profile, bufferSize)
    {
        base.ProgressStatusChanged += (s, e) => ProgressStatusChanged?.Invoke(this, new ProgressStatusEventArgs(e.Status, e.Progress.Current, e.Progress.Total));
    }
}

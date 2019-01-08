using NBi.Extensibility.FlatFile;

namespace NBi.Core.FlatFile
{
    public class CsvProfile : PocketCsvReader.CsvProfile, IFlatFileProfile
    {
        public static new CsvProfile SemiColumnDoubleQuote { get; private set; } = new CsvProfile(';');

        private CsvProfile(char fieldSeparator)
            : base(fieldSeparator, '\"') { }

        internal CsvProfile(bool firstRowHeader)
            : base(firstRowHeader) { }

        internal CsvProfile(char fieldSeparator, char textQualifier, string recordSeparator, bool firstRowHeader, bool performanceOptimized, string emptyCell, string missingCell)
            : base(fieldSeparator, textQualifier, recordSeparator, firstRowHeader, performanceOptimized, emptyCell, missingCell) { }

        //public bool ShouldSerializeFieldSeparator() => false;
        //public bool ShouldSerializeTextQualifier() => false;
        //public bool ShouldSerializeRecordSeparator() => false;
        //public bool ShouldSerializeFirstRowHeader() => false;
        //public bool ShouldSerializePerformanceOptmized() => false;
        //public bool ShouldSerializeEmptyCell() => false;
        //public bool ShouldSerializeMissingCell() => false;
    }
}

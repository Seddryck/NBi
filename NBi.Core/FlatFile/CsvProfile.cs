using NBi.Extensibility.FlatFile;

namespace NBi.Core.FlatFile
{
    public class CsvProfile : PocketCsvReader.CsvProfile, IFlatFileProfile
    {
        public static new CsvProfile SemiColumnDoubleQuote { get; private set; } = new CsvProfile(';');

        private CsvProfile(char fieldSeparator)
            : base(fieldSeparator, '\"') { }

        public CsvProfile(bool firstRowHeader)
            : base(firstRowHeader) { }

        public CsvProfile(char fieldSeparator, char textQualifier, string recordSeparator, bool firstRowHeader, bool performanceOptimized, string emptyCell, string missingCell)
            : base(fieldSeparator, textQualifier, recordSeparator, firstRowHeader, performanceOptimized, emptyCell, missingCell) { }
    }
}

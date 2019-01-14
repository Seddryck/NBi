using System.Collections.Generic;
using NBi.Extensibility.FlatFile;

namespace NBi.Core.FlatFile
{
    public class CsvProfile : PocketCsvReader.CsvProfile, IFlatFileProfile
    {
        public static new CsvProfile SemiColumnDoubleQuote { get; private set; } = new CsvProfile(';');

        public CsvProfile(IDictionary<string, object> attributes)
            : base (
            (char)  (attributes.ContainsKey("field-separator")          ? attributes["field-separator"]         : ';'),
            (char)  (attributes.ContainsKey("text-qualifier")           ? attributes["text-qualifier"]          : '\"'),
            (string)(attributes.ContainsKey("record-separator")         ? attributes["record-separator"]        : "\r\n"),
            (bool)  (attributes.ContainsKey("first-row-header")         ? attributes["first-row-header"]        : false),
            (bool)  (attributes.ContainsKey("performance-optimized")    ? attributes["performance-optimized"]   : true),
            (string)(attributes.ContainsKey("missing-cell")             ? attributes["missing-cell"]            : "(null)"),
            (string)(attributes.ContainsKey("empty-cell")               ? attributes["empty-cell"]              : "(empty)")
        ) {}

        public IDictionary<string, object> Attributes => new Dictionary<string, object>()
                {
                    { "field-separator", base.FieldSeparator },
                    { "text-qualifier", base.TextQualifier },
                    { "record-separator", base.RecordSeparator },
                    { "first-row-header", base.FirstRowHeader },
                    { "performance-optimized", base.PerformanceOptmized },
                    { "missing-cell", base.MissingCell },
                    { "empty-cell", base.EmptyCell },
                };

        private CsvProfile(char fieldSeparator)
            : base(fieldSeparator, '\"') { }

        public CsvProfile(bool firstRowHeader)
            : base(firstRowHeader) { }

        public CsvProfile(char fieldSeparator, char textQualifier, string recordSeparator, bool firstRowHeader, bool performanceOptimized, string emptyCell, string missingCell)
            : base(fieldSeparator, textQualifier, recordSeparator, firstRowHeader, performanceOptimized, emptyCell, missingCell) { }
    }
}

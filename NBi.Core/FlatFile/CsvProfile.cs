using System.Collections.Generic;
using NBi.Extensibility.FlatFile;
using PocketCsvReader;

namespace NBi.Core.FlatFile;

public class CsvProfile : PocketCsvReader.CsvProfile, IFlatFileProfile
{
    public static new CsvProfile SemiColumnDoubleQuote { get; private set; } = new CsvProfile(';');

    public CsvProfile(IDictionary<string, object> attributes)
        : base (
        (char)  (attributes.TryGetValue("field-separator", out var fs) ? fs : ';'),
        (char)  (attributes.TryGetValue("text-qualifier", out var tq) ? tq : '\"'),
        '\\',
        (string)(attributes.TryGetValue("record-separator", out var rs) ? rs : "\r\n"),
        (bool)  (attributes.TryGetValue("first-row-header", out var frh) ? frh : false),
        (bool)  (attributes.TryGetValue("performance-optimized", out var po) ? po : true),
        (int)   (attributes.TryGetValue("buffer-size", out var bs) ? bs : 4096),
        (string)(attributes.TryGetValue("empty-cell", out var ec) ? ec : "(empty)"),
        (string)(attributes.TryGetValue("missing-cell", out var ms) ? ms : "(null)")
    ) {}

    public IDictionary<string, object> Attributes => new Dictionary<string, object>()
            {
                { "field-separator", Dialect.Delimiter },
                { "text-qualifier", Dialect.QuoteChar! },
                { "record-separator", Dialect.LineTerminator },
                { "first-row-header", Dialect.Header },
                { "performance-optimized", !ParserOptimizations.RowCountAtStart },
                { "missing-cell", Dialect.MissingCell ?? "(null)" },
                { "empty-cell", base.EmptyCell },
            };

    internal CsvProfile(char fieldSeparator)
        : this(fieldSeparator, '\"', "\r\n", false, true, "(empty)", "(null)")
    { }

    public CsvProfile(bool firstRowHeader)
        : this(';', '\"', "\r\n", firstRowHeader, true, "(empty)", "(null)")
    { }

    public CsvProfile(char fieldSeparator, char textQualifier, string recordSeparator, bool firstRowHeader, bool performanceOptimized, string emptyCell, string missingCell)
          : base(fieldSeparator, textQualifier, '\\', recordSeparator, firstRowHeader, !performanceOptimized, 4096, emptyCell, missingCell)
    { }
}

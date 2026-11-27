namespace NBi.Model.Settings;

public sealed class CsvProfile
{
    public CsvProfile() : this(';', '"', "\r\n")
    {
    }

    public CsvProfile(char fieldSeparator, char textQualifier, string recordSeparator)
        => (FieldSeparator, TextQualifier, RecordSeparator) = (fieldSeparator, textQualifier, recordSeparator);

    public char FieldSeparator { get; set; }
    public char TextQualifier { get; set; }
    public string RecordSeparator { get; set; }
    public bool FirstRowHeader { get; set; }
    public string EmptyCell { get; set; } = "(empty)";
    public string MissingCell { get; set; } = "(null)";
}

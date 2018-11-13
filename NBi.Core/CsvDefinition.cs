namespace NBi.Core
{
    public class CsvProfile
    {
        public virtual char FieldSeparator { get; set; }
        public char TextQualifier { get; set; }
        public virtual string RecordSeparator { get; set; }
        public virtual bool FirstRowHeader { get; set; }
        public virtual string MissingCell { get; set; }
        public virtual string EmptyCell { get; set; }

        protected CsvProfile()
        {
        }

        public CsvProfile(char fieldSeparator, char textQualifier)
            : this (fieldSeparator, textQualifier, "\r\n")
        {
        }

        public CsvProfile(char fieldSeparator, string recordSeparator)
            : this(fieldSeparator, '\"', recordSeparator)
        {
        }

        public CsvProfile(char fieldSeparator, char textQualifier, string recordSeparator)
            : this(fieldSeparator, textQualifier, recordSeparator, false)
        {
        }

        public CsvProfile(char fieldSeparator, char textQualifier, string recordSeparator, bool firstRowHeader)
            : this(fieldSeparator, textQualifier, recordSeparator, firstRowHeader, "(empty)", "(null)")
        {

        }

        public CsvProfile(char fieldSeparator, char textQualifier, string recordSeparator, bool firstRowHeader, string emptyCell, string missingCell)
            : this()
        {
            FieldSeparator = fieldSeparator;
            TextQualifier = textQualifier;
            RecordSeparator = recordSeparator;
            FirstRowHeader = firstRowHeader;
            EmptyCell = emptyCell;
            MissingCell = missingCell;
        }

        public static CsvProfile CommaDoubleQuote
        {
            get
            {
                return new CsvProfile(',', '\"');
            }
        }

        public static CsvProfile SemiColumnDoubleQuote
        {
            get
            {
                return new CsvProfile(';', '\"');
            }
        }

        public bool ShouldSerializeTextQualifier() => false;

        public bool ShouldSerializeFieldSeparator() => false;

        public bool ShouldSerializeRecordSeparator() => false;

        public bool ShouldSerializeFirstRowHeader() => false;
        public bool ShouldSerializeEmptyCell() => false;
        public bool ShouldSerializeMissingCell() => false;


    }
}

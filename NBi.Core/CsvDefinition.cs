namespace NBi.Core
{
    public class CsvProfile
    {
        public virtual char FieldSeparator { get; set; }
        public char TextQualifier { get; set; }
        public virtual string RecordSeparator { get; set; }

        protected CsvProfile()
        {
        }

        public CsvProfile(char fieldSeparator, char textQualifier)
            : this (fieldSeparator, textQualifier, "\r\n")
        {
        }

        public CsvProfile(char fieldSeparator, char textQualifier, string recordSeparator)
            : this()
        {
            FieldSeparator = fieldSeparator;
            TextQualifier = textQualifier;
            RecordSeparator = recordSeparator;
        }

        public CsvProfile(char fieldSeparator, string recordSeparator)
            : this(fieldSeparator, '\"', recordSeparator)
        {
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

        public bool ShouldSerializeTextQualifier()
        {
            return false;
        }

        public bool ShouldSerializeFieldSeparator()
        {
            return false;
        }

        public bool ShouldSerializeRecordSeparator()
        {
            return false;
        }

    }
}

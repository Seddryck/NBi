namespace NBi.Core
{
    public class CsvProfile
    {
        public char FieldSeparator { get; private set; }
        public char TextQualifier { get; private set; }
        public string RecordSeparator { get; private set; }


        public CsvProfile(char fieldSeparator, char textQualifier)
            : this (fieldSeparator, textQualifier, "\r\n")
        {
        }

        public CsvProfile(char fieldSeparator, char textQualifier, string recordSeparator)
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

    }
}

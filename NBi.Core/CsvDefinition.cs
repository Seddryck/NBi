namespace NBi.Core
{
    public class CsvDefinition
    {
        public char FieldSeparator { get; private set; }
        public char TextQualifier { get; private set; }
        public string RecordSeparator { get; private set; }


        public CsvDefinition(char fieldSeparator, char textQualifier)
            : this (fieldSeparator, textQualifier, "\r\n")
        {
        }

        public CsvDefinition(char fieldSeparator, char textQualifier, string recordSeparator)
        {
            FieldSeparator = fieldSeparator;
            TextQualifier = textQualifier;
            RecordSeparator = recordSeparator;
        }

        public CsvDefinition(char fieldSeparator, string recordSeparator)
            : this(fieldSeparator, '\"', recordSeparator)
        {
        }

        public static CsvDefinition CommaDoubleQuote
        {
            get
            {
                return new CsvDefinition(',', '\"');
            }
        }

        public static CsvDefinition SemiColumnDoubleQuote
        {
            get
            {
                return new CsvDefinition(';', '\"');
            }
        }

    }
}

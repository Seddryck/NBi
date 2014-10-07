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

        public static CsvDefinition CommaDoubleQuote()
        {
            return new CsvDefinition(',', '\"');
        }

        public static CsvDefinition SemiColumnDoubleQuote()
        {
            return new CsvDefinition(';', '\"');
        }

    }
}

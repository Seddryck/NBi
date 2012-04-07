namespace NBi.Core.Analysis.Query
{
    public class CsvDefinition
    {
        public char FieldSeparator { get; private set; }
        public char TextQualifier { get; private set; }

        public CsvDefinition(char fieldSeparator, char textQualifier)
        {
            FieldSeparator = fieldSeparator;
            TextQualifier = textQualifier;
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

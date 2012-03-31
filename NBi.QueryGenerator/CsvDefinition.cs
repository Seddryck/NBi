using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.QueryGenerator
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

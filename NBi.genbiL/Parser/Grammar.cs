using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;
using NBi.GenbiL.Parser.Valuable;

namespace NBi.GenbiL.Parser
{
    class Grammar
    {
        public static readonly Parser<string> Textual = Parse.Letter.AtLeastOnce().Text().Token();
        public static readonly Parser<string> BracketTextual = Parse.CharExcept("[]").AtLeastOnce().Text().Contained(Parse.Char('['), Parse.Char(']')).Token();
        public static readonly Parser<string> CurlyBraceTextual = Parse.CharExcept("{}").AtLeastOnce().Text().Contained(Parse.Char('{'), Parse.Char('}')).Token();
        public static readonly Parser<string> QuotedTextual = Parse.CharExcept("'").AtLeastOnce().Text().Contained(Parse.Char('\''), Parse.Char('\'')).Token();
        public static readonly Parser<string> Empty = Parse.IgnoreCase("Empty").Return("").Token();
        public static readonly Parser<string> None = Parse.IgnoreCase("None").Return("(none)").Token();
        public static readonly Parser<string> ExtendedQuotedTextual = Empty.Or(None).Or(QuotedTextual).Token();
        public static readonly Parser<string> Record = Textual.Or(BracketTextual);
        public static readonly Parser<IEnumerable<string>> RecordSequence = Record.DelimitedBy(Parse.Char(','));
        public static readonly Parser<IEnumerable<string>> QuotedRecordSequence = QuotedTextual.DelimitedBy(Parse.Char(','));
        public static readonly Parser<IEnumerable<string>> ExtendedQuotedRecordSequence = ExtendedQuotedTextual.DelimitedBy(Parse.Char(','));
        public static readonly Parser<ValuableType> ValuableColumn = Parse.IgnoreCase("Columns").Or(Parse.IgnoreCase("Column")).Return(ValuableType.Column).Token();
        public static readonly Parser<ValuableType> ValuableValue = Parse.IgnoreCase("Values").Or(Parse.IgnoreCase("Value")).Return(ValuableType.Value).Token();
        public static readonly Parser<ValuableType> ValuableClass = ValuableColumn.Or(ValuableValue);
        public static readonly Parser<IEnumerable<IValuable>> Valuables = 
        (
            from valuableClass in Grammar.ValuableClass
            from items in Grammar.ExtendedQuotedRecordSequence
            select new ValuableBuilder().Build(valuableClass, items)
        );

        public static readonly Parser<char> Terminator = Parse.Char(';').Token();
        public static readonly Parser<bool> Boolean = Parse.IgnoreCase("on").Return(true)
                                                        .Or(Parse.IgnoreCase("yes").Return(true))
                                                        .Or(Parse.IgnoreCase("true").Return(true))
                                                        .Or(Parse.IgnoreCase("off").Return(false))
                                                        .Or(Parse.IgnoreCase("no").Return(false))
                                                        .Or(Parse.IgnoreCase("false").Return(false));
    }
}

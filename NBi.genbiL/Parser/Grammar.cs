using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;
using NBi.GenbiL.Parser.Valuable;

namespace NBi.GenbiL.Parser;

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

    public static readonly Parser<IEnumerable<IValuable>> ValuableColumns = 
    (
        from valuableClass in Parse.IgnoreCase("Columns").Or(Parse.IgnoreCase("Column"))
        from items in Grammar.QuotedRecordSequence
        select new ValuableBuilder().Build(ValuableType.Column, items)
    );
    public static readonly Parser<IEnumerable<IValuable>> ValuableValues = 
    (
        from valuableClass in Parse.IgnoreCase("Values").Or(Parse.IgnoreCase("Value"))
        from items in Grammar.ExtendedQuotedRecordSequence
        select new ValuableBuilder().Build(ValuableType.Value, items)
    );
    public static readonly Parser<IValuable> ValuableColumn =
    (
        from valuableClass in Parse.IgnoreCase("Columns").Or(Parse.IgnoreCase("Column"))
        from item in Grammar.QuotedTextual
        select new ValuableBuilder().Build(ValuableType.Column, item)
    );
    public static readonly Parser<IValuable> ValuableValue =
    (
        from valuableClass in Parse.IgnoreCase("Values").Or(Parse.IgnoreCase("Value"))
        from item in Grammar.QuotedTextual
        select new ValuableBuilder().Build(ValuableType.Value, item)
    );

    public static readonly Parser<IValuable> Valuable = ValuableColumn.Or(ValuableValue);
    public static readonly Parser<IEnumerable<IValuable>> Valuables = ValuableColumns.Or(ValuableValues);

    public static readonly Parser<char> Terminator = Parse.Char(';').Token();
    public static readonly Parser<bool> Boolean = Parse.IgnoreCase("on").Return(true)
                                                    .Or(Parse.IgnoreCase("yes").Return(true))
                                                    .Or(Parse.IgnoreCase("true").Return(true))
                                                    .Or(Parse.IgnoreCase("off").Return(false))
                                                    .Or(Parse.IgnoreCase("no").Return(false))
                                                    .Or(Parse.IgnoreCase("false").Return(false));
}

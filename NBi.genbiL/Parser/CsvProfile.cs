using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Setting;
using NBi.GenbiL.Action.Setting.CsvProfile;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Parser;

class CsvProfile
{
    readonly static Parser<ICsvProfileAction> FieldSeparatorParser =
    (
            from tag in Keyword.FieldSeparator
            from to in Keyword.To
            from value in Grammar.QuotedTextual
            select new FieldSeparatorAction(value[0])
    );

    readonly static Parser<ICsvProfileAction> RecordSeparatorParser =
    (
            from tag in Keyword.RecordSeparator
            from to in Keyword.To
            from value in Grammar.QuotedTextual
            select new RecordSeparatorAction(value)
    );

    readonly static Parser<ICsvProfileAction> TextQualifierParser =
    (
            from tag in Keyword.TextQualifier
            from to in Keyword.To
            from value in Grammar.QuotedTextual
            select new TextQualifierAction(value[0])
    );

    readonly static Parser<ICsvProfileAction> FirstRowHeaderParser =
    (
            from tag in Keyword.FirstRowHeader
            from to in Keyword.To
            from value in Grammar.Boolean
            select new FirstRowHeaderAction(value)
    );

    readonly static Parser<ICsvProfileAction> EmptyCellParser =
    (
            from tag in Keyword.EmptyCell
            from to in Keyword.To
            from value in Grammar.QuotedTextual
            select new EmptyCellAction(value)
    );

    readonly static Parser<ICsvProfileAction> MissingCellParser =
    (
            from tag in Keyword.MissingCell
            from to in Keyword.To
            from value in Grammar.QuotedTextual
            select new MissingCellAction(value)
    );

    public readonly static Parser<IAction> Parser =
    (
            from csvProfile in Keyword.CsvProfile
            from set in Keyword.Set
            from action in FieldSeparatorParser.Or(RecordSeparatorParser)
                            .Or(TextQualifierParser).Or(FirstRowHeaderParser)
                            .Or(EmptyCellParser).Or(MissingCellParser)
            select action
    );
}

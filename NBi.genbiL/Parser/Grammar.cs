using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace NBi.GenbiL.Parser
{
    class Grammar
    {
        public static readonly Parser<string> Textual = Parse.Letter.AtLeastOnce().Text().Token();
        public static readonly Parser<string> BracketTextual = Parse.CharExcept("[]").AtLeastOnce().Text().Contained(Parse.Char('['), Parse.Char(']')).Token();
        public static readonly Parser<string> CurlyBraceTextual = Parse.CharExcept("{}").AtLeastOnce().Text().Contained(Parse.Char('{'), Parse.Char('}')).Token();
        public static readonly Parser<string> QuotedTextual = Parse.CharExcept("'").AtLeastOnce().Text().Contained(Parse.Char('\''), Parse.Char('\'')).Token();
        public static readonly Parser<string> Record = Textual.Or(BracketTextual);
        public static readonly Parser<IEnumerable<string>> RecordSequence = Record.DelimitedBy(Parse.Char(','));
        public static readonly Parser<char> Terminator = Parse.Char(';').Token();
        public static readonly Parser<bool> Boolean = Parse.IgnoreCase("on").Return(true)
                                                        .Or(Parse.IgnoreCase("yes").Return(true))
                                                        .Or(Parse.IgnoreCase("true").Return(true))
                                                        .Or(Parse.IgnoreCase("off").Return(false))
                                                        .Or(Parse.IgnoreCase("no").Return(false))
                                                        .Or(Parse.IgnoreCase("false").Return(false));
    }
}

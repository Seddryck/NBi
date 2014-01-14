using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace NBi.GenbiL.Parser
{
    class Grammar
    {
        public static readonly Parser<string> Textual = Parse.Letter.AtLeastOnce().Text().Token();
        public static readonly Parser<string> BracketTextual = Parse.CharExcept("[]").AtLeastOnce().Text().Contained(Parse.Char('['), Parse.Char(']'));
        public static readonly Parser<string> QuotedTextual = Parse.CharExcept("'").AtLeastOnce().Text().Contained(Parse.Char('\''), Parse.Char('\''));
        public static readonly Parser<string> Record = Textual.Or(BracketTextual);
        public static readonly Parser<IEnumerable<string>> RecordSequence = Record.DelimitedBy(Parse.Char(','));
        public static readonly Parser<char> Terminator = Parse.Char(';');
    }
}

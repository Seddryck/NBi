﻿using NBi.GenbiL.Action;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Parser
{
    class Comment
    {
        static Parser<string> SingleLineComment(string open)
        {
            return from first in Parse.String(open)
                   from rest in Parse.AnyChar.Except(Parse.Char((char)13)).Many().Text()
                   select rest;
        }

        static Parser<string> MultiLineComment(string open, string close)
        {
            return from first in Parse.String(open)
                   from rest in Parse.AnyChar.Except(Parse.String(close))
                                .Or(Parse.Char((char)13))
                                .Many().Text()
                   from last in Parse.String(close)
                   select rest;
        }

        static Parser<string> Commentar(string single, string openMulti, string closeMulti)
        {
            return SingleLineComment(single).Or(MultiLineComment(openMulti, closeMulti));
        }

        public readonly static Parser<IAction> Parser = 
        (
            from comment in Commentar("//", "/*", "*/")
            select new EmptyAction()
        );

    }
}

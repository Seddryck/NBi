using NBi.GenbiL.Action;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Parser;

class Comment
{
    static Parser<string> SingleLineComment(string open)
    {
        return from first in Parse.String(open)
               from rest in Parse.AnyChar.Until(Parse.Return("").End().Or(Parse.String(Environment.NewLine))).Text()
               select rest;
    }

    static Parser<string> MultiLineComment(string open, string close)
    {
        return from first in Parse.String(open)
               from rest in Parse.AnyChar.Until(Parse.String(close)).Text()
               from lineTerminator in Parse.LineTerminator.XMany()
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

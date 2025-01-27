using System;
using System.Collections.Generic;
using System.Linq;
using NBi.GenbiL.Action;
using Sprache;

namespace NBi.GenbiL.Parser;

public class Recipe
{
    

    readonly static Parser<IAction> LineParser =
    (
            from sentence in Comment.Parser.Or(Action.Parser)
            select sentence
    );

    public readonly static Parser<IEnumerable<IAction>> Parser = LineParser.XMany().End();
}

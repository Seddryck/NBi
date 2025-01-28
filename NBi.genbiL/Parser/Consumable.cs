using System;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Template;
using Sprache;
using NBi.GenbiL.Action.Consumable;

namespace NBi.GenbiL.Parser;

class Consumable
{
    readonly static Parser<IConsumableAction> consumableSetParser =
        (
            from set in Keyword.Set
            from name in Grammar.QuotedTextual
            from to in Keyword.To
            from value in Grammar.QuotedTextual
            select new SetConsumableAction(name, value)
        );
    
    public readonly static Parser<IAction> Parser =
    (
            from @case in Keyword.Consumable
            from action in consumableSetParser
            select action
    );
}

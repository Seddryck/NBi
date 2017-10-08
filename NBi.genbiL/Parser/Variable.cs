using System;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Template;
using Sprache;
using NBi.GenbiL.Action.Variable;

namespace NBi.GenbiL.Parser
{
    class Variable
    {
        readonly static Parser<IVariableAction> variableSetParser =
            (
                from set in Keyword.Set
                from name in Grammar.QuotedTextual
                from to in Keyword.To
                from value in Grammar.QuotedTextual
                select new SetVariableAction(name, value)
            );
        
        public readonly static Parser<IAction> Parser =
        (
                from @case in Keyword.Variable
                from action in variableSetParser
                select action
        );
    }
}

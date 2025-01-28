using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Variable;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Parser;

class Variable
{
    readonly static Parser<IVariableAction> includeParser =
    (
            from include in (Keyword.Add).Or(Keyword.Include)
            from file in Keyword.File
            from filename in Grammar.QuotedTextual.Token()
            select new IncludeVariableAction(filename)
    );

    public readonly static Parser<IAction> Parser =
    (
            from @case in Keyword.Variable
            from action in includeParser
            select action
    );
}

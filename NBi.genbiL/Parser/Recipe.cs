using System;
using System.Collections.Generic;
using System.Linq;
using NBi.GenbiL.Action;
using Sprache;

namespace NBi.GenbiL.Parser
{
    public class Recipe
    {
        readonly static Parser<IAction> ActionParser =
        (
                from sentence in Case.Parser.Or(Setting.Parser.Or(Suite.Parser.Or(Template.Parser)))
                from termintaor in Grammar.Terminator.AtLeastOnce()
                select sentence
        );

        public readonly static Parser<IEnumerable<IAction>> Parser = ActionParser.Many();
    }
}

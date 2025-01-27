using NBi.GenbiL.Action;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Parser;

class Action
{
    public readonly static Parser<IAction> Parser =
    (
            from sentence in Case.Parser
                                .Or(Setting.Parser)
                                .Or(Suite.Parser)
                                .Or(Template.Parser)
                                .Or(Variable.Parser)
                                .Or(Consumable.Parser)
                                .Or(CsvProfile.Parser)
            from terminator in Grammar.Terminator.AtLeastOnce()
            select sentence
    );
}

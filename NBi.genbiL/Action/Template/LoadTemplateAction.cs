using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using System.Collections.Generic;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Template;

public abstract class LoadTemplateAction : ITemplateAction
{
    protected List<ITemplateAction> actions;
    public LoadTemplateAction(ITemplateAction action)
    {
        actions = new List<ITemplateAction>()
        {
            new ClearTemplateAction(),
            action
        };
    }

    public void Execute(GenerationState state)
    {
        foreach (var action in actions)
            action.Execute(state);
    }

    public abstract string Display { get; }
}

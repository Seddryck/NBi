using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using System.Collections.Generic;

namespace NBi.GenbiL.Action.Template;

public class LoadStringTemplateAction : LoadTemplateAction
{
    public LoadStringTemplateAction(string templateString)
        : base(new AddStringTemplateAction(templateString))
    { }

    public override string Display
    {
        get
        {
            return string.Format("Loading Template from string");
        }
    }
}
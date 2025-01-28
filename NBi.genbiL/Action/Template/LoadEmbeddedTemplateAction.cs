using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using System.Collections.Generic;

namespace NBi.GenbiL.Action.Template;

public class LoadEmbeddedTemplateAction : LoadTemplateAction
{
    public LoadEmbeddedTemplateAction(string filename)
        : base(new AddEmbeddedTemplateAction(filename))
    { }

    public override string Display
    {
        get
        {
            return string.Format($"Loading Template from embedded resource");
        }
    }
}

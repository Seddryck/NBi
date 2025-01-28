using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using System.Collections.Generic;

namespace NBi.GenbiL.Action.Template;

public class LoadFileTemplateAction : LoadTemplateAction
{
    public LoadFileTemplateAction(string filename)
        : base(new AddFileTemplateAction(filename))
    { }

    public override string Display
    {
        get
        {
            return string.Format("Loading Template from external file");
        }
    }
}

using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using System.IO;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Template;

public class AddStringTemplateAction : ITemplateAction
{
    public string TemplateString { get; set; }
    public AddStringTemplateAction(string templateString)
        : base()
    {
        TemplateString = templateString;
    }

    public void Execute(GenerationState state)
    {
        state.Templates.Add(TemplateString);
    }

    public string Display
    {
        get
        {
            return string.Format($"Adding new Template from string '{TemplateString}'");
        }
    }
}
using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using System.IO;
using System.Reflection;
using NBi.GenbiL.Stateful;
using NBi.GenbiL.Templating.Resources;

namespace NBi.GenbiL.Action.Template;

public class AddEmbeddedTemplateAction : ITemplateAction
{
    private string GetTemplatesPath() => $"{typeof(ResourcesFolder).Namespace}";

    public string Filename { get; set; }
    public AddEmbeddedTemplateAction(string filename)
        : base()
    {
        Filename = filename;
    }

    public void Execute(GenerationState state)
    {
        var assembly = Assembly.GetAssembly(typeof(ResourcesFolder));

        if (assembly == null)
            throw new InvalidOperationException("Can't find the dll containing the embedded templates.");

        if (Filename.StartsWith("SubsetOf"))
            Filename = Filename.Replace("SubsetOf", "ContainedIn");

        using var stream = assembly.GetManifestResourceStream($"{GetTemplatesPath()}.{Filename}.txt");
        if (stream == null)
            throw new ArgumentOutOfRangeException($"{GetTemplatesPath()}.{Filename}.txt");
        using var reader = new StreamReader(stream);
        state.Templates.Add(reader.ReadToEnd());
    }
    
    public string Display
    {
        get
        {
            return string.Format($"Adding new template from embedded resource '{Filename}'");
        }
    }
}

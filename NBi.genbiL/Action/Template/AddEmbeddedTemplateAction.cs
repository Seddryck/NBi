using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using System.IO;
using System.Reflection;

namespace NBi.GenbiL.Action.Template
{
    public class AddEmbeddedTemplateAction : ITemplateAction
    {
        private const string TEMPLATE_DIRECTORY = "NBi.Service.Templates.";

        public string Filename { get; set; }
        public AddEmbeddedTemplateAction(string filename)
            : base()
        {
            Filename = filename;
        }

        public void Execute(GenerationState state)
        {
            var assembly = Assembly.LoadFrom("NBi.Service.dll");

            using (var stream = assembly.GetManifestResourceStream($"{TEMPLATE_DIRECTORY}{Filename}.txt"))
            {
                if (stream == null)
                    throw new ArgumentOutOfRangeException($"{Filename}");
                using (var reader = new StreamReader(stream))
                    state.Templates.Add(reader.ReadToEnd());
            }
        }
        
        public string Display
        {
            get
            {
                return string.Format($"Adding new template from embedded resource '{Filename}'");
            }
        }
    }
}

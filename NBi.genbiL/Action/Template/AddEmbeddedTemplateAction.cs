using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using System.IO;
using System.Reflection;
using NBi.Service;

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
            var assembly = Assembly.GetAssembly(typeof(TestSuiteManager));

            if (assembly == null)
                throw new InvalidOperationException("Can't find the dll containing the embedded templates.");

            if (Filename.StartsWith("SubsetOf"))
                Filename = Filename.Replace("SubsetOf", "ContainedIn");

            using (var stream = assembly.GetManifestResourceStream($"{TEMPLATE_DIRECTORY}{Filename}.txt"))
            {
                if (stream == null)
                    throw new ArgumentOutOfRangeException($"{TEMPLATE_DIRECTORY}{Filename}.txt");
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

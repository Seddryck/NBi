using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NBi.Service;

namespace NBi.GenbiL.Action.Template
{
    public class LoadPredefinedTemplateAction : ITemplateAction
    {
        private const string TEMPLATE_DIRECTORY = "NBi.Service.Templates.";
        private const string TEMPLATE_DEFAULT = "ExistsDimension";

        public string ResourceName { get; set; }
        public LoadPredefinedTemplateAction(string resourceName)
            : base()
        {
            ResourceName = resourceName;
        }

        public void Execute(GenerationState state)
        {
            var value = string.Empty;           //Template
            using (var stream = Assembly.GetAssembly(typeof(TemplateManager)).GetManifestResourceStream(string.Format("{0}{1}.txt", TEMPLATE_DIRECTORY, ResourceName)))
            {
                using (var reader = new StreamReader(stream))
                {
                    value = reader.ReadToEnd();
                }
            }
            state.Template.Code = value;
        }

        public string[] GetEmbeddedLabels()
        {
            var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            IEnumerable<string> labels = resources.Where(t => t.StartsWith(TEMPLATE_DIRECTORY) && t.EndsWith(".txt")).ToList();
            labels = labels.Select(t => t.Replace(TEMPLATE_DIRECTORY, ""));
            labels = labels.Select(t => t.Substring(0, t.Length - 4));
            labels = labels.Select(t => SplitCamelCase(t));
            return labels.ToArray();
        }
        private static string SplitCamelCase(string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }

        public string Display
        {
            get
            {
                return string.Format("Loading predefined template from '{0}'"
                    , ResourceName
                    );
            }
        }
    }
}

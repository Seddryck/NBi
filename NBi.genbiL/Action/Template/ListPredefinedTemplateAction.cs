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
    public class ListPredefinedTemplateAction : ITemplateAction
    {
        private const string TEMPLATE_DIRECTORY = "NBi.Service.Templates.";
        private const string TEMPLATE_DEFAULT = "ExistsDimension";
        
        public ListPredefinedTemplateAction()
            : base()
        {
        }

        public void Execute(GenerationState state)
        {
            var resources = Assembly.GetAssembly(typeof(TemplateManager)).GetManifestResourceNames();
            IEnumerable<string> labels = resources.Where(t => t.StartsWith(TEMPLATE_DIRECTORY) && t.EndsWith(".txt")).ToList();
            labels = labels.Select(t => t.Replace(TEMPLATE_DIRECTORY, ""));
            labels = labels.Select(t => t.Substring(0, t.Length - 4));
            labels = labels.Select(t => SplitCamelCase(t));
            state.Template.PredefinedLabels = labels.ToList();
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
                return string.Format("Listing predefined templates from '{0}'"
                    , TEMPLATE_DIRECTORY.Remove(TEMPLATE_DIRECTORY.Length-2)
                    );
            }
        }
    }
}

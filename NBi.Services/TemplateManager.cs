using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NBi.Service
{
    public class TemplateManager
    {
        private const string TEMPLATE_DIRECTORY = "NBi.Service.Templates.";
        private const string TEMPLATE_DEFAULT = "ExistsDimension";
        public string Code { get; private set; }

        public TemplateManager()
        {

        }

        public void Persist(string filename, string content)
        {
            using (TextWriter tw = new StreamWriter(filename))
            {
                tw.Write(content);
            }
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

        public string GetDefaultContent()
        {
            return GetEmbeddedTemplate(TEMPLATE_DEFAULT);
        }

        public string GetEmbeddedTemplate(string resourceName)
        {
            var value = string.Empty;           //Template
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("{0}{1}.txt", TEMPLATE_DIRECTORY, resourceName)))
            {
                using (var reader = new StreamReader(stream))
                {
                    value = reader.ReadToEnd();
                }
            }
            Code = value;
            return value;
        }

        public string GetExternalTemplate(string resourceName)
        {
            var tpl = string.Empty;           //Template
            using (var stream = new StreamReader(resourceName))
            {
                tpl = stream.ReadToEnd();
            }
            Code = tpl;
            return tpl;
        }
    }
}

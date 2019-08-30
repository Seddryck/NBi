using NBi.Core;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NBi.NUnit.Builder.Helper
{
    class ConnectionStringHelper
    {
        public string Execute(BaseItem xml, SettingsXml.DefaultScope scope)
            => Execute(xml.ConnectionString, xml.Settings.References, xml.Settings.GetDefault(scope), xml.Roles, xml.Settings.BasePath);

        protected string Execute(string rawConnectionString, IEnumerable<ReferenceXml> references, DefaultXml @default, string roles, string basePath)
        {
            var connectionString = GetBaseConnectionString(rawConnectionString, references, @default);

            //If it's an ODC file
            if (!string.IsNullOrEmpty(connectionString) && connectionString.TrimEnd().EndsWith(".odc"))
                return new OfficeDataConnectionFileParser(basePath).GetConnectionString(connectionString);

            //We must remove all the characters such as \r \n or \t
            if (!string.IsNullOrEmpty(connectionString))
                connectionString = connectionString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

            if (!string.IsNullOrEmpty(roles))
                connectionString = ReplaceRoles(connectionString, roles);

            return connectionString;
        }

        private string ReplaceRoles(string connectionString, string newRoles)
        {
            var pattern = "Roles(\\s)*=(\\s)*(?<RolesValue>([^;]*))";
            var options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled;
            var reg = new Regex(pattern, options);
            var match = reg.Match(connectionString);
            if (match.Success)
                connectionString = reg.Replace(connectionString, string.Format("Roles=\"{0}\";", newRoles));
            else
                connectionString = string.Format("{0};Roles=\"{1}\";", connectionString, newRoles);

            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, string.Format("ConnectionString string used '{0}'", connectionString));

            return connectionString;
        }

        private string GetBaseConnectionString(string connectionString, IEnumerable<ReferenceXml> references, DefaultXml @default)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                if (!connectionString.StartsWith("@"))
                    return connectionString;
                else
                    return references.FirstOrDefault(x => x.Name == connectionString.Remove(0, 1))?.ConnectionString?.GetValue();
            }
            else if (@default != null && @default.ConnectionString != null)
                return @default.ConnectionString.GetValue();
            else
                return null;
        }
    }
}

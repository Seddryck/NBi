using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Xml.Settings;

namespace NBi.Xml.Items
{
    public abstract class BaseItem
    {
        [XmlIgnore()]
        public virtual DefaultXml Default { get; set; }
        [XmlIgnore()]
        public virtual SettingsXml Settings { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        [XmlAttribute("roles")]
        public string Roles { get; set; }

        public virtual string GetConnectionString()
        {
            var connectionString = GetBaseConnectionString();

            if (!string.IsNullOrEmpty(Roles))
                connectionString = ReplaceRoles(connectionString, Roles);

            return connectionString;
        }

        protected string ReplaceRoles(string connectionString, string newRoles)
        {
            string pattern = "Roles(\\s)*=(\\s)*(?<RolesValue>([^;]*))";
            RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled;
            Regex reg = new Regex(pattern, options);
            var match = reg.Match(connectionString);
            if (match.Success)
                connectionString = reg.Replace(connectionString, string.Format("Roles=\"{0}\";", newRoles));
            else
                connectionString = string.Format("{0};Roles=\"{1}\";", connectionString, newRoles);

            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("ConnectionString string used '{0}'", connectionString);

            return connectionString;
        }

        protected string GetBaseConnectionString()
        {
            if (!string.IsNullOrEmpty(ConnectionString) && ConnectionString.StartsWith("@"))
                return Settings.GetReference(ConnectionString.Remove(0, 1)).ConnectionString;

            //Else get the ConnectionString as-is
            //if ConnectionString is specified then return it
            if (!string.IsNullOrEmpty(ConnectionString))
                return ConnectionString;

            //Else get the default ConnectionString 
            if (Default != null && !string.IsNullOrEmpty(Default.ConnectionString))
                return Default.ConnectionString;
            return null;
        }
    }
}

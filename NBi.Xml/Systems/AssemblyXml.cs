using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.Assemblies;

namespace NBi.Xml.Systems
{
    public class AssemblyXml : AbstractSystemUnderTestXml
    {
        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("class")]
        public string Klass { get; set; }

        [XmlAttribute("method")]
        public string Method { get; set; }

        [XmlAttribute("static")]
        public bool Static { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        [XmlAttribute("connectionString-ref")]
        public string ConnectionStringReference { get; set; }

        [XmlElement("parameter")]
        public List<ParameterXml> Parameters { get; set; }

        public AssemblyXml()
        {
            Parameters = new List<ParameterXml>();
        }

        public override object Instantiate()
        {
            var am = new AssemblyManager();
            object execRes = null;
            if (Static)
            {
                var type = am.GetStatic(Path, Klass);
                execRes = am.ExecuteStatic(type, Method, GetParameters());
            }
            else
            {
                var classInstance = am.GetInstance(Path, Klass, null);
                execRes = am.Execute(classInstance, Method, GetParameters());
            }
            
            //If we're trying to create a queryXml then we need to instiante it!
            if (GetConnectionString() != null && execRes is string) //It means that we've a query
            {
                var queryXml = new QueryXml()
                {
                    InlineQuery = (string)execRes,
                    ConnectionString = GetConnectionString()
                };
                return queryXml.Instantiate();
            }

            return execRes;
        }

        public string GetConnectionString()
        {
            //if Sql is specified then return it
            if (!string.IsNullOrEmpty(ConnectionString))
                return ConnectionString;

            //Else get the reference ConnectionString 
            if (!string.IsNullOrEmpty(ConnectionStringReference))
                return null;
                //TODO remove previous line and uncomment the next
                //return Settings.GetReference(ConnectionStringReference).ConnectionString;

            //Else get the default ConnectionString 
            if (Default != null && !string.IsNullOrEmpty(Default.ConnectionString))
                return Default.ConnectionString;
            return null;
        }


        protected Dictionary<string, object> GetParameters()
        {
            var dico = new Dictionary<string, object>();
            
            foreach (ParameterXml param in this.Parameters)
            {
                dico.Add(param.Name, param.Value);
            }
            return dico;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Assemblies;

namespace NBi.Xml.Items
{
    public class AssemblyXml : QueryableXml
    {
        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("class")]
        public string Klass { get; set; }

        [XmlAttribute("method")]
        public string Method { get; set; }

        [XmlAttribute("static")]
        public bool Static { get; set; }

        [XmlElement("method-parameter")]
        public List<AssemblyParameterXml> MethodParameters { get; set; }

        public AssemblyXml()
        {
            MethodParameters = new List<AssemblyParameterXml>();
        }


        protected Dictionary<string, object> GetMethodParameters()
        {
            var dico = new Dictionary<string, object>();

            foreach (AssemblyParameterXml param in this.MethodParameters)
            {
                dico.Add(param.Name, param.Value);
            }
            return dico;
        }

        public override string GetQuery()
        {
            var assemblyManager = new AssemblyManager();
            object methodExecution = null;
            if (Static)
            {
                var type = assemblyManager.GetStatic(Path, Klass);
                methodExecution = assemblyManager.ExecuteStatic(type, Method, GetMethodParameters());
            }
            else
            {
                var classInstance = assemblyManager.GetInstance(Path, Klass, null);
                methodExecution = assemblyManager.Execute(classInstance, Method, GetMethodParameters());
            }

            if (methodExecution is string) //It means that we've a query
                return (string)methodExecution;

            throw new InvalidOperationException("The method should return a string (query)");
        }

    }
}

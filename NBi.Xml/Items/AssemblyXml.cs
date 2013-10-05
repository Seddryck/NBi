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

        [XmlElement("parameter")]
        public List<ParameterXml> Parameters { get; set; }

        public AssemblyXml()
        {
            Parameters = new List<ParameterXml>();
        }

        //public override object Instantiate()
        //{
        //    var am = new AssemblyManager();
        //    object execRes = null;
        //    if (Static)
        //    {
        //        var type = am.GetStatic(Path, Klass);
        //        execRes = am.ExecuteStatic(type, Method, GetParameters());
        //    }
        //    else
        //    {
        //        var classInstance = am.GetInstance(Path, Klass, null);
        //        execRes = am.Execute(classInstance, Method, GetParameters());
        //    }
            
        //    //If we're trying to create a queryXml then we need to instiante it!
        //    if (GetConnectionString() != null && execRes is string) //It means that we've a query
        //    {
        //        var queryXml = new QueryXml()
        //        {
        //            InlineQuery = (string)execRes,
        //            ConnectionString = GetConnectionString()
        //        };
        //        return queryXml.Instantiate();
        //    }

        //    return execRes;
        //}


        protected Dictionary<string, object> GetParameters()
        {
            var dico = new Dictionary<string, object>();
            
            foreach (ParameterXml param in this.Parameters)
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
                methodExecution = assemblyManager.ExecuteStatic(type, Method, GetParameters());
            }
            else
            {
                var classInstance = assemblyManager.GetInstance(Path, Klass, null);
                methodExecution = assemblyManager.Execute(classInstance, Method, GetParameters());
            }

            if (methodExecution is string) //It means that we've a query
                return (string)methodExecution;

            throw new InvalidOperationException("The method should return a string (query)");
        }

    }
}
